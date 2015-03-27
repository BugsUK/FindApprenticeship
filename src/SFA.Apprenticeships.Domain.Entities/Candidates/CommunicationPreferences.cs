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
            SendApprenticeshipApplicationsExpiring = true;
            SendMarketingCommunications = true;
            SendSavedSearchAlertsViaEmail = true;
            SendSavedSearchAlertsViaText = false;
        }

        public bool AllowEmail { get; set; }

        public bool AllowMobile { get; set; }

        public bool VerifiedMobile { get; set; }

        public string MobileVerificationCode { get; set; }

        public bool AllowTraineeshipPrompts { get; set; }

        public bool SendApplicationStatusChanges { get; set; }

        public bool SendApprenticeshipApplicationsExpiring { get; set; }

        public bool SendMarketingCommunications { get; set; }

        public bool SendSavedSearchAlertsViaEmail { get; set; }

        public bool SendSavedSearchAlertsViaText { get; set; }
    }
}
