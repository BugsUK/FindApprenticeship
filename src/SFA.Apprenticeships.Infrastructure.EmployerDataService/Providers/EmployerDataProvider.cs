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
    using Domain.Entities.Organisations;
    using EmployerDataService;
    using Mappers;
    using WebServices.Wcf;
    using ErrorCodes = Infrastructure.EmployerDataService.ErrorCodes;

    public class EmployerDataProvider : IVerifiedOrganisationProvider
    {
        private const string EndpointConfigurationName = "EmployerDataService";

        private readonly ILogService _logger;
        private readonly IConfigurationService _configurationService;
        private readonly IPostalAddressSearchService _postalAddressSearchService;
        private readonly IWcfService<EmployerLookupSoap> _service;

        private string _credentials;
        private readonly EmployerMapper _employerMapper;

        public EmployerDataProvider(
            ILogService logger,
            IConfigurationService configurationService,
            IWcfService<EmployerLookupSoap> service,
            IPostalAddressSearchService postalAddressSearchService)
        {
            _logger = logger;
            _configurationService = configurationService;
            _service = service;
            _postalAddressSearchService = postalAddressSearchService;
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

                ConciseEmployerStructure[] employers = null;

                _service.Use(EndpointConfigurationName, client =>
                    employers = client.ByUrn(Convert.ToInt32(referenceNumber), Credentials));

                if (employers == null || employers.Length == 0)
                {
                    _logger.Debug("EmployerDataService.ByUrn did not find employer with reference number='{0}'", referenceNumber);
                    return null;
                }

                if (employers.Length > 1)
                {
                    // TODO: AG: could this ever be the case when fetching by reference number?
                    _logger.Warn("EmployerDataService.ByUrn found multiple matches for reference number='{0}', will return first result", referenceNumber);
                }

                var employerToMap = employers.First();

                _logger.Debug("EmployerDataService.ByUrn found employer with reference number='{0}'", referenceNumber);
                
                return ValidatedAddress(employerToMap);
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

                var freeText = string.IsNullOrWhiteSpace(postcodeOrTown)
                    ? $"{employerName}"
                    : $"{employerName}, {postcodeOrTown}";

                _service.Use(EndpointConfigurationName, client =>
                    employers = client.ByFreeText(freeText, isPhonetic, orgWorkplace, region, Credentials));

                if (employers == null || employers.Length == 0)
                {
                    _logger.Debug($"EmployerDataService.ByFreeText did not find any employers with reference number='{employerName}', post code or town='{postcodeOrTown}'");
                    resultCount = 0;
                    return new List<VerifiedOrganisationSummary>();
                }

                _logger.Debug($"EmployerDataService.ByFreeText {0} employer(s) with reference number='{employerName}', post code or town='{postcodeOrTown}'");

                resultCount = employers.Length;
                // TODO: AG: US814: include reference number aliases.
                var results = employers.Select(ValidatedAddress);

                return results;
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

        /// <summary>
        /// This is a public method so as to be testable.
        /// The SOAP service result is not injectable, making this code impossible to reach if it were invoked
        /// as part of the interface methods.
        /// </summary>
        /// <param name="employerToMap"></param>
        /// <returns></returns>
        public VerifiedOrganisationSummary ValidatedAddress(ConciseEmployerStructure employerToMap)
        {
            //OO 20/01/16: Employer Address validation
            //Here, we are replicating what AVMS does for address verification
            //get address by postcode and line 1
            //if address not found, get address by postcode alone
            //if address not found, just use the retrieved address
            var postCodeToSearch = employerToMap.Address.PostCode;
            var addressLine1 = employerToMap.Address.PAON?.Items.FirstOrDefault()?.ToString();
            var verifiedAddress = _postalAddressSearchService.GetValidatedAddress(postCodeToSearch, addressLine1) ??
                                  _postalAddressSearchService.GetValidatedAddresses(postCodeToSearch)?.FirstOrDefault();

            // TODO: AG: US814: include reference number aliases.
            var employerResult = _employerMapper.ToVerifiedOrganisationSummary(employerToMap, Enumerable.Empty<string>());

            employerResult.Address = verifiedAddress ?? employerResult.Address;

            return employerResult;
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
