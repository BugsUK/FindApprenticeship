using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;

namespace SFA.WebProxy
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(name: "Proxy", routeTemplate: "{*path}", handler:
            HttpClientFactory.CreatePipeline(
                innerHandler: new HttpClientHandler(), // will never get here if proxy is doing its job
                handlers: new DelegatingHandler[] { new ProxyHandler(new DefaultRouting2()) }),
                defaults: new { path = RouteParameter.Optional },
                constraints: null
            );
        }
    }
}
