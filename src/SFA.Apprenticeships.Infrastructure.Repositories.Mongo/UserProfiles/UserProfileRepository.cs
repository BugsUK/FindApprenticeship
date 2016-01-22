namespace SFA.Apprenticeships.Infrastructure.Repositories.Mongo.UserProfiles
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Domain.Entities.Users;
    using Domain.Interfaces.Repositories;
    using Common;
    using Common.Configuration;
    using Entities;
    using MongoDB.Driver.Builders;
    using SFA.Infrastructure.Interfaces;

    public class UserProfileRepository : GenericMongoClient<MongoProviderUser>, IProviderUserReadRepository, IProviderUserWriteRepository
    {
        private readonly IMapper _mapper;
        private readonly ILogService _logger;

        public UserProfileRepository(IConfigurationService configurationService, IMapper mapper, ILogService logger)
        {
            var config = configurationService.Get<MongoConfiguration>();
            Initialise(config.UserProfilesDb, "providerUsers");
            _mapper = mapper;
            _logger = logger;
        }

        public ProviderUser Get(Guid id)
        {
            _logger.Debug("Called Mongodb to get provider user with Id={0}", id);

            var mongoEntity = Collection.FindOneById(id);

            return mongoEntity == null ? null : _mapper.Map<MongoProviderUser, ProviderUser>(mongoEntity);
        }

        public ProviderUser Get(string username)
        {
            _logger.Debug("Called Mongodb to get provider user with Username={0}", username);

            var mongoEntity = Collection.FindOne(Query<MongoProviderUser>.EQ(e => e.Username, username));

            return mongoEntity == null ? null : _mapper.Map<MongoProviderUser, ProviderUser>(mongoEntity);
        }

        public IEnumerable<ProviderUser> GetForProvider(string ukprn)
        {
            _logger.Debug("Called Mongodb to get provider users for provider with UKPRN={0}", ukprn);

            var mongoEntities = Collection.Find(Query<MongoProviderUser>.EQ(e => e.Ukprn, ukprn));

            var entities =
                _mapper.Map<IEnumerable<MongoProviderUser>, IEnumerable<ProviderUser>>(mongoEntities).ToList();

            _logger.Debug("Found {1} provider users for provider with UKPRN={0}", ukprn, entities.Count);

            return entities;
        }

        public void Delete(Guid id)
        {
            _logger.Debug("Calling repository to delete provider user with Id={0}", id);

            Collection.Remove(Query<MongoProviderUser>.EQ(o => o.Id, id));

            _logger.Debug("Deleted provider user with Id={0}", id);
        }

        public ProviderUser Save(ProviderUser entity)
        {
            _logger.Debug("Called Mongodb to save provider user with username={0}", entity.Username);

            UpdateEntityTimestamps(entity);

            var mongoEntity = _mapper.Map<ProviderUser, MongoProviderUser>(entity);

            Collection.Save(mongoEntity);

            _logger.Debug("Saved provider user to Mongodb with username={0}", entity.Username);

            return _mapper.Map<MongoProviderUser, ProviderUser>(mongoEntity);
        }
    }
}
