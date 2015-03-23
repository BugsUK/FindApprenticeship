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
        private string _mobileVerificationCode;
        private bool _allowEmail;
        private bool _allowMobile;
        private bool _verifiedMobile;
        private bool _allowTraineeshipPrompts;

        private bool _sendApplicationSubmitted;
        private bool _sendApplicationStatusChanges;
        private bool _sendApprenticeshipApplicationsExpiring;
        private bool _sendSavedSearchAlerts;
        private bool _sendMarketingComms;

        public CandidateBuilder(Guid candidateId)
        {
            _candidateId = candidateId;
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

        public CandidateBuilder SendApplicationSubmitted(bool sendApplicationSubmitted)
        {
            _sendApplicationSubmitted = sendApplicationSubmitted;
            return this;
        }

        public CandidateBuilder SendApplicationStatusChanges(bool sendApplicationStatusChanges)
        {
            _sendApplicationStatusChanges = sendApplicationStatusChanges;
            return this;
        }

        public CandidateBuilder SendApprenticeshipApplicationsExpiring(bool sendApprenticeshipApplicationsExpiring)
        {
            _sendApprenticeshipApplicationsExpiring = sendApprenticeshipApplicationsExpiring;
            return this;
        }

        public CandidateBuilder SendSavedSearchAlerts(bool sendSavedSearchAlerts)
        {
            _sendSavedSearchAlerts = sendSavedSearchAlerts;
            return this;
        }

        public CandidateBuilder SendMarketingComms(bool sendMarketingComms)
        {
            _sendMarketingComms = sendMarketingComms;
            return this;
        }

        public CandidateBuilder AllowAllCommunications()
        {
            AllowEmail(true);
            AllowMobile(true);
            VerifiedMobile(true);
            SendApplicationSubmitted(true);
            SendApplicationStatusChanges(true);
            SendApprenticeshipApplicationsExpiring(true);
            SendSavedSearchAlerts(true);

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
                    PhoneNumber = _phoneNumber
                },
                CommunicationPreferences = new CommunicationPreferences
                {
                    AllowEmail = _allowEmail,
                    AllowMobile = _allowMobile,
                    VerifiedMobile = _verifiedMobile,
                    MobileVerificationCode = _mobileVerificationCode,
                    AllowTraineeshipPrompts = _allowTraineeshipPrompts,

                    SendApplicationSubmitted = _sendApplicationSubmitted,
                    SendApplicationStatusChanges = _sendApplicationStatusChanges,
                    SendApprenticeshipApplicationsExpiring = _sendApprenticeshipApplicationsExpiring,
                    SendSavedSearchAlertsViaEmail = _sendSavedSearchAlerts,
                    SendSavedSearchAlertsViaText = _sendSavedSearchAlerts,
                    SendMarketingCommunications = _sendMarketingComms
                }
            };

            return candidate;
        }
    }
}