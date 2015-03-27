namespace SFA.Apprenticeships.Web.ContactForms.AcceptanceTests.Pages
{
    using OpenQA.Selenium;
    using SpecBind.Pages;

    [PageNavigation("/gla-employer-enquiry")]
    [PageAlias("ThankYouPageGlaEmployerEnquiry")]
    public class ThankYouPageGlaEmployerEnquiry : BaseValidationPage
    {
        public ThankYouPageGlaEmployerEnquiry(ISearchContext context)
            : base(context)
        {
        }

        [ElementLocator(Id = "ThankYouLabel")]
        public IWebElement ThankYouLabel { get; set; }


        [ElementLocator(Id = "SuccessMessageText")]
        public IWebElement SuccessMessageLabel { get; set; }


    }
}