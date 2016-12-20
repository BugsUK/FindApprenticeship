namespace SFA.DAS.RAA.Api.AcceptanceTests.Steps
{
    using System;
    using Constants;
    using TechTalk.SpecFlow;

    [Binding]
    public class AuthorizationSteps
    {
        [When(@"I authorize my request with an invalid API key")]
        public void WhenIAuthorizeMyRequestWithAnInvalidApiKey()
        {
            ScenarioContext.Current.Add(ScenarioContextKeys.ApiKey, "INVALID");
        }

        [When(@"I authorize my request with an unknown API key")]
        public void WhenIAuthorizeMyRequestWithAnUnknownApiKey()
        {
            ScenarioContext.Current.Add(ScenarioContextKeys.ApiKey, Guid.NewGuid());
        }

        [When(@"I authorize my request with a Provider API key")]
        public void WhenIAuthorizeMyRequestWithAProviderApiKey()
        {
            ScenarioContext.Current.Add(ScenarioContextKeys.ApiKey, ApiKeys.ProviderApiKey);
        }
    }
}
