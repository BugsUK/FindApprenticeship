namespace SFA.WebProxy.Routing
{
    using System;
    using System.Net.Http;
    using Models;

    public interface IProxyRouting
    {
        Routing GetRouting(Uri requestUri, HttpMethod method, string ipAddress, string requestContent, RouteIdentifier routeIdentifier);
    }
}