namespace SFA.Apprenticeships.Application.UnitTests.Candidates.Configuration
{
    using Application.Candidates.Configuration;

    public class HousekeepingConfigurationBuilder
    {
        private readonly HousekeepingConfiguration _configuration = new HousekeepingConfiguration
        {
            HousekeepingCycleInHours = 24,
            SendAccountReminderStrategyA = new SendAccountReminderStrategyA
            {
                SendAccountReminderOneAfterCycles = 7,
                SendAccountReminderTwoAfterCycles = 21
            },
            SendAccountReminderStrategyB = new SendAccountReminderStrategyB
            {
                SendAccountReminderAfterCycles = 1,
                SendAccountReminderEveryCycles = 7
            },
            SetPendingDeletionAfterCycles = 31
        };

        public HousekeepingConfiguration Build()
        {
            return _configuration;
        }

        public HousekeepingConfigurationBuilder WithStrategyA(int housekeepingCycleInHours, int sendAccountReminderOneAfterCycles, int sendAccountReminderTwoAfterCycles, int setPendingDeletionAfterCycles)
        {
            _configuration.HousekeepingCycleInHours = housekeepingCycleInHours;
            _configuration.SendAccountReminderStrategyA.SendAccountReminderOneAfterCycles = sendAccountReminderOneAfterCycles;
            _configuration.SendAccountReminderStrategyA.SendAccountReminderTwoAfterCycles = sendAccountReminderTwoAfterCycles;
            _configuration.SetPendingDeletionAfterCycles = setPendingDeletionAfterCycles;

            return this;
        }

        public HousekeepingConfigurationBuilder WithStrategyB(int housekeepingCycleInHours, int sendAccountReminderAfterCycles, int sendAccountReminderEveryCycles, int setPendingDeletionAfterCycles)
        {
            _configuration.HousekeepingCycleInHours = housekeepingCycleInHours;
            _configuration.SendAccountReminderStrategyB.SendAccountReminderAfterCycles = sendAccountReminderAfterCycles;
            _configuration.SendAccountReminderStrategyB.SendAccountReminderEveryCycles = sendAccountReminderEveryCycles;
            _configuration.SetPendingDeletionAfterCycles = setPendingDeletionAfterCycles;

            return this;
        }
    }
}