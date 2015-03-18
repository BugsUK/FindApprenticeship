namespace SFA.Apprenticeships.Application.Services.Communication.Strategies
{
    using System.Collections.Generic;
    using Application.Interfaces;
    using Application.Interfaces.Communications;
    using Interfaces;

    public class SendEmployerEnquiryStrategy : ISendEmployerEnquiryStrategy
    {
        private readonly IEmailDispatcher _emailDispatcher;
        private readonly ILogService _logger;

        public SendEmployerEnquiryStrategy(IEmailDispatcher emailDispatcher, ILogService logger)
        {
            _emailDispatcher = emailDispatcher;
            _logger = logger;
        }

        public void Send(MessageTypes messageType, IEnumerable<CommunicationToken> tokens)
        {
            throw new System.NotImplementedException();
        }
    }
}