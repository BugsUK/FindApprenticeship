namespace SFA.Apprenticeships.Web.Recruit.Providers
{
    using System;
    using System.Linq;
    using Application.Interfaces.Applications;
    using Application.Interfaces.Logging;
    using Application.Interfaces.VacancyPosting;
    using Common.ViewModels;
    using Domain.Entities.Applications;
    using Domain.Entities.Vacancies.ProviderVacancies.Apprenticeship;
    using Domain.Interfaces.Configuration;
    using Domain.Interfaces.Mapping;
    using ViewModels.Application;
    using ViewModels.Application.Apprenticeship;

    public class ApplicationProvider : IApplicationProvider
    {
        private readonly IConfigurationService _configurationService;
        private readonly IVacancyPostingService _vacancyPostingService;
        private readonly IApprenticeshipApplicationService _apprenticeshipApplicationService;
        private readonly IMapper _mapper;
        private readonly ILogService _logService;

        public ApplicationProvider(IConfigurationService configurationService, IVacancyPostingService vacancyPostingService, IApprenticeshipApplicationService apprenticeshipApplicationService, IMapper mapper, ILogService logService)
        {
            _configurationService = configurationService;
            _vacancyPostingService = vacancyPostingService;
            _apprenticeshipApplicationService = apprenticeshipApplicationService;
            _mapper = mapper;
            _logService = logService;
        }

        public VacancyApplicationsViewModel GetVacancyApplicationsViewModel(long vacancyReferenceNumber)
        {
            var vacancy = _vacancyPostingService.GetVacancy(vacancyReferenceNumber);
            var viewModel = _mapper.Map<ApprenticeshipVacancy, VacancyApplicationsViewModel>(vacancy);

            var applications = _apprenticeshipApplicationService.GetSubmittedApplicationSummaries((int) vacancyReferenceNumber);
            var applicationSummaryViewModels = applications.Select(a => _mapper.Map<ApprenticeshipApplicationSummary, ApplicationSummaryViewModel>(a)).ToList();

            //TODO: return as part of data query - probably needs migration
            viewModel.RejectedApplicationsCount = applicationSummaryViewModels.Count(vm => vm.Status == ApplicationStatuses.Unsuccessful);
            viewModel.UnresolvedApplicationsCount = applicationSummaryViewModels.Count(vm => vm.Status <= ApplicationStatuses.InProgress);

            viewModel.ApplicationSummaries = new PageableViewModel<ApplicationSummaryViewModel>
            {
                Page = applicationSummaryViewModels
            };

            return viewModel;
        }

        public ApprenticeshipApplicationViewModel GetApprenticeshipApplicationViewModel(Guid applicationId)
        {
            var application = _apprenticeshipApplicationService.GetApplication(applicationId);
            var viewModel = ConvertToApprenticeshipApplicationViewModel(application);
            return viewModel;
        }

        public ApprenticeshipApplicationViewModel GetApprenticeshipApplicationViewModelForReview(Guid applicationId)
        {
            var application = _apprenticeshipApplicationService.GetApplicationForReview(applicationId);
            var viewModel = ConvertToApprenticeshipApplicationViewModel(application);
            return viewModel;
        }

        public void UpdateApprenticeshipApplicationViewModelNotes(Guid applicationId, string notes)
        {
            _apprenticeshipApplicationService.UpdateApplicationNotes(applicationId, notes);
        }

        private ApprenticeshipApplicationViewModel ConvertToApprenticeshipApplicationViewModel(ApprenticeshipApplicationDetail application)
        {
            var vacancy = _vacancyPostingService.GetVacancy(application.Vacancy.Id);
            var viewModel = _mapper.Map<ApprenticeshipApplicationDetail, ApprenticeshipApplicationViewModel>(application);
            viewModel.Vacancy = _mapper.Map<ApprenticeshipVacancy, ApplicationVacancyViewModel>(vacancy);
            return viewModel;
        }
    }
}