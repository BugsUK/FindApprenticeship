namespace SFA.Apprenticeships.Infrastructure.FAAUnitTests.LegacyWebServices.Candidate
{
    using System;
    using Infrastructure.LegacyWebServices.GatewayServiceProxy;
    using Infrastructure.LegacyWebServices.Wcf;

    public class MockGatewayService : IWcfService<GatewayServiceContract>
    {
        private readonly GatewayServiceContract _gatewayServiceContract;

        public MockGatewayService(GatewayServiceContract gatewayServiceContract)
        {
            _gatewayServiceContract = gatewayServiceContract;
        }

        public void Use(Action<GatewayServiceContract> action)
        {
            action(_gatewayServiceContract);
        }

        public void Use(string endpointConfigurationName, string endpointAddress, Action<GatewayServiceContract> action)
        {
            action(_gatewayServiceContract);
        }

        public void Use(string endpointConfigurationName, Action<GatewayServiceContract> action)
        {
            action(_gatewayServiceContract);
        }
    }
}