namespace SFA.Apprenticeships.Infrastructure.Repositories.Sql.Tests.Schemas.Vacancy
{
    using System;
    using System.Collections.Generic;
    using Common;
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
        private readonly IMapper _mapper = new ApprenticeshipVacancyMappers();
        private IGetOpenConnection _connection;

        private Mock<IDateTimeService> _dateTimeService;

        [TestFixtureSetUp]
        public void SetUpFixture()
        {
            _connection =
                new GetOpenConnectionFromConnectionString(DatabaseConfigurationProvider.Instance.TargetConnectionString);

            _dateTimeService = new Mock<IDateTimeService>();
            _dateTimeService.Setup(d => d.MinValue).Returns(DateTime.MinValue);
            _dateTimeService.Setup(d => d.UtcNow).Returns(DateTime.UtcNow);
        }

        [Test, Category("Integration")]
        public void SimpleGetTest()
        {
            var logger = new Mock<ILogService>();
            
            IVacancyReadRepository readRepository = new VacancyRepository(_connection, _mapper,
                _dateTimeService.Object, logger.Object);
            IVacancyWriteRepository writeRepository = new VacancyRepository(_connection, _mapper,
                _dateTimeService.Object, logger.Object);

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
                    .Excluding(x => x.CreatedDateTime));
        }

        [Test, Category("Integration")]
        public void SimpleSaveAndUpdateTest()
        {
            var logger = new Mock<ILogService>();
            IVacancyWriteRepository writeRepository = new VacancyRepository(_connection, _mapper,
                _dateTimeService.Object, logger.Object);
            
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
            var logger = new Mock<ILogService>();

            IVacancyWriteRepository writeRepository = new VacancyRepository(_connection, _mapper,
                _dateTimeService.Object, logger.Object);
            IVacancyReadRepository readRepository = new VacancyRepository(_connection, _mapper,
                _dateTimeService.Object, logger.Object);

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
            vacancy.VacancyManagerId = 1;
            vacancy.OwnerPartyId = 1;
            vacancy.UpdatedDateTime = null;
            vacancy.CreatedDateTime = DateTime.MinValue;
            vacancy.ClosingDate = DateTime.UtcNow.AddDays(2);

            var entity = writeRepository.Create(vacancy);

            var totalResultsCount = 0;
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
    }
}