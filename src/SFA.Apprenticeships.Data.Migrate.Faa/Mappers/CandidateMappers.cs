namespace SFA.Apprenticeships.Data.Migrate.Faa.Mappers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using Entities;
    using Entities.Mongo;
    using Entities.Sql;
    using Infrastructure.Repositories.Sql.Common;
    using Candidate = Entities.Sql.Candidate;

    public class CandidateMappers : ICandidateMappers
    {
        private const int CandidateStatusTypeIdPreRegistered = 1;
        private const int CandidateStatusTypeIdActivated = 2;
        private const int CandidateStatusTypeIdRegistered = 3;
        private const int CandidateStatusTypeIdSuspended = 4;
        private const int CandidateStatusTypeIdPendingDelete = 5;
        private const int CandidateStatusTypeIdDeleted = 6;

        private int _candidateId;

        public CandidateMappers(IGetOpenConnection targetDatabase)
        {
            _candidateId = targetDatabase.Query<int>($"SELECT ISNULL(min(CandidateId), 0) FROM Candidate").Single() - 1;
            if (_candidateId > -1)
            {
                _candidateId = -1;
            }
        }

        public CandidateMappers(int candidateId)
        {
            _candidateId = candidateId;
        }

        public CandidatePerson MapCandidatePerson(CandidateUser candidateUser, IDictionary<string, int> persons)
        {
            var address = candidateUser.Candidate.RegistrationDetails.Address;
            var town = address.AddressLine4 ?? address.AddressLine3 ?? address.AddressLine2;

            var candidate = new Candidate
            {
                CandidateId = GetCandidateId(candidateUser.Candidate.LegacyCandidateId),
                PersonId = 0, //Unspecified person. Filled in later
                CandidateStatusTypeId = CandidateStatusTypeIdPreRegistered,
                DateofBirth = candidateUser.Candidate.RegistrationDetails.DateOfBirth,
                AddressLine1 = address.AddressLine1,
                AddressLine2 = address.AddressLine2 ?? "",
                AddressLine3 = address.AddressLine3 ?? "",
                AddressLine4 = "",
                AddressLine5 = null,
                Town = town,
                //CountyId = candidateUser.Candidate.,
                Postcode = address.Postcode,
                /*LocalAuthorityId = candidateUser.Candidate.,
                Longitude = candidateUser.Candidate.,
                Latitude = candidateUser.Candidate.,
                GeocodeEasting = candidateUser.Candidate.,
                GeocodeNorthing = candidateUser.Candidate.,
                NiReference = candidateUser.Candidate.,
                VoucherReferenceNumber = candidateUser.Candidate.,
                UniqueLearnerNumber = candidateUser.Candidate.,
                UlnStatusId = candidateUser.Candidate.,
                Gender = candidateUser.Candidate.,
                EthnicOrigin = candidateUser.Candidate.,
                EthnicOriginOther = candidateUser.Candidate.,
                ApplicationLimitEnforced = candidateUser.Candidate.,
                LastAccessedDate = candidateUser.Candidate.,
                AdditionalEmail = candidateUser.Candidate.,
                Disability = candidateUser.Candidate.,
                DisabilityOther = candidateUser.Candidate.,
                HealthProblems = candidateUser.Candidate.,
                ReceivePushedContent = candidateUser.Candidate.,
                ReferralAgent = candidateUser.Candidate.,
                DisableAlerts = candidateUser.Candidate.,
                UnconfirmedEmailAddress = candidateUser.Candidate.,
                MobileNumberUnconfirmed = candidateUser.Candidate.,
                DoBFailureCount = candidateUser.Candidate.,
                ForgottenUsernameRequested = candidateUser.Candidate.,
                ForgottenPasswordRequested = candidateUser.Candidate.,
                TextFailureCount = candidateUser.Candidate.,
                EmailFailureCount = candidateUser.Candidate.,
                LastAccessedManageApplications = candidateUser.Candidate.,
                ReferralPoints = candidateUser.Candidate.,
                BeingSupportedBy = candidateUser.Candidate.,
                LockedForSupportUntil = candidateUser.Candidate.,
                NewVacancyAlertEmail = candidateUser.Candidate.,
                NewVacancyAlertSMS = candidateUser.Candidate.,
                AllowMarketingMessages = candidateUser.Candidate.,
                ReminderMessageSent = candidateUser.Candidate.,*/
                CandidateGuid = candidateUser.Candidate.Id
            };

            var person = new Person
            {
                Title = 0,
                OtherTitle = "",
                FirstName = candidateUser.Candidate.RegistrationDetails.FirstName,
                MiddleNames = candidateUser.Candidate.RegistrationDetails.MiddleNames,
                Surname = candidateUser.Candidate.RegistrationDetails.LastName,
                LandlineNumber = candidateUser.Candidate.RegistrationDetails.PhoneNumber,
                MobileNumber = "",
                Email = candidateUser.Candidate.RegistrationDetails.EmailAddress.ToLower(),
                PersonTypeId = 1
            };

            return new CandidatePerson
            {
                Candidate = candidate,
                Person = person
            };
        }

        public CandidatePersonDictionary MapCandidatePersonDictionary(CandidateUser candidateUser, IDictionary<string, int> persons)
        {
            var candidatePerson = MapCandidatePerson(candidateUser, persons);

            var candidateDictionary = new Dictionary<string, object>
            {
                {"CandidateId", candidatePerson.Candidate.CandidateId},
                {"PersonId", candidatePerson.Candidate.PersonId},
                {"CandidateStatusTypeId", candidatePerson.Candidate.CandidateStatusTypeId},
                {"DateofBirth", candidatePerson.Candidate.DateofBirth},
                {"AddressLine1", candidatePerson.Candidate.AddressLine1},
                {"AddressLine2", candidatePerson.Candidate.AddressLine2},
                {"AddressLine3", candidatePerson.Candidate.AddressLine3},
                {"AddressLine4", candidatePerson.Candidate.AddressLine4},
                {"AddressLine5", candidatePerson.Candidate.AddressLine5},
                {"Town", candidatePerson.Candidate.Town},
                {"CountyId", candidatePerson.Candidate.CountyId},
                {"Postcode", candidatePerson.Candidate.Postcode},
                {"LocalAuthorityId", candidatePerson.Candidate.LocalAuthorityId},
                {"Longitude", candidatePerson.Candidate.Longitude},
                {"Latitude", candidatePerson.Candidate.Latitude},
                {"GeocodeEasting", candidatePerson.Candidate.GeocodeEasting},
                {"GeocodeNorthing", candidatePerson.Candidate.GeocodeNorthing},
                {"NiReference", candidatePerson.Candidate.NiReference},
                {"VoucherReferenceNumber", candidatePerson.Candidate.VoucherReferenceNumber},
                {"UniqueLearnerNumber", candidatePerson.Candidate.UniqueLearnerNumber},
                {"UlnStatusId", candidatePerson.Candidate.UlnStatusId},
                {"Gender", candidatePerson.Candidate.Gender},
                {"EthnicOrigin", candidatePerson.Candidate.EthnicOrigin},
                {"EthnicOriginOther", candidatePerson.Candidate.EthnicOriginOther},
                {"ApplicationLimitEnforced", candidatePerson.Candidate.ApplicationLimitEnforced},
                {"LastAccessedDate", candidatePerson.Candidate.LastAccessedDate},
                {"AdditionalEmail", candidatePerson.Candidate.AdditionalEmail},
                {"Disability", candidatePerson.Candidate.Disability},
                {"DisabilityOther", candidatePerson.Candidate.DisabilityOther},
                {"HealthProblems", candidatePerson.Candidate.HealthProblems},
                {"ReceivePushedContent", candidatePerson.Candidate.ReceivePushedContent},
                {"ReferralAgent", candidatePerson.Candidate.ReferralAgent},
                {"DisableAlerts", candidatePerson.Candidate.DisableAlerts},
                {"UnconfirmedEmailAddress", candidatePerson.Candidate.UnconfirmedEmailAddress},
                {"MobileNumberUnconfirmed", candidatePerson.Candidate.MobileNumberUnconfirmed},
                {"DoBFailureCount", candidatePerson.Candidate.DoBFailureCount},
                {"ForgottenUsernameRequested", candidatePerson.Candidate.ForgottenUsernameRequested},
                {"ForgottenPasswordRequested", candidatePerson.Candidate.ForgottenPasswordRequested},
                {"TextFailureCount", candidatePerson.Candidate.TextFailureCount},
                {"EmailFailureCount", candidatePerson.Candidate.EmailFailureCount},
                {"LastAccessedManageApplications", candidatePerson.Candidate.LastAccessedManageApplications},
                {"ReferralPoints", candidatePerson.Candidate.ReferralPoints},
                {"BeingSupportedBy", candidatePerson.Candidate.BeingSupportedBy},
                {"LockedForSupportUntil", candidatePerson.Candidate.LockedForSupportUntil},
                {"NewVacancyAlertEmail", candidatePerson.Candidate.NewVacancyAlertEmail},
                {"NewVacancyAlertSMS", candidatePerson.Candidate.NewVacancyAlertSMS},
                {"AllowMarketingMessages", candidatePerson.Candidate.AllowMarketingMessages},
                {"ReminderMessageSent", candidatePerson.Candidate.ReminderMessageSent},
                {"CandidateGuid", candidatePerson.Candidate.CandidateGuid}
            };

            var personDictionary = new Dictionary<string, object>
            {
                {"Title", candidatePerson.Person.Title},
                {"OtherTitle", candidatePerson.Person.OtherTitle},
                {"FirstName", candidatePerson.Person.FirstName},
                {"MiddleNames", candidatePerson.Person.MiddleNames},
                {"Surname", candidatePerson.Person.Surname},
                {"LandlineNumber", candidatePerson.Person.LandlineNumber},
                {"MobileNumber", candidatePerson.Person.MobileNumber},
                {"Email", candidatePerson.Person.Email},
                {"PersonTypeId", candidatePerson.Person.PersonTypeId}
            };

            return new CandidatePersonDictionary
            {
                Candidate = candidateDictionary,
                Person = personDictionary
            };
        }

        private int GetCandidateId(int legacyCandidateId)
        {
            if (legacyCandidateId == 0)
            {
                var candidateId = _candidateId;
                Interlocked.Decrement(ref _candidateId);
                return candidateId;
            }

            return legacyCandidateId;
        }
    }
}