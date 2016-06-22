namespace SFA.Apprenticeships.Data.Migrate.Faa.Repository.Mongo
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Configuration;
    using Entities.Mongo;
    using MongoDB.Driver;

    using SFA.Apprenticeships.Application.Interfaces;
    using SFA.Infrastructure.Interfaces;

    public class CandidateUserRepository
    {
        private const string CollectionName = "candidates";

        private readonly ILogService _logService;
        private readonly IMongoDatabase _database;

        public CandidateUserRepository(IConfigurationService configurationService, ILogService logService)
        {
            _logService = logService;
            var connectionString = configurationService.Get<MigrateFromFaaToAvmsPlusConfiguration>().CandidatesDb;
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
                   .Include(c => c.Id)
                   .Include(c => c.LegacyCandidateId);
        }

        public async Task<IAsyncCursor<Candidate>> GetAllCandidateUsers(CancellationToken cancellationToken)
        {
            var sort = Builders<Candidate>.Sort.Ascending(a => a.DateCreated);
            var options = new FindOptions<Candidate>
            {
                Sort = sort,
                BatchSize = 1000,
                Projection = GetCandidateProjection()
            };
            return await _database.GetCollection<Candidate>(CollectionName).FindAsync(Builders<Candidate>.Filter.Empty, options, cancellationToken);
        }

        public async Task<IAsyncCursor<Candidate>> GetAllCandidateUsersCreatedSince(DateTime lastCreatedDate, CancellationToken cancellationToken)
        {
            var sort = Builders<Candidate>.Sort.Ascending(a => a.DateCreated);
            var options = new FindOptions<Candidate>
            {
                Sort = sort,
                BatchSize = 1000,
                Projection = GetCandidateProjection()
            };
            var filter = Builders<Candidate>.Filter.Gt(a => a.DateCreated, lastCreatedDate);

            return await _database.GetCollection<Candidate>(CollectionName).FindAsync(filter, options, cancellationToken);
        }

        public async Task<IAsyncCursor<Candidate>> GetAllCandidateUsersUpdatedSince(DateTime lastUpdatedDate, CancellationToken cancellationToken)
        {
            var sort = Builders<Candidate>.Sort.Ascending(a => a.DateUpdated);
            var options = new FindOptions<Candidate>
            {
                Sort = sort,
                BatchSize = 1000,
                Projection = GetCandidateProjection()
            };
            var filter = Builders<Candidate>.Filter.Gt(a => a.DateUpdated, lastUpdatedDate);

            return await _database.GetCollection<Candidate>(CollectionName).FindAsync(filter, options, cancellationToken);
        }

        private static ProjectionDefinition<Candidate> GetCandidateProjection()
        {
            return Builders<Candidate>.Projection
                .Include(c => c.Id)
                .Include(c => c.DateCreated)
                .Include(c => c.DateUpdated)
                .Include(c => c.LegacyCandidateId)
                .Include(c => c.RegistrationDetails.FirstName)
                .Include(c => c.RegistrationDetails.MiddleNames)
                .Include(c => c.RegistrationDetails.LastName)
                .Include(c => c.RegistrationDetails.DateOfBirth)
                .Include(c => c.RegistrationDetails.Address.AddressLine1)
                .Include(c => c.RegistrationDetails.Address.AddressLine2)
                .Include(c => c.RegistrationDetails.Address.AddressLine3)
                .Include(c => c.RegistrationDetails.Address.AddressLine4)
                .Include(c => c.RegistrationDetails.Address.Postcode)
                .Include(c => c.RegistrationDetails.Address.GeoPoint.Longitude)
                .Include(c => c.RegistrationDetails.Address.GeoPoint.Latitude)
                .Include(c => c.RegistrationDetails.EmailAddress)
                .Include(c => c.RegistrationDetails.PhoneNumber)
                .Include(c => c.MonitoringInformation.Gender)
                .Include(c => c.MonitoringInformation.DisabilityStatus)
                .Include(c => c.MonitoringInformation.Ethnicity);
        }
    }
}