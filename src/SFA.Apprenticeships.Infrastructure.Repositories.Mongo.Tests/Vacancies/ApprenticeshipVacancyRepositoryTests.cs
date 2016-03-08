namespace SFA.Apprenticeships.Infrastructure.Repositories.Mongo.Tests.Vacancies
{
    using System;
    using System.Collections.Generic;
    using Domain.Entities.Raa.Vacancies;
    using Domain.Raa.Interfaces.Queries;
    using Domain.Raa.Interfaces.Repositories;
    using FluentAssertions;
    using Tests;
    using Mongo.Vacancies.Entities;
    using MongoDB.Driver;
    using MongoDB.Driver.Builders;
    using NUnit.Framework;
    using Ploeh.AutoFixture;

    public class ApprenticeshipVacancyRepositoryTests : RepositoryIntegrationTest
    {
        private int expiredVacancyIdCumTitle = 1;
        private int expiredQaVacancyIdCumTitle = 2;
        private int futureVacancyIdCumTitle = 3;

        [TearDown]
        public void TearDown()
        {
            var mongoConnectionString = MongoConfiguration.VacancyDb;
            var mongoDbName = MongoUrl.Create(mongoConnectionString).DatabaseName;

            var database = new MongoClient(mongoConnectionString)
                .GetServer()
                .GetDatabase(mongoDbName);

            var collection = database.GetCollection<MongoApprenticeshipVacancy>("apprenticeshipVacancies");
            collection.Remove(Query.EQ("Title", expiredVacancyIdCumTitle.ToString()));
            collection.Remove(Query.EQ("Title", expiredQaVacancyIdCumTitle.ToString()));
            collection.Remove(Query.EQ("Title", futureVacancyIdCumTitle.ToString()));
        }

        /// <summary>
        /// Vacancies eligible for closure are those which are:
        /// 1) past their closing date
        /// 2) in VacancyStatuses.Live
        /// </summary>
        [Test, Category("Integration")]
        public void ShouldFindVacanciesEligibleForClosure()
        {
            //Arrange
            var reader = Container.GetInstance<IVacancyReadRepository>();
            var writer = Container.GetInstance<IVacancyWriteRepository>();

            var expiredLiveVacancy = new Fixture().Build<Vacancy>()
                .With(av => av.VacancyId, expiredVacancyIdCumTitle)
                .With(av => av.Title, expiredVacancyIdCumTitle.ToString())
                .With(av => av.Status, VacancyStatus.Live)
                .With(av => av.ClosingDate, DateTime.Now.AddDays(-1))
                .Create();

            var expiredQaVacancy = new Fixture().Build<Vacancy>()
                .With(av => av.VacancyId, expiredQaVacancyIdCumTitle)
                .With(av => av.Title, expiredQaVacancyIdCumTitle.ToString())
                .With(av => av.Status, VacancyStatus.ReservedForQA)
                .With(av => av.ClosingDate, DateTime.Now.AddDays(-1))
                .Create();

            var futureLiveVacancy = new Fixture().Build<Vacancy>()
                .With(av => av.VacancyId, futureVacancyIdCumTitle)
                .With(av => av.Title, futureVacancyIdCumTitle.ToString())
                .With(av => av.Status, VacancyStatus.Live)
                .With(av => av.ClosingDate, DateTime.Now.AddDays(100))
                .Create();

            writer.Create(expiredLiveVacancy);
            writer.Create(expiredQaVacancy);
            writer.Create(futureLiveVacancy);

            var yesterday = DateTime.UtcNow.AddDays(-1);
            var endOfDay = new DateTime(yesterday.Year, yesterday.Month, yesterday.Day, 23, 59, 59);

            var query = new ApprenticeshipVacancyQuery()
            {
                CurrentPage = 1,
                PageSize = 25,
                DesiredStatuses = new List<VacancyStatus> { VacancyStatus.Live },
                LatestClosingDate = endOfDay
            };

            //Act
            int resultsCount;
            var vacanciesEligibleForClosure = reader.Find(query, out resultsCount);

            //Assert
            vacanciesEligibleForClosure.Should().NotBeEmpty();
            vacanciesEligibleForClosure.Should().NotContain(v => v.Status != VacancyStatus.Live);
            vacanciesEligibleForClosure.Should().Contain(v => v.Status == VacancyStatus.Live);
            vacanciesEligibleForClosure.Should().NotContain(v => !v.ClosingDate.HasValue);
            vacanciesEligibleForClosure.Should().Contain(v => v.ClosingDate.HasValue);
            vacanciesEligibleForClosure.Should().NotContain(v => v.ClosingDate.Value.CompareTo(endOfDay) >= 0);
            vacanciesEligibleForClosure.Should().Contain(v => v.ClosingDate.Value.CompareTo(endOfDay) < 0);
        }
    }
}