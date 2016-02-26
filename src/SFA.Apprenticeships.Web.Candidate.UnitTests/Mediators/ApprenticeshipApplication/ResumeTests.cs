using SFA.Apprenticeships.Web.Common.UnitTests.Mediators;

namespace SFA.Apprenticeships.Web.Candidate.UnitTests.Mediators.ApprenticeshipApplication
{
    using System;
    using Builders;
    using Candidate.Mediators.Application;
    using Common.Constants;
    using Common.ViewModels.Applications;
    using Common.ViewModels.VacancySearch;
    using Constants.Pages;
    using Domain.Entities.Applications;
    using Domain.Entities.Vacancies;
    using Moq;
    using NUnit.Framework;

    [TestFixture]
    public class ResumeTests : TestsBase
    {
        private const int ValidVacancyId = 1;
        private const int InvalidVacancyId = 99999;

        [Test]
        public void HasError()
        {
            ApprenticeshipApplicationProvider.Setup(p => p.GetApplicationViewModel(It.IsAny<Guid>(), InvalidVacancyId)).Returns(new ApprenticeshipApplicationViewModel("Vacancy not found"));
            
            var response = Mediator.Resume(Guid.NewGuid(), InvalidVacancyId);

            response.AssertMessage(ApprenticeshipApplicationMediatorCodes.Resume.HasError, "Vacancy not found", UserMessageLevel.Warning, false);
        }

        [Test]
        public void IncorrectState()
        {
            ApprenticeshipApplicationProvider.Setup(p => p.GetApplicationViewModel(It.IsAny<Guid>(), ValidVacancyId)).Returns(new ApprenticeshipApplicationViewModelBuilder().WithStatus(ApplicationStatuses.Submitted).Build);

            var response = Mediator.Resume(Guid.NewGuid(), ValidVacancyId);

            response.AssertMessage(ApprenticeshipApplicationMediatorCodes.Resume.IncorrectState, MyApplicationsPageMessages.ApplicationInIncorrectState, UserMessageLevel.Info, false);
        }

        [Test]
        public void Ok()
        {
            ApprenticeshipApplicationProvider.Setup(p => p.GetApplicationViewModel(It.IsAny<Guid>(), ValidVacancyId)).Returns(new ApprenticeshipApplicationViewModelBuilder().WithVacancyStatus(VacancyStatuses.Live).Build);
            
            var response = Mediator.Resume(Guid.NewGuid(), ValidVacancyId);

            response.AssertCode(ApprenticeshipApplicationMediatorCodes.Resume.Ok, false, true);
        }

        [Test]
        public void ApplicationExpired()
        {
            ApprenticeshipApplicationProvider.Setup(p => p.GetApplicationViewModel(It.IsAny<Guid>(), ValidVacancyId)).Returns(new ApprenticeshipApplicationViewModel
            {
                Status = ApplicationStatuses.ExpiredOrWithdrawn
            });

            var response = Mediator.Resume(Guid.NewGuid(), ValidVacancyId);

            response.AssertMessage(ApprenticeshipApplicationMediatorCodes.Resume.HasError, MyApplicationsPageMessages.ApprenticeshipNoLongerAvailable, UserMessageLevel.Warning, false);
        }

        [Test]
        public void VacancyExpired()
        {
            ApprenticeshipApplicationProvider.Setup(p => p.GetApplicationViewModel(It.IsAny<Guid>(), ValidVacancyId)).Returns(new ApprenticeshipApplicationViewModel
            {
                VacancyDetail = new ApprenticeshipVacancyDetailViewModel
                {
                    VacancyStatus = VacancyStatuses.Expired
                }
            });

            var response = Mediator.Resume(Guid.NewGuid(), ValidVacancyId);

            response.AssertMessage(ApprenticeshipApplicationMediatorCodes.Resume.HasError, MyApplicationsPageMessages.ApprenticeshipNoLongerAvailable, UserMessageLevel.Warning, false);
        }
    }
}