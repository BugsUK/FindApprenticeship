namespace SFA.Apprenticeships.Infrastructure.Communication.IntegrationTests
{
    using Common.IoC;
    using Configuration;
    using Domain.Interfaces.Configuration;
    using Email;
    using IoC;
    using Logging.IoC;
    using NUnit.Framework;
    using System.Linq;
    using StructureMap;

    [TestFixture]
    public class SendGridConfigurationTests
    {
        private SendGridConfiguration _sendGridConfiguration = null;
        [SetUp]
        public void SetUp()
        {
            var container = new Container(x =>
            {
                x.AddRegistry<CommonRegistry>();
                x.AddRegistry<LoggingRegistry>();
                x.AddRegistry<CommunicationRegistry>();
            });

            _sendGridConfiguration =
                container.GetInstance<IConfigurationService>()
                    .Get<SendGridConfiguration>(SendGridConfiguration.EmailConfigurationName);
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
