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
            candidate.DateofBirth.Should().Be(candidateUser.Candidate.RegistrationDetails.DateOfBirth);
            candidate.AddressLine1.Should().Be(candidateUser.Candidate.RegistrationDetails.Address.AddressLine1);
            candidate.AddressLine2.Should().Be(candidateUser.Candidate.RegistrationDetails.Address.AddressLine2);
            candidate.AddressLine3.Should().Be(candidateUser.Candidate.RegistrationDetails.Address.AddressLine3);
            candidate.AddressLine4.Should().Be("");
            candidate.AddressLine5.Should().Be(null);
            candidate.Town.Should().Be(candidateUser.Candidate.RegistrationDetails.Address.AddressLine4);
            //candidate.CountyId.Should().Be(0);
            candidate.Postcode.Should().Be(candidateUser.Candidate.RegistrationDetails.Address.Postcode);
            /*candidate.LocalAuthorityId.Should().Be(0);
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

        [Test]
        public void CandidateUserDictionaryTest()
        {
            //Arrange
            var candidateUser = new CandidateUserBuilder().WithStatus(10).Build();

            //Act
            var candidate = _candidateMappers.MapCandidate(candidateUser);
            var candidatDictionary = _candidateMappers.MapCandidateDictionary(candidateUser);

            //Assert
            candidatDictionary["CandidateId"].Should().Be(candidate.CandidateId);
            candidatDictionary["PersonId"].Should().Be(candidate.PersonId);
            candidatDictionary["CandidateStatusTypeId"].Should().Be(candidate.CandidateStatusTypeId);
            candidatDictionary["DateofBirth"].Should().Be(candidate.DateofBirth);
            candidatDictionary["AddressLine1"].Should().Be(candidate.AddressLine1);
            candidatDictionary["AddressLine2"].Should().Be(candidate.AddressLine2);
            candidatDictionary["AddressLine3"].Should().Be(candidate.AddressLine3);
            candidatDictionary["AddressLine4"].Should().Be(candidate.AddressLine4);
            candidatDictionary["AddressLine5"].Should().Be(candidate.AddressLine5);
            candidatDictionary["Town"].Should().Be(candidate.Town);
            candidatDictionary["CountyId"].Should().Be(candidate.CountyId);
            candidatDictionary["Postcode"].Should().Be(candidate.Postcode);
            candidatDictionary["LocalAuthorityId"].Should().Be(candidate.LocalAuthorityId);
            candidatDictionary["Longitude"].Should().Be(candidate.Longitude);
            candidatDictionary["Latitude"].Should().Be(candidate.Latitude);
            candidatDictionary["GeocodeEasting"].Should().Be(candidate.GeocodeEasting);
            candidatDictionary["GeocodeNorthing"].Should().Be(candidate.GeocodeNorthing);
            candidatDictionary["NiReference"].Should().Be(candidate.NiReference);
            candidatDictionary["VoucherReferenceNumber"].Should().Be(candidate.VoucherReferenceNumber);
            candidatDictionary["UniqueLearnerNumber"].Should().Be(candidate.UniqueLearnerNumber);
            candidatDictionary["UlnStatusId"].Should().Be(candidate.UlnStatusId);
            candidatDictionary["Gender"].Should().Be(candidate.Gender);
            candidatDictionary["EthnicOrigin"].Should().Be(candidate.EthnicOrigin);
            candidatDictionary["EthnicOriginOther"].Should().Be(candidate.EthnicOriginOther);
            candidatDictionary["ApplicationLimitEnforced"].Should().Be(candidate.ApplicationLimitEnforced);
            candidatDictionary["LastAccessedDate"].Should().Be(candidate.LastAccessedDate);
            candidatDictionary["AdditionalEmail"].Should().Be(candidate.AdditionalEmail);
            candidatDictionary["Disability"].Should().Be(candidate.Disability);
            candidatDictionary["DisabilityOther"].Should().Be(candidate.DisabilityOther);
            candidatDictionary["HealthProblems"].Should().Be(candidate.HealthProblems);
            candidatDictionary["ReceivePushedContent"].Should().Be(candidate.ReceivePushedContent);
            candidatDictionary["ReferralAgent"].Should().Be(candidate.ReferralAgent);
            candidatDictionary["DisableAlerts"].Should().Be(candidate.DisableAlerts);
            candidatDictionary["UnconfirmedEmailAddress"].Should().Be(candidate.UnconfirmedEmailAddress);
            candidatDictionary["MobileNumberUnconfirmed"].Should().Be(candidate.MobileNumberUnconfirmed);
            candidatDictionary["DoBFailureCount"].Should().Be(candidate.DoBFailureCount);
            candidatDictionary["ForgottenUsernameRequested"].Should().Be(candidate.ForgottenUsernameRequested);
            candidatDictionary["ForgottenPasswordRequested"].Should().Be(candidate.ForgottenPasswordRequested);
            candidatDictionary["TextFailureCount"].Should().Be(candidate.TextFailureCount);
            candidatDictionary["EmailFailureCount"].Should().Be(candidate.EmailFailureCount);
            candidatDictionary["LastAccessedManageApplications"].Should().Be(candidate.LastAccessedManageApplications);
            candidatDictionary["ReferralPoints"].Should().Be(candidate.ReferralPoints);
            candidatDictionary["BeingSupportedBy"].Should().Be(candidate.BeingSupportedBy);
            candidatDictionary["LockedForSupportUntil"].Should().Be(candidate.LockedForSupportUntil);
            candidatDictionary["NewVacancyAlertEmail"].Should().Be(candidate.NewVacancyAlertEmail);
            candidatDictionary["NewVacancyAlertSMS"].Should().Be(candidate.NewVacancyAlertSMS);
            candidatDictionary["AllowMarketingMessages"].Should().Be(candidate.AllowMarketingMessages);
            candidatDictionary["ReminderMessageSent"].Should().Be(candidate.ReminderMessageSent);
            candidatDictionary["CandidateGuid"].Should().Be(candidate.CandidateGuid);
        }
    }
}