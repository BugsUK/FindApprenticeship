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

        public Organisation GetByReferenceNumber(string referenceNumber)
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

        public IEnumerable<ProviderSite> GetProviderSites(string ukprn)
        {
            Condition.Requires(ukprn).IsNotNullOrEmpty();

            _logService.Debug("Calling LegacyProviderProvider to get provider sites for provider with UKPRN='{0}'.", ukprn);

            return _legacyProviderProvider.GetProviderSites(ukprn);
        }

        public Employer GetEmployer(string providerSiteErn, string ern)
        {
            Condition.Requires(providerSiteErn).IsNotNullOrEmpty();

            _logService.Debug("Calling LegacyEmployerProvider to get employer for provider with ERN='{0}' and employer with ERN='{1}'.", providerSiteErn, ern);

            return _legacyEmployerProvider.GetEmployer(providerSiteErn, ern);
        }

        public IEnumerable<Employer> GetEmployers(string providerSiteErn)
        {
            Condition.Requires(providerSiteErn).IsNotNullOrEmpty();

            _logService.Debug("Calling LegacyEmployerProvider to get employers for provider with ERN='{0}'.", providerSiteErn);

            return _legacyEmployerProvider.GetEmployers(providerSiteErn);
        }
    }
}
