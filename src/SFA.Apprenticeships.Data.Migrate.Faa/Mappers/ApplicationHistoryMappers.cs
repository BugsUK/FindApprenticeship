namespace SFA.Apprenticeships.Data.Migrate.Faa.Mappers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Entities.Mongo;
    using Entities.Sql;

    public static class ApplicationHistoryMappers
    {
        private const int ApplicationStatusTypeIdSaved = 0;
        private const int ApplicationStatusTypeIdUnsent = 1;
        private const int ApplicationStatusTypeIdSent = 2;
        private const int ApplicationStatusTypeIdInProgress = 3;
        private const int ApplicationStatusTypeIdWithdrawn = 4;
        private const int ApplicationStatusTypeIdUnsuccessful = 5;
        private const int ApplicationStatusTypeIdSuccessful = 6;
        private const int ApplicationStatusTypeIdPastApplication = 7;

        public static IList<ApplicationHistory> MapApplicationHistory(this VacancyApplication vacancyApplication, int applicationId, IDictionary<int, Dictionary<int, int>> applicationHistoryIds, IDictionary<int, List<ApplicationHistorySummary>> sourceApplicationHistorySummaries)
        {
            var applicationHistory = new List<ApplicationHistory>();

            if (vacancyApplication.Status == 5)
            {
                //Saved
                applicationHistory.Add(GetApplicationHistory(applicationId, vacancyApplication.DateCreated, ApplicationStatusTypeIdSaved, applicationHistoryIds, sourceApplicationHistorySummaries));
            }
            if (vacancyApplication.Status >= 10)
            {
                //Draft
                applicationHistory.Add(GetApplicationHistory(applicationId, vacancyApplication.DateCreated, ApplicationStatusTypeIdUnsent, applicationHistoryIds, sourceApplicationHistorySummaries));
            }
            if (vacancyApplication.Status == 15)
            {
                //Withdrawn
                applicationHistory.Add(GetApplicationHistory(applicationId, vacancyApplication.DateUpdated ?? vacancyApplication.DateCreated, ApplicationStatusTypeIdWithdrawn, applicationHistoryIds, sourceApplicationHistorySummaries));
            }
            if (vacancyApplication.Status >= 30)
            {
                //Submitted
                applicationHistory.Add(GetApplicationHistory(applicationId, vacancyApplication.DateApplied ?? vacancyApplication.DateUpdated ?? vacancyApplication.DateCreated, ApplicationStatusTypeIdSent, applicationHistoryIds, sourceApplicationHistorySummaries));
            }
            if (vacancyApplication.Status >= 40)
            {
                //In Progress
                applicationHistory.Add(GetApplicationHistory(applicationId, vacancyApplication.DateUpdated ?? vacancyApplication.DateCreated, ApplicationStatusTypeIdInProgress, applicationHistoryIds, sourceApplicationHistorySummaries));
            }
            if (vacancyApplication.Status == 80)
            {
                //Successful
                applicationHistory.Add(GetApplicationHistory(applicationId, vacancyApplication.SuccessfulDateTime ?? vacancyApplication.DateUpdated ?? vacancyApplication.DateCreated, ApplicationStatusTypeIdSuccessful, applicationHistoryIds, sourceApplicationHistorySummaries));
            }
            if (vacancyApplication.Status == 90)
            {
                //Unsuccessful
                applicationHistory.Add(GetApplicationHistory(applicationId, vacancyApplication.UnsuccessfulDateTime ?? vacancyApplication.DateUpdated ?? vacancyApplication.DateCreated, ApplicationStatusTypeIdUnsuccessful, applicationHistoryIds, sourceApplicationHistorySummaries));
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

        private static ApplicationHistory GetApplicationHistory(int applicationId, DateTime applicationHistoryEventDate, int applicationHistoryEventSubTypeId, IDictionary<int, Dictionary<int, int>> applicationHistoryIds, IDictionary<int, List<ApplicationHistorySummary>> sourceApplicationHistorySummaries)
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
                ApplicationHistoryEventSubTypeId = applicationHistoryEventSubTypeId,
                Comment = "Status Change"
            };
        }

        private static int GetApplicationHistoryId(int applicationId, int applicationHistoryEventSubTypeId, IDictionary<int, Dictionary<int, int>> applicationHistoryIds)
        {
            var applicationHistoryId = 0;

            if (applicationHistoryIds.ContainsKey(applicationId))
            {
                var ids = applicationHistoryIds[applicationId];
                if (ids.ContainsKey(applicationHistoryEventSubTypeId))
                {
                    applicationHistoryId = ids[applicationHistoryEventSubTypeId];
                }
            }

            return applicationHistoryId;
        }

        private static DateTime? GetApplicationHistoryEventDate(int applicationId, int applicationHistoryEventSubTypeId, IDictionary<int, List<ApplicationHistorySummary>> sourceApplicationHistorySummaries)
        {
            if (sourceApplicationHistorySummaries.ContainsKey(applicationId))
            {
                var applicationHistorySummaries = sourceApplicationHistorySummaries[applicationId].OrderByDescending(ahs => ahs.ApplicationHistoryEventDate);
                var applicationHistoryEventDate = applicationHistorySummaries.FirstOrDefault(ahs => ahs.ApplicationHistoryEventSubTypeId == applicationHistoryEventSubTypeId)?.ApplicationHistoryEventDate;
                return applicationHistoryEventDate;
            }

            return null;
        }
    }
}