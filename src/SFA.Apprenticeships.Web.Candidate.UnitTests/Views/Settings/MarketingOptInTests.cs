namespace SFA.Apprenticeships.Web.Candidate.UnitTests.Views.Settings
{
    using Builders;
    using FluentAssertions;
    using NUnit.Framework;

    // TODO: US733: review / possibly extend unit tests.

    [TestFixture]
    public class MarketingOptInTests
    {
        [TestCase(false, false, false)]
        [TestCase(false, false, true)]
        [TestCase(false, true, false)]
        [TestCase(false, true, true)]
        [TestCase(true, true, true)]
        public void ShouldRenderEmailCommunicationPreferences(
            bool enableApplicationStatusChangeAlertsViaEmail,
            bool enableExpiringApplicationAlertsViaEmail,
            bool enableMarketingViaEmail)
        {
            var viewModel = new SettingsViewModelBuilder()
                .SmsEnabled(true)
                .EnableApplicationStatusChangeAlertsViaEmail(enableApplicationStatusChangeAlertsViaEmail)
                .EnableExpiringApplicationAlertsViaEmail(enableExpiringApplicationAlertsViaEmail)
                .EnableMarketingViaEmail(enableMarketingViaEmail)
                .Build();

            var result = new SettingsViewBuilder().With(viewModel).Render();

            var enableApplicationStatusChangeAlertsViaEmailCheckbox = result.GetElementbyId("EnableApplicationStatusChangeAlertsViaEmail");
            var enableApplicationStatusChangeAlertsViaTextCheckbox = result.GetElementbyId("EnableApplicationStatusChangeAlertsViaText");

            var enableExpiringApplicationAlertsViaEmailCheckbox = result.GetElementbyId("EnableExpiringApplicationAlertsViaEmail");
            var enableExpiringApplicationAlertsViaTextCheckbox = result.GetElementbyId("EnableExpiringApplicationAlertsViaText");

            var enableMarketingViaEmailCheckbox = result.GetElementbyId("EnableMarketingViaEmail");
            var enableMarketingViaTextCheckbox = result.GetElementbyId("EnableMarketingViaText");

            enableApplicationStatusChangeAlertsViaEmailCheckbox.Should().NotBeNull();
            enableApplicationStatusChangeAlertsViaTextCheckbox.Should().NotBeNull();

            enableExpiringApplicationAlertsViaEmailCheckbox.Should().NotBeNull();
            enableExpiringApplicationAlertsViaTextCheckbox.Should().NotBeNull();

            enableMarketingViaEmailCheckbox.Should().NotBeNull();
            enableMarketingViaTextCheckbox.Should().NotBeNull();

            enableApplicationStatusChangeAlertsViaEmailCheckbox.ParentNode.InnerText.Should().Be(string.Empty);
            enableExpiringApplicationAlertsViaEmailCheckbox.ParentNode.InnerText.Should().Be(string.Empty);
            enableMarketingViaEmailCheckbox.ParentNode.InnerText.Should().Be(string.Empty);

            if (enableApplicationStatusChangeAlertsViaEmail)
            {
                enableApplicationStatusChangeAlertsViaEmailCheckbox.Attributes["checked"].Should().NotBeNull();
            }
            else
            {
                enableApplicationStatusChangeAlertsViaEmailCheckbox.Attributes["checked"].Should().BeNull();
            }

            if (enableExpiringApplicationAlertsViaEmail)
            {
                enableExpiringApplicationAlertsViaEmailCheckbox.Attributes["checked"].Should().NotBeNull();
            }
            else
            {
                enableExpiringApplicationAlertsViaEmailCheckbox.Attributes["checked"].Should().BeNull();
            }

            if (enableMarketingViaEmail)
            {
                enableMarketingViaEmailCheckbox.Attributes["checked"].Should().NotBeNull();
            }
            else
            {
                enableMarketingViaEmailCheckbox.Attributes["checked"].Should().BeNull();
            }
        }

        [TestCase(false, false, false)]
        [TestCase(false, false, true)]
        [TestCase(false, true, false)]
        [TestCase(false, true, true)]
        [TestCase(true, true, true)]
        public void ShouldRenderTextCommunicationPreferences(
            bool enableApplicationStatusChangeAlertsViaText,
            bool enableExpiringApplicationAlertsViaText,
            bool enableMarketingViaText)
        {
            var viewModel = new SettingsViewModelBuilder()
                .SmsEnabled(true)
                .EnableApplicationStatusChangeAlertsViaText(enableApplicationStatusChangeAlertsViaText)
                .EnableExpiringApplicationAlertsViaText(enableExpiringApplicationAlertsViaText)
                .EnableMarketingViaText(enableMarketingViaText)
                .Build();

            var result = new SettingsViewBuilder().With(viewModel).Render();

            var enableApplicationStatusChangeAlertsViaTextCheckbox = result.GetElementbyId("EnableApplicationStatusChangeAlertsViaText");
            var enableExpiringApplicationAlertsViaTextCheckbox = result.GetElementbyId("EnableExpiringApplicationAlertsViaText");
            var enableMarketingViaTextCheckbox = result.GetElementbyId("EnableMarketingViaText");

            enableApplicationStatusChangeAlertsViaTextCheckbox.Should().NotBeNull();
            enableExpiringApplicationAlertsViaTextCheckbox.Should().NotBeNull();
            enableMarketingViaTextCheckbox.Should().NotBeNull();

            enableApplicationStatusChangeAlertsViaTextCheckbox.ParentNode.InnerText.Should().Be("the status of one of your applications changes");
            enableExpiringApplicationAlertsViaTextCheckbox.ParentNode.InnerText.Should().Be("an apprenticeship is approaching its closing date");
            enableMarketingViaTextCheckbox.ParentNode.InnerText.Should().Be("we send you updates on news and information");

            if (enableApplicationStatusChangeAlertsViaText)
            {
                enableApplicationStatusChangeAlertsViaTextCheckbox.Attributes["checked"].Should().NotBeNull();
            }
            else
            {
                enableApplicationStatusChangeAlertsViaTextCheckbox.Attributes["checked"].Should().BeNull();
            }

            if (enableExpiringApplicationAlertsViaText)
            {
                enableExpiringApplicationAlertsViaTextCheckbox.Attributes["checked"].Should().NotBeNull();
            }
            else
            {
                enableExpiringApplicationAlertsViaTextCheckbox.Attributes["checked"].Should().BeNull();
            }

            if (enableMarketingViaText)
            {
                enableMarketingViaTextCheckbox.Attributes["checked"].Should().NotBeNull();
            }
            else
            {
                enableMarketingViaTextCheckbox.Attributes["checked"].Should().BeNull();
            }
        }
    }
}