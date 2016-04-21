namespace SFA.Apprenticeships.Data.Migrate.UnitTests.Faa.Mappers
{
    using System.Collections.Generic;
    using Builders;
    using FluentAssertions;
    using Migrate.Faa.Mappers;
    using NUnit.Framework;

    [TestFixture]
    public class CandidateMappersTests
    {
        private readonly ICandidateMappers _candidateMappers = new CandidateMappers(-1);

        [Test]
        public void PendingActivationCandidateUserTest()
        {
            //Arrange
            var candidateUser = new CandidateUserBuilder().WithStatus(10).Build();

            //Act
            var candidatePerson = _candidateMappers.MapCandidatePerson(candidateUser, new Dictionary<string, int>());
            var candidate = candidatePerson.Candidate;
            var person = candidatePerson.Person;

            //Assert
            candidate.CandidateId.Should().Be(candidateUser.Candidate.LegacyCandidateId);
            candidate.PersonId.Should().Be(0);
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

            person.Title.Should().Be(0);
            person.OtherTitle.Should().Be("");
            person.FirstName.Should().Be(candidateUser.Candidate.RegistrationDetails.FirstName);
            person.MiddleNames.Should().Be(candidateUser.Candidate.RegistrationDetails.MiddleNames);
            person.Surname.Should().Be(candidateUser.Candidate.RegistrationDetails.LastName);
            person.LandlineNumber.Should().Be(candidateUser.Candidate.RegistrationDetails.PhoneNumber);
            person.MobileNumber.Should().Be("");
            person.Email.Should().Be(candidateUser.Candidate.RegistrationDetails.EmailAddress.ToLower());
            person.PersonTypeId.Should().Be(1);
        }

        [Test]
        public void CandidateUserDictionaryTest()
        {
            //Arrange
            var candidateUser = new CandidateUserBuilder().WithStatus(10).Build();

            //Act
            var candidatePerson = _candidateMappers.MapCandidatePerson(candidateUser, new Dictionary<string, int>());
            var candidate = candidatePerson.Candidate;
            var person = candidatePerson.Person;
            var candidatePersonDictionary = _candidateMappers.MapCandidatePersonDictionary(candidateUser, new Dictionary<string, int>());
            var candidateDictionary = candidatePersonDictionary.Candidate;
            var personDictionary = candidatePersonDictionary.Person;

            //Assert
            candidateDictionary["CandidateId"].Should().Be(candidate.CandidateId);
            candidateDictionary["PersonId"].Should().Be(candidate.PersonId);
            candidateDictionary["CandidateStatusTypeId"].Should().Be(candidate.CandidateStatusTypeId);
            candidateDictionary["DateofBirth"].Should().Be(candidate.DateofBirth);
            candidateDictionary["AddressLine1"].Should().Be(candidate.AddressLine1);
            candidateDictionary["AddressLine2"].Should().Be(candidate.AddressLine2);
            candidateDictionary["AddressLine3"].Should().Be(candidate.AddressLine3);
            candidateDictionary["AddressLine4"].Should().Be(candidate.AddressLine4);
            candidateDictionary["AddressLine5"].Should().Be(candidate.AddressLine5);
            candidateDictionary["Town"].Should().Be(candidate.Town);
            candidateDictionary["CountyId"].Should().Be(candidate.CountyId);
            candidateDictionary["Postcode"].Should().Be(candidate.Postcode);
            candidateDictionary["LocalAuthorityId"].Should().Be(candidate.LocalAuthorityId);
            candidateDictionary["Longitude"].Should().Be(candidate.Longitude);
            candidateDictionary["Latitude"].Should().Be(candidate.Latitude);
            candidateDictionary["GeocodeEasting"].Should().Be(candidate.GeocodeEasting);
            candidateDictionary["GeocodeNorthing"].Should().Be(candidate.GeocodeNorthing);
            candidateDictionary["NiReference"].Should().Be(candidate.NiReference);
            candidateDictionary["VoucherReferenceNumber"].Should().Be(candidate.VoucherReferenceNumber);
            candidateDictionary["UniqueLearnerNumber"].Should().Be(candidate.UniqueLearnerNumber);
            candidateDictionary["UlnStatusId"].Should().Be(candidate.UlnStatusId);
            candidateDictionary["Gender"].Should().Be(candidate.Gender);
            candidateDictionary["EthnicOrigin"].Should().Be(candidate.EthnicOrigin);
            candidateDictionary["EthnicOriginOther"].Should().Be(candidate.EthnicOriginOther);
            candidateDictionary["ApplicationLimitEnforced"].Should().Be(candidate.ApplicationLimitEnforced);
            candidateDictionary["LastAccessedDate"].Should().Be(candidate.LastAccessedDate);
            candidateDictionary["AdditionalEmail"].Should().Be(candidate.AdditionalEmail);
            candidateDictionary["Disability"].Should().Be(candidate.Disability);
            candidateDictionary["DisabilityOther"].Should().Be(candidate.DisabilityOther);
            candidateDictionary["HealthProblems"].Should().Be(candidate.HealthProblems);
            candidateDictionary["ReceivePushedContent"].Should().Be(candidate.ReceivePushedContent);
            candidateDictionary["ReferralAgent"].Should().Be(candidate.ReferralAgent);
            candidateDictionary["DisableAlerts"].Should().Be(candidate.DisableAlerts);
            candidateDictionary["UnconfirmedEmailAddress"].Should().Be(candidate.UnconfirmedEmailAddress);
            candidateDictionary["MobileNumberUnconfirmed"].Should().Be(candidate.MobileNumberUnconfirmed);
            candidateDictionary["DoBFailureCount"].Should().Be(candidate.DoBFailureCount);
            candidateDictionary["ForgottenUsernameRequested"].Should().Be(candidate.ForgottenUsernameRequested);
            candidateDictionary["ForgottenPasswordRequested"].Should().Be(candidate.ForgottenPasswordRequested);
            candidateDictionary["TextFailureCount"].Should().Be(candidate.TextFailureCount);
            candidateDictionary["EmailFailureCount"].Should().Be(candidate.EmailFailureCount);
            candidateDictionary["LastAccessedManageApplications"].Should().Be(candidate.LastAccessedManageApplications);
            candidateDictionary["ReferralPoints"].Should().Be(candidate.ReferralPoints);
            candidateDictionary["BeingSupportedBy"].Should().Be(candidate.BeingSupportedBy);
            candidateDictionary["LockedForSupportUntil"].Should().Be(candidate.LockedForSupportUntil);
            candidateDictionary["NewVacancyAlertEmail"].Should().Be(candidate.NewVacancyAlertEmail);
            candidateDictionary["NewVacancyAlertSMS"].Should().Be(candidate.NewVacancyAlertSMS);
            candidateDictionary["AllowMarketingMessages"].Should().Be(candidate.AllowMarketingMessages);
            candidateDictionary["ReminderMessageSent"].Should().Be(candidate.ReminderMessageSent);
            candidateDictionary["CandidateGuid"].Should().Be(candidate.CandidateGuid);

            personDictionary["Title"].Should().Be(person.Title);
            personDictionary["OtherTitle"].Should().Be(person.OtherTitle);
            personDictionary["FirstName"].Should().Be(person.FirstName);
            personDictionary["MiddleNames"].Should().Be(person.MiddleNames);
            personDictionary["Surname"].Should().Be(person.Surname);
            personDictionary["LandlineNumber"].Should().Be(person.LandlineNumber);
            personDictionary["MobileNumber"].Should().Be(person.MobileNumber);
            personDictionary["Email"].Should().Be(person.Email);
            personDictionary["PersonTypeId"].Should().Be(person.PersonTypeId);
        }

        [Test]
        public void NoLegacyIdCandidateTest()
        {
            //Arrange
            var candidateUser = new CandidateUserBuilder().WithLegacyCandidateId(0).Build();

            //Act
            var candidatePerson = _candidateMappers.MapCandidatePerson(candidateUser, new Dictionary<string, int>());
            var candidate = candidatePerson.Candidate;

            //Assert
            candidate.CandidateId.Should().Be(-1);
        }
    }
}