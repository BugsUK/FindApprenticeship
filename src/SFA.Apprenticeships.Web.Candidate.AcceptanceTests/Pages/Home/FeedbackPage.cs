namespace SFA.Apprenticeships.Web.Candidate.AcceptanceTests.Pages.Home
{
    using SpecBind.Pages;
    using OpenQA.Selenium;

    [PageNavigation("/feedback")]
    [PageAlias("FeedbackPage")]
    public class FeedbackPage : BaseValidationPage
    {
        public FeedbackPage(ISearchContext context)
            : base(context)
        {
        }

        [ElementLocator(Id = "Name")]
        public IWebElement Name { get; set; }

        [ElementLocator(Id = "Email")]
        public IWebElement Email { get; set; }

        [ElementLocator(Id = "Details")]
        public IWebElement Details { get; set; }

        [ElementLocator(Id = "give-feedback-form-button")]
        public IWebElement GiveFeedbackButton { get; set; }
    }
}
