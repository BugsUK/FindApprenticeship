namespace SFA.Apprenticeship.Api.AvService.ServiceImplementation.Version51
{
    using System;
    using System.ServiceModel;
    using SFA.Infrastructure.Interfaces;
    using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.WCF;
    using MessageContracts.Version51;
    using Namespaces.Version51;
    using ServiceContracts.Version51;

    [ExceptionShielding("Default Exception Policy")]
    [ServiceBehavior(Namespace = Namespace.Uri)]
    public class ReferenceDataService : IReferenceData
    {
        public GetErrorCodesResponse GetErrorCodes(GetErrorCodesRequest request)
        {
            throw new NotImplementedException();
        }

        public GetApprenticeshipFrameworksResponse GetApprenticeshipFrameworks(GetApprenticeshipFrameworksRequest request)
        {
            throw new NotImplementedException();
        }

        public GetRegionResponse GetRegion(GetRegionRequest request)
        {
            throw new NotImplementedException();
        }

        public GetCountiesResponse GetCounties(GetCountiesRequest request)
        {
            throw new NotImplementedException();
        }

        public GetLocalAuthoritiesResponse GetLocalAuthorities(GetLocalAuthoritiesRequest request)
        {
            throw new NotImplementedException();
        }
    }
}
