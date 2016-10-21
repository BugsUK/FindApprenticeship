namespace SFA.Apprenticeships.Web.Raa.Common.Providers
{
    using Application.Interfaces;
    using Application.Interfaces.Applications;
    using Application.Interfaces.Candidates;
    using Application.Interfaces.Employers;
    using Application.Interfaces.Providers;
    using Application.Interfaces.Security;
    using Application.Interfaces.VacancyPosting;
    using Common.Extensions;
    using Domain.Entities.Applications;
    using Domain.Entities.Candidates;
    using Domain.Entities.Raa.Locations;
    using Domain.Entities.Raa.Vacancies;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using ViewModels.Application;
    using ViewModels.Application.Apprenticeship;
    using ViewModels.Application.Traineeship;
    using ViewModels.VacancyStatus;
    using Web.Common.ViewModels;
    using Web.Common.ViewModels.Locations;
    using Order = Domain.Raa.Interfaces.Repositories.Models.Order;

    public class ApplicationProvider : IApplicationProvider
    {
        private readonly IVacancyPostingService _vacancyPostingService;
        private readonly IApprenticeshipApplicationService _apprenticeshipApplicationService;
        private readonly ITraineeshipApplicationService _traineeshipApplicationService;
        private readonly ICandidateApplicationService _candidateApplicationService;
        private readonly IProviderService _providerService;
        private readonly IEmployerService _employerService;
        private readonly IMapper _mapper;
        private readonly IEncryptionService<AnonymisedApplicationLink> _encryptionService;
        private readonly IDateTimeService _dateTimeService;
        private readonly ICurrentUserService _currentUserService;

        public ApplicationProvider(IVacancyPostingService vacancyPostingService,
            IApprenticeshipApplicationService apprenticeshipApplicationService, ITraineeshipApplicationService traineeshipApplicationService,
            ICandidateApplicationService candidateApplicationService, IProviderService providerService, IEmployerService employerService,
            IMapper mapper, IEncryptionService<AnonymisedApplicationLink> encryptionService, IDateTimeService dateTimeService,
            ICurrentUserService currentUserService)
        {
            _vacancyPostingService = vacancyPostingService;
            _apprenticeshipApplicationService = apprenticeshipApplicationService;
            _traineeshipApplicationService = traineeshipApplicationService;
            _candidateApplicationService = candidateApplicationService;
            _providerService = providerService;
            _employerService = employerService;
            _mapper = mapper;
            _encryptionService = encryptionService;
            _dateTimeService = dateTimeService;
            _currentUserService = currentUserService;
        }

        public ShareApplicationsViewModel GetShareApplicationsViewModel(int vacancyReferenceNumber)
        {
            var vacancy = _vacancyPostingService.GetVacancyByReferenceNumber(vacancyReferenceNumber);
            var vacancyOwnerRelationship = _providerService.GetVacancyOwnerRelationship(vacancy.VacancyOwnerRelationshipId, false);  // Closed vacancies can certainly have non-current vacancy parties
            var employer = _employerService.GetEmployer(vacancyOwnerRelationship.EmployerId, false);
            var ukprn = _currentUserService.GetClaimValue("ukprn");
            var provider = _providerService.GetProvider(ukprn);
            var viewModel = new ShareApplicationsViewModel();
            viewModel.EmployerName = employer.FullName;
            viewModel.ProviderName = provider.TradingName;
            viewModel.VacancyType = vacancy.VacancyType;
            viewModel.VacancyReferenceNumber = vacancyReferenceNumber;

            List<ApplicationSummary> applications = vacancy.VacancyType == VacancyType.Traineeship
                ? _traineeshipApplicationService.GetSubmittedApplicationSummaries(vacancy.VacancyId).Select(a => (ApplicationSummary)a).ToList()
                : _apprenticeshipApplicationService.GetSubmittedApplicationSummaries(vacancy.VacancyId).Select(a => (ApplicationSummary)a).ToList();

            var @new = applications.Where(v => v.Status == ApplicationStatuses.Submitted).ToList();
            var viewed = applications.Where(v => v.Status == ApplicationStatuses.InProgress).ToList();
            var successful = applications.Where(v => v.Status == ApplicationStatuses.Successful).ToList();
            var unsuccessful = applications.Where(v => v.Status == ApplicationStatuses.Unsuccessful).ToList();

            viewModel.NewApplicationsCount = @new.Count;
            viewModel.InProgressApplicationsCount = viewed.Count;
            viewModel.SuccessfulApplicationsCount = successful.Count;
            viewModel.UnsuccessfulApplicationsCount = unsuccessful.Count;
            viewModel.ApplicationSummaries = _mapper.Map<List<ApplicationSummary>, List<ApplicationSummaryViewModel>>(applications.OrderBy(a => a.CandidateDetails.LastName).ToList());

            return viewModel;
        }

        public BulkDeclineCandidatesViewModel GetBulkDeclineCandidatesViewModel(VacancyApplicationsSearchViewModel vacancyApplicationsSearchViewModel)
        {
            var vacancy = _vacancyPostingService.GetVacancyByReferenceNumber(vacancyApplicationsSearchViewModel.VacancyReferenceNumber);
            var vacancyOwnerRelationship = _providerService.GetVacancyOwnerRelationship(vacancy.VacancyOwnerRelationshipId, false);  // Closed vacancies can certainly have non-current vacancy parties
            var employer = _employerService.GetEmployer(vacancyOwnerRelationship.EmployerId, false);
            var viewModel = new BulkDeclineCandidatesViewModel
            {
                EmployerName = employer.FullName,
                VacancyType = vacancy.VacancyType,
                VacancyReferenceNumber = vacancy.VacancyReferenceNumber,
                VacancyTitle = vacancy.Title,
                VacancyId = vacancy.VacancyId
            };
            List<ApplicationSummary> applications = vacancy.VacancyType == VacancyType.Traineeship
                ? _traineeshipApplicationService.GetSubmittedApplicationSummaries(vacancy.VacancyId)
                .Where(v => v.Status == ApplicationStatuses.InProgress || v.Status == ApplicationStatuses.Submitted)
                .Select(a => (ApplicationSummary)a).ToList()
                : _apprenticeshipApplicationService.GetSubmittedApplicationSummaries(vacancy.VacancyId)
                .Where(v => v.Status == ApplicationStatuses.InProgress || v.Status == ApplicationStatuses.Submitted)
                .Select(a => (ApplicationSummary)a).ToList();

            var @new = applications.Where(v => v.Status == ApplicationStatuses.Submitted).ToList();
            var inProgress = applications.Where(v => v.Status == ApplicationStatuses.InProgress).ToList();

            switch (vacancyApplicationsSearchViewModel.FilterType)
            {
                case VacancyApplicationsFilterTypes.New:
                    applications = @new;
                    break;
                case VacancyApplicationsFilterTypes.InProgress:
                    applications = inProgress;
                    break;
            }
            viewModel.NewApplicationsCount = @new.Count;
            viewModel.InProgressApplicationsCount = inProgress.Count;
            viewModel.VacancyApplicationsSearch = vacancyApplicationsSearchViewModel;

            viewModel.ApplicationSummariesViewModel = new PageableViewModel<ApplicationSummaryViewModel>
            {
                Page = GetOrderedApplicationSummaries(vacancyApplicationsSearchViewModel.OrderByField, vacancyApplicationsSearchViewModel.Order, applications)
                .Skip((vacancyApplicationsSearchViewModel.CurrentPage - 1) * vacancyApplicationsSearchViewModel.PageSize)
                .Take(vacancyApplicationsSearchViewModel.PageSize)
                .Select(_mapper.Map<ApplicationSummary, ApplicationSummaryViewModel>).ToList(),
                ResultsCount = applications.Count,
                CurrentPage = vacancyApplicationsSearchViewModel.CurrentPage,
                TotalNumberOfPages = applications.Count == 0 ? 1 : (int)Math.Ceiling((double)applications.Count / vacancyApplicationsSearchViewModel.PageSize)
            };
            return viewModel;
        }

        public BulkDeclineCandidatesViewModel GetBulkDeclineCandidatesViewModel(int vacancyReferenceNumber)
        {
            var vacancy = _vacancyPostingService.GetVacancyByReferenceNumber(vacancyReferenceNumber);
            var vacancyOwnerRelationship = _providerService.GetVacancyOwnerRelationship(vacancy.VacancyOwnerRelationshipId, false);  // Closed vacancies can certainly have non-current vacancy parties
            var employer = _employerService.GetEmployer(vacancyOwnerRelationship.EmployerId, false);
            var viewModel = new BulkDeclineCandidatesViewModel
            {
                EmployerName = employer.FullName,
                VacancyType = vacancy.VacancyType,
                VacancyReferenceNumber = vacancyReferenceNumber,
                VacancyTitle = vacancy.Title,
                VacancyId = vacancy.VacancyId
            };
            List<ApplicationSummary> applications = vacancy.VacancyType == VacancyType.Traineeship
                ? _traineeshipApplicationService.GetSubmittedApplicationSummaries(vacancy.VacancyId)
                .Where(v => v.Status == ApplicationStatuses.InProgress || v.Status == ApplicationStatuses.Submitted)
                .Select(a => (ApplicationSummary)a).ToList()
                : _apprenticeshipApplicationService.GetSubmittedApplicationSummaries(vacancy.VacancyId)
                .Where(v => v.Status == ApplicationStatuses.InProgress || v.Status == ApplicationStatuses.Submitted)
                .Select(a => (ApplicationSummary)a).ToList();

            var @new = applications.Where(v => v.Status == ApplicationStatuses.Submitted).ToList();
            var viewed = applications.Where(v => v.Status == ApplicationStatuses.InProgress).ToList();
            viewModel.NewApplicationsCount = @new.Count;
            viewModel.InProgressApplicationsCount = viewed.Count;
            var vacancyApplicationsSearch = new VacancyApplicationsSearchViewModel(vacancyReferenceNumber);
            viewModel.ApplicationSummariesViewModel = new PageableViewModel<ApplicationSummaryViewModel>
            {
                Page = GetOrderedApplicationSummaries(vacancyApplicationsSearch.OrderByField, vacancyApplicationsSearch.Order, applications)
                .Skip((vacancyApplicationsSearch.CurrentPage - 1) * vacancyApplicationsSearch.PageSize).Take(vacancyApplicationsSearch.PageSize)
                .Select(_mapper.Map<ApplicationSummary, ApplicationSummaryViewModel>).ToList(),
                ResultsCount = applications.Count,
                CurrentPage = vacancyApplicationsSearch.CurrentPage,
                TotalNumberOfPages = applications.Count == 0 ? 1 : (int)Math.Ceiling((double)applications.Count / vacancyApplicationsSearch.PageSize)
            };
            viewModel.VacancyApplicationsSearch = vacancyApplicationsSearch;
            return viewModel;
        }

        public BulkApplicationsRejectViewModel GetBulkApplicationsRejectViewModel(
            BulkApplicationsRejectViewModel bulkApplicationsRejectViewModel)
        {
            IList<BulkRejectApplicationViewModel> bulkRejectApplications = new List<BulkRejectApplicationViewModel>();

            var vacancy =
                _vacancyPostingService.GetVacancyByReferenceNumber(
                    bulkApplicationsRejectViewModel.VacancyReferenceNumber);

            foreach (string applicationId in bulkApplicationsRejectViewModel.ApplicationIds)
            {
                ApprenticeshipApplicationDetail applicationDetail = _apprenticeshipApplicationService.GetApplicationForReview(new Guid(applicationId));
                if (applicationDetail != null)
                {
                    BulkRejectApplicationViewModel bulkRejectApplicationViewModel = new BulkRejectApplicationViewModel
                    {
                        ApplicationId = applicationId,
                        FirstName = applicationDetail.CandidateDetails.FirstName,
                        LastName = applicationDetail.CandidateDetails.LastName
                    };
                    bulkRejectApplications.Add(bulkRejectApplicationViewModel);
                }
            }
            bulkApplicationsRejectViewModel.BulkRejectApplications = bulkRejectApplications;
            bulkApplicationsRejectViewModel.VacancyTitle = vacancy.Title;
            return bulkApplicationsRejectViewModel;
        }

        public VacancyApplicationsViewModel GetVacancyApplicationsViewModel(VacancyApplicationsSearchViewModel vacancyApplicationsSearch)
        {
            var vacancy = _vacancyPostingService.GetVacancyByReferenceNumber(vacancyApplicationsSearch.VacancyReferenceNumber);
            var vacancyOwnerRelationship = _providerService.GetVacancyOwnerRelationship(vacancy.VacancyOwnerRelationshipId, false);  // Closed vacancies can certainly have non-current vacancy parties
            var employer = _employerService.GetEmployer(vacancyOwnerRelationship.EmployerId, false);
            var viewModel = _mapper.Map<Vacancy, VacancyApplicationsViewModel>(vacancy);
            viewModel.EmployerName = employer.FullName;
            viewModel.EmployerGeoPoint = _mapper.Map<GeoPoint, GeoPointViewModel>(employer.Address.GeoPoint);

            var applications = vacancy.VacancyType == VacancyType.Traineeship
                ? _traineeshipApplicationService.GetSubmittedApplicationSummaries(vacancy.VacancyId).Select(a => (ApplicationSummary)a).ToList()
                : _apprenticeshipApplicationService.GetSubmittedApplicationSummaries(vacancy.VacancyId).Select(a => (ApplicationSummary)a).ToList();

            applications = SearchCandidateApplications(vacancyApplicationsSearch, applications);

            var @new = applications.Where(v => v.Status == ApplicationStatuses.Submitted).ToList();
            var inProgress = applications.Where(v => v.Status == ApplicationStatuses.InProgress).ToList();
            var successful = applications.Where(v => v.Status == ApplicationStatuses.Successful).ToList();
            var unsuccessful = applications.Where(v => v.Status == ApplicationStatuses.Unsuccessful).ToList();

            switch (vacancyApplicationsSearch.FilterType)
            {
                case VacancyApplicationsFilterTypes.New:
                    applications = @new;
                    break;
                case VacancyApplicationsFilterTypes.InProgress:
                    applications = inProgress;
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
            viewModel.InProgressApplicationsCount = inProgress.Count;
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

            IList<CandidateSummary> candidateSummaries = _candidateApplicationService.GetCandidateSummaries(viewModel.ApplicationSummaries.Page.Select(@as => @as.CandidateId));
            foreach (var application in viewModel.ApplicationSummaries.Page)
            {
                application.AnonymousLinkData =
                    _encryptionService.Encrypt(new AnonymisedApplicationLink(application.ApplicationId,
                        _dateTimeService.TwoWeeksFromUtcNow));
                var candidateSummary = candidateSummaries.FirstOrDefault(cs => cs.EntityId == application.CandidateId);
                if (candidateSummary != null && candidateSummary.LegacyCandidateId != 0)
                {
                    var legacyCandidateReference = candidateSummary.LegacyCandidateId.ToString("0###-###-###").Substring(1, 11);
                    application.ApplicantID = $"{application.ApplicantID} ({legacyCandidateReference})";
                }
            }

            return viewModel;
        }

        private List<ApplicationSummary> SearchCandidateApplications(VacancyApplicationsSearchViewModel vacancyApplicationsSearch, List<ApplicationSummary> applications)
        {
            if (!vacancyApplicationsSearch.IsCandidateSearch())
            {
                return applications;
            }

            var applicantId = CandidateSearchExtensions.GetCandidateId(vacancyApplicationsSearch.ApplicantId);
            if (applicantId.HasValue)
            {
                var candidate = _candidateApplicationService.GetCandidate(applicantId.Value);
                if (candidate != null)
                    return applications.Where(a => a.CandidateId == candidate.EntityId).ToList();
            }

            var candidateGuidPrefix = CandidateSearchExtensions.GetCandidateGuidPrefix(vacancyApplicationsSearch.ApplicantId);
            if (!string.IsNullOrEmpty(candidateGuidPrefix))
            {
                return applications.Where(a => a.CandidateId.ToString().StartsWith(candidateGuidPrefix, StringComparison.InvariantCultureIgnoreCase)).ToList();
            }

            return
                applications.Where(
                    a =>
                        (string.IsNullOrEmpty(vacancyApplicationsSearch.FirstName) || a.CandidateDetails.FirstName.StartsWith(vacancyApplicationsSearch.FirstName, StringComparison.InvariantCultureIgnoreCase)) &&
                        (string.IsNullOrEmpty(vacancyApplicationsSearch.LastName) || a.CandidateDetails.LastName.StartsWith(vacancyApplicationsSearch.LastName, StringComparison.InvariantCultureIgnoreCase)) &&
                        (string.IsNullOrEmpty(vacancyApplicationsSearch.Postcode) || a.CandidateDetails.Address.Postcode.Replace(" ", "").StartsWith(vacancyApplicationsSearch.Postcode.Replace(" ", ""), StringComparison.InvariantCultureIgnoreCase))
                        ).ToList();
        }

        private static IEnumerable<ApplicationSummary> GetOrderedApplicationSummaries(string orderByField, Order order, IEnumerable<ApplicationSummary> applications)
        {
            IEnumerable<ApplicationSummary> page;
            switch (orderByField)
            {
                case VacancyApplicationsSearchViewModel.OrderByFieldLastName:
                    page = order == Order.Descending
                        ? applications.OrderByDescending(a => a.CandidateDetails.LastName).ThenBy(a => a.CandidateDetails.FirstName).ThenByDescending(a => a.DateApplied)
                        : applications.OrderBy(a => a.CandidateDetails.LastName).ThenBy(a => a.CandidateDetails.FirstName).ThenByDescending(a => a.DateApplied);
                    break;
                case VacancyApplicationsSearchViewModel.OrderByFieldFirstName:
                    page = order == Order.Descending
                        ? applications.OrderByDescending(a => a.CandidateDetails.FirstName).ThenBy(a => a.CandidateDetails.LastName).ThenByDescending(a => a.DateApplied)
                        : applications.OrderBy(a => a.CandidateDetails.FirstName).ThenBy(a => a.CandidateDetails.LastName).ThenByDescending(a => a.DateApplied);
                    break;
                case VacancyApplicationsSearchViewModel.OrderByFieldManagerNotes:
                    page = order == Order.Descending
                        ? applications.OrderByDescending(a => a.Notes).ThenBy(a => a.CandidateDetails.LastName).ThenBy(a => a.CandidateDetails.FirstName).ThenByDescending(a => a.DateApplied)
                        : applications.OrderBy(a => a.Notes).ThenBy(a => a.CandidateDetails.LastName).ThenBy(a => a.CandidateDetails.FirstName).ThenByDescending(a => a.DateApplied);
                    break;
                case VacancyApplicationsSearchViewModel.OrderByFieldStatus:
                    page = order == Order.Descending
                        ? applications.OrderByDescending(a => a.Status).ThenBy(a => a.CandidateDetails.LastName).ThenBy(a => a.CandidateDetails.FirstName).ThenByDescending(a => a.DateApplied)
                        : applications.OrderBy(a => a.Status).ThenBy(a => a.CandidateDetails.LastName).ThenBy(a => a.CandidateDetails.FirstName).ThenByDescending(a => a.DateApplied);
                    break;
                case VacancyApplicationsSearchViewModel.OrderByFieldSubmitted:
                    page = order == Order.Descending
                        ? applications.OrderByDescending(a => a.DateApplied).ThenBy(a => a.CandidateDetails.LastName).ThenBy(a => a.CandidateDetails.FirstName).ThenByDescending(a => a.DateApplied)
                        : applications.OrderBy(a => a.DateApplied).ThenBy(a => a.CandidateDetails.LastName).ThenBy(a => a.CandidateDetails.FirstName).ThenByDescending(a => a.DateApplied);
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

        public ApprenticeshipApplicationDetail GetApprenticeshipApplicationDetails(string applicationId)
        {
            return _apprenticeshipApplicationService.GetApplication(new Guid(applicationId));
        }

        public void UpdateApprenticeshipApplicationViewModelNotes(Guid applicationId, string notes, bool publishUpdate)
        {
            _apprenticeshipApplicationService.UpdateApplicationNotes(applicationId, notes, publishUpdate);
        }

        public ApplicationSelectionViewModel SendSuccessfulDecision(ApplicationSelectionViewModel applicationSelectionViewModel)
        {
            var applicationId = applicationSelectionViewModel.ApplicationId;
            _apprenticeshipApplicationService.SetSuccessfulDecision(applicationId);
            return applicationSelectionViewModel;
        }

        public ApplicationSelectionViewModel SendUnsuccessfulDecision(ApplicationSelectionViewModel applicationSelectionViewModel, string candidateApplicationFeedback)
        {
            var applicationId = applicationSelectionViewModel.ApplicationId;
            _apprenticeshipApplicationService.SetUnsuccessfulDecision(applicationId, candidateApplicationFeedback);
            return applicationSelectionViewModel;
        }

        public BulkApplicationsRejectViewModel SendBulkUnsuccessfulDecision(
            BulkApplicationsRejectViewModel bulkApplicationsRejectViewModel)
        {
            foreach (var application in bulkApplicationsRejectViewModel.ApplicationIds.First().Split(','))
            {
                _apprenticeshipApplicationService.SetUnsuccessfulDecision(new Guid(application), bulkApplicationsRejectViewModel.UnSuccessfulReason);
            }
            return bulkApplicationsRejectViewModel;
        }

        public ApplicationSelectionViewModel SetStateInProgress(ApplicationSelectionViewModel applicationSelectionViewModel)
        {
            var applicationId = applicationSelectionViewModel.ApplicationId;
            _apprenticeshipApplicationService.SetStateInProgress(applicationId);
            return applicationSelectionViewModel;
        }

        public ApplicationSelectionViewModel SetStateSubmitted(ApplicationSelectionViewModel applicationSelectionViewModel)
        {
            var applicationId = applicationSelectionViewModel.ApplicationId;
            _apprenticeshipApplicationService.SetStateSubmitted(applicationId);
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

        public void UpdateTraineeshipApplicationViewModelNotes(Guid applicationId, string notes, bool publishUpdate)
        {
            _traineeshipApplicationService.UpdateApplicationNotes(applicationId, notes, publishUpdate);
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
            var vacancyOwnerRelationship = _providerService.GetVacancyOwnerRelationship(vacancy.VacancyOwnerRelationshipId, false);  // Closed vacancies can certainly have non-current vacancy parties
            var employer = _employerService.GetEmployer(vacancyOwnerRelationship.EmployerId, false);
            var viewModel = _mapper.Map<ApprenticeshipApplicationDetail, ApprenticeshipApplicationViewModel>(application);
            viewModel.ApplicationSelection = applicationSelectionViewModel;
            viewModel.Vacancy = _mapper.Map<Vacancy, ApplicationVacancyViewModel>(vacancy);
            viewModel.Vacancy.EmployerName = employer.FullName;
            return viewModel;
        }

        private TraineeshipApplicationViewModel ConvertToTraineeshipApplicationViewModel(TraineeshipApplicationDetail application, ApplicationSelectionViewModel applicationSelectionViewModel)
        {
            var vacancy = _vacancyPostingService.GetVacancy(application.Vacancy.Id);
            var vacancyOwnerRelationship = _providerService.GetVacancyOwnerRelationship(vacancy.VacancyOwnerRelationshipId, false);  // Closed vacancies can certainly have non-current vacancy parties
            var employer = _employerService.GetEmployer(vacancyOwnerRelationship.EmployerId, false);
            var viewModel = _mapper.Map<TraineeshipApplicationDetail, TraineeshipApplicationViewModel>(application);
            viewModel.ApplicationSelection = applicationSelectionViewModel;
            viewModel.Vacancy = _mapper.Map<Vacancy, ApplicationVacancyViewModel>(vacancy);
            viewModel.Vacancy.EmployerName = employer.FullName;
            return viewModel;
        }

        #endregion
    }
}
