namespace SFA.Apprenticeships.Infrastructure.Repositories.Vacancies.Sql.Tests
{
    using System;
    using System.Data.SqlClient;
    using System.IO;
    using Domain.Entities.Vacancies.ProviderVacancies;
    using Domain.Entities.Vacancies.ProviderVacancies.Apprenticeship;
    using Domain.Interfaces.Repositories;
    using FluentAssertions;
    using Mappers;
    using Moq;
    using NewDB.Domain.Entities;
    using NewDB.Domain.Entities.Vacancy;
    using NUnit.Framework;
    using SFA.Infrastructure.Interfaces;
    using SFA.Infrastructure.Sql;
    using TrainingType = Domain.Entities.Vacancies.ProviderVacancies.TrainingType;
    using Vacancy = NewDB.Domain.Entities.Vacancy.Vacancy;
    using WageType = Domain.Entities.Vacancies.ProviderVacancies.WageType;
    using Web.Common.Configuration;
    using Ploeh.AutoFixture;
    using FluentAssertions.Equivalency;

    [TestFixture]
    public class MapperTests : BaseTests
    {
        readonly IMapper _mapper = new ApprenticeshipVacancyMappers();

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
                    //                    .With(av => av.EntityId, Guid.Empty)
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
        public void DoesDatabaseVacancyObjectMappingRoundTripViaDomainObjectExcludingHardOnes()
        {
            // Arrange
            var mapper = new ApprenticeshipVacancyMappers();
            var domainVacancy1 = new Fixture().Create<ApprenticeshipVacancy>();

            // Act

            var databaseVacancy = mapper.Map<ApprenticeshipVacancy, Vacancy>(domainVacancy1);
            var domainVacancy2 = mapper.Map<Vacancy, ApprenticeshipVacancy>(databaseVacancy);

            // Assert
            domainVacancy2.ShouldBeEquivalentTo(domainVacancy1, options => ExcludeHardOnes(options));
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
            databaseVacancy2.ShouldBeEquivalentTo(databaseVacancy1, options => ExcludeHardOnes(options));
        }

        [Test]
        [Ignore("Not implemented yet")]
        public void DoesApprenticeshipVacancyDomainObjectMappingRoundTripViaDatabaseObjectIncludingHardOnes()
        {
            // Arrange
            var mapper = new ApprenticeshipVacancyMappers();
            var databaseVacancy1 = CreateValidDatabaseVacancy();

            // Act

            var domainVacancy = mapper.Map<Vacancy, ApprenticeshipVacancy>(databaseVacancy1);
            var databaseVacancy2 = mapper.Map<ApprenticeshipVacancy, Vacancy>(domainVacancy);

            // Assert
            databaseVacancy2.ShouldBeEquivalentTo(databaseVacancy1);
        }
    }
}