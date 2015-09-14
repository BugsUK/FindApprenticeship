using SFA.Apprenticeships.Web.Common.UnitTests.Mediators;

namespace SFA.Apprenticeships.Web.Candidate.UnitTests.Mediators.TraineeshipApplication
{
    using System;
    using System.Collections.Generic;
    using Builders;
    using Candidate.Mediators.Application;
    using Candidate.ViewModels.Applications;
    using Candidate.ViewModels.Candidate;
    using Candidate.ViewModels.VacancySearch;
    using Common.Constants;
    using Common.Models.Application;
    using Constants.Pages;
    using Moq;
    using NUnit.Framework;

    [TestFixture]
    public class SubmitTests : TestsBase
    {
        private const int ValidVacancyId = 1;

        [Test]
        public void IncorrectState()
        {
            var viewModel = new TraineeshipApplicationViewModel
            {
                Candidate = new TraineeshipCandidateViewModel(),
                VacancyDetail = new TraineeshipVacancyDetailViewModel(),
                ViewModelStatus = ApplicationViewModelStatus.ApplicationInIncorrectState
            };
            TraineeshipApplicationProvider.Setup(p => p.GetApplicationViewModel(It.IsAny<Guid>(), ValidVacancyId)).Returns(new TraineeshipApplicationViewModel { ViewModelStatus = ApplicationViewModelStatus.ApplicationInIncorrectState });
            TraineeshipApplicationProvider.Setup(p => p.PatchApplicationViewModel(It.IsAny<Guid>(), It.IsAny<TraineeshipApplicationViewModel>(), It.IsAny<TraineeshipApplicationViewModel>())).Returns<Guid, TraineeshipApplicationViewModel, TraineeshipApplicationViewModel>((cid, svm, vm) => vm);
            TraineeshipApplicationProvider.Setup(p => p.SubmitApplication(It.IsAny<Guid>(), It.IsAny<int>(), It.IsAny<TraineeshipApplicationViewModel>())).Returns<Guid, int, TraineeshipApplicationViewModel>((cid, vid, vm) => vm);
            
            var response = Mediator.Submit(Guid.NewGuid(), ValidVacancyId, viewModel);

            response.AssertCode(TraineeshipApplicationMediatorCodes.Submit.IncorrectState, false);
        }

        [Test]
        public void GetApplicationViewModelError()
        {
            var getApplicationViewModel = new TraineeshipApplicationViewModelBuilder().WithMessage(ApplicationPageMessages.SubmitApplicationFailed).Build();
            TraineeshipApplicationProvider.Setup(p => p.GetApplicationViewModel(It.IsAny<Guid>(), ValidVacancyId)).Returns(getApplicationViewModel);

            var viewModel = new TraineeshipApplicationViewModelBuilder().Build();
            var response = Mediator.Submit(Guid.NewGuid(), ValidVacancyId, viewModel);

            response.AssertMessage(TraineeshipApplicationMediatorCodes.Submit.Error, ApplicationPageMessages.SubmitApplicationFailed, UserMessageLevel.Warning, true, true);
        }

        [Test]
        public void SubmitApplicationError()
        {
            var viewModel = new TraineeshipApplicationViewModel
            {
                Candidate = new TraineeshipCandidateViewModel(),
                VacancyDetail = new TraineeshipVacancyDetailViewModel(),
                ViewModelStatus = ApplicationViewModelStatus.Error
            };
            TraineeshipApplicationProvider.Setup(p => p.GetApplicationViewModel(It.IsAny<Guid>(), ValidVacancyId)).Returns(new TraineeshipApplicationViewModel { ViewModelStatus = ApplicationViewModelStatus.Error });
            TraineeshipApplicationProvider.Setup(p => p.PatchApplicationViewModel(It.IsAny<Guid>(), It.IsAny<TraineeshipApplicationViewModel>(), It.IsAny<TraineeshipApplicationViewModel>())).Returns<Guid, TraineeshipApplicationViewModel, TraineeshipApplicationViewModel>((cid, svm, vm) => vm);
            TraineeshipApplicationProvider.Setup(p => p.SubmitApplication(It.IsAny<Guid>(), It.IsAny<int>(), It.IsAny<TraineeshipApplicationViewModel>())).Returns<Guid, int, TraineeshipApplicationViewModel>((cid, vid, vm) => vm);
            
            var response = Mediator.Submit(Guid.NewGuid(), ValidVacancyId, viewModel);

            response.AssertMessage(TraineeshipApplicationMediatorCodes.Submit.Error, ApplicationPageMessages.SubmitApplicationFailed, UserMessageLevel.Warning, true, true);
        }

