namespace SFA.Apprenticeships.Infrastructure.Repositories.Providers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Application.Interfaces.Logging;
    using Domain.Entities.Providers;
    using Domain.Interfaces.Configuration;
    using Domain.Interfaces.Mapping;
    using Domain.Interfaces.Repositories;
    using Entities;
    using Mongo.Common;
    using Mongo.Common.Configuration;
    using MongoDB.Driver.Builders;

    public class ProviderSiteRepository : GenericMongoClient<MongoProviderSite>, IProviderSiteReadRepository, IProviderSiteWriteRepository
    {
        private readonly IMapper _mapper;
        private readonly ILogService _logger;

        public ProviderSiteRepository(IConfigurationService configurationService, IMapper mapper, ILogService logger)
        {
            var config = configurationService.Get<MongoConfiguration>();
            Initialise(config.ProvidersDb, "providerSites");
            _mapper = mapper;
            _logger = logger;
        }

        public ProviderSite Get(Guid id)
        {
            _logger.Debug("Called Mongodb to get provider site with Id={0}", id);

            var mongoEntity = Collection.FindOneById(id);

            return mongoEntity == null ? null : _mapper.Map<MongoProviderSite, ProviderSite>(mongoEntity);
        }

        public IEnumerable<ProviderSite> GetForProvider(string ukprn)
        {
            _logger.Debug("Called Mongodb to get provider sites for provider with UKPRN={0}", ukprn);

            var mongoEntities = Collection.Find(Query<MongoProviderSite>.EQ(e => e.Ukprn, ukprn));

            var entities =
                _mapper.Map<IEnumerable<MongoProviderSite>, IEnumerable<ProviderSite>>(mongoEntities).ToList();

            _logger.Debug("Found {1} provider sites for provider with UKPRN={0}", ukprn, entities.Count);

            return entities;
        }

        public void Delete(Guid id)
        {
            _logger.Debug("Calling repository to delete provider site with Id={0}", id);

            Collection.Remove(Query<MongoProviderSite>.EQ(o => o.Id, id));

            _logger.Debug("Deleted provider with Id={0}", id);
        }

        public ProviderSite Save(ProviderSite entity)
        {
            _logger.Debug("Called Mongodb to save provider site for provider with UKPRN={0}", entity.Ukprn);

            UpdateEntityTimestamps(entity);

            var mongoEntity = _mapper.Map<ProviderSite, MongoProviderSite>(entity);

            Collection.Save(mongoEntity);

            _logger.Debug("Saved provider site to Mongodb for provider with UKPRN={0}", entity.Ukprn);

            return _mapper.Map<MongoProviderSite, ProviderSite>(mongoEntity);
        }
    }
}