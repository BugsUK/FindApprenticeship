namespace SFA.Apprenticeships.Domain.Raa.Interfaces.Repositories
{
    using Entities.Raa.Vacancies;

    public interface IVacancyWriteRepository
    {
        Vacancy Create(Vacancy vacancy);

        void Delete(int vacancyId);

        Vacancy ReserveVacancyForQA(int vacancyReferenceNumber);
        void UnReserveVacancyForQa(int vacancyReferenceNumber);

        void IncrementOfflineApplicationClickThrough(int vacancyId);

        Vacancy Update(Vacancy vacancy);
    }
}