using SFA.Apprenticeships.Web.Common.UnitTests.Mediators;

namespace SFA.Apprenticeships.Web.Candidate.UnitTests.Mediators.ApprenticeshipApplication
{
    using System;
    using Candidate.Mediators.Application;
    using Candidate.ViewModels.Applications;
    using Common.Constants;
    using Common.Constants.Pages;
    using Common.Models.Application;
    using Common.ViewModels.Applications;
    using Common.ViewModels.Candidate;
    using Common.ViewModels.VacancySearch;
    using Constants.Pages;
    using Domain.Entities.Applications;
    using Domain.Entities.Vacancies;
    using Moq;
    using NUnit.Framework;

    [TestFixture]
    public class SubmitTests : TestsBase
    {
        private const int ValidVacancyId = 1;

        [Test]
        public void VacancyNotFound()
        {
            var postedViewModel = new ApprenticeshipApplicationPreviewViewModel
            {
                AcceptSubmit = true
            };

            var viewModel = new ApprenticeshipApplicationPreviewViewModel
            {
                Candidate = new ApprenticeshipCandidateViewModel(),
                VacancyDetail = new ApprenticeshipVacancyDetailViewModel(),
                Status = ApplicationStatuses.ExpiredOrWithdrawn
            };

            ApprenticeshipApplicationProvider.Setup(p => p.GetApplicationPreviewViewModel(It.IsAny<Guid>(), It.IsAny<int>())).Returns(viewModel);
            ApprenticeshipApplicationProvider.Setup(p => p.SubmitApplication(It.IsAny<Guid>(), It.IsAny<int>())).Returns(viewModel);

            var response = Mediator.Submit(Guid.NewGuid(), ValidVacancyId, postedViewModel);

            response.AssertCode(ApprenticeshipApplicationMediatorCodes.Submit.VacancyNotFound, false);
        }
        
        [Test]
        public void VacancyNotFound_GatewayError()
        {
            var postedViewModel = new ApprenticeshipApplicationPreviewViewModel
            {
                AcceptSubmit = true
            };

            var savedViewModel = new ApprenticeshipApplicationPreviewViewModel
            {
                Candidate = new ApprenticeshipCandidateViewModel(),
                VacancyDetail = new ApprenticeshipVacancyDetailViewModel
                {
                    VacancyStatus = VacancyStatuses.Live
                }
            };

            var submittedViewModel = new ApprenticeshipApplicationViewModel(ApprenticeshipVacancyDetailPageMessages.GetVacancyDetailFailed)
            {
                Candidate = new ApprenticeshipCandidateViewModel(),
                VacancyDetail = new ApprenticeshipVacancyDetailViewModel(ApprenticeshipVacancyDetailPageMessages.GetVacancyDetailFailed)
            };

            ApprenticeshipApplicationProvider.Setup(p => p.GetApplicationPreviewViewModel(It.IsAny<Guid>(), It.IsAny<int>())).Returns(savedViewModel);
            ApprenticeshipApplicationProvider.Setup(p => p.SubmitApplication(It.IsAny<Guid>(), It.IsAny<int>())).Returns(submittedViewModel);
            
            var response = Mediator.Submit(Guid.NewGuid(), ValidVacancyId, postedViewModel);

            response.AssertMessage(ApprenticeshipApplicationMediatorCodes.Submit.Error, ApplicationPageMessages.SubmitApplicationFailed, UserMessageLevel.Warning, false, true);
        }

        [Test]
        public void IncorrectState()
        {
            var postedViewModel = new ApprenticeshipApplicationPreviewViewModel
            {
                AcceptSubmit = true
            };

            var viewModel = new ApprenticeshipApplicationPreviewViewModel
            {
                Candidate = new ApprenticeshipCandidateViewModel(),
                VacancyDetail = new ApprenticeshipVacancyDetailViewModel
                {
                    VacancyStatus = VacancyStatuses.Live
                },
                ViewModelStatus = ApplicationViewModelStatus.ApplicationInIncorrectState
            };

            ApprenticeshipApplicationProvider.Setup(p => p.GetApplicationPreviewViewModel(It.IsAny<Guid>(), It.IsAny<int>())).Returns(viewModel);
            ApprenticeshipApplicationProvider.Setup(p => p.SubmitApplication(It.IsAny<Guid>(), It.IsAny<int>())).Returns(viewModel);
            
            var response = Mediator.Submit(Guid.NewGuid(), ValidVacancyId, postedViewModel);

            response.AssertMessage(ApprenticeshipApplicationMediatorCodes.Submit.IncorrectState, MyApplicationsPageMessages.ApplicationInIncorrectState, UserMessageLevel.Info, false);
        }

