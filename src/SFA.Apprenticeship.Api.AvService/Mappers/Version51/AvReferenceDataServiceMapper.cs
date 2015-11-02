namespace SFA.Apprenticeship.Api.AvService.Mappers.Version51
{
    using Apprenticeships.Infrastructure.Common.Mappers;
    using MessageContracts.Version51;
    using DataContracts.Version51;

    public class AvReferenceDataServiceMapper : MapperEngine
    {
        public override void Initialise()
        {
            // GetApprenticeshipFrameworksRequest
            Mapper.CreateMap
                <GetApprenticeshipFrameworksRequest, AvRds.GetApprenticeshipFrameworksRequest>();

            // GetApprenticeshipFrameworksResponse
            Mapper.CreateMap
                <AvRds.GetApprenticeshipFrameworksResponse, GetApprenticeshipFrameworksResponse>();

            // ApprenticeshipFrameworkAndOccupationData
            Mapper.CreateMap
                <AvRds.ApprenticeshipFrameworkAndOccupationData, ApprenticeshipFrameworkAndOccupationData>();
        }
    }
}
