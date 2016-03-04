namespace SFA.Apprenticeships.Domain.Interfaces.Repositories
{
    public interface IReferenceNumberRepository
    {
        int GetNextVacancyReferenceNumber();

        int GetNextLegacyApplicationId();
    }
}
