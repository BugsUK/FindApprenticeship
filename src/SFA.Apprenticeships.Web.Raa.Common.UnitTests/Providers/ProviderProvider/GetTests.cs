namespace SFA.Apprenticeships.Web.Raa.Common.UnitTests.Providers.ProviderProvider
{
    using System.Linq;
    using Domain.Entities.Raa.Parties;
    using FluentAssertions;
    using NUnit.Framework;
    using Ploeh.AutoFixture;

    [TestFixture]
    public class GetTests : TestBase
    {
        [Test]
        public void ShouldGetProviderViewModel()
        {
            // Arrange.
            var providerProvider = GetProviderProvider();

            var provider = new Fixture()
                .Create<Provider>();

            MockProviderService.Setup(mock =>
                mock.GetProvider(provider.Ukprn))
                .Returns(provider);

            var providerSites = new Fixture()
                .Build<ProviderSite>()
                .CreateMany(5)
                .ToArray();

            MockProviderService.Setup(mock =>
                mock.GetProviderSites(provider.Ukprn))
                .Returns(providerSites);

            // Act.
            var viewModel = providerProvider.GetProviderViewModel(provider.Ukprn);

            // Assert.
            viewModel.Should().NotBeNull();

            viewModel.ProviderId.Should().Be(provider.ProviderId);
            viewModel.ProviderName.Should().Be(provider.Name);
            viewModel.IsMigrated.Should().Be(provider.IsMigrated);

            viewModel.ProviderSiteViewModels.Should().NotBeNull();
            viewModel.ProviderSiteViewModels.Count().Should().Be(providerSites.Length);
        }
    }
}
