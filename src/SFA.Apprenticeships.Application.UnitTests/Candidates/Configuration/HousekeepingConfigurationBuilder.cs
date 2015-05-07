namespace SFA.Apprenticeships.Application.UnitTests.Candidates.Configuration
{
    using Application.Candidates.Configuration;

    public class HousekeepingConfigurationBuilder
    {
        private int _housekeepingCycleInHours = 24;
        private int _sendAccountReminderAfterCycles = 1;
        private int _sendAccountReminderEveryCycles = 7;
        private int _setPendingDeletionAfterCycles = 31;

        public HousekeepingConfiguration Build()
        {
            var configuration = new HousekeepingConfiguration
            {
                HousekeepingCycleInHours = _housekeepingCycleInHours,
                SendAccountReminderAfterCycles = _sendAccountReminderAfterCycles,
                SendAccountReminderEveryCycles = _sendAccountReminderEveryCycles,
                SetPendingDeletionAfterCycles = _setPendingDeletionAfterCycles
            };

            return configuration;
        }

        public HousekeepingConfigurationBuilder With(int housekeepingCycleInHours, int sendAccountReminderAfterCycles, int sendAccountReminderEveryCycles, int setPendingDeletionAfterCycles)
        {
            _housekeepingCycleInHours = housekeepingCycleInHours;
            _sendAccountReminderAfterCycles = sendAccountReminderAfterCycles;
            _sendAccountReminderEveryCycles = sendAccountReminderEveryCycles;
            _setPendingDeletionAfterCycles = setPendingDeletionAfterCycles;

            return this;
        }
    }
}