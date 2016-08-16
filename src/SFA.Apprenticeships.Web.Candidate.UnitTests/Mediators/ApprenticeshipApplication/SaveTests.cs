
namespace SFA.Apprenticeships.Web.Candidate.UnitTests.Mediators.ApprenticeshipApplication
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
    using Moq;
    using NUnit.Framework;

    [TestFixture]
    [Parallelizable]
    public class SaveTests : TestsBase
    {
        private const int ValidVacancyId = 1;

        [Test]
        public void AlreadySubmitted()
        {
            var viewModel = new ApprenticeshipApplicationViewModel
            {
                Candidate = new ApprenticeshipCandidateViewModel(),
                VacancyDetail = new ApprenticeshipVacancyDetailViewModel()
            };
            ApprenticeshipApplicationProvider.Setup(p => p.GetApplicationViewModel(It.IsAny<Guid>(), ValidVacancyId)).Returns(new ApprenticeshipApplicationViewModel { Status = ApplicationStatuses.Submitted, VacancyDetail = new ApprenticeshipVacancyDetailViewModel() });
            ApprenticeshipApplicationProvider.Setup(p => p.PatchApplicationViewModel(It.IsAny<Guid>(), It.IsAny<ApprenticeshipApplicationViewModel>(), It.IsAny<ApprenticeshipApplicationViewModel>())).Returns<Guid, ApprenticeshipApplicationViewModel, ApprenticeshipApplicationViewModel>((cid, svm, vm) => vm);

            var response = Mediator.Save(Guid.NewGuid(), ValidVacancyId, viewModel);

            response.AssertMessage(ApprenticeshipApplicationMediatorCodes.Save.IncorrectState, MyApplicationsPageMessages.ApplicationInIncorrectState, UserMessageLevel.Info, false);
        }

        [Test]
        public void Error()
        {
            var viewModel = new ApprenticeshipApplicationViewModel
            {
                Candidate = new ApprenticeshipCandidateViewModel(),
                VacancyDetail = new ApprenticeshipVacancyDetailViewModel()
            };
            ApprenticeshipApplicationProvider.Setup(p => p.GetApplicationViewModel(It.IsAny<Guid>(), ValidVacancyId)).Returns(new ApprenticeshipApplicationViewModel("Has Error"));
            
            var response = Mediator.Save(Guid.NewGuid(), ValidVacancyId, viewModel);

            response.AssertMessage(ApprenticeshipApplicationMediatorCodes.Save.Error, ApplicationPageMessages.SaveFailed, UserMessageLevel.Warning, true);
        }

        [Test]
        public void OkIsJavascript()
        {
            var viewModel = new ApprenticeshipApplicationViewModel
            {
                Candidate = new ApprenticeshipCandidateViewModel(),
                VacancyDetail = new ApprenticeshipVacancyDetailViewModel(),
                IsJavascript = true
            };
            ApprenticeshipApplicationProvider.Setup(p => p.GetApplicationViewModel(It.IsAny<Guid>(), ValidVacancyId)).Returns(new ApprenticeshipApplicationViewModel { Status = ApplicationStatuses.Draft, VacancyDetail = new ApprenticeshipVacancyDetailViewModel() });
            ApprenticeshipApplicationProvider.Setup(p => p.PatchApplicationViewModel(It.IsAny<Guid>(), It.IsAny<ApprenticeshipApplicationViewModel>(), It.IsAny<ApprenticeshipApplicationViewModel>())).Returns<Guid, ApprenticeshipApplicationViewModel, ApprenticeshipApplicationViewModel>((cid, svm, vm) => vm);
            ApprenticeshipApplicationProvider.Setup(p => p.SubmitApplication(It.IsAny<Guid>(), It.IsAny<int>())).Returns(viewModel);
            
            var response = Mediator.Save(Guid.NewGuid(), ValidVacancyId, viewModel);

            response.AssertCode(ApprenticeshipApplicationMediatorCodes.Save.Ok, true);
        }

        [Test]
        public void Ok()
        {
            var viewModel = new ApprenticeshipApplicationViewModel
            {
                Candidate = new ApprenticeshipCandidateViewModel(),
                VacancyDetail = new ApprenticeshipVacancyDetailViewModel()
            };
            ApprenticeshipApplicationProvider.Setup(p => p.GetApplicationViewModel(It.IsAny<Guid>(), ValidVacancyId)).Returns(new ApprenticeshipApplicationViewModel { Status = ApplicationStatuses.Draft, VacancyDetail = new ApprenticeshipVacancyDetailViewModel() });
            ApprenticeshipApplicationProvider.Setup(p => p.PatchApplicationViewModel(It.IsAny<Guid>(), It.IsAny<ApprenticeshipApplicationViewModel>(), It.IsAny<ApprenticeshipApplicationViewModel>())).Returns<Guid, ApprenticeshipApplicationViewModel, ApprenticeshipApplicationViewModel>((cid, svm, vm) => vm);
            ApprenticeshipApplicationProvider.Setup(p => p.SubmitApplication(It.IsAny<Guid>(), It.IsAny<int>())).Returns(viewModel);
            
            var response = Mediator.Save(Guid.NewGuid(), ValidVacancyId, viewModel);

            response.AssertCode(ApprenticeshipApplicationMediatorCodes.Save.Ok, true);
        }

        [Test]
        public void VacancyNotFound()
        {
            var viewModel = new ApprenticeshipApplicationViewModel
            {
                Candidate = new ApprenticeshipCandidateViewModel(),
                VacancyDetail = new ApprenticeshipVacancyDetailViewModel()
            };
            ApprenticeshipApplicationProvider.Setup(p => p.GetApplicationViewModel(It.IsAny<Guid>(), ValidVacancyId)).Returns(new ApprenticeshipApplicationViewModel { Status = ApplicationStatuses.ExpiredOrWithdrawn });
            
            var response = Mediator.Save(Guid.NewGuid(), ValidVacancyId, viewModel);

            response.AssertCodeAndMessage(ApprenticeshipApplicationMediatorCodes.Save.VacancyNotFound);
        }

        [Test]
        public void ValidationError()
        {
            var viewModel = new ApprenticeshipApplicationViewModel
            {
                Candidate = new ApprenticeshipCandidateViewModel
                {
                    Education = new EducationViewModel
                    {
                        FromYear = "1066"
                    }
                },
                VacancyDetail = new ApprenticeshipVacancyDetailViewModel()
            };
            ApprenticeshipApplicationProvider.Setup(p => p.GetApplicationViewModel(It.IsAny<Guid>(), ValidVacancyId))
                .Returns(new ApprenticeshipApplicationViewModel
                {
                    Status = ApplicationStatuses.Draft,
                    VacancyDetail = new ApprenticeshipVacancyDetailViewModel()
                });
            ApprenticeshipApplicationProvider.Setup(
                p =>
                    p.PatchApplicationViewModel(It.IsAny<Guid>(), It.IsAny<ApprenticeshipApplicationViewModel>(),
                        It.IsAny<ApprenticeshipApplicationViewModel>()))
                .Returns<Guid, ApprenticeshipApplicationViewModel, ApprenticeshipApplicationViewModel>(
                    (cid, svm, vm) => vm);

            var response = Mediator.Save(Guid.NewGuid(), ValidVacancyId, viewModel);

            response.AssertValidationResult(ApprenticeshipApplicationMediatorCodes.Save.ValidationError);
        }

        [Test]
        public void OfflineVacancy()
        {
            var viewModel = new ApprenticeshipApplicationViewModel
            {
                Candidate = new ApprenticeshipCandidateViewModel(),
                VacancyDetail = new ApprenticeshipVacancyDetailViewModel()
            };
            ApprenticeshipApplicationProvider.Setup(p => p.GetApplicationViewModel(It.IsAny<Guid>(), ValidVacancyId)).Returns(new ApprenticeshipApplicationViewModel { Status = ApplicationStatuses.Draft, VacancyDetail = new ApprenticeshipVacancyDetailViewModel { ApplyViaEmployerWebsite = true } });
            ApprenticeshipApplicationProvider.Setup(p => p.PatchApplicationViewModel(It.IsAny<Guid>(), It.IsAny<ApprenticeshipApplicationViewModel>(), It.IsAny<ApprenticeshipApplicationViewModel>())).Returns<Guid, ApprenticeshipApplicationViewModel, ApprenticeshipApplicationViewModel>((cid, svm, vm) => vm);
            ApprenticeshipApplicationProvider.Setup(p => p.SubmitApplication(It.IsAny<Guid>(), It.IsAny<int>())).Returns(viewModel);

            var response = Mediator.Save(Guid.NewGuid(), ValidVacancyId, viewModel);

            response.AssertCode(ApprenticeshipApplicationMediatorCodes.Save.OfflineVacancy, false);
        }
    }
}