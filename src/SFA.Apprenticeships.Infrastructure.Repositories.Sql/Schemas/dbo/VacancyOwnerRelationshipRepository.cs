namespace SFA.Apprenticeships.Infrastructure.Repositories.Sql.Schemas.dbo
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Common;
    using Domain.Entities.Providers;
    using Domain.Interfaces.Repositories;
    using Entities;
    using SFA.Infrastructure.Interfaces;

    public class VacancyOwnerRelationshipRepository : IProviderSiteEmployerLinkReadRepository, IProviderSiteEmployerLinkWriteRepository
    {
        private readonly IMapper _mapper;
        private readonly ILogService _logger;
        private readonly IGetOpenConnection _getOpenConnection;

        public VacancyOwnerRelationshipRepository(IGetOpenConnection getOpenConnection,
            IMapper mapper,
            ILogService logger)
        {
            _getOpenConnection = getOpenConnection;
            _mapper = mapper;
            _logger = logger;
        }

        public ProviderSiteEmployerLink Get(string providerSiteErn, string ern)
        {
            _logger.Debug("Called SQL DB to get ProviderSiteEmployerLink with providerSiteErn={0} and employer edsurn={1}", providerSiteErn, ern);

            var vor = _getOpenConnection.Query<VacancyOwnerRelationship>(@"SELECT vor.*
FROM dbo.VacancyOwnerRelationship vor
INNER JOIN dbo.ProviderSite ps on ps.ProviderSiteID = vor.ProviderSiteID
INNER JOIN dbo.Employer e on e.EmployerId = vor.EmployerId
WHERE ps.EDSURN = @providerSiteEdsUrn
AND e.EdsUrn = @employerEdsUrn", new { providerSiteEdsUrn = providerSiteErn, employerEdsUrn =  ern }).SingleOrDefault();

            _logger.Debug("Attempting to map VacancyOwnerRelationship with providerSiteEdsUrn={0} and employerEdsUrn={1}, to a ProviderSiteEmployerLink", providerSiteErn, ern);

            var result = _mapper.Map<VacancyOwnerRelationship, ProviderSiteEmployerLink>(vor);
            return result;
        }

        public IEnumerable<ProviderSiteEmployerLink> GetForProviderSite(string providerSiteErn)
        {
            _logger.Debug("Called SQL DB to get ProviderSiteEmployerLinks with providerSiteErn={0}", providerSiteErn);

            var vorList = _getOpenConnection.Query<VacancyOwnerRelationship>(@"SELECT vor.*
FROM dbo.VacancyOwnerRelationship vor
INNER JOIN dbo.ProviderSite ps on ps.ProviderSiteID = vor.ProviderSiteID
WHERE ps.EDSURN = @providerSiteEdsUrn", new { providerSiteEdsUrn = providerSiteErn }).ToList();

            _logger.Debug("Attempting to map List of VacancyOwnerRelationship with providerSiteEdsUrn={0}, to a List of ProviderSiteEmployerLink", providerSiteErn);

            var result = _mapper.Map<List<VacancyOwnerRelationship>, List<ProviderSiteEmployerLink>>(vorList);
            return result;
        }

        public ProviderSiteEmployerLink Save(ProviderSiteEmployerLink providerSiteEmployerLink)
        {
            //get providerSite by edsurn
            const string providerSiteSql = "SELECT ps.* FROM dbo.ProviderSite ps WHERE ps.EDSURN = @providerSiteEdsUrn";
            var providerSiteParams = new
            {
                providerSiteEmployerLink.ProviderSiteErn
            };


            //get employer by edsurn
            const string employerSql = "SELECT e.* FROM dbo.Employer e WHERE e.EdsUrn = @employerEdsUrn";
            var employerParams = new
            {
                providerSiteEmployerLink.Employer.Ern
            };
            
            //map vor
            var vor = _mapper.Map<ProviderSiteEmployerLink, VacancyOwnerRelationship>(providerSiteEmployerLink);
            var employer = _getOpenConnection.Query<Employer>(employerSql, employerParams).SingleOrDefault();
            var providerSite = _getOpenConnection.Query<Entities.ProviderSite>(providerSiteSql, providerSiteParams).SingleOrDefault();
            vor.EmployerId = employer.EmployerId;
            vor.ProviderSiteID = providerSite.ProviderSiteId;
            
            //save
            _logger.Debug("Called SQL DB to save VacancyOwnerRelationship with Id={0}", providerSiteEmployerLink.EntityId);

            //UpdateEntityTimestamps(providerSiteEmployerLink);

            //TODO: SQL: UpdateTimeStamps
            try
            {
                var result = (int)_getOpenConnection.Insert(vor);
                vor.VacancyOwnerRelationshipId = result;
            }
            catch (Exception ex)
            {
                // TODO: Detect key violation

                if (!_getOpenConnection.UpdateSingle(vor))
                    throw new Exception("Failed to update record after failed insert", ex);
            }

            _logger.Debug("Saved VacancyOwnerRelationship to SQL DB with Id={0}", providerSiteEmployerLink.EntityId);

            var endResult = _mapper.Map<VacancyOwnerRelationship, ProviderSiteEmployerLink>(vor);
            endResult.Employer = providerSiteEmployerLink.Employer;

            return endResult;
        }
    }
}
