namespace SFA.Apprenticeships.Infrastructure.Raa
{
    using Domain.Raa.Interfaces.Repositories;
    using System.Collections.Generic;
    using System.Linq;
    using Application.ReferenceData;
    using Domain.Entities.Raa.Vacancies;
    using Domain.Entities.ReferenceData;
    using Presentation;

    public class ReferenceDataProvider  : IReferenceDataProvider
    {
        private readonly IReferenceRepository _referenceRepository;

        public ReferenceDataProvider(IReferenceRepository referenceRepository)
        {
            _referenceRepository = referenceRepository;
        }

        public IEnumerable<Category> GetCategories()
        {
            var categories = GetFrameworks().ToList();
            var standardSectors = GetSectors();

            foreach (var standardSector in standardSectors)
            {
                var sectorSubjectAreaTier1Id = standardSector.ApprenticeshipOccupationId;
                var standardSectorCode = CategoryPrefixes.GetStandardSectorCode(standardSector.Id);
                var sectorSubjectAreaTier1Category = categories.Single(c => c.Id == sectorSubjectAreaTier1Id);
                var standards = standardSector.Standards.Select(s => new Category(s.Id, CategoryPrefixes.GetStandardCode(s.Id), s.Name, standardSectorCode, CategoryType.Standard)).ToList();
                var standardSectorCategory = new Category(standardSector.Id, standardSectorCode, standardSector.Name, CategoryPrefixes.GetSectorSubjectAreaTier1Code(sectorSubjectAreaTier1Category.CodeName), CategoryType.StandardSector, standards);
                sectorSubjectAreaTier1Category.SubCategories.Add(standardSectorCategory);
            }

            //Order the new standard sectors correctly
            foreach (var category in categories)
            {
                var orderedSubCategories = category.SubCategories.OrderBy(c => c.FullName).ThenBy(c => c.CodeName).ToList();
                category.SubCategories.Clear();
                foreach (var subCategory in orderedSubCategories)
                {
                    //Remove duplicated categories
                    subCategory.FullName = FullNameFormatter.Format(subCategory.FullName);
                    var duplicateCategory = category.SubCategories.SingleOrDefault(c => c.FullName == subCategory.FullName);
                    if (duplicateCategory == null)
                    {
                        category.SubCategories.Add(subCategory);
                    }
                    else
                    {
                        //Create combined category and replace duplicates
                        var combinedCodeName = duplicateCategory.CodeName + "|" + subCategory.CodeName;
                        var combinedSubcategories = duplicateCategory.SubCategories ?? new List<Category>();
                        foreach (var standard in subCategory.SubCategories)
                        {
                            combinedSubcategories.Add(standard);
                        }
                        foreach (var standard in combinedSubcategories)
                        {
                            standard.ParentCategoryCodeName = combinedCodeName;
                        }
                        var combinedCategory = new Category(0, combinedCodeName, duplicateCategory.FullName, duplicateCategory.ParentCategoryCodeName, CategoryType.Combined, combinedSubcategories);
                        var currentIndex = category.SubCategories.IndexOf(duplicateCategory);
                        category.SubCategories.Insert(currentIndex, combinedCategory);
                        category.SubCategories.Remove(duplicateCategory);
                    }
                }
            }

            return categories;
        }

        /// <summary>
        /// This searches SubCategories (Frameworks and StandardSectors) by full name
        /// </summary>
        /// <param name="subCategoryName"></param>
        /// <returns></returns>
        public Category GetSubCategoryByName(string subCategoryName)
        {
            return GetCategories().SelectMany(c => c.SubCategories).FirstOrDefault(sc => sc.FullName == subCategoryName);
        }

        /// <summary>
        /// This searches Categories (SectorSubjectAreaTier1) by full name
        /// </summary>
        /// <param name="categoryName"></param>
        /// <returns></returns>
        public Category GetCategoryByName(string categoryName)
        {
            return GetCategories().FirstOrDefault(c => c.FullName == categoryName);
        }

        /// <summary>
        /// This searches SubCategories (Frameworks and StandardSectors) by codename
        /// </summary>
        /// <param name="subCategoryCode"></param>
        /// <returns></returns>
        public Category GetSubCategoryByCode(string subCategoryCode)
        {
            return GetCategories().SelectMany(c => c.SubCategories).FirstOrDefault(sc => sc.CodeName.Split('|').Contains(subCategoryCode));
        }

        /// <summary>
        /// This searches Categories (SectorSubjectAreaTier1) by code
        /// </summary>
        /// <param name="categoryCode"></param>
        /// <returns></returns>
        public Category GetCategoryByCode(string categoryCode)
        {
            return GetCategories().FirstOrDefault(c => c.CodeName == categoryCode);
        }

        public IEnumerable<Category> GetFrameworks()
        {
            var occupations = _referenceRepository.GetOccupations().ToList();

            occupations.ForEach(o =>
            {
                o.Frameworks.ToList().ForEach(f => f.ParentCategoryCodeName = o.CodeName);
            });

            return occupations.Select(o => o.ToCategory()).ToList();
        }

        public IEnumerable<Sector> GetSectors()
        {
            var sectors = _referenceRepository.GetSectors();
            var standards = _referenceRepository.GetStandards();

            foreach (var sector in sectors)
            {
                sector.Standards = standards.Where(s => s.ApprenticeshipSectorId == sector.Id).ToList();
            }

            return sectors;
        }
    }
}
