namespace SFA.DAS.RAA.Api.AcceptanceTests.Extensions
{
    using System.Net.Http;
    using System.Net.Http.Headers;
    using Constants;
    using TechTalk.SpecFlow;

    public static class HttpClientExtensions
    {
        public static void SetAuthorization(this HttpClient httpClient)
        {
            if (ScenarioContext.Current.ContainsKey(ScenarioContextKeys.ApiKey))
            {
                var apiKey = ScenarioContext.Current[ScenarioContextKeys.ApiKey].ToString();
                ScenarioContext.Current.Remove(ScenarioContextKeys.ApiKey);
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", apiKey);
            }
        }
    }
}