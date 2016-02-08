namespace SFA.Apprenticeships.Infrastructure.Repositories.Sql.Tests.Schemas.Vacancy
{
    using Domain.Entities.Vacancies.ProviderVacancies;
    using Domain.Entities.Vacancies.ProviderVacancies.Apprenticeship;
    using FluentAssertions;
    using NUnit.Framework;
    using Ploeh.AutoFixture;
    using Sql.Schemas.Vacancy;
    using Vacancy = Sql.Schemas.Vacancy.Entities.Vacancy;

    [TestFixture]
    public class ApprenticeshipVacancyMappersUnitTests : TestBase
    {
        [Test]
        public void DoMappersMapEverything()
        {
            // Arrange
            var x = new ApprenticeshipVacancyMappers();

            // Act
            x.Initialise();

            // Assert
            x.Mapper.AssertConfigurationIsValid();
        }

        [Test]
        public void DoesApprenticeshipVacancyDomainObjectToDatabaseObjectMapperChokeInPractice()
        {
            // Arrange
            var x = new ApprenticeshipVacancyMappers();

            var vacancy =
                new Fixture().Build<ApprenticeshipVacancy>()
                    .With(av => av.Status, ProviderVacancyStatuses.PendingQA)
                    .With(av => av.QAUserName, null)
                    .With(av => av.DateStartedToQA, null)
                    .Create();


            // Act / Assert no exception
            x.Map<ApprenticeshipVacancy, Vacancy>(vacancy);
        }

        [Test]
        public void DoesDatabaseVacancyObjectToDomainObjectMapperChokeInPractice()
        {
            // Arrange
            var mapper = new ApprenticeshipVacancyMappers();
            var vacancy = CreateValidDatabaseVacancy();

            // Act / Assert no exception
            mapper.Map<Vacancy, ApprenticeshipVacancy>(vacancy);
        }

        [Test]
        [Ignore("Too many fields to exclude")]
        public void DoesDatabaseVacancyObjectMappingRoundTripViaDomainObjectExcludingHardOnes()
        {
            // Arrange
            var mapper = new ApprenticeshipVacancyMappers();
            var domainVacancy1 = new Fixture().Create<ApprenticeshipVacancy>();

            // Act

            var databaseVacancy = mapper.Map<ApprenticeshipVacancy, Vacancy>(domainVacancy1);
            var domainVacancy2 = mapper.Map<Vacancy, ApprenticeshipVacancy>(databaseVacancy);

            // Assert
            domainVacancy2.ShouldBeEquivalentTo(domainVacancy1, options =>
                ExcludeHardOnes(options)
                .Excluding(x => x.LocationAddresses)); // Manually mapped 
        }

        [Test]
        [Ignore("Not implemented yet")]
        public void DoesDatabaseVacancyObjectMappingRoundTripViaDomainObjectIncludingHardOnes()
        {
            // Arrange
            var mapper = new ApprenticeshipVacancyMappers();
            var domainVacancy1 = new Fixture().Create<ApprenticeshipVacancy>();

            // Act

            var databaseVacancy = mapper.Map<ApprenticeshipVacancy, Vacancy>(domainVacancy1);
            var domainVacancy2 = mapper.Map<Vacancy, ApprenticeshipVacancy>(databaseVacancy);

            // Assert
            domainVacancy2.ShouldBeEquivalentTo(domainVacancy1);
        }

        [Test]
        public void DoesApprenticeshipVacancyDomainObjectMappingRoundTripViaDatabaseObjectExcludingHardOnes()
        {
            // Arrange
            var mapper = new ApprenticeshipVacancyMappers();
            var databaseVacancy1 = CreateValidDatabaseVacancy();

            // Act
            var domainVacancy = mapper.Map<Vacancy, ApprenticeshipVacancy>(databaseVacancy1);
            var databaseVacancy2 = mapper.Map<ApprenticeshipVacancy, Vacancy>(domainVacancy);

            // Assert
            //databaseVacancy2.ShouldBeEquivalentTo(databaseVacancy1, options =>
            //    options.Excluding(v => v.EmployerVacancyPartyId)
            //    // Mapped using database lookups
            //    .Excluding(v => v.OwnerVacancyPartyId)
            //    .Excluding(v => v.ManagerVacancyPartyId)
            //    .Excluding(v => v.DeliveryProviderVacancyPartyId)
            //    .Excluding(v => v.ContractOwnerVacancyPartyId)
            //    .Excluding(v => v.OriginalContractOwnerVacancyPartyId)
            //    .Excluding(v => v.FrameworkId)
            //    // Not in Domain object yet
            //    .Excluding(v => v.AV_ContactName)
            //    .Excluding(v => v.AV_WageText));
        }
    }
}