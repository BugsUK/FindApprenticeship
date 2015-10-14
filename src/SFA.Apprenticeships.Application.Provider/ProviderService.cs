using SFA.Apprenticeships.Application.Interfaces.Employers;

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
        private readonly IProviderSiteEmployerLinkReadRepository _providerSiteEmployerLinkReadRepository;
        private readonly IProviderSiteEmployerLinkWriteRepository _providerSiteEmployerLinkWriteRepository;
        private readonly ILogService _logService;

        public ProviderService(IOrganisationService organisationService, IProviderReadRepository providerReadRepository, IProviderWriteRepository providerWriteRepository, IProviderSiteReadRepository providerSiteReadRepository, IProviderSiteWriteRepository providerSiteWriteRepository, IProviderSiteEmployerLinkReadRepository providerSiteEmployerLinkReadRepository, IProviderSiteEmployerLinkWriteRepository providerSiteEmployerLinkWriteRepository, ILogService logService)
        {
            _organisationService = organisationService;
            _providerReadRepository = providerReadRepository;
            _providerWriteRepository = providerWriteRepository;
            _providerSiteReadRepository = providerSiteReadRepository;
            _providerSiteWriteRepository = providerSiteWriteRepository;
            _providerSiteEmployerLinkReadRepository = providerSiteEmployerLinkReadRepository;
            _providerSiteEmployerLinkWriteRepository = providerSiteEmployerLinkWriteRepository;
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

        public void SaveProvider(Provider provider)
        {
            _providerWriteRepository.Save(provider);
        }

        public ProviderSite GetProviderSite(string ukprn, string ern)
        {
            Condition.Requires(ukprn).IsNotNullOrEmpty();
            Condition.Requires(ern).IsNotNullOrEmpty();

            _logService.Debug("Calling ProviderSiteReadRepository to get provider site with UKPRN='{0}' and ERN='{1}'.", ukprn, ern);

            var providerSite = _providerSiteReadRepository.Get(ern);

            if (providerSite != null)
            {
                return providerSite;
            }

            _logService.Debug("Calling OrganisationService to get provider site with UKPRN='{0}' and ERN='{1}'.", ukprn, ern);

            providerSite = _organisationService.GetProviderSite(ukprn, ern);

            return providerSite;
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

        public void SaveProviderSites(IEnumerable<ProviderSite> providerSites)
        {
            foreach (var providerSite in providerSites)
            {
                _providerSiteWriteRepository.Save(providerSite);
            }
        }

        public ProviderSiteEmployerLink GetProviderSiteEmployerLink(string providerSiteErn, string ern)
        {
            Condition.Requires(providerSiteErn).IsNotNullOrEmpty();
            Condition.Requires(ern).IsNotNullOrEmpty();

            _logService.Debug("Calling ProviderSiteEmployerLinkReadRepository to get provider site employer link for provider site with ERN='{0}' and employer with ERN='{1}'.", providerSiteErn, ern);

            var providerSiteEmployerLink = _providerSiteEmployerLinkReadRepository.Get(providerSiteErn, ern);

            if (providerSiteEmployerLink != null)
            {
                return providerSiteEmployerLink;
            }

            _logService.Debug("Calling OrganisationService to get provider site employer link for provider site with ERN='{0}' and employer with ERN='{1}'.", providerSiteErn, ern);

            providerSiteEmployerLink = _organisationService.GetProviderSiteEmployerLink(providerSiteErn, ern);

            return providerSiteEmployerLink;
        }

        public ProviderSiteEmployerLink SaveProviderSiteEmployerLink(ProviderSiteEmployerLink providerSiteEmployerLink)
        {
            return _providerSiteEmployerLinkWriteRepository.Save(providerSiteEmployerLink);
        }

        public IEnumerable<ProviderSiteEmployerLink> GetProviderSiteEmployerLinks(EmployerSearchRequest request)
        {
            Condition.Requires(request).IsNotNull();

            _logService.Debug("Calling ProviderSiteEmployerLinkReadRepository to get provider site employer link for provider site with ERN='{0}'.", request.ProviderSiteErn);

            //TODO: Reinstate once we've worked out a migration strategy
            /*IEnumerable<ProviderSiteEmployerLink> providerSiteEmployerLinks = _providerSiteEmployerLinkReadRepository.GetForProviderSite(providerSiteErn).ToList();

            if (providerSiteEmployerLinks.Any())
            {
                return providerSiteEmployerLinks;
            }*/

            _logService.Debug("Calling OrganisationService to get provider site employer link for provider site with ERN='{0}'.", request.ProviderSiteErn);

            var providerSiteEmployerLinks = _organisationService.GetProviderSiteEmployerLinks(request);

            return providerSiteEmployerLinks;
        }
    }
}
