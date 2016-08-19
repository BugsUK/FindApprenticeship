﻿namespace SFA.Apprenticeships.Web.Candidate.UnitTests.Mediators.ApprenticeshipApplication
{
    using System;
    using Candidate.Mediators.Application;
    using Candidate.ViewModels.Applications;
    using Candidate.ViewModels.Candidate;
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
    public class PreviewAndSubmitTests : TestsBase
    {
        private const int ValidVacancyId = 1;

        [Test]
        public void Error()
        {
            var viewModel = new ApprenticeshipApplicationViewModel
            {
                Candidate = new ApprenticeshipCandidateViewModel(),
                VacancyDetail = new ApprenticeshipVacancyDetailViewModel()
            };
            ApprenticeshipApplicationProvider.Setup(p => p.GetApplicationViewModel(It.IsAny<Guid>(), ValidVacancyId))
                .Returns(new ApprenticeshipApplicationViewModel("Has error") {Status = ApplicationStatuses.Draft});

            var response = Mediator.PreviewAndSubmit(Guid.NewGuid(), ValidVacancyId, viewModel);

            response.AssertMessage(ApprenticeshipApplicationMediatorCodes.PreviewAndSubmit.Error,
                ApplicationPageMessages.PreviewFailed, UserMessageLevel.Warning, true);
        }

        [Test]
        public void IncorrectState()
        {
            var viewModel = new ApprenticeshipApplicationViewModel
            {
                Candidate = new ApprenticeshipCandidateViewModel(),
                VacancyDetail = new ApprenticeshipVacancyDetailViewModel()
            };
            ApprenticeshipApplicationProvider.Setup(p => p.GetApplicationViewModel(It.IsAny<Guid>(), ValidVacancyId))
                .Returns(new ApprenticeshipApplicationViewModel
                {
                    Status = ApplicationStatuses.Submitting,
                    VacancyDetail = new ApprenticeshipVacancyDetailViewModel
                    {
                        VacancyStatus = VacancyStatuses.Live
                    }
                });

            var response = Mediator.PreviewAndSubmit(Guid.NewGuid(), ValidVacancyId, viewModel);

            response.AssertMessage(ApprenticeshipApplicationMediatorCodes.PreviewAndSubmit.IncorrectState,
                MyApplicationsPageMessages.ApplicationInIncorrectState, UserMessageLevel.Info, false);
        }

        [Test]
        public void OfflineVacancy()
        {
            var viewModel = new ApprenticeshipApplicationViewModel
            {
                Candidate = new ApprenticeshipCandidateViewModel(),
                VacancyDetail = new ApprenticeshipVacancyDetailViewModel()
            };
            ApprenticeshipApplicationProvider.Setup(p => p.GetApplicationViewModel(It.IsAny<Guid>(), ValidVacancyId))
                .Returns(new ApprenticeshipApplicationViewModel
                {
                    Status = ApplicationStatuses.Draft,
                    VacancyDetail = new ApprenticeshipVacancyDetailViewModel
                    {
                        ApplyViaEmployerWebsite = true
                    }
                });
            ApprenticeshipApplicationProvider.Setup(
                p =>
                    p.PatchApplicationViewModel(It.IsAny<Guid>(), It.IsAny<ApprenticeshipApplicationViewModel>(),
                        It.IsAny<ApprenticeshipApplicationViewModel>()))
                .Returns<Guid, ApprenticeshipApplicationViewModel, ApprenticeshipApplicationViewModel>(
                    (cid, svm, vm) => vm);
            ApprenticeshipApplicationProvider.Setup(p => p.SubmitApplication(It.IsAny<Guid>(), It.IsAny<int>()))
                .Returns(viewModel);

            var response = Mediator.PreviewAndSubmit(Guid.NewGuid(), ValidVacancyId, viewModel);

            response.AssertCode(ApprenticeshipApplicationMediatorCodes.PreviewAndSubmit.OfflineVacancy, false);
        }

        [Test]
        public void Ok()
        {
            var viewModel = new ApprenticeshipApplicationViewModel
            {
                Candidate = new ApprenticeshipCandidateViewModel(),
                VacancyDetail = new ApprenticeshipVacancyDetailViewModel()
            };
            ApprenticeshipApplicationProvider.Setup(p => p.GetApplicationViewModel(It.IsAny<Guid>(), ValidVacancyId))
                .Returns(new ApprenticeshipApplicationViewModel
                {
                    Status = ApplicationStatuses.Draft,
                    VacancyDetail = new ApprenticeshipVacancyDetailViewModel
                    {
                        VacancyStatus = VacancyStatuses.Live
                    }
                });
            ApprenticeshipApplicationProvider.Setup(
                p =>
                    p.PatchApplicationViewModel(It.IsAny<Guid>(), It.IsAny<ApprenticeshipApplicationViewModel>(),
                        It.IsAny<ApprenticeshipApplicationViewModel>()))
                .Returns<Guid, ApprenticeshipApplicationViewModel, ApprenticeshipApplicationViewModel>(
                    (cid, svm, vm) => vm);
            ApprenticeshipApplicationProvider.Setup(p => p.SubmitApplication(It.IsAny<Guid>(), It.IsAny<int>()))
                .Returns(viewModel);

            var response = Mediator.PreviewAndSubmit(Guid.NewGuid(), ValidVacancyId, viewModel);

            response.AssertCode(ApprenticeshipApplicationMediatorCodes.PreviewAndSubmit.Ok, false, true);
        }

        [Test]
        public void VacancyExpired()
        {
            var viewModel = new ApprenticeshipApplicationViewModel
            {
                Candidate = new ApprenticeshipCandidateViewModel(),
                VacancyDetail = new ApprenticeshipVacancyDetailViewModel()
            };
            ApprenticeshipApplicationProvider.Setup(p => p.GetApplicationViewModel(It.IsAny<Guid>(), ValidVacancyId))
                .Returns(new ApprenticeshipApplicationViewModel
                {
                    Status = ApplicationStatuses.Draft,
                    VacancyDetail = new ApprenticeshipVacancyDetailViewModel
                    {
                        VacancyStatus = VacancyStatuses.Expired
                    }
                });
            ApprenticeshipApplicationProvider.Setup(
                p =>
                    p.PatchApplicationViewModel(It.IsAny<Guid>(), It.IsAny<ApprenticeshipApplicationViewModel>(),
                        It.IsAny<ApprenticeshipApplicationViewModel>()))
                .Returns<Guid, ApprenticeshipApplicationViewModel, ApprenticeshipApplicationViewModel>(
                    (cid, svm, vm) => vm);
            ApprenticeshipApplicationProvider.Setup(p => p.SubmitApplication(It.IsAny<Guid>(), It.IsAny<int>()))
                .Returns(viewModel);

            var response = Mediator.PreviewAndSubmit(Guid.NewGuid(), ValidVacancyId, viewModel);

            response.AssertCode(ApprenticeshipApplicationMediatorCodes.PreviewAndSubmit.VacancyNotFound, false);
        }

        [Test]
        public void VacancyNotFound()
        {
            var viewModel = new ApprenticeshipApplicationViewModel
            {
                Candidate = new ApprenticeshipCandidateViewModel(),
                VacancyDetail = new ApprenticeshipVacancyDetailViewModel()
            };
            ApprenticeshipApplicationProvider.Setup(p => p.GetApplicationViewModel(It.IsAny<Guid>(), ValidVacancyId))
                .Returns(new ApprenticeshipApplicationViewModel
                {
                    Status = ApplicationStatuses.ExpiredOrWithdrawn,
                    VacancyDetail = new ApprenticeshipVacancyDetailViewModel()
                });

            var response = Mediator.PreviewAndSubmit(Guid.NewGuid(), ValidVacancyId, viewModel);

            response.AssertCode(ApprenticeshipApplicationMediatorCodes.PreviewAndSubmit.VacancyNotFound, false);
        }

        [Test]
        public void ValidationError()
        {
            var viewModel = new ApprenticeshipApplicationViewModel
            {
                Candidate = new ApprenticeshipCandidateViewModel
                {
                    AboutYou = new AboutYouViewModel()
                },
                VacancyDetail = new ApprenticeshipVacancyDetailViewModel()
            };
            ApprenticeshipApplicationProvider.Setup(p => p.GetApplicationViewModel(It.IsAny<Guid>(), ValidVacancyId))
                .Returns(new ApprenticeshipApplicationViewModel
                {
                    Status = ApplicationStatuses.Draft,
                    VacancyDetail = new ApprenticeshipVacancyDetailViewModel
                    {
                        VacancyStatus = VacancyStatuses.Live
                    }
                });
            ApprenticeshipApplicationProvider.Setup(
                p =>
                    p.PatchApplicationViewModel(It.IsAny<Guid>(), It.IsAny<ApprenticeshipApplicationViewModel>(),
                        It.IsAny<ApprenticeshipApplicationViewModel>()))
                .Returns<Guid, ApprenticeshipApplicationViewModel, ApprenticeshipApplicationViewModel>(
                    (cid, svm, vm) => vm);

            var response = Mediator.PreviewAndSubmit(Guid.NewGuid(), ValidVacancyId, viewModel);

            response.AssertValidationResult(ApprenticeshipApplicationMediatorCodes.PreviewAndSubmit.ValidationError);
        }
    }
}