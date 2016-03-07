namespace SFA.Apprenticeships.Application.Interfaces.Vacancies
{
    public interface ICandidateVacancyService
    {
        void IncrementOfflineApplicationClickThrough(int vacancyReferenceNumber);
    }
}