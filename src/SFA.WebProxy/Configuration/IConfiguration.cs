namespace SFA.WebProxy.Configuration
{
    using System;
    using System.Text.RegularExpressions;

    public interface IConfiguration
    {
        /// <summary>
        /// Whether to forward requests to the secondary. If set then the requests will be made to the secondary and the reponses potentially logged (see IsLoggingEnabled),
        /// but it is only the response from the primary that gets forwarded on to the requestor.
        /// </summary>
        bool AreNonPrimaryRequestsEnabled { get; }

        /// <summary>If set then all requests and responses are logged. See AreNonPrimaryRequestsEnabled, AzureStorageConnectionString and possibly FileProxyLoggingRootPath</summary>
        bool IsLoggingEnabled { get; }

        /// <summary>The default primary. Requests will be forwarded here and the responses will by default be forwarded on to the requestor.
        /// AutomaticRouteToCompatabilityWebServiceRegex and data in the SqlServerConnectionString database can override this.</summary>
        Uri NasAvWebServiceRootUri { get; }

        /// <summary>The default secondary. Requests will still be forwarded here (if AreNonPrimaryRequestsEnabled is set) but responses will by default only be recorded for comparison purposes.
        /// AutomaticRouteToCompatabilityWebServiceRegex and data in the SqlServerConnectionString database can override this.</summary>
        Uri CompatabilityWebServiceRootUri { get; }

        /// <summary>If the request Uri matches then the primary and secondary are swapped. That is, CompatabilityWebServiceRootUri becomes the primary.</summary>
        Regex AutomaticRouteToCompatabilityWebServiceRegex { get; }

        /// <summary>Only used when using the FileProxyLogging logger.</summary>
        string FileProxyLoggingRootPath { get; }

        /// <summary>Where to log when using the AzureBlobStorageLogging logger.</summary>
        string AzureStorageConnectionString { get; }

        /// <summary>Used to access the WebProxy.WebProxyConsumer table which specifies which (if any) requests will have the primary and secondary swapped, that is,
        /// CompatabilityWebServiceRootUri becomes the primary</summary>
        string SqlServerConnectionString { get; }
    }
}