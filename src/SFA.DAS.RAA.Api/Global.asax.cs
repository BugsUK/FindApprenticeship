namespace SFA.DAS.RAA.Api
{
    using System.Web.Http;
    using DependencyResolution;
    using Handlers;
    using Services;

    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            //Configure IoC
            var container = IoC.Initialize();
            GlobalConfiguration.Configuration.DependencyResolver = new StructureMapWebApiDependencyResolver(container);

            //Register API Key handler
            GlobalConfiguration.Configuration.MessageHandlers.Add(new ApiKeyHandler(container.GetInstance<IAuthenticationService>()));

            GlobalConfiguration.Configure(WebApiConfig.Register);

            Microsoft.ApplicationInsights.Extensibility.TelemetryConfiguration.Active.InstrumentationKey = System.Web.Configuration.WebConfigurationManager.AppSettings["APPINSIGHTS_INSTRUMENTATIONKEY"];
        }
    }
}
