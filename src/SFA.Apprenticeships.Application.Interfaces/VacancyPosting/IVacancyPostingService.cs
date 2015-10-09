namespace SFA.Apprenticeships.Application.Interfaces.VacancyPosting
{
    using Domain.Entities.Vacancies.ProviderVacancies.Apprenticeship;

    public interface IVacancyPostingService
    {
        ApprenticeshipVacancy SaveApprenticeshipVacancy(ApprenticeshipVacancy vacancy);

        long GetNextVacancyReferenceNumber();

        ApprenticeshipVacancy GetVacancy(long vacancyReferenceNumber);
    }
}
