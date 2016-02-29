namespace SFA.Apprenticeships.Infrastructure.Repositories.Sql.Tests.Schemas.Vacancy
{
    using System;
    using Common;
    using Domain.Entities.Raa.Vacancies;
    using Domain.Raa.Interfaces.Repositories;
    using FluentAssertions;
    using Moq;
    using NUnit.Framework;
    using Ploeh.AutoFixture;
    using SFA.Infrastructure.Interfaces;
    using Sql.Common;
    using Sql.Schemas.Vacancy;

    [TestFixture(Category = "Integration")]
    public class VacancyRepositoryTests : TestBase
    {
        private readonly IMapper _mapper = new ApprenticeshipVacancyMappers();
        private IGetOpenConnection _connection;

        [TestFixtureSetUp]
        public void SetUpFixture()
        {
            _connection =
                new GetOpenConnectionFromConnectionString(DatabaseConfigurationProvider.Instance.TargetConnectionString);
        }

        [Test, Category("Integration")]
        public void SimpleGetTest()
        {
            var logger = new Mock<ILogService>();
            var dateTimeService = new Mock<IDateTimeService>();
            IVacancyReadRepository readRepository = new VacancyRepository(_connection, _mapper,
                dateTimeService.Object, logger.Object);
            IVacancyWriteRepository writeRepository = new VacancyRepository(_connection, _mapper,
                dateTimeService.Object, logger.Object);

            const string title = "Vacancy title";
            var vacancyGuid = Guid.NewGuid();

            var fixture = new Fixture();
            fixture.Customizations.Add(
                new StringGenerator(() =>
                    Guid.NewGuid().ToString().Substring(0, 10)));

            var vacancy = CreateValidDomainVacancy();
            vacancy.VacancyGuid = vacancyGuid;
            vacancy.Title = title;
            vacancy.Status = VacancyStatus.Draft;
                // Changed from PendingQA to Draft because PendingQA is not still in the db
            vacancy.VacancyManagerId = 1;
            vacancy.Address.Postcode = "CV1 2WT";
            vacancy.Address.County = "CAM";
            vacancy.OwnerPartyId = 1;


            writeRepository.Save(vacancy);

            var entity = readRepository.GetByReferenceNumber((int) vacancy.VacancyReferenceNumber);

            entity.ShouldBeEquivalentTo(vacancy, options =>
                ForShallowSave(options)
                    .Excluding(x => x.SelectedMemberPath.EndsWith("Comment"))
                    .Excluding(x => x.VacancyId)
                    .Excluding(x => x.SectorCodeName) // what is this?
                    .Excluding(x => x.Address.PostalAddressId)
                    .Excluding(x => x.Address.ValidationSourceCode)
                    .Excluding(x => x.Address.ValidationSourceKeyValue)
                    .Excluding(x => x.Address.DateValidated)
                    .Excluding(x => x.Address.County) // do a DB lookup
                    .Excluding(x => x.LastEditedById) // Get from VacancyHistory table (not yet implemented)
                    .Excluding(x => x.ClosingDate)
                    .Excluding(x => x.PossibleStartDate)
                    .Excluding(x => x.DateFirstSubmitted)
                    .Excluding(x => x.UpdatedDateTime)
                    .Excluding(x => x.CreatedDateTime));
        }

        [Test, Category("Integration")]
        public void SimpleSaveAndUpdateTest()
        {
            var logger = new Mock<ILogService>();
            var dateTimeService = new Mock<IDateTimeService>();
            IVacancyWriteRepository writeRepository = new VacancyRepository(_connection, _mapper,
                dateTimeService.Object, logger.Object);

            const string title = "Vacancy title";
            var vacancyGuid = Guid.NewGuid();

            var fixture = new Fixture();
            fixture.Customizations.Add(
                new StringGenerator(() =>
                    Guid.NewGuid().ToString().Substring(0, 10)));


            var vacancy = CreateValidDomainVacancy();
            vacancy.VacancyGuid = vacancyGuid;
            vacancy.Title = title;
            vacancy.Status = VacancyStatus.Draft;
                // Changed from PendingQA to Draft because PendingQA is not still in the db
            vacancy.TrainingProvided = null;

            var entity = writeRepository.Save(vacancy);
            vacancy.VacancyId = entity.VacancyId;
            vacancy.TrainingProvided = null;
            writeRepository.Save(vacancy);
        }

        /*
        private IEnumerable<object> GetSeedObjects()
        {
            var vacancies = new List<Vacancy>();

            for (int i = 0; i < 11; i++)
            {
                var frameworkId = i % 2 == 0 ? FrameworkIdFramework1 : FrameworkIdFramework2; //5 framework 1, 6 framework 2
                var date = DateTime.Today.AddDays(-i);

                vacancies.Add(new Vacancy
                {
                    //VacancyId = Guid.NewGuid(),
                    VacancyReferenceNumber = VacancyReferenceNumber++,
                    //AV_ContactName = "av contact name",
                    //VacancyTypeCode = VacancyTypeCode_Apprenticeship,
                    //VacancyStatusCode = VacancyStatusCode_Live,
                    //VacancyLocationTypeCode = VacancyLocationType.Specific,
                    Title = "Test vacancy",
                    //TrainingTypeCode = TrainingTypeCode_Framework,
                    //LevelCode = LevelCode_Intermediate,
                    // FrameworkId = frameworkId,
                    //WageValue = 100.0M,
                    //WageTypeCode = WageTypeCode_Custom,
                    //WageIntervalCode = WageIntervalCode_Weekly,
                    //ClosingDate = date,
                    //PublishedDateTime = date,
                    //ContractOwnerVacancyPartyId = 1,
                    //DeliveryProviderVacancyPartyId = 1,
                    //EmployerVacancyPartyId = 1,
                    //ManagerVacancyPartyId = 3,
                    //OriginalContractOwnerVacancyPartyId = 1,
                    //ParentVacancyId = null,
                    //OwnerVacancyPartyId = 1,
                    //DurationValue = 3,
                    //DurationTypeCode = DurationTypeCode_Years
                });
            }

            vacancies.Add(new Vacancy
            {
                //VacancyId = VacancyId_VacancyAParent,
                VacancyReferenceNumber = VacancyReferenceNumber++,
                //AV_ContactName = "av contact name",
                //VacancyTypeCode = VacancyTypeCode_Apprenticeship,
                //VacancyStatusCode = VacancyStatusCode_Parent,
                //VacancyLocationTypeCode = VacancyLocationType.Specific,
                Title = "Test vacancy",
                //TrainingTypeCode = TrainingTypeCode_Framework,
                //LevelCode = LevelCode_Intermediate,
                // FrameworkId = FrameworkIdFramework1,
                //WageValue = 100.0M,
                //WageTypeCode = WageTypeCode_Custom,
                //WageIntervalCode = WageIntervalCode_Weekly,
                //ClosingDate = DateTime.Now,
                //ContractOwnerVacancyPartyId = 1,
                //DeliveryProviderVacancyPartyId = 1,
                //EmployerVacancyPartyId = 1,
                //ManagerVacancyPartyId = 3,
                //OriginalContractOwnerVacancyPartyId = 1,
                //ParentVacancyId = null,
                //OwnerVacancyPartyId = 1,
                //DurationValue = 3,
                //DurationTypeCode = DurationTypeCode_Years
            });

            vacancies.Add(new Vacancy
            {
                //VacancyId = VacancyIdVacancyA,
                VacancyReferenceNumber = VacancyReferenceNumberVacancyA,
                //AV_ContactName = "av contact name",
                //VacancyTypeCode = VacancyTypeCode_Apprenticeship,
                //VacancyStatusCode = VacancyStatusCode_Live,
                //VacancyLocationTypeCode = VacancyLocationType.Specific,
                Title = "Test vacancy",
                //TrainingTypeCode = TrainingTypeCode_Framework,
                //LevelCode = LevelCode_Intermediate,
                //FrameworkId = FrameworkIdFramework1,
                //WageValue = 100.0M,
                //WageTypeCode = WageTypeCode_Custom,
                //WageIntervalCode = WageIntervalCode_Weekly,
                //ClosingDate = DateTime.Now,
                //ContractOwnerVacancyPartyId = 1,
                //DeliveryProviderVacancyPartyId = 1,
                //EmployerVacancyPartyId = 1,
                //ManagerVacancyPartyId = 3,
                //OriginalContractOwnerVacancyPartyId = 1,
                //ParentVacancyId = null,
                //OwnerVacancyPartyId = 1,
                //DurationValue = 3,
                //DurationTypeCode = DurationTypeCode_Years,
                //PublishedDateTime = DateTime.UtcNow.AddDays(-1)
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
                FrameworkId = FrameworkIdFramework1,
                CodeName = "F01",
                FullName = "Framework 1",
                ShortName = "Framework 1",
                FrameworkStatusId = 1,
                OccupationId = 1
            };

            var framework2 = new Framework
            {
                FrameworkId = FrameworkIdFramework2,
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
                EdsUrn = 101,
                UKPrn = null,
                PostalAddressId = 1
            };

            var vacancyParty2 = new VacancyParty
            {
                VacancyPartyTypeCode = "PS",
                FullName = "Provider A",
                Description = "A",
                WebsiteUrl = "URL",
                EdsUrn = null,
                UKPrn = 202,
                PostalAddressId = 2
            };

            var vacancyParty3 = new VacancyParty
            {
                VacancyPartyTypeCode = "PS",
                FullName = "Provider B",
                Description = "B",
                WebsiteUrl = "URL",
                EdsUrn = 103,
                UKPrn = 203,
                PostalAddressId = 3
            };

            var postalAddress1 = new PostalAddress
            {
                AddressLine1 = "AddressLine1 1",
                AddressLine2 = "AddressLine2",
                AddressLine3 = "AddressLine3",
                AddressLine4 = "AddressLine4",
                AddressLine5 = "AddressLine5",
                Postcode = "PC1 1AA"
            };

            var postalAddress2 = new PostalAddress
            {
                AddressLine1 = "AddressLine1 2",
                AddressLine2 = "AddressLine2",
                AddressLine3 = "AddressLine3",
                AddressLine4 = "AddressLine4",
                AddressLine5 = "AddressLine5",
                Postcode = "PC1 1AA"
            };

            var postalAddress3 = new PostalAddress
            {
                AddressLine1 = "AddressLine1 3",
                AddressLine2 = "AddressLine2",
                AddressLine3 = "AddressLine3",
                AddressLine4 = "AddressLine4",
                AddressLine5 = "AddressLine5",
                Postcode = "PC1 1AA"
            };

            var seedObjects = (new object[] { occupation, occupation2, framework1, framework2, postalAddress1, postalAddress2, postalAddress3, vacancyParty1, vacancyParty2, vacancyParty3 }).Union(vacancies);

            return seedObjects;
        }*/

        /*
        [Test, Ignore, Category("Integration")]
        public void GetVacancyByVacancyReferenceNumberTest()
        {
            // configure _mapper
            var logger = new Mock<ILogService>();
            IApprenticeshipVacancyReadRepository repository = new ApprenticeshipVacancyRepository(_connection, _mapper,
                logger.Object);

            var vacancy = repository.Get(VacancyReferenceNumberVacancyA);

            vacancy.Status.Should().Be(ProviderVacancyStatuses.Live);
            vacancy.Title.Should().Be("Test vacancy");
            vacancy.WageType.Should().Be(WageType.Custom);
            vacancy.TrainingType = TrainingType.Frameworks;
        }

        [Test, Ignore, Category("Integration")]
        public void GetVacancyByGuidTest()
        {
            // configure _mapper
            var logger = new Mock<ILogService>();
            IApprenticeshipVacancyReadRepository repository = new ApprenticeshipVacancyRepository(_connection, _mapper,
                logger.Object);

            var vacancy = repository.Get(VacancyIdVacancyA);

            vacancy.Status.Should().Be(ProviderVacancyStatuses.Live);
            vacancy.Title.Should().Be("Test vacancy");
            vacancy.WageType.Should().Be(WageType.Custom);
            vacancy.TrainingType = TrainingType.Frameworks;
        }

        [Test, Ignore, ExpectedException(typeof(ArgumentNullException)), Category("Integration")]
        public void ShouldNotBeAbleToDeepSaveAVacancyWithLocationsAsNull()
        {
            var newReferenceNumber = VacancyReferenceNumber++;
            var logger = new Mock<ILogService>();

            IApprenticeshipVacancyReadRepository readRepository = new ApprenticeshipVacancyRepository(_connection, _mapper,
                logger.Object);
            IApprenticeshipVacancyWriteRepository writeRepository = new ApprenticeshipVacancyRepository(_connection, _mapper,
                logger.Object);

            var vacancy = readRepository.Get(VacancyReferenceNumberVacancyA);

            vacancy.VacancyReferenceNumber = newReferenceNumber;
            vacancy.LocationAddresses = null;
            writeRepository.DeepSave(vacancy);
        }

        [Test, Ignore, Category("Integration")]
        public void UpdateTest()
        {
            var newReferenceNumber = VacancyReferenceNumber++;
            var logger = new Mock<ILogService>();

            IApprenticeshipVacancyReadRepository readRepository = new ApprenticeshipVacancyRepository(_connection, _mapper,
                logger.Object);
            IApprenticeshipVacancyWriteRepository writeRepository = new ApprenticeshipVacancyRepository(_connection, _mapper,
                logger.Object);

            var vacancy = readRepository.Get(VacancyReferenceNumberVacancyA);

            vacancy.VacancyReferenceNumber = newReferenceNumber;
            vacancy.LocationAddresses = new List<VacancyLocationAddress>();
            writeRepository.DeepSave(vacancy);

            vacancy = readRepository.Get(VacancyReferenceNumberVacancyA);

            vacancy.Should().BeNull();

            vacancy = readRepository.Get(newReferenceNumber);

            vacancy.Status.Should().Be(ProviderVacancyStatuses.Live);
            vacancy.Title.Should().Be("Test vacancy");
            vacancy.WageType.Should().Be(WageType.Custom);
            vacancy.TrainingType = TrainingType.Frameworks;
        }

        [Test, Ignore, Category("Integration")]
        public void RoundTripWithLocationTypeEqualsEmployerTest()
        {
            // Arrange
            var logger = new Mock<ILogService>();
            var repository = new ApprenticeshipVacancyRepository(_connection, _mapper, logger.Object);

            // By default the vacancy is a vacancy with location type = employer
            var vacancy = CreateValidDomainVacancy();

            // Act
            repository.DeepSave(vacancy);

            var loadedVacancy = repository.Get(vacancy.VacancyReferenceNumber.Value);

            // Assert
            loadedVacancy.ShouldBeEquivalentTo(vacancy,
                options => ExcludeHardOnes(options)
                .Excluding(x => x.LocationAddresses)
                .Using<DateTime>(ctx => ctx.Subject.Should().BeCloseTo(ctx.Expectation, 1000))
                .WhenTypeIs<DateTime>());
        }

        [Test, Ignore, Category("Integration")]
        public void RoundTripWithLocationTypeEqualsMultipleTest()
        {
            // Arrange
            var logger = new Mock<ILogService>();
            var repository = new ApprenticeshipVacancyRepository(_connection, _mapper, logger.Object);

            // By default the vacancy is a vacancy with location type = employer
            var vacancy = CreateValidDomainVacancy();
            vacancy.IsEmployerLocationMainApprenticeshipLocation = false;
            var locations = new Fixture()
                .Build<VacancyLocationAddress>()
                .WithAutoProperties()
                .CreateMany(6)
                .ToList();
            locations.ForEach(l => l.Address.ValidationSourceCode = "PCA");
            vacancy.LocationAddresses = locations;

            // Act
            repository.DeepSave(vacancy);

            var loadedVacancy = repository.Get(vacancy.VacancyReferenceNumber.Value);

            // Assert
            loadedVacancy.ShouldBeEquivalentTo(vacancy,
                options => ExcludeHardOnes(options)
                    .Excluding(x => Regex.IsMatch(x.SelectedMemberPath, "[[0-9]+\\].Address.DateValidated"))
                    .Excluding(x => Regex.IsMatch(x.SelectedMemberPath, "[[0-9]+\\].Address.County"))
                    .Excluding(x => Regex.IsMatch(x.SelectedMemberPath, "[[0-9]+\\].Address.PostalAddressId"))
                    .Using<DateTime>(ctx => ctx.Subject.Should().BeCloseTo(ctx.Expectation, 1000))
                    .WhenTypeIs<DateTime>());
        }

        [Test, Ignore, Category("Integration")]
        public void GetForProviderByUkprnAndProviderSiteErnTest()
        {
            var logger = new Mock<ILogService>();
            IApprenticeshipVacancyReadRepository repository = new ApprenticeshipVacancyRepository(_connection, _mapper,
                logger.Object);

            var vacancies = repository.GetForProvider("203", "103");
            vacancies.Should().HaveCount(12);

            vacancies = repository.GetForProvider("202", "103");
            vacancies.Should().HaveCount(0);

            vacancies = repository.GetForProvider("202", "101");
            vacancies.Should().HaveCount(0);
        }

        [Test, Ignore, Category("Integration")]
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
        }

        [Test, Ignore, Category("Integration")]
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
            locations.ForEach(l => l.Address.ValidationSourceCode = "PCA");

            var vacancy = CreateValidDomainVacancy();
            vacancy.VacancyReferenceNumber = VacancyReferenceNumber++;
            vacancy.VacancyGuid = vacancyGuid;
            vacancy.Title = title;
            vacancy.LocationAddresses = locations;
            vacancy.IsEmployerLocationMainApprenticeshipLocation = (vacancy.LocationAddresses.Count == 1);

            writeRepository.DeepSave(vacancy);

            var loadedVacancy = readRepository.Get(vacancyGuid);

            loadedVacancy.ShouldBeEquivalentTo(vacancy,
                options => ExcludeHardOnes(options)
                    .Excluding(x => Regex.IsMatch(x.SelectedMemberPath, "[[0-9]+\\].Address.DateValidated"))
                    .Excluding(x => Regex.IsMatch(x.SelectedMemberPath, "[[0-9]+\\].Address.County"))
                    .Excluding(x => Regex.IsMatch(x.SelectedMemberPath, "[[0-9]+\\].Address.PostalAddressId"))
                    .Using<DateTime>(ctx => ctx.Subject.Should().BeCloseTo(ctx.Expectation, 1000))
                    .WhenTypeIs<DateTime>());
        }

        [Test, Ignore, Category("Integration")]
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
            locations.ForEach(l => l.Address.ValidationSourceCode = "PCA");

            var vacancy = CreateValidDomainVacancy();
            vacancy.VacancyGuid = vacancyGuid;
            vacancy.Title = title;
            vacancy.LocationAddresses = locations;
            vacancy.IsEmployerLocationMainApprenticeshipLocation = (vacancy.LocationAddresses.Count == 1);

            writeRepository.DeepSave(vacancy);

            var newLocations = new Fixture()
                .Build<VacancyLocationAddress>()
                .WithAutoProperties()
                .CreateMany(newNumberOfLocations)
                .ToList();
            newLocations.ForEach(l => l.Address.ValidationSourceCode = "PCA");

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
                options => options
                    .Excluding(x => Regex.IsMatch(x.SelectedMemberPath, "[[0-9]+\\].Address.DateValidated"))
                    .Excluding(x => Regex.IsMatch(x.SelectedMemberPath, "[[0-9]+\\].Address.County"))
                    .Excluding(x => Regex.IsMatch(x.SelectedMemberPath, "[[0-9]+\\].Address.PostalAddressId"))
                );
        }

        [Test, Ignore, Category("Integration")]
        public void ShallowSaveVacancyShouldSaveFrameworkId()
        {
            var logger = new Mock<ILogService>();
            IApprenticeshipVacancyReadRepository readRepository = new ApprenticeshipVacancyRepository(_connection, _mapper,
                logger.Object);
            IApprenticeshipVacancyWriteRepository writeRepository = new ApprenticeshipVacancyRepository(_connection, _mapper,
                logger.Object);

            var vacancy = CreateValidDomainVacancy();
            vacancy.FrameworkCodeName = "F02";
            vacancy.StandardId = null;
            vacancy.TrainingType = TrainingType.Frameworks;

            writeRepository.ShallowSave(vacancy);

            var loadedVacancy = readRepository.Get(vacancy.VacancyGuid);

            loadedVacancy.FrameworkCodeName.Should().Be(vacancy.FrameworkCodeName);
        }

        [Test, Ignore, Category("Integration")]
        public void DeepSaveVacancyShouldSaveFrameworkId()
        {
            var logger = new Mock<ILogService>();
            IApprenticeshipVacancyReadRepository readRepository = new ApprenticeshipVacancyRepository(_connection, _mapper,
                logger.Object);
            IApprenticeshipVacancyWriteRepository writeRepository = new ApprenticeshipVacancyRepository(_connection, _mapper,
                logger.Object);

            var vacancy = CreateValidDomainVacancy();
            vacancy.FrameworkCodeName = "F02";
            vacancy.StandardId = null;
            vacancy.TrainingType = TrainingType.Frameworks;

            writeRepository.DeepSave(vacancy);

            var loadedVacancy = readRepository.Get(vacancy.VacancyGuid);

            loadedVacancy.FrameworkCodeName.Should().Be(vacancy.FrameworkCodeName);
        }

        [Test, Ignore, Category("Integration")]
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

        [Test, Ignore, Category("Integration")]
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

        [Test, Ignore, Category("Integration")]
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

        [Test, Ignore, Category("Integration")]
        public void ReserveVacancyForQaTest()
        {
            var logger = new Mock<ILogService>();
            IApprenticeshipVacancyWriteRepository writeRepository = new ApprenticeshipVacancyRepository(_connection, _mapper,
                logger.Object);
            IApprenticeshipVacancyReadRepository readRepository = new ApprenticeshipVacancyRepository(_connection, _mapper,
                logger.Object);

            var vacancy = CreateValidDomainVacancy();
            vacancy.VacancyReferenceNumber = VacancyReferenceNumber++;
            vacancy.DateSubmitted = null;
            vacancy.Status = ProviderVacancyStatuses.PendingQA;

            writeRepository.DeepSave(vacancy);

            writeRepository.ReserveVacancyForQA(vacancy.VacancyReferenceNumber.Value);
            var loadedVacancy = readRepository.Get(vacancy.VacancyReferenceNumber.Value);

            loadedVacancy.Status.Should().Be(ProviderVacancyStatuses.ReservedForQA);
            loadedVacancy.DateStartedToQA.Should().BeCloseTo(DateTime.UtcNow, 1000);
        }

        [Test, Ignore, Category("Integration")]
        public void ReplaceLocationInformationTest()
        {
            var logger = new Mock<ILogService>();
            IApprenticeshipVacancyWriteRepository writeRepository = new ApprenticeshipVacancyRepository(_connection, _mapper,
                logger.Object);
            IApprenticeshipVacancyReadRepository readRepository = new ApprenticeshipVacancyRepository(_connection, _mapper,
                logger.Object);

            var vacancy = CreateValidDomainVacancy();
            vacancy.VacancyReferenceNumber = VacancyReferenceNumber++;
            vacancy.DateSubmitted = null;
            vacancy.Status = ProviderVacancyStatuses.PendingQA;

            writeRepository.DeepSave(vacancy);

            const int newNumberOfLocations = 13;

            var newLocations = new Fixture()
                .Build<VacancyLocationAddress>()
                .WithAutoProperties()
                .CreateMany(newNumberOfLocations)
                .ToList();
            newLocations.ForEach(l => l.Address.ValidationSourceCode = "PCA");

            const string aComment = "A comment";

            bool? isEmployerLocationMainApprenticeshipLocation = false;
            int? numberOfPositions = null;
            const string locationAddressesComment = aComment;
            const string additionalLocationInformation = "additional location information";
            const string additionalLocationInformationComment = aComment;

            writeRepository.ReplaceLocationInformation(vacancy.VacancyGuid,
                isEmployerLocationMainApprenticeshipLocation, numberOfPositions, newLocations, locationAddressesComment,
                additionalLocationInformation, additionalLocationInformationComment);

            var dbVacancy = readRepository.Get(vacancy.VacancyReferenceNumber.Value);

            dbVacancy.NumberOfPositions.Should().Be(numberOfPositions);
            dbVacancy.AdditionalLocationInformation.Should().Be(additionalLocationInformation);
            dbVacancy.AdditionalLocationInformationComment.Should().Be(additionalLocationInformationComment);
            dbVacancy.LocationAddressesComment.Should().Be(locationAddressesComment);
            dbVacancy.LocationAddresses.ShouldBeEquivalentTo(newLocations,
                options => options
                    .Excluding(x => Regex.IsMatch(x.SelectedMemberPath, "[[0-9]+\\].Address.DateValidated"))
                    .Excluding(x => Regex.IsMatch(x.SelectedMemberPath, "[[0-9]+\\].Address.County"))
                    .Excluding(x => Regex.IsMatch(x.SelectedMemberPath, "[[0-9]+\\].Address.PostalAddressId")));
            dbVacancy.IsEmployerLocationMainApprenticeshipLocation.Should()
                .Be(isEmployerLocationMainApprenticeshipLocation);
        }*/
    }
}