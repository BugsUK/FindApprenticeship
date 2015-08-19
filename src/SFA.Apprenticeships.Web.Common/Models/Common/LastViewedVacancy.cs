namespace SFA.Apprenticeships.Web.Common.Models.Common
{
    using Domain.Entities.Vacancies;

    public class LastViewedVacancy
    {
        public int Id { get; set; }
        public VacancyType Type { get; set; }
    }
}