namespace SFA.DAS.RAA.Api.AcceptanceTests
{
    using System.Web.Http;
    using Owin;

    public class TestStartup : Startup
    {
        public override void Configuration(IAppBuilder app)
        {
            // do your web api, IoC, etc setup here
            var config = new HttpConfiguration();
            //config.DependencyResolver = 
            config.MapHttpAttributeRoutes();
            // ...etc
            app.UseWebApi(config);
        }
    }
}