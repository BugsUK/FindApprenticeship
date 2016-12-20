namespace SFA.DAS.RAA.Api.AcceptanceTests
{
    using System.Web.Http;
    using Api.DependencyResolution;
    using Handlers;
    using Owin;
    using Services;
    using IoC = DependencyResolution.IoC;

    public class TestStartup : Startup
    {
        public override void Configuration(IAppBuilder app)
        {
            //Duplicated from API Global.asax.cs
            var config = new HttpConfiguration();

            //Configure IoC
            var container = IoC.Initialize();
            config.DependencyResolver = new StructureMapWebApiDependencyResolver(container);

            //Register API Key handler
            config.MessageHandlers.Add(new ApiKeyHandler(container.GetInstance<IAuthenticationService>()));

            WebApiConfig.Register(config);

            app.UseWebApi(config);
        }
    }
}