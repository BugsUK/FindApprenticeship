namespace SFA.Apprenticeship.Api.AvService.ServiceContracts.Version51
{
    using System.ServiceModel;
    using FaultContracts.Version51;
    using MessageContracts.Version51;
    using Namespaces.Version51;

    [ServiceContract(Namespace = Namespace.Uri)]
    public interface IVacancyManagement
    {
        [OperationContract]
        [FaultContract(typeof(SystemFaultContract))]
        VacancyUploadResponse UploadVacancies(VacancyUploadRequest request);
    }
}
