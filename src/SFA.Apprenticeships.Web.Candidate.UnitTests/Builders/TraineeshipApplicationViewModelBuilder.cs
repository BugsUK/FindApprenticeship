namespace SFA.Apprenticeships.Web.Candidate.UnitTests.Builders
{
    using System.Collections.Generic;
    using System.Linq;
    using Common.Models.Application;
    using Common.ViewModels.Applications;
    using Common.ViewModels.Candidate;
    using Common.ViewModels.VacancySearch;
    using Domain.Entities.Vacancies;

    public class TraineeshipApplicationViewModelBuilder
    {
        private string _viewModelMessage;
        private ApplicationViewModelStatus _viewModelStatus;

        private bool _isJavascript;

        private IEnumerable<QualificationsViewModel> _qualifications;
        private IEnumerable<WorkExperienceViewModel> _workExperience;
        private IEnumerable<TrainingCourseViewModel> _trainingCourses;
        private VacancyStatuses _vacancyStatus;
        private MonitoringInformationViewModel _monitoringInformationViewModel;

        public TraineeshipApplicationViewModelBuilder WithMessage(string message)
        {
            _viewModelMessage = message;
            return this;
        }

        public TraineeshipApplicationViewModelBuilder WithQualifications(IEnumerable<QualificationsViewModel> qualifications)
        {
            _qualifications = qualifications;
            return this;
        }

        public TraineeshipApplicationViewModelBuilder WithWorkExperience(IEnumerable<WorkExperienceViewModel> workExperience)
        {
            _workExperience = workExperience;
            return this;
        }

        public TraineeshipApplicationViewModelBuilder WithTrainingCourses(List<TrainingCourseViewModel> trainingCourses)
        {
            _trainingCourses = trainingCourses;
            return this;
        }
 
        public TraineeshipApplicationViewModelBuilder HasError(ApplicationViewModelStatus viewModelStatus, string viewModelMessage)
        {
            _viewModelStatus = viewModelStatus;
            _viewModelMessage = viewModelMessage;
            return this;
        }

        public TraineeshipApplicationViewModelBuilder WithVacancyStatus(VacancyStatuses vacancyStatus)
        {
            _vacancyStatus = vacancyStatus;
            return this;
        }
        
        public TraineeshipApplicationViewModelBuilder WithMonitoringInformation(MonitoringInformationViewModel monitoringInformationViewModel)
        {
            _monitoringInformationViewModel = monitoringInformationViewModel;
            return this;
        }

        public TraineeshipApplicationViewModelBuilder IsJavascript(bool isJavascript)
        {
            _isJavascript = isJavascript;
            return this;
        }

        public TraineeshipApplicationViewModel Build()
        {
            return  new TraineeshipApplicationViewModel
            {
                ViewModelMessage = _viewModelMessage,
                ViewModelStatus = _viewModelStatus,

                IsJavascript = _isJavascript,

                Candidate = new TraineeshipCandidateViewModel
                {
                    HasQualifications = _qualifications != null,
                    Qualifications = _qualifications,
                    HasWorkExperience = _workExperience != null && _workExperience.Any(),
                    WorkExperience = _workExperience,
                    HasTrainingCourses = _trainingCourses != null && _trainingCourses.Any(),
                    TrainingCourses = _trainingCourses,
                    MonitoringInformation = _monitoringInformationViewModel
                },
                VacancyDetail = new TraineeshipVacancyDetailViewModel
                {
                    VacancyStatus = _vacancyStatus
                }
            };
        }
    }
}