namespace SFA.DAS.RAA.Api.AcceptanceTests.Steps
{
    using System;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Text;
    using Apprenticeships.Domain.Entities.Raa.Vacancies;
    using Apprenticeships.Domain.Entities.Vacancies;
    using Comparers;
    using Constants;
    using Extensions;
    using FluentAssertions;
    using Models;
    using Newtonsoft.Json;
    using TechTalk.SpecFlow;

    [Binding]
    public class EditWageSteps
    {
        [When(@"I request to change the wage for the vacancy with id: (.*) to (.*)")]
        public void WhenIRequestToChangeTheWageTypeForTheVacancyWithIdToNationalMinimum(int vacancyId, string wageString)
        {
            var wageStrings = wageString.Split(' ');
            var type = (WageType)Enum.Parse(typeof(WageType), wageStrings[0]);
            var wageUpdate = new WageUpdate
            {
                Type = type
            };

            if (type == WageType.Custom)
            {
                wageUpdate.Amount = decimal.Parse(wageStrings[1].Replace("£", ""));
                wageUpdate.Unit = (WageUnit)Enum.Parse(typeof(WageUnit), wageStrings[2]);
            }

            if (type == WageType.CustomRange)
            {
                wageUpdate.AmountLowerBound = decimal.Parse(wageStrings[1].Replace("£", ""));
                wageUpdate.AmountUpperBound = decimal.Parse(wageStrings[3].Replace("£", ""));
                wageUpdate.Unit = (WageUnit)Enum.Parse(typeof(WageUnit), wageStrings[4]);
            }

            UpdateWage(vacancyId, wageUpdate);
        }

        private static void UpdateWage(int vacancyId, WageUpdate wageUpdate)
        {
            var httpClient = FeatureContext.Current.TestServer().HttpClient;
            httpClient.SetAuthorization();

            var editVacancyWageUri = string.Format(UriFormats.EditWageVacancyIdUriFormat, vacancyId);

            ScenarioContext.Current.Add(editVacancyWageUri + ScenarioContextKeys.WageUpdate, wageUpdate);

            using (
                var response =
                    httpClient.PutAsync(editVacancyWageUri,
                        new StringContent(JsonConvert.SerializeObject(wageUpdate), Encoding.UTF8, "application/json")).Result)
            {
                ScenarioContext.Current.Add(ScenarioContextKeys.HttpResponseStatusCode, response.StatusCode);
                using (var httpContent = response.Content)
                {
                    var content = httpContent.ReadAsStringAsync().Result;
                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        var responseMessage = JsonConvert.DeserializeObject<ResponseMessage>(content);
                        ScenarioContext.Current.Add(ScenarioContextKeys.HttpResponseMessage, responseMessage);
                    }

                    var responseVacancy = JsonConvert.DeserializeObject<Vacancy>(content);
                    if (responseVacancy != null && new VacancyComparer().Equals(responseVacancy, new Vacancy()))
                    {
                        responseVacancy = null;
                    }
                    ScenarioContext.Current.Add(editVacancyWageUri, responseVacancy);
                }
            }
        }


        [Then(@"I do not see the edited vacancy wage details for the vacancy with id: (.*)")]
        public void ThenIDoNotSeeTheEditedVacancyWageDetailsForTheVacancyWithId(int vacancyId)
        {
            var editVacancyWageUri = string.Format(UriFormats.EditWageVacancyIdUriFormat, vacancyId);
            var responseVacancy = ScenarioContext.Current.Get<Vacancy>(editVacancyWageUri);
            responseVacancy.Should().BeNull();
        }

        [Then(@"I see that the wage details for the vacancy with id: (.*) have been updated")]
        public void ThenISeeThatTheWageDetailsForTheVacancyWithIdHaveBeenUpdated(int vacancyId)
        {
            var editVacancyWageUri = string.Format(UriFormats.EditWageVacancyIdUriFormat, vacancyId);
            var wageUpdate = ScenarioContext.Current.Get<WageUpdate>(editVacancyWageUri + ScenarioContextKeys.WageUpdate);
            var responseVacancy = ScenarioContext.Current.Get<Vacancy>(editVacancyWageUri);

            responseVacancy.Should().NotBeNull();
            wageUpdate.Should().NotBeNull();
            var updatedWage = responseVacancy.Wage;

            if (!wageUpdate.Type.HasValue)
            {
                return;
            }

            if (wageUpdate.Type.Value == WageType.Custom)
            {
                updatedWage.Should().NotBeNull();
                updatedWage.Type.Should().Be(updatedWage.Type);
                updatedWage.Amount.Should().Be(wageUpdate.Amount);
                updatedWage.AmountLowerBound.Should().Be(null);
                updatedWage.AmountUpperBound.Should().Be(null);
                updatedWage.Unit.Should().Be(wageUpdate.Unit);
                updatedWage.Text.Should().Be($"£{wageUpdate.Amount:N2}");
            }

            if (wageUpdate.Type.Value == WageType.CustomRange)
            {
                updatedWage.Should().NotBeNull();
                updatedWage.Type.Should().Be(updatedWage.Type);
                updatedWage.Amount.Should().Be(null);
                updatedWage.AmountLowerBound.Should().Be(updatedWage.AmountLowerBound);
                updatedWage.AmountUpperBound.Should().Be(updatedWage.AmountUpperBound);
                updatedWage.Unit.Should().Be(wageUpdate.Unit);
                updatedWage.Text.Should().Be($"£{wageUpdate.AmountLowerBound:N2} - £{wageUpdate.AmountUpperBound:N2}");
            }
        }
    }
}
