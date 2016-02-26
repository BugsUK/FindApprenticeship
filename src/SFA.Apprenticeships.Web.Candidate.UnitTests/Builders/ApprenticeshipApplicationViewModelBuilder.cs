namespace SFA.Apprenticeships.Web.Candidate.UnitTests.Builders
{
    using System.Linq;
    using Common.Models.Application;
    using Common.ViewModels.Applications;
    using Common.ViewModels.Candidate;
    using Common.ViewModels.Locations;
    using Common.ViewModels.VacancySearch;
    using Domain.Entities.Applications;
    using Domain.Entities.Vacancies;

    public class ApprenticeshipApplicationViewModelBuilder
    {
        private string _viewModelMessage;
        private ApplicationViewModelStatus _viewModelStatus;

        private bool _isJavascript;

        private ApplicationStatuses _status = ApplicationStatuses.Draft;
        private VacancyStatuses _vacancyStatus = VacancyStatuses.Live;
        private bool _applyViaEmployerWebsite;

        private bool _requiresSupportForInterview;
        private string _anythingWeCanDoToSupportYourInterview;

        private WorkExperienceViewModel[] _workExperience;
        private TrainingCourseViewModel[] _trainingCourses;

        public ApprenticeshipApplicationViewModelBuilder RequiresSupportForInterview()
        {
            _requiresSupportForInterview = true;
            return this;
        }

        public ApprenticeshipApplicationViewModelBuilder DoesNotRequireSupportForInterview()
        {
            _requiresSupportForInterview = false;
            return this;
        }

        public ApprenticeshipApplicationViewModelBuilder CanBeSupportedAtInterviewBy(string anythingWeCanDoToSupportYourInterview)
        {
            _anythingWeCanDoToSupportYourInterview = anythingWeCanDoToSupportYourInterview;
            return this;
        }

        public ApprenticeshipApplicationViewModelBuilder HasError(ApplicationViewModelStatus viewModelStatus, string viewModelMessage)
        {
            _viewModelStatus = viewModelStatus;
            _viewModelMessage = viewModelMessage;
            return this;
        }

        public ApprenticeshipApplicationViewModelBuilder HasError(ApplicationStatuses status, string viewModelMessage)
        {
            _status = status;
            _viewModelMessage = viewModelMessage;
            return this;
        }

        public ApprenticeshipApplicationViewModelBuilder HasError(string viewModelMessage)
        {
            _viewModelMessage = viewModelMessage;
            return this;
        }

        public ApprenticeshipApplicationViewModelBuilder ApplyViaEmployerWebsite(bool applyViaEmployerWebsite)
        {
            _applyViaEmployerWebsite = applyViaEmployerWebsite;
            return this;
        }

        public ApprenticeshipApplicationViewModelBuilder WithVacancyStatus(VacancyStatuses vacancyStatus)
        {
            _vacancyStatus = vacancyStatus;
            return this;
        }

        public ApprenticeshipApplicationViewModelBuilder WithStatus(ApplicationStatuses status)
        {
            _status = status;
            return this;
        }

        public ApprenticeshipApplicationViewModelBuilder WithWorkExperience(WorkExperienceViewModel[] workExperience)
        {
            _workExperience = workExperience;
            return this;
        }

        public ApprenticeshipApplicationViewModelBuilder WithTrainingCourses(TrainingCourseViewModel[] trainingCourses)
        {
            _trainingCourses = trainingCourses;
            return this;
        }

        public ApprenticeshipApplicationViewModelBuilder IsJavascript(bool isJavascript)
        {
            _isJavascript = isJavascript;
            return this;
        }

        public ApprenticeshipApplicationViewModel Build()
        {
            var viewModel = new ApprenticeshipApplicationViewModel
            {
                Status = _status,
                ViewModelMessage = _viewModelMessage,
                ViewModelStatus = _viewModelStatus,

                IsJavascript = _isJavascript,

                Candidate = new ApprenticeshipCandidateViewModel
                {
                    Education = new EducationViewModel(),
                    AboutYou = new AboutYouViewModel(),
                    Address = new AddressViewModel
                    {
                        GeoPoint = new GeoPointViewModel()
                    },
                    MonitoringInformation = new MonitoringInformationViewModel
                    {
                        RequiresSupportForInterview = _requiresSupportForInterview,
                        AnythingWeCanDoToSupportYourInterview = _anythingWeCanDoToSupportYourInterview
                    },
                    HasWorkExperience = _workExperience != null && _workExperience.Any(),
                    WorkExperience = _workExperience,
                    HasTrainingCourses = _trainingCourses != null && _trainingCourses.Any(),
                    TrainingCourses = _trainingCourses
                },
                VacancyDetail = new ApprenticeshipVacancyDetailViewModel
                {
                    VacancyAddress = new AddressViewModel
                    {
                        GeoPoint = new GeoPointViewModel()
                    },
                    ApplyViaEmployerWebsite = _applyViaEmployerWebsite,
                    VacancyStatus = _vacancyStatus
                }
            };

            return viewModel;
        }
    }
}