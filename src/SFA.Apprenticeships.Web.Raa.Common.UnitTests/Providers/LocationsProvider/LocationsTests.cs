﻿namespace SFA.Apprenticeships.Web.Raa.Common.UnitTests.Providers.LocationsProvider
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Domain.Entities.Raa.Locations;
    using Domain.Entities.Raa.Parties;
    using Domain.Entities.Raa.Vacancies;
    using FluentAssertions;
    using Moq;
    using NUnit.Framework;
    using Ploeh.AutoFixture;
    using ViewModels.VacancyPosting;
    using Web.Common.ViewModels.Locations;

    [TestFixture]
    [Parallelizable]
    public class LocationsTests : TestBase
    {
        private readonly Guid _vacancyGuid = Guid.NewGuid();

        [Test]
        public void WhenCreatingANewVacancyShouldReturnANewLocationSearchViewModel()
        {
            const string ukprn = "ukprn";
            const int employerId = 1;
            const int providerSiteId = 42;

            var provider = GetVacancyPostingProvider();
            MockProviderService.Setup(s => s.GetProviderSite(providerSiteId))
                .Returns(new Fixture().Build<ProviderSite>().With(ps => ps.ProviderSiteId, providerSiteId).Create());
            MockEmployerService.Setup(s => s.GetEmployer(employerId, It.IsAny<bool>()))
                .Returns(new Fixture().Build<Employer>().With(e => e.EmployerId, employerId).Create());

            var result = provider.LocationAddressesViewModel(ukprn, providerSiteId, employerId, _vacancyGuid);

            result.Ukprn.Should().Be(ukprn);
            result.EmployerId.Should().Be(employerId);
            result.ProviderSiteId.Should().Be(providerSiteId);
            result.VacancyGuid.Should().Be(_vacancyGuid);
            result.Addresses.Should().HaveCount(0);
        }

        [Test]
        public void IfTheVacancyAlreadyExistsShouldFillTheViewModelWithItsInformation()
        {
            const string ukprn = "ukprn";
            const int employerId = 2;
            const int providerSiteId = 43;
            
            var vacancyWithLocationAddresses = GetVacancyWithLocationAddresses(_vacancyGuid);

            MockVacancyPostingService.Setup(s => s.GetVacancy(_vacancyGuid)).Returns(vacancyWithLocationAddresses.Vacancy);
            MockVacancyPostingService.Setup(s => s.GetVacancyLocations(vacancyWithLocationAddresses.Vacancy.VacancyId))
                .Returns(vacancyWithLocationAddresses.LocationAddresses);

            var provider = GetVacancyPostingProvider();

            var result = provider.LocationAddressesViewModel(ukprn, providerSiteId, employerId, _vacancyGuid);

            result.Addresses.Count.Should().Be(2);
        }

        [Test]
        public void IfTheVacancyHasOnlyOneAddressAndItsDifferentFromTheEmployerAddressShouldAddTheVacancyAddressToVacancyLocationsList()
        {
            const string ukprn = "ukprn";
            const int employerId = 2;
            const int providerSiteId = 43;
            const string additionalLocationInformation = "additional location information";
            const int numberOfPositions = 4;

            var vacancy = GetVacancy(_vacancyGuid, 1, numberOfPositions, false, additionalLocationInformation);

            MockVacancyPostingService.Setup(s => s.GetVacancy(_vacancyGuid)).Returns(vacancy);
            MockVacancyPostingService.Setup(s => s.GetVacancyLocations(vacancy.VacancyId))
                .Returns(new List<VacancyLocation>());

            var provider = GetVacancyPostingProvider();

            var result = provider.LocationAddressesViewModel(ukprn, providerSiteId, employerId, _vacancyGuid);

            result.Addresses.Count.Should().Be(1);
        }

        [Test]
        public void AddLocationsShouldCallReplaceLocationInformation()
        {
            var vacancyReferenceNumber = 1;
            const string additionalLocationInformation = "additional location information";
            const string aNewAdditionalLocationInformation = "a new additional location information";
            const string aNewAdditionalLocationInformationComment = "a new additional location information comment";
            const string aNewLocationAddressesComment = "a new additional location addresses comment";

            var vacancyWithLocationAddresses = GetVacancyWithLocationAddresses(additionalLocationInformation);

            MockVacancyPostingService.Setup(s => s.GetVacancyByReferenceNumber(vacancyReferenceNumber)).Returns(vacancyWithLocationAddresses.Vacancy);
            MockVacancyPostingService.Setup(s => s.GetVacancyLocations(vacancyWithLocationAddresses.Vacancy.VacancyId)).Returns(vacancyWithLocationAddresses.LocationAddresses);
            MockProviderService.Setup(s => s.GetVacancyOwnerRelationship(It.IsAny<int>(), It.IsAny<string>()))
                .Returns(new VacancyOwnerRelationship());

            var provider = GetVacancyPostingProvider();

            var locationSearchViewModel = GetLocationSearchViewModel(aNewAdditionalLocationInformation,
                aNewAdditionalLocationInformationComment, vacancyReferenceNumber, aNewLocationAddressesComment);

            provider.AddLocations(locationSearchViewModel);

            MockVacancyPostingService.Verify(
                s =>
                    s.UpdateVacancy(It.Is<Vacancy>(v => v.IsEmployerLocationMainApprenticeshipLocation == locationSearchViewModel.IsEmployerLocationMainApprenticeshipLocation &&
                        v.NumberOfPositions == null &&
                        v.LocationAddressesComment == aNewLocationAddressesComment &&
                        v.AdditionalLocationInformation == aNewAdditionalLocationInformation &&
                        v.AdditionalLocationInformationComment == aNewAdditionalLocationInformationComment)));
                    
                    

            MockVacancyPostingService.Verify(
                s =>
                    s.DeleteVacancyLocationsFor(It.IsAny<int>()));
        }

        [Test]
        public void RemoveVacancyLocationInformationShouldCallReplaceLocationInformation()
        {
            var vacancyReferenceNumber = 1;
            var vacancyGuid = Guid.NewGuid();
            var vacancyWithLocationAddresses = GetVacancyWithLocationAddresses(vacancyGuid, vacancyReferenceNumber);

            MockVacancyPostingService.Setup(s => s.GetVacancy(vacancyGuid)).Returns(vacancyWithLocationAddresses.Vacancy);
            MockVacancyPostingService.Setup(s => s.GetVacancyLocations(vacancyWithLocationAddresses.Vacancy.VacancyId)).Returns(vacancyWithLocationAddresses.LocationAddresses);
            var provider = GetVacancyPostingProvider();

            provider.RemoveVacancyLocationInformation(vacancyGuid);

            MockVacancyPostingService.Verify(
                 s =>
                     s.UpdateVacancy(It.Is<Vacancy>(v => v.IsEmployerLocationMainApprenticeshipLocation == null &&
                         v.NumberOfPositions == null &&
                         v.LocationAddressesComment == null &&
                         v.AdditionalLocationInformation == null &&
                         v.AdditionalLocationInformationComment == null)));

            MockVacancyPostingService.Verify(
                s =>
                    s.DeleteVacancyLocationsFor(It.IsAny<int>()));
        }

        [Test]
        public void RemoveLocationAddressesShouldCallRemoveLocationAddresses()
        {
            //Arrange
            int vacancyReferenceNumber = 1;
            var vacancyGuid = Guid.NewGuid();
            const bool isEmployerLocationMainApprenticeshipLocation = false;

            var vacancyWithLocationAddresses = GetVacancyWithLocationAddresses(vacancyGuid, 
                vacancyReferenceNumber, 1, isEmployerLocationMainApprenticeshipLocation, 
                null, null, string.Empty);

            MockVacancyPostingService.Setup(s => s.GetVacancy(vacancyGuid)).Returns(vacancyWithLocationAddresses.Vacancy);
            var provider = GetVacancyPostingProvider();

            //Act
            provider.RemoveLocationAddresses(vacancyGuid);

            //Assert
            MockVacancyPostingService.Verify(
                 s =>
                     s.UpdateVacancy(It.Is<Vacancy>(
                         v => v.IsEmployerLocationMainApprenticeshipLocation == vacancyWithLocationAddresses.Vacancy.IsEmployerLocationMainApprenticeshipLocation &&
                         v.NumberOfPositions == vacancyWithLocationAddresses.Vacancy.NumberOfPositions &&
                         v.AdditionalLocationInformation == null &&
                         v.AdditionalLocationInformationComment == vacancyWithLocationAddresses.Vacancy.AdditionalLocationInformationComment)));                        

            MockVacancyPostingService.Verify(
                s =>
                    s.DeleteVacancyLocationsFor(It.IsAny<int>()));
        }

        [Test]
        public void AddLocationsSetVacancyNumberOfPositionsFromVacancyLocationsIfTheresOnlyOneLocationAndItsDifferentFromEmployerAddress()
        {
            const int vacancyReferenceNumber = 1;
            const int numberOfPositions = 4;
            const string additionalLocationInformation = "additional location information";

            var vacancy = GetVacancy(_vacancyGuid, 1, numberOfPositions, false, additionalLocationInformation);

            MockVacancyPostingService.Setup(s => s.GetVacancyByReferenceNumber(vacancyReferenceNumber)).Returns(vacancy);
            MockVacancyPostingService.Setup(s => s.GetVacancyLocations(vacancy.VacancyId)).Returns(new List<VacancyLocation>());
            MockProviderService.Setup(s => s.GetVacancyOwnerRelationship(It.IsAny<int>(), It.IsAny<string>()))
                .Returns(new VacancyOwnerRelationship());

            var provider = GetVacancyPostingProvider();

            var locationSearchViewModel =
                GetLocationSearchViewModelWithOneLocationDifferentFromEmployerAddress(vacancyReferenceNumber,
                    numberOfPositions);

            provider.AddLocations(locationSearchViewModel);

            MockVacancyPostingService.Verify(
                s =>
                    s.UpdateVacancy(
                        It.Is<Vacancy>(
                            v =>
                                v.IsEmployerLocationMainApprenticeshipLocation ==
                                locationSearchViewModel.IsEmployerLocationMainApprenticeshipLocation &&
                                v.NumberOfPositions == numberOfPositions)));



            MockVacancyPostingService.Verify(
                s =>
                    s.DeleteVacancyLocationsFor(It.IsAny<int>()));
        }

        private static LocationSearchViewModel GetLocationSearchViewModelWithOneLocationDifferentFromEmployerAddress(int vacancyReferenceNumber, int numberOfPositions)
        {
            var locationSearchViewModel = new LocationSearchViewModel
            {
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
                        NumberOfPositions = numberOfPositions
                    }
                }
            };

            return locationSearchViewModel;
        }

        private static LocationSearchViewModel GetLocationSearchViewModel(string aNewAdditionalLocationInformation,
            string aNewAdditionalLocationInformationComment, int vacancyReferenceNumber, string aNewLocationAddressesComment)
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
                LocationAddressesComment = aNewLocationAddressesComment
            };
            return locationSearchViewModel;
        }

        private static VacancyWithLocationAddresses GetVacancyWithLocationAddresses(string additionalLocationInformation)
        {
            return GetVacancyWithLocationAddresses(Guid.NewGuid(), 1, additionalLocationInformation);
        }

        private static VacancyWithLocationAddresses GetVacancyWithLocationAddresses(Guid vacancyGuid, int vacancyReferenceNumber)
        {
            return GetVacancyWithLocationAddresses(vacancyGuid, vacancyReferenceNumber, string.Empty);
        }

        private static VacancyWithLocationAddresses GetVacancyWithLocationAddresses(Guid vacancyGuid)
        {
            return GetVacancyWithLocationAddresses(vacancyGuid, 1, string.Empty);
        }

        private static VacancyWithLocationAddresses GetVacancyWithLocationAddresses(Guid vacancyGuid, int vacancyReferenceNumber, string additionalLocationInformation)
        {
            return GetVacancyWithLocationAddresses(vacancyGuid, vacancyReferenceNumber, null, null, null, null, additionalLocationInformation);
        }

        private static VacancyWithLocationAddresses GetVacancyWithLocationAddresses(Guid vacancyGuid, int vacancyReferenceNumber, int? numberOfPositions, bool? isEmployerLocationMainApprenticeshipLocation, string locationAddressesComment, string additionalLocationInformationComment, string additionalLocationInformation)
        {
            var addresses = new List<VacancyLocation>
            {
                new VacancyLocation
                {
                    Address = new PostalAddress
                    {
                        AddressLine4 = "address line 4 - 1",
                        AddressLine3 = "address line 3 - 1",
                        AddressLine2 = "address line 2 - 1",
                        AddressLine1 = "address line 1 - 1",
                        Postcode = "postcode",
                    },
                    NumberOfPositions = 2
                },
                new VacancyLocation
                {
                    Address = new PostalAddress
                    {
                        AddressLine4 = "address line 4 - 1",
                        AddressLine3 = "address line 3 - 1",
                        AddressLine2 = "address line 2 - 1",
                        AddressLine1 = "address line 1 - 1",
                        Postcode = "postcode",
                    },
                    NumberOfPositions = 2
                }
            };

            var vacancy = new Vacancy
            {
                AdditionalLocationInformation = additionalLocationInformation,
                //LocationAddresses = addresses,
                VacancyReferenceNumber = vacancyReferenceNumber,
                VacancyGuid = vacancyGuid,
                NumberOfPositions = numberOfPositions,
                IsEmployerLocationMainApprenticeshipLocation = isEmployerLocationMainApprenticeshipLocation,
                LocationAddressesComment = locationAddressesComment,
                AdditionalLocationInformationComment = additionalLocationInformationComment
            };

            var vacancyWithLocationAddresses = new VacancyWithLocationAddresses
            {
                Vacancy = vacancy,
                LocationAddresses = addresses
            };

            return vacancyWithLocationAddresses;
        }

        private static Vacancy GetVacancy(Guid vacancyGuid, int vacancyReferenceNumber, int? numberOfPositions, bool? isEmployerLocationMainApprenticeshipLocation, string additionalLocationInformation)
        {
            var vacancy = new Vacancy
            {
                Address = new PostalAddress
                {
                    AddressLine4 = "address line 4",
                    AddressLine3 = "address line 3",
                    AddressLine2 = "address line 2",
                    AddressLine1 = "address line 1",
                    Postcode = "postcode",
                },
                AdditionalLocationInformation = additionalLocationInformation,
                VacancyReferenceNumber = vacancyReferenceNumber,
                VacancyGuid = vacancyGuid,
                NumberOfPositions = numberOfPositions,
                IsEmployerLocationMainApprenticeshipLocation = isEmployerLocationMainApprenticeshipLocation
            };

            return vacancy;
        }

        private class VacancyWithLocationAddresses
        {
            public Vacancy Vacancy { get; set; }
            public List<VacancyLocation> LocationAddresses { get; set; }
        }
    }
}