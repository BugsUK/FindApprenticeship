namespace SFA.Apprenticeship.Api.AvService.Mappers.Version51
{
    using System.Collections.Generic;
    using System.Linq;
    using Apprenticeships.Domain.Entities.Reference;
    using DataContracts.Version51;

    public class LocalAuthorityMapper : ILocalAuthorityMapper
    {
        public LocalAuthorityData MapToLocalAuthorityData(LocalAuthority localAuthority)
        {
            return new LocalAuthorityData
            {
                ShortName = localAuthority.ShortName,
                FullName = localAuthority.FullName,
                County = localAuthority.County.FullName
            };
        }

        public List<LocalAuthorityData> MapToLocalAuthorityDatas(IEnumerable<LocalAuthority> localAuthorities)
        {
            return localAuthorities.Select(MapToLocalAuthorityData).ToList();
        }
    }
}