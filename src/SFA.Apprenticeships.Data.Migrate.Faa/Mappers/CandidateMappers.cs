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
            return new Candidate
            {
                CandidateId = candidateUser.Candidate.LegacyCandidateId,
                PersonId = -1, //Unspecified person
                CandidateStatusTypeId = CandidateStatusTypeIdPreRegistered,
                /*DateofBirth = candidateUser.Candidate.,
                AddressLine1 = candidateUser.Candidate.,
                AddressLine2 = candidateUser.Candidate.,
                AddressLine3 = candidateUser.Candidate.,
                AddressLine4 = candidateUser.Candidate.,
                AddressLine5 = candidateUser.Candidate.,
                Town = candidateUser.Candidate.,
                CountyId = candidateUser.Candidate.,
                Postcode = candidateUser.Candidate.,
                LocalAuthorityId = candidateUser.Candidate.,
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
            throw new System.NotImplementedException();
        }
    }
}