namespace SFA.Apprenticeships.Infrastructure.Repositories.Sql.Schemas.Provider
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Common;
    using Domain.Entities.Users;
    using Domain.Interfaces.Repositories;
    using SFA.Infrastructure.Interfaces;

    // TODO: SQL: AG: double check logging.

    public class ProviderUserRepository : IProviderUserReadRepository, IProviderUserWriteRepository
    {
        private readonly IGetOpenConnection _getOpenConnection;
        private readonly IMapper _mapper;
        private readonly ILogService _logger;

        public ProviderUserRepository(IGetOpenConnection getOpenConnection, IMapper mapper, ILogService logger)
        {
            _getOpenConnection = getOpenConnection;
            _mapper = mapper;
            _logger = logger;
        }

        public ProviderUser Get(int id)
        {
            _logger.Debug("Calling database to get provider user with Id={0}", id);

            const string sql = "SELECT * FROM Provider.ProviderUser WHERE ProviderUserId = @ProviderUserId";

            var sqlParams = new
            {
                ProviderUserId = id
            };

            var dbProviderUser = _getOpenConnection
                .Query<Entities.ProviderUser>(sql, sqlParams)
                .SingleOrDefault();

            return MapProviderUser(dbProviderUser);
        }

        public ProviderUser Get(string username)
        {
            _logger.Debug("Calling database to get provider user with username={0}", username);

            const string sql = "SELECT * FROM Provider.ProviderUser WHERE Username = @username";

            var sqlParams = new
            {
                username
            };

            var dbProviderUser = _getOpenConnection
                .Query<Entities.ProviderUser>(sql, sqlParams)
                .SingleOrDefault();

            return MapProviderUser(dbProviderUser);
        }

        public IEnumerable<ProviderUser> GetForProvider(string ukprn)
        {
            _logger.Debug("Calling database to get provider users for provider with Ukprn={0}", ukprn);

            const string sql = "SELECT pu.* FROM Provider.ProviderUser pu INNER JOIN dbo.Provider p ON p.ProviderId = pu.ProviderId WHERE p.Ukprn = @ukprn AND p.ProviderStatusTypeId = @providerStatusTypeId";

            var sqlParams = new
            {
                ukprn,
                providerStatusTypeId = ProviderStatus.Activated
            };

            var providerUsers = _getOpenConnection
                .Query<Entities.ProviderUser>(sql, sqlParams)
                .Select(MapProviderUser);

            return providerUsers;
        }

        public ProviderUser Save(ProviderUser providerUser)
        {
            _logger.Debug("Called SQL DB to save ProviderUser with Id={0}", providerUser.ProviderUserId);

            UpdateEntityTimestamps(providerUser);

            var dbProviderUser = MapProviderUser(providerUser);

            try
            {
                dbProviderUser.ProviderUserId = (int)_getOpenConnection.Insert(dbProviderUser);
            }
            catch (Exception e)
            {
                // TODO: SQL: AG: need to talk about Save(). Can we get an upsert rather than throw/catch?
                if (!_getOpenConnection.UpdateSingle(dbProviderUser))
                {
                    throw new Exception("Failed to update record after failed insert", e);
                }
            }

            _logger.Debug("Saved ProviderUser to SQL DB with ProviderUserId={0}", providerUser.ProviderUserId);

            return MapProviderUser(dbProviderUser);
        }

        #region Helpers

        private ProviderUser MapProviderUser(Entities.ProviderUser dbProviderUser)
        {
            return _mapper.Map<Entities.ProviderUser, ProviderUser>(dbProviderUser);
        }

        // TODO: SQL: AG: use or lose dead code.

        private Entities.ProviderUser MapProviderUser(ProviderUser providerUser)
        {
            return _mapper.Map<ProviderUser, Entities.ProviderUser>(providerUser);
        }

        private void UpdateEntityTimestamps(ProviderUser entity)
        {
            // determine whether this is a "new" entity being saved for the first time
            if (entity.DateCreated == DateTime.MinValue)
            {
                entity.DateCreated = DateTime.UtcNow;
                entity.DateUpdated = null;
            }
            else
            {
                entity.DateUpdated = DateTime.UtcNow;
            }
        }

        #endregion
    }
}