        [Test]
        public void Ok()
        {
            var viewModel = new TraineeshipApplicationViewModel
            {
                Candidate = new TraineeshipCandidateViewModel(),
                VacancyDetail = new TraineeshipVacancyDetailViewModel()
            };
            TraineeshipApplicationProvider.Setup(p => p.GetApplicationViewModel(It.IsAny<Guid>(), ValidVacancyId)).Returns(new TraineeshipApplicationViewModel());
            TraineeshipApplicationProvider.Setup(p => p.PatchApplicationViewModel(It.IsAny<Guid>(), It.IsAny<TraineeshipApplicationViewModel>(), It.IsAny<TraineeshipApplicationViewModel>())).Returns<Guid, TraineeshipApplicationViewModel, TraineeshipApplicationViewModel>((cid, svm, vm) => vm);
            TraineeshipApplicationProvider.Setup(p => p.SubmitApplication(It.IsAny<Guid>(), It.IsAny<int>(), It.IsAny<TraineeshipApplicationViewModel>())).Returns<Guid, int, TraineeshipApplicationViewModel>((cid, vid, vm) => vm);
            
            var response = Mediator.Submit(Guid.NewGuid(), ValidVacancyId, viewModel);

            response.AssertCode(TraineeshipApplicationMediatorCodes.Submit.Ok, false, true);
        }

        [Test]
        public void OkIsJavascript()
        {
            var viewModel = new TraineeshipApplicationViewModel
            {
                Candidate = new TraineeshipCandidateViewModel(),
                VacancyDetail = new TraineeshipVacancyDetailViewModel(),
                IsJavascript = true
            };
            TraineeshipApplicationProvider.Setup(p => p.GetApplicationViewModel(It.IsAny<Guid>(), ValidVacancyId)).Returns(new TraineeshipApplicationViewModel());
            TraineeshipApplicationProvider.Setup(p => p.PatchApplicationViewModel(It.IsAny<Guid>(), It.IsAny<TraineeshipApplicationViewModel>(), It.IsAny<TraineeshipApplicationViewModel>())).Returns<Guid, TraineeshipApplicationViewModel, TraineeshipApplicationViewModel>((cid, svm, vm) => vm);
            TraineeshipApplicationProvider.Setup(p => p.SubmitApplication(It.IsAny<Guid>(), It.IsAny<int>(), It.IsAny<TraineeshipApplicationViewModel>())).Returns<Guid, int, TraineeshipApplicationViewModel>((cid, vid, vm) => vm);
            
            var response = Mediator.Submit(Guid.NewGuid(), ValidVacancyId, viewModel);

            response.AssertCode(TraineeshipApplicationMediatorCodes.Submit.Ok, false, true);
        }

        [Test]
        public void FailValidation()
        {
            var viewModel = new TraineeshipApplicationViewModel
            {
                Candidate = new TraineeshipCandidateViewModel
                {
                    MonitoringInformation = new MonitoringInformationViewModel
                    {
                        AnythingWeCanDoToSupportYourInterview = new string('X', 9999)
                    }
                }
            };
            TraineeshipApplicationProvider.Setup(p => p.GetApplicationViewModel(It.IsAny<Guid>(), ValidVacancyId)).Returns(viewModel);
            TraineeshipApplicationProvider.Setup(p => p.PatchApplicationViewModel(It.IsAny<Guid>(), It.IsAny<TraineeshipApplicationViewModel>(), It.IsAny<TraineeshipApplicationViewModel>())).Returns<Guid, TraineeshipApplicationViewModel, TraineeshipApplicationViewModel>((cid, svm, vm) => vm);

            var response = Mediator.Submit(Guid.NewGuid(), ValidVacancyId, viewModel);

            response.AssertValidationResult(TraineeshipApplicationMediatorCodes.Submit.ValidationError, true, false);
        }

        [Test]
        public void FailValidationEducationLongerThan15Char()
        {
            var viewModel = new TraineeshipApplicationViewModel
            {
                Candidate = new TraineeshipCandidateViewModel
                {
                    MonitoringInformation = new MonitoringInformationViewModel(),
                    HasQualifications = true,
                    Qualifications = new List<QualificationsViewModel>()
                    {
                        new QualificationsViewModel()
                        {
                            Grade = "Grade is Longer than 15 chars",
                            QualificationType = "QUAL",
                            Year = "2012"
                        }
                    }
                }
            };
            TraineeshipApplicationProvider.Setup(p => p.GetApplicationViewModel(It.IsAny<Guid>(), ValidVacancyId)).Returns(viewModel);
            TraineeshipApplicationProvider.Setup(p => p.PatchApplicationViewModel(It.IsAny<Guid>(), It.IsAny<TraineeshipApplicationViewModel>(), It.IsAny<TraineeshipApplicationViewModel>())).Returns<Guid, TraineeshipApplicationViewModel, TraineeshipApplicationViewModel>((cid, svm, vm) => vm);

            var response = Mediator.Submit(Guid.NewGuid(), ValidVacancyId, viewModel);

            response.AssertValidationResult(TraineeshipApplicationMediatorCodes.Submit.ValidationError, true, false);
        }
    }
}