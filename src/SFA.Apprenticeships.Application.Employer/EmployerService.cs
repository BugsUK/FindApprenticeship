namespace SFA.Apprenticeships.Application.Employer
{
    using System.Collections.Generic;
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
        private readonly IEmployerWriteRepository _employerWriteRepository;
        private readonly ILogService _logService;

        public EmployerService(IOrganisationService organisationService, IEmployerReadRepository employerReadRepository, IEmployerWriteRepository employerWriteRepository, ILogService logService)
        {
            _organisationService = organisationService;
            _employerReadRepository = employerReadRepository;
            _employerWriteRepository = employerWriteRepository;
            _logService = logService;
        }

        public Employer GetEmployer(string ern)
        {
            Condition.Requires(ern).IsNotNullOrEmpty();

            _logService.Debug("Calling EmployerReadRepository to get employer with ERN='{0}'.", ern);

            var employer = _employerReadRepository.Get(ern);

            if (employer != null)
            {
                return employer;
            }

            _logService.Debug("Calling OrganisationService to get employer with ERN='{0}'.", ern);

            employer = _organisationService.GetEmployer(ern);

            return employer;
        }

        public Employer SaveEmployer(Employer employer)
        {
            return _employerWriteRepository.Save(employer);
        }

        public IEnumerable<Employer> GetEmployers(string ern, string name, string location)
        {
            return _organisationService.GetEmployers(ern, name, location);
        }
    }
}
