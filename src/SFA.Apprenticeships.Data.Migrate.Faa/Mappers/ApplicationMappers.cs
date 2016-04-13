namespace SFA.Apprenticeships.Data.Migrate.Faa.Mappers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Entities.Mongo;
    using Entities.Sql;

    public static class ApplicationMappers
    {
        private static int applicationId = -1;

        private static readonly IDictionary<string, int> WithdrawnOrDeclinedReasonIdMap = new Dictionary<string, int>
        {
            {"No longer interested", 1},
            {"Accepted another Apprenticeship offer", 2},
            {"Accepted an alternative job", 3},
            {"Decided to go to college", 4},
            {"Decided to stay on at 6th form", 5},
            {"Want to be able to apply for other Apprenticeships", 6},
            {"Personal reasons", 7},
            {"Moving away", 8},
            {"Pay or Conditions not acceptable", 9},
            {"Other (please specify)", 10}
        };

        private static readonly IDictionary<string, int> UnsuccessfulReasonIdMap = new Dictionary<string, int>
        {
            {"Please Select...", 0},
            {"You’re not eligible for an apprenticeship", 1},
            {"You haven’t met the requirements", 2},
            {"You met the employer’s/provider's requirements but have been unsuccessful", 3},
            {"You didn’t attend the interview", 4},
            {"The apprenticeship has been withdrawn", 5},
            {"You've been unsuccessful - other", 6},
            {"Not suitable for vacancy due to journey / distance involved", 7},
            {"Failed initial assessment test", 8},
            {"Employer withdrew vacancy", 9},
            {"Accepted an alternative apprenticeship position", 10},
            {"Have found other employment", 11},
            {"Taken up other learning or education", 12},
            {"Other reason for Withdrawing Application", 13},
            {"Other", 14},
            {"You’re not eligible for a traineeship", 15},
            {"The training provider couldn’t contact you", 16},
            {"Offered the position but turned it down", 17}
        };

        public static Application ToApplication(this ApprenticeshipApplication apprenticeshipApplication, Candidate candidate)
        {
            var unsuccessfulReasonId = GetUnsuccessfulReasonId(apprenticeshipApplication.UnsuccessfulReason);
            return new Application
            {
                ApplicationId = GetApplicationId(apprenticeshipApplication.LegacyApplicationId),
                CandidateId = GetCandidateId(candidate),
                VacancyId = apprenticeshipApplication.Vacancy.Id,
                ApplicationStatusTypeId = GetApplicationStatusTypeId(apprenticeshipApplication.Status),
                WithdrawnOrDeclinedReasonId = GetWithdrawnOrDeclinedReasonId(apprenticeshipApplication.WithdrawnOrDeclinedReason),
                UnsuccessfulReasonId = unsuccessfulReasonId,
                OutcomeReasonOther = GetOutcomeReasonOther(unsuccessfulReasonId),
                NextActionId = 0,
                NextActionOther = null,
                AllocatedTo = GetAllocatedTo(unsuccessfulReasonId),
                CVAttachmentId = null,
                BeingSupportedBy = null,
                LockedForSupportUntil = null,
                WithdrawalAcknowledged = GetWithdrawalAcknowledged(unsuccessfulReasonId),
                ApplicationGuid = apprenticeshipApplication.Id
            };
        }

        public static dynamic ToApplicationDynamic(this ApprenticeshipApplication apprenticeshipApplication, Candidate candidate)
        {
            var unsuccessfulReasonId = GetUnsuccessfulReasonId(apprenticeshipApplication.UnsuccessfulReason);
            return new
            {
                ApplicationId = GetApplicationId(apprenticeshipApplication.LegacyApplicationId),
                CandidateId = GetCandidateId(candidate),
                VacancyId = apprenticeshipApplication.Vacancy.Id,
                ApplicationStatusTypeId = GetApplicationStatusTypeId(apprenticeshipApplication.Status),
                WithdrawnOrDeclinedReasonId = GetWithdrawnOrDeclinedReasonId(apprenticeshipApplication.WithdrawnOrDeclinedReason),
                UnsuccessfulReasonId = unsuccessfulReasonId,
                OutcomeReasonOther = GetOutcomeReasonOther(unsuccessfulReasonId),
                NextActionId = 0,
                NextActionOther = default(string),
                AllocatedTo = GetAllocatedTo(unsuccessfulReasonId),
                CVAttachmentId = default(int?),
                BeingSupportedBy = default(string),
                LockedForSupportUntil = default(DateTime?),
                WithdrawalAcknowledged = GetWithdrawalAcknowledged(unsuccessfulReasonId),
                ApplicationGuid = apprenticeshipApplication.Id
            };
        }

        public static IDictionary<string, object> ToApplicationDictionary(this ApprenticeshipApplication apprenticeshipApplication, Candidate candidate)
        {
            var unsuccessfulReasonId = GetUnsuccessfulReasonId(apprenticeshipApplication.UnsuccessfulReason);
            return new Dictionary<string, object>
            {
                {"ApplicationId", GetApplicationId(apprenticeshipApplication.LegacyApplicationId)},
                {"CandidateId", GetCandidateId(candidate)},
                {"VacancyId", apprenticeshipApplication.Vacancy.Id},
                {"ApplicationStatusTypeId", GetApplicationStatusTypeId(apprenticeshipApplication.Status)},
                {"WithdrawnOrDeclinedReasonId", GetWithdrawnOrDeclinedReasonId(apprenticeshipApplication.WithdrawnOrDeclinedReason)},
                {"UnsuccessfulReasonId", unsuccessfulReasonId},
                {"OutcomeReasonOther", GetOutcomeReasonOther(unsuccessfulReasonId)},
                {"NextActionId", 0},
                {"NextActionOther", null},
                {"AllocatedTo", GetAllocatedTo(unsuccessfulReasonId)},
                {"CVAttachmentId", null},
                {"BeingSupportedBy", null},
                {"LockedForSupportUntil", null},
                {"WithdrawalAcknowledged", GetWithdrawalAcknowledged(unsuccessfulReasonId)},
                {"ApplicationGuid", apprenticeshipApplication.Id}
            };
        }

        private static int GetApplicationId(int legacyApplicationId)
        {
            if (legacyApplicationId == 0)
            {
                return applicationId--;
            }

            return legacyApplicationId;
        }

        private static int GetCandidateId(Candidate candidate)
        {
            return candidate?.LegacyCandidateId ?? 0;
        }

        private static int GetApplicationStatusTypeId(int status)
        {
            switch (status)
            {
                case 10:
                case 20:
                    return 1;
                case 15:
                    return 4;
                case 30:
                    return 2;
                case 40:
                    return 3;
                case 80:
                    return 5;
                case 90:
                    return 6;
                default: throw new ArgumentException($"Status value {status} was not recognised.");
            }
        }

        private static int GetWithdrawnOrDeclinedReasonId(string withdrawnOrDeclinedReason)
        {
            if (string.IsNullOrEmpty(withdrawnOrDeclinedReason))
            {
                return 0;
            }

            var map = UnsuccessfulReasonIdMap.SingleOrDefault(kvp => kvp.Key.StartsWith(withdrawnOrDeclinedReason));

            return map.Equals(default(KeyValuePair<string, int>)) ? 0 : map.Value;
        }

        private static int GetUnsuccessfulReasonId(string unsuccessfulReason)
        {
            if (string.IsNullOrEmpty(unsuccessfulReason))
            {
                return 0;
            }

            if (WithdrawnOrDeclinedReasonIdMap.ContainsKey(unsuccessfulReason))
            {
                return WithdrawnOrDeclinedReasonIdMap[unsuccessfulReason];
            }

            return 0;
        }

        private static string GetOutcomeReasonOther(int unsuccessfulReasonId)
        {
            return unsuccessfulReasonId == 0 ? null : "";
        }

        private static string GetAllocatedTo(int unsuccessfulReasonId)
        {
            return unsuccessfulReasonId == 0 || unsuccessfulReasonId == 13 ? null : "";
        }

        private static bool? GetWithdrawalAcknowledged(int unsuccessfulReasonId)
        {
            return unsuccessfulReasonId != 10 && unsuccessfulReasonId != 11 && unsuccessfulReasonId != 13;
        }
    }
}
 