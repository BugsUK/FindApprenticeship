namespace SFA.Apprenticeships.Web.ContactForms.AcceptanceTests.Pages
{
    using OpenQA.Selenium;
    using SpecBind.Pages;

    [PageNavigation("/employer-enquiry")]
    [PageAlias("ThankYouPageEmployerEnquiry")]
    public class ThankYouPageEmployerEnquiry : BaseValidationPage
    {
        public ThankYouPageEmployerEnquiry(ISearchContext context)
            : base(context)
        {
        }

        [ElementLocator(Id = "ThankYouLabel")]
        public IWebElement ThankYouLabel { get; set; }


        [ElementLocator(Id = "SuccessMessageText")]
        public IWebElement SuccessMessageLabel { get; set; }


    }
}