namespace SFA.Apprenticeships.Infrastructure.Communication.Sms
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Linq;
    using System.Net;
    using System.Xml.Linq;
    using Application.Interfaces.Communications;
    using Application.Interfaces.Logging;
    using Domain.Entities.Exceptions;
    using RestSharp;

    public class ReachSmsDispatcher : ISmsDispatcher
    {
        private readonly ILogService _logger;

        private readonly IEnumerable<KeyValuePair<MessageTypes, SmsMessageFormatter>> _messageFormatters;
        private readonly IReachSmsConfiguration _reachConfiguration;
        private readonly IRestClient _restClient;

        public ReachSmsDispatcher(ILogService logger, IReachSmsConfiguration reachConfiguration, IRestClient restClient, IEnumerable<KeyValuePair<MessageTypes, SmsMessageFormatter>> messageFormatters)
        {
            _logger = logger;
            _reachConfiguration = reachConfiguration;
            _restClient = restClient;
            _messageFormatters = messageFormatters;
        }

        public void SendSms(SmsRequest smsRequest)
        {
            var context = new
            {
                toNumber = smsRequest.MessageType,
                messageType = smsRequest.MessageType
            };

            try
            {
                _logger.Debug("Dispatching SMS: {0}", FormatLogMessage(smsRequest));

                var smsMessage = FormatSmsMessage(smsRequest);
                var restRequest = CreateRestRequest(smsRequest, smsMessage);

                _restClient.BaseUrl = new Uri(_reachConfiguration.Url);

                var restResponse = _restClient.Execute(restRequest);
                var smsMessageId = ParseRestResponse(restResponse);

                _logger.Info("Dispatched SMS: id='{0}' message='{1}' to='{2}' {3}", smsMessageId, smsMessage, smsRequest.ToNumber, FormatLogMessage(smsRequest));
            }
            catch (Exception e)
            {
                _logger.Error("Failed to dispatch SMS: {0}", FormatLogMessage(smsRequest));

                throw new DomainException(ErrorCodes.SmsError, e, context);
            }
        }

        #region Helpers

        private string FormatSmsMessage(SmsRequest request)
        {
            if (_messageFormatters.All(mf => mf.Key != request.MessageType))
            {
                var errorMessage = string.Format("GetMessageFrom: No message formatter exists for MessageType: '{0}'", request.MessageType);

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
            var smsNumberFormatter = new SmsNumberFormatter();

            restRequest.AddParameter("username", _reachConfiguration.Username);
            restRequest.AddParameter("password", _reachConfiguration.Password);
            restRequest.AddParameter("msisdn", smsNumberFormatter.Format(smsRequest.ToNumber));
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

                if (messageIdNode != null && messageResultNode != null && messageResultNode.Value == "Success")
                {
                    Guid messageId;

                    if (Guid.TryParse(messageIdNode.Value, out messageId))
                    {
                        return messageId;
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
