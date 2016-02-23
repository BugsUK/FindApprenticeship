namespace SFA.Apprenticeships.Infrastructure.Repositories.Mongo.Vacancies
{
    using System.Collections.Generic;
    using System.Linq;
    using Common;
    using Common.Configuration;
    using Domain.Entities.Raa.Locations;
    using Domain.Raa.Interfaces.Repositories;
    using Entities;
    using MongoDB.Driver.Builders;
    using SFA.Infrastructure.Interfaces;

    public class VacancyLocationRepository : GenericMongoClient2<MongoVacancyLocation>, IVacancyLocationReadRepository, IVacancyLocationWriteRepository
    {
        private readonly IMapper _mapper;
        private readonly ILogService _logger;

        public VacancyLocationRepository(IConfigurationService configurationService, IMapper mapper, ILogService logger)
        {
            var config = configurationService.Get<MongoConfiguration>();

            Initialise(config.VacancyDb, "vacancyLocationAddresses");

            _mapper = mapper;
            _logger = logger;
        }

        public List<VacancyLocation> GetForVacancyId(int vacancyId)
        {
            _logger.Debug("Called Mongodb to get apprenticeship vacancy with Id={0}", vacancyId);

            var mongoEntity = Collection.Find(Query<MongoVacancyLocation>.EQ(v => v.VacancyId, vacancyId));

            return mongoEntity.Select(e => _mapper.Map<MongoVacancyLocation, VacancyLocation>(e)).ToList();
        }

        public List<VacancyLocation> Save(List<VacancyLocation> locationAddresses)
        {
            return locationAddresses.Select(SaveEntity).Select(e => _mapper.Map<MongoVacancyLocation, VacancyLocation>(e)).ToList();
        }

        private MongoVacancyLocation SaveEntity(VacancyLocation entity)
        {
            SetCreatedDateTime(entity);
            SetUpdatedDateTime(entity);

            var mongoEntity = _mapper.Map<VacancyLocation, MongoVacancyLocation>(entity);

            Collection.Save(mongoEntity);
            return mongoEntity;
        }
    }
}