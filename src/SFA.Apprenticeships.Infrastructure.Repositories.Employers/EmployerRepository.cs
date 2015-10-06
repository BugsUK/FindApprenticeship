namespace SFA.Apprenticeships.Infrastructure.Repositories.Employers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Application.Interfaces.Logging;
    using Domain.Entities.Organisations;
    using Domain.Interfaces.Configuration;
    using Domain.Interfaces.Mapping;
    using Domain.Interfaces.Repositories;
    using Entities;
    using Mongo.Common;
    using Mongo.Common.Configuration;
    using MongoDB.Driver.Builders;

    public class EmployerRepository : GenericMongoClient<MongoEmployer>, IEmployerReadRepository, IEmployerWriteRepository
    {
        private readonly IMapper _mapper;
        private readonly ILogService _logger;

        public EmployerRepository(IConfigurationService configurationService, IMapper mapper, ILogService logger)
        {
            var config = configurationService.Get<MongoConfiguration>();
            Initialise(config.EmployersDb, "employers");
            _mapper = mapper;
            _logger = logger;
        }

        public Employer Get(Guid id)
        {
            _logger.Debug("Called Mongodb to get employer with Id={0}", id);

            var mongoEntity = Collection.FindOneById(id);

            return mongoEntity == null ? null : _mapper.Map<MongoEmployer, Employer>(mongoEntity);
        }

        public IEnumerable<Employer> GetForProviderSite(string ern)
        {
            _logger.Debug("Called Mongodb to get employers for provider site with ERN={0}", ern);

            var mongoEntities = Collection.Find(Query<MongoEmployer>.EQ(e => e.ProviderSiteErn, ern));

            var entities = _mapper.Map<IEnumerable<MongoEmployer>, IEnumerable<Employer>>(mongoEntities).ToList();

            _logger.Debug("Found {1} employers for provider site with ERN={0}", ern, entities.Count);

            return entities;
        }

        public void Delete(Guid id)
        {
            _logger.Debug("Calling repository to delete employer with Id={0}", id);

            Collection.Remove(Query<MongoEmployer>.EQ(o => o.Id, id));

            _logger.Debug("Deleted employer with Id={0}", id);
        }

        public Employer Save(Employer entity)
        {
            _logger.Debug("Called Mongodb to save employer with ERN={0}", entity.Ern);

            UpdateEntityTimestamps(entity);

            var mongoEntity = _mapper.Map<Employer, MongoEmployer>(entity);

            Collection.Save(mongoEntity);

            _logger.Debug("Saved employer to Mongodb with ERN={0}", entity.Ern);

            return _mapper.Map<MongoEmployer, Employer>(mongoEntity);
        }
    }
}
