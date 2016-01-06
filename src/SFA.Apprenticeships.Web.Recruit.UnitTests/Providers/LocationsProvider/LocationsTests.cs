namespace SFA.Apprenticeships.Web.Recruit.UnitTests.Providers.LocationsProvider
{
    using System;
    using System.Collections.Generic;
    using Common.ViewModels.Locations;
    using Domain.Entities.Locations;
    using Domain.Entities.Vacancies.ProviderVacancies.Apprenticeship;
    using FluentAssertions;
    using NUnit.Framework;
    using Raa.Common.ViewModels.VacancyPosting;

    [TestFixture]
    public class LocationsTests : TestBase
    {
        [Test]
        public void WhenCreatingANewVacancyShouldReturnANewLocationSearchViewModel()
        {
            var vacancyGuid = Guid.NewGuid();
            const string ukprn = "ukprn";
            const string ern = "ern";
            const string providerSiteErn = "providerSiteErn";

            var provider = GetVacancyPostingProvider();

            var result = provider.LocationAddressesViewModel(ukprn, providerSiteErn, ern, vacancyGuid);

            result.Ukprn.Should().Be(ukprn);
            result.Ern.Should().Be(ern);
            result.ProviderSiteErn.Should().Be(providerSiteErn);
            result.VacancyGuid.Should().Be(vacancyGuid);
            result.Addresses.Should().HaveCount(0);
        }

        [Test]
        public void IfTheVacancyAlreadyExistsShouldFillTheViewModelWithItsInformation()
        {
            var vacancyGuid = Guid.NewGuid();
            const string ukprn = "ukprn";
            const string ern = "ern";
            const string providerSiteErn = "providerSiteErn";
            const string additionalLocationInformation = "additional location information";
            var addresses = new List<VacancyLocationAddress>
            {
                new VacancyLocationAddress
                {
                    Address = new Address
                    {
                        AddressLine4 = "address line 4 - 1",
                        AddressLine3 = "address line 3 - 1",
                        AddressLine2 = "address line 2 - 1",
                        AddressLine1 = "address line 1 - 1",
                        Postcode = "postcode",
                        Uprn = "uprn"
                    },
                    NumberOfPositions = 2
                },
                new VacancyLocationAddress
                {
                    Address = new Address
                    {
                        AddressLine4 = "address line 4 - 1",
                        AddressLine3 = "address line 3 - 1",
                        AddressLine2 = "address line 2 - 1",
                        AddressLine1 = "address line 1 - 1",
                        Postcode = "postcode",
                        Uprn = "uprn"
                    },
                    NumberOfPositions = 2
                }
            };

            var vacancy = new ApprenticeshipVacancy
            {
                AdditionalLocationInformation = additionalLocationInformation,
                LocationAddresses = addresses
            };

            MockVacancyPostingService.Setup(s => s.GetVacancy(vacancyGuid)).Returns(vacancy);
            var provider = GetVacancyPostingProvider();

            var result = provider.LocationAddressesViewModel(ukprn, providerSiteErn, ern, vacancyGuid);

            result.Addresses.Count.Should().Be(2);
        }
    }
}