namespace SFA.Apprenticeships.Data.Migrate.Faa.Repository.Sql
{
    using System.Collections.Generic;
    using System.Linq;
    using Entities.Sql;
    using Infrastructure.Repositories.Sql.Common;

    public class SubVacancyRepository
    {
        private readonly IGetOpenConnection _getOpenConnection;

        public SubVacancyRepository(IGetOpenConnection getOpenConnection)
        {
            _getOpenConnection = getOpenConnection;
        }

        public IDictionary<int, SubVacancy> GetApplicationSummariesByIds(IEnumerable<int> applicationIds)
        {
            return _getOpenConnection.Query<SubVacancy>("SELECT * FROM SubVacancy WHERE AllocatedApplicationId in @applicationIds", new { applicationIds }).ToDictionary(a => a.AllocatedApplicationId, a => a);
        }
    }
}