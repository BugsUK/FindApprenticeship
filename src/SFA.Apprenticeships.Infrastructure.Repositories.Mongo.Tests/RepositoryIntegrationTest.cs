namespace SFA.Apprenticeships.Infrastructure.Repositories.Mongo.Tests
{
    using NUnit.Framework;
    using Infrastructure.Common.IoC;
    using Logging.IoC;
    using Common.Configuration;
    using Mongo.Applications.IoC;
    using Mongo.Candidates.IoC;
    using Mongo.Communication.IoC;
    using Mongo.Users.IoC;
    using Mongo.Vacancies.IoC;

    using SFA.Apprenticeships.Application.Interfaces;
    using SFA.Infrastructure.Interfaces;
    using Sql.Schemas.Vacancy.IoC;
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
                x.AddRegistry<VacancyRepositoryRegistry>();
                x.AddRegistry<VacancyReferenceNumberRegistry>();
            });

            var configurationManager = Container.GetInstance<IConfigurationService>();
            MongoConfiguration = configurationManager.Get<MongoConfiguration>();
        }
    }
}
