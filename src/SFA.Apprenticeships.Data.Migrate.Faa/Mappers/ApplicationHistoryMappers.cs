namespace SFA.Apprenticeships.Data.Migrate.Faa.Mappers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Entities.Mongo;
    using Entities.Sql;

    public static class ApplicationHistoryMappers
    {
        public static IList<ApplicationHistory> MapApplicationHistory(this VacancyApplication vacancyApplication, int applicationId, IDictionary<int, Dictionary<int, int>> applicationHistoryIds, IDictionary<int, List<ApplicationHistorySummary>> sourceApplicationHistorySummaries)
        {
            var applicationHistory = new List<ApplicationHistory>();

            if (vacancyApplication.Status == ApplicationStatuses.Saved)
            {
                //Saved
                applicationHistory.Add(GetApplicationHistory(applicationId, vacancyApplication.DateCreated, ApplicationStatusTypeIds.ApplicationStatusTypeIdSaved, applicationHistoryIds, sourceApplicationHistorySummaries));
            }
            if (vacancyApplication.Status >= ApplicationStatuses.Draft)
            {
                //Draft
                applicationHistory.Add(GetApplicationHistory(applicationId, vacancyApplication.DateCreated, ApplicationStatusTypeIds.ApplicationStatusTypeIdUnsent, applicationHistoryIds, sourceApplicationHistorySummaries));
            }
            if (vacancyApplication.Status == ApplicationStatuses.ExpiredOrWithdrawn)
            {
                //Withdrawn
                applicationHistory.Add(GetApplicationHistory(applicationId, vacancyApplication.DateUpdated ?? vacancyApplication.DateCreated, ApplicationStatusTypeIds.ApplicationStatusTypeIdWithdrawn, applicationHistoryIds, sourceApplicationHistorySummaries));
            }
            if (vacancyApplication.Status >= ApplicationStatuses.Submitted)
            {
                //Submitted
                applicationHistory.Add(GetApplicationHistory(applicationId, vacancyApplication.DateApplied ?? vacancyApplication.DateCreated, ApplicationStatusTypeIds.ApplicationStatusTypeIdSent, applicationHistoryIds, sourceApplicationHistorySummaries));
            }
            if (vacancyApplication.Status >= ApplicationStatuses.InProgress)
            {
                //In Progress
                applicationHistory.Add(GetApplicationHistory(applicationId, vacancyApplication.DateUpdated ?? vacancyApplication.DateCreated, ApplicationStatusTypeIds.ApplicationStatusTypeIdInProgress, applicationHistoryIds, sourceApplicationHistorySummaries));
            }
            if (vacancyApplication.Status == ApplicationStatuses.Successful)
            {
                //Successful
                applicationHistory.Add(GetApplicationHistory(applicationId, vacancyApplication.SuccessfulDateTime ?? vacancyApplication.DateUpdated ?? vacancyApplication.DateCreated, ApplicationStatusTypeIds.ApplicationStatusTypeIdSuccessful, applicationHistoryIds, sourceApplicationHistorySummaries));
            }
            if (vacancyApplication.Status == ApplicationStatuses.Unsuccessful)
            {
                //Unsuccessful
                applicationHistory.Add(GetApplicationHistory(applicationId, vacancyApplication.UnsuccessfulDateTime ?? vacancyApplication.DateUpdated ?? vacancyApplication.DateCreated, ApplicationStatusTypeIds.ApplicationStatusTypeIdUnsuccessful, applicationHistoryIds, sourceApplicationHistorySummaries));
            }

            return applicationHistory;
        }

        public static IList<IDictionary<string, object>> MapApplicationHistoryDictionary(this IList<ApplicationHistory> applicationHistories)
        {
            return applicationHistories.Select(ah => ah.MapApplicationHistoryDictionary()).ToList();
        }

        public static IDictionary<string, object> MapApplicationHistoryDictionary(this ApplicationHistory applicationHistory)
        {
            return new Dictionary<string, object>
            {
                {"ApplicationHistoryId", applicationHistory.ApplicationHistoryId},
                {"ApplicationId", applicationHistory.ApplicationId},
                {"UserName", applicationHistory.UserName},
                {"ApplicationHistoryEventDate", applicationHistory.ApplicationHistoryEventDate},
                {"ApplicationHistoryEventTypeId", applicationHistory.ApplicationHistoryEventTypeId},
                {"ApplicationHistoryEventSubTypeId", applicationHistory.ApplicationHistoryEventSubTypeId},
                {"Comment", applicationHistory.Comment},
            };
        }

        private static ApplicationHistory GetApplicationHistory(int applicationId, DateTime applicationHistoryEventDate, ApplicationStatusTypeIds applicationHistoryEventSubTypeId, IDictionary<int, Dictionary<int, int>> applicationHistoryIds, IDictionary<int, List<ApplicationHistorySummary>> sourceApplicationHistorySummaries)
        {
            var applicationHistoryId = GetApplicationHistoryId(applicationId, applicationHistoryEventSubTypeId, applicationHistoryIds);

            applicationHistoryEventDate = GetApplicationHistoryEventDate(applicationId, applicationHistoryEventSubTypeId, sourceApplicationHistorySummaries) ?? applicationHistoryEventDate;

            return new ApplicationHistory
            {
                ApplicationHistoryId = applicationHistoryId,
                ApplicationId = applicationId,
                UserName = "",
                ApplicationHistoryEventDate = applicationHistoryEventDate,
                ApplicationHistoryEventTypeId = 1,
                ApplicationHistoryEventSubTypeId = (int)applicationHistoryEventSubTypeId,
                Comment = "Status Change"
            };
        }

        private static int GetApplicationHistoryId(int applicationId, ApplicationStatusTypeIds applicationHistoryEventSubTypeId, IDictionary<int, Dictionary<int, int>> applicationHistoryIds)
        {
            var applicationHistoryId = 0;

            if (applicationHistoryIds.ContainsKey(applicationId))
            {
                var ids = applicationHistoryIds[applicationId];
                if (ids.ContainsKey((int)applicationHistoryEventSubTypeId))
                {
                    applicationHistoryId = ids[(int)applicationHistoryEventSubTypeId];
                }
            }

            return applicationHistoryId;
        }

        private static DateTime? GetApplicationHistoryEventDate(int applicationId, ApplicationStatusTypeIds applicationHistoryEventSubTypeId, IDictionary<int, List<ApplicationHistorySummary>> sourceApplicationHistorySummaries)
        {
            if (sourceApplicationHistorySummaries.ContainsKey(applicationId))
            {
                var applicationHistorySummaries = sourceApplicationHistorySummaries[applicationId].OrderByDescending(ahs => ahs.ApplicationHistoryEventDate);
                var applicationHistoryEventDate = applicationHistorySummaries.FirstOrDefault(ahs => ahs.ApplicationHistoryEventSubTypeId == (int)applicationHistoryEventSubTypeId)?.ApplicationHistoryEventDate;
                return applicationHistoryEventDate;
            }

            return null;
        }
    }
}