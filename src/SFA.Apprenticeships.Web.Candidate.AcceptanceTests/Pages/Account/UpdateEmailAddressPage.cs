using System;

namespace SFA.Apprenticeships.Web.Candidate.AcceptanceTests.Pages.Account
{
    using OpenQA.Selenium;
    using SpecBind.Pages;

    [PageNavigation("/updateemail")]
    [PageAlias("UpdateEmailAddressPage")]
    public class UpdateEmailAddressPage : BaseValidationPage
    {
        public UpdateEmailAddressPage(ISearchContext context)
            : base(context)
        {
        }

        [ElementLocator(Id = "EmailAddress")]
        public IWebElement NewUsername { get; set; }

        [ElementLocator(Id = "btn-sendcode")]
        public IWebElement ChangeUsernameButton { get; set; }

    }
}
