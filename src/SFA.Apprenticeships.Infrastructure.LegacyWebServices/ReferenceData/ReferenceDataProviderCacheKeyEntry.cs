namespace SFA.Apprenticeships.Infrastructure.LegacyWebServices.ReferenceData
{
    using SFA.Apprenticeships.Application.Interfaces.Caching;

    public class ReferenceDataProviderCacheKeyEntry : BaseCacheKey
    {
        private const string ReferenceDataProviderCacheKey = "SFA.Apprenticeships.ReferenceData";

        protected override string KeyPrefix
        {
            get { return ReferenceDataProviderCacheKey; }
        }

        public override CacheDuration Duration
        {
            get
            {
                //TODO: Update to use config value for cache duration. consider specifying the duration in the calling code not as a property of the cache key
                return CacheDuration.OneDay;
            }
        }
    }
}
