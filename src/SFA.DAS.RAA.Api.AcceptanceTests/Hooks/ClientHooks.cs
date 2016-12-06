namespace SFA.DAS.RAA.Api.AcceptanceTests.Hooks
{
    using RestSharp;
    using TechTalk.SpecFlow;

    [Binding]
    public sealed class ClientHooks
    {
        // For additional details on SpecFlow hooks see http://go.specflow.org/doc-hooks

        [BeforeScenario]
        public void BeforeScenario()
        {
            IRestClient client = new RestClient("http://sfadasraaapi-dev.azurewebsites.net/");
            ScenarioContext.Current.Set(client);
        }

        [AfterScenario]
        public void AfterScenario()
        {

        }
    }
}
