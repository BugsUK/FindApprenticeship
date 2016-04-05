namespace SFA.Apprenticeships.Infrastructure.Repositories.Sql.IntegrationTests.Schemas.dbo.Vacancy
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Common;
    using Domain.Entities.Raa.Locations;
    using Domain.Entities.Raa.Reference;
    using Domain.Entities.Raa.Vacancies;
    using Domain.Raa.Interfaces.Queries;
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
        private const string UserName = "IntegrationTestUserName";

        private readonly IMapper _mapper = new VacancyMappers();
        private readonly Mock<ILogService> _logger = new Mock<ILogService>();
        private IGetOpenConnection _connection;

        private Mock<IDateTimeService> _dateTimeService;
        private Mock<ICurrentUserService> _currentUserService;

        [TestFixtureSetUp]
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
            vacancy.Address.County = "CAM";
            vacancy.OwnerPartyId = SeedData.VacancyOwnerRelationships.TestOne.VacancyOwnerRelationshipId;
            vacancy.FrameworkCodeName = null;
            vacancy.SectorCodeName = "ALB";

            writeRepository.Create(vacancy);

            var entity = readRepository.GetByReferenceNumber(vacancy.VacancyReferenceNumber);

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
                    .Excluding(x => x.WageUnit)); //remove this after changes in DB
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
            vacancy.Address.County = "CAM";
            vacancy.OwnerPartyId = SeedData.VacancyOwnerRelationships.TestOne.VacancyOwnerRelationshipId;
            vacancy.FrameworkCodeName = null;
            vacancy.SectorCodeName = "ALB";

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
            vacancy.Address.County = "CAM";
            vacancy.OwnerPartyId = SeedData.VacancyOwnerRelationships.TestOne.VacancyOwnerRelationshipId;
            vacancy.FrameworkCodeName = null;
            vacancy.SectorCodeName = "ALB";

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
            IVacancyReadRepository readRepository = new VacancyRepository(_connection, _mapper,
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
            vacancy.Status = VacancyStatus.Live;
            vacancy.VacancyManagerId = SeedData.ProviderSites.HopwoodCampus.ProviderSiteId;
            vacancy.OwnerPartyId = SeedData.VacancyOwnerRelationships.TestOne.VacancyOwnerRelationshipId;
            vacancy.UpdatedDateTime = null;
            vacancy.CreatedDateTime = DateTime.MinValue;
            vacancy.ClosingDate = DateTime.UtcNow.AddDays(2);

            writeRepository.Create(vacancy);

            int totalResultsCount;
            var findResults = readRepository.Find(new ApprenticeshipVacancyQuery
            {
                CurrentPage = 1,
                LatestClosingDate = DateTime.UtcNow.AddDays(3),
                LiveDate = DateTime.UtcNow.AddHours(-2),
                PageSize = 10,
                DesiredStatuses = new List<VacancyStatus> { VacancyStatus.Live }
            }, out totalResultsCount);

            findResults.Should().HaveCount(1);
            totalResultsCount.Should().Be(1);

            findResults = readRepository.Find(new ApprenticeshipVacancyQuery
            {
                CurrentPage = 1,
                LatestClosingDate = DateTime.UtcNow.AddDays(3),
                LiveDate = DateTime.UtcNow.AddHours(-2),
                PageSize = 10,
                DesiredStatuses = new List<VacancyStatus> { VacancyStatus.Closed }
            }, out totalResultsCount);

            findResults.Should().HaveCount(0);
            totalResultsCount.Should().Be(0);

            findResults = readRepository.Find(new ApprenticeshipVacancyQuery
            {
                CurrentPage = 2,
                LatestClosingDate = DateTime.UtcNow.AddDays(3),
                LiveDate = DateTime.UtcNow.AddHours(-2),
                PageSize = 10,
                DesiredStatuses = new List<VacancyStatus> { VacancyStatus.Live }
            }, out totalResultsCount);

            findResults.Should().HaveCount(0);
            totalResultsCount.Should().Be(1);

            findResults = readRepository.Find(new ApprenticeshipVacancyQuery
            {
                CurrentPage = 2,
                LatestClosingDate = DateTime.UtcNow.AddDays(1),
                LiveDate = DateTime.UtcNow.AddHours(-2),
                PageSize = 10,
                DesiredStatuses = new List<VacancyStatus> { VacancyStatus.Live }
            }, out totalResultsCount);

            findResults.Should().HaveCount(0);
            totalResultsCount.Should().Be(0);

            findResults = readRepository.Find(new ApprenticeshipVacancyQuery
            {
                CurrentPage = 2,
                LatestClosingDate = DateTime.UtcNow.AddDays(3),
                LiveDate = DateTime.UtcNow.AddHours(2),
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

            var vacancy1 = CreateValidDomainVacancy();
            vacancy1.VacancyManagerId = SeedData.ProviderSites.HopwoodCampus.ProviderSiteId;
            vacancy1.Address.Postcode = "B26 2LW";
            vacancy1.OwnerPartyId = SeedData.VacancyOwnerRelationships.TestOne.VacancyOwnerRelationshipId;

            var vacancy2 = CreateValidDomainVacancy();
            vacancy2.VacancyManagerId = SeedData.ProviderSites.HopwoodCampus.ProviderSiteId;
            vacancy2.Address.Postcode = "SW2 4NT";
            vacancy2.OwnerPartyId = SeedData.VacancyOwnerRelationships.TestOne.VacancyOwnerRelationshipId;

            var vacancy3 = CreateValidDomainVacancy();
            vacancy3.VacancyManagerId = SeedData.ProviderSites.HopwoodCampus.ProviderSiteId;
            vacancy3.Address.Postcode = "DE6 5JA";
            vacancy3.OwnerPartyId = SeedData.VacancyOwnerRelationships.TestOne.VacancyOwnerRelationshipId;

            vacancy1 = writeRepository.Create(vacancy1);
            vacancy2 = writeRepository.Create(vacancy2);
            vacancy3 = writeRepository.Create(vacancy3);

            var vacancies = readRepository.GetByIds(new[] {vacancy1.VacancyId, vacancy2.VacancyId, vacancy3.VacancyId});

            vacancies.Count.Should().Be(3);
            foreach (var vacancySummary in vacancies)
            {
                vacancySummary.VacancyId.Should().NotBe(0);
            }
            vacancies.Single(v => v.VacancyId == vacancy1.VacancyId).RegionalTeam.Should().Be(RegionalTeam.WestMidlands);
            vacancies.Single(v => v.VacancyId == vacancy2.VacancyId).RegionalTeam.Should().Be(RegionalTeam.SouthEast);
            vacancies.Single(v => v.VacancyId == vacancy3.VacancyId).RegionalTeam.Should().Be(RegionalTeam.EastMidlands);
        }

        [Test, Category("Integration")]
        public void GetMultiLocationTest()
        {
            IVacancyWriteRepository writeRepository = new VacancyRepository(_connection, _mapper,
                _dateTimeService.Object, _logger.Object, _currentUserService.Object);
            IVacancyReadRepository readRepository = new VacancyRepository(_connection, _mapper,
                _dateTimeService.Object, _logger.Object, _currentUserService.Object);

            IVacancyLocationWriteRepository locationWriteRepository = new VacancyLocationRepository(_connection, _mapper,
                _dateTimeService.Object, _logger.Object);

            var vacancy = CreateValidDomainVacancy();
            vacancy.VacancyManagerId = SeedData.ProviderSites.HopwoodCampus.ProviderSiteId;
            vacancy.Address.Postcode = null;
            vacancy.OwnerPartyId = SeedData.VacancyOwnerRelationships.TestOne.VacancyOwnerRelationshipId;
            vacancy.IsEmployerLocationMainApprenticeshipLocation = false;

            vacancy = writeRepository.Create(vacancy);
            
            var vacancyLocations = new List<VacancyLocation>
            {
                new VacancyLocation
                {
                    VacancyId = vacancy.VacancyId,
                    Address = new PostalAddress
                    {
                        Postcode = "SW2 4NT"
                    }
                },
                new VacancyLocation
                {
                    VacancyId = vacancy.VacancyId,
                    Address = new PostalAddress
                    {
                        Postcode = "B26 2LW"
                    }
                }
            };

            locationWriteRepository.Save(vacancyLocations);

            var entity = readRepository.Get(vacancy.VacancyId);

            entity.RegionalTeam.Should().Be(RegionalTeam.SouthEast);
        }
    }
}