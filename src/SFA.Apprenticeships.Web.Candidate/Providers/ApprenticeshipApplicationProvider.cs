namespace SFA.Apprenticeships.Web.Candidate.Providers
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using Application.Interfaces.Logging;
    using Application.Interfaces.ReferenceData;
    using Application.Interfaces.Vacancies;
    using Common.Configuration;
    using Common.Constants;
    using Common.Providers;
    using Domain.Entities.Locations;
    using Domain.Entities.ReferenceData;
    using Domain.Entities.Vacancies;
    using Application.Interfaces.Candidates;
    using Domain.Entities.Applications;
    using Domain.Entities.Exceptions;
    using Domain.Entities.Vacancies.Apprenticeships;
    using Domain.Interfaces.Configuration;
    using Domain.Interfaces.Mapping;
    using Constants.Pages;
    using ViewModels.Applications;
    using ViewModels.MyApplications;
    using Common.Models.Application;
    using ViewModels.VacancySearch;
    using ErrorCodes = Domain.Entities.ErrorCodes;
    using ApplicationErrorCodes = Application.Interfaces.Applications.ErrorCodes;

    //TODO: DFSW/AG This whole class needs refactoring or possibly reimplementing plus unit tests.
    public class ApprenticeshipApplicationProvider : IApprenticeshipApplicationProvider
    {
        private readonly ILogService _logger;
        private readonly IUserDataProvider _userDataProvider;
        private readonly IReferenceDataService _referenceDataService;
        private readonly IMapper _apprenticeshipCandidateWebMappers;
        private readonly IApprenticeshipVacancyProvider _apprenticeshipVacancyProvider;
        private readonly ICandidateService _candidateService;
        private readonly IConfigurationService _configurationService;

        public ApprenticeshipApplicationProvider(
            IApprenticeshipVacancyProvider apprenticeshipVacancyProvider,
            ICandidateService candidateService,
            IMapper apprenticeshipCandidateWebMappers,
            IConfigurationService configurationService, ILogService logger,
            IUserDataProvider userDataProvider,
            IReferenceDataService referenceDataService)
        {
            _apprenticeshipVacancyProvider = apprenticeshipVacancyProvider;
            _candidateService = candidateService;
            _apprenticeshipCandidateWebMappers = apprenticeshipCandidateWebMappers;
            _configurationService = configurationService;
            _logger = logger;
            _userDataProvider = userDataProvider;
            _referenceDataService = referenceDataService;
        }

        public ApprenticeshipApplicationViewModel GetApplicationViewModel(Guid candidateId, int vacancyId)
        {
            _logger.Debug(
                "Calling ApprenticeshipApplicationProvider to get the Application View Model for candidate ID: {0}, vacancy ID: {1}.",
                candidateId, vacancyId);

            try
            {
                var applicationDetails = _candidateService.GetApplication(candidateId, vacancyId);

                if (applicationDetails == null)
                {
                    return new ApprenticeshipApplicationViewModel(MyApplicationsPageMessages.ApplicationNotFound, ApplicationViewModelStatus.ApplicationNotFound);
                }

                if (applicationDetails.Status == ApplicationStatuses.Saved)
                {
                    applicationDetails = _candidateService.CreateDraftFromSavedVacancy(candidateId, vacancyId);
                }

                var applicationViewModel = _apprenticeshipCandidateWebMappers.Map<ApprenticeshipApplicationDetail, ApprenticeshipApplicationViewModel>(applicationDetails);

                return PatchWithVacancyDetail(candidateId, vacancyId, applicationViewModel);
            }
            catch (CustomException e)
            {
                if (e.Code == ErrorCodes.EntityStateError)
                {
                    _logger.Info(e.Message, e);
                    return
                        new ApprenticeshipApplicationViewModel(MyApplicationsPageMessages.ApplicationInIncorrectState,
                            ApplicationViewModelStatus.ApplicationInIncorrectState);
                }

                var message =
                    string.Format(
                        "Unhandled custom exception while getting the Application View Model for candidate ID: {0}, vacancy ID: {1}.",
                        candidateId, vacancyId);
                _logger.Error(message, e);
                return new ApprenticeshipApplicationViewModel(MyApplicationsPageMessages.UnhandledError, ApplicationViewModelStatus.Error);
            }
            catch (Exception e)
            {
                var message = string.Format("Get Application View Model failed for candidate ID: {0}, vacancy ID: {1}.",
                    candidateId, vacancyId);

                _logger.Error(message, e);

                return
                    new ApprenticeshipApplicationViewModel(
                        MyApplicationsPageMessages.CreateOrRetrieveApplicationFailed,
                        ApplicationViewModelStatus.Error);
            }
        }

        public ApprenticeshipApplicationPreviewViewModel GetApplicationPreviewViewModel(Guid candidateId, int vacancyId)
        {
            var viewModel = GetApplicationViewModel(candidateId, vacancyId);

            return new ApprenticeshipApplicationPreviewViewModel
            {
                Candidate = viewModel.Candidate,
                DateUpdated = viewModel.DateUpdated,
                DateApplied = viewModel.DateApplied,
                VacancyId = viewModel.VacancyId,
                VacancyDetail = viewModel.VacancyDetail,
                Status = viewModel.Status,
                ViewModelMessage = viewModel.ViewModelMessage,
                AcceptSubmit = false
            };
        }

        public ApprenticeshipApplicationViewModel CreateApplicationViewModel(Guid candidateId, int vacancyId)
        {
            _logger.Debug(
                "Calling ApprenticeshipApplicationProvider to get the Application View Model for candidate ID: {0}, vacancy ID: {1}.",
                candidateId, vacancyId);

            try
            {
                var applicationDetails = _candidateService.CreateApplication(candidateId, vacancyId);
                if (applicationDetails == null)
                {
                    return new ApprenticeshipApplicationViewModel
                    {
                        Status = ApplicationStatuses.ExpiredOrWithdrawn,
                        ViewModelMessage = MyApplicationsPageMessages.ApprenticeshipNoLongerAvailable
                    };
                }

                RecalculateSavedAndDraftCount(candidateId, null);
                var applicationViewModel = _apprenticeshipCandidateWebMappers.Map<ApprenticeshipApplicationDetail, ApprenticeshipApplicationViewModel>(applicationDetails);
                return PatchWithVacancyDetail(candidateId, vacancyId, applicationViewModel);
            }
            catch (CustomException e)
            {
                if (e.Code == ErrorCodes.EntityStateError)
                {
                    _logger.Info(e.Message, e);
                    return
                        new ApprenticeshipApplicationViewModel(MyApplicationsPageMessages.ApplicationInIncorrectState,
                            ApplicationViewModelStatus.ApplicationInIncorrectState);
                }

                var message =
                    string.Format(
                        "Unhandled custom exception while getting the Application View Model for candidate ID: {0}, vacancy ID: {1}.",
                        candidateId, vacancyId);
                _logger.Error(message, e);
                return new ApprenticeshipApplicationViewModel("Unhandled error", ApplicationViewModelStatus.Error);
            }
            catch (Exception e)
            {
                var message = string.Format("Get Application View Model failed for candidate ID: {0}, vacancy ID: {1}.",
                    candidateId, vacancyId);

                _logger.Error(message, e);

                return
                    new ApprenticeshipApplicationViewModel(
                        MyApplicationsPageMessages.CreateOrRetrieveApplicationFailed,
                        ApplicationViewModelStatus.Error);
            }
        }

        public ApprenticeshipApplicationViewModel PatchApplicationViewModel(Guid candidateId,
            ApprenticeshipApplicationViewModel savedModel, ApprenticeshipApplicationViewModel submittedModel)
        {
            _logger.Debug(
                "Calling ApprenticeshipApplicationProvider to patch the Application View Model for candidate ID: {0}.",
                candidateId);

            try
            {
                if (submittedModel.IsJavascript && !submittedModel.Candidate.MonitoringInformation.RequiresSupportForInterview)
                {
                    submittedModel.Candidate.MonitoringInformation.AnythingWeCanDoToSupportYourInterview = string.Empty;
                }

                savedModel.Candidate.AboutYou = submittedModel.Candidate.AboutYou;
                savedModel.Candidate.MonitoringInformation = submittedModel.Candidate.MonitoringInformation;
                savedModel.Candidate.Education = submittedModel.Candidate.Education;
                savedModel.Candidate.HasQualifications = submittedModel.Candidate.HasQualifications;
                savedModel.Candidate.Qualifications = submittedModel.Candidate.Qualifications;
                savedModel.Candidate.HasWorkExperience = submittedModel.Candidate.HasWorkExperience;
                savedModel.Candidate.WorkExperience = submittedModel.Candidate.WorkExperience;
                savedModel.Candidate.HasTrainingCourses = submittedModel.Candidate.HasTrainingCourses;
                savedModel.Candidate.TrainingCourses = submittedModel.Candidate.TrainingCourses;
                savedModel.Candidate.EmployerQuestionAnswers = submittedModel.Candidate.EmployerQuestionAnswers;

                return savedModel;
            }
            catch (Exception e)
            {
                var message =
                    string.Format(
                        "Patch application View Model failed for user {0}.", candidateId);
                if (submittedModel == null)
                {
                    message += " submittedModel was null";
                }
                else if (submittedModel.Candidate == null)
                {
                    message += " submittedModel.Candidate was null";
                }
                else if (submittedModel.Candidate.AboutYou == null)
                {
                    message += " submittedModel.Candidate.AboutYou was null";
                }
                _logger.Error(message, e);
                throw;
            }
        }

        public void SaveApplication(Guid candidateId, int vacancyId,
            ApprenticeshipApplicationViewModel apprenticeshipApplicationViewModel)
        {
            _logger.Debug(
                "Calling ApprenticeshipApplicationProvider to save the Application View Model for candidate ID: {0}, vacancy ID: {1}.",
                candidateId, vacancyId);

            try
            {
                var application =
                    _apprenticeshipCandidateWebMappers.Map<ApprenticeshipApplicationViewModel, ApprenticeshipApplicationDetail>(
                        apprenticeshipApplicationViewModel);

                _candidateService.SaveApplication(candidateId, vacancyId, application);
                _logger.Info("Application View Model saved for candidate ID: {0}, vacancy ID: {1}.",
                    candidateId, vacancyId);
            }
            catch (Exception e)
            {
                var message =
                    string.Format(
                        "Save application failed for user {0}.",
                        candidateId);
                _logger.Error(message, e);
                throw;
            }
        }

        public ApprenticeshipApplicationViewModel SubmitApplication(Guid candidateId, int vacancyId)
        {
            _logger.Debug(
                "Calling ApprenticeshipApplicationProvider to submit the Application for candidate ID: {0}, vacancy ID: {1}.",
                candidateId, vacancyId);

            var model = new ApprenticeshipApplicationViewModel();

            try
            {
                model = GetApplicationViewModel(candidateId, vacancyId);

                if (model.HasError())
                {
                    return model;
                }

                _candidateService.SubmitApplication(candidateId, vacancyId);

                _logger.Info("Application submitted for candidate ID: {0}, vacancy ID: {1}.",
                    candidateId, vacancyId);

                return model;
            }
            catch (CustomException e)
            {
                if (e.Code == ErrorCodes.EntityStateError)
                {
                    _logger.Info(e.Message, e);
                    return
                        new ApprenticeshipApplicationViewModel(ApplicationViewModelStatus.ApplicationInIncorrectState)
                        {
                            Status = model.Status
                        };
                }

                var message =
                    string.Format(
                        "Unhandled custom exception while submitting the Application for candidate ID: {0}, vacancy ID: {1}.",
                        candidateId, vacancyId);
                _logger.Error(message, e);

                return FailedApplicationViewModel(vacancyId, candidateId, "Submission of application",
                    ApplicationPageMessages.SubmitApplicationFailed, e);
            }
            catch (Exception e)
            {
                var message =
                    string.Format(
                        "Submit Application failed for candidate ID: {0}, vacancy ID: {1}.",
                        candidateId, vacancyId);
                _logger.Error(message, e);

                return FailedApplicationViewModel(vacancyId, candidateId, "Submission of application",
                    ApplicationPageMessages.SubmitApplicationFailed, e);
            }
        }

        public ApprenticeshipApplicationViewModel ArchiveApplication(Guid candidateId, int vacancyId)
        {
            _logger.Debug(
                "Calling ApprenticeshipApplicationProvider to archive the Application for candidate ID: {0}, vacancy ID: {1}.",
                candidateId, vacancyId);

            try
            {
                _candidateService.ArchiveApplication(candidateId, vacancyId);
                _logger.Info("Application archived for candidate ID: {0}, vacancy ID: {1}.",
                    candidateId, vacancyId);
            }
            catch (Exception e)
            {
                var message =
                    string.Format(
                        "Archive application failed for candidate ID: {0}, vacancy ID: {1}.",
                        candidateId, vacancyId);
                _logger.Error(message, e);

                return FailedApplicationViewModel(vacancyId, candidateId, "Archive of application",
                    ApplicationPageMessages.ArchiveFailed, e);
            }

            return new ApprenticeshipApplicationViewModel();
        }

        public ApprenticeshipApplicationViewModel UnarchiveApplication(Guid candidateId, int vacancyId)
        {
            _logger.Debug(
                "Calling ApprenticeshipApplicationProvider to ensure Application is unarchived for candidate ID: {0}, vacancy ID: {1}.",
                candidateId, vacancyId);

            try
            {
                _candidateService.UnarchiveApplication(candidateId, vacancyId);

                _logger.Info("Application unarchived for candidate ID: {0}, vacancy ID: {1}.",
                    candidateId, vacancyId);
            }
            catch (Exception e)
            {
                var message =
                    string.Format(
                        "Unarchive application failed for candidate ID: {0}, vacancy ID: {1}.",
                        candidateId, vacancyId);
                _logger.Error(message, e);

                return FailedApplicationViewModel(vacancyId, candidateId, "Unarchive of application",
                    ApplicationPageMessages.UnarchiveFailed, e);
            }

            return new ApprenticeshipApplicationViewModel();
        }

        public ApprenticeshipApplicationViewModel DeleteApplication(Guid candidateId, int vacancyId)
        {
            _logger.Debug(
                "Calling ApprenticeshipApplicationProvider to delete the Application for candidate ID: {0}, vacancy ID: {1}.",
                candidateId, vacancyId);

            try
            {
                _candidateService.DeleteApplication(candidateId, vacancyId);
                _logger.Info("Application deleted for candidate ID: {0}, vacancy ID: {1}.",
                    candidateId, vacancyId);
            }
            catch (CustomException e)
            {
                if (e.Code == ErrorCodes.EntityStateError)
                {
                    _logger.Info(e.Message, e);
                    return new ApprenticeshipApplicationViewModel();
                }

                var message =
                    string.Format(
                        "Unhandled custom exception while deleting the Application for candidate ID: {0}, vacancy ID: {1}.",
                        candidateId, vacancyId);
                _logger.Error(message, e);

                return FailedApplicationViewModel(vacancyId, candidateId, "Delete of application",
                    ApplicationPageMessages.DeleteFailed, e);
            }
            catch (Exception e)
            {
                var message =
                    string.Format(
                        "Delete application failed for candidate ID: {0}, vacancy ID: {1}.",
                        candidateId, vacancyId);
                _logger.Error(message, e);

                return FailedApplicationViewModel(vacancyId, candidateId, "Delete of application",
                    ApplicationPageMessages.DeleteFailed, e);
            }

            return new ApprenticeshipApplicationViewModel();
        }

        public WhatHappensNextApprenticeshipViewModel GetWhatHappensNextViewModel(Guid candidateId, int vacancyId, string searchReturnUrl)
        {
            _logger.Debug(
                "Calling ApprenticeshipApplicationProvider to get the What Happens Next data for candidate ID: {0}, vacancy ID: {1}.",
                candidateId, vacancyId);

            try
            {
                var applicationDetails = _candidateService.GetApplication(candidateId, vacancyId);
                var candidate = _candidateService.GetCandidate(candidateId);
                RecalculateSavedAndDraftCount(candidateId, null);

                if (applicationDetails == null || candidate == null)
                {
                    var message =
                        string.Format(
                            "Get What Happens Next View Model failed as no application was found for candidate ID: {0}, vacancy ID: {1}.",
                            candidateId, vacancyId);

                    _logger.Info(message);
                    return new WhatHappensNextApprenticeshipViewModel(MyApplicationsPageMessages.ApplicationNotFound);
                }

                var model = _apprenticeshipCandidateWebMappers.Map<ApprenticeshipApplicationDetail, ApprenticeshipApplicationViewModel>(applicationDetails);
                var patchedModel = PatchWithVacancyDetail(candidateId, vacancyId, model);

                if (patchedModel.HasError())
                {
                    return new WhatHappensNextApprenticeshipViewModel(patchedModel.ViewModelMessage);
                }

                var whatHappensNextViewModel = new WhatHappensNextApprenticeshipViewModel
                {
                    VacancyReference = patchedModel.VacancyDetail.VacancyReference,
                    VacancyTitle = patchedModel.VacancyDetail.Title,
                    Status = patchedModel.Status,
                    VacancyStatus = patchedModel.VacancyDetail.VacancyStatus,
                    ProviderContactInfo = patchedModel.VacancyDetail.Contact
                };


                whatHappensNextViewModel = WhatHappensNextSavedAndDrafts(whatHappensNextViewModel, candidateId);
                whatHappensNextViewModel = WhatHappensNextSuggestions(whatHappensNextViewModel, candidateId, applicationDetails.Vacancy.Id, searchReturnUrl);
                return whatHappensNextViewModel;
            }
            catch (Exception e)
            {
                var message =
                    string.Format("Get What Happens Next View Model failed for candidate ID: {0}, vacancy ID: {1}.",
                        candidateId, vacancyId);

                _logger.Error(message, e);

                return new WhatHappensNextApprenticeshipViewModel(MyApplicationsPageMessages.CreateOrRetrieveApplicationFailed);
            }
        }

        private WhatHappensNextApprenticeshipViewModel WhatHappensNextSavedAndDrafts(WhatHappensNextApprenticeshipViewModel whatHappensNextViewModel, Guid candidateId)
        {
            var apprenticeshipApplicationSummaries = _candidateService.GetApprenticeshipApplications(candidateId);
            var savedAndDraftApplications = apprenticeshipApplicationSummaries.Where(a => a.Status == ApplicationStatuses.Draft || a.Status == ApplicationStatuses.Saved).OrderBy(a => a.ClosingDate);
            whatHappensNextViewModel.SavedAndDraftApplications = savedAndDraftApplications.Select(s => new MyApprenticeshipApplicationViewModel(s)).ToList();
            return whatHappensNextViewModel;
        }

        private WhatHappensNextApprenticeshipViewModel WhatHappensNextSuggestions(WhatHappensNextApprenticeshipViewModel whatHappensNextViewModel, Guid candidateId, int vacancyId, string searchReturnUrl)
        {
            var searchReturnViewModel = ApprenticeshipSearchViewModel.FromSearchUrl(searchReturnUrl) ?? new ApprenticeshipSearchViewModel { WithinDistance = 40, ResultsPerPage = 5, PageNumber = 1 } ;
            var searchLocation = _apprenticeshipCandidateWebMappers.Map<ApprenticeshipSearchViewModel, Location>(searchReturnViewModel);

            var searchParameters = new ApprenticeshipSearchParameters
            {
                VacancyLocationType = ApprenticeshipLocationType.NonNational,
                ApprenticeshipLevel = searchReturnViewModel.ApprenticeshipLevel,
                SortType = VacancySearchSortType.Distance,
                Location = searchLocation,
                PageNumber = 1,
                PageSize = searchReturnViewModel.ResultsPerPage,
                SearchRadius = searchReturnViewModel.WithinDistance
            };

            var suggestedVacancies = _candidateService.GetSuggestedApprenticeshipVacancies(searchParameters, candidateId, vacancyId);

            var searchedCategory = (suggestedVacancies.SearchParameters.SubCategoryCodes != null && suggestedVacancies.SearchParameters.SubCategoryCodes.Length == 1
                ? _referenceDataService.GetSubCategoryByCode(suggestedVacancies.SearchParameters.SubCategoryCodes[0])
                : _referenceDataService.GetCategoryByCode(suggestedVacancies.SearchParameters.CategoryCode)) ??
                                        new Category { FullName = "Unknown" };

            whatHappensNextViewModel.SuggestedVacanciesSearchViewModel = new ApprenticeshipSearchViewModel(suggestedVacancies.SearchParameters);
            whatHappensNextViewModel.SuggestedVacanciesCategory = searchedCategory.FullName;
            whatHappensNextViewModel.SuggestedVacanciesSearchDistance = suggestedVacancies.SearchParameters.SearchRadius;
            whatHappensNextViewModel.SuggestedVacanciesSearchLocation = suggestedVacancies.SearchParameters.Location.Name;
            whatHappensNextViewModel.SuggestedVacancies =
                suggestedVacancies.Results.Select(x => new SuggestedVacancyViewModel
                {
                    VacancyId = x.Id,
                    VacancyTitle = x.Title,
                    IsPositiveAboutDisability = x.IsPositiveAboutDisability,
                    Distance = Math.Round(x.Distance, 1, MidpointRounding.AwayFromZero).ToString(CultureInfo.InvariantCulture)
                }).ToList();

            return whatHappensNextViewModel;
        }

        public MyApplicationsViewModel GetMyApplications(Guid candidateId)
        {
            _logger.Debug("Calling ApprenticeshipApplicationProvider to get the applications for candidate ID: {0}.",
                candidateId);

            try
            {
                var apprenticeshipApplicationSummaries = _candidateService.GetApprenticeshipApplications(candidateId);
                RecalculateSavedAndDraftCount(candidateId, apprenticeshipApplicationSummaries);

                var apprenticeshipApplications = apprenticeshipApplicationSummaries
                    .Select(each => new MyApprenticeshipApplicationViewModel(each))
                    .ToList();

                var traineeshipApplicationSummaries = _candidateService.GetTraineeshipApplications(candidateId);

                var traineeshipApplications = traineeshipApplicationSummaries
                    .Select(each => new MyTraineeshipApplicationViewModel
                    {
                        VacancyId = each.LegacyVacancyId,
                        VacancyStatus = each.VacancyStatus,
                        Title = each.Title,
                        EmployerName = each.EmployerName,
                        IsArchived = each.IsArchived,
                        DateApplied = each.DateApplied
                    })
                    .ToList();

                var traineeshipFeatureViewModel = GetTraineeshipFeatureViewModel(candidateId, apprenticeshipApplicationSummaries, traineeshipApplicationSummaries);
                var lastApplicationStatusNotification = _userDataProvider.Get(UserDataItemNames.LastApplicationStatusNotification);
                DateTime? lastApplicationStatusNotificationDateTime = null;

                if (!string.IsNullOrWhiteSpace(lastApplicationStatusNotification))
                {
                    lastApplicationStatusNotificationDateTime = new DateTime(long.Parse(lastApplicationStatusNotification), DateTimeKind.Utc);
                }

                return new MyApplicationsViewModel(apprenticeshipApplications, traineeshipApplications, traineeshipFeatureViewModel, lastApplicationStatusNotificationDateTime);
            }
            catch (Exception e)
            {
                var message = string.Format("Get MyApplications failed for candidate ID: {0}.", candidateId);

                _logger.Error(message, e);

                throw;
            }
        }

        public TraineeshipFeatureViewModel GetTraineeshipFeatureViewModel(Guid candidateId)
        {
            try
            {
                var apprenticeshipApplicationSummaries = _candidateService.GetApprenticeshipApplications(candidateId);
                var traineeshipApplicationSummaries = _candidateService.GetTraineeshipApplications(candidateId);
                var traineeshipFeatureViewModel = GetTraineeshipFeatureViewModel(candidateId, apprenticeshipApplicationSummaries, traineeshipApplicationSummaries);

                return traineeshipFeatureViewModel;
            }
            catch (Exception e)
            {
                var message = string.Format("Get Traineeship Feature View Model failed for candidate ID: {0}.", candidateId);

                _logger.Error(message, e);

                throw;
            }
        }

        public SavedVacancyViewModel SaveVacancy(Guid candidateId, int vacancyId)
        {
            var applicationDetail = _candidateService.SaveVacancy(candidateId, vacancyId);

            return new SavedVacancyViewModel
            {
                ApplicationStatus = applicationDetail == null ? default(ApplicationStatuses?) : applicationDetail.Status
            };
        }

        public SavedVacancyViewModel DeleteSavedVacancy(Guid candidateId, int vacancyId)
        {
            var applicationDetail = _candidateService.DeleteSavedVacancy(candidateId, vacancyId);

            return new SavedVacancyViewModel
            {
                ApplicationStatus = applicationDetail == null ? default(ApplicationStatuses?) : applicationDetail.Status
            };
        }

        #region Helpers

        private TraineeshipFeatureViewModel GetTraineeshipFeatureViewModel(Guid candidateId, IList<ApprenticeshipApplicationSummary> apprenticeshipApplicationSummaries, IList<TraineeshipApplicationSummary> traineeshipApplicationSummaries)
        {
            var candididate = _candidateService.GetCandidate(candidateId);

            var webConfiguration = _configurationService.Get<CommonWebConfiguration>();
            var unsuccessfulApplicationsToShowTraineeshipsPrompt = webConfiguration.UnsuccessfulApplicationsToShowTraineeshipsPrompt;
            var allowTraineeshipPrompts = candididate.CommunicationPreferences.AllowTraineeshipPrompts;

            var sufficentUnsuccessfulApprenticeshipApplicationsToPrompt = apprenticeshipApplicationSummaries.Count(each => each.Status == ApplicationStatuses.Unsuccessful) >= unsuccessfulApplicationsToShowTraineeshipsPrompt;
            var candidateHasSuccessfulApprenticeshipApplication = apprenticeshipApplicationSummaries.Any(each => each.Status == ApplicationStatuses.Successful);
            var candidateHasAppliedForTraineeship = traineeshipApplicationSummaries.Any();

            var viewModel = new TraineeshipFeatureViewModel
            {
                ShowTraineeshipsPrompt = allowTraineeshipPrompts && sufficentUnsuccessfulApprenticeshipApplicationsToPrompt && !candidateHasSuccessfulApprenticeshipApplication && !candidateHasAppliedForTraineeship,
                ShowTraineeshipsLink = (sufficentUnsuccessfulApprenticeshipApplicationsToPrompt || candidateHasAppliedForTraineeship)
            };

            return viewModel;
        }

        private ApprenticeshipApplicationViewModel FailedApplicationViewModel(
            int vacancyId,
            Guid candidateId,
            string failure,
            string failMessage, Exception e)
        {
            var message = string.Format("{0} {1} failed for user {2}", failure, vacancyId, candidateId);
            _logger.Error(message, e);
            return new ApprenticeshipApplicationViewModel(failMessage, ApplicationViewModelStatus.Error);
        }

        private ApprenticeshipApplicationViewModel PatchWithVacancyDetail(
            Guid candidateId, int vacancyId, ApprenticeshipApplicationViewModel apprenticeshipApplicationViewModel)
        {
            // TODO: why have a patch method like this? should be done in mapper.
            var vacancyDetailViewModel = _apprenticeshipVacancyProvider.GetVacancyDetailViewModel(candidateId, vacancyId);

            if (vacancyDetailViewModel == null || vacancyDetailViewModel.VacancyStatus == VacancyStatuses.Unavailable)
            {
                apprenticeshipApplicationViewModel.ViewModelMessage = MyApplicationsPageMessages.ApprenticeshipNoLongerAvailable;
                apprenticeshipApplicationViewModel.Status = ApplicationStatuses.ExpiredOrWithdrawn;

                return apprenticeshipApplicationViewModel;
            }

            if (vacancyDetailViewModel.HasError())
            {
                apprenticeshipApplicationViewModel.ViewModelMessage = vacancyDetailViewModel.ViewModelMessage;

                return apprenticeshipApplicationViewModel;
            }

            apprenticeshipApplicationViewModel.VacancyDetail = vacancyDetailViewModel;
            apprenticeshipApplicationViewModel.Candidate.EmployerQuestionAnswers.SupplementaryQuestion1 = vacancyDetailViewModel.SupplementaryQuestion1;
            apprenticeshipApplicationViewModel.Candidate.EmployerQuestionAnswers.SupplementaryQuestion2 = vacancyDetailViewModel.SupplementaryQuestion2;

            return apprenticeshipApplicationViewModel;
        }

        private void RecalculateSavedAndDraftCount(Guid candidateId, IList<ApprenticeshipApplicationSummary> summaries)
        {
            var apprenticeshipApplicationSummaries = summaries ?? _candidateService.GetApprenticeshipApplications(candidateId) ?? new List<ApprenticeshipApplicationSummary>();
            var savedOrDraft = apprenticeshipApplicationSummaries.Count(a => a.Status == ApplicationStatuses.Draft || a.Status == ApplicationStatuses.Saved);
            _userDataProvider.Push(UserDataItemNames.SavedAndDraftCount, savedOrDraft.ToString(CultureInfo.InvariantCulture));
        }

        #endregion
    }
}
