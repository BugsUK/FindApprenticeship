namespace SFA.WebProxy.Configuration
{
    using System;
    using System.Configuration;

    public class ConfigurationManagerConfiguration : IConfiguration
    {
        public bool AreNonPrimaryRequestsEnabled => Convert.ToBoolean(ConfigurationManager.AppSettings["AreNonPrimaryRequestsEnabled"]);
        public bool IsLoggingEnabled => Convert.ToBoolean(ConfigurationManager.AppSettings["IsLoggingEnabled"]);
        public string CompatabilityWebServiceRootUrl => ConfigurationManager.AppSettings["CompatabilityWebServiceRootUrl"];
        public string NasAvWebServiceRootUri => ConfigurationManager.AppSettings["NasAvWebServiceRootUri"];
        public string FileProxyLoggingRootPath => ConfigurationManager.AppSettings["FileProxyLoggingRootPath"];
        public string AzureStorageConnectionString => ConfigurationManager.AppSettings["AzureStorageConnectionString"];
    }
}