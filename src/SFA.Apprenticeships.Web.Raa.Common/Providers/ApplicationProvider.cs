namespace SFA.Apprenticeships.Web.Raa.Common.Providers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using SFA.Infrastructure.Interfaces;
    using Application.Interfaces.Applications;
    using Application.Interfaces.Employers;
    using Application.Interfaces.Providers;
    using Application.Interfaces.Security;
    using Application.Interfaces.VacancyPosting;
    using Domain.Entities.Applications;
    using Domain.Entities.Raa.Locations;
    using Domain.Entities.Raa.Vacancies;
    using Factories;
    using ViewModels;
    using ViewModels.Application;
    using ViewModels.Application.Apprenticeship;
    using ViewModels.Application.Traineeship;
    using Web.Common.ViewModels;
    using Web.Common.ViewModels.Locations;
    using Order = ViewModels.Order;

    public class ApplicationProvider : IApplicationProvider
    {
        private readonly IConfigurationService _configurationService;
        private readonly IVacancyPostingService _vacancyPostingService;
        private readonly IApprenticeshipApplicationService _apprenticeshipApplicationService;
        private readonly ITraineeshipApplicationService _traineeshipApplicationService;
        private readonly IProviderService _providerService;
        private readonly IEmployerService _employerService;
        private readonly IMapper _mapper;
        private readonly ILogService _logService;
        private readonly IEncryptionService<AnonymisedApplicationLink> _encryptionService;
        private readonly IDateTimeService _dateTimeService;

        public ApplicationProvider(IConfigurationService configurationService, IVacancyPostingService vacancyPostingService, IApprenticeshipApplicationService apprenticeshipApplicationService, ITraineeshipApplicationService traineeshipApplicationService, IProviderService providerService, IEmployerService employerService, IMapper mapper, ILogService logService, IEncryptionService<AnonymisedApplicationLink> encryptionService, IDateTimeService dateTimeService)
        {
            _configurationService = configurationService;
            _vacancyPostingService = vacancyPostingService;
            _apprenticeshipApplicationService = apprenticeshipApplicationService;
            _traineeshipApplicationService = traineeshipApplicationService;
            _providerService = providerService;
            _employerService = employerService;
            _mapper = mapper;
            _logService = logService;
            _encryptionService = encryptionService;
            _dateTimeService = dateTimeService;
        }

        public ShareApplicationsViewModel GetShareApplicationsViewModel(int vacancyReferenceNumber)
        {
            var vacancy = _vacancyPostingService.GetVacancyByReferenceNumber(vacancyReferenceNumber);
            var vacancyParty = _providerService.GetVacancyParty(vacancy.OwnerPartyId, false);  // Closed vacancies can certainly have non-current vacancy parties
            var employer = _employerService.GetEmployer(vacancyParty.EmployerId);
            var viewModel = new ShareApplicationsViewModel();
            viewModel.EmployerName = employer.Name;
            viewModel.VacancyType = vacancy.VacancyType;
            viewModel.VacancyReferenceNumber = vacancyReferenceNumber;

            var applications = vacancy.VacancyType == VacancyType.Traineeship
                ? _traineeshipApplicationService.GetSubmittedApplicationSummaries(vacancy.VacancyId).Select(a => (ApplicationSummary)a).ToList()
                : _apprenticeshipApplicationService.GetSubmittedApplicationSummaries(vacancy.VacancyId).Select(a => (ApplicationSummary)a).ToList();

            var @new = applications.Where(v => v.Status == ApplicationStatuses.Submitted).ToList();
            var viewed = applications.Where(v => v.Status == ApplicationStatuses.InProgress).ToList();
            var successful = applications.Where(v => v.Status == ApplicationStatuses.Successful).ToList();
            var unsuccessful = applications.Where(v => v.Status == ApplicationStatuses.Unsuccessful).ToList();

            viewModel.NewApplicationsCount = @new.Count;
            viewModel.ViewedApplicationsCount = viewed.Count;
            viewModel.SuccessfulApplicationsCount = successful.Count;
            viewModel.UnsuccessfulApplicationsCount = unsuccessful.Count;
            viewModel.ApplicationSummaries = _mapper.Map<List<ApplicationSummary>, List<ApplicationSummaryViewModel>>(applications.OrderBy(a => a.CandidateDetails.LastName).ToList());

            return viewModel;
        }

        public VacancyApplicationsViewModel GetVacancyApplicationsViewModel(VacancyApplicationsSearchViewModel vacancyApplicationsSearch)
        {
            var vacancy = _vacancyPostingService.GetVacancyByReferenceNumber(vacancyApplicationsSearch.VacancyReferenceNumber);
            var vacancyParty = _providerService.GetVacancyParty(vacancy.OwnerPartyId, false);  // Closed vacancies can certainly have non-current vacancy parties
            var employer = _employerService.GetEmployer(vacancyParty.EmployerId);
            var viewModel = _mapper.Map<Vacancy, VacancyApplicationsViewModel>(vacancy);
            viewModel.EmployerName = employer.Name;
            viewModel.EmployerGeoPoint = _mapper.Map<GeoPoint, GeoPointViewModel>(employer.Address.GeoPoint);

            var applications = vacancy.VacancyType == VacancyType.Traineeship
                ? _traineeshipApplicationService.GetSubmittedApplicationSummaries(vacancy.VacancyId).Select(a => (ApplicationSummary)a).ToList()
                : _apprenticeshipApplicationService.GetSubmittedApplicationSummaries(vacancy.VacancyId).Select(a => (ApplicationSummary)a).ToList();

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

            viewModel.VacancyApplicationsSearch = vacancyApplicationsSearch;
            viewModel.ApplicationSummaries = new PageableViewModel<ApplicationSummaryViewModel>
            {
                Page = GetOrderedApplicationSummaries(vacancyApplicationsSearch.OrderByField, vacancyApplicationsSearch.Order, applications).Skip((vacancyApplicationsSearch.CurrentPage - 1) * vacancyApplicationsSearch.PageSize).Take(vacancyApplicationsSearch.PageSize).Select(_mapper.Map<ApplicationSummary, ApplicationSummaryViewModel>).ToList(),
                ResultsCount = applications.Count,
                CurrentPage = vacancyApplicationsSearch.CurrentPage,
                TotalNumberOfPages = applications.Count == 0 ? 1 : (int)Math.Ceiling((double)applications.Count / vacancyApplicationsSearch.PageSize)
            };

            foreach (var application in viewModel.ApplicationSummaries.Page)
            {
                application.AnonymousLinkData =
                    _encryptionService.Encrypt(new AnonymisedApplicationLink(application.ApplicationId,
                        _dateTimeService.TwoWeeksFromUtcNow));
            }

            return viewModel;
        }

        private static IEnumerable<ApplicationSummary> GetOrderedApplicationSummaries(string orderByField, Order order, IEnumerable<ApplicationSummary> applications)
        {
            IEnumerable<ApplicationSummary> page;
            switch (orderByField)
            {
                case VacancyApplicationsSearchViewModel.OrderByFieldLastName:
                    page = order == Order.Descending
                        ? applications.OrderByDescending(a => a.CandidateDetails.LastName)
                        : applications.OrderBy(a => a.CandidateDetails.LastName);
                    break;
                default:
                    page = applications;
                    break;
            }
            return page;
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

        public ApplicationSelectionViewModel SendSuccessfulDecision(ApplicationSelectionViewModel applicationSelectionViewModel)
        {
            var applicationId = applicationSelectionViewModel.ApplicationId;

            _apprenticeshipApplicationService.SetSuccessfulDecision(applicationId);

            return applicationSelectionViewModel;
        }

        public ApplicationSelectionViewModel SendUnsuccessfulDecision(ApplicationSelectionViewModel applicationSelectionViewModel)
        {
            var applicationId = applicationSelectionViewModel.ApplicationId;

            _apprenticeshipApplicationService.SetUnsuccessfulDecision(applicationId);

            return applicationSelectionViewModel;
        }

        public TraineeshipApplicationViewModel GetTraineeshipApplicationViewModel(ApplicationSelectionViewModel applicationSelectionViewModel)
        {
            var application = _traineeshipApplicationService.GetApplication(applicationSelectionViewModel.ApplicationId);
            var viewModel = ConvertToTraineeshipApplicationViewModel(application, applicationSelectionViewModel);

            return viewModel;
        }

        public TraineeshipApplicationViewModel GetTraineeshipApplicationViewModelForReview(ApplicationSelectionViewModel applicationSelectionViewModel)
        {
            var application = _traineeshipApplicationService.GetApplicationForReview(applicationSelectionViewModel.ApplicationId);
            var viewModel = ConvertToTraineeshipApplicationViewModel(application, applicationSelectionViewModel);

            return viewModel;
        }

        public void UpdateTraineeshipApplicationViewModelNotes(Guid applicationId, string notes)
        {
            _traineeshipApplicationService.UpdateApplicationNotes(applicationId, notes);
        }

        public void ShareApplications(int vacancyReferenceNumber, string providerName, IDictionary<string, string> applicationLinks, DateTime linkExpiryDateTime, string recipientEmailAddress)
        {
            var vacancy = _vacancyPostingService.GetVacancyByReferenceNumber(vacancyReferenceNumber);

            _employerService.SendApplicationLinks(vacancy.Title, providerName, applicationLinks, linkExpiryDateTime, recipientEmailAddress);
        }

        #region Helpers

        private ApprenticeshipApplicationViewModel ConvertToApprenticeshipApplicationViewModel(ApprenticeshipApplicationDetail application, ApplicationSelectionViewModel applicationSelectionViewModel)
        {
            var vacancy = _vacancyPostingService.GetVacancy(application.Vacancy.Id);
            var vacancyParty = _providerService.GetVacancyParty(vacancy.OwnerPartyId, false);  // Closed vacancies can certainly have non-current vacancy parties
            var employer = _employerService.GetEmployer(vacancyParty.EmployerId);
            var viewModel = _mapper.Map<ApprenticeshipApplicationDetail, ApprenticeshipApplicationViewModel>(application);

            viewModel.ApplicationSelection = applicationSelectionViewModel;
            viewModel.Vacancy = _mapper.Map<Vacancy, ApplicationVacancyViewModel>(vacancy);
            viewModel.Vacancy.EmployerName = employer.Name;

            return viewModel;
        }

        private TraineeshipApplicationViewModel ConvertToTraineeshipApplicationViewModel(TraineeshipApplicationDetail application, ApplicationSelectionViewModel applicationSelectionViewModel)
        {
            var vacancy = _vacancyPostingService.GetVacancy(application.Vacancy.Id);
            var vacancyParty = _providerService.GetVacancyParty(vacancy.OwnerPartyId, false);  // Closed vacancies can certainly have non-current vacancy parties
            var employer = _employerService.GetEmployer(vacancyParty.EmployerId);
            var viewModel = _mapper.Map<TraineeshipApplicationDetail, TraineeshipApplicationViewModel>(application);

            viewModel.ApplicationSelection = applicationSelectionViewModel;
            viewModel.Vacancy = _mapper.Map<Vacancy, ApplicationVacancyViewModel>(vacancy);
            viewModel.Vacancy.EmployerName = employer.Name;

            return viewModel;
        }

        #endregion
    }
}
