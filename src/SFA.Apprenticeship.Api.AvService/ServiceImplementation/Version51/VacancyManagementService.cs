namespace SFA.Apprenticeship.Api.AvService.ServiceImplementation.Version51
{
    using System;
    using System.Security;
    using System.ServiceModel;
    using Apprenticeships.Application.Interfaces.Logging;
    using Mediators.Version51;
    using MessageContracts.Version51;
    using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.WCF;
    using Namespaces.Version51;
    using ServiceContracts.Version51;

    [ExceptionShielding("Default Exception Policy")]
    [ServiceBehavior(Namespace = Namespace.Uri)]
    public class VacancyManagementService : IVacancyManagement
    {
        private readonly ILogService _logService;
        private readonly IVacancyUploadServiceMediator _vacancyUploadMediator;

        public VacancyManagementService(
            ILogService logService,
            IVacancyUploadServiceMediator vacancyUploadMediator)
        {
            _logService = logService;
            _vacancyUploadMediator = vacancyUploadMediator;
        }

        public VacancyUploadResponse UploadVacancies(VacancyUploadRequest request)
        {
            object context = new
            {
                request?.ExternalSystemId,
                request?.MessageId
            };

            try
            {
                if (request == null)
                {
                    throw new ArgumentNullException(nameof(request));
                }

                // TODO: API: AG: remove test code.
                if (request.MessageId == Guid.Empty)
                {
                    throw new SecurityException();
                }

                return _vacancyUploadMediator.UploadVacancies(request);
            }
            catch (Exception e)
            {
                _logService.Error(e, context);
                throw;
            }
        }
    }
}
