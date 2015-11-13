﻿namespace SFA.Apprenticeship.Api.AvService.ServiceContracts.Version50
{
    using System.ServiceModel;
    using FaultContracts.Version50;
    using MessageContracts.Version50;
    using Namespaces.Version50;

    [ServiceContract(Namespace = Namespace.Uri)]
    public interface IApplicationTracking
    {
        [OperationContract]
        [FaultContract(typeof(SystemFaultContract))]
        SubmitApplicationTrackingResponse Submit(SubmitApplicationTrackingRequest request);
    }
}