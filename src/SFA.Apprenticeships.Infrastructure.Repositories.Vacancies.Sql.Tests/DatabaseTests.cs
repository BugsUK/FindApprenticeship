namespace SFA.Apprenticeships.Infrastructure.Repositories.Vacancies.Sql.Tests
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using Domain.Entities.Vacancies.ProviderVacancies;
    using Domain.Interfaces.Queries;
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
    using SFA.Infrastructure.Azure.Configuration;
    using SFA.Infrastructure.Configuration;
    using System.Text.RegularExpressions;
    [TestFixture(Category = "Integration")]
    public class DatabaseTests : BaseTests
    {
        private static string _connectionString = string.Empty;
        private readonly IMapper _mapper = new ApprenticeshipVacancyMappers();
		private readonly Mock<ILogService> _logService = new Mock<ILogService>();
        private const string DatabaseProjectName = "SFA.Apprenticeships.Data";

        [OneTimeSetUp]
        public void SetUpFixture()
        {
            var configurationManager = new ConfigurationManager();

            var configurationService = new AzureBlobConfigurationService(configurationManager, _logService.Object);

            var environment = configurationService.Get<CommonWebConfiguration>().Environment;

            string databaseName = $"RaaTest-{environment}";
            _connectionString = $"Server=SQLSERVERTESTING;Database={databaseName};Trusted_Connection=True;";

            var databaseProjectPath = AppDomain.CurrentDomain.BaseDirectory + $"\\..\\..\\..\\{DatabaseProjectName}";
            var dacPacRelativePath = $"\\bin\\{environment}\\{DatabaseProjectName}.dacpac";
            var dacpacFilePath = Path.Combine(databaseProjectPath + dacPacRelativePath);
            if (!File.Exists(dacpacFilePath))
            {
                //For NCrunch on Dave's machine
                databaseProjectPath = $"C:\\_Git\\Beta\\src\\{DatabaseProjectName}";
                dacpacFilePath = Path.Combine(databaseProjectPath + dacPacRelativePath);
            }
                
            var dbInitialiser = new DatabaseInitialiser(dacpacFilePath, _connectionString, databaseName);

            dbInitialiser.Publish(true);

            var seedScripts = new string[]
            {
            };
            var seedObjects = GetSeedObjects();

            dbInitialiser.Seed(seedScripts);
            dbInitialiser.Seed(seedObjects);

            
        }

        [Test]
        public void GetVacancyByVacancyReferenceNumberTest()
        {
            // configure _mapper
            IGetOpenConnection connection = new GetOpenConnectionFromConnectionString(_connectionString);
            var logger = new Mock<ILogService>();
            IApprenticeshipVacancyReadRepository repository = new ApprenticeshipVacancyRepository(connection, _mapper,
                logger.Object);

            var vacancy = repository.Get(VacancyReferenceNumber_VacancyA);

            vacancy.Status.Should().Be(ProviderVacancyStatuses.Live);
            vacancy.Title.Should().Be("Test vacancy");
            vacancy.WageType.Should().Be(WageType.Custom);
            vacancy.TrainingType = TrainingType.Frameworks;
        }

        [Test]
        public void GetVacancyByGuidTest()
        {
            // configure _mapper
            IGetOpenConnection connection = new GetOpenConnectionFromConnectionString(_connectionString);
            var logger = new Mock<ILogService>();
            IApprenticeshipVacancyReadRepository repository = new ApprenticeshipVacancyRepository(connection, _mapper,
                logger.Object);

            var vacancy = repository.Get(VacancyId_VacancyA);

            vacancy.Status.Should().Be(ProviderVacancyStatuses.Live);
            vacancy.Title.Should().Be("Test vacancy");
            vacancy.WageType.Should().Be(WageType.Custom);
            vacancy.TrainingType = TrainingType.Frameworks;
        }

        [Test]
        public void UpdateTest()
        {
            var newReferenceNumber = 3L;
            IGetOpenConnection connection = new GetOpenConnectionFromConnectionString(_connectionString);
            var logger = new Mock<ILogService>();

            IApprenticeshipVacancyReadRepository readRepository = new ApprenticeshipVacancyRepository(connection, _mapper,
                logger.Object);
            IApprenticeshipVacancyWriteRepository writeRepository = new ApprenticeshipVacancyRepository(connection, _mapper,
                logger.Object);

            var vacancy = readRepository.Get(VacancyReferenceNumber_VacancyA);

            vacancy.VacancyReferenceNumber = newReferenceNumber;
            vacancy.LocationAddresses = null; // TODO: Change to separate repo method
            writeRepository.Save(vacancy);

            vacancy = readRepository.Get(VacancyReferenceNumber_VacancyA);

            vacancy.Should().BeNull();

            vacancy = readRepository.Get(newReferenceNumber);

            vacancy.Status.Should().Be(ProviderVacancyStatuses.Live);
            vacancy.Title.Should().Be("Test vacancy");
            vacancy.WageType.Should().Be(WageType.Custom);
            vacancy.TrainingType = TrainingType.Frameworks;
        }

        [Test]
        public void RoundTripTest()
        {
            // Arrange
            var database = new GetOpenConnectionFromConnectionString(_connectionString);
            var logger = new Mock<ILogService>();
            var repository = new ApprenticeshipVacancyRepository(database, _mapper, logger.Object);

            var vacancy = CreateValidDomainVacancy();

            // Act
            repository.Save(vacancy);

            var loadedVacancy = repository.Get(vacancy.VacancyReferenceNumber);

            // Assert
            loadedVacancy.ShouldBeEquivalentTo(vacancy,
                options => ExcludeHardOnes(options)
                .Excluding(x => Regex.IsMatch(x.SelectedMemberPath, "LocationAddresses\\[[0-9]+\\].Address.Uprn")) //TODO
                .Using<DateTime>(ctx => ctx.Subject.Should().BeCloseTo(ctx.Expectation, 1000))
                .WhenTypeIs<DateTime>());
        }
        
        [Test]
        public void GetForProviderByUkprnAndProviderSiteErnTest()
        {
            IGetOpenConnection connection = new GetOpenConnectionFromConnectionString(_connectionString);
            var logger = new Mock<ILogService>();
            IApprenticeshipVacancyReadRepository repository = new ApprenticeshipVacancyRepository(connection, _mapper,
                logger.Object);

            var vacancies = repository.GetForProvider("1", "3");
            vacancies.Should().HaveCount(12);

            vacancies = repository.GetForProvider("2", "3");
            vacancies.Should().HaveCount(0);

            vacancies = repository.GetForProvider("1", "4");
            vacancies.Should().HaveCount(0);
        }

        [Test]
        public void GetWithStatusTest()
        {
            IGetOpenConnection connection = new GetOpenConnectionFromConnectionString(_connectionString);
            var logger = new Mock<ILogService>();
            IApprenticeshipVacancyReadRepository repository = new ApprenticeshipVacancyRepository(connection, _mapper,
                logger.Object);

            var vacancies = repository.GetWithStatus(ProviderVacancyStatuses.ParentVacancy, ProviderVacancyStatuses.Live);
            vacancies.Should().HaveCount(13);

            vacancies = repository.GetWithStatus(ProviderVacancyStatuses.Live);
            vacancies.Should().HaveCount(12);

            vacancies = repository.GetWithStatus(ProviderVacancyStatuses.ParentVacancy);
            vacancies.Should().HaveCount(1);

            vacancies = repository.GetWithStatus(ProviderVacancyStatuses.PendingQA);
            vacancies.Should().HaveCount(0);
        }

/*
        public void FindTest()
        {
            IGetOpenConnection connection = new GetOpenConnectionFromConnectionString(_connectionString);
            var logger = new Mock<ILogService>();
            IApprenticeshipVacancyReadRepository repository = new ApprenticeshipVacancyRepository(connection, _mapper,
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
                IGetOpenConnection connection = new GetOpenConnectionFromConnectionString(_connectionString);
                var logger = new Mock<ILogService>();
                IApprenticeshipVacancyWriteRepository repository = new ApprenticeshipVacancyRepository(connection, _mapper,
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
                    VacancyId = Guid.NewGuid(),
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
                EdsErn = 1,
                UKPrn = null
            };

            var vacancyParty2 = new VacancyParty
            {
                VacancyPartyTypeCode = "PS",
                FullName = "Provider A",
                Description = "A",
                WebsiteUrl = "URL",
                EdsErn = null,
                UKPrn = 1
            };

            var vacancyParty3 = new VacancyParty
            {
                VacancyPartyTypeCode = "PS",
                FullName = "Provider B",
                Description = "B",
                WebsiteUrl = "URL",
                EdsErn = 3,
                UKPrn = 1
            };

            var seedObjects = (new object[] {occupation, occupation2, framework1, framework2, vacancyParty1, vacancyParty2, vacancyParty3}).Union(vacancies);
            
            return seedObjects;
        }
    }
}