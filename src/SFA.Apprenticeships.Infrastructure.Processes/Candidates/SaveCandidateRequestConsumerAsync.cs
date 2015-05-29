namespace SFA.Apprenticeships.Infrastructure.Processes.Candidates
{
    using System;
    using System.Threading.Tasks;
    using Application.Candidate;
    using Application.Interfaces.Logging;
    using Domain.Entities.Exceptions;
    using Domain.Interfaces.Messaging;
    using Domain.Interfaces.Repositories;
    using EasyNetQ.AutoSubscribe;

    public class SaveCandidateRequestConsumerAsync : IConsumeAsync<SaveCandidateRequest>
    {
        private readonly IMessageBus _messageBus;
        private readonly ICandidateReadRepository _candidateReadRepository;
        private readonly ILegacyCandidateProvider _legacyCandidateProvider;
        private readonly ILogService _logger;

        public SaveCandidateRequestConsumerAsync(IMessageBus messageBus, ICandidateReadRepository candidateReadRepository, ILegacyCandidateProvider legacyCandidateProvider, ILogService logger)
        {
            _messageBus = messageBus;
            _candidateReadRepository = candidateReadRepository;
            _legacyCandidateProvider = legacyCandidateProvider;
            _logger = logger;
        }

        [SubscriptionConfiguration(PrefetchCount = 2)]
        [AutoSubscriberConsumer(SubscriptionId = "SaveCandidateRequestConsumerAsync")]
        public Task Consume(SaveCandidateRequest request)
        {
            return Task.Run(() =>
            {
                if (request.ProcessTime.HasValue && request.ProcessTime > DateTime.Now)
                {
                    try
                    {
                        _messageBus.PublishMessage(request);
                        return;
                    }
                    catch
                    {
                        _logger.Error("Failed to re-queue deferred 'Create Candidate' request: {{ 'CandidateId': '{0}' }}", request.CandidateId);
                        throw;
                    }
                }

                SaveCandidate(request);
            });
        }

        private void SaveCandidate(SaveCandidateRequest request)
        {
            try
            {
                var candidate = _candidateReadRepository.Get(request.CandidateId);
                _legacyCandidateProvider.UpdateCandidate(candidate);
            }
            catch (CustomException ex)
            {
                HandleCustomException(request, ex);
            }
            catch (Exception ex)
            {
                _logger.Error(
                    string.Format("Save candidate with Id = {0} request async process failed.", request.CandidateId), ex);
                Requeue(request);
            }
        }

        private void HandleCustomException(SaveCandidateRequest request, CustomException ex)
        {
            switch (ex.Code)
            {
                case Application.Interfaces.Candidates.ErrorCodes.CandidateNotFoundError:
                    //TODO: This can happen when a user requests that their account should be deleted. We need to work out what to do in that case. Probably set the candidate's status to inactive and the application's status to draft
                    _logger.Error("Legacy candidate was not found. Update candidate cannot be processed: Candidate Id: \"{0}\"", request.CandidateId);
                    break;
                case Application.Interfaces.Candidates.ErrorCodes.CandidateStateError:
                    _logger.Error("Legacy candidate is in an invalid state. Update candidate cannot be processed: Candidate Id: \"{0}\"", request.CandidateId);
                    break;
                default:
                    _logger.Warn(string.Format("Save/Update candidate with Id = {0} request async process failed. Queuing for retry.", request.CandidateId), ex);
                    Requeue(request);
                    break;
            }
        }

        private void Requeue(SaveCandidateRequest request)
        {
            request.ProcessTime = request.ProcessTime.HasValue ? DateTime.Now.AddMinutes(5) : DateTime.Now.AddSeconds(30);
            _messageBus.PublishMessage(request);
        }
    }
}