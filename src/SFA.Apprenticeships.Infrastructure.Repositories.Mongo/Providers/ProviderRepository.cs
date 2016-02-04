namespace SFA.Apprenticeships.Infrastructure.Repositories.Mongo.Providers
{
    using System;
    using Domain.Entities.Providers;
    using Domain.Interfaces.Repositories;
    using Mongo.Common;
    using Mongo.Common.Configuration;
    using Mongo.Providers.Entities;
    using MongoDB.Driver.Builders;
    using SFA.Infrastructure.Interfaces;

    public class ProviderRepository : GenericMongoClient<MongoProvider, Guid>, IProviderReadRepository, IProviderWriteRepository
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

        public Provider Get(Guid id)
        {
            _logger.Debug("Called Mongodb to get provider with Id={0}", id);

            var mongoEntity = Collection.FindOneById(id);

            return mongoEntity == null ? null : _mapper.Map<MongoProvider, Provider>(mongoEntity);
        }

        public Provider Get(string ukprn)
        {
            _logger.Debug("Called Mongodb to get provider with UKPRN={0}", ukprn);

            var mongoEntity = Collection.FindOne(Query<MongoProvider>.EQ(e => e.Ukprn, ukprn));

            return mongoEntity == null ? null : _mapper.Map<MongoProvider, Provider>(mongoEntity);
        }

        public void Delete(Guid id)
        {
            _logger.Debug("Calling repository to delete provider with Id={0}", id);

            Collection.Remove(Query<MongoProvider>.EQ(o => o.Id, id));

            _logger.Debug("Deleted provider with Id={0}", id);
        }

        public Provider Save(Provider entity)
        {
            _logger.Debug("Called Mongodb to save provider with UKPRN={0}", entity.Ukprn);

            UpdateEntityTimestamps(entity);

            var mongoEntity = _mapper.Map<Provider, MongoProvider>(entity);

            Collection.Save(mongoEntity);

            _logger.Debug("Saved provider to Mongodb with UKPRN={0}", entity.Ukprn);

            return _mapper.Map<MongoProvider, Provider>(mongoEntity);
        }
    }
}
