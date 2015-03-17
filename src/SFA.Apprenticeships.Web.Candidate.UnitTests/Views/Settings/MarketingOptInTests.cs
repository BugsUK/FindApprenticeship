namespace SFA.Apprenticeships.Web.Candidate.UnitTests.Views.Settings
{
    using Builders;
    using FluentAssertions;
    using NUnit.Framework;

    [TestFixture]
    public class MarketingOptInTests
    {
        [TestCase(false, false, false, false)]
        [TestCase(true, true, true, true)]
        [TestCase(true, false, true, true)]
        [TestCase(false, true, false, false)]
        public void US519_AC2_AC3_MarketingPreferences(
            bool sendApplicationSubmitted,
            bool sendApplicationStatusChanges,
            bool sendApprenticeshipApplicationsExpiring,
            bool sendMarketingCommunications
            )
        {
            var viewModel = new SettingsViewModelBuilder()
                .SmsEnabled(true)
                .SendApplicationSubmitted(sendApplicationSubmitted)
                .SendApplicationStatusChanges(sendApplicationStatusChanges)
                .SendApprenticeshipApplicationsExpiring(sendApprenticeshipApplicationsExpiring)
                .SendMarketingComms(sendMarketingCommunications)
                .Build();

            var result = new SettingsViewBuilder().With(viewModel).Render();

            var sendApplicationSubmittedCheckBox = result.GetElementbyId("SendApplicationSubmitted");
            var sendApplicationStatusChangesCheckBox = result.GetElementbyId("SendApplicationStatusChanges");
            var sendApprenticeshipApplicationsExpiringCheckBox = result.GetElementbyId("SendApprenticeshipApplicationsExpiring");
            var sendMarketingCommsCheckBox = result.GetElementbyId("SendMarketingCommunications");

            sendApplicationSubmittedCheckBox.Should().NotBeNull();
            sendApplicationStatusChangesCheckBox.Should().NotBeNull();
            sendApprenticeshipApplicationsExpiringCheckBox.Should().NotBeNull();
            sendMarketingCommsCheckBox.Should().NotBeNull();

            sendApplicationSubmittedCheckBox.ParentNode.InnerText.Should().Be("you submit an application form");
            sendApplicationStatusChangesCheckBox.ParentNode.InnerText.Should().Be("the status of one of your applications changes");
            sendApprenticeshipApplicationsExpiringCheckBox.ParentNode.InnerText.Should().Be("an apprenticeship is approaching its closing date");
            sendMarketingCommsCheckBox.ParentNode.InnerText.Should().Be("we send you updates on news and information");

            if (sendApplicationSubmitted)
            {
                sendApplicationSubmittedCheckBox.Attributes["checked"].Should().NotBeNull();
            }
            else
            {
                sendApplicationSubmittedCheckBox.Attributes["checked"].Should().BeNull();
            }

            if (sendApplicationStatusChanges)
            {
                sendApplicationStatusChangesCheckBox.Attributes["checked"].Should().NotBeNull();
            }
            else
            {
                sendApplicationStatusChangesCheckBox.Attributes["checked"].Should().BeNull();
            }

            if (sendApprenticeshipApplicationsExpiring)
            {
                sendApprenticeshipApplicationsExpiringCheckBox.Attributes["checked"].Should().NotBeNull();
            }
            else
            {
                sendApprenticeshipApplicationsExpiringCheckBox.Attributes["checked"].Should().BeNull();
            }

            if (sendMarketingCommunications)
            {
                sendMarketingCommsCheckBox.Attributes["checked"].Should().NotBeNull();
            }
            else
            {
                sendMarketingCommsCheckBox.Attributes["checked"].Should().BeNull();
            }
        }

        [TestCase(true)]
        [TestCase(false)]
        public void SmsEnabledFeatureToggle(bool smsEnabled)
        {
            var viewModel = new SettingsViewModelBuilder().SmsEnabled(smsEnabled).Build();

            var result = new SettingsViewBuilder().With(viewModel).Render();

            var allowEmailCommsCheckBox = result.GetElementbyId("AllowEmailComms");
            var allowSmsCommsCheckBox = result.GetElementbyId("AllowSmsComms");

            allowEmailCommsCheckBox.Should().NotBeNull();
            allowEmailCommsCheckBox.ParentNode.InnerText.Should().Be("Email");

            if (smsEnabled)
            {
                allowSmsCommsCheckBox.Should().NotBeNull();
                allowSmsCommsCheckBox.ParentNode.InnerText.Should().Be("Text");
            }
            else
            {
                allowSmsCommsCheckBox.Should().BeNull();
            }
        }
    }
}