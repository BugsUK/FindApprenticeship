namespace SFA.Apprenticeships.Infrastructure.Repositories.IntegrationTests
{
    using Common.IoC;
    using Domain.Interfaces.Configuration;
    using Logging.IoC;
    using Mongo.Common.Configuration;
    using NUnit.Framework;
    using Repositories.Applications.IoC;
    using Repositories.Candidates.IoC;
    using Repositories.Communication.IoC;
    using Repositories.Users.IoC;
    using StructureMap;

    [SetUpFixture]
    public class RepositoryIntegrationTest
    {
        protected MongoConfiguration MongoConfiguration;
        protected Container Container;

        [SetUp]
        public void SetUpContainer()
        {
            Container = new Container(x =>
            {
                x.AddRegistry<CommonRegistry>();
                x.AddRegistry<LoggingRegistry>();
                x.AddRegistry<ApplicationRepositoryRegistry>();
                x.AddRegistry<CandidateRepositoryRegistry>();
                x.AddRegistry<UserRepositoryRegistry>();
                x.AddRegistry<CommunicationRepositoryRegistry>();
            });

            var configurationManager = Container.GetInstance<IConfigurationService>();
            MongoConfiguration = configurationManager.Get<MongoConfiguration>(MongoConfiguration.MongoConfigurationName);
        }
    }
}
