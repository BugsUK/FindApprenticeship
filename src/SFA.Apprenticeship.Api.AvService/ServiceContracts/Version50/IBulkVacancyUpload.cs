namespace SFA.Apprenticeship.Api.AvService.ServiceContracts.Version50
{
    using System.ServiceModel;
    using FaultContracts.Version50;
    using MessageContracts.Version50;

    [ServiceContract(Namespace=CommonNamespaces.ExternalInterfaces)]
    public interface IBulkVacancyUpload
    {
        [OperationContract]
        [FaultContract(typeof(SystemFaultContract))]
        BulkVacancyUploadResponse UploadVacancies( BulkVacancyUploadRequest request );
    }
}
