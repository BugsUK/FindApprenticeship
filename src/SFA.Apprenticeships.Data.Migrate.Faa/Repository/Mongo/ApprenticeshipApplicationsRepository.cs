namespace SFA.Apprenticeships.Data.Migrate.Faa.Repository.Mongo
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Configuration;
    using Entities.Mongo;
    using MongoDB.Driver;
    using SFA.Infrastructure.Interfaces;

    public class ApprenticeshipApplicationsRepository
    {
        private readonly IMongoDatabase _database;

        public ApprenticeshipApplicationsRepository(IConfigurationService configurationService)
        {
            var connectionString = configurationService.Get<MongoConfiguration>().MetricsApplicationsDb;
            var databaseName = MongoUrl.Create(connectionString).DatabaseName;
            _database = new MongoClient(connectionString).GetDatabase(databaseName);
        }

        public async Task<long> GetApprenticeshipApplicationsCount(CancellationToken cancellationToken)
        {
            var cursor = _database.GetCollection<ApprenticeshipApplication>("apprenticeships").CountAsync(Builders<ApprenticeshipApplication>.Filter.Gte(a => a.Status, 10), cancellationToken: cancellationToken);
            return await cursor;
        }

        public async Task<long> GetApprenticeshipApplicationsCreatedSinceCount(DateTime lastCreatedDate, CancellationToken cancellationToken)
        {
            var filter = Builders<ApprenticeshipApplication>.Filter.Gte(a => a.Status, 10) & Builders<ApprenticeshipApplication>.Filter.Gt(a => a.DateCreated, lastCreatedDate);
            var cursor = _database.GetCollection<ApprenticeshipApplication>("apprenticeships").CountAsync(filter, cancellationToken: cancellationToken);
            return await cursor;
        }

        public async Task<long> GetApprenticeshipApplicationsUpdatedSinceCount(DateTime lastUpdatedDate, CancellationToken cancellationToken)
        {
            var filter = Builders<ApprenticeshipApplication>.Filter.Gte(a => a.Status, 10) & Builders<ApprenticeshipApplication>.Filter.Gt(a => a.DateUpdated, lastUpdatedDate);
            var cursor = _database.GetCollection<ApprenticeshipApplication>("apprenticeships").CountAsync(filter, cancellationToken: cancellationToken);
            return await cursor;
        }

        public async Task<IAsyncCursor<ApprenticeshipApplication>> GetAllApprenticeshipApplications(CancellationToken cancellationToken)
        {
            var sort = Builders<ApprenticeshipApplication>.Sort.Ascending(a => a.DateCreated);
            var options = new FindOptions<ApprenticeshipApplication>
            {
                Sort = sort,
                BatchSize = 1000,
                Projection = GetApprenticeshipApplicationProjection()
            };
            var cursor = _database.GetCollection<ApprenticeshipApplication>("apprenticeships").FindAsync(Builders<ApprenticeshipApplication>.Filter.Gte(a => a.Status, 10), options, cancellationToken);
            return await cursor;
        }

        public async Task<IAsyncCursor<ApprenticeshipApplication>> GetAllApprenticeshipApplicationsCreatedSince(DateTime lastCreatedDate, CancellationToken cancellationToken)
        {
            var sort = Builders<ApprenticeshipApplication>.Sort.Ascending(a => a.DateCreated);
            var options = new FindOptions<ApprenticeshipApplication>
            {
                Sort = sort,
                BatchSize = 1000,
                Projection = GetApprenticeshipApplicationProjection()
            };
            var filter = Builders<ApprenticeshipApplication>.Filter.Gte(a => a.Status, 10) & Builders<ApprenticeshipApplication>.Filter.Gt(a => a.DateCreated, lastCreatedDate);
            var cursor = _database.GetCollection<ApprenticeshipApplication>("apprenticeships").FindAsync(filter, options, cancellationToken);
            return await cursor;
        }

        public async Task<IAsyncCursor<ApprenticeshipApplication>> GetAllApprenticeshipApplicationsUpdatedSince(DateTime lastUpdatedDate, CancellationToken cancellationToken)
        {
            var sort = Builders<ApprenticeshipApplication>.Sort.Ascending(a => a.DateUpdated);
            var options = new FindOptions<ApprenticeshipApplication>
            {
                Sort = sort,
                BatchSize = 1000,
                Projection = GetApprenticeshipApplicationProjection()
            };
            var filter = Builders<ApprenticeshipApplication>.Filter.Gte(a => a.Status, 10) & Builders<ApprenticeshipApplication>.Filter.Gt(a => a.DateUpdated, lastUpdatedDate);
            var cursor = _database.GetCollection<ApprenticeshipApplication>("apprenticeships").FindAsync(filter, options, cancellationToken);
            return await cursor;
        }

        private static ProjectionDefinition<ApprenticeshipApplication> GetApprenticeshipApplicationProjection()
        {
            return Builders<ApprenticeshipApplication>.Projection
                .Include(a => a.Id)
                .Include(a => a.DateCreated)
                .Include(a => a.DateUpdated)
                .Include(a => a.Status)
                .Include(a => a.CandidateId)
                .Include(a => a.LegacyApplicationId)
                .Include(a => a.WithdrawnOrDeclinedReason)
                .Include(a => a.UnsuccessfulReason)
                .Include(a => a.Vacancy.Id)
                .Include(a => a.Vacancy.VacancyReference)
                .Include(a => a.Vacancy.Title);
        }
    }
}