namespace SFA.Apprenticeship.Api.AvService.ServiceImplementation.Version51
{
    using System;
    using System.ServiceModel;
    using Common;
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
            // TODO: US872: AG: unit test.
            return new GetErrorCodesResponse
            {
                MessageId = request.MessageId,
                ErrorCodes = ApiErrors.AllErrorCodes
            };
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
