namespace SFA.Apprenticeships.Infrastructure.LegacyWebServices.Vacancy
{
    using System;
    using Apprenticeships.Application.Vacancy;
    using Domain.Entities.Vacancies;

    using Apprenticeships.Application.Interfaces;
    using Apprenticeships.Application.Interfaces.Caching;

    public class CachedLegacyVacancyDataProvider<TVacancyDetail> : IVacancyDataProvider<TVacancyDetail> where TVacancyDetail : VacancyDetail
    {
        private readonly ILogService _logger;

        private static readonly BaseCacheKey VacancyDataCacheKey = new VacancyDataServiceCacheKeyEntry();
        private readonly ICacheService _cacheService;
        private readonly IVacancyDataProvider<TVacancyDetail> _vacancyDataProvider;

        public CachedLegacyVacancyDataProvider(ICacheService cacheService, IVacancyDataProvider<TVacancyDetail> vacancyDataProvider, ILogService logger)
        {
            _cacheService = cacheService;
            _vacancyDataProvider = vacancyDataProvider;
            _logger = logger;
        }

        public TVacancyDetail GetVacancyDetails(int vacancyId, bool errorIfNotFound)
        {
            _logger.Debug("Calling GetVacancyDetails for VacancyId: {0}", vacancyId);
            return _cacheService.Get(VacancyDataCacheKey, vacancyId1 => _vacancyDataProvider.GetVacancyDetails(vacancyId1, errorIfNotFound), vacancyId);
        }

        public int GetVacancyId(int vacancyReferenceNumber)
        {
            _logger.Debug("Calling GetVacancyId for vacancyReferenceNumber: {0}", vacancyReferenceNumber);
            return (int)_cacheService.Get(VacancyDataCacheKey, vacancyReferenceNumber1 => (object)_vacancyDataProvider.GetVacancyId(vacancyReferenceNumber1), vacancyReferenceNumber);
        }
    }
}
