namespace SFA.WebProxy.Logging
{
    using System;
    using System.IO;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using Configuration;
    using Microsoft.WindowsAzure.Storage;
    using Microsoft.WindowsAzure.Storage.Blob;
    using Models;

    public class AzureBlobStorageLogging : IProxyLogging
    {
        private readonly IConfiguration _configuration;
        private readonly CloudBlobContainer _container;

        public AzureBlobStorageLogging(IConfiguration configuration)
        {
            _configuration = configuration;

            var storageAccount = CloudStorageAccount.Parse(_configuration.AzureStorageConnectionString);
            var blobClient = storageAccount.CreateCloudBlobClient();
            _container = blobClient.GetContainerReference("webproxy");
            _container.CreateIfNotExists();
        }

        public void LogRequest(HttpRequestMessage request, string requestContent, RouteIdentifier routeIdentifier)
        {
            if (_configuration.IsLoggingEnabled)
            {
                try
                {
                    using (var logStream = new StreamWriter(GetCloudBlockBlob(routeIdentifier, "request").OpenWrite()))
                    {
                        logStream.WriteLine(request.RequestUri);
                        var headers = request.GetHeadersLoggingString();
                        logStream.WriteLine(headers);
                        logStream.WriteLine(requestContent);
                    }
                }
                catch (Exception)
                {
                    //Swallow exeption to ensure logging errors don't cause the request to fail
                }
            }
        }

        public void LogResponseContent(Stream content, RouteIdentifier routeIdentifier)
        {
            if (_configuration.IsLoggingEnabled)
            {
                try
                {
                    using (var logStream = GetCloudBlockBlob(routeIdentifier, "response_").OpenWrite())
                    {
                        content.CopyTo(logStream);
                    }
                }
                catch (Exception)
                {
                    //Swallow exeption to ensure logging errors don't cause the request to fail
                }
            }
        }

        public void LogResponseCancelled(Route route, HttpRequestHeaders httpRequestHeaders, HttpContentHeaders contentHeaders, AggregateException aggregateException)
        {
            LogResponseError("Request to " + route.Uri + " was cancelled", httpRequestHeaders, contentHeaders, aggregateException, route.Identifier);
        }

        public void LogResponseFaulted(Route route, HttpRequestHeaders httpRequestHeaders, HttpContentHeaders contentHeaders, AggregateException aggregateException)
        {
            LogResponseError("Request to " + route.Uri + " faulted", httpRequestHeaders, contentHeaders, aggregateException, route.Identifier);
        }

        public void LogResponseError(string message, HttpRequestHeaders httpRequestHeaders, HttpContentHeaders contentHeaders, AggregateException aggregateException, RouteIdentifier routeIdentifier)
        {
            if (_configuration.IsLoggingEnabled)
            {
                try
                {
                    using (var logStream = new StreamWriter(GetCloudBlockBlob(routeIdentifier, "request").OpenWrite()))
                    {
                        logStream.WriteLine(message);
                        var headers = httpRequestHeaders.GetHeadersLoggingString(contentHeaders);
                        logStream.WriteLine(headers);
                        var exceptionString = aggregateException.GetLoggingString();
                        logStream.WriteLine(exceptionString);
                    }
                }
                catch (Exception)
                {
                    //Swallow exeption to ensure logging errors don't cause the request to fail
                }
            }
        }

        private CloudBlockBlob GetCloudBlockBlob(RouteIdentifier routeIdentifier, string additionalIdentifier)
        {
            var dateTime = routeIdentifier.DateTime;
            var datePath = dateTime.ToString("yyyy-MM-dd");
            var timePath = dateTime.ToString("hh-mm-ss");

            var blobPath = $"{datePath}\\{additionalIdentifier}{routeIdentifier.Name}\\{timePath}_{routeIdentifier.Id}.log";

            var blockBlob = _container.GetBlockBlobReference(blobPath);

            blockBlob.Properties.ContentType = "application/json";

            return blockBlob;
        }
    }
}