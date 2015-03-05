namespace SFA.Apprenticeships.Web.ContactForms.AcceptanceTests.Pages
{
    using OpenQA.Selenium;
    using SpecBind.Pages;

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