namespace SFA.DAS.RAA.Api.AcceptanceTests.Extensions
{
    using Microsoft.Owin.Testing;
    using TechTalk.SpecFlow;

    public static class FeatureContextExtensions
    {
        private const string TestServerKey = "TestServer";

        public static TestServer TestServer(this FeatureContext source)
        {
            return source.Get<TestServer>(TestServerKey);
        }

        public static void TestServer(this FeatureContext source, TestServer server)
        {
            source.Add(TestServerKey, server);
        }
    }
}