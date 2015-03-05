namespace SFA.Apprenticeships.Web.Employer.AcceptanceTests.Pages
{
    using global::SpecBind.Pages;
    using global::SpecBind.Selenium;
    using OpenQA.Selenium;
    using Templates.EditorFor;

    [PageNavigation("/employerenquiry")]
    [PageAlias("ThankYouPage")]
    public class ThankYouPage : BaseValidationPage
    {
        public ThankYouPage(ISearchContext context)
            : base(context)
        {
        }

        [ElementLocator(Id = "ThankYouLabel")]
        public IWebElement ThankYouLabel { get; set; }


        [ElementLocator(Id = "SuccessMessageText")]
        public IWebElement SuccessMessageLabel { get; set; }


    }
}