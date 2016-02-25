namespace SFA.Apprenticeships.Application.ReferenceData
{
    using System.Collections.Generic;
    using Domain.Entities.Raa.Vacancies;
    using Domain.Entities.ReferenceData;
    using Interfaces.ReferenceData;

    public class ReferenceDataService : IReferenceDataService
    {
        private readonly IReferenceDataProvider _referenceDataProvider;

        public ReferenceDataService(IReferenceDataProvider referenceDataProvider)
        {
            _referenceDataProvider = referenceDataProvider;
        }

        public IEnumerable<Category> GetCategories()
        {
            return _referenceDataProvider.GetCategories();
        }

        public Category GetSubCategoryByName(string subCategoryName)
        {
            return _referenceDataProvider.GetSubCategoryByName(subCategoryName);
        }

        public Category GetCategoryByName(string categoryName)
        {
            return _referenceDataProvider.GetCategoryByName(categoryName);
        }

        public Category GetSubCategoryByCode(string subCategoryCode)
        {
            return _referenceDataProvider.GetSubCategoryByCode(subCategoryCode);
        }

        public Category GetCategoryByCode(string categoryCode)
        {
            return _referenceDataProvider.GetCategoryByCode(categoryCode);
        }

        public IEnumerable<Sector> GetSectors()
        {
            return _referenceDataProvider.GetSectors();
        }
    }
}
