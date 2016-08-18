namespace SFA.Apprenticeships.Data.Migrate.Faa.Repository.Mongo
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Configuration;
    using Entities.Mongo;
    using MongoDB.Driver;

    using SFA.Apprenticeships.Application.Interfaces;
    using SFA.Infrastructure.Interfaces;

    public class VacancyApplicationsRepository
    {
        private const ApplicationStatuses Status = ApplicationStatuses.Saved;

        private readonly string _collectionName;
        private readonly ILogService _logService;
        private readonly IMongoDatabase _database;

        public VacancyApplicationsRepository(string collectionName, IConfigurationService configurationService, ILogService logService)
        {
            _collectionName = collectionName;
            _logService = logService;
            var connectionString = configurationService.Get<MigrateFromFaaToAvmsPlusConfiguration>().SourceApplicationsDb;
            var databaseName = MongoUrl.Create(connectionString).DatabaseName;
            _database = new MongoClient(connectionString).GetDatabase(databaseName);
        }

        public async Task<long> GetVacancyApplicationsCount(CancellationToken cancellationToken)
        {
            var cursor = _database.GetCollection<VacancyApplication>(_collectionName).CountAsync(Builders<VacancyApplication>.Filter.Gte(a => a.Status, Status), cancellationToken: cancellationToken);
            return await cursor;
        }

        public async Task<long> GetVacancyApplicationsCreatedSinceCount(DateTime lastCreatedDate, CancellationToken cancellationToken)
        {
            var filter = Builders<VacancyApplication>.Filter.Gte(a => a.Status, Status) & Builders<VacancyApplication>.Filter.Gt(a => a.DateCreated, lastCreatedDate);
            var cursor = _database.GetCollection<VacancyApplication>(_collectionName).CountAsync(filter, cancellationToken: cancellationToken);
            return await cursor;
        }

        public async Task<long> GetVacancyApplicationsUpdatedSinceCount(DateTime lastUpdatedDate, CancellationToken cancellationToken)
        {
            var filter = Builders<VacancyApplication>.Filter.Gte(a => a.Status, Status) & Builders<VacancyApplication>.Filter.Gt(a => a.DateUpdated, lastUpdatedDate);
            var cursor = _database.GetCollection<VacancyApplication>(_collectionName).CountAsync(filter, cancellationToken: cancellationToken);
            return await cursor;
        }

        public async Task<IAsyncCursor<VacancyApplication>> GetAllVacancyApplications(CancellationToken cancellationToken)
        {
            var sort = Builders<VacancyApplication>.Sort.Ascending(a => a.DateCreated);
            var options = new FindOptions<VacancyApplication>
            {
                Sort = sort,
                BatchSize = 1000,
                Projection = GetVacancyApplicationProjection()
            };
            var cursor = _database.GetCollection<VacancyApplication>(_collectionName).FindAsync(Builders<VacancyApplication>.Filter.Gte(a => a.Status, Status), options, cancellationToken);
            return await cursor;
        }

        public async Task<IAsyncCursor<VacancyApplication>> GetAllVacancyApplicationsCreatedSince(DateTime lastCreatedDate, CancellationToken cancellationToken)
        {
            var sort = Builders<VacancyApplication>.Sort.Ascending(a => a.DateCreated);
            var options = new FindOptions<VacancyApplication>
            {
                Sort = sort,
                BatchSize = 1000,
                Projection = GetVacancyApplicationProjection()
            };
            var filter = Builders<VacancyApplication>.Filter.Gte(a => a.Status, Status) & Builders<VacancyApplication>.Filter.Gt(a => a.DateCreated, lastCreatedDate);
            var cursor = _database.GetCollection<VacancyApplication>(_collectionName).FindAsync(filter, options, cancellationToken);
            return await cursor;
        }

        public async Task<IAsyncCursor<VacancyApplication>> GetAllVacancyApplicationsUpdatedSince(DateTime lastUpdatedDate, CancellationToken cancellationToken)
        {
            var sort = Builders<VacancyApplication>.Sort.Ascending(a => a.DateUpdated);
            var options = new FindOptions<VacancyApplication>
            {
                Sort = sort,
                BatchSize = 1000,
                Projection = GetVacancyApplicationProjection()
            };
            var filter = Builders<VacancyApplication>.Filter.Gte(a => a.Status, Status) & Builders<VacancyApplication>.Filter.Gt(a => a.DateUpdated, lastUpdatedDate);
            var cursor = _database.GetCollection<VacancyApplication>(_collectionName).FindAsync(filter, options, cancellationToken);
            return await cursor;
        }

        private static ProjectionDefinition<VacancyApplication> GetVacancyApplicationProjection()
        {
            return Builders<VacancyApplication>.Projection
                .Include(a => a.Id)
                .Include(a => a.DateCreated)
                .Include(a => a.DateUpdated)
                .Include(a => a.Status)
                .Include(a => a.DateApplied)
                .Include(a => a.CandidateId)
                .Include(a => a.LegacyApplicationId)
                .Include(a => a.CandidateInformation.EducationHistory.Institution)
                .Include(a => a.CandidateInformation.EducationHistory.FromYear)
                .Include(a => a.CandidateInformation.EducationHistory.ToYear)
                .Include(a => a.Notes)
                .Include(a => a.SuccessfulDateTime)
                .Include(a => a.UnsuccessfulDateTime)
                .Include(a => a.WithdrawnOrDeclinedReason)
                .Include(a => a.UnsuccessfulReason)
                .Include(a => a.Vacancy.Id)
                .Include(a => a.Vacancy.VacancyReference)
                .Include(a => a.Vacancy.Title);
        }
    }
}