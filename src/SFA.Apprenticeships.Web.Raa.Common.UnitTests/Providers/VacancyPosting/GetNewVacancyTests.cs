namespace SFA.Apprenticeships.Web.Raa.Common.UnitTests.Providers.VacancyPosting
{
    using System;
    using Domain.Entities.Raa.Parties;
    using FluentAssertions;
    using Moq;
    using NUnit.Framework;
    using Ploeh.AutoFixture;

    [TestFixture]
    [Parallelizable]
    public class GetNewVacancyTests : TestBase
    {
        private const int ProviderSiteId = 1;
        private const int EmployerId = 2;
        private const int VacancyPartyId = 4;
        private static readonly Guid VacancyGuid = Guid.NewGuid();

        private static readonly VacancyParty VacancyParty = new VacancyParty
        {
            ProviderSiteId = ProviderSiteId,
            EmployerId = EmployerId
        };

        [SetUp]
        public void SetUp()
        {
            MockProviderService
                .Setup(mock => mock.GetVacancyParty(VacancyPartyId, true))
                .Returns(VacancyParty);
            MockEmployerService.Setup(s => s.GetEmployer(VacancyParty.EmployerId))
                .Returns(new Fixture().Build<Employer>().Create());
        }

        [Test]
        public void ShouldDefaultToPreferredSite()
        {
            // Arrange.
            var provider = GetVacancyPostingProvider();

            // Act.
            var viewModel = provider.GetNewVacancyViewModel(VacancyPartyId, VacancyGuid, null);

            // Assert.
            MockProviderService.Verify(mock =>
                mock.GetVacancyParty(VacancyPartyId, true), Times.Once);
            MockEmployerService.Verify(s => s.GetEmployer(EmployerId), Times.Once);

            viewModel.Should().NotBeNull();
            viewModel.OwnerParty.ProviderSiteId.Should().Be(ProviderSiteId);
        }
    }
}
