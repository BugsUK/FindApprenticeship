namespace SFA.Apprenticeships.Infrastructure.Repositories.Sql.Schemas.Provider
{
    using System;
    using System.Linq;
    using Domain.Interfaces.Repositories;
    using SFA.Infrastructure.Interfaces;
    using Common;
    using Domain.Entities.Raa.Parties;
    using Domain.Entities.Users;
    using Domain.Raa.Interfaces.Repositories;
    
    public class ProviderRepository : IProviderReadRepository, IProviderWriteRepository
    {
        private readonly IGetOpenConnection _getOpenConnection;
        private readonly IMapper _mapper;
        private readonly ILogService _logger;

        public ProviderRepository(IGetOpenConnection getOpenConnection, IMapper mapper, ILogService logger)
        {
            _getOpenConnection = getOpenConnection;
            _mapper = mapper;
            _logger = logger;
        }

        /// <summary>
        /// Gets one by Id. Default return is null.
        /// </summary>
        /// <param name="providerId"></param>
        /// <returns></returns>
        public Provider Get(int providerId)
        {
            _logger.Debug("Getting provider with ProviderId={0}", providerId);

            var dbProvider = GetById(providerId);

            _logger.Debug("Got provider with ProviderId={0}", providerId);

            return MapProvider(dbProvider);
        }

        public Provider GetViaUkprn(string ukprn)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets one by Ukprn. Default return is null.
        /// </summary>
        /// <param name="ukprn"></param>
        /// <returns></returns>
        public Provider Get(string ukprn)
        {
            _logger.Debug("Getting activated provider with Ukprn={0}", ukprn);

            const string sql = "SELECT * FROM dbo.Provider WHERE UKPRN = @ukprn AND ProviderStatusTypeId = @providerStatusTypeId";

            var sqlParams = new
            {
                ukprn,
                //providerStatusTypeID = ProviderStatus.Activated //TODO: fix this
            };

            var dbVacancy = _getOpenConnection.Query<Entities.Provider>(sql, sqlParams).SingleOrDefault();

            _logger.Debug("Got activated provider with Ukprn={0}", ukprn);

            return MapProvider(dbVacancy);
        }

        /// <summary>
        /// Deletes one by Id.
        /// </summary>
        /// <param name="providerId"></param>
        public void Delete(int providerId)
        {
            _logger.Debug("Deleting provider with ProviderId={0}", providerId);

            var objectToDelete = GetById(providerId);

            _getOpenConnection.DeleteSingle(objectToDelete);

            _logger.Debug("Deleted provider with ProviderId={0}", providerId);
        }

        /// <summary>
        /// UpdateEntityTimeStamps
        /// Save to Collection
        /// Return Saved object - this should be returned from the collection.save operation.
        /// </summary>
        /// <param name="provider"></param>
        /// <returns></returns>
        public Provider Save(Provider provider)
        {
            _logger.Debug("Saving provider with Ukprn={0}", provider.Ukprn);

            var dbProvider = MapProvider(provider);

            // TODO: SQL: AG: note that we are not attempting to insert a new Provider, we always update (temporary).

            if (!_getOpenConnection.UpdateSingle(dbProvider))
            {
                throw new Exception($"Failed to save provider with Ukprn={provider.Ukprn}");
            }

            return MapProvider(dbProvider);
        }

        #region Helpers

        private Entities.Provider MapProvider(Provider provider)
        {
            return _mapper.Map<Provider, Entities.Provider>(provider);
        }

        private Provider MapProvider(Entities.Provider provider)
        {
            return _mapper.Map<Entities.Provider, Provider>(provider);
        }

        private Entities.Provider GetById(int providerId)
        {
            const string sql = "SELECT * FROM dbo.Provider WHERE ProviderId = @providerId";

            var sqlParams = new
            {
                providerId
            };

            var dbVacancy = _getOpenConnection.Query<Entities.Provider>(sql, sqlParams).SingleOrDefault();

            return dbVacancy;
        }

        #endregion
    }
}