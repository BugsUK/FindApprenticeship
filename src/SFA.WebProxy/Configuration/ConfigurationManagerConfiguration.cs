namespace SFA.WebProxy.Configuration
{
    using System;
    using System.Configuration;
    using System.Text.RegularExpressions;

    public class ConfigurationManagerConfiguration : IConfiguration
    {
        public bool AreNonPrimaryRequestsEnabled => Convert.ToBoolean(ConfigurationManager.AppSettings["AreNonPrimaryRequestsEnabled"]);

        public bool IsLoggingEnabled => Convert.ToBoolean(ConfigurationManager.AppSettings["IsLoggingEnabled"]);

        public Uri NasAvWebServiceRootUri => new Uri(ConfigurationManager.AppSettings["NasAvWebServiceRootUri"]);

        public Uri CompatabilityWebServiceRootUri => new Uri(ConfigurationManager.AppSettings["CompatabilityWebServiceRootUri"]);

        public Regex AutomaticRouteToCompatabilityWebServiceRegex
        {
            get
            {
                var appSetting = ConfigurationManager.AppSettings["AutomaticRouteToCompatabilityWebServiceRegex"];
                return string.IsNullOrEmpty(appSetting) ? null : new Regex(appSetting, RegexOptions.IgnoreCase);
            }
        }

        public string FileProxyLoggingRootPath => ConfigurationManager.AppSettings["FileProxyLoggingRootPath"];

        public string AzureStorageConnectionString => ConfigurationManager.AppSettings["AzureStorageConnectionString"];

        public string SqlServerConnectionString => ConfigurationManager.AppSettings["SqlServerConnectionString"];
    }
}