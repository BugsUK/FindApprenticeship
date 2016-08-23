namespace SFA.Apprenticeships.Infrastructure.Communication.Sms
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Linq;
    using System.Net;
    using System.Xml.Linq;
    using Application.Interfaces.Communications;
    using Configuration;
    using Domain.Entities.Exceptions;
    using RestSharp;

    using SFA.Apprenticeships.Application.Interfaces;
    using ErrorCodes = Application.Interfaces.Communications.ErrorCodes;

    public class ReachSmsDispatcher : ISmsDispatcher
    {
        private readonly ILogService _logger;

        private readonly IEnumerable<KeyValuePair<MessageTypes, SmsMessageFormatter>> _messageFormatters;
        private readonly SmsConfiguration _reachConfiguration;
        private readonly ISmsNumberFormatter _smsNumberFormatter;
        private readonly IRestClient _restClient;

        public ReachSmsDispatcher(
            ILogService logger,
            IRestClient restClient,
            IConfigurationService configurationService,
            ISmsNumberFormatter smsNumberFormatter,
            IEnumerable<KeyValuePair<MessageTypes, SmsMessageFormatter>> messageFormatters
            )
        {
            _logger = logger;
            _restClient = restClient;
            _reachConfiguration = configurationService.Get<SmsConfiguration>();
            _smsNumberFormatter = smsNumberFormatter;
            _messageFormatters = messageFormatters;
        }

        public void SendSms(SmsRequest smsRequest)
        {
            var context = new
            {
                toNumber = smsRequest.ToNumber,
                messageType = smsRequest.MessageType
            };

            var logMessage = FormatLogMessage(smsRequest);

            try
            {
                _logger.Debug("Sending SMS: {0}", logMessage);

                var smsMessage = FormatSmsMessage(smsRequest);
                var restRequest = CreateRestRequest(smsRequest, smsMessage);

                _restClient.BaseUrl = new Uri(_reachConfiguration.Url);

                var restResponse = _restClient.Execute(restRequest);
                var smsMessageId = ParseRestResponse(restResponse);

                _logger.Info("Sent SMS: id='{0}' message='{1}' {2}", smsMessageId, smsMessage, logMessage);
            }
            catch (BoundaryException e)
            {
                if (e.Code == ErrorCodes.SmsErrorInvalidMobileNumber)
                {
                    _logger.Info("Failed to send SMS. The number was invalid. This is an unrecoverable error and should not be re-queued: {0}", e, logMessage);
                }
                else
                {
                    _logger.Error("Failed to send SMS: {0}", e, logMessage);

                    throw new DomainException(ErrorCodes.SmsError, e, context);
                }
            }
            catch (Exception e)
            {
                _logger.Error("Failed to send SMS: {0}", e, logMessage);

                throw new DomainException(ErrorCodes.SmsError, e, context);
            }
        }

        #region Helpers

        private string FormatSmsMessage(SmsRequest request)
        {
            if (_messageFormatters.All(mf => mf.Key != request.MessageType))
            {
                var errorMessage = string.Format("FormatSmsMessage: No message formatter exists for MessageType: '{0}'", request.MessageType);

                _logger.Error(errorMessage);

                throw new ConfigurationErrorsException(errorMessage);
            }

            return _messageFormatters.First(m => m.Key == request.MessageType).Value.GetMessage(request.Tokens);
        }

        private static string FormatLogMessage(SmsRequest request)
        {
            return string.Format("type='{0}' to='{1}'", request.MessageType, request.ToNumber);
        }

        private RestRequest CreateRestRequest(SmsRequest smsRequest, string smsMessage)
        {
            var restRequest = new RestRequest(Method.POST);

            restRequest.AddParameter("username", _reachConfiguration.Username);
            restRequest.AddParameter("password", _reachConfiguration.Password);
            restRequest.AddParameter("msisdn", _smsNumberFormatter.Format(smsRequest.ToNumber));
            restRequest.AddParameter("message", smsMessage);
            restRequest.AddParameter("originator", _reachConfiguration.Originator);
            restRequest.AddParameter("callbackurl", _reachConfiguration.CallbackUrl);

            return restRequest;
        }

        private static Guid ParseRestResponse(IRestResponse restResponse)
        {
            if (restResponse.StatusCode != HttpStatusCode.OK)
            {
                throw new BoundaryException(ErrorCodes.SmsError, new { httpStatusCode = restResponse.StatusCode });
            }

            var content = restResponse.Content;

            if (string.IsNullOrWhiteSpace(content))
            {
                throw new BoundaryException(ErrorCodes.SmsError, new { content = "<null>" });
            }

            try
            {
                var xml = XElement.Parse(restResponse.Content);

                var messageIdNode = xml.Element("MessageId");
                var messageResultNode = xml.Element("MessageResult");

                if (messageIdNode != null && messageResultNode != null)
                {
                    var value = messageResultNode.Value;
                    if (value == "Success")
                    {
                        Guid messageId;

                        if (Guid.TryParse(messageIdNode.Value, out messageId))
                        {
                            return messageId;
                        }
                    }

                    if (value == "Failure : Bad To Number")
                    {
                        throw new BoundaryException(ErrorCodes.SmsErrorInvalidMobileNumber, new { content });
                    }
                }

                throw new BoundaryException(ErrorCodes.SmsError, new { content });
            }
            catch (BoundaryException)
            {
                throw;
            }
            catch (Exception e)
            {
                throw new BoundaryException(ErrorCodes.SmsError, e, new { content });
            }
        }

        #endregion
    }
}