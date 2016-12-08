using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TechTalk.SpecFlow;

namespace SFA.DAS.RAA.Api.AcceptanceTests.Hooks
{
    [Binding]
    public sealed class TestServerHooks
    {
        // For additional details on SpecFlow hooks see http://go.specflow.org/doc-hooks

        [BeforeFeature]
        public void BeforeFeature()
        {
            FeatureContext.Current.TestServer(TestServer.Create<Startup>());
        }

        [AfterFeature]
        public void AfterFeature()
        {
            //TODO: implement logic that has to run after executing each scenario
        }
    }
}
