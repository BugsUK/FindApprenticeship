namespace SFA.Apprenticeships.Domain.Raa.Interfaces.Repositories
{
    public interface IOfflineApprenticeshipVacancyRepository
    {
        void IncrementOfflineApplicationClickThrough(long vacancyReferenceNumber);
    }
}