        [Test]
        public void GetApplicationViewModelError()
        {
            var postedViewModel = new ApprenticeshipApplicationPreviewViewModel
            {
                AcceptSubmit = true
            };

            var savedViewModel = new ApprenticeshipApplicationPreviewViewModel
            {
                Candidate = new ApprenticeshipCandidateViewModel(),
                VacancyDetail = new ApprenticeshipVacancyDetailViewModel
                {
                    VacancyStatus = VacancyStatuses.Live
                }
            };
            var submittedApplicationViewModel = new ApprenticeshipApplicationViewModel
            {
                Candidate = new ApprenticeshipCandidateViewModel(),
                VacancyDetail = new ApprenticeshipVacancyDetailViewModel(),
                ViewModelStatus = ApplicationViewModelStatus.Error,
                ViewModelMessage = "An error message"
            };
            ApprenticeshipApplicationProvider.Setup(p => p.GetApplicationPreviewViewModel(It.IsAny<Guid>(), It.IsAny<int>())).Returns(savedViewModel);
            ApprenticeshipApplicationProvider.Setup(p => p.SubmitApplication(It.IsAny<Guid>(), It.IsAny<int>())).Returns(submittedApplicationViewModel);
            
            var response = Mediator.Submit(Guid.NewGuid(), ValidVacancyId, postedViewModel);

            response.AssertMessage(ApprenticeshipApplicationMediatorCodes.Submit.Error, ApplicationPageMessages.SubmitApplicationFailed, UserMessageLevel.Warning, false, true);
        }

        [Test]
        public void SubmitApplicationError()
        {
            var postedViewModel = new ApprenticeshipApplicationPreviewViewModel
            {
                AcceptSubmit = true
            };

            var viewModel = new ApprenticeshipApplicationPreviewViewModel
            {
                Candidate = new ApprenticeshipCandidateViewModel(),
                VacancyDetail = new ApprenticeshipVacancyDetailViewModel(),
                ViewModelStatus = ApplicationViewModelStatus.Error,
                ViewModelMessage = "An error message"
            };

            ApprenticeshipApplicationProvider.Setup(p => p.GetApplicationPreviewViewModel(It.IsAny<Guid>(), It.IsAny<int>())).Returns(viewModel);
            ApprenticeshipApplicationProvider.Setup(p => p.SubmitApplication(It.IsAny<Guid>(), It.IsAny<int>())).Returns(viewModel);
            
            var response = Mediator.Submit(Guid.NewGuid(), ValidVacancyId, postedViewModel);

            response.AssertMessage(ApprenticeshipApplicationMediatorCodes.Submit.Error, ApplicationPageMessages.SubmitApplicationFailed, UserMessageLevel.Warning, false, true);
        }

        [Test]
        public void ValidationError()
        {
            var postedViewModel = new ApprenticeshipApplicationPreviewViewModel
            {
                AcceptSubmit = true
            };

            var viewModel = new ApprenticeshipApplicationPreviewViewModel
            {
                Candidate = new ApprenticeshipCandidateViewModel
                {
                    Education = new EducationViewModel
                    {
                        NameOfMostRecentSchoolCollege = "A School",
                        FromYear = "0",
                        ToYear = "0"
                    }
                },
                VacancyDetail = new ApprenticeshipVacancyDetailViewModel
                {
                    VacancyStatus = VacancyStatuses.Live
                },
                ViewModelStatus = ApplicationViewModelStatus.Error
            };

            ApprenticeshipApplicationProvider.Setup(p => p.GetApplicationPreviewViewModel(It.IsAny<Guid>(), It.IsAny<int>())).Returns(viewModel);
            ApprenticeshipApplicationProvider.Setup(p => p.SubmitApplication(It.IsAny<Guid>(), It.IsAny<int>())).Returns(viewModel);
            
            var response = Mediator.Submit(Guid.NewGuid(), ValidVacancyId, postedViewModel);

            response.AssertValidationResult(ApprenticeshipApplicationMediatorCodes.Submit.ValidationError);
        }

        [Test]
        public void Ok()
        {
            var postedViewModel = new ApprenticeshipApplicationPreviewViewModel
            {
                AcceptSubmit = true
            };

            var viewModel = new ApprenticeshipApplicationPreviewViewModel
            {
                Candidate = new ApprenticeshipCandidateViewModel(),
                VacancyDetail = new ApprenticeshipVacancyDetailViewModel
                {
                    VacancyStatus = VacancyStatuses.Live
                }
            };

            ApprenticeshipApplicationProvider.Setup(p => p.GetApplicationPreviewViewModel(It.IsAny<Guid>(), It.IsAny<int>())).Returns(viewModel);
            ApprenticeshipApplicationProvider.Setup(p => p.CreateApplicationViewModel(It.IsAny<Guid>(), ValidVacancyId)).Returns(new ApprenticeshipApplicationViewModel());
            ApprenticeshipApplicationProvider.Setup(p => p.PatchApplicationViewModel(It.IsAny<Guid>(), It.IsAny<ApprenticeshipApplicationViewModel>(), It.IsAny<ApprenticeshipApplicationViewModel>())).Returns<Guid, ApprenticeshipApplicationViewModel, ApprenticeshipApplicationViewModel>((cid, svm, vm) => vm);
            ApprenticeshipApplicationProvider.Setup(p => p.SubmitApplication(It.IsAny<Guid>(), It.IsAny<int>())).Returns(viewModel);
            
            var response = Mediator.Submit(Guid.NewGuid(), ValidVacancyId, postedViewModel);

            response.AssertCode(ApprenticeshipApplicationMediatorCodes.Submit.Ok, false, true);
        }

