using System;
using TechTalk.SpecFlow;

namespace SFA.DAS.RAA.Api.AcceptanceTests.Steps
{
    using Apprenticeships.Application.Interfaces;
    using Apprenticeships.Domain.Entities.Raa.Vacancies;
    using Apprenticeships.Domain.Entities.Vacancies;
    using Apprenticeships.Infrastructure.Repositories.Sql.Schemas.Vacancy;
    using Factories;
    using Moq;
    using Ploeh.AutoFixture;
    using DbVacancy = Apprenticeships.Infrastructure.Repositories.Sql.Schemas.Vacancy.Entities.Vacancy;

    [Binding]
    public class SetupVacancySteps
    {
        private static readonly IMapper VacancyMappers = new VacancyMappers();

        [Given(@"I have a (.*) vacancy with id: (.*), a fixed wage of £(.*) (.*)")]
        public void GivenIHaveAVacancyWithAFixedWage(string vacancyStatusString, int vacancyId, decimal amount, string unitString)
        {
            var vacancyStatus = (VacancyStatus)Enum.Parse(typeof(VacancyStatus), vacancyStatusString);
            var wageUnit = (WageUnit)Enum.Parse(typeof(WageUnit), unitString);

            var wage = new Wage(WageType.Custom, amount, null, null, $"£{amount} {wageUnit}", wageUnit, 20, null);
            var vacancy = new Fixture().Build<Vacancy>()
                .With(v => v.VacancyId, vacancyId)
                .With(v => v.Status, vacancyStatus)
                .With(v => v.Wage, wage)
                .Create();

            ScenarioContext.Current.Add($"vacancyId: {vacancy.VacancyId}", vacancy);

            var dbVacancy = VacancyMappers.Map<Vacancy, DbVacancy>(vacancy);

            RaaMockFactory.GetMockGetOpenConnection().Setup(
                m => m.Query<DbVacancy>(VacancyRepository.SelectByIdSql, It.Is<object>(o => o.GetHashCode() == new { vacancyId }.GetHashCode()), null, null))
                .Returns(new[] { dbVacancy });
        }
    }
}
