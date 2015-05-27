namespace SFA.Apprenticeships.Application.UnitTests.Candidates.Configuration
{
    using Application.Candidates.Configuration;

    public class HousekeepingConfigurationBuilder
    {
        private readonly HousekeepingConfiguration _configuration = new HousekeepingConfiguration
        {
            HousekeepingCycleInHours = 24,
            ActivationReminderStrategy = new ActivationReminderStrategy
            {
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
            },
            SendMobileVerificationCodeReminderAfterCycles = 1,
            DormantAccountStrategy = new DormantAccountStrategy
            {
                SendReminderAfterCycles = 90,
                SendFinalReminderAfterCycles = 330,
                SetPendingDeletionAfterCycles = 365
            },
            HardDeleteAccountAfterCycles = 14
        };

        public HousekeepingConfiguration Build()
        {
            return _configuration;
        }

        public HousekeepingConfigurationBuilder WithStrategyA(int housekeepingCycleInHours, int sendAccountReminderOneAfterCycles, int sendAccountReminderTwoAfterCycles, int setPendingDeletionAfterCycles)
        {
            _configuration.HousekeepingCycleInHours = housekeepingCycleInHours;
            _configuration.ActivationReminderStrategy.SendAccountReminderStrategyA.SendAccountReminderOneAfterCycles = sendAccountReminderOneAfterCycles;
            _configuration.ActivationReminderStrategy.SendAccountReminderStrategyA.SendAccountReminderTwoAfterCycles = sendAccountReminderTwoAfterCycles;
            _configuration.ActivationReminderStrategy.SetPendingDeletionAfterCycles = setPendingDeletionAfterCycles;

            return this;
        }

        public HousekeepingConfigurationBuilder WithStrategyB(int housekeepingCycleInHours, int sendAccountReminderAfterCycles, int sendAccountReminderEveryCycles, int setPendingDeletionAfterCycles)
        {
            _configuration.HousekeepingCycleInHours = housekeepingCycleInHours;
            _configuration.ActivationReminderStrategy.SendAccountReminderStrategyB.SendAccountReminderAfterCycles = sendAccountReminderAfterCycles;
            _configuration.ActivationReminderStrategy.SendAccountReminderStrategyB.SendAccountReminderEveryCycles = sendAccountReminderEveryCycles;
            _configuration.ActivationReminderStrategy.SetPendingDeletionAfterCycles = setPendingDeletionAfterCycles;

            return this;
        }

        public HousekeepingConfigurationBuilder SendMobileVerificationCodeReminderStrategy(int housekeepingCycleInHours, int sendMobileVerificationCodeReminderAfterCycles)
        {
            _configuration.HousekeepingCycleInHours = housekeepingCycleInHours;
            _configuration.SendMobileVerificationCodeReminderAfterCycles = sendMobileVerificationCodeReminderAfterCycles;

            return this;
        }
    }
}