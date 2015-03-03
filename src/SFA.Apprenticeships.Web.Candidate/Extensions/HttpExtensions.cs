namespace SFA.Apprenticeships.Web.Candidate.Extensions
{
    using System.IO;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Web;

    public static class HttpExtensions
    {
        public static string ToDebugString(this HttpRequestBase request)
        {
            var sb = new StringBuilder();
            sb.AppendFormat("Request: {0} {1}", request.RequestType, request.RawUrl);
            sb.AppendLine();
            sb.AppendLine("Headers:");
            foreach (string key in request.Headers)
            {
                var httpRequestHeader = request.Headers[key];
                sb.AppendFormat(" {0}: {1}", key, string.Join(", ", httpRequestHeader));
                sb.AppendLine();
            }
            string content;
            using (var inputStream = request.InputStream)
            {
                using (var streamReader = new StreamReader(inputStream, Encoding.UTF8))
                {
                    content = streamReader.ReadToEnd();
                }
            }
            sb.AppendLine("Content:");
            sb.AppendLine(RemovePasswords(content));
            return sb.ToString();
        }

        private static string RemovePasswords(string content)
        {
            return Regex.Replace(content, "\"password\":\"[^\"]+\"", "\"Password\":\"########\"", RegexOptions.IgnoreCase);
        }
    }
}