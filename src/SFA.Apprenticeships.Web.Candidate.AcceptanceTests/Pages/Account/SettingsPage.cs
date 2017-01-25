namespace SFA.Apprenticeships.Web.Candidate.AcceptanceTests.Pages.Account
{
    using OpenQA.Selenium;
    using Registration;
    using SpecBind.Pages;
    using Templates.EditorFor;

    [PageNavigation("/settings")]
    [PageAlias("SettingsPage")]
    public class SettingsPage : BaseValidationPage
    {
        public SettingsPage(ISearchContext context)
            : base(context)
        {
        }

        public string ClearAllSettings
        {
            get
            {
                Firstname.Clear();
                Lastname.Clear();
                Day.Clear();
                Month.Clear();
                Year.Clear();
                Phonenumber.Clear();
                AddressLine1.Clear();
                AddressLine2.Clear();
                AddressLine3.Clear();
                AddressLine4.Clear();
                Postcode.Clear();

                return "Done";
            }
        }

        [ElementLocator(Id = "Firstname")]
        public IWebElement Firstname { get; set; }

        [ElementLocator(Id = "Lastname")]
        public IWebElement Lastname { get; set; }

        [ElementLocator(Id = "PhoneNumber")]
        public IWebElement Phonenumber { get; set; }

        [ElementLocator(Id = "update-details-button")]
        public IWebElement UpdateDetailsButton { get; set; }

        #region Date of birth

        [ElementLocator(Class = "form-date")]
        public DateOfBirthTemplate DateOfBirth { get; set; }

        public IWebElement Day { get { return DateOfBirth.Day; } }

        public IWebElement Month { get { return DateOfBirth.Month; } }

        public IWebElement Year { get { return DateOfBirth.Year; } }

        #endregion

        #region Address Template

        [ElementLocator(Id = "address-details")]
        public AddressTemplate Address { get; set; }

        public IWebElement AddressLine1 { get { return Address.AddressLine1; } }

        public IWebElement AddressLine2 { get { return Address.AddressLine2; } }

        public IWebElement AddressLine3 { get { return Address.AddressLine3; } }

        public IWebElement AddressLine4 { get { return Address.AddressLine4; } }

        public IWebElement Postcode { get { return Address.Postcode; } }

        public string Uprn { get { return Address.Uprn.GetAttribute("value"); } }

        public string Latitude { get { return Address.Latitude.GetAttribute("value"); } }

        public string Longitude { get { return Address.Longitude.GetAttribute("value"); } }

        #region Search Inputs

        [ElementLocator(Id = "postcode-search")]
        public IWebElement PostcodeSearch { get; set; }

        [ElementLocator(Id = "ui-id-1")]
        public IElementList<IWebElement, AddressDropdownItem> AddressDropdown { get; set; }

        [ElementLocator(Id = "ui-id-1")]
        public IWebElement Addresses { get; set; }

        #endregion

        #endregion

        #region Update Username

        [ElementLocator(Id = "settings-username")]
        public IWebElement Username { get; set; }

        [ElementLocator(Id = "settings-change-username")]
        public IWebElement ChangeUsernameLink { get; set; }

        [ElementLocator(Id = "settings-pending-username")]
        public IWebElement PendingUsername { get; set; }

        [ElementLocator(Id = "settings-confirm-username")]
        public IWebElement ConfirmPendingUsernameLink { get; set; }

        #endregion

        #region Monitoring Information

        #region Gender

        [ElementLocator(Id = "gender-male")]
        public IWebElement GenderMale { get; set; }

        [ElementLocator(Id = "gender-female")]
        public IWebElement GenderFemale { get; set; }

        [ElementLocator(Id = "gender-other")]
        public IWebElement GenderOther { get; set; }

        [ElementLocator(Id = "gender-prefer-not-to-say")]
        public IWebElement GenderPreferNotToSay { get; set; }

        #endregion

        #region Disability

        [ElementLocator(Id = "equality-diversity-summary-link")]
        public IWebElement ShowDiversity { get; set; }

        [ElementLocator(Id = "disability-yes")]
        public IWebElement DisabilityYes { get; set; }

        [ElementLocator(Id = "disability-no")]
        public IWebElement DisabilityNo { get; set; }

        [ElementLocator(Id = "disability-prefno")]
        public IWebElement DisabilityPreferNotToSay { get; set; }

        [ElementLocator(Id = "disability-support-yes")]
        public IWebElement WantSupportInInterview { get; set; }

        [ElementLocator(Id = "disability-support-yes")]
        public IWebElement DontWantSupportInInterview { get; set; }

        [ElementLocator(Id = "MonitoringInformation_AnythingWeCanDoToSupportYourInterview")]
        public IWebElement SupportDetails { get; set; }

        #endregion

        #region Ethnicity

        [ElementLocator(Id = "MonitoringInformation_Ethnicity")]
        public IWebElement EthnicDropdown { get; set; }

        #endregion

        #endregion

        [ElementLocator(Id = "verifyContainer")]
        public IWebElement VerifyContainer { get; set; }

        [ElementLocator(Id = "EnableApplicationStatusChangeAlertsViaEmail")]
        public IWebElement EnableApplicationStatusChangeAlertsViaEmail { get; set; }

        [ElementLocator(Id = "EnableApplicationStatusChangeAlertsViaText")]
        public IWebElement EnableApplicationStatusChangeAlertsViaText { get; set; }

        [ElementLocator(Id = "EnableExpiringApplicationAlertsViaEmail")]
        public IWebElement EnableExpiringApplicationAlertsViaEmail { get; set; }

        [ElementLocator(Id = "EnableExpiringApplicationAlertsViaText")]
        public IWebElement EnableExpiringApplicationAlertsViaText { get; set; }

        [ElementLocator(Id = "EnableSavedSearchAlertsViaEmail")]
        public IWebElement EnableSavedSearchAlertsViaEmail { get; set; }

        [ElementLocator(Id = "EnableSavedSearchAlertsViaText")]
        public IWebElement EnableSavedSearchAlertsViaText { get; set; }

        [ElementLocator(Id = "EnableMarketingViaEmail")]
        public IWebElement EnableMarketingViaEmail { get; set; }

        [ElementLocator(Id = "EnableMarketingViaText")]
        public IWebElement EnableMarketingViaText { get; set; }

        [ElementLocator(Id = "find-apprenticeship-link")]
        public IWebElement FindApprenticeshipLink { get; set; }

        [ElementLocator(Id = "find-traineeship-link")]
        public IWebElement FindTraineeshipLink { get; set; }
    }
}
