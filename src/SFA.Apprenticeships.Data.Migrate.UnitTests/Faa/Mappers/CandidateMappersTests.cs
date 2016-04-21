namespace SFA.Apprenticeships.Data.Migrate.UnitTests.Faa.Mappers
{
    using Builders;
    using FluentAssertions;
    using Migrate.Faa.Mappers;
    using NUnit.Framework;

    [TestFixture]
    public class CandidateMappersTests
    {
        private readonly ICandidateMappers _candidateMappers = new CandidateMappers();

        [Test]
        public void PendingActivationCandidateUserTest()
        {
            //Arrange
            var candidateUser = new CandidateUserBuilder().WithStatus(10).Build();

            //Act
            var candidate = _candidateMappers.MapCandidate(candidateUser);

            //Assert
            candidate.CandidateId.Should().Be(candidateUser.Candidate.LegacyCandidateId);
            candidate.PersonId.Should().Be(-1);
            candidate.CandidateStatusTypeId.Should().Be(1);
            /*candidate.DateofBirth.Should().Be(0);
            candidate.AddressLine1.Should().Be(0);
            candidate.AddressLine2.Should().Be(0);
            candidate.AddressLine3.Should().Be(0);
            candidate.AddressLine4.Should().Be(0);
            candidate.AddressLine5.Should().Be(0);
            candidate.Town.Should().Be(0);
            candidate.CountyId.Should().Be(0);
            candidate.Postcode.Should().Be(0);
            candidate.LocalAuthorityId.Should().Be(0);
            candidate.Longitude.Should().Be(0);
            candidate.Latitude.Should().Be(0);
            candidate.GeocodeEasting.Should().Be(0);
            candidate.GeocodeNorthing.Should().Be(0);
            candidate.NiReference.Should().Be(0);
            candidate.VoucherReferenceNumber.Should().Be(0);
            candidate.UniqueLearnerNumber.Should().Be(0);
            candidate.UlnStatusId.Should().Be(0);
            candidate.Gender.Should().Be(0);
            candidate.EthnicOrigin.Should().Be(0);
            candidate.EthnicOriginOther.Should().Be(0);
            candidate.ApplicationLimitEnforced.Should().Be(0);
            candidate.LastAccessedDate.Should().Be(0);
            candidate.AdditionalEmail.Should().Be(0);
            candidate.Disability.Should().Be(0);
            candidate.DisabilityOther.Should().Be(0);
            candidate.HealthProblems.Should().Be(0);
            candidate.ReceivePushedContent.Should().Be(0);
            candidate.ReferralAgent.Should().Be(0);
            candidate.DisableAlerts.Should().Be(0);
            candidate.UnconfirmedEmailAddress.Should().Be(0);
            candidate.MobileNumberUnconfirmed.Should().Be(0);
            candidate.DoBFailureCount.Should().Be(0);
            candidate.ForgottenUsernameRequested.Should().Be(0);
            candidate.ForgottenPasswordRequested.Should().Be(0);
            candidate.TextFailureCount.Should().Be(0);
            candidate.EmailFailureCount.Should().Be(0);
            candidate.LastAccessedManageApplications.Should().Be(0);
            candidate.ReferralPoints.Should().Be(0);
            candidate.BeingSupportedBy.Should().Be(0);
            candidate.LockedForSupportUntil.Should().Be(0);
            candidate.NewVacancyAlertEmail.Should().Be(0);
            candidate.NewVacancyAlertSMS.Should().Be(0);
            candidate.AllowMarketingMessages.Should().Be(0);
            candidate.ReminderMessageSent.Should().Be(0);*/
            candidate.CandidateGuid.Should().Be(candidateUser.Candidate.Id);
        }
    }
}