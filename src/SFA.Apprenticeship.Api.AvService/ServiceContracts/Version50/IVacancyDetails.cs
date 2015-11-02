namespace SFA.Apprenticeship.Api.AvService.ServiceContracts.Version50
{
    using System.ServiceModel;
    using FaultContracts.Version50;
    using MessageContracts.Version50;

    [ServiceContract(Namespace=CommonNamespaces.ExternalInterfaces)]
    public interface IVacancyDetails
    {
        [OperationContract]
        [FaultContract(typeof(SystemFaultContract))]
        VacancyDetailsResponse Get( VacancyDetailsRequest request );
    }
}
