﻿namespace SFA.Apprenticeships.Domain.Entities.UnitTests.Builder
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
        private bool _verifiedMobile;
        private DateTime? _mobileVerificationCodeDate;

        private bool _allowTraineeshipPrompts;

        private bool _enableApplicationStatusChangeAlertsViaEmail;
        private bool _enableApplicationStatusChangeAlertsViaText;

        private bool _enableExpiringApplicationAlertsViaText;
        private bool _enableExpiringApplicationAlertsViaEmail;

        private bool _enableSavedSearchAlertsViaEmail;
        private bool _enableSavedSearchAlertsViaText;

        private bool _enableMarketingViaEmail;
        private bool _enableMarketingViaText;

        private HelpPreferences _helpPreferences;
        private ApplicationTemplate _applicationTemplate;

        private Gender? _gender;
        private DisabilityStatus _disabilityStatus;
        private int? _ethnicity;

        private DateTime _dateOfBirth;

        public CandidateBuilder(Guid candidateId)
        {
            _candidateId = candidateId;
            _helpPreferences = new HelpPreferences();
            _applicationTemplate = new ApplicationTemplate();
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

        public CandidateBuilder VerifiedMobile(bool verifiedMobile)
        {
            _verifiedMobile = verifiedMobile;
            return this;
        }

        public CandidateBuilder MobileVerificationCodeDateCreated(DateTime? mobileVerificationCodeDate)
        {
            _mobileVerificationCodeDate = mobileVerificationCodeDate;
            return this;
        }

        public CandidateBuilder MobileVerificationCode(string mobileVerificationCode)
        {
            _mobileVerificationCode = mobileVerificationCode;
            return this;
        }

        public CandidateBuilder AllowTraineeshipPrompts(bool allowTraineeshipPrompts)
        {
            _allowTraineeshipPrompts = allowTraineeshipPrompts;
            return this;
        }

        public CandidateBuilder EnableOneCommunicationPreferenceViaEmail(bool enable)
        {
            EnableApplicationStatusChangeAlertsViaEmail(enable);
            return this;
        }

        public CandidateBuilder EnableOneCommunicationPreferenceViaText(bool enable)
        {
            EnableApplicationStatusChangeAlertsViaText(enable);
            return this;
        }

        public CandidateBuilder EnableApplicationStatusChangeAlertsViaText(bool enabe)
        {
            _enableApplicationStatusChangeAlertsViaText = enabe;
            return this;
        }

        public CandidateBuilder EnableApplicationStatusChangeAlertsViaEmail(bool enable)
        {
            _enableApplicationStatusChangeAlertsViaEmail = enable;
            return this;
        }

        public CandidateBuilder EnableExpiringApplicationAlertsViaText(bool enable)
        {
            _enableExpiringApplicationAlertsViaText = enable;
            return this;
        }

        public CandidateBuilder EnableExpiringApplicationAlertsViaEmail(bool enable)
        {
            _enableExpiringApplicationAlertsViaEmail = enable;
            return this;
        }

        public CandidateBuilder EnableSavedSearchAlertsViaEmailAndText(bool enable)
        {
            EnableSavedSearchAlertsViaEmail(enable);
            EnableSavedSearchAlertsViaText(enable);
            return this;
        }

        public CandidateBuilder EnableSavedSearchAlertsViaEmail(bool enable)
        {
            _enableSavedSearchAlertsViaEmail = enable;
            return this;
        }

        public CandidateBuilder EnableSavedSearchAlertsViaText(bool enable)
        {
            _enableSavedSearchAlertsViaText = enable;
            return this;
        }

        public CandidateBuilder EnableMarketingViaEmail(bool enable)
        {
            _enableMarketingViaEmail = enable;
            return this;
        }

        public CandidateBuilder EnableMarketingViaText(bool enable)
        {
            _enableMarketingViaText = enable;
            return this;
        }

        public CandidateBuilder EnableAllCommunications(bool enable = true)
        {
            VerifiedMobile(enable);

            EnableApplicationStatusChangeAlertsViaEmail(enable);
            EnableApplicationStatusChangeAlertsViaText(enable);

            EnableExpiringApplicationAlertsViaEmail(enable);
            EnableExpiringApplicationAlertsViaText(enable);

            EnableMarketingViaEmail(enable);
            EnableMarketingViaText(enable);

            EnableSavedSearchAlertsViaEmail(enable);
            EnableSavedSearchAlertsViaText(enable);

            return this;
        }

        public CandidateBuilder With(HelpPreferences helpPreferences)
        {
            _helpPreferences = helpPreferences;
            return this;
        }

        public CandidateBuilder With(ApplicationTemplate applicationTemplate)
        {
            _applicationTemplate = applicationTemplate;
            return this;
        }

        public CandidateBuilder With(Gender? gender)
        {
            _gender = gender;
            return this;
        }

        public CandidateBuilder With(DisabilityStatus disabilityStatus)
        {
            _disabilityStatus = disabilityStatus;
            return this;
        }

        public CandidateBuilder With(int? ethnicity)
        {
            _ethnicity = ethnicity;
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
                    EmailAddress = _emailAddress,
                    DateOfBirth = _dateOfBirth
                },
                CommunicationPreferences = new CommunicationPreferences
                {
                    VerifiedMobile = _verifiedMobile,
                    MobileVerificationCode = _mobileVerificationCode,
                    MobileVerificationCodeDateCreated = _mobileVerificationCodeDate,
                    AllowTraineeshipPrompts = _allowTraineeshipPrompts,
                    ApplicationStatusChangePreferences = new CommunicationPreference
                    {
                        EnableEmail = _enableApplicationStatusChangeAlertsViaEmail,
                        EnableText = _enableApplicationStatusChangeAlertsViaText
                    },
                    ExpiringApplicationPreferences = new CommunicationPreference
                    {
                        EnableEmail = _enableExpiringApplicationAlertsViaEmail,
                        EnableText = _enableExpiringApplicationAlertsViaText
                    },
                    SavedSearchPreferences = new CommunicationPreference
                    {
                        EnableEmail = _enableSavedSearchAlertsViaEmail,
                        EnableText = _enableSavedSearchAlertsViaText
                    },
                    MarketingPreferences = new CommunicationPreference
                    {
                        EnableEmail = _enableMarketingViaEmail,
                        EnableText = _enableMarketingViaText
                    }
                },

                HelpPreferences = _helpPreferences,
                ApplicationTemplate = _applicationTemplate,

                MonitoringInformation = new MonitoringInformation
                {
                    Gender = _gender,
                    DisabilityStatus = _disabilityStatus,
                    Ethnicity = _ethnicity
                }
            };

            return candidate;
        }

        public CandidateBuilder WithDateOfBirth(DateTime dateOfBirth)
        {
            _dateOfBirth = dateOfBirth;
            return this;
        }
    }
}