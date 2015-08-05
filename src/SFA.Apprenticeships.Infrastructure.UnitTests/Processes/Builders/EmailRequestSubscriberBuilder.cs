﻿namespace SFA.Apprenticeships.Infrastructure.UnitTests.Processes.Builders
{
    using Application.Interfaces.Communications;
    using Application.Interfaces.Logging;
    using Infrastructure.Processes.Communications;
    using Moq;

    public class EmailRequestSubscriberBuilder
    {
        private Mock<IEmailDispatcher> _dispatcher = new Mock<IEmailDispatcher>();
        private readonly Mock<ILogService> _logService = new Mock<ILogService>(); 

        public EmailRequestSubscriber Build()
        {
            var subscriber = new EmailRequestSubscriber(_dispatcher.Object, _logService.Object);
            return subscriber;
        }

        public EmailRequestSubscriberBuilder With(Mock<IEmailDispatcher> dispatcher)
        {
            _dispatcher = dispatcher;
            return this;
        }
    }
}