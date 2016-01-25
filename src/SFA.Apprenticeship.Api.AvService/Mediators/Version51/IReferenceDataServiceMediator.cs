namespace SFA.Apprenticeship.Api.AvService.Mediators.Version51
{
    using System.Collections.Generic;
    using DataContracts.Version51;
    using MessageContracts.Version51;

    public interface IReferenceDataServiceMediator
    {
        // TODO: AG: US872: mediator should return response object, not list directly.
        List<ApprenticeshipFrameworkAndOccupationData> GetApprenticeshipFrameworks();

        GetErrorCodesResponse GetErrorCodes(GetErrorCodesRequest request);

        GetCountiesResponse GetCounties(GetCountiesRequest request);

        GetRegionResponse GetRegions(GetRegionRequest request);

        GetLocalAuthoritiesResponse GetLocalAuthorities(GetLocalAuthoritiesRequest request);
    }
}
