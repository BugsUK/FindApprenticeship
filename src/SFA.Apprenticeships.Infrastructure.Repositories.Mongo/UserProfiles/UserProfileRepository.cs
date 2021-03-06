﻿namespace SFA.Apprenticeships.Infrastructure.Repositories.Mongo.UserProfiles
{
    using System;
    using Common;
    using Common.Configuration;
    using Domain.Entities.Raa.Users;
    using Entities;
    using MongoDB.Driver.Builders;

    using SFA.Apprenticeships.Application.Interfaces;

    public class UserProfileRepository : GenericMongoClient2<MongoProviderUser>//, IProviderUserReadRepository, IProviderUserWriteRepository
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

        public ProviderUser Get(int providerUserId)
        {
            _logger.Debug("Called Mongodb to get provider user with Id={0}", providerUserId);

            var mongoEntity = Collection.FindOneById(providerUserId);

            return mongoEntity == null ? null : _mapper.Map<MongoProviderUser, ProviderUser>(mongoEntity);
        }

        public ProviderUser Get(string username)
        {
            _logger.Debug("Called Mongodb to get provider user with Username={0}", username);

            var mongoEntity = Collection.FindOne(Query<MongoProviderUser>.EQ(e => e.Username, username));

            return mongoEntity == null ? null : _mapper.Map<MongoProviderUser, ProviderUser>(mongoEntity);
        }

        //public IEnumerable<ProviderUser> GetForProvider(string ukprn)
        //{
        //    _logger.Debug("Called Mongodb to get provider users for provider with UKPRN={0}", ukprn);

        //    var mongoEntities = Collection.Find(Query<MongoProviderUser>.EQ(e => e.Ukprn, ukprn));

        //    var entities =
        //        _mapper.Map<IEnumerable<MongoProviderUser>, IEnumerable<ProviderUser>>(mongoEntities).ToList();

        //    _logger.Debug("Found {1} provider users for provider with UKPRN={0}", ukprn, entities.Count);

        //    return entities;
        //}

        public ProviderUser Save(ProviderUser entity)
        {
            _logger.Debug("Called Mongodb to save provider user with username={0}", entity.Username);

            if (entity.ProviderUserGuid == Guid.Empty)
            {
                entity.ProviderUserGuid = Guid.NewGuid();
                entity.ProviderUserId = entity.ProviderUserGuid.GetHashCode();
            }

            SetCreatedDateTime(entity);
            SetUpdatedDateTime(entity);

            var mongoEntity = _mapper.Map<ProviderUser, MongoProviderUser>(entity);

            Collection.Save(mongoEntity);

            _logger.Debug("Saved provider user to Mongodb with username={0}", entity.Username);

            return _mapper.Map<MongoProviderUser, ProviderUser>(mongoEntity);
        }
    }
}