        [Test]
        public void VacancyExpired()
        {
            var postedViewModel = new ApprenticeshipApplicationPreviewViewModel
            {
                AcceptSubmit = true
            };

            var viewModel = new ApprenticeshipApplicationPreviewViewModel
            {
                Candidate = new ApprenticeshipCandidateViewModel(),
                VacancyDetail = new ApprenticeshipVacancyDetailViewModel()
            };

            ApprenticeshipApplicationProvider.Setup(p => p.GetApplicationPreviewViewModel(It.IsAny<Guid>(), It.IsAny<int>())).Returns(viewModel);

            ApprenticeshipApplicationProvider.Setup(p => p.CreateApplicationViewModel(It.IsAny<Guid>(), ValidVacancyId)).Returns(new ApprenticeshipApplicationViewModel
            {
                VacancyDetail = new ApprenticeshipVacancyDetailViewModel
                {
                    VacancyStatus = VacancyStatuses.Expired
                }
            });

            ApprenticeshipApplicationProvider.Setup(p => p.PatchApplicationViewModel(It.IsAny<Guid>(), It.IsAny<ApprenticeshipApplicationViewModel>(), It.IsAny<ApprenticeshipApplicationViewModel>())).Returns<Guid, ApprenticeshipApplicationViewModel, ApprenticeshipApplicationViewModel>((cid, svm, vm) => vm);
            ApprenticeshipApplicationProvider.Setup(p => p.SubmitApplication(It.IsAny<Guid>(), It.IsAny<int>())).Returns(viewModel);

            var response = Mediator.Submit(Guid.NewGuid(), ValidVacancyId, postedViewModel);

            response.AssertCode(ApprenticeshipApplicationMediatorCodes.Submit.VacancyNotFound, false);
        }

        [Test]
        public void ErrorGettingApplicationViewModel()
        {
            var postedViewModel = new ApprenticeshipApplicationPreviewViewModel
            {
                AcceptSubmit = true
            };

            var viewModel = new ApprenticeshipApplicationPreviewViewModel
            {
                Candidate = new ApprenticeshipCandidateViewModel(),
                VacancyDetail = new ApprenticeshipVacancyDetailViewModel()
            };

            ApprenticeshipApplicationProvider.Setup(p => p.GetApplicationPreviewViewModel(It.IsAny<Guid>(), It.IsAny<int>())).Returns(viewModel);
            ApprenticeshipApplicationProvider.Setup(p => p.CreateApplicationViewModel(It.IsAny<Guid>(), ValidVacancyId)).Returns(new ApprenticeshipApplicationViewModel(MyApplicationsPageMessages.ApplicationNotFound, ApplicationViewModelStatus.ApplicationNotFound));
            ApprenticeshipApplicationProvider.Setup(p => p.PatchApplicationViewModel(It.IsAny<Guid>(), It.IsAny<ApprenticeshipApplicationViewModel>(), It.IsAny<ApprenticeshipApplicationViewModel>())).Returns<Guid, ApprenticeshipApplicationViewModel, ApprenticeshipApplicationViewModel>((cid, svm, vm) => vm);
            ApprenticeshipApplicationProvider.Setup(p => p.SubmitApplication(It.IsAny<Guid>(), It.IsAny<int>())).Returns(viewModel);

            var response = Mediator.Submit(Guid.NewGuid(), ValidVacancyId, postedViewModel);

            response.AssertCode(ApprenticeshipApplicationMediatorCodes.Submit.VacancyNotFound, false);
        }

        [Test]
        public void AcceptSubmitValidationError()
        {
            var postedViewModel = new ApprenticeshipApplicationPreviewViewModel
            {
                AcceptSubmit = false
            };

            var viewModel = new ApprenticeshipApplicationPreviewViewModel
            {
                Candidate = new ApprenticeshipCandidateViewModel(),
                VacancyDetail = new ApprenticeshipVacancyDetailViewModel
                {
                    VacancyStatus = VacancyStatuses.Live
                },
                Status = ApplicationStatuses.Draft
            };

            ApprenticeshipApplicationProvider.Setup(p => p.GetApplicationPreviewViewModel(It.IsAny<Guid>(), It.IsAny<int>())).Returns(viewModel);
            ApprenticeshipApplicationProvider.Setup(p => p.SubmitApplication(It.IsAny<Guid>(), It.IsAny<int>())).Returns(viewModel);

            var response = Mediator.Submit(Guid.NewGuid(), ValidVacancyId, postedViewModel);

            response.AssertValidationResult(ApprenticeshipApplicationMediatorCodes.Submit.AcceptSubmitValidationError);
        }
    }
}