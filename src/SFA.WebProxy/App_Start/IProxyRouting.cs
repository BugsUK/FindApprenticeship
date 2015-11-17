namespace SFA.WebProxy
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Net.Http;
    using System.Threading.Tasks;

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