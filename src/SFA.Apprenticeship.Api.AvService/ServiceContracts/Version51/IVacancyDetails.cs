namespace SFA.Apprenticeship.Api.AvService.ServiceContracts.Version51
{
    using System.ServiceModel;
    using FaultContracts.Version51;
    using MessageContracts.Version51;
    using Namespaces.Version51;

    [ServiceContract(Namespace = Namespace.Uri)]
    public interface IVacancyDetails
    {
        [OperationContract]
        [FaultContract(typeof(SystemFaultContract))]
        VacancyDetailsResponse Get(VacancyDetailsRequest request);
    }
}
