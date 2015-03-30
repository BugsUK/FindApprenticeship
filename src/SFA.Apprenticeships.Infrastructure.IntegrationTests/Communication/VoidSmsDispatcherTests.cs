namespace SFA.Apprenticeships.Infrastructure.IntegrationTests.Communication
{
    using Application.Interfaces.Communications;
    using Common.IoC;
    using Infrastructure.Communication.IoC;
    using Infrastructure.Communication.Sms;
    using Logging.IoC;
    using NUnit.Framework;
    using StructureMap;

    [TestFixture]
    public class VoidSmsDispatcherTests
    {
        private ISmsDispatcher _voidSmsDispatcher;

        [SetUp]
        public void SetUp()
        {
            var container = new Container(x =>
            {
                x.AddRegistry<CommonRegistry>();
                x.AddRegistry<LoggingRegistry>();
                x.AddRegistry<CommunicationRegistry>();
            });

            _voidSmsDispatcher = container.GetInstance<ISmsDispatcher>("VoidSmsDispatcher");
        }

        [Test, Category("Integration"), Category("SmokeTests")]
        public void ShouldConstructVoidSmsDispatcher()
        {
            Assert.IsNotNull(_voidSmsDispatcher);
            Assert.IsInstanceOf<VoidSmsDispatcher>(_voidSmsDispatcher);
        }
    }
}