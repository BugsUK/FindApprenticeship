namespace SFA.Apprenticeships.Infrastructure.Common.Configuration
{
    using Domain.Interfaces.Caching;
    using Domain.Interfaces.Configuration;

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

        public TSettings Get<TSettings>(string settingName) where TSettings : class
        {
            return _cacheService.Get(ConfigurationCacheKey, _configurationService.Get<TSettings>, settingName);
        }

        public T GetCloudAppSetting<T>(string key)
        {
            //Not caching as system is self managed
            return _configurationService.GetCloudAppSetting<T>(key);
        }
    }
}
