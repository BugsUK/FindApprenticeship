namespace SFA.Apprenticeships.Data.Migrate.UnitTests.Faa.Mappers
{
    using System;
    using System.Collections.Generic;
    using Builders;
    using FluentAssertions;
    using Migrate.Faa.Entities.Mongo;
    using Migrate.Faa.Entities.Sql;
    using Migrate.Faa.Mappers;
    using Moq;
    using NUnit.Framework;
    using SFA.Infrastructure.Interfaces;
    using CandidateSummary = Migrate.Faa.Entities.Sql.CandidateSummary;

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
            var candidatePerson = _candidateMappers.MapCandidatePerson(candidateUser, new Dictionary<Guid, CandidateSummary>(), new Dictionary<string, int>(), new Dictionary<int, int>(), new Dictionary<int, int>(), false);
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
            candidate.Longitude.Should().Be(null);
            candidate.Latitude.Should().Be(null);
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

        [TestCase(0, 1)] //Unknown
        [TestCase(10, 1)] //PendingActivation
        [TestCase(20, 2)] //Active
        //We don't feed back the status changes to AVMS so users never go from Activated to any other state. As a result always return Activated for reporting purposes
        [TestCase(30, 2)] //Inactive
        [TestCase(90, 2)] //Locked
        [TestCase(100, 2)] //Dormant
        [TestCase(999, 2)] //PendingDeletion
        public void StatusTest(int status, int expectedCandidateStatusTypeId)
        {
            //Arrange
            var candidateUser = new CandidateUserBuilder().WithStatus(status).Build();

            //Act
            var candidatePerson = _candidateMappers.MapCandidatePerson(candidateUser, new Dictionary<Guid, CandidateSummary>(), new Dictionary<string, int>(), new Dictionary<int, int>(), new Dictionary<int, int>(), false);
            var candidate = candidatePerson.Candidate;

            //Assert
            candidate.CandidateStatusTypeId.Should().Be(expectedCandidateStatusTypeId);
        }

        [TestCase(0, 0)]
        [TestCase(1, 1)]
        [TestCase(2, 2)]
        [TestCase(3, 3)]
        [TestCase(4, 0)]
        public void GenderTest(int gender, int expectedGender)
        {
            //Arrange
            var candidateUser = new CandidateUserBuilder().WithStatus(10).WithGender(gender).Build();

            //Act
            var candidatePerson = _candidateMappers.MapCandidatePerson(candidateUser, new Dictionary<Guid, CandidateSummary>(), new Dictionary<string, int>(), new Dictionary<int, int>(), new Dictionary<int, int>(), false);
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
            var candidatePerson = _candidateMappers.MapCandidatePerson(candidateUser, new Dictionary<Guid, CandidateSummary>(), new Dictionary<string, int>(), new Dictionary<int, int>(), new Dictionary<int, int>(), false);
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
            var candidatePerson = _candidateMappers.MapCandidatePerson(candidateUser, new Dictionary<Guid, CandidateSummary>(), new Dictionary<string, int>(), new Dictionary<int, int>(), new Dictionary<int, int>(), false);
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
            var candidatePerson = _candidateMappers.MapCandidatePerson(candidateUser, new Dictionary<Guid, CandidateSummary>(), new Dictionary<string, int>(), new Dictionary<int, int>(), new Dictionary<int, int>(), false);
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

            personDictionary["PersonId"].Should().Be(person.PersonId);
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
            var candidatePerson = _candidateMappers.MapCandidatePerson(candidateUser, new Dictionary<Guid, CandidateSummary>(), new Dictionary<string, int>(), new Dictionary<int, int>(), new Dictionary<int, int>(), false);
            var candidate = candidatePerson.Candidate;

            //Assert
            candidate.CandidateId.Should().Be(0);
        }

        [Test]
        public void MatchingCandidateAndPersonIdCandidateTest()
        {
            //Arrange
            var candidateUser = new CandidateUserBuilder().WithLegacyCandidateId(0).Build();
            const int candidateId = 42;
            const int personId = 43;
            var candidateSummaries = new Dictionary<Guid, CandidateSummary>
            {
                {candidateUser.Candidate.Id, new CandidateSummary {CandidateGuid = candidateUser.Candidate.Id, CandidateId = candidateId, PersonId = personId}}
            };

            //Act
            var candidatePerson = _candidateMappers.MapCandidatePerson(candidateUser, candidateSummaries, new Dictionary<string, int>(), new Dictionary<int, int>(), new Dictionary<int, int>(), false);
            var candidate = candidatePerson.Candidate;
            var person = candidatePerson.Person;

            //Assert
            candidate.CandidateId.Should().Be(candidateId);
            candidate.PersonId.Should().Be(personId);
            person.PersonId.Should().Be(personId);
        }

        [Test]
        public void VeryLongEmailTest()
        {
            //Arrange
            var candidateUser = new CandidateUserBuilder().WithEmailAddress("testtesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttest@testtesttesttesttesttesttest.com").Build();

            //Act
            var candidatePerson = _candidateMappers.MapCandidatePerson(candidateUser, new Dictionary<Guid, CandidateSummary>(), new Dictionary<string, int>(), new Dictionary<int, int>(), new Dictionary<int, int>(), false);
            var candidate = candidatePerson.Candidate;
            var person = candidatePerson.Person;

            //Assert
            candidate.AdditionalEmail.Should().Be("");

            person.Email.Should().Be(candidateUser.Candidate.RegistrationDetails.EmailAddress.ToLower());
        }

        [TestCase("B26 2LW", "B262L")]
        [TestCase("B26 2LW", "B262")]
        [TestCase("B26 2LW", "B26")]
        [TestCase("B26 2LW", "B2")]
        [TestCase("B26 2LW", "B")]
        [TestCase("AL10 9AB", "AL109A")]
        [TestCase("AL10 9AB", "AL109")]
        [TestCase("AL10 9AB", "AL10")]
        [TestCase("AL10 9AB", "AL1")]
        [TestCase("AL10 9AB", "AL")]
        public void CountyLocalAuthorityTests(string postCode, string vacancyPostCode)
        {
            //Arrange
            const int countyId = 12;
            const int localAuthorityId = 42;
            var candidateUser = new CandidateUserBuilder().WithStatus(10).WithPostCode(postCode).Build();
            var vacancyLocalAuthorities = new Dictionary<string, int> {{vacancyPostCode, localAuthorityId}};
            var localAuthorityCountyIds = new Dictionary<int, int> {{ localAuthorityId, countyId } };

            //Act
            var candidatePerson = _candidateMappers.MapCandidatePerson(candidateUser, new Dictionary<Guid, CandidateSummary>(), vacancyLocalAuthorities, localAuthorityCountyIds, new Dictionary<int, int>(), false);
            var candidate = candidatePerson.Candidate;

            //Assert
            candidate.CountyId.Should().Be(countyId);
            candidate.LocalAuthorityId.Should().Be(localAuthorityId);
        }

        [Test]
        public void AnonymisationTestTest()
        {
            //Arrange
            var candidateUser = new CandidateUserBuilder().WithStatus(20).Build();

            //Act
            var candidatePerson = _candidateMappers.MapCandidatePerson(candidateUser, new Dictionary<Guid, CandidateSummary>(), new Dictionary<string, int>(), new Dictionary<int, int>(), new Dictionary<int, int>(), true);
            var candidate = candidatePerson.Candidate;
            var person = candidatePerson.Person;

            //Assert
            candidate.CandidateId.Should().Be(candidateUser.Candidate.LegacyCandidateId);
            candidate.PersonId.Should().Be(0);
            candidate.CandidateStatusTypeId.Should().Be(2);
            candidate.DateofBirth.Should().Be(candidateUser.Candidate.RegistrationDetails.DateOfBirth);
            candidate.AddressLine1.Should().Be("");
            candidate.AddressLine2.Should().Be(candidateUser.Candidate.RegistrationDetails.Address.AddressLine2);
            candidate.AddressLine3.Should().Be(candidateUser.Candidate.RegistrationDetails.Address.AddressLine3);
            candidate.AddressLine4.Should().Be(candidateUser.Candidate.RegistrationDetails.Address.AddressLine4);
            candidate.AddressLine5.Should().Be("");
            candidate.Town.Should().Be("N/A");
            candidate.CountyId.Should().Be(0);
            candidate.Postcode.Should().Be(candidateUser.Candidate.RegistrationDetails.Address.Postcode);
            candidate.LocalAuthorityId.Should().Be(0);
            candidate.Longitude.Should().Be(null);
            candidate.Latitude.Should().Be(null);
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
            candidate.AdditionalEmail.Should().Be(candidateUser.Candidate.Id + "@anon.com");
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
            person.FirstName.Should().Be("Candidate");
            person.MiddleNames.Should().Be("");
            person.Surname.Should().Be(candidateUser.Candidate.Id.ToString().Replace("-", ""));
            person.LandlineNumber.Should().Be("07999999999");
            person.MobileNumber.Should().Be("");
            person.Email.Should().Be(candidateUser.Candidate.Id + "@anon.com");
            person.PersonTypeId.Should().Be(1);
        }

        [Test]
        public void ActivatedCandidateWithHistoryTest()
        {
            //Arrange
            var candidateUser = new CandidateUserBuilder().WithStatus(20).Build();

            //Act
            var candidateWithHistory = _candidateMappers.MapCandidateWithHistory(candidateUser, new Dictionary<Guid, CandidateSummary>(), new Dictionary<string, int>(), new Dictionary<int, int>(), new Dictionary<int, int>(), new Dictionary<int, Dictionary<int, int>>(), false);

            //Assert
            var candidatePerson = candidateWithHistory.CandidatePerson;
            candidatePerson.Candidate.CandidateStatusTypeId.Should().Be(2);
            var candidateHistory = candidateWithHistory.CandidateHistory;
            candidateHistory.Should().NotBeNullOrEmpty();
            candidateHistory.Count.Should().Be(3);
            var createdHistory = candidateHistory[0];
            createdHistory.CandidateHistoryEventTypeId.Should().Be(1);
            createdHistory.CandidateHistorySubEventTypeId.Should().Be(1);
            var activatedHistory = candidateHistory[1];
            activatedHistory.CandidateId.Should().Be(candidateUser.Candidate.LegacyCandidateId);
            activatedHistory.CandidateHistoryEventTypeId.Should().Be(1);
            activatedHistory.CandidateHistorySubEventTypeId.Should().Be(2);
            // ReSharper disable once PossibleInvalidOperationException
            activatedHistory.EventDate.Should().Be(candidateUser.User.ActivationDate.Value);
            activatedHistory.Comment.Should().BeNull();
            activatedHistory.UserName.Should().Be("NAS Gateway");
            var noteHistory = candidateHistory[2];
            noteHistory.CandidateId.Should().Be(candidateUser.Candidate.LegacyCandidateId);
            noteHistory.CandidateHistoryEventTypeId.Should().Be(3);
            noteHistory.CandidateHistorySubEventTypeId.Should().Be(0);
            noteHistory.EventDate.Should().Be(candidateUser.User.ActivationDate.Value);
            noteHistory.Comment.Should().Be("NAS Exemplar registered Candidate.");
            noteHistory.UserName.Should().Be("NAS Gateway");
        }

        [Test]
        public void SchoolAttendedTest()
        {
            //Arrange
            const int legacyCandidateId = 42;
            var applicationTemplate = new ApplicationTemplate
            {
                EducationHistory = new Education
                {
                    Institution = "John Port School",
                    FromYear = 1990,
                    ToYear = 1997
                }
            };
            var candidateUser = new CandidateUserBuilder().WithLegacyCandidateId(legacyCandidateId).WithStatus(20).WithApplicationTemplate(applicationTemplate).Build();

            //Act
            var candidatePerson = _candidateMappers.MapCandidatePerson(candidateUser, new Dictionary<Guid, CandidateSummary>(), new Dictionary<string, int>(), new Dictionary<int, int>(), new Dictionary<int, int>(), false);

            //Assert
            candidatePerson.SchoolAttended.Should().NotBeNull();
            candidatePerson.SchoolAttended.SchoolAttendedId.Should().Be(0);
            candidatePerson.SchoolAttended.CandidateId.Should().Be(legacyCandidateId);
            candidatePerson.SchoolAttended.SchoolId.Should().Be(null);
            candidatePerson.SchoolAttended.OtherSchoolName.Should().Be(applicationTemplate.EducationHistory.Institution);
            candidatePerson.SchoolAttended.OtherSchoolTown.Should().BeNull();
            candidatePerson.SchoolAttended.StartDate.Should().Be(new DateTime(applicationTemplate.EducationHistory.FromYear, 1, 1));
            candidatePerson.SchoolAttended.EndDate.Should().Be(new DateTime(applicationTemplate.EducationHistory.ToYear, 1, 1));
            candidatePerson.SchoolAttended.ApplicationId.Should().Be(null);
        }

        [Test]
        public void SchoolAttendedDictionaryTest()
        {
            //Arrange
            const int legacyCandidateId = 42;
            var applicationTemplate = new ApplicationTemplate
            {
                EducationHistory = new Education
                {
                    Institution = "John Port School",
                    FromYear = 1990,
                    ToYear = 1997
                }
            };
            var candidateUser = new CandidateUserBuilder().WithLegacyCandidateId(legacyCandidateId).WithStatus(20).WithApplicationTemplate(applicationTemplate).Build();

            //Act
            var candidatePerson = _candidateMappers.MapCandidatePerson(candidateUser, new Dictionary<Guid, CandidateSummary>(), new Dictionary<string, int>(), new Dictionary<int, int>(), new Dictionary<int, int>(), false);
            var schoolAttendedDictionary = candidatePerson.SchoolAttended.MapSchoolAttendedDictionary();

            //Assert
            schoolAttendedDictionary["SchoolAttendedId"].Should().Be(0);
            schoolAttendedDictionary["CandidateId"].Should().Be(legacyCandidateId);
            schoolAttendedDictionary["SchoolId"].Should().Be(null);
            schoolAttendedDictionary["OtherSchoolName"].Should().Be(applicationTemplate.EducationHistory.Institution);
            schoolAttendedDictionary["OtherSchoolTown"].Should().BeNull();
            schoolAttendedDictionary["StartDate"].Should().Be(new DateTime(applicationTemplate.EducationHistory.FromYear, 1, 1));
            schoolAttendedDictionary["EndDate"].Should().Be(new DateTime(applicationTemplate.EducationHistory.ToYear, 1, 1));
            schoolAttendedDictionary["ApplicationId"].Should().Be(null);
        }

        [Test]
        public void SourceSchoolAttendedTest()
        {
            //Arrange
            const int legacyCandidateId = 42;
            var applicationTemplate = new ApplicationTemplate
            {
                EducationHistory = new Education
                {
                    Institution = "John Port School",
                    FromYear = 1990,
                    ToYear = 1997
                }
            };
            var candidateUser = new CandidateUserBuilder().WithLegacyCandidateId(legacyCandidateId).WithStatus(20).WithApplicationTemplate(applicationTemplate).Build();
            const int schoolAttendedId = 44;
            var schoolAttendedIds = new Dictionary<int, int>
            {
                {legacyCandidateId, schoolAttendedId}
            };

            //Act
            var candidatePerson = _candidateMappers.MapCandidatePerson(candidateUser, new Dictionary<Guid, CandidateSummary>(), new Dictionary<string, int>(), new Dictionary<int, int>(), schoolAttendedIds, false);

            //Assert
            candidatePerson.SchoolAttended.Should().NotBeNull();
            candidatePerson.SchoolAttended.SchoolAttendedId.Should().Be(schoolAttendedId);
            candidatePerson.SchoolAttended.CandidateId.Should().Be(legacyCandidateId);
            candidatePerson.SchoolAttended.SchoolId.Should().Be(null);
            candidatePerson.SchoolAttended.OtherSchoolName.Should().Be(applicationTemplate.EducationHistory.Institution);
            candidatePerson.SchoolAttended.OtherSchoolTown.Should().BeNull();
            candidatePerson.SchoolAttended.StartDate.Should().Be(new DateTime(applicationTemplate.EducationHistory.FromYear, 1, 1));
            candidatePerson.SchoolAttended.EndDate.Should().Be(new DateTime(applicationTemplate.EducationHistory.ToYear, 1, 1));
            candidatePerson.SchoolAttended.ApplicationId.Should().Be(null);
        }

        [Test]
        public void EmptySchoolAttendedTest()
        {
            //Arrange
            const int legacyCandidateId = 42;
            var applicationTemplate = new ApplicationTemplate
            {
                EducationHistory = new Education()
            };
            var candidateUser = new CandidateUserBuilder().WithLegacyCandidateId(legacyCandidateId).WithStatus(20).WithApplicationTemplate(applicationTemplate).Build();

            //Act
            var candidatePerson = _candidateMappers.MapCandidatePerson(candidateUser, new Dictionary<Guid, CandidateSummary>(), new Dictionary<string, int>(), new Dictionary<int, int>(), new Dictionary<int, int>(), false);

            //Assert
            candidatePerson.SchoolAttended.Should().BeNull();
        }

        [TestCase(false, false, false, false)]
        [TestCase(true, false, false, true)]
        [TestCase(false, true, false, false)]
        [TestCase(false, false, true, false)]
        [TestCase(false, true, true, true)]
        [TestCase(true, true, true, true)]
        public void AllowMarketingMessagesCandidateUserTest(bool allowEmail, bool allowText, bool verifiedMobile, bool expected)
        {
            //Arrange
            var communicationPreferences = new CommunicationPreferences
            {
                VerifiedMobile = verifiedMobile,
                MarketingPreferences = new CommunicationPreference
                {
                    EnableEmail = allowEmail,
                    EnableText = allowText
                }
            };
            var candidateUser = new CandidateUserBuilder().WithStatus(20).WithCommunicationPreferences(communicationPreferences).Build();

            //Act
            var candidatePerson = _candidateMappers.MapCandidatePerson(candidateUser, new Dictionary<Guid, CandidateSummary>(), new Dictionary<string, int>(), new Dictionary<int, int>(), new Dictionary<int, int>(), false);
            var candidate = candidatePerson.Candidate;

            //Assert
            candidate.AllowMarketingMessages.Should().Be(expected);
        }
    }
}