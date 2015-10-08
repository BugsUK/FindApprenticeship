namespace SFA.Apprenticeships.Domain.Interfaces.Repositories
{
    public interface IReferenceNumberRepository
    {
        long GetNextVacancyReferenceNumber();
    }
}
