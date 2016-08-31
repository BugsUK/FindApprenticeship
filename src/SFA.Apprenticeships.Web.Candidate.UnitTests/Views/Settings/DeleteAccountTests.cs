namespace SFA.Apprenticeships.Web.Candidate.UnitTests.Views.Settings
{
    using Builders;
    using FluentAssertions;
    using NUnit.Framework;

    [TestFixture]
    public class DeleteAccountTests
    {
        [Test]
        public void DeleteAccountContainer()
        {
            var viewModel = new SettingsViewModelBuilder().Build();

            var result = new SettingsViewBuilder().With(viewModel).Render();

            var savedSearchesHeading = result.GetElementbyId("deleteAccountHeading");
            savedSearchesHeading.Should().NotBeNull();
            savedSearchesHeading.InnerText.Should().Contain("Delete your account");
        }
    }
}