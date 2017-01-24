using System.Collections.Generic;

namespace SFA.Apprenticeships.Web.Candidate.AcceptanceTests.Bindings
{
    using System;
    using Domain.Interfaces.Repositories;
    using FluentAssertions;
    using Generators;
    using global::SpecBind.Helpers;
    using IoC;
    using OpenQA.Selenium;
    using SpecBind.BrowserSupport;
    using System.Linq;
    using TechTalk.SpecFlow;

    [Binding]
    public class DataGeneratorBinding : Steps
    {   
        private readonly ITokenManager _tokenManager;
        private readonly IUserReadRepository _userReadRepository;
        private readonly ICandidateReadRepository _candidateReadRepository;
        private readonly IWebDriver _driver;
        private readonly Dictionary<string, int> _positions = new Dictionary<string, int>
        {
            {"first", 1},
            {"second", 2}
        };
        private string _email;

        public DataGeneratorBinding(ITokenManager tokenManager, IBrowser browser)
        {
            _tokenManager = tokenManager;
            _userReadRepository = WebTestRegistry.Container.GetInstance<IUserReadRepository>();
            _candidateReadRepository = WebTestRegistry.Container.GetInstance<ICandidateReadRepository>();
            _driver = BindingUtils.Driver(browser);
        }

        [Given("I have created a new email address")]
        [When("I have created a new email address")]
        public void GivenICreateANewUserEmailAddress()
        {
            _email = EmailGenerator.GenerateEmailAddress();
            _tokenManager.SetToken(BindingData.UserEmailAddressTokenName, _email);
            _tokenManager.SetToken(BindingData.InvalidPasswordTokenName, new string(BindingData.Password.Reverse().ToArray()));
        }

        [When("I create a new email address to update my username")]
        public void GivenICreateANewUserEmailAddressToChangeO()
        {
            _email = EmailGenerator.GenerateEmailAddress();
            _tokenManager.SetToken(BindingData.NewEmailAddressTokenName, _email);
        }

        [When(@"I recieve the update email address verification code")]
        public void WhenIRecieveTheUpdateEmailAddressVerificationCode()
        {
            var email = _tokenManager.GetTokenByKey(BindingData.UserEmailAddressTokenName);
            var user = _userReadRepository.Get(email);
            _tokenManager.SetToken(BindingData.NewEmailAddressVerificationCode, user.PendingUsernameCode);
            _tokenManager.SetToken(BindingData.PasswordTokenName, BindingData.Password);
        }


        [Given(@"I select the ""(.*)"" apprenticeship vacancy in location ""(.*)""")]
        [When(@"I select the ""(.*)"" apprenticeship vacancy in location ""(.*)""")]
        public void WhenISelectTheNthApprenticeshipVacancy(string position, string location)
        {
            var expectedPosition = _positions[position];
            SearchForAnApprenticeshipVacancyIn(location);

            SelectVacancyInPosition(expectedPosition);
        }

        [Given(@"I select the ""(.*)"" apprenticeship vacancy in location ""(.*)"" that can apply by this website")]
        [When(@"I select the ""(.*)"" apprenticeship vacancy in location ""(.*)"" that can apply by this website")]
        public void WhenISelectTheNthApprenticeshipVacancyThatCanApplyByThisWebsite(string position, string location)
        {
            var expectedPosition = _positions[position];
            SearchForAnApprenticeshipVacancyIn(location);

            SelectVacancyApplicableViaThisWebsiteInPosition(expectedPosition);
        }

        [Given(@"I select the ""(.*)"" traineeship vacancy in location ""(.*)"" that can apply by this website")]
        [When(@"I select the ""(.*)"" traineeship vacancy in location ""(.*)"" that can apply by this website")]
        public void WhenISelectTheNthTraineeshipVacancyThatCanApplyByThisWebsite(string position, string location)
        {
            var expectedPosition = _positions[position];
            SearchForATraineeshipVacancyIn(location);

            SelectVacancyApplicableViaThisWebsiteInPosition(expectedPosition);
        }

        private void SelectVacancyInPosition(int expectedPosition)
        {
            const int numResults = 50;
            var i = 0;
            var validPositionCount = 0;

            while (validPositionCount != expectedPosition)
            {
                if (i == numResults)
                {
                    throw new Exception("Can't find any suitable apprenticeship vacancy.");
                }

                // Click on the i-th search result
                _driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(180));
                var searchResult = _driver.FindElements(
                    By.CssSelector(".search-result .vacancy-link"))
                    .Skip(i++).First();
                var vacancyId = searchResult.GetAttribute("data-vacancy-id");
                _tokenManager.SetToken(BindingData.VacancyIdToken, vacancyId);
                searchResult.Click();

                var vacancyReference = _driver.FindElement(By.Id("vacancy-reference-id")).Text;
                _tokenManager.SetToken(BindingData.VacancyReferenceToken, vacancyReference);
                validPositionCount++;
                if (validPositionCount != expectedPosition)
                    _driver.Navigate().Back();
            }
        }

