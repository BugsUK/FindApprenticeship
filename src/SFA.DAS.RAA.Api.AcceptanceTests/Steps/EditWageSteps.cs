namespace SFA.DAS.RAA.Api.AcceptanceTests.Steps
{
    using System;
    using System.Net.Http;
    using Apprenticeships.Domain.Entities.Raa.Vacancies;
    using Apprenticeships.Domain.Entities.Vacancies;
    using Constants;
    using Extensions;
    using Newtonsoft.Json;
    using TechTalk.SpecFlow;

    [Binding]
    public class EditWageSteps
    {
        [When(@"I request to change the fixed wage for the vacancy with id: (.*) to £(.*) (.*)")]
        public void WhenIRequestToChangeTheFixedWageForTheVacancyWithIdToWeekly(int vacancyId, decimal amount, string unitString)
        {
            var wageUnit = (WageUnit)Enum.Parse(typeof(WageUnit), unitString);
            var wage = ScenarioContext.Current.Get<Vacancy>($"vacancyId: {vacancyId}").Wage;
            var updatedWage = new Wage(wage.Type, amount, wage.AmountLowerBound, wage.AmountUpperBound, wage.Text, wageUnit, wage.HoursPerWeek, wage.ReasonForType);

            var httpClient = FeatureContext.Current.TestServer().HttpClient;
            httpClient.SetAuthorization();

            var editVacancyWageUri = string.Format(UriFormats.EditWageVacancyIdUriFormat, vacancyId);
            using (var response = httpClient.PutAsync(editVacancyWageUri, new StringContent(JsonConvert.SerializeObject(updatedWage))).Result)
            {
                ScenarioContext.Current.Add(ScenarioContextKeys.HttpResponseStatusCode, response.StatusCode);
                using (var httpContent = response.Content)
                {
                    var content = httpContent.ReadAsStringAsync().Result;
                    var responseVacancy = JsonConvert.DeserializeObject<Vacancy>(content);
                    ScenarioContext.Current.Add(editVacancyWageUri, responseVacancy);
                }
            }
        }
    }
}
