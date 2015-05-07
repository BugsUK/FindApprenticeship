namespace SFA.Apprenticeships.Application.UnitTests.Candidates.Strategies
{
    using System;
    using Application.Candidates.Configuration;
    using Application.Candidates.Strategies;
    using Configuration;
    using Domain.Entities.Candidates;
    using Domain.Entities.UnitTests.Builder;
    using Domain.Entities.Users;
    using Domain.Interfaces.Configuration;
    using Moq;
    using NUnit.Framework;

    [TestFixture]
    public class SendAccountRemindersStrategyBTests
    {
        [Test]
        public void DoNotSendReminderForActivatedUser()
        {
            var candidateId = Guid.NewGuid();
            var dateCreated = DateTime.Now;

            var user = new UserBuilder(candidateId).WithDateCreated(dateCreated).Activated(true).Build();
            var candidate = new CandidateBuilder(candidateId).Build();

            var successor = new Mock<IHousekeepingStrategy>();
            var strategy = new SendAccountRemindersStrategyBBuilder().With(successor.Object).Build();

            strategy.Handle(user, candidate);

            Assert(false, successor, user, candidate);
        }
        
        [Test]
        public void DoNotSendReminderBeforeOneDay()
        {
            var candidateId = Guid.NewGuid();
            var dateCreated = DateTime.Now;

            var user = new UserBuilder(candidateId).WithDateCreated(dateCreated).Activated(false).Build();
            var candidate = new CandidateBuilder(candidateId).Build();

            var successor = new Mock<IHousekeepingStrategy>();
            var strategy = new SendAccountRemindersStrategyBBuilder().With(successor.Object).Build();

            strategy.Handle(user, candidate);

            Assert(false, successor, user, candidate);
        }
        
        [Test]
        public void SendReminderAfterOneDay()
        {
            var candidateId = Guid.NewGuid();
            var dateCreated = DateTime.Now.AddDays(-1);

            var user = new UserBuilder(candidateId).WithDateCreated(dateCreated).Activated(false).Build();
            var candidate = new CandidateBuilder(candidateId).Build();

            var successor = new Mock<IHousekeepingStrategy>();
            var strategy = new SendAccountRemindersStrategyBBuilder().With(successor.Object).Build();

            strategy.Handle(user, candidate);

            Assert(true, successor, user, candidate);
        }
        
        [Test]
        public void DoNotSendReminderAfterTwoDays()
        {
            var candidateId = Guid.NewGuid();
            var dateCreated = DateTime.Now.AddDays(-2);

            var user = new UserBuilder(candidateId).WithDateCreated(dateCreated).Activated(false).Build();
            var candidate = new CandidateBuilder(candidateId).Build();

            var successor = new Mock<IHousekeepingStrategy>();
            var strategy = new SendAccountRemindersStrategyBBuilder().With(successor.Object).Build();

            strategy.Handle(user, candidate);

            Assert(false, successor, user, candidate);
        }

        [TestCase(0, false)]
        [TestCase(1, true)]
        [TestCase(2, false)]
        [TestCase(3, false)]
        [TestCase(4, false)]
        [TestCase(5, false)]
        [TestCase(6, false)]
        [TestCase(7, false)]
        [TestCase(8, true)]
        [TestCase(9, false)]
        [TestCase(10, false)]
        [TestCase(11, false)]
        [TestCase(12, false)]
        [TestCase(13, false)]
        [TestCase(14, false)]
        [TestCase(15, true)]
        [TestCase(16, false)]
        [TestCase(17, false)]
        [TestCase(18, false)]
        [TestCase(19, false)]
        [TestCase(20, false)]
        [TestCase(21, false)]
        [TestCase(22, true)]
        [TestCase(23, false)]
        [TestCase(24, false)]
        [TestCase(25, false)]
        [TestCase(26, false)]
        [TestCase(27, false)]
        [TestCase(28, false)]
        [TestCase(29, true)]
        [TestCase(30, false)]
        [TestCase(31, false)]
        [TestCase(32, false)]
        [TestCase(33, false)]
        [TestCase(34, false)]
        [TestCase(35, false)]
        [TestCase(36, false)]
        [TestCase(37, false)]
        [TestCase(38, false)]
        [TestCase(39, false)]
        [TestCase(40, false)]
        public void CompleteStrategyBTestDays(int days, bool shouldSendReminder)
        {
            var candidateId = Guid.NewGuid();
            var dateCreated = DateTime.Now.AddDays(-days).AddHours(-12);

            var user = new UserBuilder(candidateId).WithDateCreated(dateCreated).Activated(false).Build();
            var candidate = new CandidateBuilder(candidateId).Build();

            var successor = new Mock<IHousekeepingStrategy>();
            var strategy = new SendAccountRemindersStrategyBBuilder().With(successor.Object).Build();

            strategy.Handle(user, candidate);

            Assert(shouldSendReminder, successor, user, candidate);
        }

        [TestCase(0, false)]
        [TestCase(1, true)]
        [TestCase(2, true)]
        [TestCase(3, true)]
        [TestCase(4, true)]
        [TestCase(5, true)]
        [TestCase(6, false)]
        [TestCase(7, false)]
        [TestCase(8, false)]
        [TestCase(9, false)]
        [TestCase(10, false)]
        public void CompleteStrategyBTestHours(int hours, bool shouldSendReminder)
        {
            var candidateId = Guid.NewGuid();
            var dateCreated = DateTime.Now.AddHours(-hours).AddMinutes(-30);

            var user = new UserBuilder(candidateId).WithDateCreated(dateCreated).Activated(false).Build();
            var candidate = new CandidateBuilder(candidateId).Build();

            var configurationService = new Mock<IConfigurationService>();
            configurationService.Setup(s => s.Get<HousekeepingConfiguration>()).Returns(new HousekeepingConfigurationBuilder().With(1, 1, 1, 6).Build());
            var successor = new Mock<IHousekeepingStrategy>();
            var strategy = new SendAccountRemindersStrategyBBuilder().With(configurationService).With(successor.Object).Build();

            strategy.Handle(user, candidate);

            Assert(shouldSendReminder, successor, user, candidate);
        }

        private static void Assert(bool shouldSendReminder, Mock<IHousekeepingStrategy> successor, User user, Candidate candidate)
        {
            if (shouldSendReminder)
            {
                //Strategy handled the request
                successor.Verify(s => s.Handle(user, candidate), Times.Never);
            }
            else
            {
                //Strategy did not handle the request
                successor.Verify(s => s.Handle(user, candidate), Times.Once);
            }
        }
    }
}