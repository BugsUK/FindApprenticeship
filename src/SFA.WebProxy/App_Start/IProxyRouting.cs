using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;

namespace SFA.WebProxy
{
    public interface IProxyRouting
    {
        Routing GetRouting(Uri requestUri, HttpMethod method, string ipAddress, Task<string> getContentAsString);
    }

    public class Routing
    {
        public Uri PrimaryUri;
        public IEnumerable<Uri> SecondaryUris;
        public IEnumerable<Stream> RequestCopyStreams;
        public IEnumerable<Stream> ResponseCopyStreams;
    }
}