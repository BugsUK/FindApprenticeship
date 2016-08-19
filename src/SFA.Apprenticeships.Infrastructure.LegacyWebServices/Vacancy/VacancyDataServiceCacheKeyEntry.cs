namespace SFA.Apprenticeships.Infrastructure.LegacyWebServices.Vacancy
{
    using SFA.Apprenticeships.Application.Interfaces.Caching;

    public class VacancyDataServiceCacheKeyEntry : BaseCacheKey
    {
        private const string VacancyDataServiceCacheKey = "SFA.Apprenticeships.VacancyData";

        protected override string KeyPrefix
        {
            get { return VacancyDataServiceCacheKey; }
        }

        public override CacheDuration Duration
        {
            get { return CacheDuration.CacheDefault; }
        }
    }
}
