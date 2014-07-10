﻿namespace SFA.Apprenticeships.Infrastructure.Repositories.Users
{
    using System;
    using Common.Configuration;
    using Domain.Entities.Users;
    using Domain.Interfaces.Mapping;
    using Domain.Interfaces.Repositories;
    using Entities;
    using Mongo.Common;
    using MongoDB.Driver.Builders;

    public class UserRepository : GenericMongoRepository<MongoUser>, IUserReadRepository, IUserWriteRepository
    {
        private readonly IMapper _mapper;

        protected UserRepository(IConfigurationManager configurationManager, IMapper mapper)
            : base(configurationManager, "Users.mongoDB", "users")
        {
            _mapper = mapper;
        }

        public User Get(Guid id)
        {
            var mongoEntity = Collection.FindOneById(id);

            return _mapper.Map<MongoUser, User>(mongoEntity);
        }

        public User Get(string username, bool errorIfNotFound = true)
        {
            var mongoEntity = Collection.FindOne(Query.EQ("username", username));

            if (mongoEntity == null && errorIfNotFound) 
                throw new Exception("Unknown user name"); //todo: should use an application exception type

            return mongoEntity == null ? null : _mapper.Map<MongoUser, User>(mongoEntity);
        }

        public void Delete(Guid id)
        {
            throw new NotImplementedException();
        }

        public User Save(User entity)
        {
            //if (entity.IsNew())
            //{
            //    //todo: ensure unique (username) if new or unactivated: cannot have >1 activated or >1 unactivated but can have one of each
            //}

            throw new NotImplementedException();
        }
    }
}
