namespace SFA.Apprenticeships.Infrastructure.Communication.IntegrationTests
{
    using System.Linq;
    using NUnit.Framework;
    using Sms;

    [TestFixture]
    public class ReachConfigurationTests
    {
        [Test, Category("Integration"), Category("SmokeTests")]
        public void ShouldGetUsernameConfiguration()
        {
            Assert.IsNotNullOrEmpty(ReachSmsConfiguration.Instance.Username);
        }

        [Test, Category("Integration"), Category("SmokeTests")]
        public void ShouldGetPasswordConfiguration()
        {
            Assert.IsNotNullOrEmpty(ReachSmsConfiguration.Instance.Password);
        }

        [Test, Category("Integration"), Category("SmokeTests")]
        public void ShouldGetOriginatorFromConfiguration()
        {
            Assert.IsNotNullOrEmpty(ReachSmsConfiguration.Instance.Originator);
        }

        [Test, Category("Integration"), Category("SmokeTests")]
        public void ShouldGetUrlFromConfiguration()
        {
            Assert.IsNotNullOrEmpty(ReachSmsConfiguration.Instance.Url);
        }

        [Test, Category("Integration"), Category("SmokeTests")]
        public void ShouldGetCallbackUrlFromConfiguration()
        {
            Assert.IsNotNull(ReachSmsConfiguration.Instance.CallbackUrl);
        }

        [Test, Category("Integration"), Category("SmokeTests")]
        public void ShouldGetMultipleTemplates()
        {
            var templates = ReachSmsConfiguration.Instance.TemplateCollection;

            Assert.IsNotNull(templates);
            Assert.IsTrue(templates.Count > 1);
        }

        [TestCase(0), Category("Integration"), Category("SmokeTests")]
        [TestCase(1), Category("Integration"), Category("SmokeTests")]
        public void ShouldGetMultipleTemplateConfiguration(int index)
        {
            var template = ReachSmsConfiguration.Instance.Templates.ElementAt(index);

            Assert.IsNotNull(template);
            Assert.IsNotNullOrEmpty(template.Name);
            Assert.IsNotNullOrEmpty(template.Message);
        }

        [Test, Category("IntegrationProd"), Category("SmokeTests")]
        public void ShouldGetFromNameConfiguration()
        {
            const int templateIndex = 0;
            const string expectedName = "MessageTypes.SendPasswordResetCode";

            var template = ReachSmsConfiguration.Instance.Templates.ElementAt(templateIndex);

            Assert.AreEqual(expectedName, template.Name);
        }
    }
}
