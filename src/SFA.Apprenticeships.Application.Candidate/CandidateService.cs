namespace SFA.Apprenticeships.Application.Candidate
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using CuttingEdge.Conditions;
    using Domain.Entities.Applications;
    using Domain.Entities.Candidates;
    using Domain.Entities.Communication;
    using Domain.Entities.Vacancies.Apprenticeships;
    using Domain.Entities.Vacancies.Traineeships;
    using Domain.Interfaces.Repositories;
    using Infrastructure.Common.Configuration;
    using Interfaces.Candidates;
    using Interfaces.Communications;
    using SFA.Infrastructure.Interfaces;
    using Interfaces.Search;
    using Interfaces.Vacancies;
    using Strategies;
    using Strategies.Apprenticeships;
    using Strategies.SavedSearches;
    using Strategies.SuggestedVacancies;
    using Strategies.Traineeships;
    using UserAccount.Strategies;
    using IUpdateUsernameStrategy = Strategies.IUpdateUsernameStrategy;

    public class CandidateService : ICandidateService
    {
        private readonly ILogService _logger;

        private readonly IActivateCandidateStrategy _activateCandidateStrategy;
        private readonly IApprenticeshipApplicationReadRepository _apprenticeshipApplicationReadRepository;
        private readonly ITraineeshipApplicationReadRepository _traineeshipApplicationReadRepository;
        private readonly IAuthenticateCandidateStrategy _authenticateCandidateStrategy;
        private readonly IGetCandidateByIdStrategy _getCandidateByIdStrategy;
        private readonly ICreateApprenticeshipApplicationStrategy _createApplicationStrategy;
        private readonly ICreateTraineeshipApplicationStrategy _createTraineeshipApplicationStrategy;
        private readonly IGetCandidateApprenticeshipApplicationsStrategy _getCandidateApprenticeshipApplicationsStrategy;
        private readonly IGetCandidateTraineeshipApplicationsStrategy _getCandidateTraineeshipApplicationsStrategy;
        private readonly IRegisterCandidateStrategy _registerCandidateStrategy;
        private readonly ISaveApprenticeshipVacancyStrategy _saveVacancyStrategy;
        private readonly IDeleteSavedApprenticeshipVacancyStrategy _deleteSavedApprenticeshipVacancyStrategy;
        private readonly ICreateDraftApprenticeshipFromSavedVacancyStrategy _createDraftApprenticeshipFromSavedVacancyStrategy;
        private readonly IResetForgottenPasswordStrategy _resetForgottenPasswordStrategy;
        private readonly ISaveApprenticeshipApplicationStrategy _saveApplicationStrategy;
        private readonly ISaveTraineeshipApplicationStrategy _saveTraineeshipApplicationStrategy;
        private readonly IArchiveApplicationStrategy _archiveApplicationStrategy;
        private readonly ISubmitApprenticeshipApplicationStrategy _submitApprenticeshipApplicationStrategy;
        private readonly ISubmitApprenticeshipApplicationStrategy _submitLegacyApprenticeshipApplicationStrategy;
        private readonly ISubmitTraineeshipApplicationStrategy _submitTraineeshipApplicationStrategy;
        private readonly IUnlockAccountStrategy _unlockAccountStrategy;
        private readonly IDeleteApplicationStrategy _deleteApplicationStrategy;
        private readonly ISaveCandidateStrategy _saveCandidateStrategy;
        private readonly ILegacyGetCandidateVacancyDetailStrategy<ApprenticeshipVacancyDetail> _candidateApprenticeshipVacancyDetailStrategy;
        private readonly ILegacyGetCandidateVacancyDetailStrategy<TraineeshipVacancyDetail> _candidateTraineeshipVacancyDetailStrategy;
        private readonly ISendMobileVerificationCodeStrategy _sendMobileVerificationCodeStrategy;
        private readonly IVerifyMobileStrategy _verifyMobileStrategy;
        private readonly ISubmitContactMessageStrategy _submitContactMessageStrategy;
        private readonly ICreateSavedSearchStrategy _createSavedSearchStrategy;
        private readonly IRetrieveSavedSearchesStrategy _retrieveSavedSearchesStrategy;
        private readonly IUpdateSavedSearchStrategy _updateSavedSearchStrategy;
        private readonly IDeleteSavedSearchStrategy _deleteSavedSearchStrategy;
        private readonly IUpdateUsernameStrategy _updateUsernameStrategy;
        private readonly IRequestEmailReminderStrategy _requestEmailReminderStrategy;
        private readonly IUnsubscribeStrategy _unsubscribeStrategy;
        private readonly IApprenticeshipVacancySuggestionsStrategy _apprenticeshipVacancySuggestionsStrategy;
        private readonly IGetCandidateByUsernameStrategy _getCandidateByUsernameStrategy;
        private readonly IConfigurationService _configurationService;

        public CandidateService(
            IGetCandidateByIdStrategy getCandidateByIdStrategy,
            IApprenticeshipApplicationReadRepository apprenticeshipApplicationReadRepository,
            IActivateCandidateStrategy activateCandidateStrategy,
            IAuthenticateCandidateStrategy authenticateCandidateStrategy,
            ISubmitApprenticeshipApplicationStrategy submitApprenticeshipApplicationStrategy,
            ISubmitApprenticeshipApplicationStrategy submitLegacyApprenticeshipApplicationStrategy,
            IRegisterCandidateStrategy registerCandidateStrategy,
            ISaveApprenticeshipVacancyStrategy saveVacancyStrategy,
            IDeleteSavedApprenticeshipVacancyStrategy deleteSavedApprenticeshipVacancyStrategy,
            ICreateDraftApprenticeshipFromSavedVacancyStrategy createDraftApprenticeshipFromSavedVacancyStrategy,
            ICreateApprenticeshipApplicationStrategy createApplicationStrategy,
            ICreateTraineeshipApplicationStrategy createTraineeshipApplicationStrategy,
            IGetCandidateApprenticeshipApplicationsStrategy getCandidateApprenticeshipApplicationsStrategy,
            IResetForgottenPasswordStrategy resetForgottenPasswordStrategy,
            IUnlockAccountStrategy unlockAccountStrategy,
            ISaveApprenticeshipApplicationStrategy saveApplicationStrategy,
            IArchiveApplicationStrategy archiveApplicationStrategy,
            IDeleteApplicationStrategy deleteApplicationStrategy,
            ISaveCandidateStrategy saveCandidateStrategy,
            ISubmitTraineeshipApplicationStrategy submitTraineeshipApplicationStrategy,
            ISaveTraineeshipApplicationStrategy saveTraineeshipApplicationStrategy,
            ITraineeshipApplicationReadRepository traineeshipApplicationReadRepository,
            IGetCandidateTraineeshipApplicationsStrategy getCandidateTraineeshipApplicationsStrategy,
            ILegacyGetCandidateVacancyDetailStrategy<ApprenticeshipVacancyDetail> candidateApprenticeshipVacancyDetailStrategy,
            ILegacyGetCandidateVacancyDetailStrategy<TraineeshipVacancyDetail> candidateTraineeshipVacancyDetailStrategy,
            ISendMobileVerificationCodeStrategy sendMobileVerificationCodeStrategy,
            ILogService logService,
            IVerifyMobileStrategy verifyMobileStrategy,
            ISubmitContactMessageStrategy submitContactMessageStrategy,
            ICreateSavedSearchStrategy createSavedSearchStrategy,
            IRetrieveSavedSearchesStrategy retrieveSavedSearchesStrategy,
            IUpdateSavedSearchStrategy updateSavedSearchStrategy,
            IDeleteSavedSearchStrategy deleteSavedSearchStrategy,
            IUpdateUsernameStrategy updateUsernameStrategy,
            IRequestEmailReminderStrategy requestEmailReminderStrategy,
            IUnsubscribeStrategy unsubscribeStrategy,
            IApprenticeshipVacancySuggestionsStrategy apprenticeshipVacancySuggestionsStrategy,
            IGetCandidateByUsernameStrategy getCandidateByUsernameStrategy,
            IConfigurationService configurationService)
        {
            _getCandidateByIdStrategy = getCandidateByIdStrategy;
            _activateCandidateStrategy = activateCandidateStrategy;
            _authenticateCandidateStrategy = authenticateCandidateStrategy;
            _submitApprenticeshipApplicationStrategy = submitApprenticeshipApplicationStrategy;
            _submitLegacyApprenticeshipApplicationStrategy = submitLegacyApprenticeshipApplicationStrategy;
            _registerCandidateStrategy = registerCandidateStrategy;
            _saveVacancyStrategy = saveVacancyStrategy;
            _deleteSavedApprenticeshipVacancyStrategy = deleteSavedApprenticeshipVacancyStrategy;
            _createDraftApprenticeshipFromSavedVacancyStrategy = createDraftApprenticeshipFromSavedVacancyStrategy;
            _createApplicationStrategy = createApplicationStrategy;
            _createTraineeshipApplicationStrategy = createTraineeshipApplicationStrategy;
            _getCandidateApprenticeshipApplicationsStrategy = getCandidateApprenticeshipApplicationsStrategy;
            _resetForgottenPasswordStrategy = resetForgottenPasswordStrategy;
            _unlockAccountStrategy = unlockAccountStrategy;
            _apprenticeshipApplicationReadRepository = apprenticeshipApplicationReadRepository;
            _saveApplicationStrategy = saveApplicationStrategy;
            _archiveApplicationStrategy = archiveApplicationStrategy;
            _deleteApplicationStrategy = deleteApplicationStrategy;
            _saveCandidateStrategy = saveCandidateStrategy;
            _submitTraineeshipApplicationStrategy = submitTraineeshipApplicationStrategy;
            _saveTraineeshipApplicationStrategy = saveTraineeshipApplicationStrategy;
            _traineeshipApplicationReadRepository = traineeshipApplicationReadRepository;
            _getCandidateTraineeshipApplicationsStrategy = getCandidateTraineeshipApplicationsStrategy;
            _candidateApprenticeshipVacancyDetailStrategy = candidateApprenticeshipVacancyDetailStrategy;
            _candidateTraineeshipVacancyDetailStrategy = candidateTraineeshipVacancyDetailStrategy;
            _sendMobileVerificationCodeStrategy = sendMobileVerificationCodeStrategy;
            _logger = logService;
            _verifyMobileStrategy = verifyMobileStrategy;
            _submitContactMessageStrategy = submitContactMessageStrategy;
            _createSavedSearchStrategy = createSavedSearchStrategy;
            _retrieveSavedSearchesStrategy = retrieveSavedSearchesStrategy;
            _updateSavedSearchStrategy = updateSavedSearchStrategy;
            _deleteSavedSearchStrategy = deleteSavedSearchStrategy;
            _updateUsernameStrategy = updateUsernameStrategy;
            _requestEmailReminderStrategy = requestEmailReminderStrategy;
            _unsubscribeStrategy = unsubscribeStrategy;
            _apprenticeshipVacancySuggestionsStrategy = apprenticeshipVacancySuggestionsStrategy;
            _getCandidateByUsernameStrategy = getCandidateByUsernameStrategy;
            _configurationService = configurationService;
        }

        public Candidate Register(Candidate newCandidate, string password)
        {
            Condition.Requires(newCandidate);
            Condition.Requires(password).IsNotNullOrEmpty();

            _logger.Debug("Calling CandidateService to register a new candidate.");

            var candidate = _registerCandidateStrategy.RegisterCandidate(newCandidate, password);

            return candidate;
        }

        public void Activate(Guid id, string activationCode)
        {
            Condition.Requires(id);
            Condition.Requires(activationCode).IsNotNullOrEmpty();

            _logger.Info("Calling CandidateService to activate the user with id {0}.", id);

            _activateCandidateStrategy.ActivateCandidate(id, activationCode);
        }

        public Candidate Authenticate(string username, string password)
        {
            Condition.Requires(username).IsNotNullOrEmpty();
            Condition.Requires(password).IsNotNullOrEmpty();

            _logger.Debug("Calling CandidateService to authenticate the user {0}.", username);

            return _authenticateCandidateStrategy.AuthenticateCandidate(username, password);
        }

        public Candidate GetCandidate(Guid id)
        {
            _logger.Debug("Calling CandidateService to get the user with Id={0}.", id);
            return _getCandidateByIdStrategy.GetCandidate(id);
        }

        public Candidate GetCandidate(string username)
        {
            Condition.Requires(username).IsNotNullOrEmpty();

            _logger.Debug("Calling CandidateService to get the user {0}.", username);

            return _getCandidateByUsernameStrategy.GetCandidate(username);
        }

        public Candidate SaveCandidate(Candidate candidate)
        {
            Condition.Requires(candidate);

            _logger.Debug("Calling CandidateService to save a candidate.");

            return _saveCandidateStrategy.SaveCandidate(candidate);
        }

        public void UnlockAccount(string username, string accountUnlockCode)
        {
            Condition.Requires(username).IsNotNullOrEmpty();
            Condition.Requires(accountUnlockCode).IsNotNullOrEmpty();

            _logger.Debug("Calling CandidateService to unlock the account of the user {0}.", username);

            _unlockAccountStrategy.UnlockAccount(username, accountUnlockCode);
        }

        public void ResetForgottenPassword(string username, string passwordCode, string newPassword)
        {
            Condition.Requires(username).IsNotNullOrEmpty();
            Condition.Requires(passwordCode).IsNotNullOrEmpty();
            Condition.Requires(newPassword).IsNotNullOrEmpty();

            _logger.Debug("Calling CandidateService to reset the password for the user {0}.", username);

            _resetForgottenPasswordStrategy.ResetForgottenPassword(username, passwordCode, newPassword);
        }

        public ApprenticeshipApplicationDetail CreateApplication(Guid candidateId, int vacancyId)
        {
            Condition.Requires(candidateId);

            _logger.Debug(
                "Calling CandidateService to create an apprenticeship application of the user with Id={0} to the vacancy with Id={1}.",
                candidateId, vacancyId);

            return _createApplicationStrategy.CreateApplication(candidateId, vacancyId);
        }

        public ApprenticeshipApplicationDetail GetApplication(Guid candidateId, int vacancyId)
        {
            Condition.Requires(candidateId);

            _logger.Debug(
                "Calling CandidateService to get the apprenticeship application of the user with Id={0} to the vacancy with Id={1}.",
                candidateId, vacancyId);

            return _apprenticeshipApplicationReadRepository.GetForCandidate(candidateId, vacancyId);
        }

        public TraineeshipApplicationDetail CreateTraineeshipApplication(Guid candidateId, int vacancyId)
        {
            Condition.Requires(candidateId);

            _logger.Debug(
                "Calling CandidateService to create a traineeship application of the user with Id={0} to the vacancy with Id={1}.",
                candidateId, vacancyId);

            return _createTraineeshipApplicationStrategy.CreateApplication(candidateId, vacancyId);
        }

        public void ArchiveApplication(Guid candidateId, int vacancyId)
        {
            Condition.Requires(candidateId);

            _logger.Debug(
                "Calling CandidateService to archive the apprenticeship application of the user with Id={0} to the vacancy with Id={1}.",
                candidateId, vacancyId);

            _archiveApplicationStrategy.ArchiveApplication(candidateId, vacancyId);
        }

        public void UnarchiveApplication(Guid candidateId, int vacancyId)
        {
            Condition.Requires(candidateId);

            _logger.Info(
                "Calling CandidateService to unarchive the apprenticeship application of the user with Id={0} to the vacancy with Id={1}.",
                candidateId, vacancyId);

            _archiveApplicationStrategy.UnarchiveApplication(candidateId, vacancyId);
        }

        public void DeleteApplication(Guid candidateId, int vacancyId)
        {
            Condition.Requires(candidateId);

            _logger.Info(
                "Calling CandidateService to delete the apprenticeship application of the user with Id={0} to the vacancy with Id={1}.",
                candidateId, vacancyId);

            _deleteApplicationStrategy.DeleteApplication(candidateId, vacancyId);
        }

        public TraineeshipApplicationDetail GetTraineeshipApplication(Guid candidateId, int vacancyId)
        {
            Condition.Requires(candidateId);

            _logger.Debug(
                "Calling CandidateService to get the apprenticeship application of the user with Id={0} to the vacancy with Id={1}.",
                candidateId, vacancyId);

            return _traineeshipApplicationReadRepository.GetForCandidate(candidateId, vacancyId);
        }

        public void SaveApplication(Guid candidateId, int vacancyId, ApprenticeshipApplicationDetail apprenticeshipApplication)
        {
            Condition.Requires(apprenticeshipApplication);

            _logger.Debug(
                "Calling CandidateService to save the apprenticeship application of the user with Id={0} to the vacancy with Id={1}.",
                candidateId, vacancyId);

            _saveApplicationStrategy.SaveApplication(candidateId, vacancyId, apprenticeshipApplication);
        }

        public IList<ApprenticeshipApplicationSummary> GetApprenticeshipApplications(Guid candidateId, bool refresh = true)
        {
            Condition.Requires(candidateId);

            _logger.Debug(
                "Calling CandidateService to get the apprenticeship applications of the user with Id={0}.",
                candidateId);

            return _getCandidateApprenticeshipApplicationsStrategy.GetApplications(candidateId, refresh);
        }

        public void SubmitApplication(Guid candidateId, int vacancyId)
        {
            Condition.Requires(candidateId);

            _logger.Debug(
                "Calling CandidateService to submit the apprenticeship application of the user with Id={0} to the vacancy with Id={1}.",
                candidateId, vacancyId);

            var servicesConfiguration = _configurationService.Get<ServicesConfiguration>();
            if (servicesConfiguration.ServiceImplementation == ServicesConfiguration.Raa)
            {
                var vacancyDetails = _candidateApprenticeshipVacancyDetailStrategy.GetVacancyDetails(candidateId, vacancyId);
                if (vacancyDetails.EditedInRaa)
                {
                    _submitApprenticeshipApplicationStrategy.SubmitApplication(candidateId, vacancyId);
                }
                else
                {
                    _submitLegacyApprenticeshipApplicationStrategy.SubmitApplication(candidateId, vacancyId);
                }
            }
            else if (servicesConfiguration.ServiceImplementation == ServicesConfiguration.Legacy)
            {
                _submitLegacyApprenticeshipApplicationStrategy.SubmitApplication(candidateId, vacancyId);
            }
        }

        public void SubmitTraineeshipApplication(
            Guid candidateId, int vacancyId, TraineeshipApplicationDetail traineeshipApplicationDetail)
        {
            Condition.Requires(candidateId);

            _logger.Debug(
                "Calling CandidateService to submit the traineeship application of the user with Id={0} to the vacancy with Id={1}.",
                candidateId, vacancyId);

            var traineeshipDetails = _saveTraineeshipApplicationStrategy.SaveApplication(candidateId, vacancyId, traineeshipApplicationDetail);

            _submitTraineeshipApplicationStrategy.SubmitApplication(traineeshipDetails.EntityId);
        }

        public IList<TraineeshipApplicationSummary> GetTraineeshipApplications(Guid candidateId)
        {
            Condition.Requires(candidateId);

            _logger.Debug(
                "Calling CandidateService to get the traineeship applications of the user with Id={0}.",
                candidateId);

            return _getCandidateTraineeshipApplicationsStrategy.GetApplications(candidateId);
        }

        public ApprenticeshipVacancyDetail GetApprenticeshipVacancyDetail(Guid candidateId, int vacancyId)
        {
            Condition.Requires(candidateId);
            Condition.Requires(vacancyId);

            _logger.Debug("Calling CandidateService to get the apprenticeship vacancy ID {0} for candidate ID {1}.", vacancyId, candidateId);

            return _candidateApprenticeshipVacancyDetailStrategy.GetVacancyDetails(candidateId, vacancyId);
        }

        public TraineeshipVacancyDetail GetTraineeshipVacancyDetail(Guid candidateId, int vacancyId)
        {
            Condition.Requires(candidateId);
            Condition.Requires(vacancyId);

            _logger.Debug("Calling CandidateService to get the traineeship vacancy ID {0} for candidate ID {1}.", vacancyId, candidateId);

            return _candidateTraineeshipVacancyDetailStrategy.GetVacancyDetails(candidateId, vacancyId);
        }

        public void SendMobileVerificationCode(Candidate candidate)
        {
            Condition.Requires(candidate);

            _logger.Debug("Calling Send Mobile Verification Code for candidate Id: {0}", candidate.EntityId);

            _sendMobileVerificationCodeStrategy.SendMobileVerificationCode(candidate);
        }

        public void VerifyMobileCode(Guid candidateId, string verificationCode)
        {
            Condition.Requires(candidateId);
            Condition.Requires(verificationCode).IsNotNullOrEmpty();

            _logger.Info("Calling CandidateService to verify the mobile number for candidateId {0} with code {1}", candidateId, verificationCode);

            _verifyMobileStrategy.VerifyMobile(candidateId, verificationCode);
        }

        public void SubmitContactMessage(ContactMessage contactMessage)
        {
            Condition.Requires(contactMessage);

            _logger.Info("Calling CandidateService to send a contact message.");

            _submitContactMessageStrategy.SubmitMessage(contactMessage);
        }

        public ApplicationDetail SaveVacancy(Guid candidateId, int vacancyId)
        {
            Condition.Requires(candidateId);
            Condition.Requires(vacancyId);

            _logger.Info("Calling CandidateService to save vacancy id='{0}' for candidate='{1}.", vacancyId, candidateId);

            return _saveVacancyStrategy.SaveVacancy(candidateId, vacancyId);
        }

        public ApplicationDetail DeleteSavedVacancy(Guid candidateId, int vacancyId)
        {
            Condition.Requires(candidateId);
            Condition.Requires(vacancyId);

            _logger.Info("Calling CandidateService to delete saved vacancy id='{0}' for candidate='{1}.", vacancyId, candidateId);

            return _deleteSavedApprenticeshipVacancyStrategy.DeleteSavedVacancy(candidateId, vacancyId);
        }

        public ApprenticeshipApplicationDetail CreateDraftFromSavedVacancy(Guid candidateId, int vacancyId)
        {
            Condition.Requires(candidateId);
            Condition.Requires(vacancyId);

            _logger.Info("Calling CandidateService to create draft from saved vacancy, vacancy id='{0}' for candidate='{1}.", vacancyId, candidateId);

            return _createDraftApprenticeshipFromSavedVacancyStrategy.CreateDraft(candidateId, vacancyId);
        }

        public SavedSearch CreateSavedSearch(SavedSearch savedSearch)
        {
            Condition.Requires(savedSearch);

            _logger.Info("Calling CandidateService to create saved search for candidate='{0}'.", savedSearch.CandidateId);

            var createdSavedSearch = _createSavedSearchStrategy.CreateSavedSearch(savedSearch);
            return createdSavedSearch;
        }

        public IList<SavedSearch> GetSavedSearches(Guid candidateId)
        {
            Condition.Requires(candidateId);

            _logger.Info("Calling CandidateService to retrieve saved searches for candidate='{0}'.", candidateId);

            return _retrieveSavedSearchesStrategy.RetrieveSavedSearches(candidateId);
        }

        public SavedSearch UpdateSavedSearch(SavedSearch savedSearch)
        {
            Condition.Requires(savedSearch);

            _logger.Info("Calling CandidateService to update saved search with id='{0}' for candidate='{1}'.", savedSearch.EntityId, savedSearch.CandidateId);

            return _updateSavedSearchStrategy.UpdateSavedSearch(savedSearch);
        }

        public SavedSearch DeleteSavedSearch(Guid candidateId, Guid savedSearchId)
        {
            Condition.Requires(savedSearchId);

            _logger.Info("Calling CandidateService to delete saved search with id='{0}' for candidate='{1}'.", savedSearchId, candidateId);

            return _deleteSavedSearchStrategy.DeleteSavedSearch(candidateId, savedSearchId);
        }

        public SavedSearch GetSavedSearch(Guid candidateId, Guid savedSearchId)
        {
            _logger.Debug("Calling CandidateService to get saved search with id='{0}' for candidate='{1}'.", savedSearchId, candidateId);

            return _retrieveSavedSearchesStrategy
                .RetrieveSavedSearches(candidateId)
                .FirstOrDefault(each => each.EntityId == savedSearchId);
        }

        public void UpdateUsername(Guid userId, string verfiyCode, string password)
        {
            _updateUsernameStrategy.UpdateUsername(userId, verfiyCode, password);
        }

        public void RequestEmailReminder(string phoneNumber)
        {
            Condition.Requires(phoneNumber);

            _logger.Info("Using RequestEmailReminderStrategy to send email reminder for candidate(s) with mobile number='{0}'.", phoneNumber);

            _requestEmailReminderStrategy.RequestEmailReminder(phoneNumber);
        }

        public bool Unsubscribe(Guid subscriberId, SubscriptionTypes subscriptionType)
        {
            _logger.Info("Using UnsubscribeStrategy to unsubscribe candidate='{0}' from subscription type='{1}'",
                subscriberId, subscriptionType);

            return _unsubscribeStrategy.Unsubscribe(subscriberId, subscriptionType);
        }
        
        public SearchResults<ApprenticeshipSearchResponse, ApprenticeshipSearchParameters> GetSuggestedApprenticeshipVacancies(ApprenticeshipSearchParameters searchParameters, Guid candidateId, int vacancyId)
        {
            Condition.Requires(searchParameters).IsNotNull();
            Condition.Requires(vacancyId);
            
            var candidateApplications = GetApprenticeshipApplications(candidateId);
            return _apprenticeshipVacancySuggestionsStrategy.GetSuggestedApprenticeshipVacancies(searchParameters, candidateApplications, vacancyId);
        }
    }
}