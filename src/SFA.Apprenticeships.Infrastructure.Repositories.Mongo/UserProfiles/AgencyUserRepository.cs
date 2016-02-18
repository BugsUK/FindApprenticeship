namespace SFA.Apprenticeships.Infrastructure.Repositories.Mongo.UserProfiles
{
    using System;
    using Domain.Entities.Raa.Users;
    using Domain.Raa.Interfaces.Repositories;
    using Common;
    using Common.Configuration;
    using Entities;
    using MongoDB.Driver.Builders;
    using SFA.Infrastructure.Interfaces;

    public class AgencyUserRepository : GenericMongoClient2<MongoAgencyUser>, IAgencyUserReadRepository, IAgencyUserWriteRepository
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

        public void Delete(int agencyUserId)
        {
            _logger.Debug("Calling repository to delete agency user with Id={0}", agencyUserId);

            Collection.Remove(Query<MongoAgencyUser>.EQ(o => o.AgencyUserId, agencyUserId));

            _logger.Debug("Deleted agency user with Id={0}", agencyUserId);
        }

        public AgencyUser Save(AgencyUser entity)
        {
            _logger.Debug("Called Mongodb to save agency user with username={0}", entity.Username);

            SetCreatedDateTime(entity);
            SetUpdatedDateTime(entity);

            var mongoEntity = _mapper.Map<AgencyUser, MongoAgencyUser>(entity);

            Collection.Save(mongoEntity);

            _logger.Debug("Saved agency user to Mongodb with username={0}", entity.Username);

            return _mapper.Map<MongoAgencyUser, AgencyUser>(mongoEntity);
        }
    }
}