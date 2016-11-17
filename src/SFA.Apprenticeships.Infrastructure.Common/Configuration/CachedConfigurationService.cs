namespace SFA.Apprenticeships.Infrastructure.Common.Configuration
{
    using System;
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
            return _cacheService.Get(ConfigurationCacheKey, _configurationService.Get<TSettings>);
        }

        public object Get(Type settingsType)
        {
            return _cacheService.Get(ConfigurationCacheKey, () => _configurationService.Get(settingsType), settingsType);
        }
    }
}
