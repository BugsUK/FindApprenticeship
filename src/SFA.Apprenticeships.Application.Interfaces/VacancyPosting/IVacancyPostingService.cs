namespace SFA.Apprenticeships.Application.Interfaces.VacancyPosting
{
    using Domain.Entities.Vacancies.ProviderVacancies.Apprenticeship;

    public interface IVacancyPostingService
    {
        ApprenticeshipVacancy CreateApprenticeshipVacancy(ApprenticeshipVacancy vacancy);
    }
}
