namespace SFA.Apprenticeship.Api.AvService.ServiceContracts.Version51
{
    using System.ServiceModel;
    using FaultContracts.Version51;
    using MessageContracts.Version51;
    using Namespaces.Version51;

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

        [OperationContract]
        [FaultContract(typeof(SystemFaultContract))]
        GetLocalAuthoritiesResponse GetLocalAuthorities(GetLocalAuthoritiesRequest request);
    }
}
