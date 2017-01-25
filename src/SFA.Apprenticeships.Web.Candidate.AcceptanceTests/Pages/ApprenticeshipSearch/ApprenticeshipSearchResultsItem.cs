namespace SFA.Apprenticeships.Web.Candidate.AcceptanceTests.Pages.ApprenticeshipSearch
{
    using System.Linq;
    using OpenQA.Selenium;
    using SpecBind.Pages;
    using SpecBind.Selenium;

    [ElementLocator(Class = "search-result")]
    public class ApprenticeshipSearchResultsItem : WebElement
    {
        protected internal ApprenticeshipSearchResultsItem(ISearchContext searchContext) : base(searchContext)
        {
        }

        [ElementLocator(Class = "vacancy-details-list")]
        public IElementList<IWebElement, PropertyListItem> PropertyList { get; set; }

        public string DistanceDisplayed
        {
            get { return PropertyList.Any(i => i.Text.StartsWith("Distance:")).ToString(); }
        }

        public string ClosingDateDisplayed
        {
            get { return PropertyList.Any(i => i.Text.StartsWith("Closing date:")).ToString(); }
        }

        [ElementLocator(Id = "multiple-positions-nationwide")]
        public IWebElement MultiplePositionsNationwide { get; set; }

        public string NationwideDisplayed
        {
            get
            {
                var multiplePositionsNationwideCount = FindElements(By.Id("multiple-positions-nationwide")).Count;

                return (multiplePositionsNationwideCount > 0 && MultiplePositionsNationwide != null && MultiplePositionsNationwide.Text == "This apprenticeship has multiple positions across England.").ToString();
            }
        }
    }

    [ElementLocator(TagName = "li")]
    public class PropertyListItem : WebElement
    {
        protected internal PropertyListItem(ISearchContext searchContext) : base(searchContext)
        {
        }
    }
}