namespace SFA.Apprenticeships.Application.Organisation
{
    using System.Collections.Generic;
    using CuttingEdge.Conditions;
    using Domain.Entities.Organisations;
    using Domain.Entities.Providers;
    using Interfaces.Logging;
    using Interfaces.Organisations;

    public class OrganisationService : IOrganisationService
    {
        private readonly ILogService _logService;
        private readonly IVerifiedOrganisationProvider _verifiedOrganisationProvider;
        private readonly ILegacyProviderProvider _legacyProviderProvider;
        private readonly ILegacyEmployerProvider _legacyEmployerProvider;

        public OrganisationService(
            ILogService logService,
            IVerifiedOrganisationProvider verifiedOrganisationProvider,
            ILegacyProviderProvider legacyProviderProvider,
            ILegacyEmployerProvider legacyEmployerProvider)
        {
            _logService = logService;
            _verifiedOrganisationProvider = verifiedOrganisationProvider;
            _legacyProviderProvider = legacyProviderProvider;
            _legacyEmployerProvider = legacyEmployerProvider;
        }

        public VerifiedOrganisationSummary GetVerifiedOrganisationSummary(string referenceNumber)
        {
            Condition.Requires(referenceNumber).IsNotNullOrEmpty();

            _logService.Debug("Calling VerifiedOrganisationProvider to get organisation with reference='{0}'.", referenceNumber);

            return _verifiedOrganisationProvider.GetByReferenceNumber(referenceNumber);
        }

        public Provider GetProvider(string ukprn)
        {
            Condition.Requires(ukprn).IsNotNullOrEmpty();

            _logService.Debug("Calling LegacyProviderProvider to get provider with UKPRN='{0}'.", ukprn);

            return _legacyProviderProvider.GetProvider(ukprn);
        }

        public ProviderSite GetProviderSite(string ukprn, string ern)
        {
            Condition.Requires(ukprn).IsNotNullOrEmpty();
            Condition.Requires(ern).IsNotNullOrEmpty();

            _logService.Debug("Calling LegacyProviderProvider to get provider site with UKPRN='{0}' and ERN='{1}'.", ukprn, ern);

            return _legacyProviderProvider.GetProviderSite(ukprn, ern);
        }

        public IEnumerable<ProviderSite> GetProviderSites(string ukprn)
        {
            Condition.Requires(ukprn).IsNotNullOrEmpty();

            _logService.Debug("Calling LegacyProviderProvider to get provider sites for provider with UKPRN='{0}'.", ukprn);

            return _legacyProviderProvider.GetProviderSites(ukprn);
        }

        public ProviderSiteEmployerLink GetProviderSiteEmployerLink(string providerSiteErn, string ern)
        {
            Condition.Requires(providerSiteErn).IsNotNullOrEmpty();
            Condition.Requires(ern).IsNotNullOrEmpty();

            _logService.Debug("Calling LegacyProviderProvider to get provider site employer link for provider with ERN='{0}' and employer with ERN='{1}'.", providerSiteErn, ern);

            return _legacyProviderProvider.GetProviderSiteEmployerLink(providerSiteErn, ern);
        }

        public IEnumerable<ProviderSiteEmployerLink> GetProviderSiteEmployerLinks(string providerSiteErn)
        {
            Condition.Requires(providerSiteErn).IsNotNullOrEmpty();

            _logService.Debug("Calling LegacyProviderProvider to get provider site employer links for provider with ERN='{0}'.", providerSiteErn);

            return _legacyProviderProvider.GetProviderSiteEmployerLinks(providerSiteErn);
        }

        public Employer GetEmployer(string ern)
        {
            Condition.Requires(ern).IsNotNullOrEmpty();

            _logService.Debug("Calling LegacyEmployerProvider to get employer with ERN='{0}'.", ern);

            return _legacyEmployerProvider.GetEmployer(ern);
        }
    }
}
