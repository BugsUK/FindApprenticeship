﻿namespace SFA.Apprenticeships.Infrastructure.Communication.UnitTests.Commands
{
    using Application.Interfaces.Communications;
    using Builders;
    using Domain.Entities.Candidates;
    using Domain.Entities.UnitTests.Builder;
    using Domain.Entities.Users;
    using Domain.Interfaces.Messaging;
    using Domain.Interfaces.Repositories;
    using Moq;
    using Processes.Communications.Commands;

    public abstract class CandidateCommunicationCommandTestsBase
    {
        protected Mock<IMessageBus> MessageBus;
        protected Mock<ICandidateReadRepository> CandidateRepository;
        protected Mock<IUserReadRepository> UserRepository;

        protected CandidateCommunicationCommand Command;

        protected CandidateCommunicationCommandTestsBase()
        {
            MessageBus = new Mock<IMessageBus>();
            UserRepository = new Mock<IUserReadRepository>();
            CandidateRepository = new Mock<ICandidateReadRepository>();
        }

        public virtual void SetUp(CandidateCommunicationCommand command)
        {
            Command = command;

            MessageBus.ResetCalls();
            UserRepository.ResetCalls();
            CandidateRepository.ResetCalls();
        }

        protected void ShouldQueueEmail(MessageTypes messageType, Times times, string emailAddress = CommunicationRequestBuilder.DefaultTestEmailAddress)
        {
            MessageBus.Verify(mock => mock.PublishMessage(
                It.Is<EmailRequest>(emailRequest =>
                    emailRequest.MessageType == messageType &&
                    emailRequest.ToEmail == emailAddress)),
                    // TODO: AG: email tokens?
                times);
        }

        protected void ShouldQueueSms(MessageTypes messageType, Times times, string mobileNumber = CommunicationRequestBuilder.DefaultTestMobileNumber)
        {
            MessageBus.Verify(mock => mock.PublishMessage(
                It.Is<SmsRequest>(smsRequest =>
                    smsRequest.MessageType == messageType &&
                    smsRequest.ToNumber == mobileNumber)),
                // TODO: AG: email tokens?
               times);
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