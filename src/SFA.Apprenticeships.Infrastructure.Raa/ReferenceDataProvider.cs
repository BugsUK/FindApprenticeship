namespace SFA.Apprenticeships.Infrastructure.Raa
{
    using Domain.Raa.Interfaces.Repositories;
    using System.Collections.Generic;
    using System.Linq;
    using Application.ReferenceData;
    using Domain.Entities.Raa.Vacancies;
    using Domain.Entities.ReferenceData;

    public class ReferenceDataProvider  : IReferenceDataProvider
    {
        private IReferenceRepository _referenceRepository;

        private readonly IDictionary<int, string> _standardSectorToSectorSubjectAreaTier1Map = new Dictionary<int, string>
        {
            {1, "SSAT1.AHR"}, //Actuarial
            {2, "SSAT1.MFP"}, //Aerospace
            {3, "SSAT1.MFP"}, //Automotive
            {4, "SSAT1.HBY"}, //Automotive retail
            {5, "SSAT1.HBY"}, //Butchery
            {6, "SSAT1.AHR"}, //Conveyancing and probate
            {7, "SSAT1.MFP"}, //Defence
            {8, "SSAT1.PUB"}, //Dental health
            {9, "SSAT1.ITC"}, //Digital Industries
            {10, "SSAT1.MFP"}, //Electrotechnical
            {11, "SSAT1.MFP"}, //Energy & Utilities
            {12, "SSAT1.AHR"}, //Financial Services
            {13, "SSAT1.MFP"}, //Food and Drink
            {14, "SSAT1.ALB"}, //Horticulture
            {15, "SSAT1.AHR"}, //Insurance
            {16, "SSAT1.AHR"}, //Law
            {17, "SSAT1.AHR"}, //Leadership & Management
            {18, "SSAT1.MFP"}, //Life and Industrial Sciences
            {19, "SSAT1.MFP"}, //Maritime
            {20, "SSAT1.ACC"}, //Newspaper and Broadcast Media
            {21, "SSAT1.MFP"}, //Nuclear
            {22, "SSAT1.CST"}, //Property services
            {23, "SSAT1.PUB"}, //Public Service
            {24, "SSAT1.MFP"}, //Rail Design
            {25, "SSAT1.MFP"}, //Refrigeration Air Conditioning and Heat Pump (RACHP)
            {26, "SSAT1.CST"}, //Surveying
            {27, "SSAT1.PUB"}, //Housing
            {28, "SSAT1.MFP"}, //Non-destructive Testing
            {29, "SSAT1.HBY"} //Energy Management
        };

        public ReferenceDataProvider(IReferenceRepository referenceRepository)
        {
            _referenceRepository = referenceRepository;
        }

        public IEnumerable<Category> GetCategories()
        {
            var categories = GetFrameworks();
            var standardSectors = GetSectors();

            //Combining Frameworks and standards in here with garbage code as this entire service is being replaced
            foreach (var standardSector in standardSectors)
            {
                var sectorSubjectAreaTier1Code = _standardSectorToSectorSubjectAreaTier1Map[standardSector.Id];
                var standardSectorCode = CategoryPrefixes.GetStandardSectorCode(standardSector.Id);
                var sectorSubjectAreaTier1Category = categories.Single(c => c.CodeName == sectorSubjectAreaTier1Code);
                var standards = standardSector.Standards.Select(s => new Category(s.Id, CategoryPrefixes.GetStandardCode(s.Id), s.Name, standardSectorCode, CategoryType.Standard)).ToList();
                var standardSectorCategory = new Category(standardSector.Id, standardSectorCode, standardSector.Name, sectorSubjectAreaTier1Code, CategoryType.StandardSector, standards);
                sectorSubjectAreaTier1Category.SubCategories.Add(standardSectorCategory);
            }

            //Order the new standard sectors correctly
            foreach (var category in categories)
            {
                var orderedSubCategories = category.SubCategories.OrderBy(c => c.FullName).ToList();
                category.SubCategories.Clear();
                foreach (var subCategory in orderedSubCategories)
                {
                    category.SubCategories.Add(subCategory);
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
            return GetCategories().SelectMany(c => c.SubCategories).FirstOrDefault(sc => sc.CodeName == subCategoryCode);
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
