namespace SFA.Apprenticeships.Data.Migrate.Faa.Repository.Sql
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Entities.Sql;
    using Infrastructure.Repositories.Sql.Common;

    public class SubVacancyRepository
    {
        private readonly IGetOpenConnection _getOpenConnection;

        public SubVacancyRepository(IGetOpenConnection getOpenConnection)
        {
            _getOpenConnection = getOpenConnection;
        }

        public IDictionary<int, ApplicationSummary> GetApplicationSummariesByIds(IEnumerable<int> applicationIds)
        {
            return _getOpenConnection.Query<ApplicationSummary>("SELECT ApplicationId, OutcomeReasonOther FROM Application WHERE ApplicationId in @applicationIds", new { applicationIds }).ToDictionary(a => a.ApplicationId, a => a);
        }
    }
}