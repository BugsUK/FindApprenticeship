using SFA.Apprenticeships.Application.Interfaces.Employers;
using SFA.Apprenticeships.Application.Interfaces.Generic;

namespace SFA.Apprenticeships.Application.Provider
{
    using System.Collections.Generic;
    using System.Linq;
    using CuttingEdge.Conditions;
    using Domain.Entities.Raa.Parties;
    using Domain.Raa.Interfaces.Repositories;
    using Infrastructure.Interfaces;
    using Interfaces.Organisations;
    using Interfaces.Providers;

    //TODO: Much of this servcies's implementation is a crutch to coping with a current lack of a migration strategy. In the future all data will be in the repositories and so the
    //organization service will only be used for searching for new employers
    public class ProviderService : IProviderService
    {
        private readonly IOrganisationService _organisationService;
        private readonly IEmployerService _employerService;
        private readonly IProviderReadRepository _providerReadRepository;
        private readonly IProviderWriteRepository _providerWriteRepository;
        private readonly IProviderSiteReadRepository _providerSiteReadRepository;
        private readonly IProviderSiteWriteRepository _providerSiteWriteRepository;
        private readonly IVacancyPartyReadRepository _vacancyPartyReadRepository;
        private readonly IVacancyPartyWriteRepository _vacancyPartyWriteRepository;
        private readonly ILogService _logService;

        public ProviderService(IOrganisationService organisationService, IProviderReadRepository providerReadRepository, IProviderWriteRepository providerWriteRepository, IProviderSiteReadRepository providerSiteReadRepository, IProviderSiteWriteRepository providerSiteWriteRepository, IVacancyPartyReadRepository vacancyPartyReadRepository, IVacancyPartyWriteRepository vacancyPartyWriteRepository, ILogService logService, IEmployerService employerService)
        {
            _organisationService = organisationService;
            _providerReadRepository = providerReadRepository;
            _providerWriteRepository = providerWriteRepository;
            _providerSiteReadRepository = providerSiteReadRepository;
            _providerSiteWriteRepository = providerSiteWriteRepository;
            _vacancyPartyReadRepository = vacancyPartyReadRepository;
            _vacancyPartyWriteRepository = vacancyPartyWriteRepository;
            _logService = logService;
            _employerService = employerService;
        }

        public Provider GetProviderViaOwnerParty(int vacancyPartyId)
        {
            //TODO: Remove null checks following SQL migration
            var vacancyParty = _vacancyPartyReadRepository.Get(vacancyPartyId);

            if (vacancyParty != null)
            {
                var providerSite = _providerSiteReadRepository.Get(vacancyParty.ProviderSiteId);
                if (providerSite != null)
                {
                    return _providerReadRepository.GetById(providerSite.ProviderId);
                }
            }
            return new Provider();
        }

        public Provider GetProvider(int providerId)
        {
            return _providerReadRepository.GetById(providerId);
        }

        public Provider GetProvider(string ukprn)
        {
            Condition.Requires(ukprn).IsNotNullOrEmpty();

            _logService.Debug("Calling ProviderReadRepository to get provider with UKPRN='{0}'.", ukprn);

            var provider = _providerReadRepository.GetByUkprn(ukprn);

            if (provider != null)
            {
                return provider;
            }

            _logService.Debug("Calling OrganisationService to get provider with UKPRN='{0}'.", ukprn);

            provider = _organisationService.GetProvider(ukprn);

            return provider;
        }

        public IEnumerable<Provider> GetProviders(IEnumerable<int> providerIds)
        {
            return _providerReadRepository.GetByIds(providerIds);
        }

        public void SaveProvider(Provider provider)
        {
            _providerWriteRepository.Update(provider);
        }

        public ProviderSite GetProviderSite(int providerSiteId)
        {
            return _providerSiteReadRepository.Get(providerSiteId);
        }

