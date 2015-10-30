namespace SFA.Apprenticeship.Api.AvmsCompatability.Mappers.Version51
{
    using Apprenticeships.Infrastructure.Common.Mappers;
    using MessageContracts.Version51;
    using DataContracts.Version51;

    public class LegacyReferenceDataServiceMapper : MapperEngine
    {
        public override void Initialise()
        {
            // GetApprenticeshipFrameworksRequest
            Mapper.CreateMap
                <GetApprenticeshipFrameworksRequest, LegacyReferenceDataService.GetApprenticeshipFrameworksRequest>();

            // GetApprenticeshipFrameworksResponse
            Mapper.CreateMap
                <LegacyReferenceDataService.GetApprenticeshipFrameworksResponse, GetApprenticeshipFrameworksResponse>();

            // ApprenticeshipFrameworkAndOccupationData
            Mapper.CreateMap
                <LegacyReferenceDataService.ApprenticeshipFrameworkAndOccupationData, ApprenticeshipFrameworkAndOccupationData>();
        }
    }
}
