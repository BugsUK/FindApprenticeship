using SFA.Apprenticeships.Web.Raa.Common.ViewModels.Provider;

namespace SFA.Apprenticeships.Web.Recruit.UnitTests.Mediators.Provider
{
    using FluentAssertions;
    using NUnit.Framework;
    using Recruit.Mediators.Provider;

    [TestFixture]
    [Parallelizable]
    public class ProviderMediatorTests
    {
        [Test]
        [Ignore("Review")]
        public void AddSite_ShouldDefaultSearchMode()
        {
            // Arrange.
            var mediator = new ProviderMediator(null, null, null, null, null, null);

            // Act.
            var response = mediator.AddSite();

            // Assert.
            response.Should().NotBeNull();
            response.ViewModel.Should().NotBeNull();
            response.ViewModel.SiteSearchMode.Should().Be(ProviderSiteSearchMode.EmployerReferenceNumber);
        }
    }
}
