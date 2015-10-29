namespace SFA.Apprenticeship.Api.AvmsCompatability.ServiceImplementation.Version51
{
    using System.Linq;
    using System.ServiceModel;
    using Apprenticeships.Application.ReferenceData;
    using AvmsCompatability;
    using DataContracts.Version51;
    using MessageContracts.Version51;
    using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.WCF;
    using ServiceContracts.Version51;

    [ExceptionShielding("Default Exception Policy")]
    [ServiceBehavior(Namespace = CommonNamespaces.ExternalInterfacesRel51)]
    public class ReferenceDataService : IReferenceData
    {
        private readonly IReferenceDataProvider _referenceDataProvider;

        public ReferenceDataService(IReferenceDataProvider referenceDataProvider)
        {
            _referenceDataProvider = referenceDataProvider;
        }

        public GetErrorCodesResponse GetErrorCodes(GetErrorCodesRequest request)
        {
            throw new System.NotImplementedException();
        }

        public GetApprenticeshipFrameworksResponse GetApprenticeshipFrameworks(GetApprenticeshipFrameworksRequest request)
        {
            // TODO: authenticate:
            //  - request.ExternalSystemId
            //  - request.PublicKey

            _referenceDataProvider.GetCategories();

            return new GetApprenticeshipFrameworksResponse
            {
                MessageId = request.MessageId,
                ApprenticeshipFrameworks = Enumerable.Empty<ApprenticeshipFrameworkAndOccupationData>().ToList()
            };
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
