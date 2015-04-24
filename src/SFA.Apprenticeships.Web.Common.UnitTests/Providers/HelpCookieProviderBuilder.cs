namespace SFA.Apprenticeships.Web.Common.UnitTests.Providers
{
    using Application.Interfaces.Candidates;
    using Common.Providers;
    using Moq;

    public class HelpCookieProviderBuilder
    {
        private Mock<ICandidateService> _candidateService = new Mock<ICandidateService>();

        public IHelpCookieProvider Build()
        {
            var provider = new HelpCookieProvider(_candidateService.Object);

            return provider;
        }

        public HelpCookieProviderBuilder With(Mock<ICandidateService> candidateService)
        {
            _candidateService = candidateService;
            return this;
        }
    }
}