namespace SFA.Apprenticeships.Domain.Entities.Vacancies.ProviderVacancies
{
    public enum ProviderVacancyStatuses
    {
        Unknown = 0,        //DOESN'T EXIST IN DB
        Draft = 1,
        Live = 2,
        RejectedByQA = 3,
        Deleted = 4,        //NEW FROM DB
        Submitted = 5,      //NEW FROM DB
        Closed = 6,
        Withdrawn = 7,
        Completed = 8,
        PostedInError = 9,
        PendingQA = 10,     //DOESN'T EXIST IN DB
        ReservedForQA = 11, //DOESN'T EXIST IN DB
        ParentVacancy = 12  //DOESN'T EXIST IN DB
    }
}
