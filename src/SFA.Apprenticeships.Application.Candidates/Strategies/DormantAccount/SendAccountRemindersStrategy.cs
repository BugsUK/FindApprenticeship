namespace SFA.Apprenticeships.Application.Candidates.Strategies.DormantAccount
{
    using System;
    using SFA.Infrastructure.Interfaces;
    using Configuration;
    using Domain.Entities.Candidates;
    using Domain.Entities.Users;
    using Domain.Interfaces.Repositories;
    using Interfaces.Communications;

    public class SendAccountRemindersStrategy : HousekeepingStrategy
    {
        private readonly IUserWriteRepository _userWriteRepository;
        private readonly ICandidateWriteRepository _candidateWriteRepository;
        private readonly IAuditRepository _auditRepository;
        private readonly ICommunicationService _communicationService;
        private readonly ILogService _logService;

        public SendAccountRemindersStrategy(IConfigurationService configurationService, IUserWriteRepository userWriteRepository, ICandidateWriteRepository candidateWriteRepository, IAuditRepository auditRepository, ICommunicationService communicationService, ILogService logService)
            : base(configurationService)
        {
            _userWriteRepository = userWriteRepository;
            _candidateWriteRepository = candidateWriteRepository;
            _auditRepository = auditRepository;
            _communicationService = communicationService;
            _logService = logService;
        }

        protected override bool DoHandle(User user, Candidate candidate)
        {
            if (user == null || candidate == null) return false;

            if (user.Status != UserStatuses.Active && user.Status != UserStatuses.Locked && user.Status != UserStatuses.Dormant) return false;

            var lastLogin = user.GetLastLogin();

            var housekeepingCyclesSinceLastLogin = GetHousekeepingCyclesSince(lastLogin);

            var configuration = Configuration.DormantAccountStrategy;

            //Ensure accounts are set to dormant if cycle was missed
            if (user.Status != UserStatuses.Dormant && housekeepingCyclesSinceLastLogin >= configuration.SendReminderAfterCycles)
            {
                SetAccountDormant(user, candidate, lastLogin);
                SendAccountReminder(user, candidate, configuration, lastLogin);
                return true;
            }

            //Remind on the first cycle
            if (housekeepingCyclesSinceLastLogin == configuration.SendReminderAfterCycles)
            {
                SendAccountReminder(user, candidate, configuration, lastLogin);
                return true;
            }

            //Remind on the second cycle
            if (housekeepingCyclesSinceLastLogin == configuration.SendFinalReminderAfterCycles)
            {
                SendAccountReminder(user, candidate, configuration, lastLogin);
                return true;
            }

            return false;
        }

        protected void SetAccountDormant(User user, Candidate candidate, DateTime lastLogin)
        {
            _logService.Info("Setting User with Id: {0} to Dormant and disabling comms", user.EntityId);

            var candidateUser = new
            {
                User = user,
                Candidate = candidate
            };

            _auditRepository.Audit(candidateUser, AuditEventTypes.CandidateUserMakeDormant, user.EntityId);

            if (!user.LastLogin.HasValue)
            {
                user.LastLogin = lastLogin;
            }
            user.Status = UserStatuses.Dormant;
            _userWriteRepository.Save(user);

            candidate.DisableAllOptionalCommunications();
            _candidateWriteRepository.Save(candidate);

            _logService.Info("Set User with Id: {0} to Dormant and disabled comms", user.EntityId);
        }

        protected void SendAccountReminder(User user, Candidate candidate, DormantAccountStrategy configuration, DateTime lastLogin)
        {
            var lastLoginDelta = DateTime.UtcNow - lastLogin;
            var lastLoginInCycles = lastLoginDelta.TotalHours / Configuration.HousekeepingCycleInHours;
            var lastLoginInDays = lastLoginDelta.Days;
            var lastLoginInDaysFormatted = lastLoginInCycles >= configuration.SendFinalReminderAfterCycles ? "almost a year" : string.Format("{0} days", lastLoginInDays);

            var pendingDeletionAfterHours = configuration.SetPendingDeletionAfterCycles*
                                            Configuration.HousekeepingCycleInHours;

            var tomorrow = DateTime.UtcNow.AddDays(1);
            var accountExpiryDate = lastLogin.AddHours(pendingDeletionAfterHours);
            if (accountExpiryDate < tomorrow)
            {
                accountExpiryDate = tomorrow;
            }

            _communicationService.SendMessageToCandidate(candidate.EntityId, MessageTypes.SendDormantAccountReminder,
                new[]
                {
                    new CommunicationToken(CommunicationTokens.CandidateFirstName, candidate.RegistrationDetails.FirstName),
                    new CommunicationToken(CommunicationTokens.LastLogin, lastLoginInDaysFormatted),
                    new CommunicationToken(CommunicationTokens.AccountExpiryDate, accountExpiryDate.ToLongDateString()),
                    new CommunicationToken(CommunicationTokens.Username, candidate.RegistrationDetails.EmailAddress)
                });
        }
    }
}