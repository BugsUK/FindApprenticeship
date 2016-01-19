namespace SFA.Apprenticeships.Application.Vacancy.SiteMap
{
    using System.Collections.Generic;
    using System.Linq;
    using SFA.Infrastructure.Interfaces.Caching;

    public class SiteMapVacancyProvider : ISiteMapVacancyProvider
    {
        private const string CacheKey = "SiteMap.Vacancies";
        private readonly ICacheService _cacheService;

        public SiteMapVacancyProvider(ICacheService cacheService)
        {
            _cacheService = cacheService;
        }

        public IEnumerable<SiteMapVacancy> GetVacancies()
        {
            return _cacheService.Get<SiteMapVacancy[]>(CacheKey);
        }

        public void SetVacancies(IEnumerable<SiteMapVacancy> siteMapVacancies)
        {
            _cacheService.PutObject(CacheKey, siteMapVacancies.ToArray(), CacheDuration.OneDay);
        }
    }
}