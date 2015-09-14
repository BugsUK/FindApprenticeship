namespace SFA.Apprenticeships.Web.Recruit.UnitTests.Mediators.Provider
{
    using FluentAssertions;
    using NUnit.Framework;
    using Recruit.Mediators.Provider;
    using ViewModels.Provider;

    [TestFixture]
    public class ProviderMediatorTests
    {
        [Test]
        [Ignore]
        public void AddSite_ShouldDefaultSearchMode()
        {
            // Arrange.
            var mediator = new ProviderMediator();

            // Act.
            var response = mediator.AddSite();

            // Assert.
            response.Should().NotBeNull();
            response.ViewModel.SiteSearchMode.Should().Be(ProviderSiteSearchMode.EmployerReferenceNumber);
        }
    }
}
