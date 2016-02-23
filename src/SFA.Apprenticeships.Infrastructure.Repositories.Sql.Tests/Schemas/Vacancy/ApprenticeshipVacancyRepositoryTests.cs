namespace SFA.Apprenticeships.Infrastructure.Repositories.Sql.Tests.Schemas.Vacancy
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;
    using FluentAssertions;
    using Moq;
    using NUnit.Framework;
    using Sql.Common;
    using Sql.Schemas.Reference.Entities;
    using Sql.Schemas.Vacancy;
    using Sql.Schemas.Vacancy.Entities;
    using Common;
    using Domain.Raa.Interfaces.Repositories;
    using SFA.Infrastructure.Interfaces;
    using TrainingType = Domain.Entities.Raa.Vacancies.TrainingType;
    using Vacancy = Sql.Schemas.Vacancy.Entities.Vacancy;
    using WageType = Domain.Entities.Raa.Vacancies.WageType;

    [TestFixture(Category = "Integration")]
    public class ApprenticeshipVacancyRepositoryTests : TestBase
    {
        private readonly IMapper _mapper = new ApprenticeshipVacancyMappers();
        private IGetOpenConnection _connection;

        [TestFixtureSetUp]
        public void SetUpFixture()
        {
            var dbInitialiser = new DatabaseInitialiser();

            dbInitialiser.Publish(true);

            var seedScripts = new string[]
            {
            };
            var seedObjects = GetSeedObjects();

            dbInitialiser.Seed(seedScripts);
            dbInitialiser.Seed(seedObjects);

            _connection = dbInitialiser.GetOpenConnection();
        }

        [Test]
        public void GetVacancyByVacancyReferenceNumberTest()
        {
            // configure _mapper
            var logger = new Mock<ILogService>();
            IVacancyReadRepository repository = new VacancyRepository(_connection, _mapper,
                logger.Object);

            var vacancy = repository.Get(VacancyReferenceNumber_VacancyA);

            vacancy.Status.Should().Be(Domain.Entities.Raa.Vacancies.VacancyStatus.Live);
            vacancy.Title.Should().Be("Test vacancy");
            vacancy.WageType.Should().Be(WageType.Custom);
            vacancy.TrainingType = TrainingType.Frameworks;
        }

        [Test]
        public void GetVacancyByGuidTest()
        {
            // configure _mapper
            var logger = new Mock<ILogService>();
            IVacancyReadRepository repository = new VacancyRepository(_connection, _mapper,
                logger.Object);

            var vacancy = repository.Get(VacancyId_VacancyA);

            vacancy.Status.Should().Be(Domain.Entities.Raa.Vacancies.VacancyStatus.Live);
            vacancy.Title.Should().Be("Test vacancy");
            vacancy.WageType.Should().Be(WageType.Custom);
            vacancy.TrainingType = TrainingType.Frameworks;
        }

        [Test]
        public void UpdateTest()
        {
            var newReferenceNumber = 3L;
            var logger = new Mock<ILogService>();

            IVacancyReadRepository readRepository = new VacancyRepository(_connection, _mapper,
                logger.Object);
            IVacancyWriteRepository writeRepository = new VacancyRepository(_connection, _mapper,
                logger.Object);

            var vacancy = readRepository.Get(VacancyReferenceNumber_VacancyA);

            vacancy.VacancyReferenceNumber = newReferenceNumber;
            //vacancy.LocationAddresses = null; // TODO: Change to separate repo method
            writeRepository.Save(vacancy);

            vacancy = readRepository.Get(VacancyReferenceNumber_VacancyA);

            vacancy.Should().BeNull();

            vacancy = readRepository.GetByReferenceNumber(newReferenceNumber);

            vacancy.Status.Should().Be(Domain.Entities.Raa.Vacancies.VacancyStatus.Live);
            vacancy.Title.Should().Be("Test vacancy");
            vacancy.WageType.Should().Be(WageType.Custom);
            vacancy.TrainingType = TrainingType.Frameworks;
        }

        [Test]
        public void RoundTripTest()
        {
            // Arrange
            var logger = new Mock<ILogService>();
            var repository = new VacancyRepository(_connection, _mapper, logger.Object);

            var vacancy = CreateValidDomainVacancy();

            // Act
            repository.Save(vacancy);

            var loadedVacancy = repository.GetByReferenceNumber(vacancy.VacancyReferenceNumber);

            // Assert
            loadedVacancy.ShouldBeEquivalentTo(vacancy,
                options => ExcludeHardOnes(options)
                .Excluding(x => Regex.IsMatch(x.SelectedMemberPath, "LocationAddresses\\[[0-9]+\\].Address.Uprn")) //TODO
                .Using<DateTime>(ctx => ctx.Subject.Should().BeCloseTo(ctx.Expectation, 1000))
                .WhenTypeIs<DateTime>());
        }

        [Test]
        public void GetWithStatusTest()
        {
            var logger = new Mock<ILogService>();
            IVacancyReadRepository repository = new VacancyRepository(_connection, _mapper,
                logger.Object);

            var vacancies = repository.GetWithStatus(Domain.Entities.Raa.Vacancies.VacancyStatus.ParentVacancy, Domain.Entities.Raa.Vacancies.VacancyStatus.Live);
            vacancies.Should().HaveCount(13);

            vacancies = repository.GetWithStatus(Domain.Entities.Raa.Vacancies.VacancyStatus.Live);
            vacancies.Should().HaveCount(12);

            vacancies = repository.GetWithStatus(Domain.Entities.Raa.Vacancies.VacancyStatus.ParentVacancy);
            vacancies.Should().HaveCount(1);

            vacancies = repository.GetWithStatus(Domain.Entities.Raa.Vacancies.VacancyStatus.PendingQA);
            vacancies.Should().HaveCount(0);
        }

        /*
        public void FindTest()
        {
            var logger = new Mock<ILogService>();
            IVacancyReadRepository repository = new VacancyRepository(_connection, _mapper,
                logger.Object);

            int totalResultsCount;
            var query = new ApprenticeshipVacancyQuery
            {
                
            };
            var vacancies = repository.Find(query, out totalResultsCount);

        }
                
        [Test]
        public void ReserveVacancyForQaTest()
        {
            var logger = new Mock<ILogService>();
            IVacancyWriteRepository repository = new VacancyRepository(_connection, _mapper,
                logger.Object);

            repository.ReserveVacancyForQA(1);

            var vacancy = GetVacancy(1L);
        }*/

        private static IEnumerable<object> GetSeedObjects()
        {
            var vacancies = new List<Vacancy>();

            for (int i = 0; i < 11; i++)
            {
                var frameworkId = i%2 == 0 ? FrameworkId_Framework1 : FrameworkId_Framework2; //5 framework 1, 6 framework 2
                var date = DateTime.Today.AddDays(-i);

                vacancies.Add(new Vacancy
                {
                    VacancyId = 42,
                    VacancyReferenceNumber = null,
                    AV_ContactName = "av contact name",
                    VacancyTypeCode = VacancyTypeCode_Apprenticeship,
                    VacancyStatusCode = VacancyStatusCode_Live,
                    VacancyLocationTypeCode = VacancyLocationTypeCode_Specific,
                    Title = "Test vacancy",
                    TrainingTypeCode = TrainingTypeCode_Framework,
                    LevelCode = LevelCode_Intermediate,
                    FrameworkId = frameworkId,
                    WageValue = 100.0M,
                    WageTypeCode = WageTypeCode_Custom,
                    WageIntervalCode = WageIntervalCode_Weekly,
                    ClosingDate = date,
                    PublishedDateTime = date,
                    ContractOwnerVacancyPartyId = 1,
                    DeliveryProviderVacancyPartyId = 1,
                    EmployerVacancyPartyId = 1,
                    ManagerVacancyPartyId = 3,
                    OriginalContractOwnerVacancyPartyId = 1,
                    ParentVacancyId = null,
                    OwnerVacancyPartyId = 1,
                    DurationValue = 3,
                    DurationTypeCode = DurationTypeCode_Years
                });
            }

            vacancies.Add(new Vacancy
            {
                VacancyId = VacancyId_VacancyAParent,
                VacancyReferenceNumber = null,
                AV_ContactName = "av contact name",
                VacancyTypeCode = VacancyTypeCode_Apprenticeship,
                VacancyStatusCode = VacancyStatusCode_Parent,
                VacancyLocationTypeCode = VacancyLocationTypeCode_Specific,
                Title = "Test vacancy",
                TrainingTypeCode = TrainingTypeCode_Framework,
                LevelCode = LevelCode_Intermediate,
                FrameworkId = FrameworkId_Framework1,
                WageValue = 100.0M,
                WageTypeCode = WageTypeCode_Custom,
                WageIntervalCode = WageIntervalCode_Weekly,
                ClosingDate = DateTime.Now,
                ContractOwnerVacancyPartyId = 1,
                DeliveryProviderVacancyPartyId = 1,
                EmployerVacancyPartyId = 1,
                ManagerVacancyPartyId = 3,
                OriginalContractOwnerVacancyPartyId = 1,
                ParentVacancyId = null,
                OwnerVacancyPartyId = 1,
                DurationValue = 3,
                DurationTypeCode = DurationTypeCode_Years
            });

            vacancies.Add(new Vacancy
            {
                VacancyId = VacancyId_VacancyA,
                VacancyReferenceNumber = VacancyReferenceNumber_VacancyA,
                AV_ContactName = "av contact name",
                VacancyTypeCode = VacancyTypeCode_Apprenticeship,
                VacancyStatusCode = VacancyStatusCode_Live,
                VacancyLocationTypeCode = VacancyLocationTypeCode_Specific,
                Title = "Test vacancy",
                TrainingTypeCode = TrainingTypeCode_Framework,
                LevelCode = LevelCode_Intermediate,
                FrameworkId = FrameworkId_Framework1,
                WageValue = 100.0M,
                WageTypeCode = WageTypeCode_Custom,
                WageIntervalCode = WageIntervalCode_Weekly,
                ClosingDate = DateTime.Now,
                ContractOwnerVacancyPartyId = 1,
                DeliveryProviderVacancyPartyId = 1,
                EmployerVacancyPartyId = 1,
                ManagerVacancyPartyId = 3,
                OriginalContractOwnerVacancyPartyId = 1,
                ParentVacancyId = null,
                OwnerVacancyPartyId = 1,
                DurationValue = 3,
                DurationTypeCode = DurationTypeCode_Years,
                PublishedDateTime = DateTime.UtcNow.AddDays(-1)
            });

            var occupation = new Occupation
            {
                OccupationId = 1,
                OccupationStatusId = 1,
                CodeName = "O01",
                FullName = "Occupation 1",
                ShortName = "Occupation 1"
            };

            var occupation2 = new Occupation
            {
                OccupationId = 2,
                OccupationStatusId = 1,
                CodeName = "O02",
                FullName = "Occupation 2",
                ShortName = "Occupation 2"
            };

            var framework1 = new Framework
            {
                FrameworkId = FrameworkId_Framework1,
                CodeName = "F01",
                FullName = "Framework 1",
                ShortName = "Framework 1",
                FrameworkStatusId = 1,
                OccupationId = 1
            };

            var framework2 = new Framework
            {
                FrameworkId = FrameworkId_Framework2,
                CodeName = "F02",
                FullName = "Framework 2",
                ShortName = "Framework 2",
                FrameworkStatusId = 1,
                OccupationId = 2
            };

            var vacancyParty1 = new VacancyParty
            {
                VacancyPartyTypeCode = "ES",
                FullName = "Employer A",
                Description = "A",
                WebsiteUrl = "URL",
                EdsUrn = 1,
                UKPrn = null
            };

            var vacancyParty2 = new VacancyParty
            {
                VacancyPartyTypeCode = "PS",
                FullName = "Provider A",
                Description = "A",
                WebsiteUrl = "URL",
                EdsUrn = null,
                UKPrn = 1
            };

            var vacancyParty3 = new VacancyParty
            {
                VacancyPartyTypeCode = "PS",
                FullName = "Provider B",
                Description = "B",
                WebsiteUrl = "URL",
                EdsUrn = 3,
                UKPrn = 1
            };

            var seedObjects = (new object[] {occupation, occupation2, framework1, framework2, vacancyParty1, vacancyParty2, vacancyParty3}).Union(vacancies);
            
            return seedObjects;
        }
    }
}