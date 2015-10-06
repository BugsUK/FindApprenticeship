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

        public IEnumerable<Employer> GetEmployers(string ern)
        {
            Condition.Requires(ern).IsNotNullOrEmpty();

            _logService.Debug("Calling EmployerReadRepository to get employers for provider site with ERN='{0}'.", ern);

            IEnumerable<Employer> employers = _employerReadRepository.GetForProviderSite(ern).ToList();

            if (employers.Any())
            {
                return employers;
            }

            _logService.Debug("Calling OrganisationService to get employers for provider site with ERN='{0}'.", ern);

            employers = _organisationService.GetEmployers(ern);

            return employers;
        }
    }
}
