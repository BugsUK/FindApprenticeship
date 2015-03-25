namespace SFA.Apprenticeships.Web.Candidate.UnitTests.Mediators.ApprenticeshipSearch
{
    using System;
    using Candidate.Mediators.Search;
    using Candidate.Providers;
    using Candidate.ViewModels.Account;
    using Candidate.ViewModels.VacancySearch;
    using Common.Constants;
    using Constants.Pages;
    using FluentAssertions;
    using Moq;
    using NUnit.Framework;
    using Ploeh.AutoFixture;

    [TestFixture]
    public class RunSavedSearchTests
    {
        [Test]
        public void Ok()
        {
            // Arrange.
            var savedSearchId = Guid.NewGuid();
            var candidateId = Guid.NewGuid();
            var candidateServiceProvider = new Mock<ICandidateServiceProvider>();

            var viewModel = new ApprenticeshipSearchViewModel
            {
                SavedSearchId = savedSearchId.ToString()
            };

            var savedSearch = new Fixture()
                .Build<SavedSearchViewModel>()
                .With(each => each.ViewModelMessage, null)
                .Create();

            candidateServiceProvider
                .Setup(p => p.GetSavedSearch(savedSearchId)).Returns(savedSearch);

            var mediator = new ApprenticeshipSearchMediatorBuilder()
                .With(candidateServiceProvider).Build();

            // Act.
            var response = mediator.RunSavedSearch(candidateId, viewModel);

            // Assert.
            candidateServiceProvider.Verify(p => p.GetSavedSearch(savedSearchId), Times.Once);

            response.Code.Should().Be(ApprenticeshipSearchMediatorCodes.RunSavedSearch.Ok);
            response.ViewModel.Should().Be(savedSearch);
        }

        [Test]
        public void SavedSearchNotFound_UnknownSavedSearchId()
        {
            // Arrange.
            var savedSearchId = Guid.NewGuid();
            var candidateId = Guid.NewGuid();
            var candidateServiceProvider = new Mock<ICandidateServiceProvider>();

            var viewModel = new ApprenticeshipSearchViewModel
            {
                SavedSearchId = savedSearchId.ToString()
            };

            candidateServiceProvider
                .Setup(p => p.GetSavedSearch(savedSearchId)).Returns((SavedSearchViewModel)null);

            var mediator = new ApprenticeshipSearchMediatorBuilder()
                .With(candidateServiceProvider).Build();

            // Act.
            var response = mediator.RunSavedSearch(candidateId, viewModel);

            // Assert.
            candidateServiceProvider.Verify(p => p.GetSavedSearch(savedSearchId), Times.Once);

            response.AssertMessage(
                ApprenticeshipSearchMediatorCodes.RunSavedSearch.SavedSearchNotFound,
                ApprenticeshipsSearchPageMessages.SavedSearchNotFound,
                UserMessageLevel.Error,
                false);
        }

        [Test]
        public void SavedSearchNotFound_InvalidSavedSearchId()
        {
            // Arrange.
            var candidateId = Guid.NewGuid();
            var candidateServiceProvider = new Mock<ICandidateServiceProvider>();

            var viewModel = new ApprenticeshipSearchViewModel
            {
                SavedSearchId = "X"
            };

            var mediator = new ApprenticeshipSearchMediatorBuilder()
                .With(candidateServiceProvider).Build();

            // Act.
            var response = mediator.RunSavedSearch(candidateId, viewModel);

            // Assert.
            candidateServiceProvider.Verify(p => p.GetSavedSearch(It.IsAny<Guid>()), Times.Never);

            response.AssertMessage(
                ApprenticeshipSearchMediatorCodes.RunSavedSearch.SavedSearchNotFound,
                ApprenticeshipsSearchPageMessages.SavedSearchNotFound,
                UserMessageLevel.Error,
                false);
        }

        [Test]
        public void RunSaveSearchFailed()
        {
            // Arrange.
            var savedSearchId = Guid.NewGuid();
            var candidateId = Guid.NewGuid();
            var candidateServiceProvider = new Mock<ICandidateServiceProvider>();

            var viewModel = new ApprenticeshipSearchViewModel
            {
                SavedSearchId = savedSearchId.ToString()
            };

            candidateServiceProvider
                .Setup(p => p.GetSavedSearch(savedSearchId))
                .Returns(new SavedSearchViewModel
                {
                    ViewModelMessage = "Error" 
                });

            var mediator = new ApprenticeshipSearchMediatorBuilder()
                .With(candidateServiceProvider).Build();

            // Act.
            var response = mediator.RunSavedSearch(candidateId, viewModel);

            // Assert.
            candidateServiceProvider.Verify(p => p.GetSavedSearch(savedSearchId), Times.Once);

            response.AssertMessage(
                ApprenticeshipSearchMediatorCodes.RunSavedSearch.RunSaveSearchFailed,
                ApprenticeshipsSearchPageMessages.RunSavedSearchFailed,
                UserMessageLevel.Error,
                true);
        }
    }
}
