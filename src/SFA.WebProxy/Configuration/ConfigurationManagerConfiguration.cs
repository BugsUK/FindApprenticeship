namespace SFA.WebProxy.Configuration
{
    using System;
    using System.Configuration;
    using System.Text.RegularExpressions;

    public class ConfigurationManagerConfiguration : IConfiguration
    {
        public bool AreNonPrimaryRequestsEnabled => Convert.ToBoolean(ConfigurationManager.AppSettings["AreNonPrimaryRequestsEnabled"]);
        public bool IsLoggingEnabled => Convert.ToBoolean(ConfigurationManager.AppSettings["IsLoggingEnabled"]);
        public Uri CompatabilityWebServiceRootUri => new Uri(ConfigurationManager.AppSettings["CompatabilityWebServiceRootUrl"]);
        public Uri NasAvWebServiceRootUri => new Uri(ConfigurationManager.AppSettings["NasAvWebServiceRootUri"]);
        public string FileProxyLoggingRootPath => ConfigurationManager.AppSettings["FileProxyLoggingRootPath"];
        public string AzureStorageConnectionString => ConfigurationManager.AppSettings["AzureStorageConnectionString"];
        public Regex AutomaticRouteToCompatabilityWebServiceRegex => new Regex(ConfigurationManager.AppSettings["AutomaticRouteToCompatabilityWebServiceRegex"], RegexOptions.IgnoreCase);
        public Regex ConfigurableRouteToCompatabilityWebServiceRegex => new Regex(ConfigurationManager.AppSettings["ConfigurableRouteToCompatabilityWebServiceRegex"], RegexOptions.IgnoreCase);
    }
}