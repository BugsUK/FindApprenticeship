namespace SFA.WebProxy.Logging
{
    using System;
    using System.Net.Http;
    using System.Net.Http.Headers;
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

        public static string GetHeadersLoggingString(this HttpRequestHeaders headers, HttpContentHeaders contentHeaders)
        {
            var sb = new StringBuilder();
            foreach (var httpRequestHeader in headers)
            {
                var separator = httpRequestHeader.Key == "User-Agent" ? " " : ", ";
                sb.AppendFormat("{0}: {1}\r\n", httpRequestHeader.Key, string.Join(separator, httpRequestHeader.Value));
            }
            if (contentHeaders != null)
            {
                foreach (var httpRequestHeader in contentHeaders)
                {
                    sb.AppendFormat("{0}: {1}\r\n", httpRequestHeader.Key, string.Join(", ", httpRequestHeader.Value));
                }
            }
            return sb.ToString();
        }

        public static string GetLoggingString(this AggregateException exception)
        {
            var sb = new StringBuilder();
            sb.AppendLine(exception.Message);
            if (!string.IsNullOrEmpty(exception.StackTrace))
            {
                sb.AppendLine(exception.StackTrace);
            }
            foreach (var innerException in exception.InnerExceptions)
            {
                AppendException(sb, innerException);
            }
            return sb.ToString();
        }

        private static void AppendException(StringBuilder sb, Exception exception)
        {
            sb.AppendLine(exception.Message);
            if (!string.IsNullOrEmpty(exception.StackTrace))
            {
                sb.AppendLine(exception.StackTrace);
            }
            if (exception.InnerException != null)
            {
                AppendException(sb, exception.InnerException);
            }
        }
    }
}