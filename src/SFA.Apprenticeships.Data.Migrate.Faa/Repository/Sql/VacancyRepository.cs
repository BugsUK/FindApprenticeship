namespace SFA.Apprenticeships.Data.Migrate.Faa.Repository.Sql
{
    using System.Collections.Generic;
    using Infrastructure.Repositories.Sql.Common;

    public class VacancyRepository
    {
        private readonly IGetOpenConnection _getOpenConnection;

        public VacancyRepository(IGetOpenConnection getOpenConnection)
        {
            _getOpenConnection = getOpenConnection;
        }

        public HashSet<int> GetAllVacancyIds()
        {
            return new HashSet<int>(_getOpenConnection.Query<int>("SELECT VacancyId FROM Vacancy"));
        }
    }
}