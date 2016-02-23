namespace SFA.Apprenticeships.Infrastructure.Repositories.Mongo.Tests.Vacancies
{
    using System;
    using System.Collections.Generic;
    using System.Security.Principal;
    using System.Threading;
    using Domain.Entities.Locations;
    using Domain.Entities.Raa.Locations;
    using Domain.Entities.Raa.Vacancies;
    using Domain.Raa.Interfaces.Repositories;
    using FluentAssertions;
    using Tests;
    using Mongo.Vacancies.Entities;
    using MongoDB.Driver;
    using MongoDB.Driver.Builders;
    using NUnit.Framework;
    using Ploeh.AutoFixture;

    public class ApprenticeshipVacancyWriteRepositoryTests : RepositoryIntegrationTest
    {
        private const long IntegrationTestVacancyReferenceNumber = long.MaxValue;

        [TearDown]
        public void TearDown()
        {
            var mongoConnectionString = MongoConfiguration.VacancyDb;
            var mongoDbName = MongoUrl.Create(mongoConnectionString).DatabaseName;

            var database = new MongoClient(mongoConnectionString)
                .GetServer()
                .GetDatabase(mongoDbName);

            var collection = database.GetCollection<MongoApprenticeshipVacancy>("apprenticeshipVacancies");
            collection.Remove(Query.EQ("VacancyReferenceNumber", IntegrationTestVacancyReferenceNumber));
        }

        [Test, Category("Integration")]
        public void ShouldCreateAndDeleteVacancy()
        {
            //Arrange
            var reader = Container.GetInstance<IVacancyReadRepository>();
            var writer = Container.GetInstance<IVacancyWriteRepository>();

            var vacancy =
                new Fixture().Build<Vacancy>()
                    .With(av => av.VacancyId, 0)
                    .With(av => av.VacancyReferenceNumber, IntegrationTestVacancyReferenceNumber)
                    .Create();

            //Act
            writer.Save(vacancy);
            var savedVacancy = reader.GetByReferenceNumber(IntegrationTestVacancyReferenceNumber);
            writer.Delete(savedVacancy.VacancyId);
            var deletedApplication = reader.GetByReferenceNumber(IntegrationTestVacancyReferenceNumber);

            //Assert
            savedVacancy.Should().NotBeNull();
            savedVacancy.VacancyReferenceNumber.Should().Be(IntegrationTestVacancyReferenceNumber);
            deletedApplication.Should().BeNull();
        }

        [Test, Category("Integration")]
        public void ReserveVacancyForQaShouldSetProperties()
        {
            //Arrange
            var reader = Container.GetInstance<IVacancyReadRepository>();
            var writer = Container.GetInstance<IVacancyWriteRepository>();

            var vacancy =
                new Fixture().Build<Vacancy>()
                    .With(av => av.VacancyId, 0)
                    .With(av => av.VacancyReferenceNumber, IntegrationTestVacancyReferenceNumber)
                    .With(av => av.Status, VacancyStatus.PendingQA)
                    .With(av => av.QAUserName, null)
                    .With(av => av.DateStartedToQA, null)
                    .Create();

            const string qaUserName = "qa@test.com";
            Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity(qaUserName), null);

            //Act
            writer.Save(vacancy);
            var savedVacancy = reader.GetByReferenceNumber(IntegrationTestVacancyReferenceNumber);
            var reservedVacancy = writer.ReserveVacancyForQA(IntegrationTestVacancyReferenceNumber);

            //Assert
            savedVacancy.Should().NotBeNull();
            savedVacancy.VacancyReferenceNumber.Should().Be(IntegrationTestVacancyReferenceNumber);
            savedVacancy.Status.Should().Be(VacancyStatus.PendingQA);
            savedVacancy.QAUserName.Should().BeNullOrEmpty();
            savedVacancy.DateStartedToQA.Should().Be(null);

            reservedVacancy.Should().NotBeNull();
            reservedVacancy.VacancyReferenceNumber.Should().Be(IntegrationTestVacancyReferenceNumber);
            reservedVacancy.Status.Should().Be(VacancyStatus.ReservedForQA);
            reservedVacancy.QAUserName.Should().Be(qaUserName);
            reservedVacancy.DateStartedToQA.Should().BeCloseTo(DateTime.UtcNow, 1000);
        }

        [Test, Category("Integration")]
        public void ReplaceLocationInformationTest()
        {
            //Arrange
            var writer = Container.GetInstance<IVacancyWriteRepository>();
            const string title = "vacancy title";

            var vacancy =
                new Fixture().Build<Vacancy>()
                    .With(av => av.VacancyId, 0)
                    .With(av => av.VacancyReferenceNumber, IntegrationTestVacancyReferenceNumber)
                    .With(av => av.Status, VacancyStatus.PendingQA)
                    .With(av => av.Title, title)
                    .With(av => av.QAUserName, null)
                    .With(av => av.DateStartedToQA, null)
                    .Create();
            //Act
            writer.Save(vacancy);
            const bool isEmployerLocationMainApprenticeshipLocation = false;
            int? numberOfPositions = null;
            IEnumerable<VacancyLocation> vacancyLocationAddresses = new[]
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
                        //Uprn = "uprn"
                    },
                    NumberOfPositions = 1
                },
                new VacancyLocation
                {
                    Address = new PostalAddress
                    {
                        AddressLine4 = "address line 4 - 2",
                        AddressLine3 = "address line 3 - 2",
                        AddressLine2 = "address line 2 - 2",
                        AddressLine1 = "address line 1 - 2",
                        Postcode = "postcode",
                        //Uprn = "uprn"
                    },
                    NumberOfPositions = 1
                },
                new VacancyLocation
                {
                    Address = new PostalAddress
                    {
                        AddressLine4 = "address line 4 - 3",
                        AddressLine3 = "address line 3 - 3",
                        AddressLine2 = "address line 2 - 3",
                        AddressLine1 = "address line 1 - 3",
                        Postcode = "postcode",
                        //Uprn = "uprn"
                    },
                    NumberOfPositions = 1
                }
            };
            const string locationAddressesComment = "location addresses comment";
            const string additionalLocationInformation = "additional location information";
            const string additionalLocationInformationComment = "additional location information comment";
            var savedVacancy = writer.ReplaceLocationInformation(IntegrationTestVacancyReferenceNumber, isEmployerLocationMainApprenticeshipLocation,
                numberOfPositions, vacancyLocationAddresses, locationAddressesComment, additionalLocationInformation,
                additionalLocationInformationComment);

            //Assert
            savedVacancy.Should().NotBeNull();
            savedVacancy.VacancyReferenceNumber.Should().Be(IntegrationTestVacancyReferenceNumber);
            savedVacancy.Status.Should().Be(VacancyStatus.PendingQA);
            savedVacancy.Title.Should().Be(title);
            savedVacancy.IsEmployerLocationMainApprenticeshipLocation.Should()
                .Be(isEmployerLocationMainApprenticeshipLocation);
            savedVacancy.NumberOfPositions.Should().Be(numberOfPositions);
            savedVacancy.LocationAddressesComment.Should().Be(locationAddressesComment);
            savedVacancy.AdditionalLocationInformation.Should().Be(additionalLocationInformation);
            savedVacancy.AdditionalLocationInformationComment.Should().Be(additionalLocationInformationComment);
            //savedVacancy.LocationAddresses.ShouldBeEquivalentTo(vacancyLocationAddresses);
        }
    }
}