namespace SFA.Apprenticeships.Web.Employer.AcceptanceTests
{
    using OpenQA.Selenium;
    using SpecBind.Pages;

    public class BasePage
    {
        protected readonly ISearchContext Context;
        protected readonly IWebDriver Driver;

        public BasePage(ISearchContext context)
        {
            Context = context;
        }

        public BasePage(IWebDriver driver)
        {
            Driver = driver;
        }

        public BasePage(IWebDriver driver, ISearchContext context)
        {
            Context = context;
            Driver = driver;
        }

        [ElementLocator(Id = "InfoMessageText")]
        public IWebElement InfoMessageText { get; set; }

        [ElementLocator(Id = "SuccessMessageText")]
        public IWebElement SuccessMessageText { get; set; }

        [ElementLocator(Id = "WarningMessageText")]
        public IWebElement WarningMessageText { get; set; }

        [ElementLocator(Id = "ErrorMessageText")]
        public IWebElement ErrorMessageText { get; set; }
    }
}