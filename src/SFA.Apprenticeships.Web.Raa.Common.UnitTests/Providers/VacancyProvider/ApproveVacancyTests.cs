namespace SFA.Apprenticeships.Web.Raa.Common.UnitTests.Providers.VacancyProvider
{
    using System.Collections.Generic;
    using System.Linq;
    using Application.Interfaces.Vacancies;
    using Application.Interfaces.VacancyPosting;
    using Common.Providers;
    using Domain.Entities.Raa.Locations;
    using Domain.Entities.Raa.Vacancies;
    using FluentAssertions;
    using Moq;
    using NUnit.Framework;
    using Ploeh.AutoFixture;
    using Application.Interfaces;
    using Application.Interfaces.Locations;
    using Domain.Entities.Exceptions;
    using Web.Common.Configuration;

    [TestFixture]
    [Parallelizable]
    public class ApproveVacancyTests
    {
        [TestCase(1)]
        [TestCase(10)]
        public void ApproveMultilocationVacancy(int locationAddressCount)
        {
            //Arrange
            var vacancyReferenceNumber = 1;
            var parentVacancyId = 2;
            var locationAddresses = new Fixture().Build<VacancyLocation>()
                .CreateMany(locationAddressCount).ToList();

            var vacancy = new Fixture().Build<Vacancy>()
                .With(x => x.VacancyReferenceNumber, vacancyReferenceNumber)
                .With(x => x.IsEmployerLocationMainApprenticeshipLocation, false)
                .With(x => x.VacancyId, parentVacancyId)
                .Create();

            var vacanyLockingService = new Mock<IVacancyLockingService>();
            var vacancyPostingService = new Mock<IVacancyPostingService>();

            vacancyPostingService.Setup(r => r.GetVacancyByReferenceNumber(vacancyReferenceNumber))
                .Returns(vacancy);
            vacancyPostingService.Setup(s => s.GetVacancyLocations(vacancy.VacancyId)).Returns(locationAddresses);

            //set up so that a bunch of vacancy reference numbers are created that are not the same as the one supplied above
            var fixture = new Fixture { RepeatCount = locationAddressCount - 1 };
            var vacancyNumbers = fixture.Create<List<int>>();
            vacancyPostingService.Setup(r => r.GetNextVacancyReferenceNumber()).ReturnsInOrder(vacancyNumbers.ToArray());

            vacanyLockingService.Setup(vls => vls.IsVacancyAvailableToQABy(It.IsAny<string>(), It.IsAny<Vacancy>()))
                .Returns(true);

            var vacancyProvider =
                new VacancyProviderBuilder()
                    .With(vacancyPostingService)
                    .With(vacanyLockingService)
                    .Build();

            //Act
            vacancyProvider.ApproveVacancy(vacancyReferenceNumber);

            //Assert
            //get the submitted vacancy once
            vacancyPostingService.Verify(r => r.GetVacancyByReferenceNumber(vacancyReferenceNumber), Times.Once);
            //save the original vacancy with a status of Live and itself as a parent vacancy
            vacancyPostingService.Verify(
                r =>
                    r.UpdateVacancy(
                        It.Is<Vacancy>(
                            av =>
                                av.VacancyReferenceNumber == vacancyReferenceNumber &&
                                av.Status == VacancyStatus.Live &&
                                av.ParentVacancyId == parentVacancyId &&
                                av.IsEmployerLocationMainApprenticeshipLocation.Value &&
                                av.Address.Postcode == locationAddresses.First().Address.Postcode &&
                                av.Address.AddressLine1 == locationAddresses.First().Address.AddressLine1 &&
                                av.Address.AddressLine2 == locationAddresses.First().Address.AddressLine2 &&
                                av.Address.AddressLine3 == locationAddresses.First().Address.AddressLine3 &&
                                av.Address.AddressLine4 == locationAddresses.First().Address.AddressLine4 &&
                                av.Address.AddressLine5 == locationAddresses.First().Address.AddressLine5 &&
                                av.NumberOfPositions == locationAddresses.First().NumberOfPositions)));

            //save new vacancies with a status of Live
            foreach (var number in vacancyNumbers)
            {
                vacancyPostingService.Verify(r =>
                    r.CreateVacancy(It.Is<Vacancy>(av => av.VacancyReferenceNumber == number
                                                                       && av.Status == VacancyStatus.Live &&
                                                                       av.ParentVacancyId ==
                                                                       parentVacancyId &&
                                                                       av.IsEmployerLocationMainApprenticeshipLocation
                                                                           .Value)), Times.Once);
            }

            //save new vacancies with only one of the new addresses and the position count
            foreach (var location in locationAddresses.Skip(1))
            {
                vacancyPostingService.Verify(r => r.CreateVacancy(It.Is<Vacancy>(av
                    => av.Address.Postcode == location.Address.Postcode
                       && av.Address.AddressLine1 == location.Address.AddressLine1
                       && av.Address.AddressLine2 == location.Address.AddressLine2
                       && av.Address.AddressLine3 == location.Address.AddressLine3
                       && av.Address.AddressLine4 == location.Address.AddressLine4
                       && av.Address.AddressLine5 == location.Address.AddressLine5
                       && av.NumberOfPositions == location.NumberOfPositions)));
            }

            //save the submitted vacancy once
            vacancyPostingService.Verify(r => r.UpdateVacancy(It.IsAny<Vacancy>()), Times.Once);

            //Create each child vacancy once
            vacancyPostingService.Verify(r => r.CreateVacancy(It.IsAny<Vacancy>()),
                Times.Exactly(locationAddressCount - 1));

            vacancyPostingService.Verify(s => s.DeleteVacancyLocationsFor(vacancy.VacancyId));
        }

        [Test]
        public void ApproveVacancy()
        {
            //Arrange
            var vacancyReferenceNumber = 1;
            var vacancy = new Fixture().Build<Vacancy>()
                .With(x => x.VacancyReferenceNumber, vacancyReferenceNumber)
                .With(x => x.IsEmployerLocationMainApprenticeshipLocation, true)
                .Create();

            var vacanyLockingService = new Mock<IVacancyLockingService>();
            var configurationService = new Mock<IConfigurationService>();
            var vacancyPostingService = new Mock<IVacancyPostingService>();
            configurationService.Setup(x => x.Get<CommonWebConfiguration>())
                .Returns(new CommonWebConfiguration { BlacklistedCategoryCodes = "" });

            vacancyPostingService.Setup(r => r.GetVacancyByReferenceNumber(vacancyReferenceNumber)).Returns(vacancy);

            vacanyLockingService.Setup(vls => vls.IsVacancyAvailableToQABy(It.IsAny<string>(), It.IsAny<Vacancy>()))
                .Returns(true);

            var vacancyProvider =
                new VacancyProviderBuilder()
                    .With(configurationService)
                    .With(vacancyPostingService)
                    .With(vacanyLockingService)
                    .Build();

            //Act
            var result = vacancyProvider.ApproveVacancy(vacancyReferenceNumber);

            //Assert
            result.Should().Be(QAActionResultCode.Ok);
            vacancyPostingService.Verify(r => r.GetVacancyByReferenceNumber(vacancyReferenceNumber));
            vacancyPostingService.Verify(
                r =>
                    r.UpdateVacancy(
                        It.Is<Vacancy>(
                            av =>
                                av.VacancyReferenceNumber == vacancyReferenceNumber &&
                                av.Status == VacancyStatus.Live)));
        }

        [Test]
        public void ApproveVacancyShouldCallGeocodeServiceIfAddressIsNotGeocoded()
        {
            //Arrange
            const int vacancyReferenceNumber = 1;
            var address = new PostalAddress
            {
                Postcode = "CV1 2WT"
            };
            var vacancy = new Fixture().Build<Vacancy>()
                .With(x => x.VacancyReferenceNumber, vacancyReferenceNumber)
                .With(x => x.IsEmployerLocationMainApprenticeshipLocation, true)
                .With(x => x.Address, address)
                .Create();

            var vacanyLockingService = new Mock<IVacancyLockingService>();
            var configurationService = new Mock<IConfigurationService>();
            var vacancyPostingService = new Mock<IVacancyPostingService>();
            var geocodingService = new Mock<IGeoCodeLookupService>();

            configurationService.Setup(x => x.Get<CommonWebConfiguration>())
                .Returns(new CommonWebConfiguration { BlacklistedCategoryCodes = "" });

            vacancyPostingService.Setup(r => r.GetVacancyByReferenceNumber(vacancyReferenceNumber)).Returns(vacancy);

            vacanyLockingService.Setup(vls => vls.IsVacancyAvailableToQABy(It.IsAny<string>(), It.IsAny<Vacancy>()))
                .Returns(true);

            var vacancyProvider =
                new VacancyProviderBuilder()
                    .With(configurationService)
                    .With(vacancyPostingService)
                    .With(vacanyLockingService)
                    .With(geocodingService)
                    .Build();

            //Act
            var result = vacancyProvider.ApproveVacancy(vacancyReferenceNumber);

            //Assert
            geocodingService.Verify(s => s.GetGeoPointFor(address));
        }

        [Test]
        public void ApproveVacancyShoulReturnErrorIfGeocodeServiceFails()
        {
            //Arrange
            const int vacancyReferenceNumber = 1;
            var address = new PostalAddress
            {
                Postcode = "CV1 2WT"
            };
            var vacancy = new Fixture().Build<Vacancy>()
                .With(x => x.VacancyReferenceNumber, vacancyReferenceNumber)
                .With(x => x.IsEmployerLocationMainApprenticeshipLocation, true)
                .With(x => x.Address, address)
                .Create();

            var vacanyLockingService = new Mock<IVacancyLockingService>();
            var configurationService = new Mock<IConfigurationService>();
            var vacancyPostingService = new Mock<IVacancyPostingService>();
            var geocodingService = new Mock<IGeoCodeLookupService>();

            configurationService.Setup(x => x.Get<CommonWebConfiguration>())
                .Returns(new CommonWebConfiguration { BlacklistedCategoryCodes = "" });

            vacancyPostingService.Setup(r => r.GetVacancyByReferenceNumber(vacancyReferenceNumber)).Returns(vacancy);

            vacanyLockingService.Setup(vls => vls.IsVacancyAvailableToQABy(It.IsAny<string>(), It.IsAny<Vacancy>()))
                .Returns(true);

            geocodingService.Setup(s => s.GetGeoPointFor(address))
                .Throws(new CustomException(Application.Interfaces.Locations.ErrorCodes.GeoCodeLookupProviderFailed));

            var vacancyProvider =
                new VacancyProviderBuilder()
                    .With(configurationService)
                    .With(vacancyPostingService)
                    .With(vacanyLockingService)
                    .With(geocodingService)
                    .Build();

            //Act
            var result = vacancyProvider.ApproveVacancy(vacancyReferenceNumber);

            //Assert
            result.Should().Be(QAActionResultCode.GeocodingFailure);
        }

        [TestCase(1)]
        [TestCase(10)]
        public void ApproveMultilocationVacancyShouldCallGeoCodeVacancyIfLocationIsNotGeocoded(int locationAddressCount)
        {
            //Arrange
            const int vacancyReferenceNumber = 1;
            const int parentVacancyId = 2;
            var locationAddresses = new Fixture().Build<VacancyLocation>()
                .CreateMany(locationAddressCount).ToList();

            foreach (var locationAddress in locationAddresses)
            {
                locationAddress.Address.GeoPoint.Easting = 0;
                locationAddress.Address.GeoPoint.Northing = 0;
                locationAddress.Address.GeoPoint.Latitude = 0;
                locationAddress.Address.GeoPoint.Longitude = 0;
            }

            var vacancy = new Fixture().Build<Vacancy>()
                .With(x => x.VacancyReferenceNumber, vacancyReferenceNumber)
                .With(x => x.IsEmployerLocationMainApprenticeshipLocation, false)
                .With(x => x.VacancyId, parentVacancyId)
                .Create();

            var vacanyLockingService = new Mock<IVacancyLockingService>();
            var vacancyPostingService = new Mock<IVacancyPostingService>();
            var geocodingService = new Mock<IGeoCodeLookupService>();

            vacancyPostingService.Setup(r => r.GetVacancyByReferenceNumber(vacancyReferenceNumber))
                .Returns(vacancy);
            vacancyPostingService.Setup(s => s.GetVacancyLocations(vacancy.VacancyId)).Returns(locationAddresses);

            //set up so that a bunch of vacancy reference numbers are created that are not the same as the one supplied above
            var fixture = new Fixture { RepeatCount = locationAddressCount - 1 };
            var vacancyNumbers = fixture.Create<List<int>>();
            vacancyPostingService.Setup(r => r.GetNextVacancyReferenceNumber()).ReturnsInOrder(vacancyNumbers.ToArray());

            vacanyLockingService.Setup(vls => vls.IsVacancyAvailableToQABy(It.IsAny<string>(), It.IsAny<Vacancy>()))
                .Returns(true);

            var vacancyProvider =
                new VacancyProviderBuilder()
                    .With(vacancyPostingService)
                    .With(vacanyLockingService)
                    .With(geocodingService)
                    .Build();

            //Act
            vacancyProvider.ApproveVacancy(vacancyReferenceNumber);

            //Assert
            geocodingService.Verify(s => s.GetGeoPointFor(It.IsAny<PostalAddress>()), Times.Exactly(locationAddressCount));
            
        }

        [TestCase(1)]
        [TestCase(10)]
        public void ApproveMultilocationVacancyShouldReturnErrorIfGeoCodeVacancyFails(int locationAddressCount)
        {
            //Arrange
            const int vacancyReferenceNumber = 1;
            const int parentVacancyId = 2;
            var locationAddresses = new Fixture().Build<VacancyLocation>()
                .CreateMany(locationAddressCount).ToList();

            foreach (var locationAddress in locationAddresses)
            {
                locationAddress.Address.GeoPoint.Easting = 0;
                locationAddress.Address.GeoPoint.Northing = 0;
                locationAddress.Address.GeoPoint.Latitude = 0;
                locationAddress.Address.GeoPoint.Longitude = 0;
            }

            var vacancy = new Fixture().Build<Vacancy>()
                .With(x => x.VacancyReferenceNumber, vacancyReferenceNumber)
                .With(x => x.IsEmployerLocationMainApprenticeshipLocation, false)
                .With(x => x.VacancyId, parentVacancyId)
                .Create();

            var vacanyLockingService = new Mock<IVacancyLockingService>();
            var vacancyPostingService = new Mock<IVacancyPostingService>();
            var geocodingService = new Mock<IGeoCodeLookupService>();

            vacancyPostingService.Setup(r => r.GetVacancyByReferenceNumber(vacancyReferenceNumber))
                .Returns(vacancy);
            vacancyPostingService.Setup(s => s.GetVacancyLocations(vacancy.VacancyId)).Returns(locationAddresses);
            geocodingService.Setup(s => s.GetGeoPointFor(It.IsAny<PostalAddress>()))
                .Throws(new CustomException(Application.Interfaces.Locations.ErrorCodes.GeoCodeLookupProviderFailed));

            //set up so that a bunch of vacancy reference numbers are created that are not the same as the one supplied above
            var fixture = new Fixture { RepeatCount = locationAddressCount - 1 };
            var vacancyNumbers = fixture.Create<List<int>>();
            vacancyPostingService.Setup(r => r.GetNextVacancyReferenceNumber()).ReturnsInOrder(vacancyNumbers.ToArray());

            vacanyLockingService.Setup(vls => vls.IsVacancyAvailableToQABy(It.IsAny<string>(), It.IsAny<Vacancy>()))
                .Returns(true);

            var vacancyProvider =
                new VacancyProviderBuilder()
                    .With(vacancyPostingService)
                    .With(vacanyLockingService)
                    .With(geocodingService)
                    .Build();

            //Act
            var result = vacancyProvider.ApproveVacancy(vacancyReferenceNumber);

            //Assert
            result.Should().Be(QAActionResultCode.GeocodingFailure);

        }

        [Test]
        public void ShouldReturnInvalidVacancyIfTheUserCantQATheVacancy()
        {
            const int vacanyReferenceNumber = 1;
            const string userName = "userName";

            var vacancyPostingService = new Mock<IVacancyPostingService>();
            var vacanyLockingService = new Mock<IVacancyLockingService>();
            var currentUserService = new Mock<ICurrentUserService>();

            currentUserService.Setup(cus => cus.CurrentUserName).Returns(userName);
            vacancyPostingService.Setup(vps => vps.GetVacancyByReferenceNumber(vacanyReferenceNumber))
                .Returns(new Vacancy {VacancyReferenceNumber = vacanyReferenceNumber});
            vacanyLockingService.Setup(vls => vls.IsVacancyAvailableToQABy(userName, It.IsAny<Vacancy>()))
                .Returns(false);
            
            var vacancyProvider =
                new VacancyProviderBuilder()
                    .With(vacancyPostingService)
                    .With(vacanyLockingService)
                    .With(currentUserService)
                    .Build();

            var result = vacancyProvider.ApproveVacancy(vacanyReferenceNumber);

            result.Should().Be(QAActionResultCode.InvalidVacancy);
        }
    }
}