namespace SFA.DAS.RAA.Api.AcceptanceTests.Steps
{
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
        [When(@"I request to link employer identified with EDSURN: (.*) to provider site identified with EDSURN: (.*) with description: (.*) and website: (.*)")]
        public async Task WhenIRequestToLinkEmployerIdentifiedWithEdsUrnToProviderSiteIdentifiedWithEdsUrn(int employerEdsUrn, int providerSiteEdsUrn, string description, string website)
        {
            var linkEmployerUri = employerEdsUrn == 0 ? UriFormats.LinkEmployerUri : string.Format(UriFormats.LinkEmployerEdsUrnUriFormat, employerEdsUrn);

            var employerProviderSiteLink = new EmployerProviderSiteLinkRequest
            {
                ProviderSiteEdsUrn = providerSiteEdsUrn == 0 ? (int?)null : providerSiteEdsUrn,
                EmployerDescription = description == "null" ? null : description,
                EmployerWebsiteUrl = website == "null" ? null : website
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

                    var responseEmployerProviderSiteLink = JsonConvert.DeserializeObject<EmployerProviderSiteLinkRequest>(content);
                    if (Equals(responseEmployerProviderSiteLink, new EmployerProviderSiteLinkRequest()))
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
            var linkEmployerUri = employerEdsUrn == 0 ? UriFormats.LinkEmployerUri : string.Format(UriFormats.LinkEmployerEdsUrnUriFormat, employerEdsUrn);
            var responseEmployerProviderSiteLink = ScenarioContext.Current.Get<EmployerProviderSiteLinkRequest>(linkEmployerUri);
            responseEmployerProviderSiteLink.Should().NotBeNull();

            var expectedLink = new EmployerProviderSiteLinkRequest
            {
                EmployerEdsUrn = employerEdsUrn,
                ProviderSiteEdsUrn = providerSiteEdsUrn,
                EmployerDescription = "The description",
                EmployerWebsiteUrl = "http://www.test.com"
            };

            responseEmployerProviderSiteLink.Equals(expectedLink).Should().BeTrue();
        }


        [Then(@"I do not see the employer link for the employer identified with EDSURN: (.*) and the provider site identified with EDSURN: (.*)")]
        public void ThenIDoNotSeeTheEmployerLinkForTheEmployerIdentifiedWithEdsUrnAndTheProviderSiteIdentifiedWithEdsUrn(int employerEdsUrn, int providerSiteEdsUrn)
        {
            var linkEmployerUri = employerEdsUrn == 0 ? UriFormats.LinkEmployerUri : string.Format(UriFormats.LinkEmployerEdsUrnUriFormat, employerEdsUrn);
            var responseEmployerProviderSiteLink = ScenarioContext.Current.Get<EmployerProviderSiteLinkRequest>(linkEmployerUri);
            responseEmployerProviderSiteLink.Should().BeNull();
        }
    }
}
