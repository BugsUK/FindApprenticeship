﻿using System.Web.Http;

namespace SFA.Apprenticeships.Web.Employer
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {         
            //config.Routes.MapHttpRoute(
            //    name: "DefaultApi",
            //    routeTemplate: "api/{controller}/{id}",
            //    defaults: new { id = RouteParameter.Optional });

            config.MapHttpAttributeRoutes();
        }
    }
}
