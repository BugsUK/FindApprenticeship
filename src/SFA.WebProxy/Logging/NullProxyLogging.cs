namespace SFA.WebProxy.Logging
{
    using System;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using Models;
    using System.IO;
    public class NullProxyLogging : IProxyLogging
    {
        public void LogRequest(HttpRequestMessage request, string requestContent, RouteIdentifier routeIdentifier)
        {
            
        }

        public void LogResponseContent(Stream content, RouteIdentifier routeIdentifier)
        {
            
        }

        public void LogResponseCancelled(Route route, HttpRequestHeaders httpRequestHeaders, HttpContentHeaders contentHeaders, AggregateException aggregateException)
        {
            
        }

        public void LogResponseFaulted(Route route, HttpRequestHeaders httpRequestHeaders, HttpContentHeaders contentHeaders, AggregateException aggregateException)
        {
            
        }
    }
}