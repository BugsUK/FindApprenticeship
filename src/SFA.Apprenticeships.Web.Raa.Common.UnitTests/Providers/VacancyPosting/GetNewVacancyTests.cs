namespace SFA.Apprenticeships.Web.Raa.Common.UnitTests.Providers.VacancyPosting
{
    using System;
    using Domain.Entities.Raa.Locations;
    using Domain.Entities.Raa.Parties;
    using FluentAssertions;
    using Moq;
    using NUnit.Framework;

    [TestFixture]
    public class GetNewVacancyTests : TestBase
    {
        protected const int ProviderSiteId = 1;
        protected const int EmployerId = 2;
        protected const int ProviderId = 3;
        protected static readonly string ProviderSiteUrn = Guid.NewGuid().ToString();
        protected static readonly string EdsErn = Guid.NewGuid().ToString();
        protected static readonly string Ukprn = Guid.NewGuid().ToString();
        protected static readonly Guid VacancyGuid = Guid.NewGuid();

        private static readonly Employer Employer = new Employer
        {
            EdsErn = EdsErn,
            Address = new PostalAddress
            {
                GeoPoint = new GeoPoint()
            }
        };

        private static readonly VacancyParty VacancyParty = new VacancyParty
        {
            ProviderSiteId = ProviderSiteId,
            EmployerId = EmployerId
        };

        [SetUp]
        public void SetUp()
        {
            MockProviderService
                .Setup(mock => mock.GetVacancyParty(ProviderSiteId, EmployerId))
                .Returns(VacancyParty);
        }

        [Test]
        public void ShouldDefaultToPreferredSite()
        {
            // Arrange.
            var provider = GetVacancyPostingProvider();

            // Act.
            var viewModel = provider.GetNewVacancyViewModel(ProviderId, ProviderSiteId, EmployerId, VacancyGuid, null);

            // Assert.
            MockProviderService.Verify(mock =>
                mock.GetVacancyParty(ProviderSiteId, EmployerId), Times.Once);

            viewModel.Should().NotBeNull();
            viewModel.ProviderSiteEmployerLink.ProviderSiteErn.Should().Be(ProviderSiteUrn);
        }
    }
}
