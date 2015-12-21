namespace SFA.Apprenticeships.Web.Recruit.UnitTests.Mediators.Locations
{
    using System;
    using Common.UnitTests.Mediators;
    using FluentAssertions;
    using Moq;
    using NUnit.Framework;
    using Raa.Common.ViewModels.VacancyPosting;
    using Recruit.Mediators.VacancyPosting;

    [TestFixture]
    public class LocationsTests : TestsBase
    {
        [Test]
        public void ShouldReturnAViewModelWithTheCurrentPageAsOne()
        {
            var vacancyGuid = Guid.NewGuid();
            const string ukprn = "ukprn";
            const string ern = "ern";
            const string providerSiteErn = "providerSiteErn";

            VacancyPostingProvider.Setup(
                p =>
                    p.LocationAddressesViewModel(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(),
                        It.IsAny<Guid>())).Returns(new LocationSearchViewModel());

            var mediator = GetMediator();

            var result = mediator.GetLocationAddressesViewModel(providerSiteErn, ern, ukprn, vacancyGuid, false);

            result.ViewModel.CurrentPage.Should().Be(1);
        }
    }
}