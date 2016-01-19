namespace SFA.Apprenticeships.Infrastructure.Processes.Applications
{
    using System;
    using Application.Candidate;
    using SFA.Infrastructure.Interfaces;
    using Domain.Entities.Exceptions;
    using Domain.Interfaces.Messaging;
    using Domain.Interfaces.Repositories;
    using ErrorCodes = Application.Interfaces.Applications.ErrorCodes;

    public class SubmitTraineeshipApplicationRequestSubscriber : IServiceBusSubscriber<SubmitTraineeshipApplicationRequest>
    {
        private readonly ILogService _logger;
        private readonly ITraineeshipApplicationReadRepository _traineeshipApplicationReadRepository;
        private readonly ITraineeshipApplicationWriteRepository _traineeeshipApplicationWriteRepository;
        private readonly ICandidateReadRepository _candidateReadRepository;

        private readonly ILegacyApplicationProvider _legacyApplicationProvider;
        private readonly ILegacyCandidateProvider _legacyCandidateProvider;

        public SubmitTraineeshipApplicationRequestSubscriber(
            ILogService logger,
            ITraineeshipApplicationReadRepository traineeshipApplicationReadRepository,
            ITraineeshipApplicationWriteRepository traineeeshipApplicationWriteRepository,
            ICandidateReadRepository candidateReadRepository,
            ILegacyApplicationProvider legacyApplicationProvider,
            ILegacyCandidateProvider legacyCandidateProvider)
        {
            _legacyApplicationProvider = legacyApplicationProvider;
            _legacyCandidateProvider = legacyCandidateProvider;
            _traineeshipApplicationReadRepository = traineeshipApplicationReadRepository;
            _traineeeshipApplicationWriteRepository = traineeeshipApplicationWriteRepository;
            _candidateReadRepository = candidateReadRepository;
            _logger = logger;
        }

        [ServiceBusTopicSubscription(TopicName = "SubmitTraineeshipApplication")]
        public ServiceBusMessageStates Consume(SubmitTraineeshipApplicationRequest request)
        {
            return CreateApplication(request);
        }

        public ServiceBusMessageStates CreateApplication(SubmitTraineeshipApplicationRequest request)
        {
            _logger.Debug("Creating traineeship application Id: {0}", request.ApplicationId);

            var applicationDetail = _traineeshipApplicationReadRepository.Get(request.ApplicationId, true);

            try
            {
                var candidate = _candidateReadRepository.Get(applicationDetail.CandidateId, true);

                if (candidate.LegacyCandidateId == 0)
                {
                    _logger.Info("Candidate with Id: {0} has not been created in the legacy system. Message will be requeued",
                        applicationDetail.CandidateId);

                    return ServiceBusMessageStates.Requeue;
                }
                
                // Update candidate disability status to match the application.
                candidate.MonitoringInformation.DisabilityStatus = applicationDetail.CandidateInformation.DisabilityStatus;
                _legacyCandidateProvider.UpdateCandidate(candidate);

                applicationDetail.LegacyApplicationId = _legacyApplicationProvider.CreateApplication(applicationDetail);
                _traineeeshipApplicationWriteRepository.Save(applicationDetail);

                return ServiceBusMessageStates.Complete;
            }
            catch (CustomException e)
            {
                return HandleCustomException(request, e);
            }
            catch (Exception e)
            {
                _logger.Error("Submit traineeship application with Id = {0} request async process failed.",
                    e, request.ApplicationId);

                return ServiceBusMessageStates.Requeue;
            }
        }

        private ServiceBusMessageStates HandleCustomException(
            SubmitTraineeshipApplicationRequest request,
            CustomException e)
        {
            switch (e.Code)
            {
                case ErrorCodes.ApplicationDuplicatedError:
                    _logger.Warn("Traineeship application has already been submitted to legacy system: Application Id: \"{0}\"", request.ApplicationId);
                    break;

                case Application.Interfaces.Candidates.ErrorCodes.CandidateStateError:
                    _logger.Error("Legacy candidate is in an invalid state. Traineeship application cannot be processed: Application Id: \"{0}\"", request.ApplicationId);
                    break;

                case Application.Interfaces.Candidates.ErrorCodes.CandidateNotFoundError:
                    _logger.Error("Legacy candidate was not found. Traineeship application cannot be processed: Application Id: \"{0}\"", request.ApplicationId);
                    break;

                case Application.Interfaces.Vacancies.ErrorCodes.LegacyVacancyStateError:
                    _logger.Warn("Legacy Vacancy was in an invalid state. Traineeship application cannot be processed: Application Id: \"{0}\"", request.ApplicationId);
                    break;

                case Domain.Entities.ErrorCodes.EntityStateError:
                    _logger.Error(string.Format("Traineeship application is in an invalid state: Application Id: \"{0}\"", request.ApplicationId), e);
                    break;

                default:
                    _logger.Warn(string.Format("Submit traineeship application with Id = {0} request async process failed.", request.ApplicationId), e);
                    return ServiceBusMessageStates.Requeue;
            }

            return ServiceBusMessageStates.Complete;
        }
    }
}
