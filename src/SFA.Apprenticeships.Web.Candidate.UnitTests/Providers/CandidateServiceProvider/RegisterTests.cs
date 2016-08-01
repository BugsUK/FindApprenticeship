namespace SFA.Apprenticeships.Web.Candidate.UnitTests.Providers.CandidateServiceProvider
{
    using Application.Interfaces.Candidates;
    using Builders;
    using Domain.Entities.Candidates;
    using FluentAssertions;
    using Moq;
    using NUnit.Framework;

    [TestFixture]
    [Parallelizable]
    public class RegisterTests
    {
        [Test]
        public void Us796_Ac1_DefaultCommunicationPreferences()
        {
            Candidate candidate = null;
            var candidateService = new Mock<ICandidateService>();

            candidateService.Setup(cs => cs.Register(It.IsAny<Candidate>(), It.IsAny<string>())).Callback<Candidate, string>((c, s) => { candidate = c; });

            var provider = new CandidateServiceProviderBuilder().With(candidateService).Build();
            var viewModel = new RegisterViewModelBuilder().Build();

            var registered = provider.Register(viewModel);

            candidate.Should().NotBeNull();
            candidate.CommunicationPreferences.Should().NotBeNull();

            {
                var preferences = candidate.CommunicationPreferences.ApplicationStatusChangePreferences;
                    
                preferences.Should().NotBeNull();
                preferences.EnableEmail.Should().BeTrue();
                preferences.EnableText.Should().BeTrue();
            }

            {
                var preferences = candidate.CommunicationPreferences.ExpiringApplicationPreferences;
                    
                preferences.Should().NotBeNull();
                preferences.EnableEmail.Should().BeTrue();
                preferences.EnableText.Should().BeTrue();
            }

            {
                var preferences = candidate.CommunicationPreferences.MarketingPreferences;

                preferences.Should().NotBeNull();
                preferences.EnableEmail.Should().BeTrue();
                preferences.EnableText.Should().BeTrue();
            }

            {
                var preferences = candidate.CommunicationPreferences.SavedSearchPreferences;

                preferences.Should().NotBeNull();
                preferences.EnableEmail.Should().BeTrue();
                preferences.EnableText.Should().BeFalse();
            }

            candidate.CommunicationPreferences.VerifiedMobile.Should().BeFalse();
            candidate.CommunicationPreferences.MobileVerificationCode.Should().BeNullOrEmpty();

            registered.Should().BeTrue();
        }

        [Test]
        public void Us738_DoesNotAcceptUpdates()
        {
            Candidate candidate = null;
            var candidateService = new Mock<ICandidateService>();
            candidateService.Setup(cs => cs.Register(It.IsAny<Candidate>(), It.IsAny<string>())).Callback<Candidate, string>((c, s) => { candidate = c; });
            var provider = new CandidateServiceProviderBuilder().With(candidateService).Build();
            var viewModel = new RegisterViewModelBuilder().DoesNotAcceptUpdates().Build();

            var registered = provider.Register(viewModel);

            candidate.Should().NotBeNull();
            candidate.CommunicationPreferences.Should().NotBeNull();
            candidate.CommunicationPreferences.MarketingPreferences.EnableEmail.Should().BeFalse();
            candidate.CommunicationPreferences.MarketingPreferences.EnableText.Should().BeFalse();
            registered.Should().BeTrue();
        }

        [Test]
        public void Us773_DefaultHelpPreferences()
        {
            Candidate candidate = null;
            var candidateService = new Mock<ICandidateService>();
            candidateService.Setup(cs => cs.Register(It.IsAny<Candidate>(), It.IsAny<string>())).Callback<Candidate, string>((c, s) => { candidate = c; });
            var provider = new CandidateServiceProviderBuilder().With(candidateService).Build();
            var viewModel = new RegisterViewModelBuilder().Build();

            var registered = provider.Register(viewModel);

            candidate.Should().NotBeNull();
            candidate.HelpPreferences.Should().NotBeNull();
            candidate.HelpPreferences.ShowSearchTour.Should().BeTrue();
            registered.Should().BeTrue();
        }
    }
}