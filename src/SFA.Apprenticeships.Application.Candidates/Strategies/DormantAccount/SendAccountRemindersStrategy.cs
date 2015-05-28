namespace SFA.Apprenticeships.Application.Candidates.Strategies.DormantAccount
{
    using System;
    using Configuration;
    using Domain.Entities.Candidates;
    using Domain.Entities.Users;
    using Domain.Interfaces.Configuration;
    using Domain.Interfaces.Repositories;
    using Interfaces.Communications;
    using Interfaces.Logging;

    public class SendAccountRemindersStrategy : HousekeepingStrategy
    {
        private readonly IUserWriteRepository _userWriteRepository;
        private readonly ICandidateWriteRepository _candidateWriteRepository;
        private readonly ICommunicationService _communicationService;
        private readonly ILogService _logService;

        public SendAccountRemindersStrategy(IConfigurationService configurationService, IUserWriteRepository userWriteRepository, ICandidateWriteRepository candidateWriteRepository, ICommunicationService communicationService, ILogService logService) : base(configurationService)
        {
            _userWriteRepository = userWriteRepository;
            _candidateWriteRepository = candidateWriteRepository;
            _communicationService = communicationService;
            _logService = logService;
        }

        protected override bool DoHandle(User user, Candidate candidate)
        {
            if (user.Status != UserStatuses.Active && user.Status != UserStatuses.Locked && user.Status != UserStatuses.Dormant) return false;

            var housekeepingCyclesSinceLastLogin = GetHousekeepingCyclesSince(user.GetLastLogin());

            var configuration = Configuration.DormantAccountStrategy;

            //Ensure accounts are set to dormant if cycle was missed
            if (user.Status != UserStatuses.Dormant && housekeepingCyclesSinceLastLogin >= configuration.SendReminderAfterCycles)
            {
                SetAccountDormant(user, candidate);
            }

            //Remind on the first cycle
            if (housekeepingCyclesSinceLastLogin == configuration.SendReminderAfterCycles)
            {
                SendAccountReminder(user, candidate, configuration);
                return true;
            }

            //Remind on the second cycle
            if (housekeepingCyclesSinceLastLogin == configuration.SendFinalReminderAfterCycles)
            {
                SendAccountReminder(user, candidate, configuration);
                return true;
            }

            return false;
        }

        protected void SetAccountDormant(User user, Candidate candidate)
        {
            user.Status = UserStatuses.Dormant;
            _userWriteRepository.Save(user);

            candidate.DisableAllOptionalCommunications();
            _candidateWriteRepository.Save(candidate);
        }

        protected void SendAccountReminder(User user, Candidate candidate, DormantAccountStrategy configuration)
        {
            var lastLogin = user.GetLastLogin();
            var lastLoginDelta = DateTime.UtcNow - lastLogin;
            var lastLoginInCycles = lastLoginDelta.TotalHours / Configuration.HousekeepingCycleInHours;
            var lastLoginInDays = lastLoginDelta.Days;
            var lastLoginInDaysFormatted = lastLoginInCycles >= configuration.SendFinalReminderAfterCycles ? "almost a year" : string.Format("{0} days", lastLoginInDays);

            var pendingDeletionAfterHours = configuration.SetPendingDeletionAfterCycles*
                                            Configuration.HousekeepingCycleInHours;

            _communicationService.SendMessageToCandidate(candidate.EntityId, MessageTypes.SendDormantAccountReminder,
                new[]
                {
                    new CommunicationToken(CommunicationTokens.CandidateFirstName, candidate.RegistrationDetails.FirstName),
                    new CommunicationToken(CommunicationTokens.LastLogin, lastLoginInDaysFormatted),
                    new CommunicationToken(CommunicationTokens.AccountExpiryDate, lastLogin.AddHours(pendingDeletionAfterHours).ToLongDateString()),
                    new CommunicationToken(CommunicationTokens.Username, candidate.RegistrationDetails.EmailAddress)
                });
        }
    }
}