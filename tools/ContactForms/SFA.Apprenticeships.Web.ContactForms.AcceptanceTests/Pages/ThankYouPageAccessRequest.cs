namespace SFA.Apprenticeships.Web.ContactForms.AcceptanceTests.Pages
{
    using OpenQA.Selenium;
    using SpecBind.Pages;

    [PageNavigation("/access-request")]
    [PageAlias("ThankYouPageAccessRequest")]
    public class ThankYouPageAccessRequest: BaseValidationPage
    {
        public ThankYouPageAccessRequest(ISearchContext context)
            : base(context)
        {
        }

        [ElementLocator(Id = "ThankYouLabel")]
        public IWebElement ThankYouLabel { get; set; }


        [ElementLocator(Id = "SuccessMessageText")]
        public IWebElement SuccessMessageLabel { get; set; }


    }
}