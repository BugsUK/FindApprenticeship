namespace SFA.Apprenticeship.Api.AvService.Mappers.Version51
{
    using System.Collections.Generic;
    using Apprenticeships.Domain.Entities.Reference;
    using DataContracts.Version51;

    public interface ICountyMapper
    {
        CountyData MapToCountyData(County county);

        List<CountyData> MapToCountyDatas(IEnumerable<County> counties);
    }
}