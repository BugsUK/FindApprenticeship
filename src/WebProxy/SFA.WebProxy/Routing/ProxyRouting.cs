namespace SFA.WebProxy.Routing
{
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Text.RegularExpressions;
    using Configuration;
    using Models;
    using Repositories;

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
            var routing = new Routing();
            routing.Routes = new List<Route>();

            if ((requestUri.PathAndQuery + "/").StartsWith("/news/"))
            {
                routing.Routes.Add(new Route("http://bbc.co.uk" + requestUri.PathAndQuery, new RouteIdentifier("bbc"), true));
            }
            else
            {
                routing.Routes.Add(new Route("https://webapp.services.coventry.ac.uk/" + requestUri.PathAndQuery.Substring(1), new RouteIdentifier("coventryuniversitywebappservices"), true));
                routing.RewriteFrom = "https://webapp.services.coventry.ac.uk/";
                routing.RewriteTo   = "://" + requestUri.Host;
            }

            return routing;
        }
    }

    public class NasAvWebServicesRouting : IProxyRouting
    {
        private static readonly Regex ExternalSystemIdRegex = new Regex("<ns:ExternalSystemId>(.+?)</ns:ExternalSystemId>");

        private readonly IConfiguration _configuration;
        private readonly IWebProxyUserRepository _webProxyUserRepository;

        public NasAvWebServicesRouting(IConfiguration configuration, IWebProxyUserRepository webProxyUserRepository)
        {
            _configuration = configuration;
            _webProxyUserRepository = webProxyUserRepository;
        }

        public Routing GetRouting(Uri requestUri, HttpMethod method, string ipAddress, string requestContent, RouteIdentifier routeIdentifier)
        {
            var isCompatabilityWebServicePrimary = IsCompatabilityWebServicePrimary(requestUri, requestContent);

            return new Routing
            {
                Routes = new List<Route>
                {
                    new Route(new Uri(_configuration.NasAvWebServiceRootUri.OriginalString + requestUri.PathAndQuery), new RouteIdentifier(routeIdentifier, "nasavwebservice"), !isCompatabilityWebServicePrimary),
                    new Route(GetCompatabilityWebServiceUrl(requestUri), new RouteIdentifier(routeIdentifier, "compatabilitywebservice"), isCompatabilityWebServicePrimary)
                },
                RewriteFrom = Regex.Replace(_configuration.NasAvWebServiceRootUri.OriginalString, "https?", ""),
                RewriteTo   = Uri.SchemeDelimiter + requestUri.Authority
            };
        }

        public bool IsAutomaticRouteToCompatabilityWebServiceUri(Uri requestUri)
        {
            if (string.IsNullOrEmpty(_configuration.AutomaticRouteToCompatabilityWebServiceRegex?.ToString()))
            {
                return false;
            }
            return _configuration.AutomaticRouteToCompatabilityWebServiceRegex.IsMatch(requestUri.AbsoluteUri);
        }

        private bool IsCompatabilityWebServicePrimary(Uri requestUri, string requestContent)
        {
            var compatabilityWebServiceIsPrimary = IsAutomaticRouteToCompatabilityWebServiceUri(requestUri);

            if (!compatabilityWebServiceIsPrimary && !string.IsNullOrEmpty(requestContent))
            {
                var externalSystemIdMatch = ExternalSystemIdRegex.Match(requestContent);
                if (externalSystemIdMatch.Success)
                {
                    Guid externalSystemId;
                    if (Guid.TryParse(externalSystemIdMatch.Groups[1].Value, out externalSystemId))
                    {
                        var webProxyUser = _webProxyUserRepository.Get(externalSystemId);
                        if (webProxyUser != WebProxyConsumer.WebProxyConsumerNotFound && !string.IsNullOrEmpty(webProxyUser?.RouteToCompatabilityWebServiceRegex))
                        {
                            compatabilityWebServiceIsPrimary =
                                new Regex(webProxyUser.RouteToCompatabilityWebServiceRegex, RegexOptions.IgnoreCase)
                                    .IsMatch(requestUri.AbsoluteUri);
                        }
                    }
                }
            }
            return compatabilityWebServiceIsPrimary;
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
    }
}