namespace SFA.Apprenticeship.Api.AvService.Mappers.Version51
{
    using System.Collections.Generic;
    using System.Linq;
    using Apprenticeships.Domain.Entities.Reference;
    using DataContracts.Version51;

    public class CountyMapper : ICountyMapper
    {
        public CountyData MapToCountyData(County county)
        {
            return new CountyData
            {
                CodeName = county.CodeName,
                FullName = county.FullName
            };
        }

        public List<CountyData> MapToCountyDatas(IEnumerable<County> counties)
        {
            return counties.Select(MapToCountyData).ToList();
        }
    }
}