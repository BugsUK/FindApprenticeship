namespace SFA.Apprenticeships.Infrastructure.Processes.Communications
{
    using System;
    using Application.Interfaces.Communications;
    using SFA.Infrastructure.Interfaces;
    using Domain.Entities.Exceptions;
    using Domain.Interfaces.Messaging;

    using SFA.Apprenticeships.Application.Interfaces;

    public class EmailRequestSubscriber : IServiceBusSubscriber<EmailRequest>
    {
        private readonly IEmailDispatcher _dispatcher;
        private readonly ILogService _logService;

        public EmailRequestSubscriber(IEmailDispatcher dispatcher, ILogService logService)
        {
            _dispatcher = dispatcher;
            _logService = logService;
        }

        [ServiceBusTopicSubscription(TopicName = "SendEmail")]
        public ServiceBusMessageStates Consume(EmailRequest request)
        {
            try
            {
                _dispatcher.SendEmail(request);

                return ServiceBusMessageStates.Complete;
            }
            catch (CustomException ex)
            {
                switch (ex.Code)
                {
                    case ErrorCodes.EmailError:
                        _logService.Warn(
                            string.Format(
                                "Error sending email to '{0}', message type '{1}'. Failed with code '{2}' and will be retried",
                                request.ToEmail, request.MessageType, ex.Code), ex);
                        return ServiceBusMessageStates.Requeue;
                    default:
                        _logService.Warn(
                            string.Format(
                                "Error sending email to '{0}', message type '{1}'. Failed with code '{2}' and will not be retried",
                                request.ToEmail, request.MessageType, ex.Code), ex);
                        return ServiceBusMessageStates.Complete;
                }
            }
            catch (Exception ex)
            {
                _logService.Warn(
                    string.Format("Error sending email to '{0}', message type '{1}'. This will not be retried", request.ToEmail,
                        request.MessageType), ex);
                return ServiceBusMessageStates.Complete;
            }
        }
    }
}
