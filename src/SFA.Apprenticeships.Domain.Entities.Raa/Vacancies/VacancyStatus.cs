// ReSharper disable InconsistentNaming
namespace SFA.Apprenticeships.Domain.Entities.Raa.Vacancies
{
    //TODO: consider augmenting this into some sort of struct.
    //There are actions related to state, that are being constrained in the presentation layer
    //and need to be lower down, so as to tighten this up. Argument in favour of a less anaemic domain.
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
        ReservedForQA = 10 
    }
}
