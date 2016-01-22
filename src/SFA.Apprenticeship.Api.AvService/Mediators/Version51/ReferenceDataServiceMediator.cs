namespace SFA.Apprenticeship.Api.AvService.Mediators.Version51
{
    using System;
    using System.Collections.Generic;
    using Apprenticeships.Domain.Entities.WebServices;
    using Common;
    using DataContracts.Version51;
    using MessageContracts.Version51;
    using Providers;
    using Providers.Version51;

    public class ReferenceDataServiceMediator : ServiceMediatorBase, IReferenceDataServiceMediator
    {
        private readonly IReferenceDataProvider _referenceDataProvider;

        public ReferenceDataServiceMediator(
            IWebServiceAuthenticationProvider webServiceAuthenticationProvider,
            IReferenceDataProvider referenceDataProvider)
            : base(webServiceAuthenticationProvider, WebServiceCategory.Reference)
        {
            _referenceDataProvider = referenceDataProvider;
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

        public GetCountiesResponse GetCounties(GetCountiesRequest request)
        {
            AuthenticateRequest(request);

            var counties = _referenceDataProvider.GetCounties();

            return new GetCountiesResponse
            {
                MessageId = request.MessageId,
                Counties = counties
            };
        }
    }
}
