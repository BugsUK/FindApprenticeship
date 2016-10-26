namespace SFA.Apprenticeships.Data.Migrate.Faa.Subscribers
{
    using System;
    using Application.Application.Entities;
    using Application.Candidate.Entities;
    using Application.Interfaces;
    using Domain.Entities.Exceptions;
    using Domain.Interfaces.Messaging;

    public class CandidateUpdateSubscriber : IServiceBusSubscriber<CandidateUpdate>
    {
        private readonly ILogService _logService;
        private readonly ICandidateUpdater _candidateUpdater;

        public CandidateUpdateSubscriber(ICandidateUpdater candidateUpdater, ILogService logService)
        {
            _logService = logService;
            _candidateUpdater = candidateUpdater;
        }

        [ServiceBusTopicSubscription(TopicName = "CandidateUpdate")]
        public ServiceBusMessageStates Consume(CandidateUpdate request)
        {
            _logService.Debug($"Processing apprenticeship candidate update with id {request.CandidateGuid} and type {request.CandidateUpdateType}");

            try
            {
                switch (request.CandidateUpdateType)
                {
                    case CandidateUpdateType.Create:
                        _candidateUpdater.Create(request.CandidateGuid);
                        _logService.Debug($"Created apprenticeship candidate with id {request.CandidateGuid}");
                        break;
                    case CandidateUpdateType.Update:
                        _candidateUpdater.Update(request.CandidateGuid);
                        _logService.Debug($"Updated apprenticeship candidate with id {request.CandidateGuid}");
                        break;
                    case CandidateUpdateType.Delete:
                        _candidateUpdater.Delete(request.CandidateGuid);
                        _logService.Debug($"Deleted apprenticeship candidate with id {request.CandidateGuid}");
                        break;
                    default:
                        _logService.Warn($"Apprenticeship candidate update with id {request.CandidateGuid} was of an unknown or unsupported type {request.CandidateUpdateType}. Dead lettering message");
                        return ServiceBusMessageStates.DeadLetter;
                }

                return ServiceBusMessageStates.Complete;
            }
            catch (CustomException ex)
            {
                _logService.Error($"Failed to process apprenticeship candidate update with id {request.CandidateGuid} and type {request.CandidateUpdateType}. Requeuing message", ex);
                return ServiceBusMessageStates.Requeue;
            }
            catch (Exception ex)
            {
                _logService.Error($"Failed to process apprenticeship candidate update with id {request.CandidateGuid} and type {request.CandidateUpdateType}. Dead lettering message", ex);
                return ServiceBusMessageStates.DeadLetter;
            }
        }
    }
}