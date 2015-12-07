namespace SFA.WebProxy.Routing
{
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using Configuration;
    using Models;

    public class BbcRouting : IProxyRouting
    {
        public Routing GetRouting(Uri requestUri, HttpMethod method, string ipAddress, string requestContent, RouteIdentifier routeIdentifier)
        {
            return new Routing
            {
                Routes = new List<Route> {new Route("http://news.bbc.co.uk", new RouteIdentifier("bbcnews"), true)}
            };
        }
    }

    public class LogicRouting : IProxyRouting
    {
        public Routing GetRouting(Uri requestUri, HttpMethod method, string ipAddress, string requestContent, RouteIdentifier routeIdentifier)
        {
            var routes = new List<Route>();

            if ((requestUri.PathAndQuery + "/").StartsWith("/news/"))
            {
                routes.Add(new Route("http://bbc.co.uk" + requestUri.PathAndQuery, new RouteIdentifier("bbc"), true));
            }
            else
            {
                routes.Add(new Route("https://webapp.services.coventry.ac.uk/" + requestUri.PathAndQuery.Substring(1), new RouteIdentifier("coventryuniversitywebappservices"), true));
            }

            var routing = new Routing
            {
                Routes = routes,
            };

            return routing;
        }
    }

    public class NasAvWebServicesRouting : IProxyRouting
    {
        private readonly IConfiguration _configuration;

        public NasAvWebServicesRouting(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public Routing GetRouting(Uri requestUri, HttpMethod method, string ipAddress, string requestContent, RouteIdentifier routeIdentifier)
        {
            //TODO: Specify primary uri based on config
            return new Routing
            {
                Routes = new List<Route>
                {
                    new Route(new Uri(_configuration.NasAvWebServiceRootUri, requestUri.PathAndQuery), new RouteIdentifier(routeIdentifier, "nasavwebservice"), true),
                    new Route(GetCompatabilityWebServiceUrl(requestUri), new RouteIdentifier(routeIdentifier, "compatabilitywebservice"), false)
                },
            };
        }

        public Uri GetCompatabilityWebServiceUrl(Uri requestUri)
        {
            var pathAndQuery = requestUri.PathAndQuery;
            if (pathAndQuery.EndsWith(".svc"))
            {
                pathAndQuery = pathAndQuery.Substring(pathAndQuery.LastIndexOf("/", StringComparison.Ordinal));
            }
            return new Uri(_configuration.CompatabilityWebServiceRootUri, pathAndQuery);
        }

        public bool IsAutomaticRouteToCompatabilityWebServiceUri(Uri requestUri)
        {
            return _configuration.AutomaticRouteToCompatabilityWebServiceRegex.IsMatch(requestUri.AbsoluteUri) && !IsConfigurableRouteToCompatabilityWebServiceUri(requestUri);
        }

        public bool IsConfigurableRouteToCompatabilityWebServiceUri(Uri requestUri)
        {
            return _configuration.ConfigurableRouteToCompatabilityWebServiceRegex.IsMatch(requestUri.AbsoluteUri);
        }
    }
}