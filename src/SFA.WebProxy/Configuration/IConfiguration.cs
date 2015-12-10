namespace SFA.WebProxy.Configuration
{
    using System;
    using System.Text.RegularExpressions;

    public interface IConfiguration
    {
        bool AreNonPrimaryRequestsEnabled { get; }
        bool IsLoggingEnabled { get; }
        Uri NasAvWebServiceRootUri { get; }
        Uri CompatabilityWebServiceRootUri { get; }
        Regex AutomaticRouteToCompatabilityWebServiceRegex { get; }
        string FileProxyLoggingRootPath { get; }
        string AzureStorageConnectionString { get; }
        string SqlServerConnectionString { get; }
    }
}