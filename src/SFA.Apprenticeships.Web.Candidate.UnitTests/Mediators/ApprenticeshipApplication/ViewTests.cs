using SFA.Apprenticeships.Web.Common.UnitTests.Mediators;

namespace SFA.Apprenticeships.Web.Candidate.UnitTests.Mediators.ApprenticeshipApplication
{
    using System;
    using Builders;
    using Candidate.Mediators.Application;
    using Candidate.Providers;
    using Common.Constants;
    using Common.Constants.Pages;
    using Common.Models.Application;
    using Constants.Pages;
    using Domain.Entities.Applications;
    using Domain.Entities.Vacancies;
    using Moq;
    using NUnit.Framework;

    [TestFixture]
    public class ViewTests
    {
        private const int ValidVacancyId = 1;

        [Test]
        public void VacancyNotFound()
        {
            var viewModel = new ApprenticeshipApplicationViewModelBuilder().WithStatus(ApplicationStatuses.ExpiredOrWithdrawn).Build();
            var apprenticeshipApplicationProvider = new Mock<IApprenticeshipApplicationProvider>();
            apprenticeshipApplicationProvider.Setup(p => p.GetApplicationViewModel(It.IsAny<Guid>(), It.IsAny<int>())).Returns(viewModel);
            var mediator = new ApprenticeshipApplicationMediatorBuilder().With(apprenticeshipApplicationProvider).Build();

            var response = mediator.View(Guid.NewGuid(), ValidVacancyId);

            //Should still be able to view the application even if the vacancy is not available
            response.AssertCode(ApprenticeshipApplicationMediatorCodes.View.Ok, true);
        }

        [Test]
        public void ApplicationNotFound()
        {
            var viewModel = new ApprenticeshipApplicationViewModelBuilder().HasError(ApplicationViewModelStatus.ApplicationNotFound, MyApplicationsPageMessages.ApplicationNotFound).Build();
            var apprenticeshipApplicationProvider = new Mock<IApprenticeshipApplicationProvider>();
            apprenticeshipApplicationProvider.Setup(p => p.GetApplicationViewModel(It.IsAny<Guid>(), It.IsAny<int>())).Returns(viewModel);
            var mediator = new ApprenticeshipApplicationMediatorBuilder().With(apprenticeshipApplicationProvider).Build();

            var response = mediator.View(Guid.NewGuid(), ValidVacancyId);

            //Should still be able to view the application even if the vacancy is not available
            response.AssertMessage(ApprenticeshipApplicationMediatorCodes.View.ApplicationNotFound, ApplicationPageMessages.ViewApplicationFailed, UserMessageLevel.Warning, true);
        }

        [Test]
        public void Ok()
        {
            var candidateId = Guid.NewGuid();
            var apprenticeshipApplicationProvider = new Mock<IApprenticeshipApplicationProvider>();
            apprenticeshipApplicationProvider.Setup(p => p.GetApplicationViewModel(candidateId, ValidVacancyId)).Returns(new ApprenticeshipApplicationViewModelBuilder().WithVacancyStatus(VacancyStatuses.Live).Build());
            var mediator = new ApprenticeshipApplicationMediatorBuilder().With(apprenticeshipApplicationProvider).Build();

            var response = mediator.View(candidateId, ValidVacancyId);

            response.AssertCode(ApprenticeshipApplicationMediatorCodes.View.Ok, true);
        }
    }
}