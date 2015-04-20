using System;

namespace SFA.Apprenticeships.Web.Candidate.AcceptanceTests.Pages.Account
{
    using OpenQA.Selenium;
    using SpecBind.Pages;

    [PageNavigation("/verifyemail")]
    [PageAlias("VerifyEmailAddressPage")]
    public class VerifyEmailAddressPage : BaseValidationPage
    {
        public VerifyEmailAddressPage(ISearchContext context)
            : base(context)
        {
        }

        [ElementLocator(Id = "PendingUsernameCode")]
        public IWebElement PendingUsernameCode { get; set; }

        [ElementLocator(Id = "VerifyPassword")]
        public IWebElement Password { get; set; }

        [ElementLocator(Id = "verify-email-button")]
        public IWebElement VerifyEmailButton { get; set; }
    }
}
