namespace SFA.Apprenticeships.Web.Candidate.UnitTests.Builders
{
    using System.Collections.Generic;
    using Candidate.ViewModels;
    using Candidate.ViewModels.Account;
    using Candidate.ViewModels.Applications;

    public class SettingsViewModelBuilder
    {
        private string _firstname = "First";
        private string _lastname = "Last";
        private string _phoneNumber;
        private bool _verifiedMobile;
        private bool _allowEmailComms;
        private bool _allowSmsComms;
        private bool _showTraineeshipsLink;
        private bool _showTraineeshipsPrompt;
        private bool _smsEnabled;

        private bool _sendApplicationSubmitted;
        private bool _sendApplicationStatusChanges;
        private bool _sendApprenticeshipApplicationsExpiring;
        private bool _sendMarketingComms;

        private bool _sendSavedSearchAlertsViaEmail;
        private bool _sendSavedSearchAlertsViaText;

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

        public SettingsViewModelBuilder AllowEmailComms(bool allowEmailComms)
        {
            _allowEmailComms = allowEmailComms;
            return this;
        }

        public SettingsViewModelBuilder AllowSmsComms(bool allowSmsComms)
        {
            _allowSmsComms = allowSmsComms;
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

        public SettingsViewModelBuilder SendApplicationSubmitted(bool sendApplicationSubmitted)
        {
            _sendApplicationSubmitted = sendApplicationSubmitted;
            return this;
        }

        public SettingsViewModelBuilder SendApplicationStatusChanges(bool sendApplicationStatusChanges)
        {
            _sendApplicationStatusChanges = sendApplicationStatusChanges;
            return this;
        }

        public SettingsViewModelBuilder SendApprenticeshipApplicationsExpiring(bool sendApprenticeshipApplicationsExpiring)
        {
            _sendApprenticeshipApplicationsExpiring = sendApprenticeshipApplicationsExpiring;
            return this;
        }

        public SettingsViewModelBuilder SendMarketingComms(bool sendMarketingComms)
        {
            _sendMarketingComms = sendMarketingComms;
            return this;
        }

        public SettingsViewModelBuilder SendSavedSearchAlertsViaEmail(bool sendSavedSearchAlertsViaEmail)
        {
            _sendSavedSearchAlertsViaEmail = sendSavedSearchAlertsViaEmail;
            return this;
        }

        public SettingsViewModelBuilder SendSavedSearchAlertsViaText(bool sendSavedSearchAlertsViaText)
        {
            _sendSavedSearchAlertsViaText = sendSavedSearchAlertsViaText;
            return this;
        }

        public SettingsViewModelBuilder WithSavedSearchViewModels(IList<SavedSearchViewModel> savedSearches)
        {
            _savedSearches = savedSearches;
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
                AllowEmailComms = _allowEmailComms,
                AllowSmsComms = _allowSmsComms,
                TraineeshipFeature = new TraineeshipFeatureViewModel
                {
                    ShowTraineeshipsLink = _showTraineeshipsLink,
                    ShowTraineeshipsPrompt = _showTraineeshipsPrompt
                },
                SmsEnabled = _smsEnabled,

                SendApplicationSubmitted = _sendApplicationSubmitted,
                SendApplicationStatusChanges = _sendApplicationStatusChanges,
                SendApprenticeshipApplicationsExpiring = _sendApprenticeshipApplicationsExpiring,
                SendMarketingCommunications = _sendMarketingComms,

                SendSavedSearchAlertsViaEmail = _sendSavedSearchAlertsViaEmail,
                SendSavedSearchAlertsViaText = _sendSavedSearchAlertsViaText,

                SavedSearches = _savedSearches
            };

            return model;
        }
    }
}