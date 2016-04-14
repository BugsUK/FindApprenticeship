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
            var cursor = _database.GetCollection<Candidate>("candidates").CountAsync(Builders<Candidate>.Filter.Empty, cancellationToken: cancellationToken);
            return await cursor;
        }

        public async Task<IDictionary<Guid, Candidate>> GetAllCandidatesAsync(CancellationToken cancellationToken)
        {
            var candidatesCount = await GetCandidatesCount(cancellationToken);

            var candidates = new Dictionary<Guid, Candidate>((int)candidatesCount);

            var options = new FindOptions<Candidate>
            {
                BatchSize = 10000,
                Projection = Builders<Candidate>.Projection
                   .Include(a => a.Id)
                   .Include(a => a.LegacyCandidateId)
            };

            var cursor = await _database.GetCollection<Candidate>("candidates").FindAsync(Builders<Candidate>.Filter.Empty, options, cancellationToken);

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

        public async Task<IAsyncCursor<Candidate>> GetCandidateById(Guid id, CancellationToken cancellationToken)
        {
            var filter = Builders<Candidate>.Filter.Eq(c => c.Id, id);
            var cursor = await _database.GetCollection<Candidate>("candidates").FindAsync(filter, cancellationToken: cancellationToken);
            await cursor.MoveNextAsync(cancellationToken);
            return cursor;
        }

        public async Task<IAsyncCursor<Candidate>> GetCandidatesByIds(IEnumerable<Guid> ids, CancellationToken cancellationToken)
        {
            var options = new FindOptions<Candidate>
            {
                Projection = Builders<Candidate>.Projection
                   .Include(a => a.Id)
                   .Include(a => a.LegacyCandidateId)
            };
            var filter = Builders<Candidate>.Filter.In(c => c.Id, ids);
            var cursor = _database.GetCollection<Candidate>("candidates").FindAsync(filter, options, cancellationToken);
            return await cursor;
        }
    }
}