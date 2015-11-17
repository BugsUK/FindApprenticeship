using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;

namespace SFA.WebProxy
{
    public class DefaultRouting : IProxyRouting
    {
        public Routing GetRouting(Uri requestUri, HttpMethod method, string ipAddress, Task<string> task)
        {
            return new Routing()
            {
                PrimaryUri = new Uri("http://news.bbc.co.uk"),
                SecondaryUris = Enumerable.Empty<Uri>(),
                RequestCopyStreams = Enumerable.Empty<Stream>(),
                ResponseCopyStreams = Enumerable.Empty<Stream>()
            };
        }
    }

    public class DefaultRouting2 : IProxyRouting
    {
        public Routing GetRouting(Uri requestUri, HttpMethod method, string ipAddress, Task<string> task)
        {
            var routing = new Routing()
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
}