namespace SFA.Apprenticeships.Web.Raa.Common.UnitTests.Providers.VacancyPosting
{
    using System;
    using Domain.Entities.Locations;
    using Domain.Entities.Organisations;
    using Domain.Entities.Providers;
    using FluentAssertions;
    using Moq;
    using NUnit.Framework;

    [TestFixture]
    public class GetNewVacancyTests : TestBase
    {
        protected static readonly string ProviderSiteUrn = Guid.NewGuid().ToString();
        protected static readonly string Ern = Guid.NewGuid().ToString();
        protected static readonly string Ukprn = Guid.NewGuid().ToString();
        protected static readonly Guid VacancyGuid = Guid.NewGuid();

        private static readonly Employer Employer = new Employer
        {
            Ern = Ern,
            Address = new PostalAddress
            {
                GeoPoint = new GeoPoint()
            }
        };

        private static readonly ProviderSiteEmployerLink ProviderSiteEmployerLink = new ProviderSiteEmployerLink
        {
            ProviderSiteErn = ProviderSiteUrn,
            Employer = Employer
        };

        [SetUp]
        public void SetUp()
        {
            MockProviderService
                .Setup(mock => mock.GetProviderSiteEmployerLink(ProviderSiteUrn, Ern))
                .Returns(ProviderSiteEmployerLink);
        }

        [Test]
        public void ShouldDefaultToPreferredSite()
        {
            // Arrange.
            var provider = GetVacancyPostingProvider();

            // Act.
            var viewModel = provider.GetNewVacancyViewModel(Ukprn, ProviderSiteUrn, Ern, VacancyGuid, null);

            // Assert.
            MockProviderService.Verify(mock =>
                mock.GetProviderSiteEmployerLink(ProviderSiteUrn, Ern), Times.Once);

            viewModel.Should().NotBeNull();
            viewModel.ProviderSiteEmployerLink.ProviderSiteErn.Should().Be(ProviderSiteUrn);
        }
    }
}
