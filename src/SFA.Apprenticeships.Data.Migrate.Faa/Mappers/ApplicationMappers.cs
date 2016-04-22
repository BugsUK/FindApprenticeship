namespace SFA.Apprenticeships.Data.Migrate.Faa.Mappers
{
    using System;
    using System.Collections.Generic;
    using Entities;
    using Entities.Mongo;
    using Entities.Sql;
    using SFA.Infrastructure.Interfaces;

    public class ApplicationMappers : IApplicationMappers
    {
        private readonly ILogService _logService;

        public ApplicationMappers(ILogService logService)
        {
            _logService = logService;
        }

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

        public Application MapApplication(VacancyApplication apprenticeshipApplication, int candidateId, IDictionary<Guid, int> applicationIds)
        {
            var unsuccessfulReasonId = GetUnsuccessfulReasonId(apprenticeshipApplication.UnsuccessfulReason);
            return new Application
            {
                ApplicationId = GetApplicationId(apprenticeshipApplication, applicationIds),
                CandidateId = candidateId,
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
                ApplicationGuid = apprenticeshipApplication.Id,
            };
        }

        public ApplicationWithHistory MapApplicationWithHistory(VacancyApplication apprenticeshipApplication, int candidateId, IDictionary<Guid, int> applicationIds, IDictionary<int, Dictionary<int, int>> applicationHistoryIds)
        {
            var application = MapApplication(apprenticeshipApplication, candidateId, applicationIds);
            var applicationHistory = apprenticeshipApplication.MapApplicationHistory(application.ApplicationId, applicationHistoryIds);
            return new ApplicationWithHistory {Application = application, ApplicationHistory = applicationHistory};
        }

        public IDictionary<string, object> MapApplicationDictionary(Application application)
        {
            return new Dictionary<string, object>
            {
                {"ApplicationId", application.ApplicationId},
                {"CandidateId", application.CandidateId},
                {"VacancyId", application.VacancyId},
                {"ApplicationStatusTypeId", application.ApplicationStatusTypeId},
                {"WithdrawnOrDeclinedReasonId", application.WithdrawnOrDeclinedReasonId},
                {"UnsuccessfulReasonId", application.UnsuccessfulReasonId},
                {"OutcomeReasonOther", application.OutcomeReasonOther},
                {"NextActionId", application.NextActionId},
                {"NextActionOther", application.NextActionOther},
                {"AllocatedTo", application.AllocatedTo},
                {"CVAttachmentId", application.CVAttachmentId},
                {"BeingSupportedBy", application.BeingSupportedBy},
                {"LockedForSupportUntil", application.LockedForSupportUntil},
                {"WithdrawalAcknowledged", application.WithdrawalAcknowledged},
                {"ApplicationGuid", application.ApplicationGuid}
            };
        }

        private int GetApplicationId(VacancyApplication apprenticeshipApplication, IDictionary<Guid, int> applicationIds)
        {
            if (applicationIds.ContainsKey(apprenticeshipApplication.Id))
            {
                var applicationId = applicationIds[apprenticeshipApplication.Id];
                if (apprenticeshipApplication.LegacyApplicationId != 0 && apprenticeshipApplication.LegacyApplicationId != applicationId)
                {
                    _logService.Warn($"ApplicationId: {applicationId} does not match the LegacyApplicationId: {apprenticeshipApplication.LegacyApplicationId} for application with Id: {apprenticeshipApplication.Id}. This shouldn't change post submission");
                }
                return applicationId;
            }

            return apprenticeshipApplication.LegacyApplicationId;
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
                    return 6;
                case 90:
                    return 5;
                default: throw new ArgumentException($"Status value {status} was not recognised.");
            }
        }

        private static int GetWithdrawnOrDeclinedReasonId(string withdrawnOrDeclinedReason)
        {
            if (string.IsNullOrEmpty(withdrawnOrDeclinedReason))
            {
                return 0;
            }

            if (WithdrawnOrDeclinedReasonIdMap.ContainsKey(withdrawnOrDeclinedReason))
            {
                return WithdrawnOrDeclinedReasonIdMap[withdrawnOrDeclinedReason];
            }

            return 0;
        }

        private static int GetUnsuccessfulReasonId(string unsuccessfulReason)
        {
            if (string.IsNullOrEmpty(unsuccessfulReason))
            {
                return 0;
            }

            if (UnsuccessfulReasonIdMap.ContainsKey(unsuccessfulReason))
            {
                return UnsuccessfulReasonIdMap[unsuccessfulReason];
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
 