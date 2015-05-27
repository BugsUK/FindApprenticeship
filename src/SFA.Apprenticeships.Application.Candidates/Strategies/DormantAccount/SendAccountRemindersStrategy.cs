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

            //Remind on the first cycle
            if (housekeepingCyclesSinceLastLogin == configuration.SendReminderAfterCycles)
            {
                SetAccountDormant(user, candidate, configuration);
                return true;
            }

            //Remind on the second cycle
            if (housekeepingCyclesSinceLastLogin == configuration.SendFinalReminderAfterCycles)
            {
                SetAccountDormant(user, candidate, configuration);
                return true;
            }

            return false;
        }

        protected void SetAccountDormant(User user, Candidate candidate, DormantAccountStrategy configuration)
        {
            user.Status = UserStatuses.Dormant;
            _userWriteRepository.Save(user);

            candidate.DisableAllOptionalCommunications();
            _candidateWriteRepository.Save(candidate);

            //TODO: Ensure successfull login removes dormancy

            var lastLogin = user.LastLogin ?? DateTime.UtcNow;
            var lastLoginInDays = (DateTime.UtcNow - lastLogin).Days;
            var lastLoginInDaysFormatted = lastLoginInDays >= configuration.SendFinalReminderAfterCycles ? "almost a year" : string.Format("{0} days", lastLoginInDays);

            _communicationService.SendMessageToCandidate(candidate.EntityId, MessageTypes.SendDormantAccountReminder,
                new[]
                {
                    new CommunicationToken(CommunicationTokens.CandidateFirstName, candidate.RegistrationDetails.FirstName),
                    new CommunicationToken(CommunicationTokens.LastLogin, lastLoginInDaysFormatted),
                    new CommunicationToken(CommunicationTokens.AccountExpiryDate, lastLogin.AddDays(configuration.SetPendingDeletionAfterCycles).ToLongDateString()),
                    new CommunicationToken(CommunicationTokens.Username, candidate.RegistrationDetails.EmailAddress)
                });
        }
    }
}