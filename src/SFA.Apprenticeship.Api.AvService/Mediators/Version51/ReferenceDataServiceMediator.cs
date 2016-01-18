namespace SFA.Apprenticeship.Api.AvService.Mediators.Version51
{
    using System;
    using System.Collections.Generic;
    using Common;
    using DataContracts.Version51;
    using MessageContracts.Version51;
    using Providers;

    public class ReferenceDataServiceMediator : ServiceMediatorBase, IReferenceDataServiceMediator
    {
        public ReferenceDataServiceMediator(IWebServiceAuthenticationProvider webServiceAuthenticationProvider)
            : base(webServiceAuthenticationProvider)
        {
        }

        public List<ApprenticeshipFrameworkAndOccupationData> GetApprenticeshipFrameworks()
        {
            throw new NotImplementedException();
        }

        public GetErrorCodesResponse GetErrorCodes(GetErrorCodesRequest request)
        {
            AuthenticateRequest(request);

            return new GetErrorCodesResponse
            {
                MessageId = request.MessageId,
                ErrorCodes = ApiErrors.ErrorCodes
            };
        }
    }
}
