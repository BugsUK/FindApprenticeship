namespace SFA.Apprenticeships.Infrastructure.Communication.UnitTests.Candidate
{
    using System;
    using Application.Interfaces.Communications;
    using Domain.Entities.UnitTests.Builder;
    using NUnit.Framework;

    [TestFixture]
    public class CandidateCommunicationHelperTests
    {
        [TestCase(CommunicationChannels.Email, true)]
        [TestCase(CommunicationChannels.Email, false)]
        [TestCase(CommunicationChannels.Sms, true)]
        [TestCase(CommunicationChannels.Sms, false)]
        public void ShouldHonourSendApplicationSubmittedCommunicationPreference(CommunicationChannels communicationChannel, bool sendApplicationSubmitted)
        {
            // Arrange.
            var candidate = new CandidateBuilder(Guid.NewGuid())
                .SendApplicationSubmitted(sendApplicationSubmitted)
                .Build();

            // Act.

            // Assert.
            Assert.Fail();
        }
    }
}
