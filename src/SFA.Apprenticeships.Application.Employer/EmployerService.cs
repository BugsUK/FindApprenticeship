namespace SFA.Apprenticeships.Application.Employer
{
    using System.Collections.Generic;
    using System.Linq;
    using CuttingEdge.Conditions;
    using Domain.Entities.Organisations;
    using Domain.Interfaces.Repositories;
    using Interfaces.Employers;
    using Interfaces.Logging;
    using Interfaces.Organisations;

    public class EmployerService : IEmployerService
    {
        private readonly IOrganisationService _organisationService;
        private readonly IEmployerReadRepository _employerReadRepository;
        private readonly ILogService _logService;

        public EmployerService(IOrganisationService organisationService, IEmployerReadRepository employerReadRepository, ILogService logService)
        {
            _organisationService = organisationService;
            _employerReadRepository = employerReadRepository;
            _logService = logService;
        }

        public Employer GetEmployer(string providerSiteErn, string ern)
        {
            Condition.Requires(providerSiteErn).IsNotNullOrEmpty();
            Condition.Requires(ern).IsNotNullOrEmpty();

            _logService.Debug("Calling EmployerReadRepository to get employers for provider site with ERN='{0}' and employer with ERN='{1}'.", providerSiteErn, ern);

            //TODO: How are we storing these employers/relationships in our DB?

            _logService.Debug("Calling OrganisationService to get employers for provider site with ERN='{0}' and employer with ERN='{1}'.", providerSiteErn, ern);

            var employer = _organisationService.GetEmployer(providerSiteErn, ern);

            return employer;
        }

        public IEnumerable<Employer> GetEmployers(string providerSiteErn)
        {
            Condition.Requires(providerSiteErn).IsNotNullOrEmpty();

            _logService.Debug("Calling EmployerReadRepository to get employers for provider site with ERN='{0}'.", providerSiteErn);

            IEnumerable<Employer> employers = _employerReadRepository.GetForProviderSite(providerSiteErn).ToList();

            if (employers.Any())
            {
                return employers;
            }

            _logService.Debug("Calling OrganisationService to get employers for provider site with ERN='{0}'.", providerSiteErn);

            employers = _organisationService.GetEmployers(providerSiteErn);

            return employers;
        }
    }
}
