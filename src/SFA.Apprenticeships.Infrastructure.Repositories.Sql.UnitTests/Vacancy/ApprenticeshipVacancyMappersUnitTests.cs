namespace SFA.Apprenticeships.Infrastructure.Repositories.Sql.UnitTests.Vacancy
{
    using Domain.Entities.Raa.Vacancies;
    using Domain.Entities.Vacancies;
    using FluentAssertions;
    using NUnit.Framework;
    using Ploeh.AutoFixture;
    using Schemas.Vacancy;
    using Vacancy = Schemas.Vacancy.Entities.Vacancy;
    using DomainVacancy = Domain.Entities.Raa.Vacancies.Vacancy;

    [TestFixture]
    [Parallelizable]
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

        [TestCase(1, WageType.Custom)]
        [TestCase(0, WageType.LegacyText)]
        [TestCase(2, WageType.ApprenticeshipMinimum)]
        [TestCase(3, WageType.NationalMinimum)]
        [TestCase(4, WageType.Custom)]
        public void DatabaseToDomainWageTypeTests(int databaseWageType, WageType expectedWageType)
        {
            var mapper = new VacancyMappers();
            var databaseVacancy = new Fixture()
                .Build<Vacancy>()
                .With(v => v.WageType, databaseWageType)
                .With(v => v.WageUnitId, 1)
                .Create();

            var domainVacancy = mapper.Map<Vacancy, DomainVacancy>(databaseVacancy);

            domainVacancy.Wage.Type.Should().Be(expectedWageType);
        }

        

        [TestCase("123.4567", "123.46")]
        [TestCase("123.4545", "123.45")]
        [TestCase("123.45", "123.45")]
        [TestCase("123.00", "123.00")]
        [TestCase("123", "123.00")]
        // [TestCase(null)]
        public void ShouldRoundWeeklyWageToTwoDecimalPlacesFromDatabaseToDomain(
            string weeklyWageString, string expectedWeeklyWageString)
        {
            // Arrange.
            var weeklyWage = default(decimal?);

            if (!string.IsNullOrWhiteSpace(weeklyWageString))
            {
                weeklyWage = decimal.Parse(weeklyWageString);
            }

            var expectedWeeklyWage = default(decimal?);

            if (!string.IsNullOrWhiteSpace(expectedWeeklyWageString))
            {
                expectedWeeklyWage = decimal.Parse(expectedWeeklyWageString);
            }

            var mapper = new VacancyMappers();

            var databaseVacancy = new Fixture()
                .Build<Vacancy>()
                .With(each => each.WeeklyWage, weeklyWage)
                .Create();

            // Act
            var domainVacancy = mapper.Map<Vacancy, DomainVacancy>(databaseVacancy);

            // Assert
            domainVacancy.WageAmount.Should().Be(expectedWeeklyWage);
        }
    }
}
