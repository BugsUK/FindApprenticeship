namespace SFA.Apprenticeships.Infrastructure.Repositories.Sql.Schemas.Provider
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Common;
    using Domain.Entities.Raa.Users;
    using Domain.Entities.Users;
    using Domain.Raa.Interfaces.Repositories;

    using SFA.Apprenticeships.Application.Interfaces;
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

        public ProviderUser GetById(int id)
        {
            _logger.Debug("Getting provider user with Id={0}", id);

            const string sql = "SELECT * FROM Provider.ProviderUser WHERE ProviderUserId = @ProviderUserId";

            var sqlParams = new
            {
                ProviderUserId = id
            };

            var dbProviderUser = _getOpenConnection
                .Query<Entities.ProviderUser>(sql, sqlParams)
                .SingleOrDefault();

            _logger.Debug(dbProviderUser == null
                ? "Did not find provider user with username=\"{0}\""
                : "Got provider user with username=\"{0}\"", id);

            return MapProviderUser(dbProviderUser);
        }

        public ProviderUser GetByUsername(string username)
        {
            _logger.Debug("Getting provider user with username=\"{0}\"", username);

            const string sql = "SELECT * FROM Provider.ProviderUser WHERE Username = @username";

            var sqlParams = new
            {
                username
            };

            var dbProviderUser = _getOpenConnection
                .Query<Entities.ProviderUser>(sql, sqlParams)
                .SingleOrDefault();

            _logger.Debug(dbProviderUser == null
                ? "Did not find provider user with username=\"{0}\""
                : "Got provider user with username=\"{0}\"", username);

            return MapProviderUser(dbProviderUser);
        }

        public ProviderUser GetByEmail(string email)
        {
            _logger.Debug("Getting provider user with username=\"{0}\"", email);

            const string sql = "SELECT * FROM Provider.ProviderUser WHERE Email = @email";

            var sqlParams = new
            {
                email
            };

            var dbProviderUser = _getOpenConnection
                .Query<Entities.ProviderUser>(sql, sqlParams)
                .SingleOrDefault();

            _logger.Debug(dbProviderUser == null
                ? "Did not find provider user with username=\"{0}\""
                : "Got provider user with username=\"{0}\"", email);

            return MapProviderUser(dbProviderUser);
        }

        public IEnumerable<ProviderUser> GetAllByUkprn(string ukprn)
        {
            _logger.Debug("Getting provider users for provider with Ukprn=\"{0}\"", ukprn);

            const string sql = "SELECT pu.* FROM Provider.ProviderUser pu INNER JOIN dbo.Provider p ON p.ProviderId = pu.ProviderId WHERE p.Ukprn = @ukprn AND p.ProviderStatusTypeId = @providerStatusTypeId";

            var sqlParams = new
            {
                ukprn,
                providerStatusTypeId = ProviderStatuses.Activated
            };

            var providerUsers = _getOpenConnection
                .Query<Entities.ProviderUser>(sql, sqlParams)
                .Select(MapProviderUser);

            _logger.Debug("Got provider users for provider with Ukprn=\"{0}\"", ukprn);

            return providerUsers;
        }

        public ProviderUser Create(ProviderUser providerUser)
        {
            _logger.Debug("Creating provider user with ProviderUserGuid={0}", providerUser.ProviderUserGuid);

            UpdateEntityTimestamps(providerUser);

            var dbProviderUser = MapProviderUser(providerUser);

            dbProviderUser.ProviderUserId = (int)_getOpenConnection.Insert(dbProviderUser);

            _logger.Debug("Created provider user with ProviderUserGuid={0} and ProviderId={1}",
                providerUser.ProviderUserGuid, dbProviderUser.ProviderUserId);

            return MapProviderUser(dbProviderUser);
        }

        public ProviderUser Update(ProviderUser providerUser)
        {
            _logger.Debug("Updating provider user with ProviderUserId={0}", providerUser.ProviderUserId);

            UpdateEntityTimestamps(providerUser);

            var dbProviderUser = MapProviderUser(providerUser);

            _getOpenConnection.UpdateSingle(dbProviderUser);

            _logger.Debug("Updated provider user with ProviderUserId={0}", providerUser.ProviderUserId);

            return MapProviderUser(dbProviderUser);
        }

        #region Helpers

        private ProviderUser MapProviderUser(Entities.ProviderUser dbProviderUser)
        {
            return _mapper.Map<Entities.ProviderUser, ProviderUser>(dbProviderUser);
        }

        private Entities.ProviderUser MapProviderUser(ProviderUser providerUser)
        {
            return _mapper.Map<ProviderUser, Entities.ProviderUser>(providerUser);
        }

        // TODO: AG: move to UpdateEntityTimestamps to one place.
        private static void UpdateEntityTimestamps(ProviderUser entity)
        {
            if (entity.CreatedDateTime == DateTime.MinValue)
            {
                entity.CreatedDateTime = DateTime.UtcNow;
                entity.UpdatedDateTime = null;
            }
            else
            {
                entity.UpdatedDateTime = DateTime.UtcNow;
            }
        }

        #endregion
    }
}
