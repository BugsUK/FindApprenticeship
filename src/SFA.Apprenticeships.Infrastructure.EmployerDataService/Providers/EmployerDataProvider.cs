namespace SFA.Apprenticeships.Infrastructure.EmployerDataService.Providers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Application.Interfaces.Locations;
    using SFA.Infrastructure.Interfaces;
    using Application.Organisation;
    using Configuration;
    using Domain.Entities.Exceptions;
    using Domain.Entities.Raa.Locations;
    using Domain.Entities.Raa.Parties;
    using EmployerDataService;
    using Mappers;

    using SFA.Apprenticeships.Application.Interfaces;

    using WebServices.Wcf;
    using ErrorCodes = ErrorCodes;

    public class EmployerDataProvider : IVerifiedOrganisationProvider
    {
        private const string EndpointConfigurationName = "EmployerDataService";

        private readonly ILogService _logger;
        private readonly IConfigurationService _configurationService;
        private readonly IPostalAddressService _postalAddressService;
        private readonly IWcfService<EmployerLookupSoap> _service;

        private string _credentials;
        private readonly EmployerMapper _employerMapper;

        public EmployerDataProvider(
            ILogService logger,
            IConfigurationService configurationService,
            IWcfService<EmployerLookupSoap> service,
            IPostalAddressService postalAddressService)
        {
            _logger = logger;
            _configurationService = configurationService;
            _service = service;
            _postalAddressService = postalAddressService;
            _employerMapper = new EmployerMapper();
        }

        public VerifiedOrganisationSummary GetByReferenceNumber(string referenceNumber)
        {
            var context = new
            {
                referenceNumber
            };

            try
            {
                _logger.Debug("Calling EmployerDataService.ByUrn with reference number='{0}'", referenceNumber);

                DetailedEmployerStructure employer = null;

                _service.Use(EndpointConfigurationName, client =>
                    employer = client.Fetch(Convert.ToInt32(referenceNumber), false, Credentials));

                if (employer == null)
                {
                    _logger.Debug("EmployerDataService.ByUrn did not find employer with reference number='{0}'", referenceNumber);
                    return null;
                }

                _logger.Debug("EmployerDataService.ByUrn found employer with reference number='{0}'", referenceNumber);
                
                return GetVerifiedOrganisationSummary(employer);
            }
            catch (BoundaryException e)
            {
                _logger.Error(e, context);
                throw new DomainException(ErrorCodes.GetByReferenceNumberFailed, e, context);
            }
            catch (Exception e)
            {
                _logger.Error(e, context);
                throw;
            }
        }

        public IEnumerable<VerifiedOrganisationSummary> Find(string employerName, string postcodeOrTown, out int resultCount)
        {
            var context = new
            {
                employerName,
                postcodeOrTown
            };

            try
            {
                _logger.Debug($"Calling EmployerDataService.ByFreeText with reference number='{employerName}', post code or town='{postcodeOrTown}'");

                ConciseEmployerStructure[] employers = null;

                const bool isPhonetic = false;
                const string orgWorkplace = null;
                const string region = null;

                _service.Use(EndpointConfigurationName, client =>
                    employers = client.ByOrganisationAndTrading(employerName, postcodeOrTown, isPhonetic, orgWorkplace, region, Credentials));

                if (employers == null || employers.Length == 0)
                {
                    _logger.Debug($"EmployerDataService.ByFreeText did not find any employers with reference number='{employerName}', post code or town='{postcodeOrTown}'");
                    resultCount = 0;
                    return new List<VerifiedOrganisationSummary>();
                }

                _logger.Debug($"EmployerDataService.ByFreeText {0} employer(s) with reference number='{employerName}', post code or town='{postcodeOrTown}'");

                resultCount = employers.Length;
                var results = employers.Select(GetVerifiedOrganisationSummary);

                return results;
            }
            catch (BoundaryException e)
            {
                _logger.Error(e, context);
                throw new DomainException(ErrorCodes.FindFailed, e, context);
            }
            catch (Exception e)
            {
                _logger.Error(e, context);
                throw;
            }
        }

        public VerifiedOrganisationSummary GetVerifiedOrganisationSummary(DetailedEmployerStructure employer)
        {
            var validatedAddress = GetValidatedAddress(employer.Name, employer.Address);

            var organisationSummary = _employerMapper.ToVerifiedOrganisationSummary(employer);

            organisationSummary.Address = validatedAddress ?? organisationSummary.Address;

            return organisationSummary;
        }

        public VerifiedOrganisationSummary GetVerifiedOrganisationSummary(ConciseEmployerStructure employer)
        {
            var validatedAddress = GetValidatedAddress(employer.Name, employer.Address);

            var organisationSummary = _employerMapper.ToVerifiedOrganisationSummary(employer);

            organisationSummary.Address = validatedAddress ?? organisationSummary.Address;

            return organisationSummary;
        }

        /// <summary>
        /// This is a public method so as to be testable.
        /// The SOAP service result is not injectable, making this code impossible to reach if it were invoked
        /// as part of the interface methods.
        /// *This is a nasty piece of code to test, but there are some units around it.
        /// </summary>
        /// <param name="companyName"></param>
        /// <param name="address"></param>
        /// <returns></returns>
        private PostalAddress GetValidatedAddress(string companyName, BSaddressStructure address)
        {
            var street = address.StreetDescription;
            var primaryAddressableObject = address.PAON == null ? null : string.Join(", ", address.PAON.Items);
            var secondaryAddressableObject = address.SAON == null ? null : string.Join(", ", address.SAON.Items);
            var town = address.PostTown;
            var postcode = address.PostCode;

            return _postalAddressService.GetPostalAddresses(companyName, primaryAddressableObject, secondaryAddressableObject, street, town, postcode);
        }

        #region Helpers

        private string Credentials
        {
            get
            {
                if (_credentials == null)
                {
                    _credentials = _configurationService.Get<EmployerDataServiceConfiguration>().Credentials;
                }

                return _credentials;
            }
        }

        #endregion
    }
}
