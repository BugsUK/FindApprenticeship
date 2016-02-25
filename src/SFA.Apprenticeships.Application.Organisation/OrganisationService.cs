namespace SFA.Apprenticeships.Application.Organisation
{
    using System.Collections.Generic;
    using System.Linq;
    using CuttingEdge.Conditions;
    using Domain.Entities.Raa.Locations;
    using Domain.Entities.Raa.Parties;
    using Interfaces.Generic;
    using Infrastructure.Interfaces;
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

        public IEnumerable<VerifiedOrganisationSummary> GetVerifiedOrganisationSummaries(string edsUrn, string name, string location)
        {
            if (!string.IsNullOrEmpty(edsUrn))
            {
                var verifiedOrganisationSummary = _verifiedOrganisationProvider.GetByReferenceNumber(edsUrn);
                if (verifiedOrganisationSummary == null)
                {
                    return new List<VerifiedOrganisationSummary>();
                }
                return new List<VerifiedOrganisationSummary>
                {
                    verifiedOrganisationSummary
                };
            }

            int resultCount;
            return _verifiedOrganisationProvider.Find(name, location, out resultCount);
        }

        public Pageable<VerifiedOrganisationSummary> GetVerifiedOrganisationSummaries(string edsUrn, string name, string location, int currentPage, int pageSize)
        {
            var pageable = new Pageable<VerifiedOrganisationSummary>
            {
                CurrentPage = currentPage
            };

            if (!string.IsNullOrEmpty(edsUrn))
            {
                var verifiedOrganisationSummary = _verifiedOrganisationProvider.GetByReferenceNumber(edsUrn);
                if (verifiedOrganisationSummary == null)
                {
                    pageable.Page = new List<VerifiedOrganisationSummary>();
                    return pageable;
                }
                pageable.Page = new List<VerifiedOrganisationSummary>
                {
                    verifiedOrganisationSummary
                };
                pageable.ResultsCount = 1;
                pageable.TotalNumberOfPages = 1;
                return pageable;
            }

            int resultCount;
            var verifiedOrganisationSummaries = _verifiedOrganisationProvider.Find(name, location, out resultCount);
            pageable.Page = verifiedOrganisationSummaries.Skip((currentPage - 1)*pageSize).Take(pageSize).ToList();
            pageable.ResultsCount = resultCount;
            pageable.TotalNumberOfPages = (resultCount/pageSize) + 1;
            return pageable;
        }

        public Provider GetProvider(string ukprn)
        {
            Condition.Requires(ukprn).IsNotNullOrEmpty();

            _logService.Debug("Calling LegacyProviderProvider to get provider with UKPRN='{0}'.", ukprn);

            return _legacyProviderProvider.GetProvider(ukprn);
        }

        public ProviderSite GetProviderSite(string ukprn, string edsUrn)
        {
            Condition.Requires(ukprn).IsNotNullOrEmpty();
            Condition.Requires(edsUrn).IsNotNullOrEmpty();

            _logService.Debug("Calling LegacyProviderProvider to get provider site with UKPRN='{0}' and ERN='{1}'.", ukprn, edsUrn);

            return _legacyProviderProvider.GetProviderSite(ukprn, edsUrn);
        }

        public IEnumerable<ProviderSite> GetProviderSites(string ukprn)
        {
            Condition.Requires(ukprn).IsNotNullOrEmpty();

            _logService.Debug("Calling LegacyProviderProvider to get provider sites for provider with UKPRN='{0}'.", ukprn);

            return _legacyProviderProvider.GetProviderSites(ukprn);
        }

        public VacancyParty GetVacancyParty(int providerSiteId, int employerId)
        {
            Condition.Requires(providerSiteId);
            Condition.Requires(employerId);

            _logService.Debug("Calling LegacyProviderProvider to get provider site employer link for provider with Id='{0}' and employer with Id='{1}'.", providerSiteId, employerId);

            return _legacyProviderProvider.GetVacancyParty(providerSiteId, employerId);
        }

        public IEnumerable<VacancyParty> GetVacancyParties(int providerSiteId)
        {
            Condition.Requires(providerSiteId);

            _logService.Debug("Calling LegacyProviderProvider to get provider site employer links for provider with Id='{0}'.", providerSiteId);

            return _legacyProviderProvider.GetVacancyParties(providerSiteId);
        }

        public Employer GetEmployer(int employerId)
        {
            Condition.Requires(employerId);

            _logService.Debug("Calling LegacyEmployerProvider to get employer with id='{0}'.", employerId);

            return _legacyEmployerProvider.GetEmployer(employerId);
        }

        public Employer GetEmployer(string edsUrn)
        {
            Condition.Requires(edsUrn).IsNotNullOrEmpty();

            _logService.Debug("Calling LegacyEmployerProvider to get employer with ERN='{0}'.", edsUrn);

            return _legacyEmployerProvider.GetEmployer(edsUrn) ?? Convert(GetVerifiedOrganisationSummary(edsUrn));
        }

        public IEnumerable<Employer> GetByIds(IEnumerable<int> employerIds)
        {
            _logService.Debug("Calling LegacyEmployerProvider to get employers by Ids='{0}'.", employerIds);

            return _legacyEmployerProvider.GetEmployersByIds(employerIds);
        }

        public IEnumerable<Employer> GetEmployers(string edsUrn, string name, string location)
        {
            var summaries = GetVerifiedOrganisationSummaries(edsUrn, name, location);
            return summaries.Select(Convert).ToList();
        }

        public Pageable<Employer> GetEmployers(string edsUrn, string name, string location, int currentPage, int pageSize)
        {
            var summariesPage = GetVerifiedOrganisationSummaries(edsUrn, name, location, currentPage, pageSize);
            var employersPage = new Pageable<Employer>
            {
                Page = summariesPage.Page.Select(Convert).ToList(),
                ResultsCount = summariesPage.ResultsCount,
                CurrentPage = summariesPage.CurrentPage,
                TotalNumberOfPages = summariesPage.TotalNumberOfPages
            };
            return employersPage;
        }

        private static Employer Convert(VerifiedOrganisationSummary summary)
        {
            return new Employer
            {
                EdsUrn = summary.ReferenceNumber,
                Name = summary.Name,
                Address = new PostalAddress()
                {
                    AddressLine1 = summary.Address.AddressLine1,
                    AddressLine2 = summary.Address.AddressLine2,
                    AddressLine3 = summary.Address.AddressLine3,
                    AddressLine4 = summary.Address.AddressLine4,
                    GeoPoint = summary.Address.GeoPoint,
                    Postcode = summary.Address.Postcode,
                    //Uprn = summary.Address.ValidationSourceKeyValue
                }
            };
        }
    }
}
