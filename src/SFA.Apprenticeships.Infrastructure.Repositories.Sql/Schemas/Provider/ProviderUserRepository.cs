namespace SFA.Apprenticeships.Infrastructure.Repositories.Sql.Schemas.Provider
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Common;
    using Domain.Entities.Users;
    using Domain.Interfaces.Repositories;
    using SFA.Infrastructure.Interfaces;

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
            throw new NotImplementedException();
        }

        public void Delete(Guid id)
        {
            throw new NotSupportedException();
        }

        public ProviderUser Save(ProviderUser entity)
        {
            // TODO: SQL: AG: need to talk about Save(). Can we get an upsert rather than throw/catch?
            throw new NotImplementedException();
        }

        #region Helpers

        private ProviderUser MapProviderUser(Entities.ProviderUser dbProviderUser)
        {
            return _mapper.Map<Entities.ProviderUser, ProviderUser>(dbProviderUser);
        }

        // TODO: SQL: AG: use or lose dead code.

        /*
        private Entities.ProviderUser MapProviderUser(ProviderUser providerUser)
        {
            return _mapper.Map<ProviderUser, Entities.ProviderUser>(providerUser);
        }
        */

        #endregion
    }
}
