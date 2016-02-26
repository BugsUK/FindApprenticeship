namespace SFA.Apprenticeships.Web.Candidate.Providers
{
    using System;
    using System.Collections.Generic;
    using Application.Interfaces.Communications;
    using Common.ViewModels.Candidate;
    using Domain.Entities.Applications;
    using Domain.Entities.Candidates;
    using ViewModels;
    using ViewModels.Account;
    using ViewModels.Home;
    using ViewModels.Login;
    using ViewModels.Register;
    using ViewModels.VacancySearch;

    //todo: review whether operations should be defined in the account or candidate provider/service interfaces
    public interface ICandidateServiceProvider
    {
        bool Register(RegisterViewModel model);

        ActivationViewModel Activate(ActivationViewModel model, Guid candidateId);

        LoginResultViewModel Login(LoginViewModel model);

        UserNameAvailability IsUsernameAvailable(string username);

        ApplicationStatuses? GetApplicationStatus(Guid candidateId, int vacancyId);

        bool RequestForgottenPasswordResetCode(ForgottenPasswordViewModel model);

        AccountUnlockViewModel RequestAccountUnlockCode(AccountUnlockViewModel model);

        PasswordResetViewModel VerifyPasswordReset(PasswordResetViewModel passwordResetViewModel);

        AccountUnlockViewModel VerifyAccountUnlockCode(AccountUnlockViewModel model);

        bool ResendActivationCode(string username);

        Candidate GetCandidate(string username);

        Candidate GetCandidate(Guid candidateId);

        bool AcceptTermsAndConditions(Guid candidateId, string currentVersion);

        bool SendContactMessage(Guid? candidateId, ContactMessageViewModel viewModel);

        bool SendFeedback(Guid? candidateId, FeedbackViewModel viewModel);

        IEnumerable<ApprenticeshipApplicationSummary> GetApprenticeshipApplications(Guid candidateId, bool refresh = true);

        ApprenticeshipSearchViewModel CreateSavedSearch(Guid candidateId, ApprenticeshipSearchViewModel viewModel);

        SavedSearchViewModel DeleteSavedSearch(Guid candidateId, Guid savedSearchId);

        IEnumerable<SavedSearchViewModel> GetSavedSearches(Guid candidateId);

        SavedSearchViewModel GetSavedSearch(Guid candidateId, Guid savedSearchId);

        bool RequestEmailReminder(ForgottenEmailViewModel forgottenEmailViewModel);

        bool Unsubscribe(Guid subscriberId, SubscriptionTypes subscriptionType);

        void UpdateMonitoringInformation(Guid candidateId, MonitoringInformationViewModel monitoringInformationViewModel);

        string SendMobileVerificationCode(Guid candidateId);
    }
}
