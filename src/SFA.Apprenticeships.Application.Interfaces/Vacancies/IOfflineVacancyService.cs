namespace SFA.Apprenticeships.Application.Interfaces.Vacancies
{
    public interface IOfflineVacancyService
    {
        void IncrementOfflineApplicationClickThrough(long vacancyReferenceNumber);
    }
}