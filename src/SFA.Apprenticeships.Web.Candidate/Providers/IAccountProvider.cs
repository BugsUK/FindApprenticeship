namespace SFA.Apprenticeships.Web.Candidate.Providers
{
    using Domain.Entities.Candidates;
    using System;
    using ViewModels.Account;

    //todo: review whether operations should be defined in the account or candidate provider/service interfaces
    public interface IAccountProvider
    {
        SettingsViewModel GetSettingsViewModel(Guid candidateId);

        bool TrySaveSettings(Guid candidateId, SettingsViewModel model, out Candidate candidate);

        bool SetUserAccountDeletionPending(Guid candidateId, out Candidate candidate);

        bool DismissTraineeshipPrompts(Guid candidateId);

        VerifyMobileViewModel GetVerifyMobileViewModel(Guid candidateId);

        VerifyMobileViewModel VerifyMobile(Guid candidateId, VerifyMobileViewModel model);

        VerifyMobileViewModel SendMobileVerificationCode(Guid candidateId, VerifyMobileViewModel model);

        EmailViewModel UpdateEmailAddress(Guid userId, EmailViewModel emailViewModel);

        VerifyUpdatedEmailViewModel VerifyUpdatedEmailAddress(Guid userId, VerifyUpdatedEmailViewModel model);

        VerifyUpdatedEmailViewModel ResendUpdateEmailAddressCode(Guid userId);
    }
}
