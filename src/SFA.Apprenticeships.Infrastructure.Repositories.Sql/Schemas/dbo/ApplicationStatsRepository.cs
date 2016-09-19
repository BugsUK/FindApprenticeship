namespace SFA.Apprenticeships.Infrastructure.Repositories.Sql.Schemas.dbo
{
    using System.Collections.Generic;
    using System.Linq;
    using Common;
    using Domain.Entities.Applications;
    using Domain.Entities.Collections.Generic;
    using Domain.Interfaces.Repositories;

    public class ApplicationStatsRepository : IApprenticeshipApplicationStatsRepository, ITraineeshipApplicationStatsRepository
    {
        private readonly IGetOpenConnection _getOpenConnection;

        public ApplicationStatsRepository(IGetOpenConnection getOpenConnection)
        {
            _getOpenConnection = getOpenConnection;
        }

        IReadOnlyDictionary<int, IApplicationCounts> IApprenticeshipApplicationStatsRepository.GetCountsForVacancyIds(IEnumerable<int> vacancyIds)
        {
            return GetCounts(vacancyIds);
        }

        IReadOnlyDictionary<int, IApplicationCounts> ITraineeshipApplicationStatsRepository.GetCountsForVacancyIds(IEnumerable<int> vacancyIds)
        {
            return GetCounts(vacancyIds);
        }

        public IReadOnlyDictionary<int, IApplicationCounts> GetCounts(IEnumerable<int> vacancyIds)
        {
            //Implementation is common for SQL as all applications are stored in the same table
            var counts = new Dictionary<int, IApplicationCounts>();
            var splitVacancyIdsArray = DbHelpers.SplitIds(vacancyIds);
            foreach (var ids in splitVacancyIdsArray)
            {
                var sqlParams = new
                {
                    VacancyIds = ids
                };
                var allApplicationCounts = _getOpenConnection.Query<ApplicationCounts>("SELECT VacancyId, Count(*) as AllApplications FROM [Application] WHERE VacancyId IN @VacancyIds AND ApplicationStatusTypeId >= 2 GROUP BY VacancyId", sqlParams);
                var newApplicationCounts = _getOpenConnection.Query<ApplicationCounts>("SELECT VacancyId, Count(*) as NewApplications FROM [Application] WHERE VacancyId IN @VacancyIds AND ApplicationStatusTypeId = 2 GROUP BY VacancyId", sqlParams);

                foreach (var allApplicationCount in allApplicationCounts)
                {
                    counts[allApplicationCount.VacancyId] = allApplicationCount;
                }

                foreach (var newApplicationCount in newApplicationCounts)
                {
                    ((ApplicationCounts)counts[newApplicationCount.VacancyId]).NewApplications = newApplicationCount.NewApplications;
                }
            }

            return new SparseDictionary<int, IApplicationCounts>(counts, new ApplicationCounts());
        }

        private class ApplicationCounts : IApplicationCounts
        {
            public int VacancyId { get; set; }
            public int NewApplications { get; set; }
            public int AllApplications { get; set; }
        }
    }
}