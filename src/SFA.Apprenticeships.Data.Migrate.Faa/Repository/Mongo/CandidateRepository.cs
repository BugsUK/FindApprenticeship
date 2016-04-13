namespace SFA.Apprenticeships.Data.Migrate.Faa.Repository.Mongo
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Entities.Mongo;
    using Infrastructure.Repositories.Mongo.Common.Configuration;
    using MongoDB.Driver;
    using SFA.Infrastructure.Interfaces;

    public class CandidateRepository
    {
        private readonly ILogService _logService;
        private readonly IMongoDatabase _database;

        private readonly UserRepository _userRepository;

        public CandidateRepository(IConfigurationService configurationService, ILogService logService)
        {
            _logService = logService;
            var connectionString = configurationService.Get<MongoConfiguration>().MetricsCandidatesDb;
            var databaseName = MongoUrl.Create(connectionString).DatabaseName;
            _database = new MongoClient(connectionString).GetDatabase(databaseName);

            _userRepository = new UserRepository(configurationService);
        }

        public async Task<long> GetCandidatesCount()
        {
            var cursor = _database.GetCollection<Candidate>("candidates").CountAsync(Builders<Candidate>.Filter.Empty);
            return await cursor;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="lastId">Pass null or empty for the first page or the Id of the last item in the current page</param>
        public async Task<IAsyncCursor<Candidate>> GetCandidatesPageAsync(Guid? lastId)
        {
            //http://stackoverflow.com/questions/31675598/how-to-efficiently-page-batches-of-results-with-mongodb

            FilterDefinition<Candidate> filter;
            var filterBuilder = Builders<Candidate>.Filter;
            if (lastId == null || lastId == Guid.Empty)
            {
                filter = filterBuilder.Empty;
            }
            else
            {
                filter = filterBuilder.Gt(c => c.Id, lastId.Value);
            }

            var sort = Builders<Candidate>.Sort.Ascending(c => c.Id);
            var options = new FindOptions<Candidate>
            {
                Sort = sort,
                Limit = 500,
                Projection = Builders<Candidate>.Projection
                .Include(a => a.Id)
                .Include(a => a.LegacyCandidateId)
            };

            var cursor = _database.GetCollection<Candidate>("candidates").FindAsync(filter, options);
            return await cursor;
        }

        public async Task<IDictionary<Guid, Candidate>> GetAllCandidatesAsync()
        {
            var candidatesCount = await GetCandidatesCount();

            var candidates = new Dictionary<Guid, Candidate>((int)candidatesCount);

            var options = new FindOptions<Candidate>
            {
                BatchSize = 10000,
                Projection = Builders<Candidate>.Projection
                   .Include(a => a.Id)
                   .Include(a => a.LegacyCandidateId)
            };

            var cursor = await _database.GetCollection<Candidate>("candidates").FindAsync(Builders<Candidate>.Filter.Empty, options);

            while (await cursor.MoveNextAsync())
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

        public async Task<IAsyncCursor<Candidate>> GetCandidateById(Guid id)
        {
            var filter = Builders<Candidate>.Filter.Eq(c => c.Id, id);
            var cursor = await _database.GetCollection<Candidate>("candidates").FindAsync(filter);
            await cursor.MoveNextAsync();
            return cursor;
        }

        public async Task<IAsyncCursor<Candidate>> GetCandidatesByIds(IEnumerable<Guid> ids)
        {
            var options = new FindOptions<Candidate>
            {
                Projection = Builders<Candidate>.Projection
                   .Include(a => a.Id)
                   .Include(a => a.LegacyCandidateId)
            };
            var filter = Builders<Candidate>.Filter.In(c => c.Id, ids);
            var cursor = _database.GetCollection<Candidate>("candidates").FindAsync(filter, options);
            return await cursor;
        }
    }
}