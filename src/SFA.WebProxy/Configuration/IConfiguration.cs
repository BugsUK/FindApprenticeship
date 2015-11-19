namespace SFA.WebProxy.Configuration
{
    public interface IConfiguration
    {
        bool AreNonPrimaryRequestsEnabled { get; }
        bool IsLoggingEnabled { get; }
        string CompatabilityWebServiceRootUrl { get; }
        string FileProxyLoggingRootPath { get; }
        string AzureStorageConnectionString { get; }
    }
}