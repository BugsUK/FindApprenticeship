namespace SFA.Apprenticeships.Infrastructure.Raa
{
    using Application.Vacancy;
    using Domain.Entities.Vacancies;

    public class VacancyDataProvider<TVacancyDetail> : IVacancyDataProvider<TVacancyDetail> where TVacancyDetail : VacancyDetail
    {
        public TVacancyDetail GetVacancyDetails(int vacancyId, bool errorIfNotFound = false)
        {
            throw new System.NotImplementedException();
        }
    }
}