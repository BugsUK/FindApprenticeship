namespace SFA.DAS.RAA.Api.AcceptanceTests.Steps
{
    using Constants;
    using Extensions;
    using Models;
    using Newtonsoft.Json;
    using TechTalk.SpecFlow;

    [Binding]
    public class GetVacancyDetailsSteps
    {
        [When(@"I request the vacancy details for the vacancy with id: (.*)")]
        public void WhenIRequestTheVacancyDetailsForTheVacancyWithId(int vacancyId)
        {
            var testServer = FeatureContext.Current.TestServer();
            var vacancyUri = $"/vacancy/?id={vacancyId}";
            using (var response = testServer.HttpClient.GetAsync(vacancyUri).Result)
            {
                ScenarioContext.Current.Add(ScenarioContextKeys.HttpResponseStatusCode, response.StatusCode);
                using (var httpContent = response.Content)
                {
                    var content = httpContent.ReadAsStringAsync().Result;

                    var vacancy = JsonConvert.DeserializeObject<Vacancy>(content);

                    ScenarioContext.Current.Add(vacancyUri, vacancy);
                }
            }
        }


        [Then(@"I see the vacancy details for the vacancy with id: (.*)")]
        public void ThenISeeTheVacancyDetailsForTheVacancyWithId(int vacancyId)
        {
            ScenarioContext.Current.Pending();
        }
    }
}
