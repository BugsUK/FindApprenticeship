namespace SFA.Apprenticeships.Web.Candidate.AcceptanceTests.Pages.Home
{
    using SpecBind.Pages;
    using OpenQA.Selenium;

    [PageNavigation("/helpdesk")]
    [PageAlias("HelpdeskPage")]
    public class HelpdeskPage : BaseValidationPage
    {
        public HelpdeskPage(ISearchContext context)
            : base(context)
        {
        }

        [ElementLocator(Id = "Name")]
        public IWebElement Name { get; set; }

        [ElementLocator(Id = "Email")]
        public IWebElement Email { get; set; }

        [ElementLocator(Id = "contact-subject")]
        public IElementList<IWebElement, EnquiryDropdownItem> EnquiryDropdown { get; set; }

        [ElementLocator(Id = "Enquiry")]
        public IWebElement Enquiry { get; set; }

        [ElementLocator(Id = "Details")]
        public IWebElement Details { get; set; }

        [ElementLocator(Id = "send-contact-form-button")]
        public IWebElement SendButton { get; set; }
    }
}