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

        public IDictionary<Guid, ApplicationIds> GetApplicationIdsByGuid(IEnumerable<Guid> applicationGuids)
        {
            return _getOpenConnection.Query<ApplicationIds>("SELECT ApplicationId, CandidateId, VacancyId, ApplicationGuid FROM Application WHERE ApplicationGuid in @applicationGuids", new { applicationGuids }).ToDictionary(a => a.ApplicationGuid, a => a);
        }

        public IDictionary<int, Dictionary<int, ApplicationIds>> GetApplicationIdsByCandidateIds(IEnumerable<int> candidateIds)
        {
            var applicationIds = _getOpenConnection.Query<ApplicationIds>("SELECT ApplicationId, CandidateId, VacancyId, ApplicationGuid FROM Application WHERE CandidateId in @candidateIds", new { candidateIds });
            return applicationIds.GroupBy(ah => ah.CandidateId).ToDictionary(g => g.Key, g => g.ToDictionary(ga => ga.VacancyId, ga => ga));
        }

        public IDictionary<int, ApplicationSummary> GetApplicationSummariesByIds(IEnumerable<int> applicationIds)
        {
            return _getOpenConnection.Query<ApplicationSummary>("SELECT ApplicationId, OutcomeReasonOther FROM Application WHERE ApplicationId in @applicationIds", new { applicationIds }).ToDictionary(a => a.ApplicationId, a => a);
        }
    }
}