        public ProviderSite GetProviderSite(string ukprn, string edsUrn)
        {
            Condition.Requires(ukprn).IsNotNullOrEmpty();
            Condition.Requires(edsUrn).IsNotNullOrEmpty();

            _logService.Debug("Calling ProviderSiteReadRepository to get provider site with UKPRN='{0}' and ERN='{1}'.", ukprn, edsUrn);

            var providerSite = _providerSiteReadRepository.GetByEdsUrn(edsUrn);

            if (providerSite != null)
            {
                return providerSite;
            }

            _logService.Debug("Calling OrganisationService to get provider site with UKPRN='{0}' and ERN='{1}'.", ukprn, edsUrn);

            providerSite = _organisationService.GetProviderSite(ukprn, edsUrn);

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

        public IEnumerable<ProviderSite> GetProviderSites(IEnumerable<int> providerSiteIds)
        {
            return _providerSiteReadRepository.GetByIds(providerSiteIds);
        }

        public void SaveProviderSites(IEnumerable<ProviderSite> providerSites)
        {
            foreach (var providerSite in providerSites)
            {
                _providerSiteWriteRepository.Save(providerSite);
            }
        }

        public VacancyParty GetVacancyParty(int vacancyPartyId)
        {
            return _vacancyPartyReadRepository.Get(vacancyPartyId);
        }

        public VacancyParty GetVacancyParty(int providerSiteId, string edsUrn)
        {
            Condition.Requires(providerSiteId);
            Condition.Requires(edsUrn).IsNotNullOrEmpty();

            var employer = _employerService.GetEmployer(edsUrn);
            if (employer.EdsUrn != edsUrn)
            {
                employer = _employerService.GetEmployer(employer.EdsUrn);
            }
            if (employer.EmployerId == 0)
            {
                //New employer
                employer = _employerService.SaveEmployer(employer);
            }

            _logService.Debug("Calling ProviderSiteEmployerLinkReadRepository to get provider site employer link for provider site with ERN='{0}' and employer with ERN='{1}'.", providerSiteId, employer.EmployerId);

            var vacancyParty = _vacancyPartyReadRepository.Get(providerSiteId, employer.EmployerId);

            if (vacancyParty != null)
            {
                return vacancyParty;
            }

            _logService.Debug("Calling OrganisationService to get provider site employer link for provider site with ERN='{0}' and employer with ERN='{1}'.", providerSiteId, edsUrn);

            vacancyParty = _organisationService.GetVacancyParty(providerSiteId, employer.EmployerId);

            if (vacancyParty == null)
            {
                vacancyParty = new VacancyParty
                {
                    ProviderSiteId = providerSiteId,
                    EmployerId = employer.EmployerId
                };
            }

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
            return _vacancyPartyReadRepository.GetForProviderSite(providerSiteId);
        }

        private List<VacancyParty> GetVacancyParties(EmployerSearchRequest request)
        {
            Condition.Requires(request).IsNotNull();

            _logService.Debug("Calling OrganisationService to get provider site employer link for provider site with Id='{0}'.", request.ProviderSiteId);

            var vacancyParties = _organisationService.GetVacancyParties(request.ProviderSiteId).ToList();

            _logService.Debug("Calling ProviderSiteEmployerLinkReadRepository to get provider site employer link for provider site with Id='{0}'.", request.ProviderSiteId);

            var vacancyPartiesFromRepository = _vacancyPartyReadRepository.GetForProviderSite(request.ProviderSiteId);

            //Combine with results from repository
            vacancyParties = vacancyPartiesFromRepository.Union(vacancyParties, new VacancyPartyEqualityComparer()).ToList();

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

            var resultCount = results.Count();
            pageable.Page = results.Skip((currentPage - 1) * pageSize).Take(pageSize).ToList();
            pageable.ResultsCount = resultCount;
            pageable.TotalNumberOfPages = (resultCount / pageSize) + 1;

            return pageable;
        }
    }
}
