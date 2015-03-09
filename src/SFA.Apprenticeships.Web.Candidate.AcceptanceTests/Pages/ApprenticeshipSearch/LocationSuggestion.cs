namespace SFA.Apprenticeships.Web.Candidate.AcceptanceTests.Pages.ApprenticeshipSearch
{
    using OpenQA.Selenium;
    using SpecBind.Pages;
    using SpecBind.Selenium;

    [ElementLocator(TagName = "li")]
    public class LocationSuggestion : WebElement
    {
        protected internal LocationSuggestion(ISearchContext searchContext) : base(searchContext)
        {
        }
    }
}