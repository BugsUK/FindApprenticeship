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

    public class VacancyLocationAddressRepository : GenericMongoClient2<MongoVacancyLocationAddress>, IVacancyLocationAddressReadRepository, IVacancyLocationAddressWriteRepository
    {
        private readonly IMapper _mapper;
        private readonly ILogService _logger;

        public VacancyLocationAddressRepository(IConfigurationService configurationService, IMapper mapper, ILogService logger)
        {
            var config = configurationService.Get<MongoConfiguration>();

            Initialise(config.VacancyDb, "vacancyLocationAddresses");

            _mapper = mapper;
            _logger = logger;
        }

        public List<VacancyLocationAddress> GetForVacancyId(int vacancyId)
        {
            _logger.Debug("Called Mongodb to get apprenticeship vacancy with Id={0}", vacancyId);

            var mongoEntity = Collection.Find(Query<MongoVacancyLocationAddress>.EQ(v => v.VacancyId, vacancyId));

            return mongoEntity.Select(e => _mapper.Map<MongoVacancyLocationAddress, VacancyLocationAddress>(e)).ToList();
        }

        public List<VacancyLocationAddress> Save(List<VacancyLocationAddress> locationAddresses)
        {
            return locationAddresses.Select(SaveEntity).Select(e => _mapper.Map<MongoVacancyLocationAddress, VacancyLocationAddress>(e)).ToList();
        }

        private MongoVacancyLocationAddress SaveEntity(VacancyLocationAddress entity)
        {
            SetCreatedDateTime(entity);
            SetUpdatedDateTime(entity);

            var mongoEntity = _mapper.Map<VacancyLocationAddress, MongoVacancyLocationAddress>(entity);

            Collection.Save(mongoEntity);
            return mongoEntity;
        }
    }
}