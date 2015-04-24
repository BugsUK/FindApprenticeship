namespace SFA.Apprenticeships.Domain.Entities.Candidates
{
    public class CommunicationPreferences
    {
        public CommunicationPreferences()
        {
            AllowMobile = false;
            AllowEmail = true;
            VerifiedMobile = false;
            MobileVerificationCode = string.Empty;
            AllowTraineeshipPrompts = true;

            SendApplicationStatusChanges = true;
            SendApplicationStatusChangesViaEmail = true;
            SendApplicationStatusChangesViaText = false;

            SendApprenticeshipApplicationsExpiring = true;
            SendApprenticeshipApplicationsExpiringViaEmail = true;
            SendApprenticeshipApplicationsExpiringViaText = false;

            SendMarketingCommunications = true;
            SendMarketingCommunicationsViaEmail = true;
            SendMarketingCommunicationsViaText = false;

            SendSavedSearchAlertsViaEmail = true;
            SendSavedSearchAlertsViaText = false;
        }

        public bool AllowEmail { get; set; }

        public bool AllowMobile { get; set; }

        public bool VerifiedMobile { get; set; }

        public string MobileVerificationCode { get; set; }

        public bool AllowTraineeshipPrompts { get; set; }

        public bool SendApplicationStatusChanges { get; set; }

        public bool SendApplicationStatusChangesViaText { get; set; }

        public bool SendApplicationStatusChangesViaEmail { get; set; }

        public bool SendApprenticeshipApplicationsExpiring { get; set; }

        public bool SendApprenticeshipApplicationsExpiringViaText { get; set; }

        public bool SendApprenticeshipApplicationsExpiringViaEmail { get; set; }

        public bool SendMarketingCommunications { get; set; }

        public bool SendMarketingCommunicationsViaText { get; set; }

        public bool SendMarketingCommunicationsViaEmail { get; set; }

        public bool SendSavedSearchAlertsViaEmail { get; set; }

        public bool SendSavedSearchAlertsViaText { get; set; }
    }
}
