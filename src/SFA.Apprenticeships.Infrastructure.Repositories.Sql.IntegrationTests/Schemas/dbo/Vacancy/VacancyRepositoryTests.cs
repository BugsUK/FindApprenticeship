namespace SFA.Apprenticeships.Infrastructure.Repositories.Sql.IntegrationTests.Schemas.dbo.Vacancy
{
    using Common;
    using Domain.Entities.Raa.Locations;
    using Domain.Entities.Raa.Vacancies;
    using Domain.Raa.Interfaces.Queries;
    using Domain.Raa.Interfaces.Repositories;
    using FluentAssertions;
    using Moq;
    using NUnit.Framework;
    using Ploeh.AutoFixture;
    using SFA.Apprenticeships.Application.Interfaces;
    using Sql.Common;
    using Sql.Schemas.Vacancy;
    using System;
    using System.Collections.Generic;

    [TestFixture(Category = "Integration")]
    public class VacancyRepositoryTests : TestBase
    {
        private const string UserName = "IntegrationTestUserName";

        private readonly IMapper _mapper = new VacancyMappers();
        private readonly Mock<ILogService> _logger = new Mock<ILogService>();
        private readonly Mock<IConfigurationService> _configurationService = new Mock<IConfigurationService>();
        private IGetOpenConnection _connection;

        private Mock<IDateTimeService> _dateTimeService;
        private Mock<ICurrentUserService> _currentUserService;

        [OneTimeSetUp]
        public void SetUpFixture()
        {
            _connection =
                new GetOpenConnectionFromConnectionString(DatabaseConfigurationProvider.Instance.TargetConnectionString);

            _dateTimeService = new Mock<IDateTimeService>();
            _currentUserService = new Mock<ICurrentUserService>();
        }

        [SetUp]
        public void SetUp()
        {
            _dateTimeService.Setup(d => d.MinValue).Returns(DateTime.MinValue);
            _dateTimeService.Setup(d => d.UtcNow).Returns(DateTime.UtcNow);
            _currentUserService.Setup(cus => cus.CurrentUserName).Returns(UserName);
        }

        [Test, Category("Integration")]
        public void SimpleGetTest()
        {
            IVacancyReadRepository readRepository = new VacancyRepository(_connection, _mapper,
                _dateTimeService.Object, _logger.Object, _currentUserService.Object);
            IVacancyWriteRepository writeRepository = new VacancyRepository(_connection, _mapper,
                _dateTimeService.Object, _logger.Object, _currentUserService.Object);

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
            vacancy.VacancyManagerId = SeedData.ProviderSites.HopwoodCampus.ProviderSiteId;
            vacancy.Address.Postcode = "CV1 2WT";
            vacancy.Address.County = "West Midlands";
            vacancy.VacancyOwnerRelationshipId = SeedData.VacancyOwnerRelationships.TestOne.VacancyOwnerRelationshipId;
            vacancy.FrameworkCodeName = null;
            vacancy.SectorCodeName = "ALB";
            vacancy.ContractOwnerId = SeedData.Providers.HopwoodHallCollege.ProviderId;
            vacancy.Duration = 2;
            vacancy.DurationType = DurationType.Years;
            vacancy.ExpectedDuration = "2 years";
            vacancy.EmployerId = SeedData.Employers.AcmeCorp.EmployerId;
            vacancy.EmployerLocation = "Coventry";

            writeRepository.Create(vacancy);

            var entity = readRepository.GetByReferenceNumber(vacancy.VacancyReferenceNumber);

            entity.AdditionalLocationInformationComment = "AdditionalLocationInformationComment";
            writeRepository.Update(entity);
            entity = readRepository.GetByReferenceNumber(vacancy.VacancyReferenceNumber);

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
                    .Excluding(x => x.CreatedDateTime)
                    .Excluding(x => x.CreatedByProviderUsername)
                    .Excluding(x => x.VacancyLocationType)
                    .Excluding(x => x.OtherInformation)
                    .Excluding(x => x.EmployerName));

            entity.AdditionalLocationInformationComment.Should().Be("AdditionalLocationInformationComment");
        }

        [Test, Category("Integration")]
        public void CreatedDateTimeTest()
        {
            var now = new DateTime(2016, 3, 16);
            _dateTimeService.Setup(ds => ds.UtcNow).Returns(now);

            IVacancyReadRepository readRepository = new VacancyRepository(_connection, _mapper,
                _dateTimeService.Object, _logger.Object, _currentUserService.Object);
            IVacancyWriteRepository writeRepository = new VacancyRepository(_connection, _mapper,
                _dateTimeService.Object, _logger.Object, _currentUserService.Object);

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
            vacancy.VacancyManagerId = SeedData.ProviderSites.HopwoodCampus.ProviderSiteId;
            vacancy.Address.Postcode = "CV1 2WT";
            vacancy.Address.County = "West Midlands";
            vacancy.VacancyOwnerRelationshipId = SeedData.VacancyOwnerRelationships.TestOne.VacancyOwnerRelationshipId;
            vacancy.FrameworkCodeName = null;
            vacancy.SectorCodeName = "ALB";
            vacancy.ContractOwnerId = SeedData.Providers.HopwoodHallCollege.ProviderId;

            vacancy = writeRepository.Create(vacancy);
            vacancy.Status = VacancyStatus.Submitted;
            vacancy = writeRepository.Update(vacancy);
            vacancy.Status = VacancyStatus.Referred;
            vacancy = writeRepository.Update(vacancy);
            vacancy.Status = VacancyStatus.Submitted;
            vacancy = writeRepository.Update(vacancy);
            vacancy.Status = VacancyStatus.Live;
            vacancy = writeRepository.Update(vacancy);

            var entity = readRepository.GetByReferenceNumber(vacancy.VacancyReferenceNumber);

            entity.CreatedDateTime.Should().Be(now);
        }

        [Test, Category("Integration")]
        public void SimpleSaveAndUpdateTest()
        {
            IVacancyWriteRepository writeRepository = new VacancyRepository(_connection, _mapper,
                _dateTimeService.Object, _logger.Object, _currentUserService.Object);

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
            vacancy.VacancyManagerId = SeedData.ProviderSites.HopwoodCampus.ProviderSiteId;
            vacancy.Address.Postcode = "CV1 2WT";
            vacancy.Address.County = "West Midlands";
            vacancy.VacancyOwnerRelationshipId = SeedData.VacancyOwnerRelationships.TestOne.VacancyOwnerRelationshipId;
            vacancy.FrameworkCodeName = null;
            vacancy.SectorCodeName = "ALB";
            vacancy.ContractOwnerId = SeedData.Providers.HopwoodHallCollege.ProviderId;

            var entity = writeRepository.Create(vacancy);
            vacancy.VacancyId = entity.VacancyId;
            vacancy.TrainingProvided = null;
            writeRepository.Update(vacancy);
        }

        [Test, Category("Integration")]
        public void FindTest()
        {
            IVacancyWriteRepository writeRepository = new VacancyRepository(_connection, _mapper,
                _dateTimeService.Object, _logger.Object, _currentUserService.Object);
            IVacancySummaryRepository summaryRepository = new VacancySummaryRepository(_connection);

            const string title = "Vacancy title";
            var vacancyGuid = Guid.NewGuid();

            var fixture = new Fixture();
            fixture.Customizations.Add(
                new StringGenerator(() =>
                    Guid.NewGuid().ToString().Substring(0, 10)));

            var vacancy = CreateValidDomainVacancy();
            vacancy.VacancyGuid = vacancyGuid;
            vacancy.Title = title;
            vacancy.Status = VacancyStatus.Live;
            vacancy.VacancyManagerId = SeedData.ProviderSites.HopwoodCampus.ProviderSiteId;
            vacancy.VacancyOwnerRelationshipId = SeedData.VacancyOwnerRelationships.TestOne.VacancyOwnerRelationshipId;
            vacancy.UpdatedDateTime = null;
            vacancy.CreatedDateTime = DateTime.MinValue;
            vacancy.ClosingDate = DateTime.UtcNow.AddDays(2);
            vacancy.ContractOwnerId = SeedData.Providers.HopwoodHallCollege.ProviderId;

            writeRepository.Create(vacancy);

            int totalResultsCount;
            var findResults = summaryRepository.Find(new ApprenticeshipVacancyQuery
            {
                RequestedPage = 1,
                LatestClosingDate = DateTime.UtcNow.AddDays(3),
                LiveDate = DateTime.Today.AddHours(-2),
                PageSize = 10,
                DesiredStatuses = new List<VacancyStatus> { VacancyStatus.Live }
            }, out totalResultsCount);

            findResults.Count.Should().BeGreaterThan(0);
            totalResultsCount.Should().BeGreaterThan(0);

            findResults = summaryRepository.Find(new ApprenticeshipVacancyQuery
            {
                RequestedPage = 1,
                LatestClosingDate = DateTime.UtcNow.AddDays(3),
                LiveDate = DateTime.Today.AddHours(-2),
                PageSize = 10,
                DesiredStatuses = new List<VacancyStatus> { VacancyStatus.Closed }
            }, out totalResultsCount);

            findResults.Should().HaveCount(0);
            totalResultsCount.Should().Be(0);

            findResults = summaryRepository.Find(new ApprenticeshipVacancyQuery
            {
                RequestedPage = 2,
                LatestClosingDate = DateTime.UtcNow.AddDays(3),
                LiveDate = DateTime.Today.AddHours(-2),
                PageSize = 10,
                DesiredStatuses = new List<VacancyStatus> { VacancyStatus.Live }
            }, out totalResultsCount);

            findResults.Should().HaveCount(0);
            totalResultsCount.Should().Be(0);

            findResults = summaryRepository.Find(new ApprenticeshipVacancyQuery
            {
                RequestedPage = 2,
                LatestClosingDate = DateTime.UtcNow.AddDays(1),
                LiveDate = DateTime.Today.AddHours(-2),
                PageSize = 10,
                DesiredStatuses = new List<VacancyStatus> { VacancyStatus.Live }
            }, out totalResultsCount);

            findResults.Should().HaveCount(0);
            totalResultsCount.Should().Be(0);

            findResults = summaryRepository.Find(new ApprenticeshipVacancyQuery
            {
                RequestedPage = 2,
                LatestClosingDate = DateTime.UtcNow.AddDays(3),
                LiveDate = DateTime.Today.AddHours(2),
                PageSize = 10,
                DesiredStatuses = new List<VacancyStatus> { VacancyStatus.Live }
            }, out totalResultsCount);

            findResults.Should().HaveCount(0);
            totalResultsCount.Should().Be(0);
        }

        [Test, Category("Integration")]
        public void GetByIdsTest()
        {
            IVacancyWriteRepository writeRepository = new VacancyRepository(_connection, _mapper,
                _dateTimeService.Object, _logger.Object, _currentUserService.Object);
            IVacancyReadRepository readRepository = new VacancyRepository(_connection, _mapper,
                _dateTimeService.Object, _logger.Object, _currentUserService.Object);
            var summaryRepository = new VacancySummaryRepository(_connection);

            var vacancy1 = CreateValidDomainVacancy();
            vacancy1.VacancyManagerId = SeedData.ProviderSites.HopwoodCampus.ProviderSiteId;
            vacancy1.VacancyOwnerRelationshipId = SeedData.VacancyOwnerRelationships.TestOne.VacancyOwnerRelationshipId;
            vacancy1.ContractOwnerId = SeedData.Providers.HopwoodHallCollege.ProviderId;

            var vacancy2 = CreateValidDomainVacancy();
            vacancy2.VacancyManagerId = SeedData.ProviderSites.HopwoodCampus.ProviderSiteId;
            vacancy2.VacancyOwnerRelationshipId = SeedData.VacancyOwnerRelationships.TestOne.VacancyOwnerRelationshipId;
            vacancy2.ContractOwnerId = SeedData.Providers.HopwoodHallCollege.ProviderId;

            var vacancy3 = CreateValidDomainVacancy();
            vacancy3.VacancyManagerId = SeedData.ProviderSites.HopwoodCampus.ProviderSiteId;
            vacancy3.VacancyOwnerRelationshipId = SeedData.VacancyOwnerRelationships.TestOne.VacancyOwnerRelationshipId;
            vacancy3.ContractOwnerId = SeedData.Providers.HopwoodHallCollege.ProviderId;

            vacancy1 = writeRepository.Create(vacancy1);
            vacancy2 = writeRepository.Create(vacancy2);
            vacancy3 = writeRepository.Create(vacancy3);

            var vacancies = summaryRepository.GetByIds(new[] { vacancy1.VacancyId, vacancy2.VacancyId, vacancy3.VacancyId });

            vacancies.Count.Should().Be(3);
            foreach (var vacancySummary in vacancies)
            {
                vacancySummary.VacancyId.Should().NotBe(0);
            }
        }

        [Test, Category("Integration")]
        public void GetMultiLocationTest()
        {
            IVacancyWriteRepository writeRepository = new VacancyRepository(_connection, _mapper,
                _dateTimeService.Object, _logger.Object, _currentUserService.Object);
            IVacancyReadRepository readRepository = new VacancyRepository(_connection, _mapper,
                _dateTimeService.Object, _logger.Object, _currentUserService.Object);

            IVacancyLocationWriteRepository locationWriteRepository = new VacancyLocationRepository(_connection, _mapper,
                _logger.Object);

            IVacancyLocationReadRepository locationReadRepository = new VacancyLocationRepository(_connection, _mapper,
                _logger.Object);

            var vacancy = CreateValidDomainVacancy();
            vacancy.VacancyManagerId = SeedData.ProviderSites.HopwoodCampus.ProviderSiteId;
            vacancy.Address.Postcode = null;
            vacancy.Address.County = null;
            vacancy.VacancyOwnerRelationshipId = SeedData.VacancyOwnerRelationships.TestOne.VacancyOwnerRelationshipId;
            vacancy.VacancyLocationType = VacancyLocationType.MultipleLocations;
            vacancy.ContractOwnerId = SeedData.Providers.HopwoodHallCollege.ProviderId;

            vacancy = writeRepository.Create(vacancy);

            var vacancyLocations = new List<VacancyLocation>
            {
                new VacancyLocation
                {
                    VacancyId = vacancy.VacancyId,
                    Address = new PostalAddress
                    {
                        Postcode = "SW2 4NT",
                        County = "West Midlands"
                    }
                },
                new VacancyLocation
                {
                    VacancyId = vacancy.VacancyId,
                    Address = new PostalAddress
                    {
                        Postcode = "B26 2LW",
                        County = "West Midlands"
                    }
                }
            };

            locationWriteRepository.Create(vacancyLocations);

            var retrievedLocations = locationReadRepository.GetForVacancyId(vacancy.VacancyId);

            retrievedLocations.Should().HaveCount(vacancyLocations.Count);
            retrievedLocations.Exists(x => x.Address.Postcode == "SW2 4NT").Should().BeTrue();
            retrievedLocations.Exists(x => x.Address.Postcode == "B26 2LW").Should().BeTrue();
        }

        [Test, Category("Integration")]
        public void IncrementNumberOfClickThroughs()
        {
            IVacancyWriteRepository writeRepository = new VacancyRepository(_connection, _mapper,
                _dateTimeService.Object, _logger.Object, _currentUserService.Object);
            IVacancyReadRepository readRepository = new VacancyRepository(_connection, _mapper,
                _dateTimeService.Object, _logger.Object, _currentUserService.Object);


            var vacancy = CreateValidDomainVacancy();
            vacancy.VacancyGuid = Guid.NewGuid();
            vacancy.Title = "Test";
            vacancy.Status = VacancyStatus.Draft;
            // Changed from PendingQA to Draft because PendingQA is not still in the db
            vacancy.VacancyManagerId = SeedData.ProviderSites.HopwoodCampus.ProviderSiteId;
            vacancy.Address.Postcode = "CV1 2WT";
            vacancy.Address.County = "West Midlands";
            vacancy.VacancyOwnerRelationshipId = SeedData.VacancyOwnerRelationships.TestOne.VacancyOwnerRelationshipId;
            vacancy.FrameworkCodeName = null;
            vacancy.SectorCodeName = "ALB";
            vacancy.ContractOwnerId = SeedData.Providers.HopwoodHallCollege.ProviderId;
            vacancy.Duration = 2;
            vacancy.DurationType = DurationType.Years;
            vacancy.ExpectedDuration = "2 years";

            vacancy.NoOfOfflineApplicants = 0;

            vacancy = writeRepository.Create(vacancy);

            writeRepository.IncrementOfflineApplicationClickThrough(vacancy.VacancyId);
            writeRepository.IncrementOfflineApplicationClickThrough(vacancy.VacancyId);
            writeRepository.IncrementOfflineApplicationClickThrough(vacancy.VacancyId);
            writeRepository.IncrementOfflineApplicationClickThrough(vacancy.VacancyId);
            writeRepository.IncrementOfflineApplicationClickThrough(vacancy.VacancyId);

            var entity = readRepository.Get(vacancy.VacancyId);

            entity.NoOfOfflineApplicants.Should().Be(5);
        }
    }
}