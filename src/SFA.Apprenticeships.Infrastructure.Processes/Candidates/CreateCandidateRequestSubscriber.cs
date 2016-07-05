namespace SFA.Apprenticeships.Infrastructure.Processes.Candidates
{
    using System;
    using Application.Candidate;
    using Application.Interfaces;
    using SFA.Infrastructure.Interfaces;

    using Domain.Entities.Users;
    using Domain.Interfaces.Messaging;
    using Domain.Interfaces.Repositories;

    public class CreateCandidateRequestSubscriber : IServiceBusSubscriber<CreateCandidateRequest>
    {
        private readonly ILogService _logger;

        private readonly ICandidateReadRepository _candidateReadRepository;
        private readonly IUserReadRepository _userReadRepository;
        private readonly ICandidateWriteRepository _candidateWriteRepository;

        private readonly ILegacyCandidateProvider _legacyCandidateProvider;

        public CreateCandidateRequestSubscriber(
            ILogService logger,
            ICandidateReadRepository candidateReadRepository,
            IUserReadRepository userReadRepository,
            ICandidateWriteRepository candidateWriteRepository,
            ILegacyCandidateProvider legacyCandidateProvider)
        {
            _logger = logger;
            _candidateReadRepository = candidateReadRepository;
            _userReadRepository = userReadRepository;
            _candidateWriteRepository = candidateWriteRepository;
            _legacyCandidateProvider = legacyCandidateProvider;
        }

        [ServiceBusTopicSubscription(TopicName = "CreateCandidate")]
        public ServiceBusMessageStates Consume(CreateCandidateRequest request)
        {
            return CreateCandidate(request);
        }

        private ServiceBusMessageStates CreateCandidate(CreateCandidateRequest request)
        {
            try
            {
                _logger.Debug("Creating candidate Id: {0}", request.CandidateId);

                var user = _userReadRepository.Get(request.CandidateId);
                user.AssertState("Create legacy user", UserStatuses.Active, UserStatuses.Locked, UserStatuses.Dormant);

                var candidate = _candidateReadRepository.Get(request.CandidateId, true);

                if (candidate.LegacyCandidateId == 0)
                {
                    _logger.Info("Sending request to create candidate in legacy system: Candidate Id: \"{0}\"", request.CandidateId);

                    var legacyCandidateId = _legacyCandidateProvider.CreateCandidate(candidate);

                    candidate.LegacyCandidateId = legacyCandidateId;
                    _candidateWriteRepository.Save(candidate);

                    _logger.Info("Candidate created in legacy system: Candidate Id: \"{0}\", Legacy Candidate Id: \"{1}\"", request.CandidateId, legacyCandidateId);
                }
                else
                {
                    _logger.Warn("User has already been activated in legacy system: Candidate Id: \"{0}\"", request.CandidateId);
                }

                return ServiceBusMessageStates.Complete;
            }
            catch (Exception ex)
            {
                _logger.Error($"Create candidate with id {request.CandidateId} request async process failed", ex);

                return ServiceBusMessageStates.Requeue;
            }
        }
    }
}
