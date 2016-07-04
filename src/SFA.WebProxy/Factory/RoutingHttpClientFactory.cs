namespace SFA.WebProxy.Factory
{
    using System;
    using System.Net.Http;

    public class RoutingHttpClientFactory
    {
        private const string HostHeader = "Host";

        public static HttpClient Create(HttpRequestMessage request, Uri uri)
        {
            //We don't want to handle redirects ourselves. The responses should be sent back unchanged and handled by the client so their URLs are correct.
            var httpClient = new HttpClient(new HttpClientHandler {AllowAutoRedirect = false});

            // Copy request headers (.NET treats request content headers separately)
            foreach (var header in request.Headers)
            {
                if (header.Key != HostHeader)
                    httpClient.DefaultRequestHeaders.Add(header.Key, header.Value);
            }
            httpClient.DefaultRequestHeaders.Host = uri.Host;

            return httpClient;
        } 
    }
}