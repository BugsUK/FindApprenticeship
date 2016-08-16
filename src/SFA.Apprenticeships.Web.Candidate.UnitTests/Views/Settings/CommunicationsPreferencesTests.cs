namespace SFA.Apprenticeships.Web.Candidate.UnitTests.Views.Settings
{
    using Builders;
    using FluentAssertions;
    using NUnit.Framework;

    [TestFixture]
    [Parallelizable]
    public class CommunicationsPreferencesTests
    {
        [TestCase(true)]
        [TestCase(false)]
        public void US616_AC4_PhoneVerifiedIndication(bool verifiedMobile)
        {
            var viewModel = new SettingsViewModelBuilder().VerifiedMobile(verifiedMobile).Build();
            var result = new SettingsViewBuilder().With(viewModel).Render();
            
            var allowEmailCommsCheckBox = result.GetElementbyId("verifyContainer");

            if (verifiedMobile)
            {
                allowEmailCommsCheckBox.Should().NotBeNull();
                allowEmailCommsCheckBox.ChildNodes["span"].InnerText.Should().Be("Verified");
            }
            else
            {
                allowEmailCommsCheckBox.Should().BeNull();
            }
        }
    }
}