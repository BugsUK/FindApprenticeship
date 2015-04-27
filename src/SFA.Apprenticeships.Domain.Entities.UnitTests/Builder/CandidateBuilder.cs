namespace SFA.Apprenticeships.Domain.Entities.UnitTests.Builder
{
    using System;
    using Entities.Candidates;
    using Users;

    public class CandidateBuilder
    {
        private readonly Guid _candidateId;
        private string _firstName;
        private string _phoneNumber;
        private string _emailAddress;
        private string _mobileVerificationCode;
        private bool _allowEmail;
        private bool _allowMobile;
        private bool _verifiedMobile;
        private bool _allowTraineeshipPrompts;
        private bool _sendApplicationStatusChanges;
        private bool _sendApplicationStatusChangesViaEmail;
        private bool _sendApprenticeshipApplicationsExpiring;
        private bool _sendApprenticeshipApplicationsExpiringViaEmail;
        private bool _sendSavedSearchAlertsViaEmail;
        private bool _sendSavedSearchAlertsViaText;
        private bool _sendMarketingComms;
        private HelpPreferences _helpPreferences;

        public CandidateBuilder(Guid candidateId)
        {
            _candidateId = candidateId;
            _helpPreferences = new HelpPreferences();
        }

        public CandidateBuilder FirstName(string firstName)
        {
            _firstName = firstName;
            return this;
        }

        public CandidateBuilder PhoneNumber(string phoneNumber)
        {
            _phoneNumber = phoneNumber;
            return this;
        }

        public CandidateBuilder EmailAddress(string emailAddress)
        {
            _emailAddress = emailAddress;
            return this;
        }

        public CandidateBuilder AllowEmail(bool allowEmail)
        {
            _allowEmail = allowEmail;
            return this;
        }

        public CandidateBuilder AllowMobile(bool allowMobile)
        {
            _allowMobile = allowMobile;
            return this;
        }

        public CandidateBuilder VerifiedMobile(bool verifiedMobile)
        {
            _verifiedMobile = verifiedMobile;
            return this;
        }

        public CandidateBuilder MobileVerificationCode(string  mobileVerificationCode)
        {
            _mobileVerificationCode = mobileVerificationCode;
            return this;
        }

        public CandidateBuilder AllowTraineeshipPrompts(bool allowTraineeshipPrompts)
        {
            _allowTraineeshipPrompts = allowTraineeshipPrompts;
            return this;
        }

        public CandidateBuilder SendApplicationStatusChanges(bool sendApplicationStatusChanges)
        {
            _sendApplicationStatusChanges = sendApplicationStatusChanges;
            return this;
        }

        public CandidateBuilder SendApplicationStatusChangesViaEmail(bool sendApplicationStatusChangesViaEmail)
        {
            _sendApplicationStatusChangesViaEmail = sendApplicationStatusChangesViaEmail;
            return this;
        }

        public CandidateBuilder SendApprenticeshipApplicationsExpiring(bool sendApprenticeshipApplicationsExpiring)
        {
            _sendApprenticeshipApplicationsExpiring = sendApprenticeshipApplicationsExpiring;
            return this;
        }

        public CandidateBuilder SendApprenticeshipApplicationsExpiringViaEmail(bool sendApprenticeshipApplicationsExpiringViaEmail)
        {
            _sendApprenticeshipApplicationsExpiringViaEmail = sendApprenticeshipApplicationsExpiringViaEmail;
            return this;
        }

        public CandidateBuilder SendSavedSearchAlerts(bool sendSavedSearchAlerts)
        {
            SendSavedSearchAlertsViaEmail(sendSavedSearchAlerts);
            SendSavedSearchAlertsViaText(sendSavedSearchAlerts);
            return this;
        }

        public CandidateBuilder SendSavedSearchAlertsViaEmail(bool sendSavedSearchAlertsViaEmail)
        {
            _sendSavedSearchAlertsViaEmail = sendSavedSearchAlertsViaEmail;
            return this;
        }

        public CandidateBuilder SendSavedSearchAlertsViaText(bool sendSavedSearchAlertsViaText)
        {
            _sendSavedSearchAlertsViaText = sendSavedSearchAlertsViaText;
            return this;
        }

        public CandidateBuilder SendMarketingComms(bool sendMarketingComms)
        {
            _sendMarketingComms = sendMarketingComms;
            return this;
        }

        public CandidateBuilder AllowAllCommunications(bool allow = true)
        {
            AllowEmail(allow);
            AllowMobile(allow);
            VerifiedMobile(allow);
            SendApplicationStatusChanges(allow);
            SendApprenticeshipApplicationsExpiring(allow);
            SendSavedSearchAlerts(allow);

            return this;
        }

        public CandidateBuilder With(HelpPreferences helpPreferences)
        {
            _helpPreferences = helpPreferences;
            return this;
        }

        public Candidate Build()
        {
            var candidate = new Candidate
            {
                EntityId = _candidateId,
                RegistrationDetails = new RegistrationDetails
                {
                    FirstName = _firstName,
                    PhoneNumber = _phoneNumber,
                    EmailAddress = _emailAddress
                },
                CommunicationPreferences = new CommunicationPreferences
                {
                    AllowEmail = _allowEmail,
                    AllowMobile = _allowMobile,
                    VerifiedMobile = _verifiedMobile,
                    MobileVerificationCode = _mobileVerificationCode,
                    AllowTraineeshipPrompts = _allowTraineeshipPrompts,

                    SendApplicationStatusChanges = _sendApplicationStatusChanges,
                    SendApplicationStatusChangesViaEmail = _sendApplicationStatusChangesViaEmail,
                    SendApprenticeshipApplicationsExpiring = _sendApprenticeshipApplicationsExpiring,
                    SendApprenticeshipApplicationsExpiringViaEmail = _sendApprenticeshipApplicationsExpiringViaEmail,
                    SendSavedSearchAlertsViaEmail = _sendSavedSearchAlertsViaEmail,
                    SendSavedSearchAlertsViaText = _sendSavedSearchAlertsViaText,
                    SendMarketingCommunications = _sendMarketingComms
                },
                HelpPreferences = _helpPreferences
            };

            return candidate;
        }
    }
}