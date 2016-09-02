﻿namespace SFA.Apprenticeships.Infrastructure.Repositories.Sql.Schemas.Provider
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Common;
    using Domain.Raa.Interfaces.Repositories;
    using Entities;
    using Application.Interfaces;
    using ProviderSite = Domain.Entities.Raa.Parties.ProviderSite;

    public class ProviderSiteRepository : IProviderSiteReadRepository, IProviderSiteWriteRepository
    {
        private const int ActivatedEmployerTrainingProviderStatusId = 1;
        private const int OwnerRelationship = 1;

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

        public IEnumerable<ProviderSite> GetByProviderId(int providerId, bool returnAll = false)
        {
            _logger.Debug("Getting provider sites for provider={0}", providerId);

            var sql = @"
                SELECT ps.* FROM dbo.ProviderSite ps
                INNER JOIN dbo.ProviderSiteRelationship psr
                ON psr.ProviderSiteID = ps.ProviderSiteID
                WHERE psr.ProviderID = @providerId 
                AND ps.TrainingProviderStatusTypeId = @ActivatedEmployerTrainingProviderStatusId";

            if (!returnAll)
            {
                sql += $" and psr.ProviderSiteRelationShipTypeID = @OwnerRelationship";
            }

            var sqlParams = new
            {
                providerId,
                ActivatedEmployerTrainingProviderStatusId,
                OwnerRelationship
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

        private ProviderSite MapProviderSite(Entities.ProviderSite dbProviderSite, IReadOnlyDictionary<int, ProviderSiteRelationship> providerSiteRelationships)
        {
            if (dbProviderSite == null)
            {
                return null;
            }

            var providerSite = _mapper.Map<Entities.ProviderSite, ProviderSite>(dbProviderSite);

            providerSite.ProviderId = providerSiteRelationships[providerSite.ProviderSiteId].ProviderID;

            return providerSite;
        }

        // Contracted
        private IReadOnlyDictionary<int, ProviderSiteRelationship> GetProviderIdByProviderSiteId(IEnumerable<int> providerSiteIds)
        {
            //TODO: Deal with Subcontractors and recruitment consultants. Should be done with ContractOwnerId rather than like this

            const string sql = @"
                SELECT *
                FROM dbo.ProviderSiteRelationship AS psr 
                JOIN ProviderSite AS ps ON psr.ProviderSiteID = ps.ProviderSiteId 
                WHERE ps.ProviderSiteId IN @providerSiteIds
                ORDER BY psr.ProviderSiteRelationshipTypeID"; //Forces non Subcontractors and Recruitment Consultants to the end of the list to prioritize owners

            // TODO: change to ProviderSiteRelationshipTypeID = 1?

            var sqlParams = new
            {
                providerSiteIds
            };

            var map = new Dictionary<int, ProviderSiteRelationship>();

            //TODO: workaround to be able to create the index. Should be done properly.
            var providerSiteRelationships = _getOpenConnection.Query<ProviderSiteRelationship>(sql, sqlParams);
            foreach (var providerSiteRelationship in providerSiteRelationships)
            {
                if (!map.ContainsKey(providerSiteRelationship.ProviderSiteID))
                {
                    map[providerSiteRelationship.ProviderSiteID] = providerSiteRelationship;
                }
            }

            return map;
        }
    }
}
