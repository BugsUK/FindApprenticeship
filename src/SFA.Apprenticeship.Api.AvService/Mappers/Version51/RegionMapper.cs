namespace SFA.Apprenticeship.Api.AvService.Mappers.Version51
{
    using System.Collections.Generic;
    using System.Linq;
    using Apprenticeships.Domain.Entities.Reference;
    using DataContracts.Version51;

    public class RegionMapper : IRegionMapper
    {
        public RegionData MapToRegionData(Region region)
        {
            return new RegionData
            {
                CodeName = region.CodeName,
                FullName = region.FullName
            };
        }

        public List<RegionData> MapToRegionDatas(IEnumerable<Region> regions)
        {
            return regions.Select(MapToRegionData).ToList();
        }
    }
}