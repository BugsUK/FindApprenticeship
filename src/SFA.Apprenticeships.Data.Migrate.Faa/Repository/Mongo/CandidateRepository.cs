namespace SFA.Apprenticeships.Data.Migrate.Faa.Repository.Mongo
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Configuration;
    using Entities.Mongo;
    using MongoDB.Driver;
    using SFA.Infrastructure.Interfaces;

    public class CandidateRepository
    {
        private const string CollectionName = "candidates";

        private readonly ILogService _logService;
        private readonly IMongoDatabase _database;

        public CandidateRepository(IConfigurationService configurationService, ILogService logService)
        {
            _logService = logService;
            var connectionString = configurationService.Get<MongoConfiguration>().MetricsCandidatesDb;
            var databaseName = MongoUrl.Create(connectionString).DatabaseName;
            _database = new MongoClient(connectionString).GetDatabase(databaseName);
        }

        public async Task<long> GetCandidatesCount(CancellationToken cancellationToken)
        {
            var cursor = _database.GetCollection<Candidate>(CollectionName).CountAsync(Builders<Candidate>.Filter.Empty, cancellationToken: cancellationToken);
            return await cursor;
        }

        public async Task<long> GetCandidatesCreatedSinceCount(DateTime lastCreatedDate, CancellationToken cancellationToken)
        {
            var filter = Builders<Candidate>.Filter.Gt(a => a.DateCreated, lastCreatedDate);
            var cursor = _database.GetCollection<Candidate>(CollectionName).CountAsync(filter, cancellationToken: cancellationToken);
            return await cursor;
        }

        public async Task<long> GetCandidatesUpdatedSinceCount(DateTime lastUpdatedDate, CancellationToken cancellationToken)
        {
            var filter = Builders<Candidate>.Filter.Gt(a => a.DateUpdated, lastUpdatedDate);
            var cursor = _database.GetCollection<Candidate>(CollectionName).CountAsync(filter, cancellationToken: cancellationToken);
            return await cursor;
        }

        public async Task<IDictionary<Guid, CandidateSummary>> GetAllCandidateSummaries(CancellationToken cancellationToken)
        {
            var candidatesCount = await GetCandidatesCount(cancellationToken);

            var candidates = new Dictionary<Guid, CandidateSummary>((int)candidatesCount);

            var options = new FindOptions<CandidateSummary>
            {
                BatchSize = 10000,
                Projection = GetCandidateSummaryProjection()
            };

            var cursor = await _database.GetCollection<CandidateSummary>(CollectionName).FindAsync(Builders<CandidateSummary>.Filter.Empty, options, cancellationToken);

            while (await cursor.MoveNextAsync(cancellationToken))
            {
                var batch = cursor.Current;
                var count = 0;
                foreach (var candidate in batch)
                {
                    candidates[candidate.Id] = candidate;
                    count++;
                }
                _logService.Info($"Retrieved {count} totalling {candidates.Count} of {candidatesCount} candidates. {Math.Round(((double)candidates.Count/candidatesCount)*100, 2)}% complete");
            }

            return candidates;
        }

        public async Task<IAsyncCursor<CandidateSummary>> GetCandidateSummariesByIds(IEnumerable<Guid> ids, CancellationToken cancellationToken)
        {
            var options = new FindOptions<CandidateSummary>
            {
                Projection = GetCandidateSummaryProjection()
            };
            var filter = Builders<CandidateSummary>.Filter.In(c => c.Id, ids);
            var cursor = _database.GetCollection<CandidateSummary>(CollectionName).FindAsync(filter, options, cancellationToken);
            return await cursor;
        }

        private static ProjectionDefinition<CandidateSummary> GetCandidateSummaryProjection()
        {
            return Builders<CandidateSummary>.Projection
                   .Include(a => a.Id)
                   .Include(a => a.LegacyCandidateId);
        }

        public async Task<IAsyncCursor<Candidate>> GetAllCandidatesCreatedSince(DateTime lastCreatedDate, CancellationToken cancellationToken)
        {
            var sort = Builders<Candidate>.Sort.Ascending(a => a.DateCreated);
            var options = new FindOptions<Candidate>
            {
                Sort = sort,
                BatchSize = 1000,
                Projection = GetCandidateProjection()
            };
            var filter = Builders<Candidate>.Filter.Gt(a => a.DateCreated, lastCreatedDate);
            var cursor = _database.GetCollection<Candidate>(CollectionName).FindAsync(filter, options, cancellationToken);
            return await cursor;
        }

        public async Task<IAsyncCursor<Candidate>> GetAllCandidatesUpdatedSince(DateTime lastUpdatedDate, CancellationToken cancellationToken)
        {
            var sort = Builders<Candidate>.Sort.Ascending(a => a.DateUpdated);
            var options = new FindOptions<Candidate>
            {
                Sort = sort,
                BatchSize = 1000,
                Projection = GetCandidateProjection()
            };
            var filter = Builders<Candidate>.Filter.Gt(a => a.DateUpdated, lastUpdatedDate);
            var cursor = _database.GetCollection<Candidate>(CollectionName).FindAsync(filter, options, cancellationToken);
            return await cursor;
        }

        private static ProjectionDefinition<Candidate> GetCandidateProjection()
        {
            return Builders<Candidate>.Projection
                .Include(a => a.Id)
                .Include(a => a.DateCreated)
                .Include(a => a.DateUpdated)
                   .Include(a => a.LegacyCandidateId);
        }
    }
}