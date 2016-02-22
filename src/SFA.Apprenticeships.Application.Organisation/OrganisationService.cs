using SFA.Apprenticeships.Application.Interfaces.Employers;

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

        public IEnumerable<VerifiedOrganisationSummary> GetVerifiedOrganisationSummaries(string edsErn, string name, string location)
        {
            if (!string.IsNullOrEmpty(edsErn))
            {
                var verifiedOrganisationSummary = _verifiedOrganisationProvider.GetByReferenceNumber(edsErn);
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

        public Pageable<VerifiedOrganisationSummary> GetVerifiedOrganisationSummaries(string edsErn, string name, string location, int currentPage, int pageSize)
        {
            var pageable = new Pageable<VerifiedOrganisationSummary>
            {
                CurrentPage = currentPage
            };

            if (!string.IsNullOrEmpty(edsErn))
            {
                var verifiedOrganisationSummary = _verifiedOrganisationProvider.GetByReferenceNumber(edsErn);
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

        public ProviderSite GetProviderSite(string ukprn, string edsErn)
        {
            Condition.Requires(ukprn).IsNotNullOrEmpty();
            Condition.Requires(edsErn).IsNotNullOrEmpty();

            _logService.Debug("Calling LegacyProviderProvider to get provider site with UKPRN='{0}' and ERN='{1}'.", ukprn, edsErn);

            return _legacyProviderProvider.GetProviderSite(ukprn, edsErn);
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

            _logService.Debug("Calling LegacyProviderProvider to get provider site employer link for provider with id='{0}' and employer with id='{1}'.", providerSiteId, employerId);

            return _legacyProviderProvider.GetVacancyParty(providerSiteId, employerId);
        }

        public VacancyParty GetVacancyParty(int providerSiteId, string edsErn)
        {
            Condition.Requires(providerSiteId);
            Condition.Requires(edsErn).IsNotNullOrEmpty();

            _logService.Debug("Calling LegacyProviderProvider to get provider site employer link for provider with ERN='{0}' and employer with ERN='{1}'.", providerSiteId, edsErn);

            return _legacyProviderProvider.GetVacancyParty(providerSiteId, edsErn);
        }

        public IEnumerable<VacancyParty> GetProviderSiteEmployerLinks(EmployerSearchRequest request)
        {
            Condition.Requires(request).IsNotNull();

            _logService.Debug("Calling LegacyProviderProvider to get provider site employer links for provider with ERN='{0}'.", request.ProviderSiteId);

            return _legacyProviderProvider.GetProviderSiteEmployerLinks(request);
        }

        public Employer GetEmployer(int employerId)
        {
            Condition.Requires(employerId);

            _logService.Debug("Calling LegacyEmployerProvider to get employer with id='{0}'.", employerId);

            return _legacyEmployerProvider.GetEmployer(employerId);
        }

        public Employer GetEmployer(string edsErn)
        {
            Condition.Requires(edsErn).IsNotNullOrEmpty();

            _logService.Debug("Calling LegacyEmployerProvider to get employer with ERN='{0}'.", edsErn);

            return _legacyEmployerProvider.GetEmployer(edsErn);
        }

        public IEnumerable<Employer> GetByIds(IEnumerable<int> employerIds)
        {
            _logService.Debug("Calling LegacyEmployerProvider to get employers by Ids='{0}'.", employerIds);

            return _legacyEmployerProvider.GetEmployersByIds(employerIds);
        }

        public IEnumerable<Employer> GetEmployers(string edsErn, string name, string location)
        {
            var summaries = GetVerifiedOrganisationSummaries(edsErn, name, location);
            return summaries.Select(Convert).ToList();
        }

        public Pageable<Employer> GetEmployers(string edsErn, string name, string location, int currentPage, int pageSize)
        {
            var summariesPage = GetVerifiedOrganisationSummaries(edsErn, name, location, currentPage, pageSize);
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
                EdsErn = summary.ReferenceNumber,
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
