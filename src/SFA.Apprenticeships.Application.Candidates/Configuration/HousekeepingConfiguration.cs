namespace SFA.Apprenticeships.Application.Candidates.Configuration
{
    public class HousekeepingConfiguration
    {
        public int HousekeepingCycleInHours { get; set; }
        public int SendAccountReminderAfterCycles { get; set; }
        public int SendAccountReminderEveryCycles { get; set; }
        public int SetPendingDeletionAfterCycles { get; set; }
    }
}