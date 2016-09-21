namespace SFA.Apprenticeships.Data.Migrate.Faa.Repository.Sql
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Dapper;
    using Entities.Sql;
    using Infrastructure.Repositories.Sql.Common;

    public class CandidateRepository
    {
        private readonly IGetOpenConnection _getOpenConnection;

        public CandidateRepository(IGetOpenConnection getOpenConnection)
        {
            _getOpenConnection = getOpenConnection;
        }

        public IDictionary<Guid, int> GetAllCandidateIds()
        {
            return _getOpenConnection.Query<KeyValuePair<Guid, int>>("SELECT CandidateGuid as [Key], CandidateId as Value FROM Candidate").ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
        }

        public IDictionary<Guid, int> GetCandidateIdsByGuid(IEnumerable<Guid> candidateGuids)
        {
            return _getOpenConnection.Query<KeyValuePair<Guid, int>>("SELECT CandidateGuid as [Key], CandidateId as Value FROM Candidate WHERE CandidateGuid in @candidateGuids", new { candidateGuids }).ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
        }

        public IDictionary<Guid, CandidateSummary> GetCandidateSummariesByGuid(IEnumerable<Guid> candidateGuids)
        {
            return _getOpenConnection.Query<CandidateSummary>("SELECT CandidateId, PersonId, CandidateGuid FROM Candidate WHERE CandidateGuid in @candidateGuids", new { candidateGuids }).ToDictionary(cs => cs.CandidateGuid, cs => cs);
        }

        public void DeleteByCandidateGuid(ICollection<Guid> candidateGuids)
        {
            var connection = _getOpenConnection.GetOpenConnection();

            var personIds = _getOpenConnection.Query<int>("SELECT PersonId FROM Candidate WHERE CandidateGuid IN @CandidateGuids", new { candidateGuids });

            const string schoolAttendedSql =
@"DELETE FROM SchoolAttended 
WHERE CandidateId IN 
(SELECT CandidateId FROM Candidate WHERE CandidateGuid IN @CandidateGuids)";
            connection.Execute(schoolAttendedSql, new { candidateGuids });

            const string candidateHistorySql =
@"DELETE FROM CandidateHistory 
WHERE CandidateId IN 
(SELECT CandidateId FROM Candidate WHERE CandidateGuid IN @CandidateGuids)";
            connection.Execute(candidateHistorySql, new { candidateGuids });

            const string candidateSql =
@"DELETE FROM Candidate 
WHERE CandidateGuid IN @CandidateGuids";
            connection.Execute(candidateSql, new { candidateGuids });

            const string personSql =
@"DELETE FROM Person 
WHERE PersonId IN @PersonIds";
            connection.Execute(personSql, new { personIds });
        }
    }
}