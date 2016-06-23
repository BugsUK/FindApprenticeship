namespace SFA.WebProxy.Configuration
{
    using System;
    using System.Text.RegularExpressions;
    using Service;

    public class CachedConfiguration : IConfiguration
    {
        private readonly IConfiguration _configuration;
        private readonly ICacheService _cacheService;

        public CachedConfiguration(IConfiguration configuration, ICacheService cacheService)
        {
            _configuration = configuration;
            _cacheService = cacheService;
        }

        public bool AreNonPrimaryRequestsEnabled
        {
            get { return _cacheService.Get("AreNonPrimaryRequestsEnabled", () => _configuration.AreNonPrimaryRequestsEnabled); }
        }

        public bool IsLoggingEnabled
        {
            get { return _cacheService.Get("IsLoggingEnabled", () => _configuration.IsLoggingEnabled); }
        }

        public Uri NasAvWebServiceRootUri
        {
            get { return _cacheService.Get("NasAvWebServiceRootUri", () => _configuration.NasAvWebServiceRootUri); }
        }

        public Uri CompatabilityWebServiceRootUri
        {
            get { return _cacheService.Get("CompatabilityWebServiceRootUri", () => _configuration.CompatabilityWebServiceRootUri); }
        }

        public Regex AutomaticRouteToCompatabilityWebServiceRegex
        {
            get { return _cacheService.Get("AutomaticRouteToCompatabilityWebServiceRegex", () => _configuration.AutomaticRouteToCompatabilityWebServiceRegex); }
        }

        public string FileProxyLoggingRootPath
        {
            get { return _cacheService.Get("FileProxyLoggingRootPath", () => _configuration.FileProxyLoggingRootPath); }
        }

        public string AzureStorageConnectionString
        {
            get { return _cacheService.Get("AzureStorageConnectionString", () => _configuration.AzureStorageConnectionString); }
        }

        public string SqlServerConnectionString
        {
            get { return _cacheService.Get("SqlServerConnectionString", () => _configuration.SqlServerConnectionString); }
        }
    }
}