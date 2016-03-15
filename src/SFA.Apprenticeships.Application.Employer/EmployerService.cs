namespace SFA.Apprenticeships.Application.Employer
{
    using System.Collections.Generic;
    using System.Linq;
    using CuttingEdge.Conditions;
    using Domain.Entities.Raa.Parties;
    using Domain.Raa.Interfaces.Repositories;
    using Interfaces.Employers;
    using Interfaces.Generic;
    using Infrastructure.Interfaces;
    using Interfaces.Organisations;

    public class EmployerService : IEmployerService
    {
        //TODO: This class needs to use the repositories not the organization service
        private readonly IOrganisationService _organisationService;
        private readonly IEmployerReadRepository _employerReadRepository;
        private readonly IEmployerWriteRepository _employerWriteRepository;
        private readonly ILogService _logService;

        public EmployerService(IOrganisationService organisationService, ILogService logService, IEmployerReadRepository employerReadRepository, IEmployerWriteRepository employerWriteRepository)
        {
            _organisationService = organisationService;
            _logService = logService;
            _employerReadRepository = employerReadRepository;
            _employerWriteRepository = employerWriteRepository;
        }

        public Employer GetEmployer(int employerId)
        {
            Condition.Requires(employerId);

            _logService.Debug("Calling OrganisationService to get employer with Id='{0}'.", employerId);

            return _employerReadRepository.GetById(employerId) ?? _organisationService.GetEmployer(employerId);
        }

        public Employer GetEmployer(string edsUrn)
        {
            Condition.Requires(edsUrn).IsNotNullOrEmpty();

            _logService.Debug("Calling OrganisationService to get employer with ERN='{0}'.", edsUrn);

            return _employerReadRepository.GetByEdsUrn(edsUrn) ?? _organisationService.GetEmployer(edsUrn);
        }

        public IEnumerable<Employer> GetEmployers(IEnumerable<int> employerIds)
        {
            var ids = employerIds.ToList();
            return _employerReadRepository.GetByIds(ids).Union(_organisationService.GetByIds(ids), new EmployerEqualityComparer());
        }

        public IEnumerable<Employer> GetEmployers(string edsUrn, string name, string location)
        {
            return _organisationService.GetEmployers(edsUrn, name, location);
        }

        public Pageable<Employer> GetEmployers(string edsUrn, string name, string location, int currentPage, int pageSize)
        {
            return _organisationService.GetEmployers(edsUrn, name, location, currentPage, pageSize);
        }

        public Employer SaveEmployer(Employer employer)
        {
            return _employerWriteRepository.Save(employer);
        }
    }
}
