namespace SFA.Apprenticeships.Data.Migrate.UnitTests.Faa.Mappers
{
    using System;
    using System.Collections.Generic;
    using Builders;
    using FluentAssertions;
    using Migrate.Faa.Mappers;
    using Moq;
    using NUnit.Framework;
    using SFA.Infrastructure.Interfaces;

    [TestFixture]
    public class CandidateMappersTests
    {
        private readonly ICandidateMappers _candidateMappers = new CandidateMappers(new Mock<ILogService>().Object);

        [Test]
        public void PendingActivationCandidateUserTest()
        {
            //Arrange
            var candidateUser = new CandidateUserBuilder().WithStatus(10).Build();

            //Act
            var candidatePerson = _candidateMappers.MapCandidatePerson(candidateUser, new Dictionary<Guid, int>(), new Dictionary<string, int>());
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
            candidate.AddressLine4.Should().Be(candidateUser.Candidate.RegistrationDetails.Address.AddressLine4);
            candidate.AddressLine5.Should().Be("");
            candidate.Town.Should().Be("N/A");
            candidate.CountyId.Should().Be(0);
            candidate.Postcode.Should().Be(candidateUser.Candidate.RegistrationDetails.Address.Postcode);
            candidate.LocalAuthorityId.Should().Be(0);
            candidate.Longitude.Should().Be((decimal)candidateUser.Candidate.RegistrationDetails.Address.GeoPoint.Longitude);
            candidate.Latitude.Should().Be((decimal)candidateUser.Candidate.RegistrationDetails.Address.GeoPoint.Latitude);
            candidate.GeocodeEasting.Should().Be(null);
            candidate.GeocodeNorthing.Should().Be(null);
            candidate.NiReference.Should().Be("");
            candidate.VoucherReferenceNumber.Should().Be(null);
            candidate.UniqueLearnerNumber.Should().Be(null);
            candidate.UlnStatusId.Should().Be(0);
            candidate.Gender.Should().Be(0);
            candidate.EthnicOrigin.Should().Be(0);
            candidate.EthnicOriginOther.Should().Be("");
            candidate.ApplicationLimitEnforced.Should().BeFalse();
            candidate.LastAccessedDate.Should().Be(candidateUser.User.LastLogin);
            candidate.AdditionalEmail.Should().Be(candidateUser.Candidate.RegistrationDetails.EmailAddress.ToLower());
            candidate.Disability.Should().Be(14);
            candidate.DisabilityOther.Should().Be("");
            candidate.HealthProblems.Should().Be("");
            candidate.ReceivePushedContent.Should().BeFalse();
            candidate.ReferralAgent.Should().BeFalse();
            candidate.DisableAlerts.Should().BeFalse();
            candidate.UnconfirmedEmailAddress.Should().Be("");
            candidate.MobileNumberUnconfirmed.Should().BeFalse();
            candidate.DoBFailureCount.Should().Be(null);
            candidate.ForgottenUsernameRequested.Should().BeFalse();
            candidate.ForgottenPasswordRequested.Should().BeFalse();
            candidate.TextFailureCount.Should().Be(0);
            candidate.EmailFailureCount.Should().Be(0);
            candidate.LastAccessedManageApplications.Should().Be(null);
            candidate.ReferralPoints.Should().Be(0);
            candidate.BeingSupportedBy.Should().Be("NAS Exemplar");
            candidate.LockedForSupportUntil.Should().Be(null);
            candidate.NewVacancyAlertEmail.Should().Be(null);
            candidate.NewVacancyAlertSMS.Should().Be(null);
            candidate.AllowMarketingMessages.Should().BeFalse();
            candidate.ReminderMessageSent.Should().BeTrue();
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

        [TestCase(0, 0)]
        [TestCase(1, 1)]
        [TestCase(2, 2)]
        [TestCase(3, 3)]
        [TestCase(4, 3)]
        public void GenderTest(int gender, int expectedGender)
        {
            //Arrange
            var candidateUser = new CandidateUserBuilder().WithStatus(10).WithGender(gender).Build();

            //Act
            var candidatePerson = _candidateMappers.MapCandidatePerson(candidateUser, new Dictionary<Guid, int>(), new Dictionary<string, int>());
            var candidate = candidatePerson.Candidate;

            //Assert
            candidate.Gender.Should().Be(expectedGender);
        }

        // Prefer not to say
        [TestCase(99, 20, "Not provided")] 

        // White
        [TestCase(31, 16, "")] // English / Welsh / Scottish / Northern Irish / British
        [TestCase(32, 17, "")] // Irish
        [TestCase(33, 20, "Gypsy or Irish Traveller")] // Gypsy or Irish Traveller
        [TestCase(34, 18, "")] // Any other White background

        // Mixed / Multiple ethnic groups
        [TestCase(35, 13, "")] // White and Black Caribbean
        [TestCase(36, 12, "")] // White and Black African
        [TestCase(37, 11, "")] // White and Asian
        [TestCase(38, 14, "")] // Any other Mixed / Multiple ethnic background

        // Asian / Asian British
        [TestCase(39, 3, "")] // Indian
        [TestCase(40, 4, "")] // Pakistani
        [TestCase(41, 2, "")] // Bangladeshi
        [TestCase(42, 19, "")] // Chinese
        [TestCase(43, 5, "")] // Any other Asian background

        // Black / African / Caribbean / Black British
        [TestCase(44, 7, "")] // African
        [TestCase(45, 8, "")] // Caribbean
        [TestCase(46, 9, "")] // Any other Black / African / Caribbean background

        // Other ethnic group
        [TestCase(47, 20, "Arab")] // Arab
        [TestCase(98, 20, "")] // Any other ethnic group
        public void EthnicOriginTest(int ethnicity, int expectedEthnicOrigin, string expectedEthnicOriginOther)
        {
            //Arrange
            var candidateUser = new CandidateUserBuilder().WithStatus(10).WithEthnicity(ethnicity).Build();

            //Act
            var candidatePerson = _candidateMappers.MapCandidatePerson(candidateUser, new Dictionary<Guid, int>(), new Dictionary<string, int>());
            var candidate = candidatePerson.Candidate;

            //Assert
            candidate.EthnicOrigin.Should().Be(expectedEthnicOrigin);
            candidate.EthnicOriginOther.Should().Be(expectedEthnicOriginOther);
        }

        [TestCase(0, 14, "")]
        [TestCase(1, 13, "Other")]
        [TestCase(2, 0, "")]
        [TestCase(3, 14, "")]
        public void DisabilityTest(int disabilityStatus, int expectedDisability, string expectedDisabilityOther)
        {
            //Arrange
            var candidateUser = new CandidateUserBuilder().WithStatus(10).WithDisabilityStatus(disabilityStatus).Build();

            //Act
            var candidatePerson = _candidateMappers.MapCandidatePerson(candidateUser, new Dictionary<Guid, int>(), new Dictionary<string, int>());
            var candidate = candidatePerson.Candidate;

            //Assert
            candidate.Disability.Should().Be(expectedDisability);
            candidate.DisabilityOther.Should().Be(expectedDisabilityOther);
        }

        [Test]
        public void CandidateUserDictionaryTest()
        {
            //Arrange
            var candidateUser = new CandidateUserBuilder().WithStatus(10).Build();

            //Act
            var candidatePerson = _candidateMappers.MapCandidatePerson(candidateUser, new Dictionary<Guid, int>(), new Dictionary<string, int>());
            var candidate = candidatePerson.Candidate;
            var person = candidatePerson.Person;
            var candidateDictionary = _candidateMappers.MapCandidateDictionary(candidate);
            var personDictionary = _candidateMappers.MapPersonDictionary(person);

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
            var candidatePerson = _candidateMappers.MapCandidatePerson(candidateUser, new Dictionary<Guid, int>(), new Dictionary<string, int>());
            var candidate = candidatePerson.Candidate;

            //Assert
            candidate.CandidateId.Should().Be(0);
        }

        [Test]
        public void MatchingCandidateIdCandidateTest()
        {
            //Arrange
            var candidateUser = new CandidateUserBuilder().WithLegacyCandidateId(0).Build();
            const int candidateId = 42;
            var candidateIds = new Dictionary<Guid, int>
            {
                {candidateUser.Candidate.Id, candidateId}
            };

            //Act
            var candidatePerson = _candidateMappers.MapCandidatePerson(candidateUser, candidateIds, new Dictionary<string, int>());
            var candidate = candidatePerson.Candidate;

            //Assert
            candidate.CandidateId.Should().Be(candidateId);
        }

        [Test]
        public void MatchingPersonIdCandidateTest()
        {
            //Arrange
            var candidateUser = new CandidateUserBuilder().WithLegacyCandidateId(0).Build();
            const int personId = 42;
            var personIds = new Dictionary<string, int>
            {
                {candidateUser.Candidate.RegistrationDetails.EmailAddress.ToLower(), personId}
            };

            //Act
            var candidatePerson = _candidateMappers.MapCandidatePerson(candidateUser, new Dictionary<Guid, int>(), personIds);
            var candidate = candidatePerson.Candidate;

            //Assert
            candidate.PersonId.Should().Be(personId);
        }
    }
}