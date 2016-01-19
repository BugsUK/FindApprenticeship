namespace SFA.Apprenticeships.Infrastructure.Common.Configuration
{
    using SFA.Infrastructure.Interfaces.Caching;
    using SFA.Infrastructure.Interfaces;

    public class CachedConfigurationService : IConfigurationService
    {
        private static readonly ConfigurationCacheKey ConfigurationCacheKey = new ConfigurationCacheKey();
        private readonly IConfigurationService _configurationService;
        private readonly ICacheService _cacheService;

        public CachedConfigurationService(IConfigurationService configurationService, ICacheService cacheService)
        {
            _configurationService = configurationService;
            _cacheService = cacheService;
        }

        public TSettings Get<TSettings>() where TSettings : class
        {
            return _cacheService.Get(ConfigurationCacheKey, GetFromBase<TSettings>, typeof(TSettings).Name);
        }

        private TSettings GetFromBase<TSettings>(string configurationSectionName) where TSettings : class
        {
            return _configurationService.Get<TSettings>();
        }
    }
}
