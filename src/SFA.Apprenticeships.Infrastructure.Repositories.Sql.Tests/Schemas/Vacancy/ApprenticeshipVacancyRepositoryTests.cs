namespace SFA.Apprenticeships.Infrastructure.Repositories.Sql.Tests.Schemas.Vacancy
{
    using System;
    using System.Text.RegularExpressions;
    using FluentAssertions;
    using Moq;
    using NUnit.Framework;
    using Sql.Common;
    using Sql.Schemas.Vacancy;
    using Common;
    using Domain.Raa.Interfaces.Repositories;
    using SFA.Infrastructure.Interfaces;
    using TrainingType = Domain.Entities.Raa.Vacancies.TrainingType;
    using WageType = Domain.Entities.Raa.Vacancies.WageType;

    [TestFixture(Category = "Integration")]
    public class ApprenticeshipVacancyRepositoryTests : TestBase
    {
        private readonly IMapper _mapper = new ApprenticeshipVacancyMappers();
        private IGetOpenConnection _connection;

        [TestFixtureSetUp]
        public void SetUpFixture()
        {
            _connection =
                new GetOpenConnectionFromConnectionString(DatabaseConfigurationProvider.Instance.TargetConnectionString);
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
    }
}