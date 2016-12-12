namespace SFA.DAS.RAA.Api.AcceptanceTests.Comparers
{
    using Apprenticeships.Domain.Entities.Raa.Vacancies;
    using DbVacancy = Apprenticeships.Infrastructure.Repositories.Sql.Schemas.Vacancy.Entities.Vacancy;

    public class DbVacancyComparer : IMultiEqualityComparer<DbVacancy, Vacancy>
    {
        public bool Equals(DbVacancy object1, Vacancy object2)
        {
            throw new System.NotImplementedException();
        }

        public int GetHashCode(DbVacancy object1)
        {
            throw new System.NotImplementedException();
        }

        public int GetHashCode(Vacancy object2)
        {
            throw new System.NotImplementedException();
        }
    }
}