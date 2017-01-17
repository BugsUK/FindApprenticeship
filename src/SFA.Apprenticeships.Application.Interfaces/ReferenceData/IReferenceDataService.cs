namespace SFA.Apprenticeships.Application.Interfaces.ReferenceData
{
    using System.Collections.Generic;
    using Domain.Entities.Raa.Reference;
    using Domain.Entities.Raa.Vacancies;
    using Domain.Entities.ReferenceData;

    public interface IReferenceDataService
    { 
        IEnumerable<Category> GetCategories();

        Category GetSubCategoryByName(string subCategoryName);

        Category GetCategoryByName(string categoryName);

        Category GetSubCategoryByCode(string subCategoryCode);

        Category GetCategoryByCode(string categoryCode);

        IEnumerable<Category> GetFrameworks();

        IEnumerable<Sector> GetSectors();

        IList<ReleaseNote> GetReleaseNotes(DasApplication dasApplication);

        IEnumerable<County> GetCounties();

        County GetCountyById(int countyId);

        County GetCountyByCode(string countyCode);

        County GetCountyByName(string countyName);

        IEnumerable<LocalAuthority> GetLocalAuthorities();

        LocalAuthority GetLocalAuthorityById(int localAuthorityId);

        LocalAuthority GetLocalAuthorityByCode(string localAuthorityCode);

        IEnumerable<Region> GetRegions();

        Region GetRegionById(int regionId);

        Region GetRegionByCode(string regionCode);
    }
}
