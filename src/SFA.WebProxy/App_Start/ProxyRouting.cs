namespace SFA.WebProxy
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Net.Http;
    using System.Threading.Tasks;

    public class BbcRouting : IProxyRouting
    {
        public Routing GetRouting(Uri requestUri, HttpMethod method, string ipAddress, Task<string> task)
        {
            return new Routing
            {
                PrimaryUri = new Uri("http://news.bbc.co.uk"),
                SecondaryUris = Enumerable.Empty<Uri>(),
                RequestCopyStreams = Enumerable.Empty<Stream>(),
                ResponseCopyStreams = Enumerable.Empty<Stream>()
            };
        }
    }

    public class LogicRouting : IProxyRouting
    {
        public Routing GetRouting(Uri requestUri, HttpMethod method, string ipAddress, Task<string> task)
        {
            var routing = new Routing
            {
                SecondaryUris = Enumerable.Empty<Uri>(),
                RequestCopyStreams = Enumerable.Empty<Stream>(),
                ResponseCopyStreams = Enumerable.Empty<Stream>()
            };

            if ((requestUri.PathAndQuery + "/").StartsWith("/news/"))
            {
                routing.PrimaryUri = new Uri("http://bbc.co.uk" + requestUri.PathAndQuery);
            }
            else
            {
                routing.PrimaryUri = new Uri("https://webapp.services.coventry.ac.uk/" + requestUri.PathAndQuery.Substring(1));
            }

            return routing;
        }
    }

    public class NasAvWebServicesRouting : IProxyRouting
    {
        private const string RootUrl = "https://apprenticeshipvacancymatchingservice.lsc.gov.uk";

        public Routing GetRouting(Uri requestUri, HttpMethod method, string ipAddress, Task<string> getContentAsString)
        {
            //TODO: Possibly allow for redirects to redirect / to /navms/Forms/Candidate/VisitorLanding.aspx
            return new Routing
            {
                PrimaryUri = new Uri(RootUrl + requestUri.PathAndQuery),
                SecondaryUris = Enumerable.Empty<Uri>(), //TODO: use new API url
                RequestCopyStreams = Enumerable.Empty<Stream>(),
                ResponseCopyStreams = Enumerable.Empty<Stream>()
            };
        }
    }
}