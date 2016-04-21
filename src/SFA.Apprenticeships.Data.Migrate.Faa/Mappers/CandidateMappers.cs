namespace SFA.Apprenticeships.Data.Migrate.Faa.Mappers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using Entities;
    using Entities.Mongo;
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

        public CandidatePerson MapCandidatePerson(CandidateUser candidateUser)
        {
            var address = candidateUser.Candidate.RegistrationDetails.Address;
            var town = address.AddressLine4 ?? address.AddressLine3 ?? address.AddressLine2;

            var candidate = new Candidate
            {
                CandidateId = GetCandidateId(candidateUser.Candidate.LegacyCandidateId),
                PersonId = -1, //Unspecified person
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

            return new CandidatePerson
            {
                Candidate = candidate
            };
        }

        public CandidatePersonDictionary MapCandidatePersonDictionary(CandidateUser candidateUser)
        {
            var candidate = MapCandidatePerson(candidateUser);

            var candidateDictionary = new Dictionary<string, object>
            {
                {"CandidateId", candidate.Candidate.CandidateId},
                {"PersonId", candidate.Candidate.PersonId},
                {"CandidateStatusTypeId", candidate.Candidate.CandidateStatusTypeId},
                {"DateofBirth", candidate.Candidate.DateofBirth},
                {"AddressLine1", candidate.Candidate.AddressLine1},
                {"AddressLine2", candidate.Candidate.AddressLine2},
                {"AddressLine3", candidate.Candidate.AddressLine3},
                {"AddressLine4", candidate.Candidate.AddressLine4},
                {"AddressLine5", candidate.Candidate.AddressLine5},
                {"Town", candidate.Candidate.Town},
                {"CountyId", candidate.Candidate.CountyId},
                {"Postcode", candidate.Candidate.Postcode},
                {"LocalAuthorityId", candidate.Candidate.LocalAuthorityId},
                {"Longitude", candidate.Candidate.Longitude},
                {"Latitude", candidate.Candidate.Latitude},
                {"GeocodeEasting", candidate.Candidate.GeocodeEasting},
                {"GeocodeNorthing", candidate.Candidate.GeocodeNorthing},
                {"NiReference", candidate.Candidate.NiReference},
                {"VoucherReferenceNumber", candidate.Candidate.VoucherReferenceNumber},
                {"UniqueLearnerNumber", candidate.Candidate.UniqueLearnerNumber},
                {"UlnStatusId", candidate.Candidate.UlnStatusId},
                {"Gender", candidate.Candidate.Gender},
                {"EthnicOrigin", candidate.Candidate.EthnicOrigin},
                {"EthnicOriginOther", candidate.Candidate.EthnicOriginOther},
                {"ApplicationLimitEnforced", candidate.Candidate.ApplicationLimitEnforced},
                {"LastAccessedDate", candidate.Candidate.LastAccessedDate},
                {"AdditionalEmail", candidate.Candidate.AdditionalEmail},
                {"Disability", candidate.Candidate.Disability},
                {"DisabilityOther", candidate.Candidate.DisabilityOther},
                {"HealthProblems", candidate.Candidate.HealthProblems},
                {"ReceivePushedContent", candidate.Candidate.ReceivePushedContent},
                {"ReferralAgent", candidate.Candidate.ReferralAgent},
                {"DisableAlerts", candidate.Candidate.DisableAlerts},
                {"UnconfirmedEmailAddress", candidate.Candidate.UnconfirmedEmailAddress},
                {"MobileNumberUnconfirmed", candidate.Candidate.MobileNumberUnconfirmed},
                {"DoBFailureCount", candidate.Candidate.DoBFailureCount},
                {"ForgottenUsernameRequested", candidate.Candidate.ForgottenUsernameRequested},
                {"ForgottenPasswordRequested", candidate.Candidate.ForgottenPasswordRequested},
                {"TextFailureCount", candidate.Candidate.TextFailureCount},
                {"EmailFailureCount", candidate.Candidate.EmailFailureCount},
                {"LastAccessedManageApplications", candidate.Candidate.LastAccessedManageApplications},
                {"ReferralPoints", candidate.Candidate.ReferralPoints},
                {"BeingSupportedBy", candidate.Candidate.BeingSupportedBy},
                {"LockedForSupportUntil", candidate.Candidate.LockedForSupportUntil},
                {"NewVacancyAlertEmail", candidate.Candidate.NewVacancyAlertEmail},
                {"NewVacancyAlertSMS", candidate.Candidate.NewVacancyAlertSMS},
                {"AllowMarketingMessages", candidate.Candidate.AllowMarketingMessages},
                {"ReminderMessageSent", candidate.Candidate.ReminderMessageSent},
                {"CandidateGuid", candidate.Candidate.CandidateGuid}
            };

            return new CandidatePersonDictionary
            {
                Candidate = candidateDictionary
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