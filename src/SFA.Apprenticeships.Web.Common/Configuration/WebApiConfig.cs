namespace SFA.Apprenticeships.Web.Common.Configuration
{
    using System;
    using System.Web.Http;

    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "VersionApi",
                routeTemplate: "api/version/{id}",
                defaults: new { Controller = "Version", id = RouteParameter.Optional }
                );
        }
    }
}