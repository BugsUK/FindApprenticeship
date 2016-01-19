namespace SFA.Apprenticeships.Application.Candidates.Strategies
{
    using SFA.Infrastructure.Interfaces;
    using Domain.Entities.Candidates;
    using Domain.Entities.Users;
    using Interfaces.Communications;

    public class MobileVerificationCodeReminderHousekeeping : IHousekeepingChainOfResponsibility
    {
        private readonly IHousekeepingStrategy _strategy;

        public MobileVerificationCodeReminderHousekeeping(ILogService logService, IConfigurationService configurationService, ICommunicationService communicationService)
        {
            var sendMobileVerificationCodeReminder = new SendMobileVerificationCodeReminderStrategy(logService, configurationService, communicationService);
            var terminatingHousekeepingStrategy = new TerminatingHousekeepingStrategy(configurationService);

            sendMobileVerificationCodeReminder.SetSuccessor(terminatingHousekeepingStrategy);

            _strategy = sendMobileVerificationCodeReminder;
        }

        public int Order
        {
            get { return 0; }
        }

        public void Handle(User user, Candidate candidate)
        {
            _strategy.Handle(user, candidate);
        }
    }
}