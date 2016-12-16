﻿namespace SFA.DAS.RAA.Api.AcceptanceTests.Steps
{
    using System;
    using System.Net;
    using System.Net.Http;
    using System.Text;
    using Api.Models;
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
        [When(@"I request to change the fixed wage for the vacancy with id: (.*) to £(.*) (.*)")]
        public void WhenIRequestToChangeTheFixedWageForTheVacancyWithIdToWeekly(int vacancyId, decimal amount, string unitString)
        {
            var unit = (WageUnit)Enum.Parse(typeof(WageUnit), unitString);
            var wageUpdate = new WageUpdate
            {
                Type = WageType.Custom,
                Amount = amount,
                Unit = unit
            };

            UpdateWage(vacancyId, wageUpdate);
        }

        [When(@"I request to change the wage type for the vacancy with id: (.*) to (.*)")]
        public void WhenIRequestToChangeTheWageTypeForTheVacancyWithIdToNationalMinimum(int vacancyId, string typeString)
        {
            var type = (WageType)Enum.Parse(typeof(WageType), typeString);
            var wageUpdate = new WageUpdate
            {
                Type = type
            };

            UpdateWage(vacancyId, wageUpdate);
        }

        private static void UpdateWage(int vacancyId, WageUpdate wageUpdate)
        {
            var httpClient = FeatureContext.Current.TestServer().HttpClient;
            httpClient.SetAuthorization();

            var editVacancyWageUri = string.Format(UriFormats.EditWageVacancyIdUriFormat, vacancyId);
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

        [Then(@"I see that the fixed wage details for the vacancy with id: (.*) is now £(.*) (.*)")]
        public void ThenISeeThatTheFixedWageDetailsForTheVacancyWithIdIsNowWeekly(int vacancyId, decimal amount, string unitString)
        {
            var wageUnit = (WageUnit)Enum.Parse(typeof(WageUnit), unitString);
            var editVacancyWageUri = string.Format(UriFormats.EditWageVacancyIdUriFormat, vacancyId);
            var responseVacancy = ScenarioContext.Current.Get<Vacancy>(editVacancyWageUri);

            responseVacancy.Should().NotBeNull();
            var updatedWage = responseVacancy.Wage;
            updatedWage.Should().NotBeNull();
            updatedWage.Amount.Should().Be(amount);
            updatedWage.Unit.Should().Be(wageUnit);
        }
    }
}