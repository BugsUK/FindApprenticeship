using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFA.Apprenticeships.Domain.Raa.Interfaces.Repositories
{
    using Entities.Raa.Vacancies;
    using Models;

    public interface IVacancySummaryRepository
    {
        IList<VacancySummary> GetSummariesForProvider(VacancySummaryQuery query, out int totalRecords);
        VacancyCounts GetLotteryCounts(VacancySummaryQuery query);
    }
}
