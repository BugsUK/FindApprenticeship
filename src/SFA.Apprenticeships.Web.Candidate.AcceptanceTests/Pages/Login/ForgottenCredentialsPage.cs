namespace SFA.Apprenticeships.Web.Candidate.AcceptanceTests.Pages.Login
{
    using OpenQA.Selenium;
    using SpecBind.Pages;

    [PageNavigation("/login/forgottencredentials")]
    [PageAlias("ForgottenCredentialsPage")]
    public class ForgottenCredentialsPage : BaseValidationPage
    {
        public ForgottenCredentialsPage(ISearchContext context)
            : base(context)
        {
        }

        [ElementLocator(Id = "ForgottenPasswordViewModel_EmailAddress")]
        public IWebElement EmailAddress { get; set; }

        [ElementLocator(Id = "forgottenpassword-button")]
        public IWebElement SendCodeButton { get; set; }

        [ElementLocator(Text = "enter it")]
        public IWebElement UnlockAccountLink { get; set; }
    }
}