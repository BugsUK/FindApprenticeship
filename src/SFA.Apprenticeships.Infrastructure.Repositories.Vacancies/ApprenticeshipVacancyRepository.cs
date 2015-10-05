namespace SFA.Apprenticeships.Infrastructure.Repositories.Vacancies
{
    using System;
    using Application.Interfaces.Logging;
    using Domain.Entities.Vacancies.ProviderVacancies.Apprenticeship;
    using Domain.Interfaces.Configuration;
    using Domain.Interfaces.Mapping;
    using Domain.Interfaces.Repositories;
    using Mongo.Common;
    using Mongo.Common.Configuration;
    using MongoDB.Driver.Builders;
    using Entities;

    public class ApprenticeshipVacancyRepository : GenericMongoClient<MongoApprenticeshipVacancy>, IApprenticeshipVacancyReadRepository, IApprenticeshipVacancyWriteRepository
    {
        private readonly IMapper _mapper;
        private readonly ILogService _logger;

        public ApprenticeshipVacancyRepository(
            IConfigurationService configurationService,
            IMapper mapper,
            ILogService logger)
        {
            var config = configurationService.Get<MongoConfiguration>();

            Initialise(config.VacancyDb, "apprenticeshipVacancies");

            _mapper = mapper;
            _logger = logger;
        }

        public ApprenticeshipVacancy Get(Guid id)
        {
            _logger.Debug("Called Mongodb to get apprenticeship vacancy with Id={0}", id);

            var mongoEntity = Collection.FindOneById(id);

            return mongoEntity == null ? null : _mapper.Map<MongoApprenticeshipVacancy, ApprenticeshipVacancy>(mongoEntity);
        }

        public void Delete(Guid id)
        {
            _logger.Debug("Calling repository to delete apprenticeship vacancy with Id={0}", id);

            Collection.Remove(Query<MongoApprenticeshipVacancy>.EQ(o => o.Id, id));

            _logger.Debug("Deleted apprenticeship vacancy with Id={0}", id);
        }

        public ApprenticeshipVacancy Save(ApprenticeshipVacancy entity)
        {
            _logger.Debug("Called Mongodb to save apprenticeship vacancy with id={0}", entity.EntityId);

            UpdateEntityTimestamps(entity);

            var mongoEntity = _mapper.Map<ApprenticeshipVacancy, MongoApprenticeshipVacancy>(entity);

            Collection.Save(mongoEntity);

            _logger.Debug("Saved apprenticeship vacancy with to Mongodb with id={0}", entity.EntityId);

            return _mapper.Map<MongoApprenticeshipVacancy, ApprenticeshipVacancy>(mongoEntity);
        }
    }
}
