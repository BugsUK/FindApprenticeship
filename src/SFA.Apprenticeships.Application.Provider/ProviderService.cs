namespace SFA.Apprenticeships.Application.Provider
{
    using System.Collections.Generic;
    using System.Linq;
    using CuttingEdge.Conditions;
    using Domain.Entities.Providers;
    using Domain.Interfaces.Repositories;
    using Interfaces.Logging;
    using Interfaces.Organisations;
    using Interfaces.Providers;

    public class ProviderService : IProviderService
    {
        private readonly IOrganisationService _organisationService;
        private readonly IProviderReadRepository _providerReadRepository;
        private readonly IProviderWriteRepository _providerWriteRepository;
        private readonly IProviderSiteReadRepository _providerSiteReadRepository;
        private readonly IProviderSiteWriteRepository _providerSiteWriteRepository;
        private readonly ILogService _logService;

        public ProviderService(IOrganisationService organisationService, IProviderReadRepository providerReadRepository, IProviderWriteRepository providerWriteRepository, IProviderSiteReadRepository providerSiteReadRepository, IProviderSiteWriteRepository providerSiteWriteRepository, ILogService logService)
        {
            _organisationService = organisationService;
            _providerReadRepository = providerReadRepository;
            _providerWriteRepository = providerWriteRepository;
            _providerSiteReadRepository = providerSiteReadRepository;
            _providerSiteWriteRepository = providerSiteWriteRepository;
            _logService = logService;
        }

        public Provider GetProvider(string ukprn)
        {
            Condition.Requires(ukprn).IsNotNullOrEmpty();

            _logService.Debug("Calling ProviderReadRepository to get provider with UKPRN='{0}'.", ukprn);

            var provider = _providerReadRepository.Get(ukprn);

            if (provider != null)
            {
                return provider;
            }

            _logService.Debug("Calling OrganisationService to get provider with UKPRN='{0}'.", ukprn);

            provider = _organisationService.GetProvider(ukprn);

            return provider;
        }

        public IEnumerable<ProviderSite> GetProviderSites(string ukprn)
        {
            Condition.Requires(ukprn).IsNotNullOrEmpty();

            _logService.Debug("Calling ProviderSiteReadRepository to get provider sites for provider with UKPRN='{0}'.", ukprn);

            IEnumerable<ProviderSite> providerSites = _providerSiteReadRepository.GetForProvider(ukprn).ToList();

            if (providerSites.Any())
            {
                return providerSites;
            }

            _logService.Debug("Calling OrganisationService to get provider sites for provider with UKPRN='{0}'.", ukprn);

            providerSites = _organisationService.GetProviderSites(ukprn);

            return providerSites;
        }
    }
}
