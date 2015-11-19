namespace SFA.Apprenticeships.Web.Recruit.EndToEndTests
{
    using System.Security.Principal;
    using System.Threading;
    using Domain.Interfaces.Configuration;
    using Infrastructure.Mongo.Common.Configuration;
    using Infrastructure.Repositories.Vacancies.Entities;
    using IoC;
    using MongoDB.Driver;
    using NUnit.Framework;
    using StructureMap;

    public class RecruitWebEndToEndTestsBase
    {
        protected MongoConfiguration MongoConfiguration;
        protected IContainer Container;
        protected IContainer Container2;
        protected MongoCollection<MongoApprenticeshipVacancy> Collection;
        protected string QaUserName = "qaUserName";

        [SetUp]
        public void SetUpContainer()
        {
            Container = IoC.Initialize();

            var configurationManager = Container.GetInstance<IConfigurationService>();
            MongoConfiguration = configurationManager.Get<MongoConfiguration>();

            Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity(QaUserName), null);
        }

        [TearDown]
        public void TearDown()
        {
            if (Collection != null)
                Collection.RemoveAll();
        }
    }
}
