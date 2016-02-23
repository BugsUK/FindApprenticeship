namespace SFA.Apprenticeships.Infrastructure.Repositories.Mongo.Providers
{
    using Domain.Entities.Raa.Parties;
    using Domain.Raa.Interfaces.Repositories;
    using Common;
    using Common.Configuration;
    using Entities;
    using MongoDB.Driver.Builders;
    using SFA.Infrastructure.Interfaces;

    public class ProviderRepository : GenericMongoClient2<MongoProvider>, IProviderReadRepository, IProviderWriteRepository
    {
        private readonly IMapper _mapper;
        private readonly ILogService _logger;

        public ProviderRepository(IConfigurationService configurationService, IMapper mapper, ILogService logger)
        {
            var config = configurationService.Get<MongoConfiguration>();
            Initialise(config.ProvidersDb, "providers");
            _mapper = mapper;
            _logger = logger;
        }

        public Provider Get(int providerId)
        {
            _logger.Debug("Called Mongodb to get provider with Id={0}", providerId);

            var mongoEntity = Collection.FindOne(Query<MongoProvider>.EQ(e => e.ProviderId, providerId));

            return mongoEntity == null ? null : _mapper.Map<MongoProvider, Provider>(mongoEntity);
        }

        public Provider GetViaUkprn(string ukprn)
        {
            _logger.Debug("Called Mongodb to get provider with UKPRN={0}", ukprn);

            var mongoEntity = Collection.FindOne(Query<MongoProvider>.EQ(e => e.Ukprn, ukprn));

            return mongoEntity == null ? null : _mapper.Map<MongoProvider, Provider>(mongoEntity);
        }

        public void Delete(int providerId)
        {
            _logger.Debug("Calling repository to delete provider with Id={0}", providerId);

            Collection.Remove(Query<MongoProvider>.EQ(o => o.ProviderId, providerId));

            _logger.Debug("Deleted provider with Id={0}", providerId);
        }

        public Provider Save(Provider entity)
        {
            _logger.Debug("Called Mongodb to save provider with UKPRN={0}", entity.Ukprn);

            SetCreatedDateTime(entity);
            SetUpdatedDateTime(entity);

            var mongoEntity = _mapper.Map<Provider, MongoProvider>(entity);

            Collection.Save(mongoEntity);

            _logger.Debug("Saved provider to Mongodb with UKPRN={0}", entity.Ukprn);

            return _mapper.Map<MongoProvider, Provider>(mongoEntity);
        }
    }
}
