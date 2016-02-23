namespace SFA.Apprenticeships.Application.Employer
{
    using System.Collections.Generic;
    using CuttingEdge.Conditions;
    using Domain.Entities.Raa.Parties;
    using Interfaces.Employers;
    using Interfaces.Generic;
    using Infrastructure.Interfaces;
    using Interfaces.Organisations;

    public class EmployerService : IEmployerService
    {
        private readonly IOrganisationService _organisationService;
        private readonly ILogService _logService;

        public EmployerService(IOrganisationService organisationService, ILogService logService)
        {
            _organisationService = organisationService;
            _logService = logService;
        }

        public Employer GetEmployer(int employerId)
        {
            Condition.Requires(employerId);

            _logService.Debug("Calling OrganisationService to get employer with Id='{0}'.", employerId);

            return _organisationService.GetEmployer(employerId);
        }

        public IEnumerable<Employer> GetEmployers(IEnumerable<int> employerIds)
        {
            return _organisationService.GetByIds(employerIds);
        }

        public IEnumerable<Employer> GetEmployers(string edsUrn, string name, string location)
        {
            return _organisationService.GetEmployers(edsUrn, name, location);
        }

        public Pageable<Employer> GetEmployers(string edsUrn, string name, string location, int currentPage, int pageSize)
        {
            return _organisationService.GetEmployers(edsUrn, name, location, currentPage, pageSize);
        }
    }
}
