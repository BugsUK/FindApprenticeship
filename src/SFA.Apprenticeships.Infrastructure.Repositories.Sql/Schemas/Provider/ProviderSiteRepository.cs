namespace SFA.Apprenticeships.Infrastructure.Repositories.Sql.Schemas.Provider
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Common;
    using Domain.Raa.Interfaces.Repositories;
    using Application.Interfaces;
    using Domain.Entities.Feature;
    using Domain.Raa.Interfaces.Repositories.Models;
    using ProviderSite = Domain.Entities.Raa.Parties.ProviderSite;
    using ProviderSiteRelationship = Domain.Entities.Raa.Parties.ProviderSiteRelationship;

    public class ProviderSiteRepository : IProviderSiteReadRepository, IProviderSiteWriteRepository
    {
        public const string SelectByIdSql = "SELECT * FROM dbo.ProviderSite WHERE ProviderSiteId = @providerSiteId";
        public const string SelectByProviderIdSql = @"
                SELECT ps.* FROM dbo.ProviderSite ps
                INNER JOIN dbo.ProviderSiteRelationship psr
                ON psr.ProviderSiteID = ps.ProviderSiteID
                WHERE psr.ProviderID = @providerId 
                AND ps.TrainingProviderStatusTypeId = @ActivatedEmployerTrainingProviderStatusId";

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

            var sqlParams = new
            {
                providerSiteId
            };

            var dbProviderSite = _getOpenConnection.Query<Entities.ProviderSite>(SelectByIdSql, sqlParams).SingleOrDefault();

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

            var sql = SelectByProviderIdSql;

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
                var subContractorProviderSiteRelationships = providerSiteRelationships.Values.Where(psrl => psrl.Any(psr => psr.ProviderId == providerId && psr.ProviderSiteRelationShipTypeId == OwnerRelationship)).SelectMany(psrl => psrl.Where(psr => psr.ProviderSiteRelationShipTypeId == SubcontractorRelationship));
                var subContractorOwnerProviderSites = GetByProviderIds(subContractorProviderSiteRelationships.Select(psr => psr.ProviderId).Distinct(), new []{ OwnerRelationship });
                providerSites = providerSites.Union(subContractorOwnerProviderSites);
            }

            return providerSites;
        }

        public IEnumerable<ProviderSite> Search(ProviderSiteSearchParameters searchParameters)
        {
            var sql = "SELECT * FROM dbo.ProviderSite WHERE ";
            if (!string.IsNullOrEmpty(searchParameters.Id))
            {
                sql += "ProviderSiteId = @Id ";
            }
            if (!string.IsNullOrEmpty(searchParameters.EdsUrn))
            {
                sql += "EDSURN = @EdsUrn ";
            }
            if (!string.IsNullOrEmpty(searchParameters.Name))
            {
                sql += "FullName LIKE '%' + @name + '%' OR TradingName LIKE '%' + @name + '%' ";
            }
            sql += "ORDER BY FullName";

            var providerSites = _getOpenConnection.Query<Entities.ProviderSite>(sql, searchParameters);
            var providerSiteRelationships = GetProviderIdByProviderSiteId(providerSites.Select(ps => ps.ProviderSiteId).Distinct());

            return providerSites.Select(ps => MapProviderSite(ps, providerSiteRelationships));
        }

        public ProviderSiteRelationship GetProviderSiteRelationship(int providerSiteRelationshipId)
        {
            const string sql = @"
                SELECT psr.*, p.UKPRN As ProviderUkprn, p.FullName as ProviderFullName, p.TradingName as ProviderTradingName, ps.FullName as ProviderSiteFullName, ps.TradingName as ProviderSiteTradingName
                FROM dbo.ProviderSiteRelationship AS psr 
                JOIN Provider p ON psr.ProviderID = p.ProviderId 
                JOIN ProviderSite ps ON psr.ProviderSiteID = ps.ProviderSiteId 
                WHERE ProviderSiteRelationshipID = @providerSiteRelationshipId";

            var sqlParams = new
            {
                providerSiteRelationshipId
            };

            var providerSiteRelationship = _getOpenConnection.Query<Entities.ProviderSiteRelationship>(sql, sqlParams).SingleOrDefault();

            return _mapper.Map<Entities.ProviderSiteRelationship, ProviderSiteRelationship>(providerSiteRelationship);
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

        public ProviderSite Create(ProviderSite providerSite)
        {
            _logger.Info("Creating provider site with EDSURN={0}", providerSite.EdsUrn);

            var dbProviderSite = MapProviderSite(providerSite);

            _getOpenConnection.Insert(dbProviderSite);

            var newProviderSite = GetByEdsUrn(providerSite.EdsUrn);

            var providerSiteId = newProviderSite.ProviderSiteId;
            foreach (var providerSiteRelationship in providerSite.ProviderSiteRelationships)
            {
                var dbProviderSiteRelationship = _mapper.Map<ProviderSiteRelationship, Entities.ProviderSiteRelationship>(providerSiteRelationship);
                dbProviderSiteRelationship.ProviderSiteId = providerSiteId;
                _getOpenConnection.Insert(dbProviderSiteRelationship);
            }

            return GetById(providerSiteId);
        }

        public ProviderSite Update(ProviderSite providerSite)
        {
            _logger.Debug("Saving provider site with ProviderSiteId={0}", providerSite.ProviderSiteId);

            var dbProviderSite = MapProviderSite(providerSite);

            if (!_getOpenConnection.UpdateSingle(dbProviderSite))
            {
                throw new Exception($"Failed to save provider site with ProviderSiteId={providerSite.ProviderSiteId}");
            }

            foreach (var providerSiteRelationship in providerSite.ProviderSiteRelationships)
            {
                var dbProviderSiteRelationship = _mapper.Map<ProviderSiteRelationship, Entities.ProviderSiteRelationship>(providerSiteRelationship);
                
                if (!_getOpenConnection.UpdateSingle(dbProviderSiteRelationship))
                {
                    throw new Exception($"Failed to save provider site relationship with ProviderSiteRelationshipId={providerSiteRelationship.ProviderSiteRelationshipId}");
                }
            }

            return GetById(providerSite.ProviderSiteId);
        }

        public ProviderSiteRelationship Create(ProviderSiteRelationship providerSiteRelationship)
        {
            var dbProviderSiteRelationship = _mapper.Map<ProviderSiteRelationship, Entities.ProviderSiteRelationship>(providerSiteRelationship);
            providerSiteRelationship.ProviderSiteRelationshipId = (int)_getOpenConnection.Insert(dbProviderSiteRelationship);
            return providerSiteRelationship;
        }

        public void DeleteProviderSiteRelationship(int providerSiteRelationshipId)
        {
            _getOpenConnection.MutatingQuery<object>("DELETE FROM ProviderSiteFramework WHERE ProviderSiteRelationshipID = @providerSiteRelationshipId", new { providerSiteRelationshipId });
            _getOpenConnection.MutatingQuery<object>("DELETE FROM ProviderSiteLocalAuthority WHERE ProviderSiteRelationshipID = @providerSiteRelationshipId", new { providerSiteRelationshipId });
            _getOpenConnection.MutatingQuery<object>("DELETE FROM ProviderSiteRelationship WHERE ProviderSiteRelationshipID = @providerSiteRelationshipId", new { providerSiteRelationshipId });
        }

        private Entities.ProviderSite MapProviderSite(ProviderSite providerSite)
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
            if (providerSiteRelationships.Count > 0 && providerSiteRelationships.ContainsKey(providerSite.ProviderSiteId))
            {
                providerSite.ProviderSiteRelationships = _mapper.Map<List<Entities.ProviderSiteRelationship>, List<ProviderSiteRelationship>>(providerSiteRelationships[providerSite.ProviderSiteId]);
            }
            else
            {
                providerSite.ProviderSiteRelationships = new List<ProviderSiteRelationship>();
            }

            return providerSite;
        }

        private IReadOnlyDictionary<int, List<Entities.ProviderSiteRelationship>> GetProviderIdByProviderSiteId(IEnumerable<int> providerSiteIds)
        {
            const string sql = @"
                SELECT psr.*, p.UKPRN As ProviderUkprn, p.FullName as ProviderFullName, p.TradingName as ProviderTradingName, ps.FullName as ProviderSiteFullName, ps.TradingName as ProviderSiteTradingName
                FROM dbo.ProviderSiteRelationship AS psr 
                JOIN Provider p ON psr.ProviderID = p.ProviderId 
                JOIN ProviderSite ps ON psr.ProviderSiteID = ps.ProviderSiteId 
                WHERE psr.ProviderSiteId IN @providerSiteIds
                ORDER BY psr.ProviderSiteRelationshipTypeID";

            var sqlParams = new
            {
                providerSiteIds
            };

            var map = new Dictionary<int, List<Entities.ProviderSiteRelationship>>();

            var providerSiteRelationships = _getOpenConnection.Query<Entities.ProviderSiteRelationship>(sql, sqlParams);
            foreach (var providerSiteRelationship in providerSiteRelationships)
            {
                if (map.ContainsKey(providerSiteRelationship.ProviderSiteId))
                {
                    map[providerSiteRelationship.ProviderSiteId].Add(providerSiteRelationship);
                }
                else
                {
                    map[providerSiteRelationship.ProviderSiteId] = new List<Entities.ProviderSiteRelationship> { providerSiteRelationship };
                }
            }

            return map;
        }
    }
}
