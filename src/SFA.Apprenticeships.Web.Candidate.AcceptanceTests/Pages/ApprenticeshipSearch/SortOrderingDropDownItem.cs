namespace SFA.Apprenticeships.Web.Candidate.AcceptanceTests.Pages.ApprenticeshipSearch
{
    using OpenQA.Selenium;
    using SpecBind.Pages;
    using SpecBind.Selenium;

    [ElementLocator(TagName = "option")]
    public class SortOrderingDropDownItem : WebElement
    {
        protected internal SortOrderingDropDownItem(ISearchContext searchContext) : base(searchContext)
        {
        }
    }
}