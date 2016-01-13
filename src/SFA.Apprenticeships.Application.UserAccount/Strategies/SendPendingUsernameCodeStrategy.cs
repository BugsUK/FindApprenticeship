namespace SFA.Apprenticeships.Application.UserAccount.Strategies
{
    using System;
    using Domain.Entities.Candidates;
    using Domain.Entities.Users;
    using Domain.Interfaces.Repositories;
    using Interfaces.Communications;
    using SFA.Infrastructure.Interfaces;
    using Interfaces.Users;

    public class SendPendingUsernameCodeStrategy : ISendPendingUsernameCodeStrategy
    {
        private readonly ILogService _logger;

        private readonly ICandidateReadRepository _candidateReadRepository;
        private readonly ICodeGenerator _codeGenerator;
        private readonly ICommunicationService _communicationService;
        private readonly IUserReadRepository _userReadRepository;

        public SendPendingUsernameCodeStrategy(
            ILogService logger,
            ICommunicationService communicationService,
            ICodeGenerator codeGenerator,
            IUserReadRepository userReadRepository,
            ICandidateReadRepository candidateReadRepository)
        {
            _logger = logger;
            _communicationService = communicationService;
            _codeGenerator = codeGenerator;
            _userReadRepository = userReadRepository;
            _candidateReadRepository = candidateReadRepository;
        }        

        public void SendPendingUsernameCode(Guid userId)
        {
            var user = _userReadRepository.Get(userId);

            if (user == null)
            {
                _logger.Info(string.Format("Cannot send pending username code, userid not found: \"{0}\".", userId));
                return;
            }

            var candidate = _candidateReadRepository.Get(user.EntityId);

            SendPendingUsernameCodeViaCommunicationService(user, candidate);
        }

        #region Helpers

        private void SendPendingUsernameCodeViaCommunicationService(User user, Candidate candidate)
        {
            // Pending username code never expires: if user has one, use it.
            var pendingUsernameCode = string.IsNullOrEmpty(user.PendingUsernameCode)
                ? _codeGenerator.GenerateAlphaNumeric()
                : user.PendingUsernameCode;

            _communicationService.SendMessageToCandidate(candidate.EntityId, MessageTypes.SendPendingUsernameCode,
                new[]
                {
                    new CommunicationToken(CommunicationTokens.CandidateFirstName, candidate.RegistrationDetails.FirstName),
                    new CommunicationToken(CommunicationTokens.UserPendingUsername, user.PendingUsername),
                    new CommunicationToken(CommunicationTokens.UserPendingUsernameCode, pendingUsernameCode)
                });
        }

        #endregion
    }
}