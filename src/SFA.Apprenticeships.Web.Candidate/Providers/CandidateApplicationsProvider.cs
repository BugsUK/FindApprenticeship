namespace SFA.Apprenticeships.Web.Candidate.Providers
{
    using Application.Interfaces;
    using Application.Interfaces.Candidates;
    using Common.Configuration;
    using Common.Constants;
    using Common.Providers;
    using Domain.Entities.Applications;
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using ViewModels.Applications;
    using ViewModels.MyApplications;

    public class CandidateApplicationsProvider : ICandidateApplicationsProvider
    {
        private readonly ICandidateApplicationService _candidateApplicationService;
        private readonly IUserDataProvider _userDataProvider;
        private readonly IConfigurationService _configurationService;
        private readonly ILogService _logService;

        public CandidateApplicationsProvider(ICandidateApplicationService candidateApplicationService, IUserDataProvider userDataProvider, IConfigurationService configurationService, ILogService logService)
        {
            _candidateApplicationService = candidateApplicationService;
            _userDataProvider = userDataProvider;
            _configurationService = configurationService;
            _logService = logService;
        }

        public MyApplicationsViewModel GetCandidateApplications(Guid candidateId)
        {
            _logService.Debug("Calling CandidateApprenticeshipApplicationProvider to get the applications for candidate ID: {0}.",
                candidateId);

            try
            {
                var apprenticeshipApplicationSummaries = _candidateApplicationService.GetApprenticeshipApplications(candidateId);
                RecalculateSavedAndDraftCount(candidateId, apprenticeshipApplicationSummaries);

                var apprenticeshipApplications = apprenticeshipApplicationSummaries
                    .Select(each => new MyApprenticeshipApplicationViewModel(each))
                    .ToList();

                var traineeshipApplicationSummaries = _candidateApplicationService.GetTraineeshipApplications(candidateId);

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
                var message = $"Get MyApplications failed for candidate ID: {candidateId}.";

                _logService.Error(message, e);

                throw;
            }
        }

        public TraineeshipFeatureViewModel GetTraineeshipFeatureViewModel(Guid candidateId)
        {
            try
            {
                var apprenticeshipApplicationSummaries = _candidateApplicationService.GetApprenticeshipApplications(candidateId);
                var traineeshipApplicationSummaries = _candidateApplicationService.GetTraineeshipApplications(candidateId);
                var traineeshipFeatureViewModel = GetTraineeshipFeatureViewModel(candidateId, apprenticeshipApplicationSummaries, traineeshipApplicationSummaries);

                return traineeshipFeatureViewModel;
            }
            catch (Exception e)
            {
                var message = $"Get Traineeship Feature View Model failed for candidate ID: {candidateId}.";

                _logService.Error(message, e);

                throw;
            }
        }

        private TraineeshipFeatureViewModel GetTraineeshipFeatureViewModel(Guid candidateId, IList<ApprenticeshipApplicationSummary> apprenticeshipApplicationSummaries, IList<TraineeshipApplicationSummary> traineeshipApplicationSummaries)
        {
            var candididate = _candidateApplicationService.GetCandidate(candidateId);

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

        public void RecalculateSavedAndDraftCount(Guid candidateId, IList<ApprenticeshipApplicationSummary> summaries)
        {
            var apprenticeshipApplicationSummaries = summaries ?? _candidateApplicationService.GetApprenticeshipApplications(candidateId) ?? new List<ApprenticeshipApplicationSummary>();
            var savedOrDraft = apprenticeshipApplicationSummaries.Count(a => a.Status == ApplicationStatuses.Draft || a.Status == ApplicationStatuses.Saved);
            _userDataProvider.Push(UserDataItemNames.SavedAndDraftCount, savedOrDraft.ToString(CultureInfo.InvariantCulture));
        }
    }
}