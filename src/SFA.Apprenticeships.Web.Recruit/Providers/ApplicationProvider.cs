namespace SFA.Apprenticeships.Web.Recruit.Providers
{
    using System;
    using System.Linq;
    using SFA.Infrastructure.Interfaces;
    using Application.Interfaces.Applications;
    using Application.Interfaces.VacancyPosting;
    using Common.ViewModels;
    using Domain.Entities.Applications;
    using Domain.Entities.Raa.Vacancies;
    using Raa.Common.Factories;
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

        public VacancyApplicationsViewModel GetVacancyApplicationsViewModel(VacancyApplicationsSearchViewModel vacancyApplicationsSearch)
        {
            var vacancy = _vacancyPostingService.GetVacancy(vacancyApplicationsSearch.VacancyReferenceNumber);
            var viewModel = _mapper.Map<Vacancy, VacancyApplicationsViewModel>(vacancy);

            var applications = _apprenticeshipApplicationService.GetSubmittedApplicationSummaries((int)vacancyApplicationsSearch.VacancyReferenceNumber);

            var @new = applications.Where(v => v.Status == ApplicationStatuses.Submitted).ToList();
            var viewed = applications.Where(v => v.Status == ApplicationStatuses.InProgress).ToList();
            var successful = applications.Where(v => v.Status == ApplicationStatuses.Successful).ToList();
            var unsuccessful = applications.Where(v => v.Status == ApplicationStatuses.Unsuccessful).ToList();

            switch (vacancyApplicationsSearch.FilterType)
            {
                case VacancyApplicationsFilterTypes.New:
                    applications = @new;
                    break;
                case VacancyApplicationsFilterTypes.Viewed:
                    applications = viewed;
                    break;
                case VacancyApplicationsFilterTypes.Successful:
                    applications = successful;
                    break;
                case VacancyApplicationsFilterTypes.Unsuccessful:
                    applications = unsuccessful;
                    break;
            }

            //TODO: return as part of data query - probably needs migration
            viewModel.NewApplicationsCount = @new.Count;
            viewModel.ViewedApplicationsCount = viewed.Count;
            viewModel.SuccessfulApplicationsCount = successful.Count;
            viewModel.UnsuccessfulApplicationsCount = unsuccessful.Count;

            vacancyApplicationsSearch.PageSizes = SelectListItemsFactory.GetPageSizes(vacancyApplicationsSearch.PageSize);

            viewModel.VacancyApplicationsSearch = vacancyApplicationsSearch;
            viewModel.ApplicationSummaries = new PageableViewModel<ApplicationSummaryViewModel>
            {
                Page = applications.Skip((vacancyApplicationsSearch.CurrentPage - 1) * vacancyApplicationsSearch.PageSize).Take(vacancyApplicationsSearch.PageSize).Select(a => _mapper.Map<ApprenticeshipApplicationSummary, ApplicationSummaryViewModel>(a)).ToList(),
                ResultsCount = applications.Count,
                CurrentPage = vacancyApplicationsSearch.CurrentPage,
                TotalNumberOfPages = applications.Count == 0 ? 1 : (int)Math.Ceiling((double)applications.Count / vacancyApplicationsSearch.PageSize)
            };

            return viewModel;
        }

        public ApprenticeshipApplicationViewModel GetApprenticeshipApplicationViewModel(ApplicationSelectionViewModel applicationSelectionViewModel)
        {
            var application = _apprenticeshipApplicationService.GetApplication(applicationSelectionViewModel.ApplicationId);
            var viewModel = ConvertToApprenticeshipApplicationViewModel(application, applicationSelectionViewModel);
            return viewModel;
        }

        public ApprenticeshipApplicationViewModel GetApprenticeshipApplicationViewModelForReview(ApplicationSelectionViewModel applicationSelectionViewModel)
        {
            var application = _apprenticeshipApplicationService.GetApplicationForReview(applicationSelectionViewModel.ApplicationId);
            var viewModel = ConvertToApprenticeshipApplicationViewModel(application, applicationSelectionViewModel);
            return viewModel;
        }

        public void UpdateApprenticeshipApplicationViewModelNotes(Guid applicationId, string notes)
        {
            _apprenticeshipApplicationService.UpdateApplicationNotes(applicationId, notes);
        }

        public ApplicationSelectionViewModel AppointCandidate(ApprenticeshipApplicationViewModel apprenticeshipApplicationViewModel)
        {
            var applicationId = apprenticeshipApplicationViewModel.ApplicationSelection.ApplicationId;

            _apprenticeshipApplicationService.AppointCandidate(applicationId);

            return apprenticeshipApplicationViewModel.ApplicationSelection;
        }

        #region Helpers

        private ApprenticeshipApplicationViewModel ConvertToApprenticeshipApplicationViewModel(ApprenticeshipApplicationDetail application, ApplicationSelectionViewModel applicationSelectionViewModel)
        {
            var vacancy = _vacancyPostingService.GetVacancy(application.Vacancy.Id);
            var viewModel = _mapper.Map<ApprenticeshipApplicationDetail, ApprenticeshipApplicationViewModel>(application);

            viewModel.ApplicationSelection = applicationSelectionViewModel;
            viewModel.Vacancy = _mapper.Map<Vacancy, ApplicationVacancyViewModel>(vacancy);

            return viewModel;
        }

        #endregion
    }
}