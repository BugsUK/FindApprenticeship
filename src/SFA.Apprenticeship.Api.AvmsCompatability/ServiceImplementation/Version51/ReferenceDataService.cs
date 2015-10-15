namespace SFA.Apprenticeship.Api.AvmsCompatability.ServiceImplementation.Version51
{
    using System.ServiceModel;
    using AvmsCompatability;
    using MessageContracts.Version51;
    using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.WCF;
    using ServiceContracts.Version51;

    [ExceptionShielding( "Default Exception Policy" )]
    [ServiceBehavior(Namespace = CommonNamespaces.ExternalInterfacesRel51)]
    public class ReferenceDataService : IReferenceData
    {
        public GetErrorCodesResponse GetErrorCodes(GetErrorCodesRequest request)
        {
            throw new System.NotImplementedException();
        }

        public GetApprenticeshipFrameworksResponse GetApprenticeshipFrameworks(GetApprenticeshipFrameworksRequest request)
        {
            throw new System.NotImplementedException();
        }

        public GetRegionResponse GetRegion(GetRegionRequest request)
        {
            throw new System.NotImplementedException();
        }

        public GetCountiesResponse GetCounties(GetCountiesRequest request)
        {
            throw new System.NotImplementedException();
        }

        public GetLocalAuthoritiesResponse GetLocalAuthorities(GetLocalAuthoritiesRequest request)
        {
            throw new System.NotImplementedException();
        }
    }
}
