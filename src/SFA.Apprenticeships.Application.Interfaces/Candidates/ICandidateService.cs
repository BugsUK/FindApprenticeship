namespace SFA.Apprenticeships.Application.Interfaces.Candidates
{
    using Communications;
    using Domain.Entities.Applications;
    using Domain.Entities.Candidates;
    using Domain.Entities.Communication;
    using Domain.Entities.Vacancies.Apprenticeships;
    using Domain.Entities.Vacancies.Traineeships;
    using Search;
    using System;
    using System.Collections.Generic;
    using Vacancies;

    /// <summary>
    /// For candidate users to register, manage their profile and other dashboard entities
    /// </summary>
    //todo: review whether operations should be defined in the account or candidate provider/service interfaces
    public interface ICandidateService
    {
        Candidate Register(Candidate newCandidate, string password);

        void Activate(Guid id, string activationCode);

        Candidate Authenticate(string username, string password);

        Candidate GetCandidate(Guid id);

        Candidate GetCandidate(string username);

        Candidate SaveCandidate(Candidate candidate);

        void SetCandidateDeletionPending(Candidate candidate);

        ApprenticeshipApplicationDetail CreateApplication(Guid candidateId, int vacancyId); // note: only an int due to legacy - will be a Guid

        ApprenticeshipApplicationDetail GetApplication(Guid candidateId, int vacancyId);

        void ArchiveApplication(Guid candidateId, int vacancyId);

        void UnarchiveApplication(Guid candidateId, int vacancyId);

        void SaveApplication(Guid candidateId, int vacancyId, ApprenticeshipApplicationDetail apprenticeshipApplication);

        IList<ApprenticeshipApplicationSummary> GetApprenticeshipApplications(Guid candidateId, bool refresh = true);

        void SubmitApplication(Guid candidateId, int vacancyId);

        TraineeshipApplicationDetail CreateTraineeshipApplication(Guid candidateId, int vacancyId);

        TraineeshipApplicationDetail GetTraineeshipApplication(Guid candidateId, int vacancyId);

        IList<TraineeshipApplicationSummary> GetTraineeshipApplications(Guid candidateId);

        void SubmitTraineeshipApplication(Guid candidateId, int vacancyId, TraineeshipApplicationDetail traineeshipApplicationDetail); //todo: refactor to remove traineeship argument

        void UnlockAccount(string username, string accountUnlockCode);

        void ResetForgottenPassword(string username, string passwordCode, string newPassword);

        void DeleteApplication(Guid candidateId, int vacancyId);

        ApprenticeshipVacancyDetail GetApprenticeshipVacancyDetail(Guid candidateId, int vacancyId);

        TraineeshipVacancyDetail GetTraineeshipVacancyDetail(Guid candidateId, int vacancyId);

        void SendMobileVerificationCode(Candidate candidate);

        void VerifyMobileCode(Guid candidateId, string verificationCode);

        void SubmitContactMessage(ContactMessage contactMessage);

        ApplicationDetail SaveVacancy(Guid candidateId, int vacancyId);

        ApplicationDetail DeleteSavedVacancy(Guid candidateId, int vacancyId);

        ApprenticeshipApplicationDetail CreateDraftFromSavedVacancy(Guid candidateId, int vacancyId);

        SavedSearch CreateSavedSearch(SavedSearch savedSearch);

        IList<SavedSearch> GetSavedSearches(Guid candidateId);

        SavedSearch UpdateSavedSearch(SavedSearch savedSearch);

        SavedSearch DeleteSavedSearch(Guid candidateId, Guid savedSearchId);

        SavedSearch GetSavedSearch(Guid candidateId, Guid savedSearchId);

        void UpdateUsername(Guid userId, string verfiyCode, string password);

        void RequestEmailReminder(string phoneNumber);

        bool Unsubscribe(Guid subscriberId, SubscriptionTypes subscriptionType);

        SearchResults<ApprenticeshipSearchResponse, ApprenticeshipSearchParameters> GetSuggestedApprenticeshipVacancies(
            ApprenticeshipSearchParameters searchParameters, Guid candidateId, int vacancyId);
    }
}
