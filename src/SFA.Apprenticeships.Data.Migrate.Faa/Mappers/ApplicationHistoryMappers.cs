namespace SFA.Apprenticeships.Data.Migrate.Faa.Mappers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Entities.Mongo;
    using Entities.Sql;

    public static class ApplicationHistoryMappers
    {
        private const int ApplicationStatusTypeIdUnsent = 1;
        private const int ApplicationStatusTypeIdSent = 2;
        private const int ApplicationStatusTypeIdInProgress = 3;
        private const int ApplicationStatusTypeIdWithdrawn = 4;
        private const int ApplicationStatusTypeIdUnsuccessful = 5;
        private const int ApplicationStatusTypeIdSuccessful = 6;
        private const int ApplicationStatusTypeIdPastApplication = 7;

        public static IList<ApplicationHistory> MapApplicationHistory(this VacancyApplication vacancyApplication, int applicationId)
        {
            var applicationHistory = new List<ApplicationHistory>
            {
                //Draft
                GetApplicationHistory(applicationId, vacancyApplication.DateCreated, ApplicationStatusTypeIdUnsent)
            };

            if (vacancyApplication.Status == 15)
            {
                //Withdrawn
                applicationHistory.Add(GetApplicationHistory(applicationId, vacancyApplication.DateUpdated ?? vacancyApplication.DateCreated, ApplicationStatusTypeIdWithdrawn));
            }
            if (vacancyApplication.Status >= 30)
            {
                //Submitted
                applicationHistory.Add(GetApplicationHistory(applicationId, vacancyApplication.DateUpdated ?? vacancyApplication.DateCreated, ApplicationStatusTypeIdSent));
            }
            if (vacancyApplication.Status >= 40)
            {
                //In Progress
                applicationHistory.Add(GetApplicationHistory(applicationId, vacancyApplication.DateUpdated ?? vacancyApplication.DateCreated, ApplicationStatusTypeIdInProgress));
            }
            if (vacancyApplication.Status == 80)
            {
                //Unsuccessful
                applicationHistory.Add(GetApplicationHistory(applicationId, vacancyApplication.DateUpdated ?? vacancyApplication.DateCreated, ApplicationStatusTypeIdSuccessful));
            }
            if (vacancyApplication.Status == 90)
            {
                //Unsuccessful
                applicationHistory.Add(GetApplicationHistory(applicationId, vacancyApplication.DateUpdated ?? vacancyApplication.DateCreated, ApplicationStatusTypeIdUnsuccessful));
            }

            return applicationHistory;
        }

        public static IList<IDictionary<string, object>> MapApplicationHistoryDictionary(this VacancyApplication vacancyApplication, int applicationId)
        {
            return vacancyApplication.MapApplicationHistory(applicationId).Select(ah => ah.MapApplicationHistoryDictionary()).ToList();
        }

        private static IDictionary<string, object> MapApplicationHistoryDictionary(this ApplicationHistory applicationHistory)
        {
            return new Dictionary<string, object>
            {
                {"ApplicationId", applicationHistory.ApplicationId},
                {"UserName", applicationHistory.UserName},
                {"ApplicationHistoryEventDate", applicationHistory.ApplicationHistoryEventDate},
                {"ApplicationHistoryEventTypeId", applicationHistory.ApplicationHistoryEventTypeId},
                {"ApplicationHistoryEventSubTypeId", applicationHistory.ApplicationHistoryEventSubTypeId},
                {"Comment", applicationHistory.Comment},
            };
        }

        private static ApplicationHistory GetApplicationHistory(int applicationId, DateTime applicationHistoryEventDate, int applicationHistoryEventSubTypeId)
        {
            return new ApplicationHistory
            {
                ApplicationId = applicationId,
                UserName = "",
                ApplicationHistoryEventDate = applicationHistoryEventDate,
                ApplicationHistoryEventTypeId = 1,
                ApplicationHistoryEventSubTypeId = applicationHistoryEventSubTypeId,
                Comment = "Status Change"
            };
        }
    }
}