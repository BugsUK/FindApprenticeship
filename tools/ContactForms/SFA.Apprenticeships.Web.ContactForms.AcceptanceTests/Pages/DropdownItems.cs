using OpenQA.Selenium;
using SpecBind.Pages;
using SpecBind.Selenium;

namespace SFA.Apprenticeships.Web.ContactForms.AcceptanceTests.Pages
{

    [ElementLocator(TagName = "option")]
    public class EmployeeCountDropdownItem : WebElement
    {
        protected internal EmployeeCountDropdownItem(ISearchContext searchContext)
            : base(searchContext)
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
    public class UserTypeDropdownItem : WebElement
    {
        protected internal UserTypeDropdownItem(ISearchContext searchContext)
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