namespace SFA.Apprenticeships.Web.Candidate.UnitTests.Mediators.ApprenticeshipApplication
{
    using System;
    using System.Globalization;
    using Candidate.Mediators.Application;
    using Candidate.ViewModels.VacancySearch;
    using Common.Constants;
    using Common.UnitTests.Mediators;
    using FluentAssertions;
    using Moq;
    using NUnit.Framework;

    [TestFixture]
    [Parallelizable]
    public class SaveVacancyTests : TestsBase
    {
        private const int TestVacancyId = 42;

        private readonly Guid _testCandidateId = Guid.NewGuid();

        [TestCase(null)]
        [TestCase("0")]
        [TestCase("X")]
        public void ShouldSetSavedVacancyCountToOne(string savedAndDraftCount)
        {
            // Arrange.
            var viewModel = new SavedVacancyViewModel();

            ApprenticeshipApplicationProvider
                .Setup(mock => mock.SaveVacancy(_testCandidateId, TestVacancyId))
                .Returns(viewModel);

            UserDataProvider
                .Setup(mock => mock.Get(UserDataItemNames.SavedAndDraftCount))
                .Returns(savedAndDraftCount);

            // Act.
            var response = Mediator.SaveVacancy(_testCandidateId, TestVacancyId);

            // Assert.
            UserDataProvider.Verify(mock => mock.Get(UserDataItemNames.SavedAndDraftCount), Times.Once);
            UserDataProvider.Verify(mock => mock.Push(UserDataItemNames.SavedAndDraftCount, "1"), Times.Once);

            response.AssertCode(ApprenticeshipApplicationMediatorCodes.SaveVacancy.Ok, true);
            response.ViewModel.Should().Be(viewModel);
        }

        [TestCase("1")]
        [TestCase("0")]
        [TestCase("X")]
        public void ShouldSetSavedVacancyCountToZero(string savedAndDraftCount)
        {
            // Arrange.
            var viewModel = new SavedVacancyViewModel();

            ApprenticeshipApplicationProvider
                .Setup(mock => mock.DeleteSavedVacancy(_testCandidateId, TestVacancyId))
                .Returns(viewModel);

            UserDataProvider
                .Setup(mock => mock.Get(UserDataItemNames.SavedAndDraftCount))
                .Returns(savedAndDraftCount);

            // Act.
            var response = Mediator.DeleteSavedVacancy(_testCandidateId, TestVacancyId);

            // Assert.
            UserDataProvider.Verify(mock => mock.Get(UserDataItemNames.SavedAndDraftCount), Times.Once);
            UserDataProvider.Verify(mock => mock.Push(UserDataItemNames.SavedAndDraftCount, "0"), Times.Once);

            response.AssertCode(ApprenticeshipApplicationMediatorCodes.DeleteSavedVacancy.Ok, true);
            response.ViewModel.Should().Be(viewModel);
        }

        [TestCase(1)]
        [TestCase(5)]
        public void ShouldIncrementSavedVacancyCount(int savedAndDraftCount)
        {
            // Arrange.
            var viewModel = new SavedVacancyViewModel();

            ApprenticeshipApplicationProvider
                .Setup(mock => mock.SaveVacancy(_testCandidateId, TestVacancyId))
                .Returns(viewModel);

            UserDataProvider
                .Setup(mock => mock.Get(UserDataItemNames.SavedAndDraftCount))
                .Returns(savedAndDraftCount.ToString(CultureInfo.InvariantCulture));

            // Act.
            var response = Mediator.SaveVacancy(_testCandidateId, TestVacancyId);

            // Assert.
            UserDataProvider.Verify(mock => mock.Get(UserDataItemNames.SavedAndDraftCount), Times.Once);
            UserDataProvider.Verify(
                mock =>
                    mock.Push(UserDataItemNames.SavedAndDraftCount,
                        (savedAndDraftCount + 1).ToString(CultureInfo.InvariantCulture)), Times.Once);

            response.AssertCode(ApprenticeshipApplicationMediatorCodes.SaveVacancy.Ok, true);
            response.ViewModel.Should().Be(viewModel);
        }

        [TestCase(1)]
        [TestCase(5)]
        public void ShouldDecrementVacancyCount(int savedAndDraftCount)
        {
            // Arrange.
            var viewModel = new SavedVacancyViewModel();

            ApprenticeshipApplicationProvider
                .Setup(mock => mock.DeleteSavedVacancy(_testCandidateId, TestVacancyId))
                .Returns(viewModel);

            UserDataProvider
                .Setup(mock => mock.Get(UserDataItemNames.SavedAndDraftCount))
                .Returns(savedAndDraftCount.ToString(CultureInfo.InvariantCulture));

            // Act.
            var response = Mediator.DeleteSavedVacancy(_testCandidateId, TestVacancyId);

            // Assert.
            UserDataProvider.Verify(mock => mock.Get(UserDataItemNames.SavedAndDraftCount), Times.Once);
            UserDataProvider.Verify(
                mock =>
                    mock.Push(UserDataItemNames.SavedAndDraftCount,
                        (savedAndDraftCount - 1).ToString(CultureInfo.InvariantCulture)), Times.Once);

            response.AssertCode(ApprenticeshipApplicationMediatorCodes.DeleteSavedVacancy.Ok, true);
            response.ViewModel.Should().Be(viewModel);
        }

        [TestCase(0)]
        [TestCase(-5)]
        public void ShouldNotDecrementVacancyCountBelowZero(int savedAndDraftCount)
        {
            // Arrange.
            var viewModel = new SavedVacancyViewModel();

            ApprenticeshipApplicationProvider
                .Setup(mock => mock.DeleteSavedVacancy(_testCandidateId, TestVacancyId))
                .Returns(viewModel);

            UserDataProvider
                .Setup(mock => mock.Get(UserDataItemNames.SavedAndDraftCount))
                .Returns(savedAndDraftCount.ToString(CultureInfo.InvariantCulture));

            // Act.
            var response = Mediator.DeleteSavedVacancy(_testCandidateId, TestVacancyId);

            // Assert.
            UserDataProvider.Verify(mock => mock.Get(UserDataItemNames.SavedAndDraftCount), Times.Once);
            UserDataProvider.Verify(mock => mock.Push(UserDataItemNames.SavedAndDraftCount, "0"), Times.Once);

            response.AssertCode(ApprenticeshipApplicationMediatorCodes.DeleteSavedVacancy.Ok, true);
            response.ViewModel.Should().Be(viewModel);
        }
    }
}