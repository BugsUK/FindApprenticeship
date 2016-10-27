namespace SFA.Apprenticeships.Data.Migrate.Faa.Subscribers
{
    using System;
    using System.Data.SqlClient;
    using Application.Interfaces;
    using Application.UserAccount.Entities;
    using Domain.Entities.Exceptions;
    using Domain.Interfaces.Messaging;

    public class CandidateUserUpdateSubscriber : IServiceBusSubscriber<CandidateUserUpdate>
    {
        private readonly ILogService _logService;
        private readonly ICandidateUserUpdater _candidateUserUpdater;

        public CandidateUserUpdateSubscriber(ICandidateUserUpdater candidateUserUpdater, ILogService logService)
        {
            _logService = logService;
            _candidateUserUpdater = candidateUserUpdater;
        }

        [ServiceBusTopicSubscription(TopicName = "CandidateUserUpdate")]
        public ServiceBusMessageStates Consume(CandidateUserUpdate request)
        {
            _logService.Debug($"Processing apprenticeship candidate update with id {request.CandidateGuid} and type {request.CandidateUserUpdateType}");

            try
            {
                switch (request.CandidateUserUpdateType)
                {
                    case CandidateUserUpdateType.Create:
                        _candidateUserUpdater.Create(request.CandidateGuid);
                        _logService.Debug($"Created apprenticeship candidate with id {request.CandidateGuid}");
                        break;
                    case CandidateUserUpdateType.Update:
                        _candidateUserUpdater.Update(request.CandidateGuid);
                        _logService.Debug($"Updated apprenticeship candidate with id {request.CandidateGuid}");
                        break;
                    case CandidateUserUpdateType.Delete:
                        _candidateUserUpdater.Delete(request.CandidateGuid);
                        _logService.Debug($"Deleted apprenticeship candidate with id {request.CandidateGuid}");
                        break;
                    default:
                        _logService.Warn($"Apprenticeship candidate update with id {request.CandidateGuid} was of an unknown or unsupported type {request.CandidateUserUpdateType}. Dead lettering message");
                        return ServiceBusMessageStates.DeadLetter;
                }

                return ServiceBusMessageStates.Complete;
            }
            catch (CustomException ex)
            {
                _logService.Warn($"Failed to process apprenticeship candidate update with id {request.CandidateGuid} and type {request.CandidateUserUpdateType}. Requeuing message", ex);
                return ServiceBusMessageStates.Requeue;
            }
            catch (SqlException ex)
            {
                _logService.Warn($"Failed to process apprenticeship candidate update with id {request.CandidateGuid} and type {request.CandidateUserUpdateType}. Requeuing message", ex);
                return ServiceBusMessageStates.Requeue;
            }
            catch (Exception ex)
            {
                _logService.Error($"Failed to process apprenticeship candidate update with id {request.CandidateGuid} and type {request.CandidateUserUpdateType}. Dead lettering message", ex);
                return ServiceBusMessageStates.DeadLetter;
            }
        }
    }
}