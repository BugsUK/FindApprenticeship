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
    using Wcf = WebServices.Wcf;

    public class EmployerDataProvider : IVerifiedOrganisationProvider
    {
        private readonly ILogService _logger;
        private readonly IConfigurationService _configurationService;
        private readonly Wcf.IWcfService<EmployerLookupSoapClient> _service;

        private string _credentials;
        private readonly EmployerMapper _employerMapper;

        public EmployerDataProvider(
            ILogService logger,
            IConfigurationService configurationService,
            Wcf.IWcfService<EmployerLookupSoapClient> service)
        {
            _logger = logger;
            _configurationService = configurationService;
            _service = service;
            _employerMapper = new EmployerMapper();
        }

        public VerifiedOrganisation GetByReferenceNumber(string referenceNumber)
        {
            var context = new
            {
                referenceNumber
            };

            try
            {
                _logger.Debug("Calling EmployerDataService.ByUrn with reference number='{0}'", referenceNumber);

                ConciseEmployerStructure[] employer = null;

                _service.Use("EmployerDataService", client => employer = client.ByUrn(ReferenceNumberToInt32(referenceNumber), Credentials));

                if (employer == null || employer.Length == 0)
                {
                    _logger.Debug("EmployerDataService.ByUrn did not find employer with reference number='{0}'", referenceNumber);
                    return null;
                }

                var organisation = _employerMapper.ToVerifiedOrganisation(employer.First());

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

        private static int ReferenceNumberToInt32(string referenceNumber)
        {
            return Convert.ToInt32(referenceNumber);
        }

        private string Credentials
        {
            get
            {
                if (_credentials == null)
                {
                    var configuration = _configurationService.Get<EmployerDataServiceConfiguration>();

                    _credentials = configuration.UserName;
                }

                return _credentials;
            }
        }

        #endregion
    }
}
