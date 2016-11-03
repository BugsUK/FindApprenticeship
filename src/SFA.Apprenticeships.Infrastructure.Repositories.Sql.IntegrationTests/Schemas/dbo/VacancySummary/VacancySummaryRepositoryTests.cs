namespace SFA.Apprenticeships.Infrastructure.Repositories.Sql.IntegrationTests.Schemas.dbo.VacancySummary
{
    using System;
    using System.Collections.Generic;
    using SFA.Apprenticeships.Infrastructure.Repositories.Sql.IntegrationTests.Common;
    using SFA.Apprenticeships.Domain.Entities.Raa.Vacancies;
    using SFA.Apprenticeships.Domain.Raa.Interfaces.Queries;
    using NUnit.Framework;
    using SFA.Apprenticeships.Infrastructure.Repositories.Sql.Common;
    using SFA.Apprenticeships.Infrastructure.Repositories.Sql.Schemas.Vacancy;
    using SFA.Apprenticeships.Infrastructure.Repositories.Sql.IntegrationTests.Schemas.dbo.Vacancy;

    [TestFixture]
    public class VacancySummaryRepositoryTests : TestBase
    {
        private GetOpenConnectionFromConnectionString _connection;

        [OneTimeSetUp]
        public void Meep()
        {
            _connection =
                new GetOpenConnectionFromConnectionString(DatabaseConfigurationProvider.Instance.TargetConnectionString);
        }

        [Test, Category("Integration")]
        public void Test()
        {
            var vacanySummaryRepository = new VacancySummaryRepository(_connection);

            var query = new ApprenticeshipVacancyQuery()
            {
                RequestedPage = 1,
                DesiredStatuses = new List<VacancyStatus>() { VacancyStatus.Live },
                LatestClosingDate = DateTime.Now,
                EditedInRaa = false,
                PageSize = 1000
            };

            int resultCount;

            var eligibleVacancies = vacanySummaryRepository.Find(query, out resultCount);

            Assert.GreaterOrEqual(1, eligibleVacancies.Count);
        }
    }
}
