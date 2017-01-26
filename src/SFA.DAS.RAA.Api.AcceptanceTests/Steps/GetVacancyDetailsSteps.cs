namespace SFA.DAS.RAA.Api.AcceptanceTests.Steps
{
    using System;
    using System.Net;
    using System.Threading.Tasks;
    using Apprenticeships.Domain.Entities.Raa.Vacancies;
    using Apprenticeships.Infrastructure.Repositories.Sql.Schemas.Vacancy;
    using Comparers;
    using Constants;
    using Extensions;
    using Factories;
    using FluentAssertions;
    using Models;
    using Moq;
    using Newtonsoft.Json;
    using Ploeh.AutoFixture;
    using TechTalk.SpecFlow;
    using UnitTests.Factories;
    using DbVacancy = Apprenticeships.Infrastructure.Repositories.Sql.Schemas.Vacancy.Entities.Vacancy;

    [Binding]
    public class GetVacancyDetailsSteps
    {
        [When(@"I request the vacancy details for the vacancy with id: (.*)")]
        public async Task WhenIRequestTheVacancyDetailsForTheVacancyWithId(int vacancyId)
        {
            var vacancyUri = string.Format(UriFormats.VacancyIdUriFormat, vacancyId);
            await GetVacancy(vacancyUri, vacancyId, 0, Guid.Empty);
        }

        [When(@"I request the vacancy details for the vacancy with reference number: (.*)")]
        public async Task WhenIRequestTheVacancyDetailsForTheVacancyWithReferenceNumber(int vacancyReferenceNumber)
        {
            var vacancyUri = string.Format(UriFormats.VacancyReferenceNumberUriFormat, vacancyReferenceNumber * 100000);
            await GetVacancy(vacancyUri, vacancyReferenceNumber, vacancyReferenceNumber * 100000, Guid.Empty);
        }

        [When(@"I request the vacancy details for the vacancy with guid: (.*)")]
        public async Task WhenIRequestTheVacancyDetailsForTheVacancyWithGuid(Guid vacancyGuid)
        {
            var vacancyUri = string.Format(UriFormats.VacancyGuidUriFormat, vacancyGuid);
            var vacancyId = Convert.ToInt32(vacancyGuid.ToString().Substring(0, 1));
            await GetVacancy(vacancyUri, vacancyId, 0, vacancyGuid);
        }

        [When(@"I request the vacancy details for the vacancy with no identifier")]
        public async Task WhenIRequestTheVacancyDetailsForTheVacancyWithNoIdentifier()
        {
            await GetVacancy("/vacancy", 0, 0, Guid.Empty);
        }

        [Then(@"I see the vacancy details for the vacancy with id: (.*)")]
        public void ThenISeeTheVacancyDetailsForTheVacancyWithId(int vacancyId)
        {
            var vacancy = ScenarioContext.Current.Get<DbVacancy>($"vacancyId: {vacancyId}");
            var vacancyUri = string.Format(UriFormats.VacancyIdUriFormat, vacancyId);
            var responseVacancy = ScenarioContext.Current.Get<Vacancy>(vacancyUri);

            vacancy.Should().NotBeNull();
            responseVacancy.Should().NotBeNull();

            var comparer = new DbVacancyComparer();
            comparer.Equals(vacancy, responseVacancy).Should().BeTrue();
        }

        [Then(@"I see the vacancy details for the vacancy with reference number: (.*)")]
        public void ThenISeeTheVacancyDetailsForTheVacancyWithReferenceNumber(int vacancyReferenceNumber)
        {
            var vacancy = ScenarioContext.Current.Get<DbVacancy>($"vacancyReferenceNumber: {vacancyReferenceNumber * 100000}");
            var vacancyUri = string.Format(UriFormats.VacancyReferenceNumberUriFormat, vacancyReferenceNumber * 100000);
            var responseVacancy = ScenarioContext.Current.Get<Vacancy>(vacancyUri);

            vacancy.Should().NotBeNull();
            responseVacancy.Should().NotBeNull();

            var comparer = new DbVacancyComparer();
            comparer.Equals(vacancy, responseVacancy).Should().BeTrue();
        }

        [Then(@"I see the vacancy details for the vacancy with guid: (.*)")]
        public void ThenISeeTheVacancyDetailsForTheVacancyWithGuid(Guid vacancyGuid)
        {
            var vacancy = ScenarioContext.Current.Get<DbVacancy>($"vacancyGuid: {vacancyGuid}");
            var vacancyUri = string.Format(UriFormats.VacancyGuidUriFormat, vacancyGuid);
            var responseVacancy = ScenarioContext.Current.Get<Vacancy>(vacancyUri);

            vacancy.Should().NotBeNull();
            responseVacancy.Should().NotBeNull();

            var comparer = new DbVacancyComparer();
            comparer.Equals(vacancy, responseVacancy).Should().BeTrue();
        }

        [Then(@"I do not see the vacancy details for the vacancy with id: (.*)")]
        public void ThenIDoNotSeeTheVacancyDetailsForTheVacancyWithId(int vacancyId)
        {
            var vacancyUri = string.Format(UriFormats.VacancyIdUriFormat, vacancyId);
            var responseVacancy = ScenarioContext.Current.Get<Vacancy>(vacancyUri);
            responseVacancy.Should().BeNull();
        }

        [Then(@"I do not see the vacancy details for the vacancy with reference number: (.*)")]
        public void ThenIDoNotSeeTheVacancyDetailsForTheVacancyWithReferenceNumber(int vacancyReferenceNumber)
        {
            var vacancyUri = string.Format(UriFormats.VacancyReferenceNumberUriFormat, vacancyReferenceNumber * 100000);
            var responseVacancy = ScenarioContext.Current.Get<Vacancy>(vacancyUri);
            responseVacancy.Should().BeNull();
        }

        [Then(@"I do not see the vacancy details for the vacancy with guid: (.*)")]
        public void ThenIDoNotSeeTheVacancyDetailsForTheVacancyWithGuid(Guid vacancyGuid)
        {
            var vacancyUri = string.Format(UriFormats.VacancyGuidUriFormat, vacancyGuid);
            var responseVacancy = ScenarioContext.Current.Get<Vacancy>(vacancyUri);
            responseVacancy.Should().BeNull();
        }

        [Then(@"I do not see the vacancy details for the vacancy with no identifier")]
        public void ThenIDoNotSeeTheVacancyDetailsForTheVacancyWithNoIdentifier()
        {
            var responseVacancy = ScenarioContext.Current.Get<Vacancy>("/vacancy");
            responseVacancy.Should().BeNull();
        }

        private static async Task GetVacancy(string vacancyUri, int vacancyId, int vacancyReferenceNumber, Guid vacancyGuid)
        {
            var vacancy1 = new Fixture().Build<DbVacancy>()
                .With(v => v.VacancyId, 1)
                .With(v => v.VacancyReferenceNumber, 100000)
                .With(v => v.VacancyGuid, new Guid("10000000-0000-0000-0000-000000000000"))
                .With(v => v.VacancyStatusId, (int)VacancyStatus.Live)
                .With(v => v.ContractOwnerID, RaaApiUserFactory.SkillsFundingAgencyProviderId)
                .Create();
            var vacancy2 = new Fixture().Build<DbVacancy>()
                .With(v => v.VacancyId, 2)
                .With(v => v.VacancyReferenceNumber, 200000)
                .With(v => v.VacancyGuid, new Guid("20000000-0000-0000-0000-000000000000"))
                .With(v => v.VacancyStatusId, (int)VacancyStatus.Live)
                .With(v => v.ContractOwnerID, -1)
                .Create();

            ScenarioContext.Current.Add($"vacancyId: {vacancy1.VacancyId}", vacancy1);
            ScenarioContext.Current.Add($"vacancyId: {vacancy2.VacancyId}", vacancy2);

            ScenarioContext.Current.Add($"vacancyReferenceNumber: {vacancy1.VacancyReferenceNumber}", vacancy1);
            ScenarioContext.Current.Add($"vacancyReferenceNumber: {vacancy2.VacancyReferenceNumber}", vacancy2);

            ScenarioContext.Current.Add($"vacancyGuid: {vacancy1.VacancyGuid}", vacancy1);
            ScenarioContext.Current.Add($"vacancyGuid: {vacancy2.VacancyGuid}", vacancy2);

            RaaMockFactory.GetMockGetOpenConnection().Setup(
                m => m.Query<DbVacancy>(VacancyRepository.SelectByIdSql, It.Is<object>(o => o.GetHashCode() == new { vacancyId = vacancy1.VacancyId }.GetHashCode()), null, null))
                .Returns(new[] { vacancy1 });

            RaaMockFactory.GetMockGetOpenConnection().Setup(
                m => m.Query<DbVacancy>(VacancyRepository.SelectByIdSql, It.Is<object>(o => o.GetHashCode() == new { vacancyId = vacancy2.VacancyId }.GetHashCode()), null, null))
                .Returns(new[] { vacancy2 });

            //Setup vacancy reference number to vacancy id mapping
            RaaMockFactory.GetMockGetOpenConnection().Setup(
                m => m.QueryCached<int?>(It.IsAny<TimeSpan>(), VacancyRepository.SelectVacancyIdFromReferenceNumberSql, It.Is<object>(o => o.GetHashCode() == new { vacancyReferenceNumber }.GetHashCode()), null, null))
                .Returns(new int?[] { vacancyId });

            //Setup vacancy guid to vacancy id mapping
            RaaMockFactory.GetMockGetOpenConnection().Setup(
                m => m.QueryCached<int?>(It.IsAny<TimeSpan>(), VacancyRepository.SelectVacancyIdFromGuidSql, It.Is<object>(o => o.GetHashCode() == new { vacancyGuid }.GetHashCode()), null, null))
                .Returns(new int?[] { vacancyId });

            var httpClient = FeatureContext.Current.TestServer().HttpClient;
            httpClient.SetAuthorization();

            using (var response = await httpClient.GetAsync(vacancyUri))
            {
                ScenarioContext.Current.Add(ScenarioContextKeys.HttpResponseStatusCode, response.StatusCode);
                using (var httpContent = response.Content)
                {
                    var content = await httpContent.ReadAsStringAsync();
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
                    ScenarioContext.Current.Add(vacancyUri, responseVacancy);
                }
            }
        }
    }
}
