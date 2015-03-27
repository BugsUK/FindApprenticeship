namespace SFA.Apprenticeships.Web.ContactForms.AcceptanceTests.Pages
{
    using OpenQA.Selenium;
    using SpecBind.Pages;
    using SpecBind.Selenium;
    using Templates.EditorFor;

    [PageNavigation("/access-request")]
    [PageAlias("AccessRequestPage")]
    public class AccessRequestPage : BaseValidationPage
    {
        private IElementList<IWebElement, AddressDropdownItem> _addressDropdown;
        private IElementList<IWebElement, UserTypeDropdownItem> _userTypeDropdownItems;
        public AccessRequestPage(ISearchContext context)
            : base(context)
        {
        }

        [ElementLocator(Id = "Title")]
        public IWebElement Title { get; set; }

        [ElementLocator(Id = "Firstname")]
        public IWebElement Firstname { get; set; }

        [ElementLocator(Id = "Lastname")]
        public IWebElement Lastname { get; set; }

        [ElementLocator(Id = "Email")]
        public IWebElement EmailAddress { get; set; }

        [ElementLocator(Id = "ConfirmEmail")]
        public IWebElement ConfirmEmailAddress { get; set; }

        [ElementLocator(Id = "WorkPhoneNumber")]
        public IWebElement WorkPhoneNumber { get; set; }

        [ElementLocator(Id = "MobileNumber")]
        public IWebElement MobileNumber { get; set; }

        [ElementLocator(Id = "Companyname")]
        public IWebElement Companyname { get; set; }

        [ElementLocator(Id = "Position")]
        public IWebElement Position { get; set; }

        [ElementLocator(Id = "submit-access-request-button")]
        public IWebElement SendAccessRequestButton { get; set; }

        #region Address Template

        [ElementLocator(Id = "address-details")]
        public AddressTemplate Address { get; set; }
        public IWebElement AddressLine1 { get { return Address.AddressLine1; } }

        public IWebElement AddressLine2 { get { return Address.AddressLine2; } }

        public IWebElement AddressLine3 { get { return Address.AddressLine3; } }

        public IWebElement City { get { return Address.City; } }

        public IWebElement Postcode { get { return Address.Postcode; } }

        public string Latitude { get { return Address.Latitude.GetAttribute("value"); } }

        public string Longitude { get { return Address.Longitude.GetAttribute("value"); } }

        #region Search Inputs

        [ElementLocator(Id = "postcode-search")]
        public IWebElement PostcodeSearch { get; set; }

        [ElementLocator(Id = "find-addresses")]
        public IWebElement FindAddresses { get; set; }

        [ElementLocator(Id = "address-select-label")]
        public IWebElement AddressSelectLabel { get; set; }

        [ElementLocator(Id = "address-select")]
        public IElementList<IWebElement, AddressDropdownItem> AddressDropdown
        {
            get { return _addressDropdown; }
            set { _addressDropdown = value; }
        }
        [ElementLocator(Id = "address-select")]
        public IWebElement Addresses { get; set; }

        #endregion

        #endregion

        [ElementLocator(Id = "UserType")]
        public IElementList<IWebElement, UserTypeDropdownItem> UserTypeDropdown
        {
            get { return _userTypeDropdownItems; }
            set { _userTypeDropdownItems = value; }
        }
    }
}
