namespace SFA.Apprenticeships.Infrastructure.Processes.Candidates
{
    using System;
    using Application.Candidate;
    using Application.Interfaces.Logging;
    using Domain.Entities.Exceptions;
    using Domain.Interfaces.Messaging;
    using Domain.Interfaces.Repositories;

    public class SaveCandidateRequestSubscriber : IServiceBusSubscriber<SaveCandidateRequest>
    {
        private readonly ILogService _logService;
        private readonly ICandidateReadRepository _candidateReadRepository;
        private readonly ILegacyCandidateProvider _legacyCandidateProvider;

        public SaveCandidateRequestSubscriber(
            ILogService logService,
            ICandidateReadRepository candidateReadRepository,
            ILegacyCandidateProvider legacyCandidateProvider)
        {
            _logService = logService;
            _candidateReadRepository = candidateReadRepository;
            _legacyCandidateProvider = legacyCandidateProvider;
        }

        [ServiceBusTopicSubscription(TopicName = "save-candidate-request")]
        public ServiceBusMessageResult Consume(SaveCandidateRequest message)
        {
            _logService.Info("SaveCandidateRequest: {0}", message.CandidateId);

            return SaveCandidate(message);
        }

        private ServiceBusMessageResult SaveCandidate(SaveCandidateRequest request)
        {
            try
            {
                var candidate = _candidateReadRepository.Get(request.CandidateId);

                _legacyCandidateProvider.UpdateCandidate(candidate);

                return ServiceBusMessageResult.Complete();
            }
            catch (CustomException ex)
            {
                return HandleCustomException(request, ex);
            }
            catch (Exception ex)
            {
                _logService.Error(
                    string.Format("Save candidate with Id = {0} request async process failed.", request.CandidateId), ex);
                
                return Requeue(request);
            }
        }

        private ServiceBusMessageResult HandleCustomException(SaveCandidateRequest request, CustomException ex)
        {
            switch (ex.Code)
            {
                case Application.Interfaces.Candidates.ErrorCodes.CandidateNotFoundError:
                    // TODO: This can happen when a user requests that their account should be deleted. We need to work out what to do in that case. Probably set the candidate's status to inactive and the application's status to draft.
                    _logService.Error("Legacy candidate was not found. Update candidate cannot be processed: Candidate Id: \"{0}\"", request.CandidateId);
                    break;
                case Application.Interfaces.Candidates.ErrorCodes.CandidateStateError:
                    _logService.Error("Legacy candidate is in an invalid state. Update candidate cannot be processed: Candidate Id: \"{0}\"", request.CandidateId);
                    break;
                default:
                    _logService.Warn(string.Format("Save/Update candidate with Id = {0} request async process failed. Queuing for retry.", request.CandidateId), ex);
                    return Requeue(request);
            }

            return ServiceBusMessageResult.Complete();
        }

        private ServiceBusMessageResult Requeue(SaveCandidateRequest request)
        {
            request.ProcessTime = request.ProcessTime.HasValue ? DateTime.UtcNow.AddMinutes(5) : DateTime.UtcNow.AddSeconds(30);

            return ServiceBusMessageResult.Requeue(request.ProcessTime.Value);
        }
    }
}
