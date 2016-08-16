namespace SFA.Apprenticeships.Web.Recruit.UnitTests.Mediators.Locations
{
    using System;
    using FluentAssertions;
    using Moq;
    using NUnit.Framework;
    using Raa.Common.ViewModels.VacancyPosting;

    [TestFixture]
    [Parallelizable]
    public class LocationsTests : TestsBase
    {
        [Test]
        public void ShouldReturnAViewModelWithTheCurrentPageAsOne()
        {
            var vacancyGuid = Guid.NewGuid();
            const string ukprn = "ukprn";
            const int employerId = 5;
            const int providerSiteId = 44;

            VacancyPostingProvider.Setup(
                p =>
                    p.LocationAddressesViewModel(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>(),
                        It.IsAny<Guid>())).Returns(new LocationSearchViewModel());

            var mediator = GetMediator();

            var result = mediator.GetLocationAddressesViewModel(providerSiteId, employerId, ukprn, vacancyGuid, false);

            result.ViewModel.CurrentPage.Should().Be(1);
        }
    }
}