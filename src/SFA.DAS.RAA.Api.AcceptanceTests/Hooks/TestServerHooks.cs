namespace SFA.DAS.RAA.Api.AcceptanceTests.Hooks
{
    using Extensions;
    using Microsoft.Owin.Testing;
    using TechTalk.SpecFlow;

    [Binding]
    public sealed class TestServerHooks
    {
        // For additional details on SpecFlow hooks see http://go.specflow.org/doc-hooks

        [BeforeFeature]
        public static void BeforeFeature()
        {
            var testServer = TestServer.Create<TestStartup>();
            FeatureContext.Current.TestServer(testServer);
        }

        [AfterFeature]
        public static void AfterFeature()
        {
            var testServer = FeatureContext.Current.TestServer();
            testServer.Dispose();
        }
    }
}
