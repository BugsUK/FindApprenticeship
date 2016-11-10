namespace SFA.Apprenticeships.Infrastructure.Common.Configuration
{
    using Application.Interfaces;
    using Application.Interfaces.Caching;

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
            var settingName = typeof(TSettings).Name;
            return Get<TSettings>(settingName);
        }

        public TSettings Get<TSettings>(string settingName) where TSettings : class
        {
            return _cacheService.Get(ConfigurationCacheKey, GetFromBase<TSettings>, settingName);
        }

        private TSettings GetFromBase<TSettings>(string configurationSectionName) where TSettings : class
        {
            return _configurationService.Get<TSettings>(configurationSectionName);
        }
    }
}
