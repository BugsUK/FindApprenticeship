namespace SFA.Apprenticeships.Infrastructure.Repositories.UserProfiles
{
    using System;
    using Application.Interfaces.Logging;
    using Domain.Entities.Users;
    using Domain.Interfaces.Configuration;
    using Domain.Interfaces.Mapping;
    using Domain.Interfaces.Repositories;
    using Entities;
    using Mongo.Common;
    using Mongo.Common.Configuration;
    using MongoDB.Driver.Builders;

    public class AgencyUserRepository : GenericMongoClient<MongoAgencyUser>, IAgencyUserReadRepository, IAgencyUserWriteRepository
    {
        private readonly IMapper _mapper;
        private readonly ILogService _logger;

        public AgencyUserRepository(IConfigurationService configurationService, IMapper mapper, ILogService logger)
        {
            var config = configurationService.Get<MongoConfiguration>();
            Initialise(config.UserProfilesDb, "agencyUsers");
            _mapper = mapper;
            _logger = logger;
        }

        public AgencyUser Get(Guid id)
        {
            _logger.Debug("Called Mongodb to get agency user with Id={0}", id);

            var mongoEntity = Collection.FindOneById(id);

            return mongoEntity == null ? null : _mapper.Map<MongoAgencyUser, AgencyUser>(mongoEntity);
        }

        public AgencyUser Get(string username)
        {
            _logger.Debug("Called Mongodb to get agency user with Username={0}", username);

            var mongoEntity = Collection.FindOne(Query<MongoAgencyUser>.EQ(e => e.Username, username));

            return mongoEntity == null ? null : _mapper.Map<MongoAgencyUser, AgencyUser>(mongoEntity);
        }

        public void Delete(Guid id)
        {
            _logger.Debug("Calling repository to delete agency user with Id={0}", id);

            Collection.Remove(Query<MongoAgencyUser>.EQ(o => o.Id, id));

            _logger.Debug("Deleted agency user with Id={0}", id);
        }

        public AgencyUser Save(AgencyUser entity)
        {
            _logger.Debug("Called Mongodb to save agency user with username={0}", entity.Username);

            UpdateEntityTimestamps(entity);

            var mongoEntity = _mapper.Map<AgencyUser, MongoAgencyUser>(entity);

            Collection.Save(mongoEntity);

            _logger.Debug("Saved agency user to Mongodb with username={0}", entity.Username);

            return _mapper.Map<MongoAgencyUser, AgencyUser>(mongoEntity);
        }
    }
}