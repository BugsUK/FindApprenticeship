namespace SFA.Apprenticeships.Web.Candidate.UnitTests.Builders
{
    using Candidate.ViewModels.Applications;
    using Candidate.ViewModels.Candidate;
    using Candidate.ViewModels.Locations;
    using Candidate.ViewModels.VacancySearch;
    using Common.Models.Application;
    using Domain.Entities.Applications;
    using Domain.Entities.Vacancies;

    public class ApprenticeshipApplicationViewModelBuilder
    {
        private bool _requiresSupportForInterview;
        private string _anythingWeCanDoToSupportYourInterview;
        private ApplicationStatuses _status = ApplicationStatuses.Draft;
        private string _viewModelMessage;
        private bool _applyViaEmployerWebsite;
        private VacancyStatuses _vacancyStatus = VacancyStatuses.Live;
        private ApplicationViewModelStatus _viewModelStatus;

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

        public ApprenticeshipApplicationViewModel Build()
        {
            var viewModel = new ApprenticeshipApplicationViewModel
            {
                Status = _status,
                ViewModelMessage = _viewModelMessage,
                ViewModelStatus = _viewModelStatus,
                Candidate = new ApprenticeshipCandidateViewModel
                {
                    Education = new EducationViewModel(),
                    AboutYou = new AboutYouViewModel
                    {
                        RequiresSupportForInterview = _requiresSupportForInterview,
                        AnythingWeCanDoToSupportYourInterview = _anythingWeCanDoToSupportYourInterview
                    },
                    Address = new AddressViewModel
                    {
                        GeoPoint = new GeoPointViewModel()
                    }
                },
                VacancyDetail = new VacancyDetailViewModel
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