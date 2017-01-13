namespace SFA.DAS.RAA.Api.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;

    public static class HttpRequestMessageExtensions
    {
        public static Dictionary<string, string> GetQueryStrings(this HttpRequestMessage request)
        {
            var queryStrings = request.GetQueryNameValuePairs();
            if (queryStrings == null)
            {
                return new Dictionary<string, string>();
            }
            return request.GetQueryNameValuePairs().ToDictionary(kv => kv.Key, kv => kv.Value, StringComparer.OrdinalIgnoreCase);
        }

        public static bool TryGetQueryStringValue(this HttpRequestMessage request, string queryStringKey, out string queryStringValue)
        {
            var queryStrings = request.GetQueryStrings();
            return queryStrings.TryGetValue(queryStringKey, out queryStringValue);
        }
    }
}