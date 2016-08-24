namespace SFA.Apprenticeships.Web.Recruit.UnitTests.Mediators.Application
{
    using System;
    using Apprenticeships.Application.Interfaces;
    using Common.UnitTests.Mediators;
    using Moq;
    using NUnit.Framework;
    using Raa.Common.ViewModels.Application;
    using Recruit.Mediators.Application;

    [TestFixture]
    public class ApprenticeshipApplicationMediatorTests
    {
        [Test]
        public void ShouldReturnNoVacancyIdIfNoVacancyGuidIsSuppliedOnReview()
        {
            var logService = new Mock<ILogService>();
            var mediator = new ApprenticeshipApplicationMediator(null,null, null, null, logService.Object);

            var viewModel = new ApplicationSelectionViewModel(new VacancyApplicationsSearchViewModel(), Guid.Empty);

            var respone = mediator.Review(viewModel);

            respone.AssertCodeAndMessage(ApprenticeshipApplicationMediatorCodes.Review.NoApplicationId, false, false);
            logService.Verify(l => l.Error("Review vacancy failed: VacancyGuid is empty."));
        }

        [Test]
        public void ShouldReturnNoVacancyIdIfNoVacancyGuidIsSuppliedOnConfirmSuccessfulDecision()
        {
            var logService = new Mock<ILogService>();
            var mediator = new ApprenticeshipApplicationMediator(null, null, null, null, logService.Object);

            var viewModel = new ApplicationSelectionViewModel(new VacancyApplicationsSearchViewModel(), Guid.Empty);

            var respone = mediator.ConfirmSuccessfulDecision(viewModel);

            respone.AssertCodeAndMessage(ApprenticeshipApplicationMediatorCodes.ConfirmSuccessfulDecision.NoApplicationId, false, false);
            logService.Verify(l => l.Error("Confirm successful decision failed: VacancyGuid is empty."));
        }

        [Test]
        public void ShouldReturnNoVacancyIdIfNoVacancyGuidIsSuppliedOnConfirmUnsuccessfulDecision()
        {
            var logService = new Mock<ILogService>();
            var mediator = new ApprenticeshipApplicationMediator(null, null, null, null, logService.Object);

            var viewModel = new ApplicationSelectionViewModel(new VacancyApplicationsSearchViewModel(), Guid.Empty);

            var respone = mediator.ConfirmUnsuccessfulDecision(viewModel);

            respone.AssertCodeAndMessage(ApprenticeshipApplicationMediatorCodes.ConfirmUnsuccessfulDecision.NoApplicationId, false, false);
            logService.Verify(l => l.Error("Confirm unsuccessful decision failed: VacancyGuid is empty."));
        }
    }
}