namespace SFA.DAS.RAA.Api.AcceptanceTests
{
    using System;
    using System.Web.Http;
    using Api.DependencyResolution;
    using Apprenticeships.Domain.Entities.Raa.RaaApi;
    using Apprenticeships.Domain.Raa.Interfaces.Repositories;
    using Constants;
    using Handlers;
    using Moq;
    using Owin;
    using Services;
    using UnitTests.Factories;
    using IoC = DependencyResolution.IoC;

    public class TestStartup : Startup
    {
        public override void Configuration(IAppBuilder app)
        {
            var config = new HttpConfiguration();
            
            // do your web api, IoC, etc setup here
            var container = IoC.Initialize();

            config.DependencyResolver = new StructureMapWebApiDependencyResolver(container);

            var raaApiUserRepository = new Mock<IRaaApiUserRepository>();
            raaApiUserRepository.Setup(r => r.GetUser(It.IsAny<Guid>())).Returns(RaaApiUser.UnknownApiUser);
            raaApiUserRepository.Setup(r => r.GetUser(ApiKeys.ProviderApiKey)).Returns(RaaApiUserFactory.GetValidProviderApiUser(ApiKeys.ProviderApiKey));
            raaApiUserRepository.Setup(r => r.GetUser(ApiKeys.EmployerApiKey)).Returns(RaaApiUserFactory.GetValidEmployerApiUser(ApiKeys.EmployerApiKey));

            config.MessageHandlers.Add(new ApiKeyHandler(new ApiKeyAuthenticationService(raaApiUserRepository.Object)));

            config.MapHttpAttributeRoutes();

            app.UseWebApi(config);
        }
    }
}