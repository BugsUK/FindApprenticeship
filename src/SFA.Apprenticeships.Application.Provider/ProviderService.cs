using SFA.Apprenticeships.Application.Interfaces.Employers;
using SFA.Apprenticeships.Application.Interfaces.Generic;

namespace SFA.Apprenticeships.Application.Provider
{
    using System.Collections.Generic;
    using System.Linq;
    using CuttingEdge.Conditions;
    using Domain.Entities.Raa.Parties;
    using Domain.Raa.Interfaces.Repositories;
    using SFA.Infrastructure.Interfaces;
    using Interfaces.Providers;

    public class ProviderService : IProviderService
    {
        private readonly IEmployerService _employerService;
        private readonly IProviderReadRepository _providerReadRepository;
        private readonly IProviderSiteReadRepository _providerSiteReadRepository;
        private readonly IVacancyPartyReadRepository _vacancyPartyReadRepository;
        private readonly IVacancyPartyWriteRepository _vacancyPartyWriteRepository;
        private readonly ILogService _logService;

        public ProviderService(IProviderReadRepository providerReadRepository, IProviderSiteReadRepository providerSiteReadRepository, IVacancyPartyReadRepository vacancyPartyReadRepository, IVacancyPartyWriteRepository vacancyPartyWriteRepository, ILogService logService, IEmployerService employerService)
        {
            _providerReadRepository = providerReadRepository;
            _providerSiteReadRepository = providerSiteReadRepository;
            _vacancyPartyReadRepository = vacancyPartyReadRepository;
            _vacancyPartyWriteRepository = vacancyPartyWriteRepository;
            _logService = logService;
            _employerService = employerService;
        }

        public Provider GetProviderViaOwnerParty(int vacancyPartyId)
        {
            var vacancyParty = _vacancyPartyReadRepository.GetById(vacancyPartyId);
            var providerSite = _providerSiteReadRepository.GetById(vacancyParty.ProviderSiteId);
            return _providerReadRepository.GetById(providerSite.ProviderId);
        }

        public Provider GetProvider(int providerId)
        {
            return _providerReadRepository.GetById(providerId);
        }

        public Provider GetProvider(string ukprn)
        {
            Condition.Requires(ukprn).IsNotNullOrEmpty();

            _logService.Debug("Calling ProviderReadRepository to get provider with UKPRN='{0}'.", ukprn);

            return _providerReadRepository.GetByUkprn(ukprn);
        }

        public IEnumerable<Provider> GetProviders(IEnumerable<int> providerIds)
        {
            return _providerReadRepository.GetByIds(providerIds);
        }

        public ProviderSite GetProviderSite(int providerSiteId)
        {
            return _providerSiteReadRepository.GetById(providerSiteId);
        }

        public ProviderSite GetProviderSite(string edsUrn)
        {
            Condition.Requires(edsUrn).IsNotNullOrEmpty();

            _logService.Debug("Calling ProviderSiteReadRepository to get provider site with ERN='{0}'.", edsUrn);

            return _providerSiteReadRepository.GetByEdsUrn(edsUrn);
        }

        public IEnumerable<ProviderSite> GetProviderSites(string ukprn)
        {
            Condition.Requires(ukprn).IsNotNullOrEmpty();

            _logService.Debug("Calling ProviderSiteReadRepository to get provider sites for provider with UKPRN='{0}'.", ukprn);

            var provider = _providerReadRepository.GetByUkprn(ukprn);

            return _providerSiteReadRepository.GetByProviderId(provider.ProviderId);
        }

        public IEnumerable<ProviderSite> GetProviderSites(IEnumerable<int> providerSiteIds)
        {
            return _providerSiteReadRepository.GetByIds(providerSiteIds);
        }

        public VacancyParty GetVacancyParty(int vacancyPartyId)
        {
            return _vacancyPartyReadRepository.GetById(vacancyPartyId);
        }

        public VacancyParty GetVacancyParty(int providerSiteId, string edsUrn)
        {
            Condition.Requires(providerSiteId);
            Condition.Requires(edsUrn).IsNotNullOrEmpty();

            _logService.Debug("Calling Employer Service to get employer with EDSURN='{0}'.", edsUrn);

            var employer = _employerService.GetEmployer(edsUrn);

            _logService.Debug("Calling VacancyPartyReadRepository to get vacancy party for provider site with Id='{0}' and employer with Id='{1}'.", providerSiteId, employer.EmployerId);

            var vacancyParty = _vacancyPartyReadRepository.GetByProviderSiteAndEmployerId(providerSiteId, employer.EmployerId) ??
                               new VacancyParty {ProviderSiteId = providerSiteId, EmployerId = employer.EmployerId};

            return vacancyParty;
        }

        public VacancyParty SaveVacancyParty(VacancyParty vacancyParty)
        {
            return _vacancyPartyWriteRepository.Save(vacancyParty);
        }

        public IEnumerable<VacancyParty> GetVacancyParties(IEnumerable<int> vacancyPartyIds)
        {
            return _vacancyPartyReadRepository.GetByIds(vacancyPartyIds);
        }

        public IEnumerable<VacancyParty> GetVacancyParties(int providerSiteId)
        {
            return _vacancyPartyReadRepository.GetByProviderSiteId(providerSiteId);
        }

        private List<VacancyParty> GetVacancyParties(EmployerSearchRequest request)
        {
            Condition.Requires(request).IsNotNull();

            _logService.Debug("Calling VacancyPartyReadRepository to get vacancy party for provider site with Id='{0}'.", request.ProviderSiteId);

            var vacancyParties = _vacancyPartyReadRepository.GetByProviderSiteId(request.ProviderSiteId).ToList();

            if (request.IsQuery)
            {
                var employers = _employerService.GetEmployers(vacancyParties.Select(v => v.EmployerId).Distinct());

                if (request.IsEmployerEdsUrnQuery)
                {
                    employers = employers.Where(e => e.EdsUrn == request.EmployerEdsUrn);
                }
                else if (request.IsNameAndLocationQuery)
                {
                    employers = employers.Where(e =>
                        e.Name.ToLower().Contains(request.Name.ToLower()) &&
                        ((e.Address.Postcode != null &&
                          e.Address.Postcode.ToLower().Contains(request.Location.ToLower())) ||
                         (e.Address.AddressLine4 != null &&
                          e.Address.AddressLine4.ToLower().Contains(request.Location.ToLower()))));
                }
                else if (request.IsNameQuery)
                {
                    employers = employers.Where(e => e.Name.ToLower().Contains(request.Name.ToLower()));
                }
                else if (request.IsLocationQuery)
                {
                    employers = employers.Where(e =>
                        (e.Address.Postcode != null &&
                         e.Address.Postcode.ToLower().Contains(request.Location.ToLower())) ||
                        (e.Address.AddressLine4 != null &&
                         e.Address.AddressLine4.ToLower().Contains(request.Location.ToLower())));
                }

                vacancyParties = vacancyParties.Where(vp => employers.Any(e => e.EmployerId == vp.EmployerId)).ToList();
            }

            return vacancyParties;
        }

        public Pageable<VacancyParty> GetVacancyParties(EmployerSearchRequest request, int currentPage, int pageSize)
        {
            var results = GetVacancyParties(request);

            var pageable = new Pageable<VacancyParty>
            {
                CurrentPage = currentPage
            };

            var resultCount = results.Count;
            pageable.Page = results.Skip((currentPage - 1) * pageSize).Take(pageSize).ToList();
            pageable.ResultsCount = resultCount;
            pageable.TotalNumberOfPages = (resultCount / pageSize) + 1;

            return pageable;
        }
    }
}
