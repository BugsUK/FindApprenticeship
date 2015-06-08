namespace SFA.Apprenticeships.Application.Candidates.Configuration
{
    public class HousekeepingConfiguration
    {
        public int HousekeepingCycleInHours { get; set; }
        
        public ActivationReminderStrategy ActivationReminderStrategy { get; set; }
        
        public int SendMobileVerificationCodeReminderAfterCycles { get; set; }
        
        public DormantAccountStrategy DormantAccountStrategy { get; set; }

        public int HardDeleteAccountAfterCycles { get; set; }

        public ApplicationHousekeepingConfiguration Application { get; set; }

        public CommunicationHousekeepingConfiguration Communication { get; set; }
    }

    public class ApplicationHousekeepingConfiguration
    {
        public int HardDeleteDraftApplicationForExpiredVacancyAfterCycles { get; set; }
        public int HardDeleteSubmittedApplicationAfterCycles { get; set; }
    }

    public class CommunicationHousekeepingConfiguration
    {
        public int HardDeleteSavedSearchAlertAfterCycles { get; set; }

        public int HardDeleteExpiringDraftApplicationAlertAfterCycles { get; set; }

        public int HardDeleteApplicationStatusAlertAfterCycles { get; set; }
    }

    public class ActivationReminderStrategy
    {
        public SendAccountReminderStrategyA SendAccountReminderStrategyA { get; set; }
        public SendAccountReminderStrategyB SendAccountReminderStrategyB { get; set; }
        public int SetPendingDeletionAfterCycles { get; set; }
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

    public class DormantAccountStrategy
    {
        public int SendReminderAfterCycles { get; set; }
        public int SendFinalReminderAfterCycles { get; set; }
        public int SetPendingDeletionAfterCycles { get; set; }
    }
}