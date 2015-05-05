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
        [TestCase(false, false, false, false)] // no comms, no verified mobile
        [TestCase(false, true, false, false)] // sms only, no verified mobile
        [TestCase(true, false, false, true)] // email only
        [TestCase(true, true, false, true)] // email and sms, no verified mobile
        [TestCase(false, true, true, true)] // // sms only, verified mobile
        public void ShouldAllowCommunication(bool enableAnyEmail, bool enableAnyText, bool verifiedMobile, bool expected)
        {
            var candidate = new CandidateBuilder(Guid.NewGuid())
                .EnableOneCommunicationPreferenceViaEmail(enableAnyEmail)
                .EnableOneCommunicationPreferenceViaText(enableAnyText)
                .VerifiedMobile(verifiedMobile)
                .Build();

            candidate.AllowsCommunication().Should().Be(expected);
        }

        [TestCase(false, false, false)] // no sms
        [TestCase(false, true, false)] // no sms, verified mobile
        [TestCase(true, false, true)] // sms, no verified mobile
        [TestCase(true, true, false)] // sms, verified mobile
        public void ShouldRequireMobileVerification(bool enableAnyText, bool verifiedMobile, bool expected)
        {
            var candidate = new CandidateBuilder(Guid.NewGuid())
                .EnableOneCommunicationPreferenceViaText(enableAnyText)
                .VerifiedMobile(verifiedMobile)
                .Build();

            candidate.MobileVerificationRequired().Should().Be(expected);
        }

        [TestCase(false, false, false, false)] // no email or sms
        [TestCase(false, false, true, false)] // no email or sms, verified mobile
        [TestCase(false, true, false, false)] // sms only, no verified mobile
        [TestCase(false, true, true, true)] // sms only, verified mobile
        [TestCase(true, false, false, true)]  // email only
        public void ShouldSendSavedSearchAlerts(bool enableEmail, bool enableText, bool verifiedMobile, bool expected)
        {
            var candidate = new CandidateBuilder(Guid.NewGuid())
               .EnableSavedSearchAlertsViaEmail(enableEmail)
               .EnableSavedSearchAlertsViaText(enableText)
               .VerifiedMobile(verifiedMobile)
               .Build();

            candidate.ShouldSendSavedSearchAlerts().Should().Be(expected);
        }
    }
}