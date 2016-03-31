namespace SFA.Apprenticeships.Infrastructure.LegacyWebServices.ReferenceData
{
    using System.Collections.Generic;
    using System.Linq;
    using SFA.Infrastructure.Interfaces;
    using Apprenticeships.Application.ReferenceData;
    using Domain.Entities.Raa.Vacancies;
    using Domain.Entities.ReferenceData;
    using SFA.Infrastructure.Interfaces.Caching;

    public class CachedReferenceDataProvider : IReferenceDataProvider
    {
        private readonly ILogService _logger;

        private static readonly BaseCacheKey CacheKey = new ReferenceDataProviderCacheKeyEntry();
        private readonly IReferenceDataProvider _legcayService;
        private readonly ICacheService _cacheService;


        public CachedReferenceDataProvider(IReferenceDataProvider legcayService, ICacheService cacheService, ILogService logger)
        {
            _legcayService = legcayService;
            _cacheService = cacheService;
            _logger = logger;
        }

        public IEnumerable<Category> GetCategories()
        {
            _logger.Debug("Calling cached GetCategories");
            return _cacheService.Get(CacheKey, _legcayService.GetCategories);
        }


        public Category GetSubCategoryByName(string subCategoryName)
        {
            _logger.Debug("Calling cached GetSubCategoryByName");
            return _cacheService.Get(CacheKey, GetSubCategoryByNameWrap, subCategoryName);
        }

        public Category GetCategoryByName(string categoryName)
        {
            _logger.Debug("Calling cached GetCategoryByName");
            return _cacheService.Get(CacheKey, GetCategoryByNameWrap, categoryName);
        }

        public Category GetSubCategoryByCode(string subCategoryCode)
        {
            _logger.Debug("Calling cached GetSubCategoryByCode");
            return _cacheService.Get(CacheKey, GetSubCategoryByCodeWrap, subCategoryCode);
        }

        public Category GetCategoryByCode(string categoryCode)
        {
            _logger.Debug("Calling cached GetCategoryByCode");
            return _cacheService.Get(CacheKey, GetCategoryByCodeWrap, categoryCode);
        }

        public IEnumerable<Category> GetFrameworks()
        {
            _logger.Debug("Calling cached GetFrameworks");
            return _cacheService.Get(CacheKey, _legcayService.GetFrameworks);
        }

        public IEnumerable<Sector> GetSectors()
        {
            _logger.Debug("Calling cached GetSectors");
            return _cacheService.Get(CacheKey, _legcayService.GetSectors);
        }

        /// <summary>
        /// Wrapper to prevent caching all category items (or having to get all categories from service for each item)
        /// </summary>
        /// <param name="subCategoryName"></param>
        /// <returns></returns>
        private Category GetSubCategoryByNameWrap( string subCategoryName)
        {
            return GetCategories().SelectMany(c => c.SubCategories).FirstOrDefault(sc => sc.FullName == subCategoryName);
        }

        /// <summary>
        /// Wrapper to prevent caching all category items (or having to get all categories from service for each item)
        /// </summary>
        /// <param name="categoryName"></param>
        /// <returns></returns>
        private Category GetCategoryByNameWrap(string categoryName)
        {
            return GetCategories().FirstOrDefault(c => c.FullName == categoryName);
        }

        /// <summary>
        /// Wrapper to prevent caching all category items (or having to get all categories from service for each item)
        /// </summary>
        /// <param name="subCategoryCode"></param>
        /// <returns></returns>
        private Category GetSubCategoryByCodeWrap(string subCategoryCode)
        {
            return GetCategories().SelectMany(c => c.SubCategories).FirstOrDefault(sc => sc.CodeName == subCategoryCode);
        }

        /// <summary>
        /// Wrapper to prevent caching all category items (or having to get all categories from service for each item)
        /// </summary>
        /// <param name="categoryCode"></param>
        /// <returns></returns>
        private Category GetCategoryByCodeWrap(string categoryCode)
        {
            return GetCategories().FirstOrDefault(c => c.CodeName == categoryCode);
        }
    }
}
