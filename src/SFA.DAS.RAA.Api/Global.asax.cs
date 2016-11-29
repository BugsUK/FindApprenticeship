namespace SFA.DAS.RAA.Api
{
    using System.Web.Http;
    using Handlers;

    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);

            //GlobalConfiguration.Configuration.MessageHandlers.Add(new ApiKeyHandler());

            Microsoft.ApplicationInsights.Extensibility.TelemetryConfiguration.Active.InstrumentationKey =
                System.Web.Configuration.WebConfigurationManager.AppSettings["APPINSIGHTS_INSTRUMENTATIONKEY"];
        }
    }
}
