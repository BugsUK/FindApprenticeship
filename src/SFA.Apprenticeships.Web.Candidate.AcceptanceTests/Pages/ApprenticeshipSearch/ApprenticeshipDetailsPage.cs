namespace SFA.Apprenticeships.Web.Candidate.AcceptanceTests.Pages.ApprenticeshipSearch
{
    using OpenQA.Selenium;
    using SpecBind.Pages;

    [PageNavigation("/apprenticeship/[0-9]+", UrlTemplate = "/apprenticeship/{VacancyId}")]
    [PageAlias("ApprenticeshipDetailsPage")]
    public class ApprenticeshipDetailsPage
    {
        [ElementLocator(Id = "lnk-return-search-results")]
        public IWebElement ReturnToSearchResultsLink { get; set; }

        [ElementLocator(Id = "lnk-find-apprenticeship")]
        public IWebElement FindApprenticeshipLink { get; set; }

        [ElementLocator(Id = "apply-button")]
        public IWebElement ApplyButton { get; set; }

        [ElementLocator(Id="external-employer-website")]
        public IWebElement ApplyExternalLink { get; set; }

        [ElementLocator(TagName = "h1")]
        public IWebElement ApprenticeshipNoLongerAvailableHeading { get; set; }

        [ElementLocator(Text = "Track application status")]
        public IWebElement TrackApplicationStatusLink { get; set; }
    }
}