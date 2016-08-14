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
    [Parallelizable]
    public class DeleteSavedSearchTests
    {
        [Test]
        public void Success()
        {
            // Arrange.
            var candidateId = Guid.NewGuid();
            var savedSearchId = Guid.NewGuid();
            var candidateService = new Mock<ICandidateService>();

            candidateService.Setup(cs => cs.DeleteSavedSearch(candidateId, savedSearchId)).Returns(new SavedSearch());

            var provider = new CandidateServiceProviderBuilder().With(candidateService).Build();

            // Act.
            var response = provider.DeleteSavedSearch(candidateId, savedSearchId);

            // Assert.
            response.Should().NotBeNull();
            candidateService.Verify(cs => cs.DeleteSavedSearch(candidateId, savedSearchId), Times.Once);
        }

        [Test]
        public void Error()
        {
            // Arrange.
            var candidateId = Guid.NewGuid();
            var savedSearchId = Guid.NewGuid();
            var candidateService = new Mock<ICandidateService>();

            candidateService.Setup(cs => cs.DeleteSavedSearch(candidateId, savedSearchId)).Throws<Exception>();

            var provider = new CandidateServiceProviderBuilder().With(candidateService).Build();

            // Act.
            var response = provider.DeleteSavedSearch(candidateId, savedSearchId);

            // Assert.
            response.Should().NotBeNull();
            candidateService.Verify(cs => cs.DeleteSavedSearch(candidateId, savedSearchId), Times.Once);
            response.HasError().Should().BeTrue();
            response.ViewModelMessage.Should().Be(AccountPageMessages.DeleteSavedSearchFailed);
        }

        [TestCase(true)]
        [TestCase(false)]
        public void TestSavedSearchMappings(bool alertsEnabled)
        {
            // Arrange.
            var candidateId = Guid.NewGuid();
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

            candidateService.Setup(cs => cs.DeleteSavedSearch(candidateId, savedSearchId)).Returns(savedSearch);
            
            var provider = new CandidateServiceProviderBuilder().With(candidateService).Build();

            // Act.
            var viewModel = provider.DeleteSavedSearch(candidateId, savedSearchId);

            // Assert.
            candidateService.Verify(cs => cs.DeleteSavedSearch(candidateId, savedSearchId), Times.Once);

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