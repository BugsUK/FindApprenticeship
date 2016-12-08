﻿namespace SFA.DAS.RAA.Api.AcceptanceTests.Steps
{
    using Constants;
    using Extensions;
    using FluentAssertions;
    using Models;
    using Newtonsoft.Json;
    using Ploeh.AutoFixture;
    using TechTalk.SpecFlow;

    [Binding]
    public class GetVacancyDetailsSteps
    {
        [When(@"I request the vacancy details for the vacancy with id: (.*)")]
        public void WhenIRequestTheVacancyDetailsForTheVacancyWithId(int vacancyId)
        {
            var testServer = FeatureContext.Current.TestServer();

            var vacancy = new Fixture().Build<Vacancy>().With(v => v.VacancyId, vacancyId);
            ScenarioContext.Current.Add($"vacancyId: {vacancyId}", vacancy);

            var vacancyUri = $"/vacancy/?id={vacancyId}";
            using (var response = testServer.HttpClient.GetAsync(vacancyUri).Result)
            {
                ScenarioContext.Current.Add(ScenarioContextKeys.HttpResponseStatusCode, response.StatusCode);
                using (var httpContent = response.Content)
                {
                    var content = httpContent.ReadAsStringAsync().Result;

                    var responseVacancy = JsonConvert.DeserializeObject<Vacancy>(content);

                    ScenarioContext.Current.Add(vacancyUri, responseVacancy);
                }
            }
        }

        [Then(@"I see the vacancy details for the vacancy with id: (.*)")]
        public void ThenISeeTheVacancyDetailsForTheVacancyWithId(int vacancyId)
        {
            var vacancy = ScenarioContext.Current.Get<Vacancy>($"vacancyId: {vacancyId}");
            var vacancyUri = $"/vacancy/?id={vacancyId}";
            var responseVacancy = ScenarioContext.Current.Get<Vacancy>(vacancyUri);

            vacancy.Should().NotBeNull();
            responseVacancy.Should().NotBeNull();
            vacancy.Equals(responseVacancy).Should().BeTrue();
        }
    }
}
