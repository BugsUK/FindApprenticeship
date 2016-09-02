namespace SFA.Apprenticeships.Web.Recruit.UnitTests.Mediators.Locations
{
    using System;
    using System.Collections.Generic;
    using Apprenticeships.Application.Interfaces.Locations;
    using Common.Constants;
    using Common.UnitTests.Mediators;
    using Common.ViewModels.Locations;
    using Constants.Messages;
    using Domain.Entities.Exceptions;
    using FluentAssertions;
    using Moq;
    using NUnit.Framework;
    using Ploeh.AutoFixture;
    using Raa.Common.ViewModels.VacancyPosting;
    using Recruit.Mediators.VacancyPosting;

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

        [Test]
        public void ShouldReturnErrorIfPostCodeAnywhereNotAccessible()
        {
            const string ukprn = "123567";

            var viewModel = new Fixture().Create<LocationSearchViewModel>();

            VacancyPostingProvider
                .Setup(p => p.AddLocations(viewModel))
                .Throws(new CustomException(ErrorCodes.GeoCodeLookupProviderFailed));

            var mediator = GetMediator();

            var result = mediator.AddLocations(viewModel, ukprn);

            result.AssertMessage(VacancyPostingMediatorCodes.CreateVacancy.FailedGeoCodeLookup, ApplicationPageMessages.PostcodeLookupFailed, UserMessageLevel.Error);

        }
    }
}