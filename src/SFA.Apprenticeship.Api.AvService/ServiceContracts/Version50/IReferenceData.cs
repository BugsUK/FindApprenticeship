namespace SFA.Apprenticeship.Api.AvService.ServiceContracts.Version50
{
    using System.ServiceModel;
    using FaultContracts.Version50;
    using MessageContracts.Version50;
    using Namespaces.Version50;

    [ServiceContract(Namespace = Namespace.Uri)]
    public interface IReferenceData
    {
        [OperationContract]
        [FaultContract(typeof(SystemFaultContract))]
        GetErrorCodesResponse GetErrorCodes(GetErrorCodesRequest request);

        [OperationContract]
        [FaultContract(typeof(SystemFaultContract))]
        GetApprenticeshipFrameworksResponse GetApprenticeshipFrameworks(GetApprenticeshipFrameworksRequest request);

        [OperationContract]
        [FaultContract(typeof(SystemFaultContract))]
        GetRegionResponse GetRegion(GetRegionRequest request);

        [OperationContract]
        [FaultContract(typeof(SystemFaultContract))]
        GetCountiesResponse GetCounties(GetCountiesRequest request);
    }
}
