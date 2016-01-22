namespace SFA.Apprenticeships.Web.Recruit.EndToEndTests
{
    using System.Collections;
    using System.Collections.Specialized;
    using System.Security.Principal;
    using System.Threading;
    using System.Web;
    using Common.Providers;
    using Common.Services;
    using Infrastructure.Repositories.Mongo.Common.Configuration;
    using Infrastructure.Repositories.Mongo.Vacancies.Entities;
    using SFA.Infrastructure.Interfaces;
    using IoC;
    using MongoDB.Driver;
    using Moq;
    using NUnit.Framework;
    using StructureMap;
    using StructureMap.Web.Pipeline;

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

            ConfigureIoCContainer();
            
            var configurationManager = Container.GetInstance<IConfigurationService>();
            MongoConfiguration = configurationManager.Get<MongoConfiguration>();

            Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity(QaUserName), null);

            var mongoConnectionString = MongoConfiguration.VacancyDb;
            var mongoDbName = MongoUrl.Create(mongoConnectionString).DatabaseName;

            var database = new MongoClient(mongoConnectionString)
                .GetServer()
                .GetDatabase(mongoDbName);

            Collection = database.GetCollection<MongoApprenticeshipVacancy>("apprenticeshipVacancies");
            ClearCollection();
        }

        [TearDown]
        public void TearDown()
        {
            ClearCollection();
        }

        private void ClearCollection()
        {
            Collection?.RemoveAll();
        }

        private void ConfigureIoCContainer()
        {
            Container.EjectAllInstancesOf<HttpContextBase>();
            Container.EjectAllInstancesOf<IUserDataProvider>();
            Container.Configure(Configure);
        }

        private static void Configure(ConfigurationExpression obj)
        {
            obj.For<IUserDataProvider>().LifecycleIs<HybridLifecycle>().Use<FakeUserDataProvider>();
            obj.For<HttpContextBase>().LifecycleIs<HybridLifecycle>().Use(ctx => CreateMockContext(true));
            obj.For<IAuthenticationTicketService>().LifecycleIs<HybridLifecycle>().Use<AuthenticationTicketService>();
            obj.For<IUserDataProvider>().LifecycleIs<HybridLifecycle>().Use<CookieUserDataProvider>();
        }

        private static HttpContextBase CreateMockContext(bool isAuthenticated)
        {
            // Response.
            var mockRequest = new Mock<HttpRequestBase>(MockBehavior.Loose);

            mockRequest.Setup(m => m.IsLocal).Returns(false);
            mockRequest.Setup(m => m.ApplicationPath).Returns("/");
            mockRequest.Setup(m => m.ServerVariables).Returns(new NameValueCollection());
            mockRequest.Setup(m => m.RawUrl).Returns(string.Empty);
            mockRequest.Setup(m => m.Cookies).Returns(new HttpCookieCollection());
            mockRequest.Setup(m => m.IsAuthenticated).Returns(isAuthenticated);

            // Request.
            var mockResponse = new Mock<HttpResponseBase>(MockBehavior.Loose);

            mockResponse.Setup(m => m.ApplyAppPathModifier(It.IsAny<string>())).Returns<string>(virtualPath => virtualPath);
            mockResponse.Setup(m => m.Cookies).Returns(new HttpCookieCollection());

            // HttpContext.
            var mockHttpContext = new Mock<HttpContextBase>(MockBehavior.Loose);

            mockHttpContext.Setup(m => m.Items).Returns(new Hashtable());
            mockHttpContext.Setup(m => m.Request).Returns(mockRequest.Object);
            mockHttpContext.Setup(m => m.Response).Returns(mockResponse.Object);

            return mockHttpContext.Object;
        }
    }
}
