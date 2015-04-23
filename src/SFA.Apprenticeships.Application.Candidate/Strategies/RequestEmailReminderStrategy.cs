namespace SFA.Apprenticeships.Application.Candidate.Strategies
{
    using Domain.Entities.Exceptions;
    using Domain.Interfaces.Repositories;
    using Interfaces.Communications;

    public class RequestEmailReminderStrategy : IRequestEmailReminderStrategy
    {
        private readonly ICandidateReadRepository _candidateReadRepository;
        private readonly ICommunicationService _communicationService;

        public RequestEmailReminderStrategy(ICandidateReadRepository candidateReadRepository, ICommunicationService communicationService)
        {
            _candidateReadRepository = candidateReadRepository;
            _communicationService = communicationService;
        }

        public void RequestEmailReminder(string phoneNumber)
        {
            var candidates = _candidateReadRepository.GetAllCandidatesWithPhoneNumber(phoneNumber);

            var verified = false;

            foreach (var candidate in candidates)
            {
                if (candidate.CommunicationPreferences.VerifiedMobile)
                {
                    verified = true;

                    _communicationService.SendMessageToCandidate(candidate.EntityId, MessageTypes.SendEmailReminder,
                    new[]
                    {
                        new CommunicationToken(CommunicationTokens.CandidateMobileNumber, phoneNumber),
                        new CommunicationToken(CommunicationTokens.UserEmailAddress, candidate.RegistrationDetails.EmailAddress)
                    });
                }
            }

            if (!verified)
            {
                throw new CustomException(Domain.Entities.ErrorCodes.EntityStateError);
            }
        }
    }
}