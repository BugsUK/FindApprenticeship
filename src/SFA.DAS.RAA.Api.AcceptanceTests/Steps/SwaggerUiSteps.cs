namespace SFA.DAS.RAA.Api.AcceptanceTests.Steps
{
    using System.Net;
    using FluentAssertions;
    using RestSharp;
    using TechTalk.SpecFlow;

    [Binding]
    public class SwaggerUiSteps
    {
        [When(@"I request the swagger homepage")]
        public void WhenIRequestTheSwaggerHomepage()
        {
            var request = new RestRequest("swagger/ui/index", Method.GET);
            var client = ScenarioContext.Current.Get<IRestClient>();
            var response = client.Execute(request);
            ScenarioContext.Current.Add("WhenIRequestTheSwaggerHomepage", response);
        }
        
        [Then(@"I should see the swagger homepage")]
        public void ThenIShouldSeeTheSwaggerHomepage()
        {
            var response = ScenarioContext.Current.Get<IRestResponse>("WhenIRequestTheSwaggerHomepage");
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [When(@"I request the swagger docs")]
        public void WhenIRequestTheSwaggerDocs()
        {
            var request = new RestRequest("swagger/docs/v1", Method.GET);
            var client = ScenarioContext.Current.Get<IRestClient>();
            var response = client.Execute(request);
            ScenarioContext.Current.Add("WhenIRequestTheSwaggerDocs", response);
        }

        [Then(@"I should see the swagger docs")]
        public void ThenIShouldSeeTheSwaggerDocs()
        {
            var response = ScenarioContext.Current.Get<IRestResponse>("WhenIRequestTheSwaggerDocs");
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }
    }
}
