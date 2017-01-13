namespace SFA.DAS.RAA.Api
{
    using System.Web.Http;
    using DependencyResolution;
    using Handlers;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;
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

            //Configure serializer
            GlobalConfiguration.Configuration.Formatters.JsonFormatter.SerializerSettings.Converters.Add(new StringEnumConverter());
            JsonConvert.DefaultSettings = () =>
            {
                var settings = new JsonSerializerSettings();
                settings.Converters.Add(new StringEnumConverter());
                return settings;
            };

            GlobalConfiguration.Configure(WebApiConfig.Register);

            Microsoft.ApplicationInsights.Extensibility.TelemetryConfiguration.Active.InstrumentationKey = System.Web.Configuration.WebConfigurationManager.AppSettings["APPINSIGHTS_INSTRUMENTATIONKEY"];
        }
    }
}
