namespace SFA.Apprenticeships.Infrastructure.Repositories.Vacancies
{
    using System;
    using Application.Interfaces.Logging;
    using Domain.Interfaces.Configuration;
    using Domain.Interfaces.Repositories;
    using Mongo.Common;
    using Mongo.Common.Configuration;
    using MongoDB.Driver.Builders;
    using Entities;
    using MongoDB.Driver;

    public class ReferenceNumberRepository : GenericMongoClient<MongoReferenceNumber>, IReferenceNumberRepository
    {
        private readonly ILogService _logger;

        private static readonly Guid VacancyReferenceNumberId = new Guid("00000000-0000-0000-0000-000000000001");

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
            var lastReferenceNumber = result.ModifiedDocument.GetValue("lastReferenceNumber").AsInt32;

            _logger.Debug($"Called Mongodb to get next vacancy reference number={lastReferenceNumber}");

            return lastReferenceNumber;
        }
    }
}
