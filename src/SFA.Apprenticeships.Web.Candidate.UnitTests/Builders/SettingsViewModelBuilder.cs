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
        private string _phoneNumber = "0123456789";
        private bool _verifiedMobile;
        private bool _showTraineeshipsLink;
        private bool _showTraineeshipsPrompt;
        private bool _smsEnabled;

        private bool _enableApplicationStatusChangeAlertsViaEmail;
        private bool _enableExpiringApplicationAlertsViaEmail;
        private bool _enableMarketingViaEmail;

        private bool _enableSavedSearchAlertsViaEmail;
        private bool _enableSavedSearchAlertsViaText;

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

        public SettingsViewModelBuilder EnableExpiringApplicationAlertsViaEmail(bool enable)
        {
            _enableExpiringApplicationAlertsViaEmail = enable;
            return this;
        }

        public SettingsViewModelBuilder EnableMarketingViaEmail(bool enable)
        {
            _enableMarketingViaEmail = enable;
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

        // TODO: AG: US733: remove and replace with specific properties.
        public SettingsViewModelBuilder EnableAnyTextCommunication(bool enable)
        {
            _enableSavedSearchAlertsViaText = enable;
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
                EnableExpiringApplicationAlertsViaEmail = _enableExpiringApplicationAlertsViaEmail,
                EnableMarketingViaEmail = _enableMarketingViaEmail,

                EnableSavedSearchAlertsViaEmail = _enableSavedSearchAlertsViaEmail,
                EnableSavedSearchAlertsViaText = _enableSavedSearchAlertsViaText,

                SavedSearches = _savedSearches
            };

            return model;
        }
    }
}