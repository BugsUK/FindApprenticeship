namespace SFA.Apprenticeships.Infrastructure.Repositories.Mongo.Employers
{
    using System;
    using System.Linq;
    using Domain.Entities.Organisations;
    using Domain.Interfaces.Repositories;
    using Common;
    using Common.Configuration;
    using Entities;
    using MongoDB.Driver.Builders;
    using MongoDB.Driver.Linq;
    using SFA.Infrastructure.Interfaces;

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

        public Employer Get(string ern)
        {
            _logger.Debug("Called Mongodb to get employer with ern={0}", ern);

            var mongoEntity = Collection.AsQueryable().SingleOrDefault(e => e.Ern == ern);

            return mongoEntity == null ? null : _mapper.Map<MongoEmployer, Employer>(mongoEntity);
        }

        public void Delete(Guid id)
        {
            _logger.Debug("Calling repository to delete employer with Id={0}", id);

            Collection.Remove(Query<MongoEmployer>.EQ(e => e.Id, id));

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
