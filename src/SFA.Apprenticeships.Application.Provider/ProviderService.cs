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
    using Interfaces.Organisations;
    using Interfaces.Providers;

    //TODO: Much of this servcies's implementation is a crutch to coping with a current lack of a migration strategy. In the future all data will be in the repositories and so the
    //organization service will only be used for searching for new employers
    public class ProviderService : IProviderService
    {
        private readonly IOrganisationService _organisationService;
        private readonly IProviderReadRepository _providerReadRepository;
        private readonly IProviderWriteRepository _providerWriteRepository;
        private readonly IProviderSiteReadRepository _providerSiteReadRepository;
        private readonly IProviderSiteWriteRepository _providerSiteWriteRepository;
        private readonly IVacancyPartyReadRepository _vacancyPartyReadRepository;
        private readonly IVacancyPartyWriteRepository _vacancyPartyWriteRepository;
        private readonly ILogService _logService;

        public ProviderService(IOrganisationService organisationService, IProviderReadRepository providerReadRepository, IProviderWriteRepository providerWriteRepository, IProviderSiteReadRepository providerSiteReadRepository, IProviderSiteWriteRepository providerSiteWriteRepository, IVacancyPartyReadRepository vacancyPartyReadRepository, IVacancyPartyWriteRepository vacancyPartyWriteRepository, ILogService logService)
        {
            _organisationService = organisationService;
            _providerReadRepository = providerReadRepository;
            _providerWriteRepository = providerWriteRepository;
            _providerSiteReadRepository = providerSiteReadRepository;
            _providerSiteWriteRepository = providerSiteWriteRepository;
            _vacancyPartyReadRepository = vacancyPartyReadRepository;
            _vacancyPartyWriteRepository = vacancyPartyWriteRepository;
            _logService = logService;
        }

        public Provider GetProviderViaOwnerParty(int vacancyPartyId)
        {
            var vacancyParty = _vacancyPartyReadRepository.Get(vacancyPartyId);
            return _providerReadRepository.Get(vacancyParty.ProviderSiteId);
        }

        public Provider GetProvider(string ukprn)
        {
            Condition.Requires(ukprn).IsNotNullOrEmpty();

            _logService.Debug("Calling ProviderReadRepository to get provider with UKPRN='{0}'.", ukprn);

            var provider = _providerReadRepository.GetViaUkprn(ukprn);

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

        public ProviderSite GetProviderSiteViaOwnerParty(int vacancyPartyId)
        {
            throw new System.NotImplementedException();
        }

        public ProviderSite GetProviderSite(int providerSiteId)
        {
            throw new System.NotImplementedException();
        }

        public ProviderSite GetProviderSite(string ukprn, string edsErn)
        {
            Condition.Requires(ukprn).IsNotNullOrEmpty();
            Condition.Requires(edsErn).IsNotNullOrEmpty();

            _logService.Debug("Calling ProviderSiteReadRepository to get provider site with UKPRN='{0}' and ERN='{1}'.", ukprn, edsErn);

            var providerSite = _providerSiteReadRepository.Get(edsErn);

            if (providerSite != null)
            {
                return providerSite;
            }

            _logService.Debug("Calling OrganisationService to get provider site with UKPRN='{0}' and ERN='{1}'.", ukprn, edsErn);

            providerSite = _organisationService.GetProviderSite(ukprn, edsErn);

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

        public VacancyParty GetVacancyParty(int vacancyPartyId)
        {
            return _vacancyPartyReadRepository.Get(vacancyPartyId);
        }

        public VacancyParty GetVacancyParty(int providerSiteId, int employerId)
        {
            Condition.Requires(providerSiteId);
            Condition.Requires(employerId);

            _logService.Debug("Calling ProviderSiteEmployerLinkReadRepository to get provider site employer link for provider site with ERN='{0}' and employer with ERN='{1}'.", providerSiteId, employerId);

            var vacancyParty = _vacancyPartyReadRepository.Get(providerSiteId, employerId);

            if (vacancyParty != null)
            {
                return vacancyParty;
            }

            _logService.Debug("Calling OrganisationService to get provider site employer link for provider site with ERN='{0}' and employer with ERN='{1}'.", providerSiteId, employerId);

            vacancyParty = _organisationService.GetVacancyParty(providerSiteId, employerId);

            _logService.Debug("Calling OrganisationService to get provider site employer link for provider site with ERN='{0}' and employer with ERN='{1}'.", providerSiteId, employerId);

            if (vacancyParty == null)
            {
                var employer = _organisationService.GetEmployer(employerId);

                //TODO: Where should employer description and web site come from
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

        private IEnumerable<VacancyParty> GetVacancyParties(EmployerSearchRequest request)
        {
            Condition.Requires(request).IsNotNull();

            _logService.Debug("Calling OrganisationService to get provider site employer link for provider site with ERN='{0}'.", request.ProviderSiteId);

            var providerSiteEmployerLinks = _organisationService.GetProviderSiteEmployerLinks(request);

            _logService.Debug("Calling ProviderSiteEmployerLinkReadRepository to get provider site employer link for provider site with ERN='{0}'.", request.ProviderSiteId);

            var providerSiteEmployerLinksFromRepository = GetVacancyPartiesFromRepository(request);

            //Combine with results from repository
            providerSiteEmployerLinks = providerSiteEmployerLinksFromRepository.Union(providerSiteEmployerLinks, new VacancyPartyEqualityComparer()).ToList();

            return providerSiteEmployerLinks;
        }

        private IEnumerable<VacancyParty> GetVacancyPartiesFromRepository(EmployerSearchRequest request)
        {
            //TODO: All this needs moving into the employer repository 
            /*var providerSiteEmployerLinksFromRepository = _vacancyPartyReadRepository.GetForProviderSite(request.ProviderSiteEdsErn);
            //TODO: This search and the search object should be pushed into the datalayer once it's SQL based
            if (request.IsEmployerEdsUrnQuery)
            {
                providerSiteEmployerLinksFromRepository =
                    providerSiteEmployerLinksFromRepository.Where(l => l.Employer.Ern == request.EmployerEdsUrn);
            }
            else if (request.IsNameAndLocationQuery)
            {
                providerSiteEmployerLinksFromRepository =
                    providerSiteEmployerLinksFromRepository.Where(
                        l =>
                            l.Employer.Name.ToLower().Contains(request.Name.ToLower()) &&
                            ((l.Employer.Address.Postcode != null &&
                              l.Employer.Address.Postcode.ToLower().Contains(request.Location.ToLower())) ||
                             (l.Employer.Address.AddressLine4 != null &&
                              l.Employer.Address.AddressLine4.ToLower().Contains(request.Location.ToLower()))));
            }
            else if (request.IsNameQuery)
            {
                providerSiteEmployerLinksFromRepository =
                    providerSiteEmployerLinksFromRepository.Where(
                        l =>
                            l.Employer.Name.ToLower().Contains(request.Name.ToLower()));
            }
            else if (request.IsLocationQuery)
            {
                providerSiteEmployerLinksFromRepository =
                    providerSiteEmployerLinksFromRepository.Where(
                        l =>
                            ((l.Employer.Address.Postcode != null &&
                              l.Employer.Address.Postcode.ToLower().Contains(request.Location.ToLower())) ||
                             (l.Employer.Address.AddressLine4 != null &&
                              l.Employer.Address.AddressLine4.ToLower().Contains(request.Location.ToLower()))));
            }

            return providerSiteEmployerLinksFromRepository;*/

            return new List<VacancyParty>();
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
