namespace SFA.Apprenticeships.Infrastructure.Repositories.Sql.Schemas.Provider
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Common;
    using Domain.Entities.Raa.Users;
    using Domain.Entities.Users;
    using Domain.Raa.Interfaces.Repositories;
    using Domain.Raa.Interfaces.Repositories.Models;
    using Application.Interfaces;

    // TODO: SQL: AG: double check logging.

    public class ProviderUserRepository : IProviderUserReadRepository, IProviderUserWriteRepository
    {
        private const string ProviderUserSelect = "SELECT pu.*, p.Ukprn, p.TradingName as ProviderName FROM Provider.ProviderUser pu JOIN dbo.Provider p ON p.ProviderId = pu.ProviderId WHERE ";

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

            const string sql = ProviderUserSelect + "ProviderUserId = @ProviderUserId";

            var sqlParams = new
            {
                ProviderUserId = id
            };

            var dbProviderUser = _getOpenConnection
                .Query<Entities.ProviderUser>(sql, sqlParams)
                .SingleOrDefault();

            _logger.Debug(dbProviderUser == null
                ? "Did not find provider user with id=\"{0}\""
                : "Got provider user with id=\"{0}\"", id);

            return MapProviderUser(dbProviderUser);
        }

        public ProviderUser GetByUsername(string username)
        {
            _logger.Debug("Getting provider user with username=\"{0}\"", username);

            const string sql = ProviderUserSelect + "Username LIKE @username";

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

            const string sql = ProviderUserSelect + "Email LIKE @email";

            var sqlParams = new
            {
                email
            };

            var dbProviderUser = _getOpenConnection
                .Query<Entities.ProviderUser>(sql, sqlParams)
                .SingleOrDefault();

            _logger.Debug(dbProviderUser == null
                ? "Did not find provider user with email=\"{0}\""
                : "Got provider user with email=\"{0}\"", email);

            return MapProviderUser(dbProviderUser);
        }

        public IEnumerable<ProviderUser> Search(ProviderUserSearchParameters searchParameters)
        {
            var sql = ProviderUserSelect;
            var and = "";

            if (!string.IsNullOrEmpty(searchParameters.Username))
            {
                sql += "Username LIKE '%' + @Username + '%' ";
                and = "AND ";
            }
            if (!string.IsNullOrEmpty(searchParameters.Name))
            {
                sql += and + "pu.Fullname LIKE '%' + @Name + '%' ";
                and = "AND ";
            }
            if (!string.IsNullOrEmpty(searchParameters.Email))
            {
                sql += and + "Email LIKE '%' + @Email + '%' ";
                and = "AND ";
            }
            if (searchParameters.AllUnverifiedEmails)
            {
                sql += and + "(EmailVerificationCode IS NOT NULL OR ProviderUserStatusId != 20) ";
            }
            sql += "ORDER BY FullName";

            var providerUsers = _getOpenConnection
                .Query<Entities.ProviderUser>(sql, searchParameters)
                .Select(MapProviderUser);

            return providerUsers;
        }

        public IEnumerable<ProviderUser> GetAllByUkprn(string ukprn)
        {
            _logger.Debug("Getting provider users for provider with Ukprn=\"{0}\"", ukprn);

            const string sql = ProviderUserSelect + "p.Ukprn = @ukprn AND p.ProviderStatusTypeId = @providerStatusTypeId ORDER BY FullName";

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
