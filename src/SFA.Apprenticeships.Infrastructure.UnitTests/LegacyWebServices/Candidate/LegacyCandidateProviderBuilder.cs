namespace SFA.Apprenticeships.Infrastructure.UnitTests.LegacyWebServices.Candidate
{
    using System;
    using Application.Candidate;
    using SFA.Infrastructure.Interfaces;
    using Infrastructure.LegacyWebServices.Candidate;
    using Infrastructure.LegacyWebServices.GatewayServiceProxy;
    using Infrastructure.LegacyWebServices.Wcf;
    using Moq;

    using SFA.Apprenticeships.Application.Interfaces;

    public class LegacyCandidateProviderBuilder
    {
        private IWcfService<GatewayServiceContract> _service = new Mock<IWcfService<GatewayServiceContract>>().Object;
        private readonly Mock<ILogService> _logService = new Mock<ILogService>();

        public ILegacyCandidateProvider Build()
        {
            var provider = new LegacyCandidateProvider(_service, _logService.Object);
            return provider;
        }

        public LegacyCandidateProviderBuilder With(IWcfService<GatewayServiceContract> service)
        {
            _service = service;
            return this;
        }
    }
}