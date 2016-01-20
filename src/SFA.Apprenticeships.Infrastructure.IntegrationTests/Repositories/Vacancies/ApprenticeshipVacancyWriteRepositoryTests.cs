namespace SFA.Apprenticeships.Infrastructure.IntegrationTests.Repositories.Vacancies
{
    using System;
    using System.Collections.Generic;
    using System.Security.Principal;
    using System.Threading;
    using Domain.Entities.Locations;
    using Domain.Entities.Vacancies.ProviderVacancies;
    using Domain.Entities.Vacancies.ProviderVacancies.Apprenticeship;
    using Domain.Interfaces.Repositories;
    using FluentAssertions;
    using Infrastructure.Repositories.Vacancies.Entities;
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
            var reader = Container.GetInstance<IApprenticeshipVacancyReadRepository>();
            var writer = Container.GetInstance<IApprenticeshipVacancyWriteRepository>();

            var vacancy =
                new Fixture().Build<ApprenticeshipVacancy>()
                    .With(av => av.EntityId, Guid.Empty)
                    .With(av => av.VacancyReferenceNumber, IntegrationTestVacancyReferenceNumber)
                    .Create();

            //Act
            writer.Save(vacancy);
            var savedVacancy = reader.Get(IntegrationTestVacancyReferenceNumber);
            writer.Delete(savedVacancy.EntityId);
            var deletedApplication = reader.Get(IntegrationTestVacancyReferenceNumber);

            //Assert
            savedVacancy.Should().NotBeNull();
            savedVacancy.VacancyReferenceNumber.Should().Be(IntegrationTestVacancyReferenceNumber);
            deletedApplication.Should().BeNull();
        }

        [Test, Category("Integration")]
        public void ReserveVacancyForQaShouldSetProperties()
        {
            //Arrange
            var reader = Container.GetInstance<IApprenticeshipVacancyReadRepository>();
            var writer = Container.GetInstance<IApprenticeshipVacancyWriteRepository>();

            var vacancy =
                new Fixture().Build<ApprenticeshipVacancy>()
                    .With(av => av.EntityId, Guid.Empty)
                    .With(av => av.VacancyReferenceNumber, IntegrationTestVacancyReferenceNumber)
                    .With(av => av.Status, ProviderVacancyStatuses.PendingQA)
                    .With(av => av.QAUserName, null)
                    .With(av => av.DateStartedToQA, null)
                    .Create();

            const string qaUserName = "qa@test.com";
            Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity(qaUserName), null);

            //Act
            writer.Save(vacancy);
            var savedVacancy = reader.Get(IntegrationTestVacancyReferenceNumber);
            var reservedVacancy = writer.ReserveVacancyForQA(IntegrationTestVacancyReferenceNumber);

            //Assert
            savedVacancy.Should().NotBeNull();
            savedVacancy.VacancyReferenceNumber.Should().Be(IntegrationTestVacancyReferenceNumber);
            savedVacancy.Status.Should().Be(ProviderVacancyStatuses.PendingQA);
            savedVacancy.QAUserName.Should().BeNullOrEmpty();
            savedVacancy.DateStartedToQA.Should().Be(null);

            reservedVacancy.Should().NotBeNull();
            reservedVacancy.VacancyReferenceNumber.Should().Be(IntegrationTestVacancyReferenceNumber);
            reservedVacancy.Status.Should().Be(ProviderVacancyStatuses.ReservedForQA);
            reservedVacancy.QAUserName.Should().Be(qaUserName);
            reservedVacancy.DateStartedToQA.Should().BeCloseTo(DateTime.UtcNow, 1000);
        }

        [Test, Category("Integration")]
        public void ReplaceLocationInformationTest()
        {
            //Arrange
            var writer = Container.GetInstance<IApprenticeshipVacancyWriteRepository>();
            const string title = "vacancy title";

            var vacancy =
                new Fixture().Build<ApprenticeshipVacancy>()
                    .With(av => av.EntityId, Guid.Empty)
                    .With(av => av.VacancyReferenceNumber, IntegrationTestVacancyReferenceNumber)
                    .With(av => av.Status, ProviderVacancyStatuses.PendingQA)
                    .With(av => av.Title, title)
                    .With(av => av.QAUserName, null)
                    .With(av => av.DateStartedToQA, null)
                    .Create();
            //Act
            writer.Save(vacancy);
            const bool isEmployerLocationMainApprenticeshipLocation = false;
            int? numberOfPositions = null;
            IEnumerable<VacancyLocationAddress> vacancyLocationAddresses = new[]
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
                    NumberOfPositions = 1
                },
                new VacancyLocationAddress
                {
                    Address = new Address
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
                new VacancyLocationAddress
                {
                    Address = new Address
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
            savedVacancy.Status.Should().Be(ProviderVacancyStatuses.PendingQA);
            savedVacancy.Title.Should().Be(title);
            savedVacancy.IsEmployerLocationMainApprenticeshipLocation.Should()
                .Be(isEmployerLocationMainApprenticeshipLocation);
            savedVacancy.NumberOfPositions.Should().Be(numberOfPositions);
            savedVacancy.LocationAddressesComment.Should().Be(locationAddressesComment);
            savedVacancy.AdditionalLocationInformation.Should().Be(additionalLocationInformation);
            savedVacancy.AdditionalLocationInformationComment.Should().Be(additionalLocationInformationComment);
            savedVacancy.LocationAddresses.ShouldBeEquivalentTo(vacancyLocationAddresses);
        }
    }
}