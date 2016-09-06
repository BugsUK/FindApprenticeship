namespace SFA.Apprenticeships.Infrastructure.Repositories.Sql.Schemas.Provider
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Common;
    using Domain.Raa.Interfaces.Repositories;
    using Application.Interfaces;
    using Domain.Entities.Feature;
    using ProviderSite = Domain.Entities.Raa.Parties.ProviderSite;
    using ProviderSiteRelationship = Domain.Entities.Raa.Parties.ProviderSiteRelationship;

    public class ProviderSiteRepository : IProviderSiteReadRepository, IProviderSiteWriteRepository
    {
        private const int ActivatedEmployerTrainingProviderStatusId = 1;
        private const int OwnerRelationship = 1;
        private const int SubcontractorRelationship = 2;
        private const int RecruitmentConsultantRelationship = 3;

        private readonly IGetOpenConnection _getOpenConnection;
        private readonly IMapper _mapper;
        private readonly ILogService _logger;
        private readonly IConfigurationService _configurationService;

        public ProviderSiteRepository(IGetOpenConnection getOpenConnection, IMapper mapper, ILogService logger, IConfigurationService configurationService)
        {
            _getOpenConnection = getOpenConnection;
            _mapper = mapper;
            _logger = logger;
            _configurationService = configurationService;
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

            if (dbProviderSite == null)
                return null;

            var providerSiteRelationships = GetProviderIdByProviderSiteId(new List<int> { dbProviderSite.ProviderSiteId });

            return MapProviderSite(dbProviderSite, providerSiteRelationships);
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

            if (dbProviderSite == null)
                return null;

            var providerSiteRelationships = GetProviderIdByProviderSiteId(new List<int> { dbProviderSite.ProviderSiteId });

            return MapProviderSite(dbProviderSite, providerSiteRelationships);
        }
        
        public IReadOnlyDictionary<int, ProviderSite> GetByIds(IEnumerable<int> providerSiteIds)
        {
            var providerSiteIdsArray = providerSiteIds as int[] ?? providerSiteIds.ToArray();

            _logger.Debug("Getting provider sites with ProvideSiteId IN {0}", string.Join(", ", providerSiteIdsArray));

            const string sql = "SELECT * FROM dbo.ProviderSite WHERE ProviderSiteId IN @providerSiteIdsArray";

            var sqlParams = new
            {
                providerSiteIdsArray
            };

            var providerSites = _getOpenConnection.Query<Entities.ProviderSite>(sql, sqlParams);
            var providerSiteRelationships = GetProviderIdByProviderSiteId(providerSites.Select(ps => ps.ProviderSiteId).Distinct());

            return providerSites.Select(ps => MapProviderSite(ps, providerSiteRelationships)).ToDictionary(ps => ps.ProviderSiteId);
        }

        public IEnumerable<ProviderSite> GetByProviderId(int providerId)
        {
            _logger.Debug("Getting provider sites for provider={0}", providerId);

            var sql = @"
                SELECT ps.* FROM dbo.ProviderSite ps
                INNER JOIN dbo.ProviderSiteRelationship psr
                ON psr.ProviderSiteID = ps.ProviderSiteID
                WHERE psr.ProviderID = @providerId 
                AND ps.TrainingProviderStatusTypeId = @ActivatedEmployerTrainingProviderStatusId";

            var isSubcontractorsFeatureEnabled = _configurationService.Get<FeatureConfiguration>().IsSubcontractorsFeatureEnabled();
            if(!isSubcontractorsFeatureEnabled)
            {
                sql += " and psr.ProviderSiteRelationShipTypeID = @OwnerRelationship";
            }

            sql += " ORDER BY psr.ProviderSiteRelationShipTypeID, ps.TradingName, ps.Town";

            var sqlParams = new
            {
                providerId,
                ActivatedEmployerTrainingProviderStatusId,
                OwnerRelationship
            };

            var dbProviderSites = _getOpenConnection.Query<Entities.ProviderSite>(sql, sqlParams);
            var providerSiteRelationships = GetProviderIdByProviderSiteId(dbProviderSites.Select(ps => ps.ProviderSiteId).Distinct());
            var providerSites = dbProviderSites.Select(ps => MapProviderSite(ps, providerSiteRelationships));

            if (isSubcontractorsFeatureEnabled)
            {
                //Subcontractors can also see the lead's provider sites
                //Get all the ProviderSiteRelationships where the provider owns the provider site but that provider site is also subcontracted to another provider
                var subContractorProviderSiteRelationships = providerSiteRelationships.Values.Where(psrl => psrl.Any(psr => psr.ProviderID == providerId && psr.ProviderSiteRelationShipTypeID == OwnerRelationship)).SelectMany(psrl => psrl.Where(psr => psr.ProviderSiteRelationShipTypeID == SubcontractorRelationship));
                var subContractorOwnerProviderSites = GetByProviderIds(subContractorProviderSiteRelationships.Select(psr => psr.ProviderID).Distinct(), new []{ OwnerRelationship });
                providerSites = providerSites.Union(subContractorOwnerProviderSites);
            }

            return providerSites;
        }

        private IEnumerable<ProviderSite> GetByProviderIds(IEnumerable<int> providerIds, IEnumerable<int> providerSiteRelationShipTypeIds)
        {
            var sql = @"
                SELECT ps.* FROM dbo.ProviderSite ps
                INNER JOIN dbo.ProviderSiteRelationship psr
                ON psr.ProviderSiteID = ps.ProviderSiteID
                WHERE psr.ProviderID IN @providerIds
                AND psr.ProviderSiteRelationShipTypeID IN @providerSiteRelationShipTypeIds
                AND ps.TrainingProviderStatusTypeId = @ActivatedEmployerTrainingProviderStatusId
                ORDER BY psr.ProviderSiteRelationShipTypeID, ps.TradingName, ps.Town";

            var sqlParams = new
            {
                providerIds,
                providerSiteRelationShipTypeIds,
                ActivatedEmployerTrainingProviderStatusId
            };

            var providerSites = _getOpenConnection.Query<Entities.ProviderSite>(sql, sqlParams);
            var providerSiteRelationships = GetProviderIdByProviderSiteId(providerSites.Select(ps => ps.ProviderSiteId).Distinct());

            return providerSites.Select(ps => MapProviderSite(ps, providerSiteRelationships));
        }

        public ProviderSite Update(ProviderSite providerSite)
        {
            _logger.Debug("Saving provider site with ProviderSiteId={0}", providerSite.ProviderSiteId);

            var dbProviderSite = MapProvider(providerSite);

            if (!_getOpenConnection.UpdateSingle(dbProviderSite))
            {
                throw new Exception($"Failed to save provider site with ProviderSiteId={providerSite.ProviderSiteId}");
            }

            return GetById(providerSite.ProviderSiteId);
        }

        private Entities.ProviderSite MapProvider(ProviderSite providerSite)
        {
            return _mapper.Map<ProviderSite, Entities.ProviderSite>(providerSite);
        }

        private ProviderSite MapProviderSite(Entities.ProviderSite dbProviderSite, IReadOnlyDictionary<int, List<Entities.ProviderSiteRelationship>> providerSiteRelationships)
        {
            if (dbProviderSite == null)
            {
                return null;
            }

            var providerSite = _mapper.Map<Entities.ProviderSite, ProviderSite>(dbProviderSite);
            providerSite.ProviderSiteRelationships = _mapper.Map<List<Entities.ProviderSiteRelationship>, List<ProviderSiteRelationship>>(providerSiteRelationships[providerSite.ProviderSiteId]);

            return providerSite;
        }

        private IReadOnlyDictionary<int, List<Entities.ProviderSiteRelationship>> GetProviderIdByProviderSiteId(IEnumerable<int> providerSiteIds)
        {
            const string sql = @"
                SELECT *
                FROM dbo.ProviderSiteRelationship AS psr 
                JOIN ProviderSite AS ps ON psr.ProviderSiteID = ps.ProviderSiteId 
                WHERE ps.ProviderSiteId IN @providerSiteIds
                ORDER BY psr.ProviderSiteRelationshipTypeID";

            var sqlParams = new
            {
                providerSiteIds
            };

            var map = new Dictionary<int, List<Entities.ProviderSiteRelationship>>();

            var providerSiteRelationships = _getOpenConnection.Query<Entities.ProviderSiteRelationship>(sql, sqlParams);
            foreach (var providerSiteRelationship in providerSiteRelationships)
            {
                if (map.ContainsKey(providerSiteRelationship.ProviderSiteID))
                {
                    map[providerSiteRelationship.ProviderSiteID].Add(providerSiteRelationship);
                }
                else
                {
                    map[providerSiteRelationship.ProviderSiteID] = new List<Entities.ProviderSiteRelationship> { providerSiteRelationship };
                }
            }

            return map;
        }
    }
}
