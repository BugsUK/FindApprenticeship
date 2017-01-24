namespace SFA.Apprenticeships.Web.Candidate.AcceptanceTests.Pages.ApprenticeshipSearch
{
    using System;
    using System.Globalization;
    using System.Linq;
    using OpenQA.Selenium;
    using SpecBind.Pages;
    using SpecBind.Selenium;

    [PageNavigation("/apprenticeships")]
    [PageAlias("ApprenticeshipSearchResultPage")]
    public class ApprenticeshipSearchResultPage : BaseValidationPage
    {
        private IWebElement _locationAutoComplete;

        public ApprenticeshipSearchResultPage(ISearchContext context) : base(context)
        {
        }

        [ElementLocator(Class = "heading-xlarge")]
        public IWebElement SearchHeader { get; set; }

        [ElementLocator(Id = "SearchField")]
        public IWebElement SearchField { get; set; }
        
        [ElementLocator(Id = "Keywords")]
        public IWebElement Keywords { get; set; }

        [ElementLocator(Id = "Location")]
        public IWebElement Location { get; set; }

        public string ClearLocation
        {
            get
            {
                Location.Clear();
                return "True";
            }
        }

        [ElementLocator(Id = "loc-within")]
        public IWebElement WithInDistance { get; set; }

        [ElementLocator(Id = "apprenticeship-level")]
        public IWebElement ApprenticeshipLevel { get; set; }

        [ElementLocator(Id = "search-button")]
        public IWebElement Search { get; set; }

        [ElementLocator(Id = "sort-results")]
        public IWebElement SortOrderingDropDown { get; set; }

        [ElementLocator(Id = "sort-results")]
        public IElementList<IWebElement, SortOrderingDropDownItem> SortOrderingDropDownItems { get; set; }

        public string SortOrderingDropDownItemsText
        {
            get { return string.Join(",", SortOrderingDropDownItems.Select(i => i.Text)); }
        }

        public string SortOrderingDropDownItemsCount
        {
            get { return SortOrderingDropDownItems.Count().ToString(CultureInfo.InvariantCulture); }
        }

        [ElementLocator(Id = "results-per-page")]
        public IWebElement ResultsPerPageDropDown { get; set; }

        [ElementLocator(Class = "search-results")]
        public IElementList<IWebElement, ApprenticeshipSearchResultsItem> SearchResults { get; set; }

        [ElementLocator(Class = "next")]
        public IWebElement NextPage { get; set; }

        [ElementLocator(Class = "previous")]
        public IWebElement PreviousPage { get; set; }

        [ElementLocator(Id = "search-no-results-title")]
        public IWebElement NoResultsTitle { get; set; }

        [ElementLocator(Id = "nationwideLocationTypeLink")]
        public IWebElement NationwideLocationTypeLink { get; set; }

        [ElementLocator(Id = "localLocationTypeLink")]
        public IWebElement LocalLocationTypeLink { get; set; }

        [ElementLocator(Class = "ui-autocomplete")]
        public IWebElement LocationAutoComplete
        {
            get { return _locationAutoComplete; }
            set { _locationAutoComplete = value; }
        }

        [ElementLocator(Class = "ui-autocomplete")]
        public IElementList<IWebElement, LocationAutoCompleteItem> LocationAutoCompletItems { get; set; }

        [ElementLocator(Class = "search-results")]
        public IElementList<IWebElement, SearchResultItem> SearchResultItems { get; set; }

        public SearchResultItem FirstResultItem
        {
            get { return SearchResultItems.FirstOrDefault(); }
        }

        public SearchResultItem SecondResultItem
        {
            get
            {
                return SearchResultItems != null && SearchResultItems.Count() >= 2 ? SearchResultItems.Skip(1).First() : null;
            }
        }

        public string SearchResultItemsCount
        {
            get { return SearchResultItems.Count().ToString(CultureInfo.InvariantCulture); }
        }

        [ElementLocator(Id = "location-suggestions")]
        public IWebElement LocationSuggestionsContainer { get; set; }

        [ElementLocator(Id = "location-suggestions")]
        public IElementList<IWebElement, LocationSuggestion> LocationSuggestions { get; set; }

        public string LocationSuggestionsCount
        {
            get { return LocationSuggestions.Count().ToString(CultureInfo.InvariantCulture); }
        }

        [ElementLocator(Id = "search-no-results-apprenticeship-levels")]
        public IWebElement ApprenticeshipLevelAdvice { get; set; }

        [ElementLocator(Id = "search-no-results-reference-number")]
        public IWebElement ReferenceNumberAdvice { get; set; }

        [ElementLocator(Id = "categories")]
        public IWebElement Categories { get; set; }

        [ElementLocator(Id = "categories")]
        public IElementList<IWebElement, CategoryItem> CategoryItems { get; set; }

        public string CategoryItemsCount
        {
            get { return CategoryItems.Count().ToString(CultureInfo.InvariantCulture); }
        }

        [ElementLocator(Id = "ajaxLoading")]
        public IWebElement AjaxLoading { get; set; }

        [ElementLocator(Id = "headerLinkFAA")]
        public IWebElement HeaderLinkFaa { get; set; }
        
        public string ResultsAreInDistanceOrder
        {
            get
            {
                var result = true;
                SearchResultItem previousItem = null;

                for (var i = 0; i < SearchResultItems.Count(); i++)
                {
                    if (previousItem != null)
                    {
                        var currentItem = SearchResultItems.ElementAt(i);
                        var currentDistance = double.Parse(currentItem.Distance.Text);
                        var previousDistance = double.Parse(previousItem.Distance.Text);
                        result = result & currentDistance >= previousDistance;
                    }

                    previousItem = SearchResultItems.ElementAt(i);
                }

                return result.ToString();
            }
        }

        public string ResultsAreInClosingDateOrder
        {
            get
            {
                var result = true;
                SearchResultItem previousItem = null;

                for (var i = 0; i < SearchResultItems.Count(); i++)
                {
                    if (previousItem != null)
                    {
                        var currentItem = SearchResultItems.ElementAt(i);
                        var currrentClosingDate = DateTime.Parse(currentItem.ClosingDate.GetAttribute("data-date"));
                        var previousClosingDate = DateTime.Parse(previousItem.ClosingDate.GetAttribute("data-date"));
                        result = result & currrentClosingDate >= previousClosingDate;
                    }

                    previousItem = SearchResultItems.ElementAt(i);
                }

                return result.ToString();
            }
        }

        public IWebElement FirstVacancyLink
        {
            get
            {
                var item = SearchResultItems.First();
                return item.VacancyLink;
            }
        }

        public IWebElement FirstVacancyId
        {
            get
            {
                var item = SearchResultItems.First();
                return item;
            }
        }
    }

    [ElementLocator(Class = "search-result")]
    public class SearchResultItem : WebElement
    {
        public SearchResultItem(ISearchContext parent) : base(parent)
        {
        }

        public string VacancyId
        {
            get { return VacancyLink.GetAttribute("data-vacancy-id"); }
        }

        [ElementLocator(Class = "vacancy-link")]
        public IWebElement VacancyLink { get; set; }

        [ElementLocator(Class = "vacancy-title-link")]
        public IWebElement Title { get; set; }

        [ElementLocator(Class = "subtitle")]
        public IWebElement Subtitle { get; set; }

        [ElementLocator(Class = "search-shortdesc")]
        public IWebElement ShortDescription { get; set; }

        [ElementLocator(Class = "distance-value")]
        public IWebElement Distance { get; set; }

        [ElementLocator(Class = "closing-date-value")]
        public IWebElement ClosingDate { get; set; }

        public override string Text
        {
            get { return VacancyId; }
        }
    }
}