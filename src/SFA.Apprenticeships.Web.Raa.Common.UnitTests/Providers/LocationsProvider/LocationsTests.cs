namespace SFA.Apprenticeships.Web.Raa.Common.UnitTests.Providers.LocationsProvider
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Domain.Entities.Locations;
    using Domain.Entities.Vacancies.ProviderVacancies.Apprenticeship;
    using FluentAssertions;
    using Moq;
    using NUnit.Framework;
    using ViewModels.VacancyPosting;
    using Web.Common.ViewModels.Locations;

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
            
            var vacancy = GetVacancyWithLocationAddresses(additionalLocationInformation);

            MockVacancyPostingService.Setup(s => s.GetVacancy(vacancyGuid)).Returns(vacancy);
            var provider = GetVacancyPostingProvider();

            var result = provider.LocationAddressesViewModel(ukprn, providerSiteErn, ern, vacancyGuid);

            result.Addresses.Count.Should().Be(2);
        }

        [Test]
        public void AddLocationsShouldCallReplaceLocationInformation()
        {
            var vacancyReferenceNumber = 1L;
            const string additionalLocationInformation = "additional location information";
            const string aNewAdditionalLocationInformation = "a new additional location information";
            const string aNewAdditionalLocationInformationComment = "a new additional location information comment";
            const string aNewLocationAddressesComment = "a new additional location addresses comment";

            var vacancy = GetVacancyWithLocationAddresses(additionalLocationInformation);

            MockVacancyPostingService.Setup(s => s.GetVacancy(vacancyReferenceNumber)).Returns(vacancy);
            var provider = GetVacancyPostingProvider();

            var locationSearchViewModel = GetLocationSearchViewModel(vacancy.EntityId, aNewAdditionalLocationInformation,
                aNewAdditionalLocationInformationComment, vacancyReferenceNumber, aNewLocationAddressesComment);

            provider.AddLocations(locationSearchViewModel);

            MockVacancyPostingService.Verify(
                s =>
                    s.ReplaceLocationInformation(vacancy.EntityId, locationSearchViewModel.IsEmployerLocationMainApprenticeshipLocation,
                        null, It.Is<IEnumerable<VacancyLocationAddress>>(l => l.Count() == 3), aNewLocationAddressesComment,
                        aNewAdditionalLocationInformation, aNewAdditionalLocationInformationComment ));
        }

        [Test]
        public void RemoveVacancyLocationInformationShouldCallReplaceLocationInformation()
        {
            var vacancyReferenceNumber = 1L;
            var vacancyGuid = Guid.NewGuid();
            var vacancy = GetVacancyWithLocationAddresses(vacancyGuid, vacancyReferenceNumber);

            MockVacancyPostingService.Setup(s => s.GetVacancy(vacancyGuid)).Returns(vacancy);
            var provider = GetVacancyPostingProvider();

            provider.RemoveVacancyLocationInformation(vacancyGuid);

            MockVacancyPostingService.Verify(
                s =>
                    s.ReplaceLocationInformation(vacancyGuid, null, null,
                        It.Is<IEnumerable<VacancyLocationAddress>>(l => !l.Any()), null, null, null));
        }

        [Test]
        public void RemoveLocationAddressesShouldCallReplaceLocationInformation()
        {
            var vacancyReferenceNumber = 1L;
            var vacancyGuid = Guid.NewGuid();
            const string aComment = "a comment";
            const int numberOfPositions = 2;
            const bool isEmployerLocationMainApprenticeshipLocation = false;

            var vacancy = GetVacancyWithLocationAddresses(vacancyGuid, vacancyReferenceNumber, numberOfPositions, isEmployerLocationMainApprenticeshipLocation, aComment, aComment, string.Empty);

            MockVacancyPostingService.Setup(s => s.GetVacancy(vacancyGuid)).Returns(vacancy);
            var provider = GetVacancyPostingProvider();

            provider.RemoveLocationAddresses(vacancyGuid);

            MockVacancyPostingService.Verify(
                s =>
                    s.ReplaceLocationInformation(vacancyGuid, isEmployerLocationMainApprenticeshipLocation, numberOfPositions,
                        It.Is<IEnumerable<VacancyLocationAddress>>(l => !l.Any()), aComment, null, aComment));
        }

        private static LocationSearchViewModel GetLocationSearchViewModel(Guid vacancyGuid, string aNewAdditionalLocationInformation,
            string aNewAdditionalLocationInformationComment, long vacancyReferenceNumber, string aNewLocationAddressesComment)
        {
            var locationSearchViewModel = new LocationSearchViewModel
            {
                AdditionalLocationInformation = aNewAdditionalLocationInformation,
                AdditionalLocationInformationComment = aNewAdditionalLocationInformationComment,
                IsEmployerLocationMainApprenticeshipLocation = false,
                VacancyReferenceNumber = vacancyReferenceNumber,
                Addresses = new List<VacancyLocationAddressViewModel>
                {
                    new VacancyLocationAddressViewModel
                    {
                        Address = new AddressViewModel
                        {
                            AddressLine4 = "address line 4 - 1",
                            AddressLine3 = "address line 3 - 1",
                            AddressLine2 = "address line 2 - 1",
                            AddressLine1 = "address line 1 - 1",
                            Postcode = "postcode",
                            Uprn = "uprn"
                        },
                        NumberOfPositions = 1
                    },
                    new VacancyLocationAddressViewModel
                    {
                        Address = new AddressViewModel
                        {
                            AddressLine4 = "address line 4 - 2",
                            AddressLine3 = "address line 3 - 2",
                            AddressLine2 = "address line 2 - 2",
                            AddressLine1 = "address line 1 - 2",
                            Postcode = "postcode",
                            Uprn = "uprn"
                        },
                        NumberOfPositions = 1
                    },
                    new VacancyLocationAddressViewModel
                    {
                        Address = new AddressViewModel
                        {
                            AddressLine4 = "address line 4 - 3",
                            AddressLine3 = "address line 3 - 3",
                            AddressLine2 = "address line 2 - 3",
                            AddressLine1 = "address line 1 - 3",
                            Postcode = "postcode",
                            Uprn = "uprn"
                        },
                        NumberOfPositions = 1
                    }
                },
                LocationAddressesComment = aNewLocationAddressesComment,
                VacancyGuid = vacancyGuid
            };
            return locationSearchViewModel;
        }

        private static ApprenticeshipVacancy GetVacancyWithLocationAddresses(string additionalLocationInformation)
        {
            return GetVacancyWithLocationAddresses(Guid.NewGuid(), 1L, additionalLocationInformation);
        }

        private static ApprenticeshipVacancy GetVacancyWithLocationAddresses(Guid vacancyGuid, long vacancyReferenceNumber)
        {
            return GetVacancyWithLocationAddresses(vacancyGuid, vacancyReferenceNumber, string.Empty);
        }

        private static ApprenticeshipVacancy GetVacancyWithLocationAddresses(Guid vacancyGuid, long vacancyReferenceNumber, string additionalLocationInformation)
        {
            return GetVacancyWithLocationAddresses(vacancyGuid, vacancyReferenceNumber, null, null, null, null, additionalLocationInformation);
        }

        private static ApprenticeshipVacancy GetVacancyWithLocationAddresses(Guid vacancyGuid, long vacancyReferenceNumber, int? numberOfPositions, bool? isEmployerLocationMainApprenticeshipLocation, string locationAddressesComment, string additionalLocationInformationComment, string additionalLocationInformation)
        {
            var addresses = new List<VacancyLocationAddress>
            {
                new VacancyLocationAddress
                {
                    Address = new PostalAddress
                    {
                        AddressLine4 = "address line 4 - 1",
                        AddressLine3 = "address line 3 - 1",
                        AddressLine2 = "address line 2 - 1",
                        AddressLine1 = "address line 1 - 1",
                        Postcode = "postcode",
                        ValidationSourceKeyValue = "uprn"   //VGA_Address
                    },
                    NumberOfPositions = 2
                },
                new VacancyLocationAddress
                {
                    Address = new PostalAddress
                    {
                        AddressLine4 = "address line 4 - 1",
                        AddressLine3 = "address line 3 - 1",
                        AddressLine2 = "address line 2 - 1",
                        AddressLine1 = "address line 1 - 1",
                        Postcode = "postcode",
                        ValidationSourceKeyValue = "uprn"   //VGA_Address
                    },
                    NumberOfPositions = 2
                }
            };

            var vacancy = new ApprenticeshipVacancy
            {
                AdditionalLocationInformation = additionalLocationInformation,
                LocationAddresses = addresses,
                VacancyReferenceNumber = vacancyReferenceNumber,
                EntityId = vacancyGuid,
                NumberOfPositions = numberOfPositions,
                IsEmployerLocationMainApprenticeshipLocation = isEmployerLocationMainApprenticeshipLocation,
                LocationAddressesComment = locationAddressesComment,
                AdditionalLocationInformationComment = additionalLocationInformationComment
            };

            return vacancy;
        }
    }
}