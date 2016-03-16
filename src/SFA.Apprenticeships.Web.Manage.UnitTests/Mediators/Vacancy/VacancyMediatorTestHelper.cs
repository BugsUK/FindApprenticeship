namespace SFA.Apprenticeships.Web.Manage.UnitTests.Mediators.Vacancy
{
    using System.Collections.Generic;
    using System.Linq;
    using Domain.Entities.Raa.Vacancies;
    using Raa.Common.ViewModels.Vacancy;

    public static class VacancyMediatorTestHelper
    {
        public static IEnumerable<DashboardVacancySummaryViewModel> GetPendingVacancies(IEnumerable<int> vacancyReferenceNumbers )
        {
            return vacancyReferenceNumbers.Select(vacancyReferenceNumber => new DashboardVacancySummaryViewModel
            {
                VacancyReferenceNumber = vacancyReferenceNumber,
                Status = VacancyStatus.Submitted
            });
        }
    }
}