﻿namespace SFA.Apprenticeships.Web.Common.Framework
{
    using System;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Routing;
    using NLog;

    public static class HttpServerUtilityExtensions
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public static void HandleError<T>(this HttpServerUtility server, HttpContext httpContext) where T : Controller
        {
            var currentController = " ";
            var currentAction = " ";
            var currentRouteData = RouteTable.Routes.GetRouteData(new HttpContextWrapper(httpContext));

            if (currentRouteData != null)
            {
                if (currentRouteData.Values["controller"] != null && !String.IsNullOrEmpty(currentRouteData.Values["controller"].ToString()))
                    currentController = currentRouteData.Values["controller"].ToString();

                if (currentRouteData.Values["action"] != null && !String.IsNullOrEmpty(currentRouteData.Values["action"].ToString()))
                    currentAction = currentRouteData.Values["action"].ToString();
            }

            var exception = server.GetLastError();
            var httpException = exception as HttpException;

            if (httpException == null)
            {
                // TODO: Somehow get an instance of the ILogService and use that here.
                // Using NLog directly doesn't work when logging to a file.

                var verb = httpContext.Request.HttpMethod;
                
                var exceptionMessage = string.Format("Error while executing action {0}/{1} of controller {2} ({3}): {4}",
                    currentAction, verb, currentController, httpContext.Request.Url, exception.Message);

                Logger.Error(exception, exceptionMessage);
            }

            var controller = DependencyResolver.Current.GetService<T>();
            var routeData = new RouteData();
            var action = "InternalServerError";

            if (httpException != null)
            {
                switch (httpException.GetHttpCode())
                {
                    case 404:
                        action = "NotFound";
                        break;

                    case 401:
                        action = "AccessDenied";
                        break;
                }
            }

            httpContext.ClearError();
            httpContext.Response.Clear();
            httpContext.Response.StatusCode = httpException != null ? httpException.GetHttpCode() : 500;
            httpContext.Response.TrySkipIisCustomErrors = true;

            routeData.Values["controller"] = "Error";
            routeData.Values["action"] = action;

            controller.ViewData.Model = new HandleErrorInfo(exception, currentController, currentAction);
            ((IController)controller).Execute(new RequestContext(new HttpContextWrapper(httpContext), routeData));
        }
    }
}
