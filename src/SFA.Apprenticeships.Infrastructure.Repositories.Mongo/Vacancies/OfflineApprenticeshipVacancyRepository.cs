namespace SFA.Apprenticeships.Infrastructure.Repositories.Mongo.Vacancies
{
    using System;
    using Domain.Entities.Vacancies.ProviderVacancies.Apprenticeship;
    using Domain.Interfaces.Repositories;
    using Common;
    using Common.Configuration;
    using Entities;
    using MongoDB.Driver;
    using MongoDB.Driver.Builders;
    using SFA.Infrastructure.Interfaces;

    public class OfflineApprenticeshipVacancyRepository : GenericMongoClient<MongoApprenticeshipVacancy, Guid>, IOfflineApprenticeshipVacancyRepository
    {
        private readonly IMapper _mapper;
        private readonly ILogService _logger;

        public OfflineApprenticeshipVacancyRepository(IConfigurationService configurationService, IMapper mapper, ILogService logger)
        {
            var config = configurationService.Get<MongoConfiguration>();

            Initialise(config.VacancyDb, "apprenticeshipVacancies");

            _mapper = mapper;
            _logger = logger;
        }

        public void IncrementOfflineApplicationClickThrough(int vacancyReferenceNumber)
        {
            _logger.Debug("Calling Mongodb to increment the OfflineApplicationClickThroughCount property by one for vacancy with reference number: {0}", vacancyReferenceNumber);

            var args = new FindAndModifyArgs
            {
                Query = new QueryBuilder<ApprenticeshipVacancy>().And(Query<ApprenticeshipVacancy>.EQ(d => d.VacancyReferenceNumber, vacancyReferenceNumber), Query<ApprenticeshipVacancy>.EQ(d => d.OfflineVacancy, true)),
                Update = Update.Inc("OfflineApplicationClickThroughCount", 1),
                VersionReturned = FindAndModifyDocumentVersion.Modified
            };

            var result = Collection.FindAndModify(args);

            if (result.Ok)
            {
                var mongoEntity = result.GetModifiedDocumentAs<MongoApprenticeshipVacancy>();

                var entity = _mapper.Map<MongoApprenticeshipVacancy, ApprenticeshipVacancy>(mongoEntity);

                _logger.Debug(
                    "Call to Mongodb to increment the OfflineApplicationClickThroughCount property by one for vacancy with reference number: {0} successfully. New value: {1}",
                    vacancyReferenceNumber, entity.OfflineApplicationClickThroughCount);
            }
            else
            {
                _logger.Warn(
                    "Call to Mongodb to increment the OfflineApplicationClickThroughCount property by one for vacancy with reference number: {0} failed: {1}, {2}",
                    vacancyReferenceNumber, result.Code, result.ErrorMessage);
            }
        }
    }
}