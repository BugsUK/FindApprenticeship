namespace SFA.Apprenticeships.Web.ContactForms
{
    using System.Web.Mvc;
    using System.Web.Routing;
    using Constants;

    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.IgnoreRoute("{resource}.ico/{*pathInfo}");
            routes.IgnoreRoute("{folder}/{*pathInfo}", new { folder = "Content" });
            routes.IgnoreRoute("{folder}/{*pathInfo}", new { folder = "Scripts" });

            routes.MapRoute(
                name: EmployerRouteNames.SubmitEmployerEnquiry,
                url: "employer-enquiry",
                defaults: new { controller = "EmployerEnquiry", action = "SubmitEmployerEnquiry" }
                );

            routes.MapRoute(
                name: EmployerRouteNames.SubmitEmployerEnquiryUnbranded,
                url: "employer-enquiry-unbranded",
                defaults: new { controller = "EmployerEnquiry", action = "SubmitEmployerEnquiryUnbranded" }
                );

            routes.MapRoute(
               name: EmployerRouteNames.GlaSubmitEmployerEnquiry,
               url: "gla-employer-enquiry",
               defaults: new { controller = "EmployerEnquiry", action = "GlaSubmitEmployerEnquiry" }
               );

            routes.MapRoute(
               name: EmployerRouteNames.AccessRequest,
               url: "access-request",
               defaults: new { controller = "AccessRequest", action = "SubmitAccessRequest" }
               );

            routes.MapRoute(
                name: EmployerRouteNames.SubmitEmployerEnquiryThankYou,
                url: "employer-enquiry-end",
                defaults: new { controller = "EmployerEnquiry", action = "ThankYou" }
                );

            routes.MapRoute(
                name: EmployerRouteNames.SubmitEmployerEnquiryUnbrandedThankYou,
                url: "employer-enquiry-unbranded-end",
                defaults: new { controller = "EmployerEnquiry", action = "ThankYouUnbranded" }
                );

            routes.MapRoute(
               name: EmployerRouteNames.GlaSubmitEmployerEnquiryThankYou,
               url: "gla-employer-enquiry-end",
               defaults: new { controller = "EmployerEnquiry", action = "GlaThankYou" }
               );

            routes.MapRoute(
               name: EmployerRouteNames.AccessRequestThankYou,
               url: "access-request-end",
               defaults: new { controller = "AccessRequest", action = "ThankYou" }
               );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "EmployerEnquiry", action = "SubmitEmployerEnquiry", id = UrlParameter.Optional }
                );
            routes.LowercaseUrls = true;
        }
    }
}
