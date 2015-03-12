namespace SFA.Apprenticeships.Web.Candidate.UnitTests.Providers.CandidateServiceProvider
{
    using System;
    using Application.Interfaces.Candidates;
    using Constants.Pages;
    using Domain.Entities.Candidates;
    using FluentAssertions;
    using Moq;
    using NUnit.Framework;
    using Ploeh.AutoFixture;

    [TestFixture]
    public class DeleteSavedSearchTests
    {
        [Test]
        public void Success()
        {
            var savedSearchId = Guid.NewGuid();
            var candidateService = new Mock<ICandidateService>();
            candidateService.Setup(cs => cs.DeleteSavedSearch(savedSearchId)).Returns(new SavedSearch());
            var provider = new CandidateServiceProviderBuilder().With(candidateService).Build();

            var response = provider.DeleteSavedSearch(savedSearchId);

            response.Should().NotBeNull();
            candidateService.Verify(cs => cs.DeleteSavedSearch(savedSearchId), Times.Once);
        }

        [Test]
        public void Error()
        {
            var savedSearchId = Guid.NewGuid();
            var candidateService = new Mock<ICandidateService>();
            candidateService.Setup(cs => cs.DeleteSavedSearch(savedSearchId)).Throws<Exception>();
            var provider = new CandidateServiceProviderBuilder().With(candidateService).Build();

            var response = provider.DeleteSavedSearch(savedSearchId);

            response.Should().NotBeNull();
            candidateService.Verify(cs => cs.DeleteSavedSearch(savedSearchId), Times.Once);
            response.HasError().Should().BeTrue();
            response.ViewModelMessage.Should().Be(AccountPageMessages.DeleteSavedSearchFailed);
        }

        [TestCase(true)]
        [TestCase(false)]
        public void TestSavedSearchMappings(bool alertsEnabled)
        {
            var savedSearchId = Guid.NewGuid();
            var candidateService = new Mock<ICandidateService>();
            var savedSearch = new Fixture().Build<SavedSearch>()
                .With(s => s.EntityId, savedSearchId)
                .With(s => s.AlertsEnabled, alertsEnabled)
                .With(s => s.Keywords, string.Empty)
                .With(s => s.ApprenticeshipLevel, "All")
                .With(s => s.Location, "CV1 2WT")
                .With(s => s.WithinDistance, 5)
                .Create();
            candidateService.Setup(cs => cs.DeleteSavedSearch(savedSearchId)).Returns(savedSearch);
            var provider = new CandidateServiceProviderBuilder().With(candidateService).Build();

            var viewModel = provider.DeleteSavedSearch(savedSearchId);

            candidateService.Verify(cs => cs.DeleteSavedSearch(savedSearchId), Times.Once);

            viewModel.Should().NotBeNull();
            viewModel.Id.Should().Be(savedSearchId);
            viewModel.Name.Should().Be("Within 5 miles of CV1 2WT");
            viewModel.SearchUrl.Should().NotBeNull();
            viewModel.SearchUrl.Value.Should().NotBeNullOrEmpty();
            viewModel.AlertsEnabled.Should().Be(alertsEnabled);
            viewModel.ApprenticeshipLevel.Should().Be("All");
        }
    }
}