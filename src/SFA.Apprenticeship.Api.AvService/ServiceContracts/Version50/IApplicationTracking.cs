namespace SFA.Apprenticeship.Api.AvService.ServiceContracts.Version50
{
    using System.ServiceModel;
    using AvService;
    using FaultContracts.Version50;
    using MessageContracts.Version50;

    [ServiceContract(Namespace=CommonNamespaces.ExternalInterfaces)]
    public interface IApplicationTracking
    {
        [OperationContract]
        [FaultContract(typeof(SystemFaultContract))]
        SubmitApplicationTrackingResponse Submit( SubmitApplicationTrackingRequest request );
    }
}
