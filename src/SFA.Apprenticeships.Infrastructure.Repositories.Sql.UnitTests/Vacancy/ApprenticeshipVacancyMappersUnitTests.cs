namespace SFA.Apprenticeships.Infrastructure.Repositories.Sql.UnitTests.Vacancy
{
    using Domain.Entities.Raa.Vacancies;
    using FluentAssertions;
    using NUnit.Framework;
    using Ploeh.AutoFixture;
    using Schemas.Vacancy;
    using Vacancy = Schemas.Vacancy.Entities.Vacancy;
    using DomainVacancy = Domain.Entities.Raa.Vacancies.Vacancy;

    [TestFixture]
    public class ApprenticeshipVacancyMappersUnitTests : TestBase
    {
        [Test]
        public void DoMappersMapEverything()
        {
            // Arrange
            var x = new VacancyMappers();

            // Act
            x.Initialise();

            // Assert
            x.Mapper.AssertConfigurationIsValid();
        }

        [Test]
        public void DoesApprenticeshipVacancyDomainObjectToDatabaseObjectMapperChokeInPractice()
        {
            // Arrange
            var x = new VacancyMappers();

            var vacancy =
                new Fixture().Build<DomainVacancy>()
                    .With(av => av.Status, VacancyStatus.Submitted)
                    .With(av => av.QAUserName, null)
                    .With(av => av.DateStartedToQA, null)
                    .Create();

            // Act / Assert no exception
            x.Map<DomainVacancy, Vacancy>(vacancy);
        }

        [Test]
        public void DoesDatabaseVacancyObjectToDomainObjectMapperChokeInPractice()
        {
            // Arrange
            var mapper = new VacancyMappers();
            var vacancy = CreateValidDatabaseVacancy();

            // Act / Assert no exception
            mapper.Map<Vacancy, DomainVacancy>(vacancy);
        }

        [Test]
        [Ignore("Too many fields to exclude")]
        public void DoesDatabaseVacancyObjectMappingRoundTripViaDomainObjectExcludingHardOnes()
        {
            // Arrange
            var mapper = new VacancyMappers();
            var domainVacancy1 = new Fixture().Create<DomainVacancy>();

            // Act
            var databaseVacancy = mapper.Map<DomainVacancy, Vacancy>(domainVacancy1);
            var domainVacancy2 = mapper.Map<Vacancy, DomainVacancy>(databaseVacancy);

            // Assert
            domainVacancy2.ShouldBeEquivalentTo(domainVacancy1, ExcludeHardOnes);
        }

        [Test]
        [Ignore("Not implemented yet")]
        public void DoesDatabaseVacancyObjectMappingRoundTripViaDomainObjectIncludingHardOnes()
        {
            // Arrange
            var mapper = new VacancyMappers();
            var domainVacancy1 = new Fixture().Create<DomainVacancy>();

            // Act
            var databaseVacancy = mapper.Map<DomainVacancy, Vacancy>(domainVacancy1);
            var domainVacancy2 = mapper.Map<Vacancy, DomainVacancy>(databaseVacancy);

            // Assert
            domainVacancy2.ShouldBeEquivalentTo(domainVacancy1);
        }
    }
}