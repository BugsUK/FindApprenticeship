using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFA.Apprenticeships.Application.Vacancy
{
    using Domain.Entities.Raa.Vacancies;
    using Domain.Raa.Interfaces.Repositories.Models;

    public interface IVacancySummaryService
    {
        IList<VacancySummary> GetSummariesForProvider(VacancySummaryQuery query, out int totalRecords);
        VacancyCounts GetLotteryCounts(VacancySummaryQuery query);
        IList<VacancySummary> GetWithStatus(VacancySummaryByStatusQuery query, out int totalRecords);
        IList<RegionalTeamMetrics> GetRegionalTeamMetrics(VacancySummaryByStatusQuery query);
        VacancySummary GetById(int vacancyId);
        IList<VacancySummary> GetByIds(IEnumerable<int> vacancyIds);
    }
}
