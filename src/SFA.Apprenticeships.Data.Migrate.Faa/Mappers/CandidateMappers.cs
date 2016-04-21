namespace SFA.Apprenticeships.Data.Migrate.Faa.Mappers
{
    using System.Collections.Generic;
    using Entities.Mongo;
    using Candidate = Entities.Sql.Candidate;

    public class CandidateMappers : ICandidateMappers
    {
        private const int CandidateStatusTypeIdPreRegistered = 1;
        private const int CandidateStatusTypeIdActivated = 2;
        private const int CandidateStatusTypeIdRegistered = 3;
        private const int CandidateStatusTypeIdSuspended = 4;
        private const int CandidateStatusTypeIdPendingDelete = 5;
        private const int CandidateStatusTypeIdDeleted = 6;

        public Candidate MapCandidate(CandidateUser candidateUser)
        {
            var address = candidateUser.Candidate.RegistrationDetails.Address;
            var town = address.AddressLine4 ?? address.AddressLine3 ?? address.AddressLine2;

            return new Candidate
            {
                CandidateId = candidateUser.Candidate.LegacyCandidateId,
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
        }

        public IDictionary<string, object> MapCandidateDictionary(CandidateUser candidateUser)
        {
            var candidate = MapCandidate(candidateUser);

            return new Dictionary<string, object>
            {
                {"CandidateId", candidate.CandidateId},
                {"PersonId", candidate.PersonId},
                {"CandidateStatusTypeId", candidate.CandidateStatusTypeId},
                {"DateofBirth", candidate.DateofBirth},
                {"AddressLine1", candidate.AddressLine1},
                {"AddressLine2", candidate.AddressLine2},
                {"AddressLine3", candidate.AddressLine3},
                {"AddressLine4", candidate.AddressLine4},
                {"AddressLine5", candidate.AddressLine5},
                {"Town", candidate.Town},
                {"CountyId", candidate.CountyId},
                {"Postcode", candidate.Postcode},
                {"LocalAuthorityId", candidate.LocalAuthorityId},
                {"Longitude", candidate.Longitude},
                {"Latitude", candidate.Latitude},
                {"GeocodeEasting", candidate.GeocodeEasting},
                {"GeocodeNorthing", candidate.GeocodeNorthing},
                {"NiReference", candidate.NiReference},
                {"VoucherReferenceNumber", candidate.VoucherReferenceNumber},
                {"UniqueLearnerNumber", candidate.UniqueLearnerNumber},
                {"UlnStatusId", candidate.UlnStatusId},
                {"Gender", candidate.Gender},
                {"EthnicOrigin", candidate.EthnicOrigin},
                {"EthnicOriginOther", candidate.EthnicOriginOther},
                {"ApplicationLimitEnforced", candidate.ApplicationLimitEnforced},
                {"LastAccessedDate", candidate.LastAccessedDate},
                {"AdditionalEmail", candidate.AdditionalEmail},
                {"Disability", candidate.Disability},
                {"DisabilityOther", candidate.DisabilityOther},
                {"HealthProblems", candidate.HealthProblems},
                {"ReceivePushedContent", candidate.ReceivePushedContent},
                {"ReferralAgent", candidate.ReferralAgent},
                {"DisableAlerts", candidate.DisableAlerts},
                {"UnconfirmedEmailAddress", candidate.UnconfirmedEmailAddress},
                {"MobileNumberUnconfirmed", candidate.MobileNumberUnconfirmed},
                {"DoBFailureCount", candidate.DoBFailureCount},
                {"ForgottenUsernameRequested", candidate.ForgottenUsernameRequested},
                {"ForgottenPasswordRequested", candidate.ForgottenPasswordRequested},
                {"TextFailureCount", candidate.TextFailureCount},
                {"EmailFailureCount", candidate.EmailFailureCount},
                {"LastAccessedManageApplications", candidate.LastAccessedManageApplications},
                {"ReferralPoints", candidate.ReferralPoints},
                {"BeingSupportedBy", candidate.BeingSupportedBy},
                {"LockedForSupportUntil", candidate.LockedForSupportUntil},
                {"NewVacancyAlertEmail", candidate.NewVacancyAlertEmail},
                {"NewVacancyAlertSMS", candidate.NewVacancyAlertSMS},
                {"AllowMarketingMessages", candidate.AllowMarketingMessages},
                {"ReminderMessageSent", candidate.ReminderMessageSent},
                {"CandidateGuid", candidate.CandidateGuid}
            };
        }
    }
}