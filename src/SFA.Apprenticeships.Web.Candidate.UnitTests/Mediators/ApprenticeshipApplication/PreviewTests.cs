namespace SFA.Apprenticeships.Web.Candidate.UnitTests.Mediators.ApprenticeshipApplication
{
    using System;
    using Candidate.Mediators.Application;
    using Candidate.ViewModels.Applications;
    using Candidate.ViewModels.VacancySearch;
    using Common.Constants;
    using Common.UnitTests.Mediators;
    using Constants.Pages;
    using Domain.Entities.Applications;
    using Domain.Entities.Vacancies;
    using Moq;
    using NUnit.Framework;

    [TestFixture]
    [Parallelizable]
    public class PreviewTests : TestsBase
    {
        private const int ValidVacancyId = 1;
        private const int InvalidVacancyId = 99999;

        [Test]
        public void HasError()
        {
            ApprenticeshipApplicationProvider.Setup(
                p => p.GetApplicationPreviewViewModel(It.IsAny<Guid>(), InvalidVacancyId))
                .Returns(new ApprenticeshipApplicationPreviewViewModel("Vacancy not found"));

            var response = Mediator.Preview(Guid.NewGuid(), InvalidVacancyId);

            response.AssertCode(ApprenticeshipApplicationMediatorCodes.Preview.HasError, false);
        }

        [Test]
        public void IncorrectState()
        {
            ApprenticeshipApplicationProvider
                .Setup(p => p.GetApplicationPreviewViewModel(It.IsAny<Guid>(), ValidVacancyId))
                .Returns(new ApprenticeshipApplicationPreviewViewModel
                {
                    Status = ApplicationStatuses.Submitted,
                    VacancyDetail = new ApprenticeshipVacancyDetailViewModel
                    {
                        VacancyStatus = VacancyStatuses.Live
                    }
                });

            var response = Mediator.Preview(Guid.NewGuid(), ValidVacancyId);

            response.AssertMessage(ApprenticeshipApplicationMediatorCodes.Preview.IncorrectState,
                MyApplicationsPageMessages.ApplicationInIncorrectState, UserMessageLevel.Info, false);
        }

        [Test]
        public void OfflineVacancy()
        {
            ApprenticeshipApplicationProvider
                .Setup(p => p.GetApplicationPreviewViewModel(It.IsAny<Guid>(), ValidVacancyId))
                .Returns(new ApprenticeshipApplicationPreviewViewModel
                {
                    VacancyDetail = new ApprenticeshipVacancyDetailViewModel
                    {
                        ApplyViaEmployerWebsite = true
                    }
                });

            var response = Mediator.Preview(Guid.NewGuid(), ValidVacancyId);

            response.AssertCode(ApprenticeshipApplicationMediatorCodes.Preview.OfflineVacancy, false);
        }

        [Test]
        public void Ok()
        {
            ApprenticeshipApplicationProvider
                .Setup(p => p.GetApplicationPreviewViewModel(It.IsAny<Guid>(), ValidVacancyId))
                .Returns(new ApprenticeshipApplicationPreviewViewModel
                {
                    Status = ApplicationStatuses.Draft,
                    VacancyDetail = new ApprenticeshipVacancyDetailViewModel
                    {
                        VacancyStatus = VacancyStatuses.Live
                    }
                });

            var response = Mediator.Preview(Guid.NewGuid(), ValidVacancyId);

            response.AssertCode(ApprenticeshipApplicationMediatorCodes.Preview.Ok, true);
        }

        [Test]
        public void VacancyExpired()
        {
            ApprenticeshipApplicationProvider.Setup(
                p => p.GetApplicationPreviewViewModel(It.IsAny<Guid>(), ValidVacancyId))
                .Returns(new ApprenticeshipApplicationPreviewViewModel
                {
                    VacancyDetail = new ApprenticeshipVacancyDetailViewModel
                    {
                        VacancyStatus = VacancyStatuses.Expired
                    }
                });

            var response = Mediator.Preview(Guid.NewGuid(), ValidVacancyId);

            response.AssertCode(ApprenticeshipApplicationMediatorCodes.Preview.VacancyNotFound, false);
        }

        [Test]
        public void VacancyNotFound()
        {
            ApprenticeshipApplicationProvider
                .Setup(p => p.GetApplicationPreviewViewModel(It.IsAny<Guid>(), InvalidVacancyId))
                .Returns(new ApprenticeshipApplicationPreviewViewModel
                {
                    Status = ApplicationStatuses.ExpiredOrWithdrawn,
                    VacancyDetail = new ApprenticeshipVacancyDetailViewModel()
                });

            var response = Mediator.Preview(Guid.NewGuid(), InvalidVacancyId);

            response.AssertCode(ApprenticeshipApplicationMediatorCodes.Preview.VacancyNotFound, false);
        }
    }
}