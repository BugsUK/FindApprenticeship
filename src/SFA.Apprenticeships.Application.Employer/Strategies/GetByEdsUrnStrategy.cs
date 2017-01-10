namespace SFA.Apprenticeships.Application.Employer.Strategies
{
    using System.Collections.Generic;
    using Domain.Entities.Exceptions;
    using Domain.Entities.Raa.Parties;
    using Domain.Raa.Interfaces.Repositories;
    using Interfaces;
    using Interfaces.Organisations;

    public class GetByEdsUrnStrategy : IGetByEdsUrnStrategy
    {
        private readonly IEqualityComparer<Employer> _employerVerifiedOrganisationComparer = new EmployerVerifiedOrganisationComparer();

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

            //Retrieve the master record of the employer from the service and use to update the employer if neccessary
            var organisationSummary = _organisationService.GetVerifiedOrganisationSummary(edsUrn);
            if (organisationSummary != null)
            {
                var referenceEmployer = _mapper.Map<VerifiedOrganisationSummary, Employer>(organisationSummary);

                if (referenceEmployer.Address.AddressLine1 == null)
                {
                    throw new CustomException(Application.Employer.ErrorCodes.InvalidAddress);
                }

                if (employer == null)
                {
                    employer = _employerWriteRepository.Save(referenceEmployer);
                }

                if (!_employerVerifiedOrganisationComparer.Equals(employer, referenceEmployer))
                {
                    //Update employer with new data and save
                    employer.FullName = referenceEmployer.FullName;
                    employer.TradingName = referenceEmployer.TradingName;
                    employer.Address = referenceEmployer.Address;
                    employer = _employerWriteRepository.Save(employer);
                }
            }

            return employer;
        }
    }
}