        private void SelectVacancyApplicableViaThisWebsiteInPosition(int expectedPosition)
        {
            const int numResults = 50;
            var i = 0;
            var validPositionCount = 0;

            while (validPositionCount != expectedPosition)
            {
                if (i == numResults)
                {
                    throw new Exception("Can't find any suitable apprenticeship vacancy.");
                }

                // Click on the i-th search result
                _driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(180));
                var searchResult = _driver.FindElements(
                    By.CssSelector(".search-result .vacancy-link"))
                    .Skip(i++).First();
                var vacancyId = searchResult.GetAttribute("data-vacancy-id");
                _tokenManager.SetToken(BindingData.VacancyIdToken, vacancyId);
                searchResult.Click();

                try
                {
                    _driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(0));
                    _driver.FindElement(By.Id("apply-button"));
                    var vacancyReference = _driver.FindElement(By.Id("vacancy-reference-id")).Text;
                    _tokenManager.SetToken(BindingData.VacancyReferenceToken, vacancyReference);
                    validPositionCount++;
                    if (validPositionCount != expectedPosition)
                        _driver.Navigate().Back();
                }
                catch
                {
                    //Go Back
                    _driver.Navigate().Back();
                }
            }
        }

        [When(@"I select the ""(.*)"" apprenticeship vacancy in location ""(.*)"" that I can apply via the employer site")]
        public void WhenISelectTheNthVacancyInLocationThatICanApplyViaTheEmployerSite(string position, string location)
        {
            var expectedPosition = _positions[position];
            SearchForAnApprenticeshipVacancyIn(location);

            const int numResults = 50;
            var i = 0;
            var validPositionCount = 0;

            while (validPositionCount != expectedPosition)
            {
                if (i == numResults)
                {
                    throw new Exception("Can't find any suitable vacancy.");
                }

                // Click on the i-th search result
                _driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(180));
                var searchResult = _driver.FindElements(
                    By.CssSelector(".search-result .vacancy-link"))
                    .Skip(i++).First();
                var vacancyId = searchResult.GetAttribute("data-vacancy-id");
                _tokenManager.SetToken(BindingData.VacancyIdToken, vacancyId);
                searchResult.Click();

                try
                {
                    _driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(0));
                    _driver.FindElement(By.Id("external-employer-website"));
                    var vacancyReference = _driver.FindElement(By.Id("vacancy-reference-id")).Text;
                    _tokenManager.SetToken(BindingData.VacancyReferenceToken, vacancyReference);
                    validPositionCount++;
                    if (validPositionCount != expectedPosition)
                        _driver.Navigate().Back();
                }
                catch
                {
                    //Go Back
                    _driver.Navigate().Back();
                }
            }
        }


        private void SearchForAnApprenticeshipVacancyIn(string location)
        {
            Given("I navigated to the ApprenticeshipSearchPage page");

            //Clear as will be set by user's postcode if logged in
            var table = new Table("Field", "Rule", "Value");
            table.AddRow(new[] { "ClearLocation", "Equals", "Cleared" });
            Then("I see", table);

            When("I enter data", GetApprenticeshipVacancySearchData(location));
            And("I choose Search");
            Then("I am on the ApprenticeshipSearchResultPage page");
            When("I enter data", Get50ResultsPerPage());
            And("I wait 3 seconds");
            Then("I am on the ApprenticeshipSearchResultPage page");
        }

        private void SearchForATraineeshipVacancyIn(string location)
        {
            Given("I navigated to the TraineeshipSearchPage page");

            //Clear as will be set by user's postcode if logged in
            var table = new Table("Field", "Rule", "Value");
            table.AddRow(new[] { "ClearLocation", "Equals", "Cleared" });
            Then("I see", table);

            When("I enter data", GetTraineeshipVacancySearchData(location));
            And("I choose Search");
            Then("I am on the TraineeshipSearchResultPage page");
            //When("I enter data", Get50ResultsPerPage());
            //And("I wait 3 seconds");
            //Then("I am on the TraineeshipSearchResultPage page");
        }

        [Then(@"Another browser window is opened")]
        public void ThenAnotherBrowserWindowIsOpened()
        {
            _driver.WindowHandles.Count.Should().Be(2);
        }

        [When(@"I navigate to the details of the apprenticeship vacancy (.*)")]
        public void WhenINavigateToTheDetailsOfTheApprenticeshipVacancy(int vacancyid)
        {
            var vacancySearchUri = new Uri(_driver.Url);
            var vacancyDetailsUri =
                string.Format("{0}://{1}:{2}/apprenticeship/{3}", 
                vacancySearchUri.Scheme, vacancySearchUri.Host, vacancySearchUri.Port, vacancyid);

            _driver.Navigate().GoToUrl(vacancyDetailsUri);
        }

        [When(@"I navigate to the details of the traineeship vacancy (.*)")]
        public void WhenINavigateToTheDetailsOfTheTraineeshipVacancy(int vacancyid)
        {
            var vacancySearchUri = new Uri(_driver.Url);
            var vacancyDetailsUri =
                string.Format("{0}://{1}:{2}/traineeship/{3}",
                vacancySearchUri.Scheme, vacancySearchUri.Host, vacancySearchUri.Port, vacancyid);

            _driver.Navigate().GoToUrl(vacancyDetailsUri);
        }

        private Table GetApprenticeshipVacancySearchData(string location)
        {
            string[] header = { "Field", "Value" };
            string[] row1 = { "Location", location };
            string[] row2 = { "WithInDistance", "40 miles" };
            string[] row3 = { "ApprenticeshipLevel", "All levels" };
            var t = new Table(header);
            t.AddRow(row1);
            t.AddRow(row2);
            t.AddRow(row3);
            return t;
        }

        private Table GetTraineeshipVacancySearchData(string location)
        {
            string[] header = { "Field", "Value" };
            string[] row1 = { "Location", location };
            var t = new Table(header);
            t.AddRow(row1);
            return t;
        }

        private Table Get50ResultsPerPage()
        {
            string[] header = { "Field", "Value" };
            string[] row1 = { "ResultsPerPageDropDown", "50 per page" };
            var t = new Table(header);
            t.AddRow(row1);
            return t;
        }

        [Given(@"I have registered a new candidate")]
        public void GivenIHaveRegisteredANewCandidate()
        {
            Given("I navigated to the RegisterCandidatePage page");
            When("I have created a new email address");
            And("I enter data", GetRegistrationData());
            And("I choose HasAcceptedTermsAndConditions");

            //Postcode search
            And("I enter data", GetAddressData());
            And("I wait for 30 seconds to see AddressSelect");
            Then("I am on the RegisterCandidatePage page");
            When("I am on AddressDropdown list item matching criteria", GetAddressMatchingCriteria());
            And("I choose WrappedElement");
            When("I am on the RegisterCandidatePage page");

            When("I choose CreateAccountButton");
            Then("I wait 500 second for the ActivationPage page");
            When("I get the token for my newly created account");
            And("I enter data", GetActivationCodeData());
            And("I choose ActivateButton");

            Then("I wait 120 second for the MonitoringInformationPage page");
            When("I choose SkipLink");

            Then("I wait 120 second for the ApprenticeshipSearchPage page");
            And("I am on the ApprenticeshipSearchPage page");
        }

        private Table GetRegistrationData()
        {
            string[] header = { "Field", "Value" };
            string[] row1 = { "Firstname", "Firstname" };
            string[] row2 = { "Lastname", "Lastname" };
            string[] row3 = { "Phonenumber", "07469984649" };
            string[] row4 = { "EmailAddress", "{EmailToken}" };
            string[] row5 = { "Day", "01" };
            string[] row6 = { "Month", "01" };
            string[] row7 = { "Year", "2000" };
            string[] row8 = { "Password", "?Password01!" };
            string[] row9 = { "ConfirmPassword", "?Password01!" };

            var t = new Table(header);

            t.AddRow(row1);
            t.AddRow(row2);
            t.AddRow(row3);
            t.AddRow(row4);
            t.AddRow(row5);
            t.AddRow(row6);
            t.AddRow(row7);
            t.AddRow(row8);
            t.AddRow(row9);

            return t;
        }

        private Table GetAddressData()
        {
            string[] header = { "Field", "Value" };
            string[] row1 = { "PostcodeSearch", "N7 8LS" };

            var t = new Table(header);

            t.AddRow(row1);

            return t;
        }

        private Table GetAddressMatchingCriteria()
        {
            string[] header = { "Field", "Rule", "Value" };
            string[] row1 = { "Text", "Equals", "N7 8LS, 6, Furlong Road, London" };
            var t = new Table(header);
            t.AddRow(row1);
            return t;
        }

        private Table GetActivationCodeData()
        {
            string[] header = { "Field", "Value" };
            string[] row1 = { "ActivationCode", "{ActivationCodeToken}" };
            var t = new Table(header);
            t.AddRow(row1);
            return t;
        }

        [When("I get the token for my newly created account")]
        public void WhenIGetTokenForMyNewlyCreatedAccount()
        {
            var email = _tokenManager.GetTokenByKey(BindingData.UserEmailAddressTokenName);
            var user = _userReadRepository.Get(email);

            if (user != null)
            {
                _tokenManager.SetToken(BindingData.ActivationCodeTokenName, user.ActivationCode);
            }
        }

        [Then("I set my token here")]
        public void ThenISetMyTokenHere()
        {
            var user = _userReadRepository.Get(_email);

            if (user != null)
            {
                _tokenManager.SetToken(BindingData.ActivationCodeTokenName, user.ActivationCode);
            }
        }

        [When("I get my mobile verification code")]
        public void WhenIGetMyMobileVerificationCode()
        {
            var email = _tokenManager.GetTokenByKey(BindingData.UserEmailAddressTokenName);
            var user = _userReadRepository.Get(_email);
            var candidate = _candidateReadRepository.Get(user.EntityId);

            if (candidate != null)
            {
                _tokenManager.SetToken(BindingData.MobileVerificationCodeTokenName, candidate.CommunicationPreferences.MobileVerificationCode);
            }
        }
    }
}
