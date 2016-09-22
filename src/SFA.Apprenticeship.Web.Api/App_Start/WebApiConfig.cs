using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace SFA.Apprenticeship.Web.Api
{
    using Newtonsoft.Json.Serialization;

    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            config.Formatters.JsonFormatter.SerializerSettings.ContractResolver =
                new DefaultContractResolver { IgnoreSerializableAttribute = true };
        }
    }
}
