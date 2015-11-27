namespace SFA.WebProxy
{
    using System.Net.Http;
    using System.Web.Http;
    using Configuration;
    using Logging;
    using Routing;

    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            // Web API routes
            config.MapHttpAttributeRoutes();

            var configuration = new ConfigurationManagerConfiguration();

            //var proxyLogging = new FileProxyLogging(configuration);
            var proxyLogging = new AzureBlobStorageLogging(configuration);

            config.Routes.MapHttpRoute(name: "Proxy", routeTemplate: "{*path}", handler:
            HttpClientFactory.CreatePipeline(
                innerHandler: new HttpClientHandler(), // will never get here if proxy is doing its job
                handlers: new DelegatingHandler[] { new ProxyHandler(new NasAvWebServicesRouting(configuration), proxyLogging, configuration) }),
                defaults: new { path = RouteParameter.Optional },
                constraints: null
            );
        }
    }
}
