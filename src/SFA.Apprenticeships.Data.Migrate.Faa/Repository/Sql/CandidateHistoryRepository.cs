namespace SFA.Apprenticeships.Data.Migrate.Faa.Repository.Sql
{
    using System.Collections.Generic;
    using System.Linq;
    using Entities.Sql;
    using Infrastructure.Repositories.Sql.Common;

    public class CandidateHistoryRepository
    {
        private readonly IGetOpenConnection _getOpenConnection;

        public CandidateHistoryRepository(IGetOpenConnection getOpenConnection)
        {
            _getOpenConnection = getOpenConnection;
        }

        public IDictionary<int, Dictionary<int, int>> GetCandidateHistoryIdsByCandidateIds(IEnumerable<int> candidateIds)
        {
            var candidateHistoryIds = _getOpenConnection.Query<CandidateHistoryIds>("SELECT CandidateHistoryId, CandidateId, CandidateHistorySubEventTypeId FROM CandidateHistory WHERE CandidateId in @candidateIds", new { candidateIds });
            return candidateHistoryIds.GroupBy(ah => ah.CandidateId).ToDictionary(g => g.Key, g => g.ToDictionary(gch => gch.CandidateHistorySubEventTypeId, gch => gch.CandidateHistoryId));
        }

        // ReSharper disable once ClassNeverInstantiated.Local
        private class CandidateHistoryIds
        {
            // ReSharper disable UnusedAutoPropertyAccessor.Local
            public int CandidateHistoryId { get; set; }
            public int CandidateId { get; set; }
            public int CandidateHistorySubEventTypeId { get; set; }
            // ReSharper restore UnusedAutoPropertyAccessor.Local
        }
    }
}