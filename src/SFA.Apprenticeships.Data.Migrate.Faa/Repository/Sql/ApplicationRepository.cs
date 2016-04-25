namespace SFA.Apprenticeships.Data.Migrate.Faa.Repository.Sql
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Entities.Sql;
    using Infrastructure.Repositories.Sql.Common;

    public class ApplicationRepository
    {
        private readonly IGetOpenConnection _getOpenConnection;

        public ApplicationRepository(IGetOpenConnection getOpenConnection)
        {
            _getOpenConnection = getOpenConnection;
        }

        public IDictionary<Guid, int> GetApplicationIdsByGuid(IEnumerable<Guid> applicationGuids)
        {
            return _getOpenConnection.Query<KeyValuePair<Guid, int>>("SELECT ApplicationGuid as [Key], ApplicationId as Value FROM Application WHERE ApplicationGuid in @applicationGuids", new { applicationGuids }).ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
        }

        public IDictionary<Guid, ApplicationSummary> GetApplicationSummariesByGuid(IEnumerable<Guid> applicationGuids)
        {
            return _getOpenConnection.Query<ApplicationSummary>("SELECT ApplicationId, OutcomeReasonOther, ApplicationGuid FROM Application WHERE ApplicationGuid in @applicationGuids", new { applicationGuids }).ToDictionary(a => a.ApplicationGuid, a => a);
        }
    }
}