namespace SFA.Apprenticeships.Domain.Entities.UnitTests.Candidates
{
    using System;
    using Builder;
    using Entities.Candidates;
    using FluentAssertions;
    using NUnit.Framework;

    [TestFixture]
    public class CandidateHelperTests
    {
        [TestCase(true, true, true, true)]
        [TestCase(true, true, false, true)]
        [TestCase(true, false, false, true)]
        [TestCase(false, true, false, false)]
        [TestCase(false, false, false, false)]
        [TestCase(false, false, true, false)]
        public void ShouldCommunicateWithCandidate(bool allowEmail, bool allowSms, bool verifiedMobile, bool expected)
        {
            var candidate = new CandidateBuilder(Guid.NewGuid())
                .AllowEmail(allowEmail)
                .AllowMobile(allowSms)
                .VerifiedMobile(verifiedMobile)
                .Build();

            candidate.AllowsCommunication().Should().Be(expected);
        }
    }
}