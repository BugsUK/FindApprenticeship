namespace SFA.Apprenticeships.Application.Candidates.Configuration
{
    public class HousekeepingConfiguration
    {
        public int SendAccountReminderAfterHours { get; set; }
        public int SendAccountReminderEveryHours { get; set; }
        public int SetPendingDeletionAfterHours { get; set; }
    }
}