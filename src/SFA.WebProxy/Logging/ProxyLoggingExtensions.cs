namespace SFA.WebProxy.Logging
{
    using System.Net.Http;
    using System.Text;

    public static class ProxyLoggingExtensions
    {
        public static string GetHeadersLoggingString(this HttpRequestMessage request)
        {
            var sb = new StringBuilder();
            foreach (var httpRequestHeader in request.Headers)
            {
                var separator = httpRequestHeader.Key == "User-Agent" ? " " : ", ";
                sb.AppendFormat("{0}: {1}\r\n", httpRequestHeader.Key, string.Join(separator, httpRequestHeader.Value));
            }
            if (request.Content != null)
            {
                foreach (var httpRequestHeader in request.Content.Headers)
                {
                    sb.AppendFormat("{0}: {1}\r\n", httpRequestHeader.Key, string.Join(", ", httpRequestHeader.Value));
                }
            }
            return sb.ToString();
        }
    }
}