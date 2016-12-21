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
    using MongoDB.Bson;
    using MongoDB.Driver.Builders;
    using MongoDB.Driver.Linq;

    using Application.Interfaces;

    public class VacancyOwnerRelationshipRepository : GenericMongoClient2<MongoVacancyOwnerRelationship>, IVacancyOwnerRelationshipReadRepository, IVacancyOwnerRelationshipWriteRepository
    {
        private readonly IMapper _mapper;
        private readonly ILogService _logger;

        public VacancyOwnerRelationshipRepository(IConfigurationService configurationService, IMapper mapper, ILogService logger)
        {
            var config = configurationService.Get<MongoConfiguration>();
            Initialise(config.ProvidersDb, "vacancyParties");
            _mapper = mapper;
            _logger = logger;
        }

        public VacancyOwnerRelationship GetByProviderSiteAndEmployerId(int providerSiteId, int employerId, bool liveOnly = true)
        {
            _logger.Debug("Called Mongodb to get provider site employer link with providerSiteErn={0}, edsUrn={1}", providerSiteId, employerId);

            var mongoEntity = Collection.AsQueryable().SingleOrDefault(e => e.ProviderSiteId == providerSiteId && e.EmployerId == employerId);

            return mongoEntity == null ? null : _mapper.Map<MongoVacancyOwnerRelationship, VacancyOwnerRelationship>(mongoEntity);
        }

        public IEnumerable<VacancyOwnerRelationship> GetByIds(IEnumerable<int> vacancyOwnerRelationshipIds, bool currentOnly = true)
        {
            var mongoEntities = Collection.Find(Query.In("VacancyOwnerRelationshipId", new BsonArray(vacancyOwnerRelationshipIds)));

            return mongoEntities.Select(e => _mapper.Map<MongoVacancyOwnerRelationship, VacancyOwnerRelationship>(e)).ToList(); // TODO: Should be filtering by status
        }

        public IEnumerable<VacancyOwnerRelationship> GetByProviderSiteId(int providerSiteId)
        {
            _logger.Debug("Called Mongodb to get provider site employer links for provider site with ERN={0}", providerSiteId);

            var mongoEntities = Collection.Find(Query<MongoVacancyOwnerRelationship>.EQ(e => e.ProviderSiteId, providerSiteId));

            var entities = _mapper.Map<IEnumerable<MongoVacancyOwnerRelationship>, IEnumerable<VacancyOwnerRelationship>>(mongoEntities).ToList();

            _logger.Debug("Found {1} provider site employer links for provider site with ERN={0}", providerSiteId, entities.Count);

            return entities;
        }

        public bool IsADeletedVacancyOwnerRelationship(int providerSiteId, int employerId)
        {
            throw new NotImplementedException();
        }

        public VacancyOwnerRelationship Save(VacancyOwnerRelationship vacancyOwnerRelationship)
        {
            _logger.Debug("Called Mongodb to save provider site employer link with ERN={0}", vacancyOwnerRelationship.EmployerId);

            if (vacancyOwnerRelationship.VacancyOwnerRelationshipGuid == Guid.Empty)
            {
                vacancyOwnerRelationship.VacancyOwnerRelationshipGuid = Guid.NewGuid();
                vacancyOwnerRelationship.VacancyOwnerRelationshipId = vacancyOwnerRelationship.VacancyOwnerRelationshipGuid.GetHashCode();
            }

            var mongoEntity = _mapper.Map<VacancyOwnerRelationship, MongoVacancyOwnerRelationship>(vacancyOwnerRelationship);

            Collection.Save(mongoEntity);

            _logger.Debug("Saved provider site employer link to Mongodb with ERN={0}", vacancyOwnerRelationship.EmployerId);

            return _mapper.Map<MongoVacancyOwnerRelationship, VacancyOwnerRelationship>(mongoEntity);
        }

        public void ResurrectVacancyOwnerRelationship(int providerSiteId, int employerId)
        {
            throw new NotImplementedException();
        }
    }
}