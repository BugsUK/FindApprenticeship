using SFA.Apprenticeships.Application.Interfaces.Employers;

namespace SFA.Apprenticeships.Application.Organisation
{
    using System.Collections.Generic;
    using System.Linq;
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

        public IEnumerable<VerifiedOrganisationSummary> GetVerifiedOrganisationSummaries(string ern, string name, string location)
        {
            if (!string.IsNullOrEmpty(ern))
            {
                var verifiedOrganisationSummary = _verifiedOrganisationProvider.GetByReferenceNumber(ern);
                if (verifiedOrganisationSummary == null)
                {
                    return new List<VerifiedOrganisationSummary>();
                }
                return new List<VerifiedOrganisationSummary>
                {
                    verifiedOrganisationSummary
                };
            }

            return _verifiedOrganisationProvider.Find(name, location);
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

        public IEnumerable<ProviderSiteEmployerLink> GetProviderSiteEmployerLinks(EmployerSearchRequest request)
        {
            Condition.Requires(request).IsNotNull();

            _logService.Debug("Calling LegacyProviderProvider to get provider site employer links for provider with ERN='{0}'.", request.ProviderSiteErn);

            return _legacyProviderProvider.GetProviderSiteEmployerLinks(request);
        }

        public Employer GetEmployer(string ern)
        {
            Condition.Requires(ern).IsNotNullOrEmpty();

            _logService.Debug("Calling LegacyEmployerProvider to get employer with ERN='{0}'.", ern);

            return _legacyEmployerProvider.GetEmployer(ern);
        }

        public IEnumerable<Employer> GetEmployers(string ern, string name, string location)
        {
            var summaries = GetVerifiedOrganisationSummaries(ern, name, location);
            return summaries.Select(Convert).ToList();
        }

        private static Employer Convert(VerifiedOrganisationSummary summary)
        {
            return new Employer
            {
                Ern = summary.ReferenceNumber,
                Name = summary.Name,
                Address = summary.Address
            };
        }
    }
}
