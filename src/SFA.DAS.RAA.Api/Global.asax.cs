namespace SFA.DAS.RAA.Api
{
    using System.Web.Http;

    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);

            Microsoft.ApplicationInsights.Extensibility.TelemetryConfiguration.Active.InstrumentationKey =
                System.Web.Configuration.WebConfigurationManager.AppSettings["APPINSIGHTS_INSTRUMENTATIONKEY"];
        }
    }
}
