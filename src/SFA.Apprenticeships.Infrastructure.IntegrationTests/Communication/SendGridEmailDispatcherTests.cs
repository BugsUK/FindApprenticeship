namespace SFA.Apprenticeships.Infrastructure.IntegrationTests.Communication
{
    using Application.Interfaces.Communications;
    using Common.Configuration;
    using SFA.Infrastructure.Interfaces;
    using Common.IoC;
    using Infrastructure.Communication.Email;
    using Infrastructure.Communication.IoC;
    using Moq;
    using NUnit.Framework;
    using StructureMap;

    [TestFixture]
    public class SendGridEmailDispatcherTests
    {
        private IEmailDispatcher _dispatcher;
        private IEmailDispatcher _voidEmailDispatcher;

        private readonly Mock<ILogService> _logServiceMock = new Mock<ILogService>();

        [SetUp]
        public void SetUp()
        {
            var configurationStorageConnectionString = SettingsTestHelper.GetStorageConnectionString();

            var container = new Container(x =>
            {
                x.AddRegistry(new CommonRegistry(new CacheConfiguration(), configurationStorageConnectionString));
                x.AddRegistry<CommunicationRegistry>();
            });

            container.Configure(c => c.For<ILogService>().Use(_logServiceMock.Object));

            _dispatcher = container.GetInstance<IEmailDispatcher>("SendGridEmailDispatcher");
            _voidEmailDispatcher = container.GetInstance<IEmailDispatcher>("VoidEmailDispatcher");

            _logServiceMock.ResetCalls();
        }

        [Test, Category("Integration"), Category("SmokeTests")]
        public void ShoudConstructSendGridEmailDispatcher()
        {
            Assert.IsNotNull(_dispatcher);
            Assert.IsInstanceOf<SendGridEmailDispatcher>(_dispatcher);
        }

        [Test, Category("Integration"), Category("SmokeTests")]
        public void ShouldConstructVoidEmailDispatcher()
        {
            Assert.IsNotNull(_voidEmailDispatcher);
            Assert.IsInstanceOf<VoidEmailDispatcher>(_voidEmailDispatcher);
        }
    }
}