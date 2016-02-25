namespace SFA.Apprenticeships.Infrastructure.Repositories.Mongo.Tests.Vacancies
{
    using System;
    using System.Security.Principal;
    using System.Threading;
    using Domain.Entities.Raa.Vacancies;
    using Domain.Raa.Interfaces.Repositories;
    using FluentAssertions;
    using Mongo.Vacancies.Entities;
    using MongoDB.Driver;
    using MongoDB.Driver.Builders;
    using NUnit.Framework;
    using Ploeh.AutoFixture;

    public class ApprenticeshipVacancyWriteRepositoryTests : RepositoryIntegrationTest
    {
        private const int IntegrationTestVacancyReferenceNumber = int.MaxValue;

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
    }
}