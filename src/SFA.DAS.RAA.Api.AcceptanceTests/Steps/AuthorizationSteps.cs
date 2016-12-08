namespace SFA.DAS.RAA.Api.AcceptanceTests.Steps
{
    using Constants;
    using TechTalk.SpecFlow;

    [Binding]
    public class AuthorizationSteps
    {
        [When(@"I authorize my request with a Provider API key")]
        public void WhenIAuthorizeMyRequestWithAProviderApiKey()
        {
            ScenarioContext.Current.Add(ScenarioContextKeys.ApiKey, ApiKeys.ProviderApiKey);
        }
    }
}
