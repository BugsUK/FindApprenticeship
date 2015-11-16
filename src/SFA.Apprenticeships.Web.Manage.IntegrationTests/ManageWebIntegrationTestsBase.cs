namespace SFA.Apprenticeships.Web.Manage.IntegrationTests
{
    using Domain.Interfaces.Configuration;
    using Infrastructure.Mongo.Common.Configuration;
    using IoC;
    using NUnit.Framework;
    using StructureMap;

    public class ManageWebIntegrationTestsBase
    {
        protected MongoConfiguration MongoConfiguration;
        protected IContainer Container;

        [SetUp]
        public void SetUpContainer()
        {
            Container = IoC.Initialize();

            var configurationManager = Container.GetInstance<IConfigurationService>();
            MongoConfiguration = configurationManager.Get<MongoConfiguration>();
        }
    }
}