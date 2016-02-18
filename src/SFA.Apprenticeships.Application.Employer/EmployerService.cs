namespace SFA.Apprenticeships.Application.Employer
{
    using System.Collections.Generic;
    using CuttingEdge.Conditions;
    using Domain.Entities.Raa.Parties;
    using Domain.Raa.Interfaces.Repositories;
    using Interfaces.Employers;
    using Interfaces.Generic;
    using Infrastructure.Interfaces;
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

        public Employer GetEmployer(string edsErn)
        {
            Condition.Requires(edsErn).IsNotNullOrEmpty();

            _logService.Debug("Calling EmployerReadRepository to get employer with ERN='{0}'.", edsErn);

            var employer = _employerReadRepository.Get(edsErn);

            if (employer != null)
            {
                return employer;
            }

            _logService.Debug("Calling OrganisationService to get employer with ERN='{0}'.", edsErn);

            employer = _organisationService.GetEmployer(edsErn);

            return employer;
        }

        public Employer SaveEmployer(Employer employer)
        {
            return _employerWriteRepository.Save(employer);
        }

        public IEnumerable<Employer> GetEmployers(string edsErn, string name, string location)
        {
            return _organisationService.GetEmployers(edsErn, name, location);
        }

        public Pageable<Employer> GetEmployers(string edsErn, string name, string location, int currentPage, int pageSize)
        {
            return _organisationService.GetEmployers(edsErn, name, location, currentPage, pageSize);
        }
    }
}
