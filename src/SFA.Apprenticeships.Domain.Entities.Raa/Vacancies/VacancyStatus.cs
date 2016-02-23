// ReSharper disable InconsistentNaming
namespace SFA.Apprenticeships.Domain.Entities.Raa.Vacancies
{
    public enum VacancyStatus
    {
        Unknown = 0,
        Draft = 1,
        PendingQA = 2,
        Live = 3,
        ReservedForQA = 4,
        RejectedByQA = 5,
        Closed = 6,
        Completed = 7,
        Withdrawn = 8,
        ParentVacancy = 9
    }
}
