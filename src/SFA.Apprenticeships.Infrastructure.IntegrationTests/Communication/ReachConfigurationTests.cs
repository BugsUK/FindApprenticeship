namespace SFA.Apprenticeships.Infrastructure.IntegrationTests.Communication
{
    using System.Linq;
    using Common.IoC;
    using SFA.Infrastructure.Interfaces;
    using Infrastructure.Communication.Configuration;
    using Infrastructure.Communication.IoC;
    using Logging.IoC;
    using NUnit.Framework;
    using StructureMap;

    [TestFixture]
    public class ReachConfigurationTests
    {
        private SmsConfiguration _smsConfiguration = null;
        [SetUp]
        public void SetUp()
        {
            var container = new Container(x =>
            {
                x.AddRegistry<CommonRegistry>();
                x.AddRegistry<LoggingRegistry>();
                x.AddRegistry<CommunicationRegistry>();
            });

            _smsConfiguration =
                container.GetInstance<IConfigurationService>()
                    .Get<SmsConfiguration>();
        }

        [Test, Category("Integration"), Category("SmokeTests")]
        public void ShouldGetUsernameConfiguration()
        {
            Assert.IsNotNullOrEmpty(_smsConfiguration.Username);
        }

        [Test, Category("Integration"), Category("SmokeTests")]
        public void ShouldGetPasswordConfiguration()
        {
            Assert.IsNotNullOrEmpty(_smsConfiguration.Password);
        }

        [Test, Category("Integration"), Category("SmokeTests")]
        public void ShouldGetOriginatorFromConfiguration()
        {
            Assert.IsNotNullOrEmpty(_smsConfiguration.Originator);
        }

        [Test, Category("Integration"), Category("SmokeTests")]
        public void ShouldGetUrlFromConfiguration()
        {
            Assert.IsNotNullOrEmpty(_smsConfiguration.Url);
        }

        [Test, Category("Integration"), Category("SmokeTests")]
        public void ShouldGetCallbackUrlFromConfiguration()
        {
            Assert.IsNotNull(_smsConfiguration.CallbackUrl);
        }

        [Test, Category("Integration"), Category("SmokeTests")]
        public void ShouldGetMultipleTemplates()
        {
            var templates = _smsConfiguration.Templates;

            Assert.IsNotNull(templates);
            Assert.IsTrue(templates.Count() > 1);
        }

        [TestCase(0), Category("Integration"), Category("SmokeTests")]
        [TestCase(1), Category("Integration"), Category("SmokeTests")]
        public void ShouldGetMultipleTemplateConfiguration(int index)
        {
            var template = _smsConfiguration.Templates.ElementAt(index);

            Assert.IsNotNull(template);
            Assert.IsNotNullOrEmpty(template.Name);
            Assert.IsNotNullOrEmpty(template.Message);
        }

        [Test, Category("IntegrationProd"), Category("SmokeTests")]
        public void ShouldGetFromNameConfiguration()
        {
            const int templateIndex = 0;
            const string expectedName = "MessageTypes.SendPasswordResetCode";

            var template = _smsConfiguration.Templates.ElementAt(templateIndex);

            Assert.AreEqual(expectedName, template.Name);
        }
    }
}
