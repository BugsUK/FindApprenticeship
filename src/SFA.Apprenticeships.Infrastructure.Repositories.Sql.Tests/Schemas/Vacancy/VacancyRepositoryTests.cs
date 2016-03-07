namespace SFA.Apprenticeships.Infrastructure.Repositories.Sql.Tests.Schemas.Vacancy
{
    using System;
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

            var entity = writeRepository.Create(vacancy);
            vacancy.VacancyId = entity.VacancyId;
            vacancy.TrainingProvided = null;
            writeRepository.Update(vacancy);
        }

        [Test, Category("Integration")]
        public void FindTest()
        {
            var logger = new Mock<ILogService>();
            var dateTimeService = new Mock<IDateTimeService>();
            IVacancyWriteRepository writeRepository = new VacancyRepository(_connection, _mapper,
                dateTimeService.Object, logger.Object);
            IVacancyReadRepository readRepository = new VacancyRepository(_connection, _mapper,
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
            vacancy.Status = VacancyStatus.Live;
            vacancy.TrainingProvided = null;

            var entity = writeRepository.Create(vacancy);
            vacancy.VacancyId = entity.VacancyId;
            vacancy.TrainingProvided = null;

            var totalResultsCount = 0;
            var findResults = readRepository.Find(new ApprenticeshipVacancyQuery
            {
                CurrentPage = 1,
                LatestClosingDate = DateTime.UtcNow.AddHours(2),
                LiveDate = DateTime.UtcNow.AddHours(-2)
            }, out totalResultsCount);
        }
    }
}