namespace SFA.Apprenticeships.Domain.Interfaces.Repositories
{
    public interface IOfflineApprenticeshipVacancyRepository
    {
        void IncrementOfflineApplicationClickThrough(int vacancyReferenceNumber);
    }
}