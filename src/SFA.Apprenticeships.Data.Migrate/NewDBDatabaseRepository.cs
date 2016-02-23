namespace SFA.Apprenticeships.Data.Migrate
{
    using System;
    using System.Linq;

    /*
    using Vacancy = SFA.Apprenticeships.NewDB.Domain.Entities.Vacancy;
    using Infrastructure.Sql;

    public class NewDBDatabaseRespository : INewDBRepository
    {
        public IGetOpenConnection _getOpenConnection;

        public NewDBDatabaseRespository(IGetOpenConnection getOpenConnection)
        {
            _getOpenConnection = getOpenConnection;
        }

        public void AddVacancy(Vacancy.Vacancy vacancy)
        {
            _getOpenConnection.Insert(vacancy);
        }

        public Vacancy.Vacancy GetVacancy(Guid vacancyId)
        {
            return _getOpenConnection.Query<Vacancy.Vacancy>(@"
SELECT *
FROM   Vacancy.Vacancy
WHERE  VacancyId = @VacancyId
", new { VacancyId = vacancyId }).Single();
        }
    }
        */
}
