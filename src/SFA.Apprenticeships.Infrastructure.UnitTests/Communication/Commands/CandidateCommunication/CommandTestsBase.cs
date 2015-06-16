﻿namespace SFA.Apprenticeships.Infrastructure.UnitTests.Communication.Commands.CandidateCommunication
{
    using Application.Interfaces.Communications;
    using Application.Interfaces.Logging;
    using Domain.Entities.Candidates;
    using Domain.Entities.UnitTests.Builder;
    using Domain.Entities.Users;
    using Domain.Interfaces.Configuration;
    using Domain.Interfaces.Messaging;
    using Domain.Interfaces.Repositories;
    using Infrastructure.Communication.Configuration;
    using Infrastructure.Processes.Communications.Commands;
    using Builders;
    using Moq;

    public abstract class CommandTestsBase
    {
        protected Mock<ILogService> LogService;
        protected Mock<IConfigurationService> ConfigurationService;
        protected Mock<IMessageBus> MessageBus;
        protected Mock<ICandidateReadRepository> CandidateRepository;
        protected Mock<IUserReadRepository> UserRepository;

        protected CandidateCommunicationCommand Command;

        protected CommandTestsBase()
        {
            LogService = new Mock<ILogService>();
            ConfigurationService = new Mock<IConfigurationService>();
            MessageBus = new Mock<IMessageBus>();
            UserRepository = new Mock<IUserReadRepository>();
            CandidateRepository = new Mock<ICandidateReadRepository>();
        }

        public virtual void SetUp(CandidateCommunicationCommand command)
        {
            Command = command;

            LogService.ResetCalls();
            ConfigurationService.ResetCalls();
            MessageBus.ResetCalls();
            UserRepository.ResetCalls();
            CandidateRepository.ResetCalls();

            ConfigurationService.Setup(mock =>
                mock.Get<CommunicationConfiguration>())
                .Returns(new CommunicationConfiguration
                {
                    SiteDomainName = "www.example.com"
                });
        }

        protected void ShouldQueueEmail(MessageTypes messageType, int expectedCount, string emailAddress = CommunicationRequestBuilder.DefaultTestEmailAddress)
        {
            MessageBus.Verify(mock => mock.PublishMessage(
                It.Is<EmailRequest>(emailRequest =>
                    emailRequest.MessageType == messageType &&
                    emailRequest.ToEmail == emailAddress)),
                Times.Exactly(expectedCount));
        }

        protected void ShouldQueueSms(MessageTypes messageType, int expectedCount, string mobileNumber = CommunicationRequestBuilder.DefaultTestMobileNumber)
        {
            MessageBus.Verify(mock => mock.PublishMessage(
                It.Is<SmsRequest>(smsRequest =>
                    smsRequest.MessageType == messageType &&
                    smsRequest.ToNumber == mobileNumber)),
                Times.Exactly(expectedCount));
        }

        protected void AddCandidate(Candidate candidate, UserStatuses userStatus = UserStatuses.Active)
        {
            var user = new UserBuilder(candidate.EntityId)
                .WithStatus(userStatus)
                .Build();

            CandidateRepository
                .Setup(mock => mock.Get(candidate.EntityId))
                .Returns(candidate);

            UserRepository
                .Setup(mock => mock.Get(candidate.EntityId))
                .Returns(user);
        }
    }
}