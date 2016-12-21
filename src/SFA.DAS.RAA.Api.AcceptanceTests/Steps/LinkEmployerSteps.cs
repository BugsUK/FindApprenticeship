namespace SFA.DAS.RAA.Api.AcceptanceTests.Steps
{
    using System.Net;
    using System.Net.Http;
    using System.Text;
    using System.Threading.Tasks;
    using Api.Models;
    using Constants;
    using Extensions;
    using Models;
    using Newtonsoft.Json;
    using TechTalk.SpecFlow;

    [Binding]
    public class LinkEmployerSteps
    {
        [When(@"I request to link employer identified with EDSURN: (.*) to provider site identified with EDSURN: (.*)")]
        public async Task WhenIRequestToLinkEmployerIdentifiedWithEdsUrnToProviderSiteIdentifiedWithEdsUrn(int employerEdsUrn, int providerSiteEdsUrn)
        {
            var linkEmployerUri = string.Format(UriFormats.LinkEmployerEdsUrnUriFormat, employerEdsUrn);

            var employerLink = new EmployerLink
            {
                ProviderSiteEdsUrn = providerSiteEdsUrn
            };

            var httpClient = FeatureContext.Current.TestServer().HttpClient;
            httpClient.SetAuthorization();

            using (var response = await httpClient.PostAsync(linkEmployerUri, new StringContent(JsonConvert.SerializeObject(employerLink), Encoding.UTF8, "application/json")))
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

                    /*var responseVacancy = JsonConvert.DeserializeObject<Vacancy>(content);
                    if (responseVacancy != null && new VacancyComparer().Equals(responseVacancy, new Vacancy()))
                    {
                        responseVacancy = null;
                    }
                    ScenarioContext.Current.Add(vacancyUri, responseVacancy);*/
                }
            }
        }
    }
}
