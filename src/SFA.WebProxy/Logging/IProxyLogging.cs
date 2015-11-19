namespace SFA.WebProxy.Logging
{
    using System.Net.Http;
    using Models;

    public interface IProxyLogging
    {
        void LogRequest(HttpRequestMessage request, string requestContent, RouteIdentifier routeIdentifier);

        void LogResponseContent(HttpResponseMessage httpResponseMessage, RouteIdentifier routeIdentifier);
    }
}