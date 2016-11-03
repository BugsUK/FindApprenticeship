namespace SFA.Apprenticeships.Infrastructure.Repositories.Sql.Schemas.Provider
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Common;
    using Domain.Entities.Raa.Parties;
    using Domain.Entities.Users;
    using Domain.Raa.Interfaces.Repositories;
    using Domain.Raa.Interfaces.Repositories.Models;
    using Application.Interfaces;
    using Domain.Entities.Raa.Vacancies;

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

        public Provider GetById(int providerId)
        {
            _logger.Debug("Getting provider with ProviderId={0}", providerId);

            const string sql = "SELECT * FROM dbo.Provider WHERE ProviderId = @providerId";

            var sqlParams = new
            {
                providerId
            };

            var dbProvider = _getOpenConnection.Query<Entities.Provider>(sql, sqlParams).SingleOrDefault();

            _logger.Debug(dbProvider == null
                ? "Did not find provider with ProviderId={0}"
                : "Got provider with ProviderId={0}",
                providerId);

            return MapProvider(dbProvider);
        }

        public Provider GetByUkprn(string ukprn, bool errorIfNotFound = true)
        {
            _logger.Debug("Getting activated provider with Ukprn={0}", ukprn);

            const string sql =
                "SELECT * FROM dbo.Provider WHERE UKPRN = @ukprn AND ProviderStatusTypeId = @providerStatusTypeId";

            var sqlParams = new
            {
                ukprn,
                providerStatusTypeID = ProviderStatuses.Activated
            };

            var dbVacancy = _getOpenConnection.Query<Entities.Provider>(sql, sqlParams).SingleOrDefault();

            if (dbVacancy == null && !errorIfNotFound)
            {
                return null;
            }

            _logger.Debug("Got activated provider with Ukprn={0}", ukprn);

            return MapProvider(dbVacancy);
        }

        public IEnumerable<Provider> GetByIds(IEnumerable<int> providerIds)
        {
            var providerIdsArray = providerIds as int[] ?? providerIds.ToArray();

            _logger.Debug("Getting providers with Ids={0}", string.Join(", ", providerIdsArray));

            const string sql = "SELECT * FROM dbo.Provider WHERE ProviderId IN @ProviderIds";
            var sqlParams = new
            {
                ProviderIds = providerIdsArray
            };

            var providers = _getOpenConnection.Query<Entities.Provider>(sql, sqlParams);

            return providers.Select(MapProvider);
        }

        public IEnumerable<Provider> Search(ProviderSearchParameters searchParameters)
        {
            var sql = "SELECT * FROM dbo.Provider WHERE ";
            if (!string.IsNullOrEmpty(searchParameters.Id))
            {
                sql += "ProviderId = @Id ";
            }
            if (!string.IsNullOrEmpty(searchParameters.Ukprn))
            {
                sql += "UKPRN = @ukprn ";
            }
            if (!string.IsNullOrEmpty(searchParameters.Name))
            {
                sql += "FullName LIKE '%' + @name + '%' OR TradingName LIKE '%' + @name + '%' ";
            }
            sql += "ORDER BY FullName";

            var providers = _getOpenConnection.Query<Entities.Provider>(sql, searchParameters);

            return providers.Select(MapProvider);
        }

        public Provider Create(Provider provider)
        {
            _logger.Info("Creating provider with Ukprn={0}", provider.Ukprn);

            var dbProvider = MapProvider(provider);

            _getOpenConnection.Insert(dbProvider);

            return GetByUkprn(provider.Ukprn);
        }

        /// <summary>
        /// UpdateEntityTimeStamps
        /// Save to Collection
        /// Return Saved object - this should be returned from the collection.save operation.
        /// </summary>
        /// <param name="provider"></param>
        /// <returns></returns>
        public Provider Update(Provider provider)
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

        private Entities.Provider MapProvider(Provider provider)
        {
            return _mapper.Map<Provider, Entities.Provider>(provider);
        }

        private Provider MapProvider(Entities.Provider provider)
        {
            return provider == null
                ? null
                : _mapper.Map<Entities.Provider, Provider>(provider);
        }
    }
}