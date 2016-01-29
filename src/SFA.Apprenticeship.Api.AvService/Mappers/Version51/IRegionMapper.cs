namespace SFA.Apprenticeship.Api.AvService.Mappers.Version51
{
    using System.Collections.Generic;
    using Apprenticeships.Domain.Entities.Reference;
    using DataContracts.Version51;

    public interface IRegionMapper
    {
        RegionData MapToRegionData(Region region);

        List<RegionData> MapToRegionDatas(IEnumerable<Region> regions);
    }
}