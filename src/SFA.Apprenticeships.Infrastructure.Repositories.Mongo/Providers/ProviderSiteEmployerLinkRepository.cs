namespace SFA.Apprenticeships.Infrastructure.Repositories.Mongo.Providers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Domain.Entities.Providers;
    using Domain.Interfaces.Repositories;
    using Mongo.Common;
    using Mongo.Common.Configuration;
    using Mongo.Providers.Entities;
    using MongoDB.Driver.Builders;
    using MongoDB.Driver.Linq;
    using SFA.Infrastructure.Interfaces;

    public class ProviderSiteEmployerLinkRepository : GenericMongoClient<MongoProviderSiteEmployerLink, Guid>, IProviderSiteEmployerLinkReadRepository, IProviderSiteEmployerLinkWriteRepository
    {
        private readonly IMapper _mapper;
        private readonly ILogService _logger;

        public ProviderSiteEmployerLinkRepository(IConfigurationService configurationService, IMapper mapper, ILogService logger)
        {
            var config = configurationService.Get<MongoConfiguration>();
            Initialise(config.ProvidersDb, "providerSiteEmployerLinks");
            _mapper = mapper;
            _logger = logger;
        }

        public ProviderSiteEmployerLink Get(Guid id)
        {
            _logger.Debug("Called Mongodb to get provider site employer link with Id={0}", id);

            var mongoEntity = Collection.FindOneById(id);

            return mongoEntity == null ? null : _mapper.Map<MongoProviderSiteEmployerLink, ProviderSiteEmployerLink>(mongoEntity);
        }

        public ProviderSiteEmployerLink Get(string providerSiteErn, string ern)
        {
            _logger.Debug("Called Mongodb to get provider site employer link with providerSiteErn={0}, ern={1}", providerSiteErn, ern);

            var mongoEntity = Collection.AsQueryable().SingleOrDefault(e => e.ProviderSiteErn == providerSiteErn && e.Employer.Ern == ern);

            return mongoEntity == null ? null : _mapper.Map<MongoProviderSiteEmployerLink, ProviderSiteEmployerLink>(mongoEntity);
        }

        public IEnumerable<ProviderSiteEmployerLink> GetForProviderSite(string providerSiteErn)
        {
            _logger.Debug("Called Mongodb to get provider site employer links for provider site with ERN={0}", providerSiteErn);

            var mongoEntities = Collection.Find(Query<MongoProviderSiteEmployerLink>.EQ(e => e.ProviderSiteErn, providerSiteErn));

            var entities = _mapper.Map<IEnumerable<MongoProviderSiteEmployerLink>, IEnumerable<ProviderSiteEmployerLink>>(mongoEntities).ToList();

            _logger.Debug("Found {1} provider site employer links for provider site with ERN={0}", providerSiteErn, entities.Count);

            return entities;
        }

        public void Delete(Guid id)
        {
            _logger.Debug("Calling repository to delete provider site employer link with Id={0}", id);

            Collection.Remove(Query<MongoProviderSiteEmployerLink>.EQ(e => e.Id, id));

            _logger.Debug("Deleted provider site employer link with Id={0}", id);
        }

        public ProviderSiteEmployerLink Save(ProviderSiteEmployerLink entity)
        {
            _logger.Debug("Called Mongodb to save provider site employer link with ERN={0}", entity.Employer.Ern);

            UpdateEntityTimestamps(entity);

            var mongoEntity = _mapper.Map<ProviderSiteEmployerLink, MongoProviderSiteEmployerLink>(entity);

            Collection.Save(mongoEntity);

            _logger.Debug("Saved provider site employer link to Mongodb with ERN={0}", entity.Employer.Ern);

            return _mapper.Map<MongoProviderSiteEmployerLink, ProviderSiteEmployerLink>(mongoEntity);
        }
    }
}