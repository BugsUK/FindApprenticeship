namespace SFA.Apprenticeships.Application.Employer.Strategies
{
    using Domain.Entities.Raa.Parties;
    using Domain.Raa.Interfaces.Repositories;
    using Interfaces.Organisations;
    using SFA.Infrastructure.Interfaces;

    public class GetByEdsUrnStrategy : IGetByEdsUrnStrategy
    {
        private readonly IEmployerReadRepository _employerReadRepository;
        private readonly IEmployerWriteRepository _employerWriteRepository;
        private readonly IOrganisationService _organisationService;
        private readonly IMapper _mapper;
        private readonly ILogService _logService;

        public GetByEdsUrnStrategy(IEmployerReadRepository employerReadRepository, IEmployerWriteRepository employerWriteRepository, IOrganisationService organisationService, IMapper mapper, ILogService logService)
        {
            _employerReadRepository = employerReadRepository;
            _employerWriteRepository = employerWriteRepository;
            _organisationService = organisationService;
            _mapper = mapper;
            _logService = logService;
        }

        public Employer Get(string edsUrn)
        {
            _logService.Debug("Calling Employer Repository to get employer with ERN='{0}'.", edsUrn);

            var employer = _employerReadRepository.GetByEdsUrn(edsUrn);
            if (employer == null)
            {
                _logService.Info("No record of employer with ERN='{0}' found in Employer Repository. Calling Organisation Service to get employer", edsUrn);

                var organisationSummary = _organisationService.GetVerifiedOrganisationSummary(edsUrn);
                
                //We don't know about this employer yet so get the reference from the organisation service and store for future use
                employer = _mapper.Map<VerifiedOrganisationSummary, Employer>(organisationSummary);
                employer = _employerWriteRepository.Save(employer);
            }

            return employer;
        }
    }
}