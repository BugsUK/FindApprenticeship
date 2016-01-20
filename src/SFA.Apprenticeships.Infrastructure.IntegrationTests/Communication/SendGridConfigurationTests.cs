namespace SFA.Apprenticeships.Infrastructure.IntegrationTests.Communication
{
    using System.Linq;
    using Common.Configuration;
    using Common.IoC;
    using SFA.Infrastructure.Interfaces;
    using Infrastructure.Communication.Configuration;
    using Infrastructure.Communication.IoC;
    using Logging.IoC;
    using NUnit.Framework;
    using StructureMap;

    [TestFixture]
    public class SendGridConfigurationTests
    {
        private EmailConfiguration _sendGridConfiguration = null;
        [SetUp]
        public void SetUp()
        {
            var configurationStorageConnectionString = SettingsTestHelper.GetStorageConnectionString();

            var container = new Container(x =>
            {
                x.AddRegistry(new CommonRegistry(new CacheConfiguration(), configurationStorageConnectionString));
                x.AddRegistry<LoggingRegistry>();
                x.AddRegistry<CommunicationRegistry>();
            });

            _sendGridConfiguration =
                container.GetInstance<IConfigurationService>()
                    .Get<EmailConfiguration>();
        }

        [Test, Category("Integration"), Category("SmokeTests")]
        public void ShouldGetUserNameConfiguration()
        {
            Assert.IsNotNull(_sendGridConfiguration.Username);
        }

        [Test, Category("Integration"), Category("SmokeTests")]
        public void ShouldGetPasswordConfiguration()
        {
            Assert.IsNotNull(_sendGridConfiguration.Password);
        }

        [Test, Category("Integration"), Category("SmokeTests")]
        public void ShouldGetMultipleTemplates()
        {
            var templates = _sendGridConfiguration.Templates;

            Assert.IsNotNull(templates);
            Assert.IsTrue(templates.Count() > 1);
        }

        [TestCase(0), Category("Integration"), Category("SmokeTests")]
        [TestCase(1), Category("Integration"), Category("SmokeTests")]
        public void ShouldGetMultipleTemplateConfiguration(int index)
        {
            var template = _sendGridConfiguration.Templates.ElementAt(index);

            Assert.IsNotNull(template);
            Assert.IsNotNull(template.Name);
            Assert.IsNotNull(template.Id);
        }

        [Test, Category("IntegrationProd"), Category("SmokeTests")]
        public void ShouldGetFromEmailConfiguration()
        {
            const int templateIndex = 0;
            const string expectedFromEmail = "nationalhelpdesk@findapprenticeship.service.gov.uk";

            var template = _sendGridConfiguration.Templates.ElementAt(templateIndex);

            Assert.AreEqual(expectedFromEmail, template.FromEmail);
        }
    }
}
