namespace SFA.Apprenticeships.Infrastructure.LegacyWebServices.ReferenceData
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using SFA.Infrastructure.Interfaces;
    using Apprenticeships.Application.ReferenceData;
    using Configuration;
    using Domain.Entities.Exceptions;
    using Domain.Entities.Raa.Vacancies;
    using Domain.Entities.ReferenceData;
    using LegacyReferenceDataProxy;
    using Wcf;

    public class ReferenceDataProvider : IReferenceDataProvider
    {
        private readonly ILogService _logger;
        private readonly IWcfService<IReferenceData> _service;
        private readonly LegacyServicesConfiguration _legacyServicesConfiguration;

        public ReferenceDataProvider(IWcfService<IReferenceData> service, IConfigurationService configurationService, ILogService logger)
        {
            _service = service;
            _legacyServicesConfiguration = configurationService.Get<LegacyServicesConfiguration>();
            _logger = logger;
        }

        public IEnumerable<Category> GetCategories()
        {
            GetApprenticeshipFrameworksResponse response = null;

            var request = new GetApprenticeshipFrameworksRequest(_legacyServicesConfiguration.SystemId, Guid.NewGuid(), _legacyServicesConfiguration.PublicKey);

            try
            {
                _logger.Debug("Calling ReferenceData.GetApprenticeshipFrameworks");

                _service.Use("ReferenceData", client => response = client.GetApprenticeshipFrameworks(request));
                var categories = GetCategories(response);
                
                _logger.Debug("ReferenceData.GetApprenticeshipFrameworks succeeded");

                return categories;
            }
            catch (BoundaryException ex)
            {
                _logger.Warn(ex);
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
            }

            // Must return null or could be put in cache.
            return null;
        }

        public Category GetSubCategoryByName(string subCategoryName)
        {
            return GetCategories().SelectMany(c => c.SubCategories).FirstOrDefault(sc => sc.FullName == subCategoryName);
        }

        public Category GetCategoryByName(string categoryName)
        {
            return GetCategories().FirstOrDefault(c => c.FullName == categoryName);
        }

        public Category GetSubCategoryByCode(string subCategoryCode)
        {
            return GetCategories().SelectMany(c => c.SubCategories).FirstOrDefault(sc => sc.CodeName == subCategoryCode);
        }

        public Category GetCategoryByCode(string categoryCode)
        {
            return GetCategories().FirstOrDefault(c => c.CodeName == categoryCode);
        }

        public IEnumerable<Category> GetFrameworks()
        {
            return GetCategories();
        }

        public IEnumerable<Sector> GetSectors()
        {
            throw new NotImplementedException("Sectors are not exposed via the reference data service");
        }

        private IEnumerable<Category> GetCategories(GetApprenticeshipFrameworksResponse response)
        {
            if (response?.ApprenticeshipFrameworks == null || response.ApprenticeshipFrameworks.Length == 0)
            {
                _logger.Warn("No ApprenticeshipFrameworks data returned from the legacy GetApprenticeshipFrameworks service");
                return null;
            }

            var categories = new List<Category>();

            var topLevelCategories =
                response.ApprenticeshipFrameworks
                    .Select(
                        c =>
                            new Category(0, CategoryPrefixes.GetSectorSubjectAreaTier1Code(c.ApprenticeshipOccupationCodeName), c.ApprenticeshipOccupationFullName,
                                CategoryType.SectorSubjectAreaTier1))
                    .Distinct(new CategoryComparer())
                    .OrderBy(c => c.FullName);

            foreach (var topLevelCategory in topLevelCategories)
            {
                var frameworks =
                    response.ApprenticeshipFrameworks.Where(
                        c => c.ApprenticeshipOccupationCodeName == topLevelCategory.CodeName)
                        .Select(
                            d =>
                                new Category(0, CategoryPrefixes.GetFrameworkCode(d.ApprenticeshipFrameworkCodeName), d.ApprenticeshipFrameworkFullName,
                                    topLevelCategory.CodeName, CategoryType.Framework))
                        .OrderBy(c => c.FullName);

                foreach (var framework in frameworks)
                {
                    topLevelCategory.SubCategories.Add(framework);
                }

                categories.Add(topLevelCategory);
            }

            return categories;
        }

        class CategoryComparer : IEqualityComparer<Category>
        {
            public bool Equals(Category x, Category y)
            {
                return x.CodeName == y.CodeName;
            }

            public int GetHashCode(Category obj)
            {
                return obj.CodeName.GetHashCode();
            }
        }
    }
}
