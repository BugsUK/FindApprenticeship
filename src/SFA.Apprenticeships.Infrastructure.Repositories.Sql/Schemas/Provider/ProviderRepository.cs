namespace SFA.Apprenticeships.Infrastructure.Repositories.Sql.Schemas.Provider
{
    using System;
    using System.Linq;
    using Domain.Interfaces.Repositories;
    using SFA.Infrastructure.Interfaces;
    using Common;
    using Provider = Domain.Entities.Providers.Provider;

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
        /// <param name="id"></param>
        /// <returns></returns>
        public Provider Get(Guid id)
        {
            _logger.Debug("Calling database to get provider with Id={0}", id);

            var dbVacancy = GetbyId(id);

            return MapProvider(dbVacancy);
        }

        /// <summary>
        /// Gets one by UKPRN. Default return is null.
        /// </summary>
        /// <param name="ukprn"></param>
        /// <returns></returns>
        public Provider Get(string ukprn)
        {
            _logger.Debug("Calling database to get provider with UKPrn={0}", ukprn);

            var dbVacancy = _getOpenConnection.Query<Entities.Provider>("SELECT * FROM Provider.Provider WHERE UKPrn = @ProviderUKPrn", new { ProviderUKPrn = ukprn }).SingleOrDefault();

            return MapProvider(dbVacancy);
        }

        private Entities.Provider GetbyId(Guid id)
        {
            var dbVacancy = _getOpenConnection.Query<Entities.Provider>("SELECT * FROM Provider.Provider WHERE ProviderId = @ProviderGuid", new { ProviderGuid = id.ResolveToInt() }).SingleOrDefault();
            return dbVacancy;
        }

        /// <summary>
        /// Deletes one by Id.
        /// </summary>
        /// <param name="id"></param>
        public void Delete(Guid id)
        {
            _logger.Debug("Calling repository to delete provider with Id={0}", id);
            var objectToDelete = GetbyId(id);

            _getOpenConnection.DeleteSingle(objectToDelete);

            _logger.Debug("Deleted provider with Id={0}", id);
        }

        /// <summary>
        /// UpdateEntityTimeStamps
        /// Save to Collection
        /// Return Saved object - this should be returned from the collection.save operation.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public Provider Save(Provider entity)
        {
            _logger.Debug("Called Mongodb to save provider with UKPRN={0}", entity.Ukprn);

            UpdateEntityTimestamps(entity);

            var dbEntity = _mapper.Map<Provider, Entities.Provider>(entity);

            try
            {
                var result = (int)_getOpenConnection.Insert(dbEntity);
                dbEntity.ProviderId = result;
            }
            catch (Exception ex)
            {
                // TODO: Detect key violation

                if (!_getOpenConnection.UpdateSingle(dbEntity))
                    throw new Exception("Failed to update record after failed insert", ex);
            }

            _logger.Debug("Saved provider to Mongodb with UKPRN={0}", entity.Ukprn);

            return _mapper.Map<Entities.Provider, Provider>(dbEntity);
        }

        private Provider MapProvider(Entities.Provider provider)
        {
            var result = _mapper.Map<Entities.Provider, Provider>(provider);
            return result;
        }

        protected void UpdateEntityTimestamps(Provider provider)
        {
            // determine whether this is a "new" entity being saved for the first time
            if (provider.DateCreated == DateTime.MinValue)
            {
                provider.DateCreated = DateTime.UtcNow;
                provider.DateUpdated = null;
            }
            else
            {
                provider.DateUpdated = DateTime.UtcNow;
            }
        }
    }
}