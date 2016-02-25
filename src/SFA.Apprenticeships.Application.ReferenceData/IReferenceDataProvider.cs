namespace SFA.Apprenticeships.Application.ReferenceData
{
    using System.Collections.Generic;
    using Domain.Entities.Raa.Vacancies;
    using Domain.Entities.ReferenceData;

    public interface IReferenceDataProvider
    {
        IEnumerable<Category> GetCategories();

        Category GetSubCategoryByName(string subCategoryName);

        Category GetCategoryByName(string categoryName);

        Category GetSubCategoryByCode(string subCategoryCode);

        Category GetCategoryByCode(string categoryCode);

        IEnumerable<Sector> GetSectors();
    }
}
