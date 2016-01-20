namespace SFA.Apprenticeships.Infrastructure.IntegrationTests.Repositories
{
    using Common.Configuration;
    using Common.IoC;
    using SFA.Infrastructure.Interfaces;
    using Infrastructure.Repositories.Applications.IoC;
    using Infrastructure.Repositories.Candidates.IoC;
    using Infrastructure.Repositories.Communication.IoC;
    using Infrastructure.Repositories.Users.IoC;
    using Infrastructure.Repositories.Vacancies.IoC;
    using Logging.IoC;
    using Mongo.Common.Configuration;
    using NUnit.Framework;
    using StructureMap;

    [SetUpFixture]
    public class RepositoryIntegrationTest
    {
        protected MongoConfiguration MongoConfiguration;
        protected Container Container;

        [SetUp]
        public void SetUpContainer()
        {
            var tempContainer = new Container(x =>
            {
                x.AddRegistry<CommonRegistry>();
                x.AddRegistry<LoggingRegistry>();
            });

            var configurationMgr = tempContainer.GetInstance<IConfigurationManager>();
            var configurationStorageConnectionString =
                configurationMgr.GetAppSetting<string>("ConfigurationStorageConnectionString");

            Container = new Container(x =>
            {
                x.AddRegistry(new CommonRegistry(new CacheConfiguration(), configurationStorageConnectionString));
                x.AddRegistry<LoggingRegistry>();
                x.AddRegistry<ApplicationRepositoryRegistry>();
                x.AddRegistry<CandidateRepositoryRegistry>();
                x.AddRegistry<UserRepositoryRegistry>();
                x.AddRegistry<CommunicationRepositoryRegistry>();
                x.AddRegistry<VacancyRepositoryRegistry>();
            });

            var configurationManager = Container.GetInstance<IConfigurationService>();
            MongoConfiguration = configurationManager.Get<MongoConfiguration>();
        }
    }
}
