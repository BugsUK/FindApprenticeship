namespace SFA.Apprenticeships.Data.Migrate.Faa.Repository.Sql
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Dapper;
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
            return _getOpenConnection.Query<ApplicationSummary>("SELECT ApplicationId, ApplicationStatusTypeId, UnsuccessfulReasonId, OutcomeReasonOther, AllocatedTo FROM Application WHERE ApplicationId in @applicationIds", new { applicationIds }).ToDictionary(a => a.ApplicationId, a => a);
        }

        public void DeleteByCandidateId(IEnumerable<int> candidateIds)
        {
            var connection = _getOpenConnection.GetOpenConnection();

            const string subVacancySql =
@"DELETE FROM SubVacancy 
WHERE AllocatedApplicationId IN 
(SELECT ApplicationId FROM Application WHERE CandidateId IN @CandidateIds)";
            connection.Execute(subVacancySql, new {candidateIds});

            const string schoolAttendedSql =
@"DELETE FROM SchoolAttended 
WHERE CandidateId IN 
(SELECT ApplicationId FROM Application WHERE CandidateGuid IN @CandidateIds)";
            connection.Execute(schoolAttendedSql, new { candidateIds });

            const string applicationHistorySql = 
@"DELETE FROM ApplicationHistory 
WHERE ApplicationId IN 
(SELECT ApplicationId FROM Application WHERE CandidateId IN @CandidateIds)";
            connection.Execute(applicationHistorySql, new {candidateIds});

            const string applicationSql =
@"DELETE FROM Application 
WHERE CandidateId IN @CandidateIds";
            connection.Execute(applicationSql, new {candidateIds});
        }
    }
}