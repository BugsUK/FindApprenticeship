namespace SFA.Apprenticeship.Api.AvmsCompatability.ServiceImplementation.Version50
{
    using System;
    using MessageContracts.Version50;
    using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.WCF;
    using ServiceContracts.Version50;

    [ExceptionShielding( "Default Exception Policy" )]
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
    }
}
