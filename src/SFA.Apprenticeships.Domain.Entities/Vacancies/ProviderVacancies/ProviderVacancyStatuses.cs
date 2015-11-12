namespace SFA.Apprenticeships.Domain.Entities.Vacancies.ProviderVacancies
{
    public enum ProviderVacancyStatuses
    {
        Unknown = 0,
        Draft = 1,
        PendingQA = 2,
        Live = 3,
        ReservedForQA = 4,
        RejectedByQA = 5
    }
}
