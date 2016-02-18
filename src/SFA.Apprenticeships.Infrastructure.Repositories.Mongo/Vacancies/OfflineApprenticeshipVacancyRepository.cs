namespace SFA.Apprenticeships.Infrastructure.Repositories.Mongo.Vacancies
{
    using Common;
    using Common.Configuration;
    using Domain.Entities.Raa.Vacancies;
    using Domain.Raa.Interfaces.Repositories;
    using Entities;
    using MongoDB.Driver;
    using MongoDB.Driver.Builders;
    using SFA.Infrastructure.Interfaces;

    public class OfflineApprenticeshipVacancyRepository : GenericMongoClient2<MongoApprenticeshipVacancy>, IOfflineApprenticeshipVacancyRepository
    {
        private readonly IMapper _mapper;
        private readonly ILogService _logger;

        public OfflineApprenticeshipVacancyRepository(IConfigurationService configurationService, IMapper mapper, ILogService logger)
        {
            var config = configurationService.Get<MongoConfiguration>();

            Initialise(config.VacancyDb, "vacancies");

            _mapper = mapper;
            _logger = logger;
        }

        public void IncrementOfflineApplicationClickThrough(long vacancyReferenceNumber)
        {
            _logger.Debug("Calling Mongodb to increment the OfflineApplicationClickThroughCount property by one for vacancy with reference number: {0}", vacancyReferenceNumber);

            var args = new FindAndModifyArgs
            {
                Query = new QueryBuilder<Vacancy>().And(Query<Vacancy>.EQ(d => d.VacancyReferenceNumber, vacancyReferenceNumber), Query<Vacancy>.EQ(d => d.OfflineVacancy, true)),
                Update = Update.Inc("OfflineApplicationClickThroughCount", 1),
                VersionReturned = FindAndModifyDocumentVersion.Modified
            };

            var result = Collection.FindAndModify(args);

            if (result.Ok)
            {
                var mongoEntity = result.GetModifiedDocumentAs<MongoApprenticeshipVacancy>();

                var entity = _mapper.Map<MongoApprenticeshipVacancy, Vacancy>(mongoEntity);

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