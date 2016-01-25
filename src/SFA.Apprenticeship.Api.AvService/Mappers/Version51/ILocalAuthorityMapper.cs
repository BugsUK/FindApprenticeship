namespace SFA.Apprenticeship.Api.AvService.Mappers.Version51
{
    using System.Collections.Generic;
    using Apprenticeships.Domain.Entities.Reference;
    using DataContracts.Version51;

    public interface ILocalAuthorityMapper
    {
        LocalAuthorityData MapToLocalAuthorityData(LocalAuthority localAuthority);

        List<LocalAuthorityData> MapToLocalAuthorityDatas(IEnumerable<LocalAuthority> localAuthorities);
    }
}