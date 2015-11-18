namespace SFA.Apprenticeship.Api.AvService.ServiceImplementation.Version51
{
    using System.Security;
    using System.ServiceModel;
    using MessageContracts.Version51;
    using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.WCF;
    using Namespaces.Version51;
    using ServiceContracts.Version51;

    [ExceptionShielding("Default Exception Policy")]
    [ServiceBehavior(Namespace = Namespace.Uri)]
    public class VacancyManagementService : IVacancyManagement
    {
        public VacancyUploadResponse UploadVacancies(VacancyUploadRequest request)
        {
            throw new SecurityException();
        }
    }
}
