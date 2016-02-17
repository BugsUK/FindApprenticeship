namespace SFA.Apprenticeships.Web.Candidate.ViewModels.Account
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Candidate;
    using Common.ViewModels.Applications;
    using Common.ViewModels.Locations;
    using Constants.ViewModels;
    using FluentValidation.Attributes;
    using Validators;

    [Validator(typeof(SettingsViewModelClientValidator))]
    public class SettingsViewModel
    {
        public SettingsViewModel()
        {
            MonitoringInformation = new MonitoringInformationViewModel();
        }

        public enum SettingsMode
        {
            YourAccount,
            SavedSearches
        }

        public SettingsMode Mode { get; set; }

        public string Username { get; set; }

        public string PendingUsername { get; set; }

        [Display(Name = SettingsViewModelMessages.FirstnameMessages.LabelText)]
        public string Firstname { get; set; }

        [Display(Name = SettingsViewModelMessages.LastnameMessages.LabelText)]
        public string Lastname { get; set; }

        public DateViewModel DateOfBirth { get; set; }

        public AddressViewModel Address { get; set; }

        [Display(Name = SettingsViewModelMessages.PhoneNumberMessages.LabelText)]
        public string PhoneNumber { get; set; }

        public bool VerifiedMobile { get; set; }

        public TraineeshipFeatureViewModel TraineeshipFeature { get; set; }

        public bool SmsEnabled { get; set; }

        [Display(Name = "")]
        public bool EnableApplicationStatusChangeAlertsViaEmail { get; set; }

        [Display(Name = "")]
        public bool EnableApplicationStatusChangeAlertsViaText { get; set; }

        [Display(Name = "")]
        public bool EnableExpiringApplicationAlertsViaEmail { get; set; }

        [Display(Name = "")]
        public bool EnableExpiringApplicationAlertsViaText { get; set; }

        [Display(Name = "")]
        public bool EnableMarketingViaEmail { get; set; }

        [Display(Name = "")]
        public bool EnableMarketingViaText { get; set; }

        [Display(Name = SettingsViewModelMessages.SavedSearch.EmailLabelText)]
        public bool EnableSavedSearchAlertsViaEmail { get; set; }

        [Display(Name = SettingsViewModelMessages.SavedSearch.TextLabelText)]
        public bool EnableSavedSearchAlertsViaText { get; set; }

        public IList<SavedSearchViewModel> SavedSearches { get; set; }

        public MonitoringInformationViewModel MonitoringInformation { get; set; }

        public bool IsJavascript { get; set; }
    }
}