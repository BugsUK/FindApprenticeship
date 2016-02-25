namespace SFA.Apprenticeships.Infrastructure.Repositories.Mongo.Vacancies
{
    using System;
    using Common;
    using Common.Configuration;
    using Domain.Interfaces.Repositories;
    using Entities;
    using MongoDB.Driver;
    using MongoDB.Driver.Builders;
    using SFA.Infrastructure.Interfaces;

    public class ReferenceNumberRepository : GenericMongoClient<MongoReferenceNumber>, IReferenceNumberRepository
    {
        private readonly ILogService _logger;

        private static readonly Guid VacancyReferenceNumberId = new Guid("00000000-0000-0000-0000-000000000001");
        private static readonly Guid LegacyApplicationId = new Guid("00000000-0000-0000-0000-000000000002");

        public ReferenceNumberRepository(
            IConfigurationService configurationService,
            ILogService logger)
        {
            var config = configurationService.Get<MongoConfiguration>();

            Initialise(config.VacancyDb, "referenceNumbers");

            _logger = logger;
        }

        public long GetNextVacancyReferenceNumber()
        {
            _logger.Debug($"Called Mongodb to get next vacancy reference number with Id={VacancyReferenceNumberId}");

            var args = new FindAndModifyArgs
            {
                Query = Query<MongoReferenceNumber>.EQ(d => d.Id, VacancyReferenceNumberId),
                Update = Update.Inc("lastReferenceNumber", 1),
                SortBy = SortBy.Null,
                Upsert = true,
                VersionReturned = FindAndModifyDocumentVersion.Modified
            };

            var result = Collection.FindAndModify(args);
            var nextNumber = result.ModifiedDocument.GetValue("lastReferenceNumber").AsInt32;

            _logger.Debug($"Called Mongodb to get next vacancy reference number={nextNumber}");

            return nextNumber;
        }

        public long GetNextLegacyApplicationId()
        {
            _logger.Debug($"Called Mongodb to get next legacy application id with Id={LegacyApplicationId}");

            var args = new FindAndModifyArgs
            {
                Query = Query<MongoReferenceNumber>.EQ(d => d.Id, LegacyApplicationId),
                Update = Update.Inc("lastReferenceNumber", -1),
                SortBy = SortBy.Null,
                Upsert = true,
                VersionReturned = FindAndModifyDocumentVersion.Modified
            };

            var result = Collection.FindAndModify(args);
            var nextId = result.ModifiedDocument.GetValue("lastReferenceNumber").AsInt32;

            _logger.Debug($"Called Mongodb to get next legacy application id={nextId}");

            return nextId;
        }
    }
}
