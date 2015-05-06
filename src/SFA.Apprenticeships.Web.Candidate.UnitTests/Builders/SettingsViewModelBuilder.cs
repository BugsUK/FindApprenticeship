﻿namespace SFA.Apprenticeships.Web.Candidate.UnitTests.Builders
{
    using System.Collections.Generic;
    using Candidate.ViewModels;
    using Candidate.ViewModels.Account;
    using Candidate.ViewModels.Applications;
    using Candidate.ViewModels.Candidate;

    public class SettingsViewModelBuilder
    {
        private string _firstname = "First";
        private string _lastname = "Last";
        private string _phoneNumber = "0123456789";
        private bool _verifiedMobile;
        private bool _showTraineeshipsLink;
        private bool _showTraineeshipsPrompt;
        private bool _smsEnabled;

        private bool _enableApplicationStatusChangeAlertsViaEmail;
        private bool _enableApplicationStatusChangeAlertsViaText;

        private bool _enableExpiringApplicationAlertsViaEmail;
        private bool _enableExpiringApplicationAlertsViaText;

        private bool _enableMarketingViaEmail;
        private bool _enableMarketingViaText;

        private bool _enableSavedSearchAlertsViaEmail;
        private bool _enableSavedSearchAlertsViaText;

        private int? _ethnicity;
        private int? _gender;
        private int? _disabilityStatus;
        private string _anythingWeCanDoToSupportYourInterview;

        private IList<SavedSearchViewModel> _savedSearches;

        public SettingsViewModelBuilder Firstname(string firstname)
        {
            _firstname = firstname;
            return this;
        }

        public SettingsViewModelBuilder Lastname(string lastname)
        {
            _lastname = lastname;
            return this;
        }

        public SettingsViewModelBuilder PhoneNumber(string phoneNumber)
        {
            _phoneNumber = phoneNumber;
            return this;
        }

        public SettingsViewModelBuilder VerifiedMobile(bool verifiedMobile)
        {
            _verifiedMobile = verifiedMobile;
            return this;
        }

        public SettingsViewModelBuilder ShowTraineeshipsLink(bool showTraineeshipsLink)
        {
            _showTraineeshipsLink = showTraineeshipsLink;
            return this;
        }

        public SettingsViewModelBuilder ShowTraineeshipsPrompt(bool showTraineeshipsPrompt)
        {
            _showTraineeshipsPrompt = showTraineeshipsPrompt;
            return this;
        }

        public SettingsViewModelBuilder SmsEnabled(bool smsEnabled)
        {
            _smsEnabled = smsEnabled;
            return this;
        }

        public SettingsViewModelBuilder EnableApplicationStatusChangeAlertsViaEmail(bool enable)
        {
            _enableApplicationStatusChangeAlertsViaEmail = enable;
            return this;
        }

        public SettingsViewModelBuilder EnableApplicationStatusChangeAlertsViaText(bool enable)
        {
            _enableApplicationStatusChangeAlertsViaText = enable;
            return this;
        }

        public SettingsViewModelBuilder EnableExpiringApplicationAlertsViaEmail(bool enable)
        {
            _enableExpiringApplicationAlertsViaEmail = enable;
            return this;
        }

        public SettingsViewModelBuilder EnableExpiringApplicationAlertsViaText(bool enable)
        {
            _enableExpiringApplicationAlertsViaText = enable;
            return this;
        }

        public SettingsViewModelBuilder EnableMarketingViaEmail(bool enable)
        {
            _enableMarketingViaEmail = enable;
            return this;
        }

        public SettingsViewModelBuilder EnableMarketingViaText(bool enable)
        {
            _enableMarketingViaText = enable;
            return this;
        }

        public SettingsViewModelBuilder EnableSavedSearchAlertsViaEmail(bool enable)
        {
            _enableSavedSearchAlertsViaEmail = enable;
            return this;
        }

        public SettingsViewModelBuilder EnableSavedSearchAlertsViaText(bool enable)
        {
            _enableSavedSearchAlertsViaText = enable;
            return this;
        }

        public SettingsViewModelBuilder WithSavedSearchViewModels(IList<SavedSearchViewModel> savedSearches)
        {
            _savedSearches = savedSearches;
            return this;
        }

        public SettingsViewModelBuilder EnableAnyTextCommunication(bool enable)
        {
            _enableSavedSearchAlertsViaText = enable;
            return this;
        }

        public SettingsViewModelBuilder Ethnicity(int? ethnicity)
        {
            _ethnicity = ethnicity;
            return this;
        }

        public SettingsViewModelBuilder Gender(int? gender)
        {
            _gender = gender;
            return this;
        }

        public SettingsViewModelBuilder DisabilityStatus(int? disabilityStatus)
        {
            _disabilityStatus = disabilityStatus;
            return this;
        }

        public SettingsViewModelBuilder AnythingWeCanDoToSupportYourInterview(string anythingWeCanDoToSupportYourInterview)
        {
            _anythingWeCanDoToSupportYourInterview = anythingWeCanDoToSupportYourInterview;
            return this;
        }

        public SettingsViewModel Build()
        {
            var model = new SettingsViewModel
            {
                Firstname = _firstname,
                Lastname = _lastname,
                DateOfBirth = new DateViewModel
                {
                    Day = 01,
                    Month = 01,
                    Year = 1985
                },
                PhoneNumber = _phoneNumber,
                VerifiedMobile = _verifiedMobile,
                TraineeshipFeature = new TraineeshipFeatureViewModel
                {
                    ShowTraineeshipsLink = _showTraineeshipsLink,
                    ShowTraineeshipsPrompt = _showTraineeshipsPrompt
                },
                SmsEnabled = _smsEnabled,

                EnableApplicationStatusChangeAlertsViaEmail = _enableApplicationStatusChangeAlertsViaEmail,
                EnableApplicationStatusChangeAlertsViaText = _enableApplicationStatusChangeAlertsViaText,

                EnableExpiringApplicationAlertsViaEmail = _enableExpiringApplicationAlertsViaEmail,
                EnableExpiringApplicationAlertsViaText = _enableExpiringApplicationAlertsViaText,

                EnableMarketingViaEmail = _enableMarketingViaEmail,
                EnableMarketingViaText = _enableMarketingViaText,

                EnableSavedSearchAlertsViaEmail = _enableSavedSearchAlertsViaEmail,
                EnableSavedSearchAlertsViaText = _enableSavedSearchAlertsViaText,

                MonitoringInformation = new MonitoringInformationViewModel
                {
                    Ethnicity = _ethnicity,
                    Gender = _gender,
                    DisabilityStatus = _disabilityStatus,
                    AnythingWeCanDoToSupportYourInterview = _anythingWeCanDoToSupportYourInterview,
                    RequiresSupportForInterview = !string.IsNullOrWhiteSpace(_anythingWeCanDoToSupportYourInterview)
                },

                SavedSearches = _savedSearches
            };

            return model;
        }
    }
}