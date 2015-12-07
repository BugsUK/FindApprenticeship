namespace SFA.WebProxy.Configuration
{
    using System;
    using System.Text.RegularExpressions;

    public interface IConfiguration
    {
        bool AreNonPrimaryRequestsEnabled { get; }
        bool IsLoggingEnabled { get; }
        Uri CompatabilityWebServiceRootUri { get; }
        Uri NasAvWebServiceRootUri { get; }
        string FileProxyLoggingRootPath { get; }
        string AzureStorageConnectionString { get; }
        Regex AutomaticRouteToCompatabilityWebServiceRegex { get; }
        Regex ConfigurableRouteToCompatabilityWebServiceRegex { get; }
    }
}