namespace SFA.Apprenticeships.Application.Candidates.Configuration
{
    public class HousekeepingConfiguration
    {
        public int HousekeepingCycleInHours { get; set; }
        public SendAccountReminderStrategyA SendAccountReminderStrategyA { get; set; }
        public SendAccountReminderStrategyB SendAccountReminderStrategyB { get; set; }
        public int SetPendingDeletionAfterCycles { get; set; }
        public int SendMobileVerificationCodeReminderAfterCycles { get; set; }
    }

    public class SendAccountReminderStrategyA
    {
        public int SendAccountReminderOneAfterCycles { get; set; }
        public int SendAccountReminderTwoAfterCycles { get; set; }
    }

    public class SendAccountReminderStrategyB
    {
        public int SendAccountReminderAfterCycles { get; set; }
        public int SendAccountReminderEveryCycles { get; set; }
    }
}