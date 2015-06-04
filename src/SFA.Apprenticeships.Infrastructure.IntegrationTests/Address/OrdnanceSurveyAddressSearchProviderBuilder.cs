namespace SFA.Apprenticeships.Infrastructure.IntegrationTests.Address
{
    using Application.Address;
    using Application.Interfaces.Logging;
    using Common.IoC;
    using Domain.Interfaces.Configuration;
    using Infrastructure.Address;
    using Infrastructure.Communication.IoC;
    using Moq;
    using RestSharp;
    using StructureMap;

    public class OrdnanceSurveyAddressSearchProviderBuilder
    {
        public IAddressSearchProvider Build()
        {
            var container = new Container(x =>
            {
                x.AddRegistry<CommonRegistry>();
                x.AddRegistry<CommunicationRegistry>();
            });

            container.Configure(c => c.For<ILogService>().Use(new Mock<ILogService>().Object));

            var configurationService = container.GetInstance<IConfigurationService>();

            var provider = new OrdnanceSurveyAddressSearchProvider(configurationService, new RestClient());
            return provider;
        }
    }
}