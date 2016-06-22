namespace SFA.WebProxy.Logging
{
    using System;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using Models;
    using System.IO;

    public interface IProxyLogging
    {
        void LogRequest(HttpRequestMessage request, string requestContent, RouteIdentifier routeIdentifier);

        void LogResponseContent(Stream content, RouteIdentifier routeIdentifier);

        void LogResponseCancelled(Route route, HttpRequestHeaders httpRequestHeaders, HttpContentHeaders contentHeaders, AggregateException aggregateException);

        void LogResponseFaulted(Route route, HttpRequestHeaders httpRequestHeaders, HttpContentHeaders contentHeaders, AggregateException aggregateException);
    }
}