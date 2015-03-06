namespace SFA.Apprenticeships.Web.ContactForms.AcceptanceTests.Pages
{
    using OpenQA.Selenium;
    using SpecBind.Pages;
    using SpecBind.Selenium;
    using Templates.EditorFor;

    [PageNavigation("/employerenquiry")]
    [PageAlias("EmployerEnquiryPage")]
    public class EmployerEnquiryPage : BaseValidationPage
    {
        private IElementList<IWebElement, AddressDropdownItem> _addressDropdown;
        private IElementList<IWebElement, EmployeeCountDropdownItem> _employeeCountDropdown;
        private IElementList<IWebElement, EnquirySourceDropdownItem> _enquirySourceDropdown;
        private IElementList<IWebElement, PrevExperienceDropdownItem> _prevExperienceDropdown;
        private IElementList<IWebElement, WorkSectorDropdownItem> _workSectorDropdown;

        public EmployerEnquiryPage(ISearchContext context)
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

        [ElementLocator(Id = "WorkPhoneNumber")]
        public IWebElement WorkPhoneNumber { get; set; }

        [ElementLocator(Id = "MobileNumber")]
        public IWebElement MobileNumber { get; set; }

        [ElementLocator(Id = "Companyname")]
        public IWebElement Companyname { get; set; }

        [ElementLocator(Id = "Position")]
        public IWebElement Position { get; set; }

        [ElementLocator(Id = "EmployeesCount")]
        public IWebElement EmployeesCount { get; set; }

        [ElementLocator(Id = "WorkSector")]
        public IWebElement WorkSector { get; set; }

        [ElementLocator(Id = "EnquiryDescription")]
        public IWebElement EnquiryDescription { get; set; }

        [ElementLocator(Id = "PreviousExperienceType")]
        public IWebElement PreviousExperienceType { get; set; }

        [ElementLocator(Id = "EnquirySource")]
        public IWebElement EnquirySource { get; set; }

        [ElementLocator(Id = "submit-query-button")]
        public IWebElement SendEmployerEnquiryButton { get; set; }

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
        [ElementLocator(Id = "EmployeesCount")]
        public IElementList<IWebElement, EmployeeCountDropdownItem> EmployeeCountDropdown
        {
            get { return _employeeCountDropdown; }
            set { _employeeCountDropdown = value; }
        }

        [ElementLocator(Id = "EnquirySource")]
        public IElementList<IWebElement, EnquirySourceDropdownItem> EnquirySourceDropdown
        {
            get { return _enquirySourceDropdown; }
            set { _enquirySourceDropdown = value; }
        }

        [ElementLocator(Id = "PreviousExperienceType")]
        public IElementList<IWebElement, PrevExperienceDropdownItem> PrevExperienceDropdown
        {
            get { return _prevExperienceDropdown; }
            set { _prevExperienceDropdown = value; }
        }

        [ElementLocator(Id = "WorkSector")]
        public IElementList<IWebElement, WorkSectorDropdownItem> WorkSectorDropdown
        {
            get { return _workSectorDropdown; }
            set { _workSectorDropdown = value; }
        }
    }

    [ElementLocator(TagName = "option")]
    public class EmployeeCountDropdownItem : WebElement
    {
        protected internal EmployeeCountDropdownItem(ISearchContext searchContext) : base(searchContext)
        {
        }
    }

    [ElementLocator(TagName = "option")]
    public class EnquirySourceDropdownItem : WebElement
    {
        protected internal EnquirySourceDropdownItem(ISearchContext searchContext)
            : base(searchContext)
        {
        }
    }

    [ElementLocator(TagName = "option")]
    public class PrevExperienceDropdownItem : WebElement
    {
        protected internal PrevExperienceDropdownItem(ISearchContext searchContext)
            : base(searchContext)
        {
        }
    }

    [ElementLocator(TagName = "option")]
    public class WorkSectorDropdownItem : WebElement
    {
        protected internal WorkSectorDropdownItem(ISearchContext searchContext)
            : base(searchContext)
        {
        }
    }

    [ElementLocator(TagName = "option")]
    public class AddressDropdownItem : WebElement
    {
        public AddressDropdownItem(ISearchContext parent)
            : base(parent)
        {
        }

        public string AddressLine1 { get { return this.GetAttribute("data-address-line1"); } }
        public string AddressLine2 { get { return this.GetAttribute("data-address-line2"); } }
        public string AddressLine3 { get { return this.GetAttribute("data-address-line3"); } }
        public string AddressLine4 { get { return this.GetAttribute("data-city"); } }
        public string Postcode { get { return this.GetAttribute("data-post-code"); } }
        public string Latitude { get { return this.GetAttribute("data-lat"); } }
        public string Longitude { get { return this.GetAttribute("data-lon"); } }
    }
}
