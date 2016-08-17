namespace SFA.Apprenticeships.Infrastructure.Repositories.Mongo.Users
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Domain.Entities.Exceptions;
    using Domain.Entities.Users;
    using Domain.Interfaces.Repositories;
    using Common;
    using Common.Configuration;
    using Entities;
    using MongoDB.Driver.Builders;
    using MongoDB.Driver.Linq;

    using SFA.Apprenticeships.Application.Interfaces;
    using SFA.Infrastructure.Interfaces;

    public class UserRepository : GenericMongoClient<MongoUser>, IUserReadRepository, IUserWriteRepository
    {
        private readonly ILogService _logger;
        private readonly IMapper _mapper;

        public UserRepository(IConfigurationService configurationService, IMapper mapper, ILogService logger)
        {
            var config = configurationService.Get<MongoConfiguration>();
            Initialise(config.UsersDb, "users");
            _mapper = mapper;
            _logger = logger;
        }

        public User Get(Guid id)
        {
            _logger.Debug("Called Mongodb to get user with Id={0}", id);

            //Note that setting a user to pending deletion doesn't prevent it from being retrieved.
            //Code futher up the stack should perform the neccessary logic checks
            var mongoEntity = Collection.FindOneById(id);

            return mongoEntity == null ? null : _mapper.Map<MongoUser, User>(mongoEntity);
        }

        public User Get(string username, bool errorIfNotFound = true)
        {
            _logger.Debug("Called Mongodb to get user with username={0}", username);

            //Note that setting a user to pending deletion doesn't prevent it from being retrieved.
            //Code futher up the stack should perform the necessary logic checks
            var mongoEntity = Collection.Find(Query.EQ("Username", username.ToLower())).OrderBy(u => u.Status).FirstOrDefault();

            if (mongoEntity == null && errorIfNotFound)
            {
                var message = string.Format("Unknown username={0}", username);
                _logger.Debug(message, username);

                throw new CustomException(message, Application.Interfaces.Users.ErrorCodes.UnknownUserError);
            }

            return mongoEntity == null ? null : _mapper.Map<MongoUser, User>(mongoEntity);
        }

        public IEnumerable<Guid> GetUsersWithStatus(UserStatuses[] userStatuses)
        {
            _logger.Debug("Calling repository to get all the users with the following statuses: {0}", string.Join(", ", userStatuses));

            var userIds = Collection.AsQueryable().Where(u => userStatuses.Contains(u.Status)).Select(u => u.Id);

            _logger.Debug("Called repository to get all the users with the following statuses: {0}", string.Join(", ", userStatuses));

            return userIds;
        }

        public IEnumerable<Guid> GetPotentiallyDormantUsers(DateTime lastValidLogin)
        {
            _logger.Debug("Calling repository to get all the users which could be dormant");

            var userStatuses = new[] {UserStatuses.Active, UserStatuses.Locked};

            var userIds = Collection.AsQueryable().Where(u => userStatuses.Contains(u.Status) && (u.LastLogin == null || u.LastLogin <= lastValidLogin)).Select(u => u.Id);

            _logger.Debug("Called repository to get all the users which could be dormant");

            return userIds;
        }

        public IEnumerable<Guid> GetDormantUsersPotentiallyEligibleForSoftDelete(DateTime dormantAfterDateTime)
        {
            _logger.Debug("Calling repository to get all the users which are dormant and could be eligible for soft delete");

            var userIds = Collection.AsQueryable().Where(u => u.Status == UserStatuses.Dormant && (u.LastLogin == null || u.LastLogin <= dormantAfterDateTime)).Select(u => u.Id);

            _logger.Debug("Called repository to get all the users which are dormant and could be eligible for soft delete");

            return userIds;
        }

        public void Delete(Guid id)
        {
            _logger.Debug("Calling repository to delete MongoUser with Id={0}", id);

            Collection.Remove(Query<MongoUser>.EQ(o => o.Id, id));

            _logger.Debug("Deleted MongoUser with Id={0}", id);
        }

        public User Save(User entity)
        {
            _logger.Debug("Called Mongodb to save user with username={0}", entity.Username);

            UpdateEntityTimestamps(entity);

            var mongoEntity = _mapper.Map<User, MongoUser>(entity);

            Collection.Save(mongoEntity);

            _logger.Debug("Saved User to Mongodb with username={0}", entity.Username);

            return _mapper.Map<MongoUser, User>(mongoEntity);
        }
    }
}