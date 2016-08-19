namespace SFA.Apprenticeships.Data.Migrate.Faa.Entities.Mongo
{
    public enum ApplicationStatuses
    {
        Saved = 5,
        Draft = 10,
        ExpiredOrWithdrawn = 15,
        Submitting = 20,
        Submitted = 30,
        InProgress = 40,
        Successful = 80,
        Unsuccessful = 90
    }
}
