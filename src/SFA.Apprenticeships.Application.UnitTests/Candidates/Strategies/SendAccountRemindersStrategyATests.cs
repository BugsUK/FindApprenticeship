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
    public class SendAccountRemindersStrategyATests
    {
        private Guid CandidateId = Guid.Parse("727d9d37-3962-43d7-bcf2-a96e1bd93397");

        [TestCase("727d9d37-3962-43d7-bcf2-a96e1bd93396", false)]
        [TestCase("727d9d37-3962-43d7-bcf2-a96e1bd93397", true)]
        [TestCase("727d9d37-3962-43d7-bcf2-a96e1bd93398", false)]
        [TestCase("727d9d37-3962-43d7-bcf2-a96e1bd93399", true)]
        public void BaseStrategyOnGuid(string candidateIdGuid, bool shouldSendReminder)
        {
            var candidateId = Guid.Parse(candidateIdGuid);

            var dateCreated = DateTime.Now.AddDays(-7);

            var user = new UserBuilder(candidateId).WithDateCreated(dateCreated).Activated(false).Build();
            var candidate = new CandidateBuilder(candidateId).Build();

            var successor = new Mock<IHousekeepingStrategy>();
            var strategy = new SendAccountRemindersStrategyABuilder().With(successor.Object).Build();

            strategy.Handle(user, candidate);

            Assert(shouldSendReminder, successor, user, candidate);
        }

        [Test]
        public void DoNotSendReminderForActivatedUser()
        {
            var dateCreated = DateTime.Now;

            var user = new UserBuilder(CandidateId).WithDateCreated(dateCreated).Activated(true).Build();
            var candidate = new CandidateBuilder(CandidateId).Build();

            var successor = new Mock<IHousekeepingStrategy>();
            var strategy = new SendAccountRemindersStrategyABuilder().With(successor.Object).Build();

            strategy.Handle(user, candidate);

            Assert(false, successor, user, candidate);
        }
        
        [Test]
        public void DoNotSendReminderBeforeSevenDays()
        {
            var dateCreated = DateTime.Now;

            var user = new UserBuilder(CandidateId).WithDateCreated(dateCreated).Activated(false).Build();
            var candidate = new CandidateBuilder(CandidateId).Build();

            var successor = new Mock<IHousekeepingStrategy>();
            var strategy = new SendAccountRemindersStrategyABuilder().With(successor.Object).Build();

            strategy.Handle(user, candidate);

            Assert(false, successor, user, candidate);
        }
        
        [Test]
        public void SendReminderAfterSevenDays()
        {
            var dateCreated = DateTime.Now.AddDays(-7);

            var user = new UserBuilder(CandidateId).WithDateCreated(dateCreated).Activated(false).Build();
            var candidate = new CandidateBuilder(CandidateId).Build();

            var successor = new Mock<IHousekeepingStrategy>();
            var strategy = new SendAccountRemindersStrategyABuilder().With(successor.Object).Build();

            strategy.Handle(user, candidate);

            Assert(true, successor, user, candidate);
        }
        
        [Test]
        public void DoNotSendReminderAfterTwentyDays()
        {
            var dateCreated = DateTime.Now.AddDays(-20);

            var user = new UserBuilder(CandidateId).WithDateCreated(dateCreated).Activated(false).Build();
            var candidate = new CandidateBuilder(CandidateId).Build();

            var successor = new Mock<IHousekeepingStrategy>();
            var strategy = new SendAccountRemindersStrategyABuilder().With(successor.Object).Build();

            strategy.Handle(user, candidate);

            Assert(false, successor, user, candidate);
        }

        [Test]
        public void SendReminderAfterTwentyOneDays()
        {
            var dateCreated = DateTime.Now.AddDays(-21);

            var user = new UserBuilder(CandidateId).WithDateCreated(dateCreated).Activated(false).Build();
            var candidate = new CandidateBuilder(CandidateId).Build();

            var successor = new Mock<IHousekeepingStrategy>();
            var strategy = new SendAccountRemindersStrategyABuilder().With(successor.Object).Build();

            strategy.Handle(user, candidate);

            Assert(true, successor, user, candidate);
        }

        [TestCase(0, false)]
        [TestCase(1, false)]
        [TestCase(2, false)]
        [TestCase(3, false)]
        [TestCase(4, false)]
        [TestCase(5, false)]
        [TestCase(6, false)]
        [TestCase(7, true)]
        [TestCase(8, false)]
        [TestCase(9, false)]
        [TestCase(10, false)]
        [TestCase(11, false)]
        [TestCase(12, false)]
        [TestCase(13, false)]
        [TestCase(14, false)]
        [TestCase(15, false)]
        [TestCase(16, false)]
        [TestCase(17, false)]
        [TestCase(18, false)]
        [TestCase(19, false)]
        [TestCase(20, false)]
        [TestCase(21, true)]
        [TestCase(22, false)]
        [TestCase(23, false)]
        [TestCase(24, false)]
        [TestCase(25, false)]
        [TestCase(26, false)]
        [TestCase(27, false)]
        [TestCase(28, false)]
        [TestCase(29, false)]
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
        public void CompleteStrategyATestDays(int days, bool shouldSendReminder)
        {
            var dateCreated = DateTime.Now.AddDays(-days).AddHours(-12);

            var user = new UserBuilder(CandidateId).WithDateCreated(dateCreated).Activated(false).Build();
            var candidate = new CandidateBuilder(CandidateId).Build();

            var successor = new Mock<IHousekeepingStrategy>();
            var strategy = new SendAccountRemindersStrategyABuilder().With(successor.Object).Build();

            strategy.Handle(user, candidate);

            Assert(shouldSendReminder, successor, user, candidate);
        }

        [TestCase(0, false)]
        [TestCase(1, false)]
        [TestCase(2, true)]
        [TestCase(3, false)]
        [TestCase(4, true)]
        [TestCase(5, false)]
        [TestCase(6, false)]
        [TestCase(7, false)]
        [TestCase(8, false)]
        [TestCase(9, false)]
        [TestCase(10, false)]
        public void CompleteStrategyATestHours(int hours, bool shouldSendReminder)
        {
            var dateCreated = DateTime.Now.AddHours(-hours).AddMinutes(-30);

            var user = new UserBuilder(CandidateId).WithDateCreated(dateCreated).Activated(false).Build();
            var candidate = new CandidateBuilder(CandidateId).Build();

            var configurationService = new Mock<IConfigurationService>();
            configurationService.Setup(s => s.Get<HousekeepingConfiguration>()).Returns(new HousekeepingConfigurationBuilder().WithStrategyA(1, 2, 4, 6).Build());
            var successor = new Mock<IHousekeepingStrategy>();
            var strategy = new SendAccountRemindersStrategyABuilder().With(configurationService).With(successor.Object).Build();

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