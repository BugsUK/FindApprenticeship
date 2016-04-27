namespace SFA.Apprenticeships.Data.Migrate.Faa.Repository.Sql
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
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
    }
}