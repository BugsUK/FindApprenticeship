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

        County GetCounty(int countyId);

        County GetCounty(string county);

        IEnumerable<LocalAuthority> GetLocalAuthorities();

        LocalAuthority GetLocalAuthority(int localAuthorityId);

        LocalAuthority GetLocalAuthority(string localAuthorityCodeName);

        void UpdateStandard(Standard standard);
    }
}
