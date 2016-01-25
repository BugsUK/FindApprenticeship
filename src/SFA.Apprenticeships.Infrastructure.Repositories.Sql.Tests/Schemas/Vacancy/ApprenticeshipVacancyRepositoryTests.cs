namespace SFA.Apprenticeships.Infrastructure.Repositories.Sql.Tests.Schemas.Vacancy
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;
    using Domain.Entities.Vacancies.ProviderVacancies;
    using Domain.Interfaces.Repositories;
    using FluentAssertions;
    using Moq;
    using NUnit.Framework;
    using Sql.Common;
    using Sql.Schemas.Reference.Entities;
    using Sql.Schemas.Vacancy;
    using Sql.Schemas.Vacancy.Entities;
    using Common;
    using Domain.Entities.Locations;
    using Domain.Interfaces.Queries;
    using Ploeh.AutoFixture;
    using SFA.Infrastructure.Interfaces;
    using TrainingType = Domain.Entities.Vacancies.ProviderVacancies.TrainingType;
    using Vacancy = Sql.Schemas.Vacancy.Entities.Vacancy;
    using WageType = Domain.Entities.Vacancies.ProviderVacancies.WageType;

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
            IApprenticeshipVacancyReadRepository repository = new ApprenticeshipVacancyRepository(_connection, _mapper,
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
            var logger = new Mock<ILogService>();
            IApprenticeshipVacancyReadRepository repository = new ApprenticeshipVacancyRepository(_connection, _mapper,
                logger.Object);

            var vacancy = repository.Get(VacancyId_VacancyA);

            vacancy.Status.Should().Be(ProviderVacancyStatuses.Live);
            vacancy.Title.Should().Be("Test vacancy");
            vacancy.WageType.Should().Be(WageType.Custom);
            vacancy.TrainingType = TrainingType.Frameworks;
        }

        [Test, ExpectedException(typeof(ArgumentNullException))]
        public void ShouldNotBeAbleToDeepSaveAVacancyWithLocationsAsNull()
        {
            var newReferenceNumber = 3L;
            var logger = new Mock<ILogService>();

            IApprenticeshipVacancyReadRepository readRepository = new ApprenticeshipVacancyRepository(_connection, _mapper,
                logger.Object);
            IApprenticeshipVacancyWriteRepository writeRepository = new ApprenticeshipVacancyRepository(_connection, _mapper,
                logger.Object);

            var vacancy = readRepository.Get(VacancyReferenceNumber_VacancyA);

            vacancy.VacancyReferenceNumber = newReferenceNumber;
            vacancy.LocationAddresses = null;
            writeRepository.DeepSave(vacancy);
        }

        [Test]
        public void UpdateTest()
        {
            var newReferenceNumber = 3L;
            var logger = new Mock<ILogService>();

            IApprenticeshipVacancyReadRepository readRepository = new ApprenticeshipVacancyRepository(_connection, _mapper,
                logger.Object);
            IApprenticeshipVacancyWriteRepository writeRepository = new ApprenticeshipVacancyRepository(_connection, _mapper,
                logger.Object);

            var vacancy = readRepository.Get(VacancyReferenceNumber_VacancyA);

            vacancy.VacancyReferenceNumber = newReferenceNumber;
            vacancy.LocationAddresses = new List<VacancyLocationAddress>();
            writeRepository.DeepSave(vacancy);

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
            var logger = new Mock<ILogService>();
            var repository = new ApprenticeshipVacancyRepository(_connection, _mapper, logger.Object);

            var vacancy = CreateValidDomainVacancy();

            // Act
            repository.DeepSave(vacancy);

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
            var logger = new Mock<ILogService>();
            IApprenticeshipVacancyReadRepository repository = new ApprenticeshipVacancyRepository(_connection, _mapper,
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
            var logger = new Mock<ILogService>();
            IApprenticeshipVacancyReadRepository repository = new ApprenticeshipVacancyRepository(_connection, _mapper,
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

        [Test]
        public void SaveVacancyShouldSaveLocations()
        {
            var logger = new Mock<ILogService>();
            IApprenticeshipVacancyReadRepository readRepository = new ApprenticeshipVacancyRepository(_connection, _mapper,
                logger.Object);
            IApprenticeshipVacancyWriteRepository writeRepository = new ApprenticeshipVacancyRepository(_connection, _mapper,
                logger.Object);

            const string title = "Vacancy title";
            const int numberOfLocations = 5;
            var vacancyGuid = Guid.NewGuid();

            var locations = new Fixture()
                .Build<VacancyLocationAddress>()
                .WithAutoProperties()
                .CreateMany(numberOfLocations)
                .ToList();

            var vacancy = CreateValidDomainVacancy();
            vacancy.EntityId = vacancyGuid;
            vacancy.Title = title;
            vacancy.LocationAddresses = locations;

            writeRepository.DeepSave(vacancy);

            var loadedVacancy = readRepository.Get(vacancyGuid);

            loadedVacancy.ShouldBeEquivalentTo(vacancy,
                options => ExcludeHardOnes(options)
                .Excluding(x => Regex.IsMatch(x.SelectedMemberPath, "LocationAddresses\\[[0-9]+\\].Address.Uprn")) //TODO
                .Using<DateTime>(ctx => ctx.Subject.Should().BeCloseTo(ctx.Expectation, 1000))
                .WhenTypeIs<DateTime>());
        }

        [Test]
        public void ShallowSaveVacancyShouldNotSaveLocations()
        {
            var logger = new Mock<ILogService>();
            IApprenticeshipVacancyReadRepository readRepository = new ApprenticeshipVacancyRepository(_connection, _mapper,
                logger.Object);
            IApprenticeshipVacancyWriteRepository writeRepository = new ApprenticeshipVacancyRepository(_connection, _mapper,
                logger.Object);

            const string title = "Vacancy title";
            const string newTitle = "Vacancy title";
            const int numberOfLocations = 5;
            const int newNumberOfLocations = 15;
            var vacancyGuid = Guid.NewGuid();

            var locations = new Fixture()
                .Build<VacancyLocationAddress>()
                .WithAutoProperties()
                .CreateMany(numberOfLocations)
                .ToList();

            var vacancy = CreateValidDomainVacancy();
            vacancy.EntityId = vacancyGuid;
            vacancy.Title = title;
            vacancy.LocationAddresses = locations;

            writeRepository.DeepSave(vacancy);

            var newLocations = new Fixture()
                .Build<VacancyLocationAddress>()
                .WithAutoProperties()
                .CreateMany(newNumberOfLocations)
                .ToList();
            vacancy.LocationAddresses = newLocations;
            vacancy.Title = newTitle;

            writeRepository.ShallowSave(vacancy);

            var loadedVacancy = readRepository.Get(vacancyGuid);

            loadedVacancy.ShouldBeEquivalentTo(vacancy,
                options => ExcludeHardOnes(options)
                .Excluding(x => x.LocationAddresses)
                .Using<DateTime>(ctx => ctx.Subject.Should().BeCloseTo(ctx.Expectation, 1000))
                .WhenTypeIs<DateTime>());

            loadedVacancy.LocationAddresses.ShouldBeEquivalentTo(locations,
                options => options.Excluding(x => Regex.IsMatch(x.SelectedMemberPath, "[[0-9]+\\].Address.Uprn")));
        }

        [Test]
        public void FindByFrameworkCodeNameWithPaginationTest()
        {
            var logger = new Mock<ILogService>();
            IApprenticeshipVacancyReadRepository repository = new ApprenticeshipVacancyRepository(_connection, _mapper,
                logger.Object);

            int totalResultsCount;
            const int pageSize = 2;
            var query = new ApprenticeshipVacancyQuery
            {
                FrameworkCodeName = "F02",
                PageSize = pageSize,
                CurrentPage = 1
            };

            var vacancies = repository.Find(query, out totalResultsCount);
            totalResultsCount.Should().Be(5);
            vacancies.Should().HaveCount(pageSize);
        }

        [Test]
        public void FindByLiveDateWithPaginationTest()
        {
            var logger = new Mock<ILogService>();
            IApprenticeshipVacancyReadRepository repository = new ApprenticeshipVacancyRepository(_connection, _mapper,
                logger.Object);

            int totalResultsCount;
            const int pageSize = 2;
            var query = new ApprenticeshipVacancyQuery
            {
                LiveDate = DateTime.UtcNow.AddDays(-2),
                PageSize = pageSize,
                CurrentPage = 1
            };

            var vacancies = repository.Find(query, out totalResultsCount);
            totalResultsCount.Should().Be(3);
            vacancies.Should().HaveCount(pageSize);
        }

        [Test]
        public void FindByClosingDateWithPaginationTest()
        {
            var logger = new Mock<ILogService>();
            IApprenticeshipVacancyReadRepository repository = new ApprenticeshipVacancyRepository(_connection, _mapper,
                logger.Object);

            int totalResultsCount;
            const int pageSize = 2;
            var query = new ApprenticeshipVacancyQuery
            {
                LatestClosingDate = DateTime.UtcNow.AddDays(-2),
                PageSize = pageSize,
                CurrentPage = 1
            };
            var vacancies = repository.Find(query, out totalResultsCount);
            totalResultsCount.Should().Be(9);
            vacancies.Should().HaveCount(pageSize);
        }
        
        [Test]
        public void ReserveVacancyForQaTest()
        {
            var logger = new Mock<ILogService>();
            IApprenticeshipVacancyWriteRepository writeRepository = new ApprenticeshipVacancyRepository(_connection, _mapper,
                logger.Object);
            IApprenticeshipVacancyReadRepository readRepository = new ApprenticeshipVacancyRepository(_connection, _mapper,
                logger.Object);

            const long vacancyReferenceNumber = 999;
            var vacancy = CreateValidDomainVacancy();
            vacancy.VacancyReferenceNumber = vacancyReferenceNumber;
            vacancy.DateSubmitted = null;
            vacancy.Status = ProviderVacancyStatuses.PendingQA;

            writeRepository.DeepSave(vacancy);

            writeRepository.ReserveVacancyForQA(vacancyReferenceNumber);
            var loadedVacancy = readRepository.Get(vacancyReferenceNumber);
            
            loadedVacancy.Status.Should().Be(ProviderVacancyStatuses.ReservedForQA);
            loadedVacancy.DateStartedToQA.Should().BeCloseTo(DateTime.UtcNow, 1000);
        }

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