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

            var mockLogService = new Mock<ILogService>();

            container.Configure(c => c.For<ILogService>().Use(mockLogService.Object));

            var configurationService = container.GetInstance<IConfigurationService>();

            return new OrdnanceSurveyAddressSearchProvider(mockLogService.Object, configurationService, new RestClient());
        }
    }
}