namespace SFA.Apprenticeship.Api.AvService.ServiceImplementation.Version51
{
    using System;
    using System.ServiceModel;
    using Apprenticeships.Application.ReferenceData;
    using Apprenticeships.Domain.Interfaces.Mapping;
    using AvService;
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

        private const string LegacyEndpointConfigurationName = "AvRds";

        public ReferenceDataService()
        {
            _mapper = new AvReferenceDataServiceMapper();
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

            AvRds.IReferenceData legacyClient =
                new AvRds.ReferenceDataClient(LegacyEndpointConfigurationName);

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

        private AvRds.GetApprenticeshipFrameworksRequest MapFromRequest(GetApprenticeshipFrameworksRequest request)
        {
            return _mapper.Map<GetApprenticeshipFrameworksRequest, AvRds.GetApprenticeshipFrameworksRequest>(request);
        }

        private GetApprenticeshipFrameworksResponse MapFromLegacyResponse(AvRds.GetApprenticeshipFrameworksResponse legacyResponse)
        {
            return _mapper.Map<AvRds.GetApprenticeshipFrameworksResponse, GetApprenticeshipFrameworksResponse>(legacyResponse);
        }

        #endregion
    }
}
