namespace SFA.Apprenticeships.Data.Migrate.Faa.Repository.Sql
{
    using System.Collections.Generic;
    using System.Linq;
    using Application.Interfaces;
    using Entities.Sql;
    using Infrastructure.Repositories.Sql.Common;

    public class ApplicationHistoryRepository
    {
        private readonly IGetOpenConnection _getOpenConnection;
        private readonly ILogService _logService;

        public ApplicationHistoryRepository(IGetOpenConnection getOpenConnection, ILogService logService)
        {
            _getOpenConnection = getOpenConnection;
            _logService = logService;
        }

        public IDictionary<int, Dictionary<int, int>> GetApplicationHistoryIdsByApplicationIds(IEnumerable<int> applicationIds)
        {
            //TODO: This may have to deal with multiple instances of the same status type in future due to the ability to change the state of an application
            var ids = string.Join(",", applicationIds);
            _logService.Debug($"SELECT ApplicationId, COUNT(*) as repeats FROM ApplicationHistory WHERE ApplicationId IN ({ids}) GROUP BY ApplicationId, ApplicationHistoryEventTypeId, ApplicationHistoryEventSubTypeId ORDER BY repeats DESC");

            var applicationHistoryIds = _getOpenConnection.Query<ApplicationHistoryIds>("SELECT ApplicationHistoryId, ApplicationId, ApplicationHistoryEventSubTypeId FROM ApplicationHistory WHERE ApplicationId in @applicationIds", new { applicationIds });
            return applicationHistoryIds.GroupBy(ah => ah.ApplicationId).ToDictionary(g => g.Key, g => g.ToDictionary(gah => gah.ApplicationHistoryEventSubTypeId, gah => gah.ApplicationHistoryId));
        }

        // ReSharper disable once ClassNeverInstantiated.Local
        private class ApplicationHistoryIds
        {
            // ReSharper disable UnusedAutoPropertyAccessor.Local
            public int ApplicationHistoryId { get; set; }
            public int ApplicationId { get; set; }
            public int ApplicationHistoryEventSubTypeId { get; set; }
            // ReSharper restore UnusedAutoPropertyAccessor.Local
        }

        public IDictionary<int, List<ApplicationHistorySummary>> GetApplicationHistorySummariesByApplicationIds(IEnumerable<int> applicationIds)
        {
            var applicationHistorySummaries = _getOpenConnection.Query<ApplicationHistorySummary>("SELECT ApplicationHistoryId, ApplicationId, ApplicationHistoryEventDate, ApplicationHistoryEventSubTypeId FROM ApplicationHistory WHERE ApplicationId in @applicationIds", new { applicationIds });
            return applicationHistorySummaries.GroupBy(ah => ah.ApplicationId).ToDictionary(g => g.Key, g => g.ToList());
        }
    }
}