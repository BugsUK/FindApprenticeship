// ReSharper disable InconsistentNaming
namespace SFA.Apprenticeships.Domain.Entities.Raa.Vacancies
{
    public enum VacancyStatus
    {
        Unknown = 0,
        Draft = 1,
        Live = 2,
        Referred = 3,
        Deleted = 4,
        Submitted = 5,
        Closed = 6,
        Withdrawn = 7,
        Completed = 8,
        PostedInError = 9,
        PendingQA = 10,
        ReservedForQA = 11,
        ParentVacancy = 12 //TODO probably could be removed
    }
}
