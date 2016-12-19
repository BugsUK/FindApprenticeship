namespace SFA.Apprenticeships.Web.Raa.Common.ViewModels.VacancyManagement
{
    using Domain.Entities.Raa.Vacancies;
    using Vacancy;

    public class EditWageViewModel : WageUpdate
    {
        public int VacancyReferenceNumber { get; set; }

        public VacancyApplicationsState VacancyApplicationsState { get; set; }
    }
}