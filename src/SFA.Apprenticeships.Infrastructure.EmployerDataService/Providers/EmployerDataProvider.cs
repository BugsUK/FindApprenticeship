namespace SFA.Apprenticeships.Infrastructure.EmployerDataService.Providers
{
    using System;
    using System.Linq;
    using Application.Interfaces.Logging;
    using Application.Organisation;
    using Configuration;
    using Domain.Entities.Exceptions;
    using Domain.Entities.Organisations;
    using Domain.Interfaces.Configuration;
    using EmployerDataService;
    using Mappers;
    using WebServices.Wcf;

    public class EmployerDataProvider : IVerifiedOrganisationProvider
    {
        private const string EndpointConfigurationName = "EmployerDataService";

        private readonly ILogService _logger;
        private readonly IConfigurationService _configurationService;
        private readonly IWcfService<EmployerLookupSoapClient> _service;

        private string _credentials;
        private readonly EmployerMapper _employerMapper;

        public EmployerDataProvider(
            ILogService logger,
            IConfigurationService configurationService,
            IWcfService<EmployerLookupSoapClient> service)
        {
            _logger = logger;
            _configurationService = configurationService;
            _service = service;
            _employerMapper = new EmployerMapper();
        }

        public Organisation GetByReferenceNumber(string referenceNumber)
        {
            var context = new
            {
                referenceNumber
            };

            try
            {
                _logger.Debug("Calling EmployerDataService.ByUrn with reference number='{0}'", referenceNumber);

                ConciseEmployerStructure[] employers = null;

                _service.Use(
                    EndpointConfigurationName,
                    client => employers = client.ByUrn(Convert.ToInt32(referenceNumber), Credentials));

                if (employers == null || employers.Length == 0)
                {
                    _logger.Debug("EmployerDataService.ByUrn did not find employer with reference number='{0}'", referenceNumber);
                    return null;
                }

                var organisation = _employerMapper.ToVerifiedOrganisation(employers.First());

                _logger.Debug("EmployerDataService.ByUrn found employer with reference number='{0}'", referenceNumber);

                return organisation;
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

        #region Helpers

        private string Credentials
        {
            get
            {
                if (_credentials == null)
                {
                    var configuration = _configurationService.Get<EmployerDataServiceConfiguration>();

                    _credentials = configuration.Credentials;
                }

                return _credentials;
            }
        }

        #endregion
    }
}
