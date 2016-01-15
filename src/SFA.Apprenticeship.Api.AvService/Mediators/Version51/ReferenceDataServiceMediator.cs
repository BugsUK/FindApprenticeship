namespace SFA.Apprenticeship.Api.AvService.Mediators.Version51
{
    using System.Collections.Generic;
    using Common;
    using DataContracts.Version51;
    using MessageContracts.Version51;

    public class ReferenceDataServiceMediator : IReferenceDataServiceMediator
    {
        public List<ApprenticeshipFrameworkAndOccupationData> GetApprenticeshipFrameworks()
        {
            throw new System.NotImplementedException();
        }

        public GetErrorCodesResponse GetErrorCodes(GetErrorCodesRequest request)
        {
            return new GetErrorCodesResponse
            {
                MessageId = request.MessageId,
                ErrorCodes = ApiErrors.ErrorCodes
            };
        }

        public GetCountiesResponse GetCounties(GetCountiesRequest request)
        {
            throw new System.NotImplementedException();
        }
    }
}
