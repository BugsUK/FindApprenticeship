namespace SFA.Apprenticeships.Infrastructure.Repositories.Sql.Schemas.Provider
{
    using System.Collections.Generic;
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
            throw new System.NotImplementedException();
        }

        public ProviderSite GetByEdsUrn(string edsUrn)
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<ProviderSite> GetByUkprn(string ukprn)
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<ProviderSite> GetByIds(IEnumerable<int> providerSiteIds)
        {
            throw new System.NotImplementedException();
        }

        public ProviderSite Update(ProviderSite providerSite)
        {
            throw new System.NotImplementedException();
        }
    }
}