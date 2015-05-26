namespace SFA.Apprenticeships.Domain.Entities.Candidates
{
    using System;

    public class CommunicationPreferences
    {
        public CommunicationPreferences()
        {
            VerifiedMobile = false;
            MobileVerificationCode = string.Empty;
            MobileVerificationCodeDateCreated = null;

            AllowTraineeshipPrompts = true;

            ApplicationStatusChangePreferences = new CommunicationPreference
            {
                EnableEmail = true,
                EnableText = false
            };

            ExpiringApplicationPreferences = new CommunicationPreference
            {
                EnableEmail = true,
                EnableText = false
            };

            SavedSearchPreferences = new CommunicationPreference
            {
                EnableEmail = true,
                EnableText = false
            };

            MarketingPreferences = new CommunicationPreference
            {
                EnableEmail = true,
                EnableText = false
            };
        }

        // TODO: DEBT: move mobile verification fields into a new type (MobileVerification?).
        public bool VerifiedMobile { get; set; }

        public string MobileVerificationCode { get; set; }

        public DateTime? MobileVerificationCodeDateCreated { get; set; }

        public bool AllowTraineeshipPrompts { get; set; }

        public CommunicationPreference ApplicationStatusChangePreferences { get; set; }

        public CommunicationPreference ExpiringApplicationPreferences { get; set; }

        public CommunicationPreference SavedSearchPreferences { get; set; }

        public CommunicationPreference MarketingPreferences { get; set; }
    }
}