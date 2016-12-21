namespace SFA.DAS.RAA.Api.AcceptanceTests.Steps
{
    using System;
    using System.Net;
    using System.Net.Http;
    using System.Text;
    using System.Threading.Tasks;
    using Api.Models;
    using Constants;
    using Extensions;
    using FluentAssertions;
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

            var employerProviderSiteLink = new EmployerProviderSiteLink
            {
                ProviderSiteEdsUrn = providerSiteEdsUrn
            };

            var httpClient = FeatureContext.Current.TestServer().HttpClient;
            httpClient.SetAuthorization();

            using (var response = await httpClient.PostAsync(linkEmployerUri, new StringContent(JsonConvert.SerializeObject(employerProviderSiteLink), Encoding.UTF8, "application/json")))
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

                    var responseEmployerProviderSiteLink = JsonConvert.DeserializeObject<EmployerProviderSiteLink>(content);
                    if (Equals(responseEmployerProviderSiteLink, new EmployerProviderSiteLink()))
                    {
                        responseEmployerProviderSiteLink = null;
                    }
                    ScenarioContext.Current.Add(linkEmployerUri, responseEmployerProviderSiteLink);
                }
            }
        }

        [Then(@"I see the employer link for the employer identified with EDSURN: (.*) and the provider site identified with EDSURN: (.*)")]
        public void ThenISeeTheEmployerLinkForTheEmployerIdentifiedWithEdsUrnAndTheProviderSiteIdentifiedWithEdsUrn(int employerEdsUrn, int providerSiteEdsUrn)
        {
            var linkEmployerUri = string.Format(UriFormats.LinkEmployerEdsUrnUriFormat, employerEdsUrn);
            var responseEmployerProviderSiteLink = ScenarioContext.Current.Get<EmployerProviderSiteLink>(linkEmployerUri);
            responseEmployerProviderSiteLink.Should().NotBeNull();

            var expectedLink = new EmployerProviderSiteLink
            {
                EmployerId = 1,
                EmployerEdsUrn = employerEdsUrn,
                ProviderSiteId = 9,
                ProviderSiteEdsUrn = providerSiteEdsUrn,
                EmployerDescription = "The description",
                EmployerWebsiteUrl = "http://www.test.com"
            };

            responseEmployerProviderSiteLink.Equals(expectedLink).Should().BeTrue();
        }


        [Then(@"I do not see the employer link for the employer identified with EDSURN: (.*) and the provider site identified with EDSURN: (.*)")]
        public void ThenIDoNotSeeTheEmployerLinkForTheEmployerIdentifiedWithEdsUrnAndTheProviderSiteIdentifiedWithEdsUrn(int employerEdsUrn, int providerSiteEdsUrn)
        {
            var linkEmployerUri = string.Format(UriFormats.LinkEmployerEdsUrnUriFormat, employerEdsUrn);
            var responseEmployerProviderSiteLink = ScenarioContext.Current.Get<EmployerProviderSiteLink>(linkEmployerUri);
            responseEmployerProviderSiteLink.Should().BeNull();
        }
    }
}
