namespace SFA.Apprenticeships.Infrastructure.Repositories.Mongo.Providers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Common;
    using Common.Configuration;
    using Domain.Entities.Raa.Parties;
    using Domain.Raa.Interfaces.Repositories;
    using Entities;
    using MongoDB.Driver.Builders;
    using SFA.Infrastructure.Interfaces;

    public class ProviderSiteRepository : GenericMongoClient2<MongoProviderSite>, IProviderSiteReadRepository, IProviderSiteWriteRepository
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

        public ProviderSite Get(int providerSiteId)
        {
            _logger.Debug("Called Mongodb to get provider site with Id={0}", providerSiteId);

            var mongoEntity = Collection.FindOne(Query<ProviderSite>.EQ(ps => ps.ProviderSiteId, providerSiteId));

            return mongoEntity == null ? null : _mapper.Map<MongoProviderSite, ProviderSite>(mongoEntity);
        }

        public ProviderSite GetByEdsErn(string edsErn)
        {
            _logger.Debug("Called Mongodb to get provider site with ERN={0}", edsErn);

            var mongoEntity = Collection.FindOne(Query<ProviderSite>.EQ(ps => ps.EdsErn, edsErn));

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

        public void Delete(int providerSiteId)
        {
            _logger.Debug("Calling repository to delete provider site with Id={0}", providerSiteId);

            Collection.Remove(Query<MongoProviderSite>.EQ(o => o.ProviderSiteId, providerSiteId));

            _logger.Debug("Deleted provider with Id={0}", providerSiteId);
        }

        public ProviderSite Save(ProviderSite entity)
        {
            _logger.Debug("Called Mongodb to save provider site for provider with UKPRN={0}", entity.Ukprn);

            SetCreatedDateTime(entity);
            SetUpdatedDateTime(entity);

            var mongoEntity = _mapper.Map<ProviderSite, MongoProviderSite>(entity);

            Collection.Save(mongoEntity);

            _logger.Debug("Saved provider site to Mongodb for provider with UKPRN={0}", entity.Ukprn);

            return _mapper.Map<MongoProviderSite, ProviderSite>(mongoEntity);
        }
    }
}