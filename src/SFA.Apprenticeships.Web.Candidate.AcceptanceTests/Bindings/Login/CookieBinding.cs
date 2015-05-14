namespace SFA.Apprenticeships.Web.Candidate.AcceptanceTests.Bindings.Login
{
    using System;
    using System.Linq;
    using OpenQA.Selenium;
    using SpecBind.BrowserSupport;
    using TechTalk.SpecFlow;

    [Binding]
    public class CookieBinding
    {
        private readonly IWebDriver _driver;

        public CookieBinding(IBrowser browser)
        {
            _driver = BindingUtils.Driver(browser);
        }

        [Then(@"I have the cookie '(.*)' with a populated value '(.*)'")]
        public void ThenIHaveTheCookieWithAPopulatedValue(string cookieName, string cookieValueName)
        {
            var cookie = _driver.Manage().Cookies.GetCookieNamed(cookieName);

            if (cookie == null)
            {
                throw new Exception(string.Format("Cookie '{0}' does not exist", cookieName));
            }

            var cookieValues = cookie.Value.Split('&');

            if (cookieValues.Any(cookieValue => cookieValue.StartsWith(cookieValueName)))
            {
                return;
            }

            throw new Exception(string.Format("Cookie '{0}' does not have a value for {1}", cookieName, cookieValueName));
        }

        [Then(@"I have the cookie '(.*)' without a value '(.*)'")]
        public void ThenIHaveTheCookieWithoutAValue(string cookieName, string cookieValueName)
        {
            var cookie = _driver.Manage().Cookies.GetCookieNamed(cookieName);

            if (cookie == null)
            {
                throw new Exception(string.Format("Cookie '{0}' does not exist", cookieName));
            }

            var cookieValues = cookie.Value.Split('&');

            if (cookieValues.Any(cookieValue => cookieValue.StartsWith(cookieValueName)))
            {
                throw new Exception(string.Format("Cookie '{0}' does have a value for {1}", cookieName, cookieValueName));
            }
        }

        [Then(@"I do not have the cookie '(.*)'")]
        public void ThenIDoNotHaveTheCookie(string cookieName)
        {
            var cookie = _driver.Manage().Cookies.GetCookieNamed(cookieName);

            if (cookie != null)
            {
                throw new Exception(string.Format("Cookie '{0}' does exist", cookieName));
            }
        }
    }
}