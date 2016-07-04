namespace SFA.WebProxy
{
    using System.Configuration;
    using System.Net.Http;
    using System.Web.Http;
    using Configuration;
    using Logging;
    using Repositories;
    using Routing;
    using Service;

    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            // Web API routes
            config.MapHttpAttributeRoutes();

            var cacheService = new MemoryCacheService(int.Parse(ConfigurationManager.AppSettings["CacheDurationInSeconds"]));
            var configuration = new CachedConfiguration(new ConfigurationManagerConfiguration(), cacheService);

            var webProxyUserRepository = new CachedWebProxyUserRepository(new SqlServerWebProxyUserRepository(configuration), cacheService);

            //var proxyLogging = new FileProxyLogging(configuration);
            var proxyLogging = new AzureBlobStorageLogging(configuration);
            //var proxyLogging = new NullProxyLogging();

            //var proxyRouting = new LogicRouting();
            var proxyRouting = new NasAvWebServicesRouting(configuration, webProxyUserRepository);


            config.Routes.MapHttpRoute(name: "Proxy", routeTemplate: "{*path}", handler:
            HttpClientFactory.CreatePipeline(
                innerHandler: new HttpClientHandler(), // will never get here if proxy is doing its job
                handlers: new DelegatingHandler[] { new ProxyHandler(proxyRouting, proxyLogging, configuration) }),
                defaults: new { path = RouteParameter.Optional },
                constraints: null
            );
        }
    }
}
