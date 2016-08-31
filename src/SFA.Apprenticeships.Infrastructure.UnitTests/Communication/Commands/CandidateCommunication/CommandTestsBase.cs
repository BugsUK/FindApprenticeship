namespace SFA.Apprenticeships.Infrastructure.UnitTests.Communication.Commands.CandidateCommunication
{
    using Application.Interfaces.Communications;
    using SFA.Infrastructure.Interfaces;
    using Domain.Entities.Candidates;
    using Domain.Entities.UnitTests.Builder;
    using Domain.Entities.Users;
    using Domain.Interfaces.Messaging;
    using Domain.Interfaces.Repositories;
    using Infrastructure.Communication.Configuration;
    using Infrastructure.Processes.Communications.Commands;
    using Builders;
    using Moq;

    using SFA.Apprenticeships.Application.Interfaces;

    public abstract class CommandTestsBase
    {
        protected Mock<ILogService> LogService;
        protected Mock<IConfigurationService> ConfigurationService;
        protected Mock<IServiceBus> ServiceBus;
        protected Mock<ICandidateReadRepository> CandidateRepository;
        protected Mock<IUserReadRepository> UserRepository;

        protected CandidateCommunicationCommand Command;

        protected CommandTestsBase()
        {
            LogService = new Mock<ILogService>();
            ConfigurationService = new Mock<IConfigurationService>();
            ServiceBus = new Mock<IServiceBus>();
            UserRepository = new Mock<IUserReadRepository>();
            CandidateRepository = new Mock<ICandidateReadRepository>();
        }

        public virtual void SetUp(CandidateCommunicationCommand command)
        {
            Command = command;

            LogService.ResetCalls();
            ConfigurationService.ResetCalls();
            ServiceBus.ResetCalls();
            UserRepository.ResetCalls();
            CandidateRepository.ResetCalls();

            ConfigurationService.Setup(mock =>
                mock.Get<CommunicationConfiguration>())
                .Returns(new CommunicationConfiguration
                {
                    CandidateSiteDomainName = "www.example.com"
                });
        }

        protected void ShouldQueueEmail(MessageTypes messageType, int expectedCount, string emailAddress = CommunicationRequestBuilder.DefaultTestEmailAddress)
        {
            ServiceBus.Verify(mock => mock.PublishMessage(
                It.Is<EmailRequest>(emailRequest =>
                    emailRequest.MessageType == messageType &&
                    emailRequest.ToEmail == emailAddress)),
                Times.Exactly(expectedCount));
        }

        protected void ShouldQueueSms(MessageTypes messageType, int expectedCount, string mobileNumber = CommunicationRequestBuilder.DefaultTestMobileNumber)
        {
            ServiceBus.Verify(mock => mock.PublishMessage(
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