namespace SFA.Apprenticeships.Application.ReferenceData
{
    using System.Collections.Generic;
    using Domain.Entities.Raa.Reference;
    using Domain.Entities.Raa.Vacancies;
    using Domain.Entities.ReferenceData;

    public interface IReferenceDataProvider
    {
        IEnumerable<Category> GetCategories();
        IEnumerable<Category> GetCategories(params CategoryStatus[] statuses);

        Category GetSubCategoryByName(string subCategoryName);

        Category GetCategoryByName(string categoryName);

        Category GetSubCategoryByCode(string subCategoryCode);

        Category GetCategoryByCode(string categoryCode);

        IEnumerable<Category> GetFrameworks();

        IEnumerable<Sector> GetSectors();

        IEnumerable<StandardSubjectAreaTierOne> GetStandardSubjectAreaTierOnes();

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
		
		void UpdateStandard(Standard standard);
    }
}
