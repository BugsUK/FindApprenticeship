namespace SFA.Apprenticeships.Web.Candidate.AcceptanceTests.IoC
{
    using Infrastructure.Common.IoC;
    using Infrastructure.Logging.IoC;
    using Infrastructure.Repositories.Mongo.Applications.IoC;
    using Infrastructure.Repositories.Mongo.Audit.IoC;
    using Infrastructure.Repositories.Mongo.Candidates.IoC;
    using Infrastructure.Repositories.Mongo.Users.IoC;
    using Infrastructure.UserDirectory.IoC;
    using StructureMap;
    using TechTalk.SpecFlow;

    [Binding]
    public class WebTestRegistry
    {
        public static Container Container;

        [BeforeTestRun]
        public static void BeforeTestRun()
        {
            Container = new Container(x =>
            {
                x.AddRegistry<CommonRegistry>();
                x.AddRegistry<LoggingRegistry>();
                x.AddRegistry<AuditRepositoryRegistry>();
                x.AddRegistry<UserRepositoryRegistry>();
                x.AddRegistry<CandidateRepositoryRegistry>();
                x.AddRegistry<UserDirectoryRegistry>();
                x.AddRegistry<ApplicationRepositoryRegistry>();
            });
        }
    }
}
