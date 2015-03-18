namespace SFA.Apprenticeships.Application.Services.Communication.Strategies
{
    using System.Collections.Generic;
    using Application.Interfaces;
    using Application.Interfaces.Communications;
    using Interfaces;
    using Common.AppSettings;

    public class SendAccessRequestStrategy : ISendAccessRequestStrategy
    {
        private readonly IEmailDispatcher _emailDispatcher;
        private readonly ILogService _logger;

        public SendAccessRequestStrategy(IEmailDispatcher emailDispatcher, ILogService logger)
        {
            _emailDispatcher = emailDispatcher;
            _logger = logger;
        }
        
        public void Send(MessageTypes messageType, IEnumerable<CommunicationToken> tokens)
        {
            var request = new EmailRequest
            {
                ToEmail = BaseAppSettingValues.ToEmailAddress,
                MessageType = messageType,
                Tokens = tokens
            };

            _emailDispatcher.SendEmail(request);
        }
    }
}