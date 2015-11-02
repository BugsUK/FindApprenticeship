namespace SFA.Apprenticeship.Api.AvmsCompatability.ServiceImplementation.Version51
{
    using System;
    using System.ServiceModel;
    using Apprenticeships.Application.ReferenceData;
    using Apprenticeships.Domain.Interfaces.Mapping;
    using AvmsCompatability;
    using Mappers.Version51;
    using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.WCF;
    using MessageContracts.Version51;
    using ServiceContracts.Version51;

    [ExceptionShielding("Default Exception Policy")]
    [ServiceBehavior(Namespace = CommonNamespaces.ExternalInterfacesRel51)]
    public class ReferenceDataService : IReferenceData
    {
        private readonly IMapper _mapper;
        private readonly IReferenceDataProvider _referenceDataProvider;

        private const string LegacyEndpointConfigurationName = "LegacyReferenceDataService";

        public ReferenceDataService()
        {
            _mapper = new LegacyReferenceDataServiceMapper();
        }

        public ReferenceDataService(
            IMapper mapper,
            IReferenceDataProvider referenceDataProvider)
        {
            _mapper = mapper;
            _referenceDataProvider = referenceDataProvider;
        }

        public GetErrorCodesResponse GetErrorCodes(GetErrorCodesRequest request)
        {
            throw new NotImplementedException();
        }

        public GetApprenticeshipFrameworksResponse GetApprenticeshipFrameworks(GetApprenticeshipFrameworksRequest request)
        {
            // TODO: authenticate:
            //  - request.ExternalSystemId
            //  - request.PublicKey

            // _referenceDataProvider.GetCategories();

            LegacyReferenceDataService.IReferenceData legacyClient =
                new LegacyReferenceDataService.ReferenceDataClient(LegacyEndpointConfigurationName);

            var legacyRequest = MapFromRequest(request);
            var legacyResponse = legacyClient.GetApprenticeshipFrameworks(legacyRequest);
            var response = MapFromLegacyResponse(legacyResponse);

            return response;
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

        #region Helpers

        private LegacyReferenceDataService.GetApprenticeshipFrameworksRequest MapFromRequest(GetApprenticeshipFrameworksRequest request)
        {
            return _mapper.Map<GetApprenticeshipFrameworksRequest, LegacyReferenceDataService.GetApprenticeshipFrameworksRequest>(request);
        }

        private GetApprenticeshipFrameworksResponse MapFromLegacyResponse(LegacyReferenceDataService.GetApprenticeshipFrameworksResponse legacyResponse)
        {
            return _mapper.Map<LegacyReferenceDataService.GetApprenticeshipFrameworksResponse, GetApprenticeshipFrameworksResponse>(legacyResponse);
        }

        #endregion
    }
}
