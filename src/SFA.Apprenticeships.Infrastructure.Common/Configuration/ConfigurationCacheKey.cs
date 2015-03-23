namespace SFA.Apprenticeships.Infrastructure.Common.Configuration
{
    using Domain.Interfaces.Caching;

    public class ConfigurationCacheKey : BaseCacheKey
    {
        protected override string KeyPrefix
        {
            get { return "ConfigurationSettings"; }
        }

        public override CacheDuration Duration
        {
            get { return CacheDuration.FiveMinutes; }
        }
    }
}
