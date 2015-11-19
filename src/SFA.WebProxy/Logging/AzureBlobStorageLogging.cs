namespace SFA.WebProxy.Logging
{
    using System;
    using System.IO;
    using System.Net.Http;
    using Configuration;
    using Microsoft.WindowsAzure.Storage;
    using Microsoft.WindowsAzure.Storage.Blob;
    using Models;

    public class AzureBlobStorageLogging : IProxyLogging
    {
        private readonly IConfiguration _configuration;
        private readonly CloudStorageAccount _storageAccount;

        public AzureBlobStorageLogging(IConfiguration configuration)
        {
            _configuration = configuration;
            _storageAccount = CloudStorageAccount.Parse(_configuration.AzureStorageConnectionString);
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

        public void LogResponseContent(HttpResponseMessage httpResponseMessage, RouteIdentifier routeIdentifier)
        {
            if (_configuration.IsLoggingEnabled)
            {
                try
                {
                    var httpContent = httpResponseMessage.Content.ReadAsStreamAsync().Result;
                    using (var logStream = GetCloudBlockBlob(routeIdentifier, "response_").OpenWrite())
                    {
                        httpContent.CopyTo(logStream);
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
            var blobClient = _storageAccount.CreateCloudBlobClient();

            var container = blobClient.GetContainerReference("webproxy");
            container.CreateIfNotExists();

            var dateTime = routeIdentifier.DateTime;
            var datePath = dateTime.ToString("yyyy-MM-dd");
            var timePath = dateTime.ToString("hh-mm-ss");

            var blobPath = $"{datePath}\\{additionalIdentifier}{routeIdentifier.Name}\\{timePath}_{routeIdentifier.Id}.log";

            var blockBlob = container.GetBlockBlobReference(blobPath);

            blockBlob.Properties.ContentType = "application/json";

            return blockBlob;
        }
    }
}