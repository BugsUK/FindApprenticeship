namespace SFA.Apprenticeships.Infrastructure.IntegrationTests.Repositories.Vacancies
{
    using System;
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

            //Act
            writer.Save(vacancy);
            var savedVacancy = reader.Get(IntegrationTestVacancyReferenceNumber);
            var reservedVacancy = writer.ReserveVacancyForQA(IntegrationTestVacancyReferenceNumber, qaUserName);

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
    }
}