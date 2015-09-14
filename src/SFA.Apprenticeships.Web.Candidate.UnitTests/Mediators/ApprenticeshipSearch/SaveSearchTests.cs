using SFA.Apprenticeships.Web.Common.UnitTests.Mediators;

namespace SFA.Apprenticeships.Web.Candidate.UnitTests.Mediators.ApprenticeshipSearch
{
    using System;
    using Builders;
    using Candidate.Mediators.Search;
    using Candidate.Providers;
    using Candidate.ViewModels.VacancySearch;
    using Common.Constants;
    using Constants.Pages;
    using Moq;
    using NUnit.Framework;

    [TestFixture]
    public class SaveSearchTests
    {
        [Test]
        public void Ok()
        {
            var candidateId = Guid.NewGuid();
            var candidateServiceProvider = new Mock<ICandidateServiceProvider>();
            candidateServiceProvider.Setup(p => p.CreateSavedSearch(candidateId, It.IsAny<ApprenticeshipSearchViewModel>())).Returns<Guid, ApprenticeshipSearchViewModel>((cid, vm) => vm);
            var mediator = new ApprenticeshipSearchMediatorBuilder().With(candidateServiceProvider).Build();
            var viewModel = new ApprenticeshipSearchViewModelBuilder().Build();

            var response = mediator.SaveSearch(candidateId, viewModel);

            response.AssertMessage(ApprenticeshipSearchMediatorCodes.SaveSearch.Ok, VacancySearchResultsPageMessages.SaveSearchSuccess, UserMessageLevel.Success, true);
            candidateServiceProvider.Verify(p => p.CreateSavedSearch(candidateId, viewModel), Times.Once);
        }

        [Test]
        public void HasError()
        {
            var candidateId = Guid.NewGuid();
            var candidateServiceProvider = new Mock<ICandidateServiceProvider>();
            candidateServiceProvider.Setup(p => p.CreateSavedSearch(candidateId, It.IsAny<ApprenticeshipSearchViewModel>())).Returns(new ApprenticeshipSearchViewModelBuilder().WithMessage(VacancySearchResultsPageMessages.SaveSearchFailed).Build());
            var mediator = new ApprenticeshipSearchMediatorBuilder().With(candidateServiceProvider).Build();
            var viewModel = new ApprenticeshipSearchViewModelBuilder().Build();

            var response = mediator.SaveSearch(candidateId, viewModel);

            response.AssertMessage(ApprenticeshipSearchMediatorCodes.SaveSearch.HasError, VacancySearchResultsPageMessages.SaveSearchFailed, UserMessageLevel.Error, true);
            candidateServiceProvider.Verify(p => p.CreateSavedSearch(candidateId, viewModel), Times.Once);
        }
    }
}