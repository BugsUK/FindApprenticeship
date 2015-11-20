namespace SFA.Apprenticeship.Api.AvService.ServiceImplementation.Version51
{
    using System;
    using System.Security;
    using System.ServiceModel;
    using MessageContracts.Version51;
    using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.WCF;
    using Namespaces.Version51;
    using Providers.Version51;
    using ServiceContracts.Version51;

    [ExceptionShielding("Default Exception Policy")]
    [ServiceBehavior(Namespace = Namespace.Uri)]
    public class VacancyManagementService : IVacancyManagement
    {
        private readonly IVacancyUploadProvider _vacancyUploadProvider;

        public VacancyManagementService(IVacancyUploadProvider vacancyUploadProvider)
        {
            _vacancyUploadProvider = vacancyUploadProvider;
        }

        public VacancyUploadResponse UploadVacancies(VacancyUploadRequest request)
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

            return _vacancyUploadProvider.UploadVacancies(request);
        }
    }
}
