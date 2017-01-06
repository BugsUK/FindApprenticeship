namespace SFA.Apprenticeships.Application.Employer.Strategies
{
    using System.Collections.Generic;
    using System.Linq;
    using Domain.Entities.Exceptions;
    using Domain.Entities.Raa.Locations;
    using Domain.Entities.Raa.Parties;
    using Domain.Raa.Interfaces.Repositories;
    using Interfaces;
    using Interfaces.Organisations;
    using Location;

    public class GetByEdsUrnStrategy : IGetByEdsUrnStrategy
    {
        private readonly IEqualityComparer<Employer> _employerVerifiedOrganisationComparer = new EmployerVerifiedOrganisationComparer();

        private readonly IEmployerReadRepository _employerReadRepository;
        private readonly IEmployerWriteRepository _employerWriteRepository;
        private readonly IOrganisationService _organisationService;
        private readonly IAddressLookupProvider _addressLookupProvider;
        private readonly IMapper _mapper;
        private readonly ILogService _logService;

        public GetByEdsUrnStrategy(IEmployerReadRepository employerReadRepository, IEmployerWriteRepository employerWriteRepository, IOrganisationService organisationService, IAddressLookupProvider addressLookupProvider, IMapper mapper, ILogService logService)
        {
            _employerReadRepository = employerReadRepository;
            _employerWriteRepository = employerWriteRepository;
            _organisationService = organisationService;
            _mapper = mapper;
            _logService = logService;
            _addressLookupProvider = addressLookupProvider;
        }

        public Employer Get(string edsUrn)
        {
            _logService.Debug("Calling Employer Repository to get employer with ERN='{0}'.", edsUrn);

            var employer = _employerReadRepository.GetByEdsUrn(edsUrn);
            if (employer == null)
            {
                _logService.Info("No record of employer with ERN='{0}' found in Employer Repository. Calling Organisation Service to try and get employer", edsUrn);
            }

            //Retrieve the master record of the employer from the service and use to update the employer if neccessary
            var organisationSummary = _organisationService.GetVerifiedOrganisationSummary(edsUrn);
            if (organisationSummary != null)
            {
                var referenceEmployer = _mapper.Map<VerifiedOrganisationSummary, Employer>(organisationSummary);

                PatchCountyAndTown(referenceEmployer, organisationSummary.Address);

                if (referenceEmployer.Address.AddressLine1 == null)
                {
                    throw new CustomException(Application.Employer.ErrorCodes.InvalidAddress);
                }

                if (!_employerVerifiedOrganisationComparer.Equals(employer, referenceEmployer))
                {
                    //Geocode?
                    employer = _employerWriteRepository.Save(employer);
                }
            }

            return employer;
        }

        private void PatchCountyAndTown(Employer employer, PostalAddress employerAddress)
        {
            // There are some special cases where the town is not set either.
            // We need the town to be different from null to be able to insert the employer in the DB
            PatchTown(employer);

            PatchCounty(employer, employerAddress);
        }

        private void PatchCounty(Employer employer, PostalAddress postalAddress)
        {
            if (string.IsNullOrWhiteSpace(employer.Address.County))
            {
                // If we can find the address on postcode anywhere, we patch the county.
                // If we can't, we patch the county from the employer's address itself
                var address =
                    _addressLookupProvider.GetPossibleAddresses(postalAddress.Postcode)
                        .FirstOrDefault();

                employer.Address.County = address != null ? address.County : employer.Address.Town;
            }
        }

        private static void PatchTown(Employer employer)
        {
            if (string.IsNullOrWhiteSpace(employer.Address.Town))
            {
                employer.Address.Town = employer.Address.AddressLine5 ?? employer.Address.AddressLine4 ?? employer.Address.AddressLine3;
            }
        }
    }
}