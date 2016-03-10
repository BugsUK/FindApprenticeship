namespace SFA.Apprenticeships.Infrastructure.Repositories.Sql.Schemas.Provider
{
    using System.Collections.Generic;
    using System.Linq;
    using SFA.Infrastructure.Interfaces;
    using Common;
    using Domain.Entities.Raa.Parties;
    using Domain.Raa.Interfaces.Repositories;

    public class ProviderSiteRepository : IProviderSiteReadRepository, IProviderSiteWriteRepository
    {
        private readonly IGetOpenConnection _getOpenConnection;
        private readonly IMapper _mapper;
        private readonly ILogService _logger;

        public ProviderSiteRepository(IGetOpenConnection getOpenConnection, IMapper mapper, ILogService logger)
        {
            _getOpenConnection = getOpenConnection;
            _mapper = mapper;
            _logger = logger;
        }

        public ProviderSite GetById(int providerSiteId)
        {
            _logger.Debug("Getting provider site with ProviderSiteId={0}", providerSiteId);

            const string sql = "SELECT * FROM dbo.ProviderSite WHERE ProviderSiteId = @providerSiteId";

            var sqlParams = new
            {
                providerSiteId
            };

            var dbProviderSite = _getOpenConnection.Query<Entities.ProviderSite>(sql, sqlParams).SingleOrDefault();

            _logger.Debug(dbProviderSite == null
                ? "Did not find provider site with ProviderSiteId={0}"
                : "Got provider site with ProviderSiteId={0}",
                providerSiteId);

            return MapProviderSite(dbProviderSite);
        }

        public ProviderSite GetByEdsUrn(string edsUrn)
        {
            _logger.Debug("Getting provider site with EDSURN={0}", edsUrn);

            const string sql = "SELECT * FROM dbo.ProviderSite WHERE EDSURN = @edsUrn";

            var sqlParams = new
            {
                edsUrn
            };

            var dbProviderSite = _getOpenConnection.Query<Entities.ProviderSite>(sql, sqlParams).SingleOrDefault();

            _logger.Debug(dbProviderSite == null
                ? "Did not find provider site with EDSURN={0}"
                : "Got provider site with EDSURN={0}",
                edsUrn);

            return MapProviderSite(dbProviderSite);
        }

        public IEnumerable<ProviderSite> GetByUkprn(string ukprn)
        {
            _logger.Debug("Getting provider sites with UKPRN={0}", ukprn);

            const string sql = @"
                SELECT ps.* FROM dbo.ProviderSite ps
                INNER JOIN dbo.ProviderSiteRelationship psr
                ON psr.ProviderSiteID = ps.ProviderSiteID
                INNER JOIN dbo.Provider p
                ON p.ProviderID = psr.ProviderID
                WHERE p.UKPRN = @ukprn";

            var sqlParams = new
            {
                ukprn
            };

            var providerSites = _getOpenConnection.Query<Entities.ProviderSite>(sql, sqlParams);

            return providerSites.Select(MapProviderSite);
        }

        public IEnumerable<ProviderSite> GetByIds(IEnumerable<int> providerSiteIds)
        {
            var providerSiteIdsArray = providerSiteIds as int[] ?? providerSiteIds.ToArray();

            _logger.Debug("Getting provider sites with ProvideSiteId IN {0}", string.Join(", ", providerSiteIdsArray));

            const string sql = "SELECT * FROM dbo.ProviderSite WHERE ProviderSiteId IN @providerSiteIdsArray";

            var sqlParams = new
            {
                providerSiteIdsArray
            };

            var providerSites = _getOpenConnection.Query<Entities.ProviderSite>(sql, sqlParams);

            return providerSites.Select(MapProviderSite);
        }

        public ProviderSite Update(ProviderSite providerSite)
        {
            throw new System.NotImplementedException();
        }

        private ProviderSite MapProviderSite(Entities.ProviderSite dbProviderSite)
        {
            return _mapper.Map<Entities.ProviderSite, ProviderSite>(dbProviderSite);
        }
    }
}