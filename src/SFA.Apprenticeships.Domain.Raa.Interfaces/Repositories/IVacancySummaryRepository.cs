using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFA.Apprenticeships.Domain.Raa.Interfaces.Repositories
{
    using Entities.Raa.Vacancies;
    using Models;
    using Queries;

    public interface IVacancySummaryRepository
    {
        IList<VacancySummary> GetSummariesForProvider(VacancySummaryQuery query, out int totalRecords);
        VacancyCounts GetLotteryCounts(VacancySummaryQuery query);
        IList<VacancySummary> GetByStatus(VacancySummaryByStatusQuery query, out int totalRecords);
        IList<RegionalTeamMetrics> GetRegionalTeamMetrics(VacancySummaryByStatusQuery query);
        VacancySummary GetById(int vacancyId);
        VacancySummary GetByReferenceNumber(int vacancyReferenceNumber);
        List<VacancySummary> GetByIds(IEnumerable<int> vacancyId);
        IList<VacancySummary> Find(ApprenticeshipVacancyQuery query, out int totalRecords);
    }
}
