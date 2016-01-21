namespace SFA.Apprenticeships.Infrastructure.IntegrationTests.Postcode
{
    using Common.Configuration;
    using Common.IoC;
    using Infrastructure.Postcode.IoC;
    using Logging.IoC;
    using NUnit.Framework;
    using SFA.Infrastructure.Interfaces;
    using StructureMap;

    public class PostCodeBaseTests
    {
        protected Container Container;

        [SetUp]
        public void Setup()
        {
            Container = new Container(x =>
            {
                x.AddRegistry<CommonRegistry>();
                x.AddRegistry<LoggingRegistry>();
                x.AddRegistry<PostcodeRegistry>();
            });
        }
    }
}