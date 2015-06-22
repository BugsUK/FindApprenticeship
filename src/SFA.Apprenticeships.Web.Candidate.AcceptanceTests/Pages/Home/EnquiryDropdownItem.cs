namespace SFA.Apprenticeships.Web.Candidate.AcceptanceTests.Pages.Home
{
    using OpenQA.Selenium;
    using SpecBind.Pages;
    using SpecBind.Selenium;

    [ElementLocator(TagName = "option")]
    public class EnquiryDropdownItem : WebElement
    {
        public EnquiryDropdownItem(ISearchContext parent)
            : base(parent)
        {
        }
    }
}