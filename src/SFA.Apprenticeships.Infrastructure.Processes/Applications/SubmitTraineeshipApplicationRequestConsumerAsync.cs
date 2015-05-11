namespace SFA.Apprenticeships.Infrastructure.Processes.Applications
{
    using System;
    using System.Threading.Tasks;
    using Application.Candidate;
    using Application.Interfaces.Logging;
    using Domain.Entities.Exceptions;
    using Domain.Interfaces.Messaging;
    using Domain.Interfaces.Repositories;
    using EasyNetQ.AutoSubscribe;
    using ErrorCodes = Application.Interfaces.Applications.ErrorCodes;

    public class SubmitTraineeshipApplicationRequestConsumerAsync : IConsumeAsync<SubmitTraineeshipApplicationRequest>
    {
        private readonly ILogService _logger;
        private readonly ILegacyApplicationProvider _legacyApplicationProvider;
        private readonly ILegacyCandidateProvider _legacyCandidateProvider;
        private readonly ITraineeshipApplicationReadRepository _apprenticeshipApplicationReadRepository;
        private readonly ITraineeshipApplicationWriteRepository _traineeeshipApplicationWriteRepository;
        private readonly ICandidateReadRepository _candidateReadRepository;
        private readonly IMessageBus _messageBus;

        public SubmitTraineeshipApplicationRequestConsumerAsync(
            ILegacyApplicationProvider legacyApplicationProvider,
            ILegacyCandidateProvider legacyCandidateProvider,
            ITraineeshipApplicationReadRepository apprenticeshipApplicationReadRepository,
            ITraineeshipApplicationWriteRepository traineeeshipApplicationWriteRepository,
            ICandidateReadRepository candidateReadRepository,
            IMessageBus messageBus,
            ILogService logger)
        {
            _legacyApplicationProvider = legacyApplicationProvider;
            _legacyCandidateProvider = legacyCandidateProvider;
            _apprenticeshipApplicationReadRepository = apprenticeshipApplicationReadRepository;
            _traineeeshipApplicationWriteRepository = traineeeshipApplicationWriteRepository;
            _candidateReadRepository = candidateReadRepository;
            _messageBus = messageBus;
            _logger = logger;
        }

        [SubscriptionConfiguration(PrefetchCount = 2)]
        [AutoSubscriberConsumer(SubscriptionId = "SubmitTraineeshipApplicationRequestConsumerAsync")]
        public Task Consume(SubmitTraineeshipApplicationRequest request)
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
                        _logger.Error("Failed to re-queue deferred 'Submit Traineeship Application' request: {{ 'ApplicationId': '{0}' }}", request.ApplicationId);
                        throw;
                    }
                }

                CreateApplication(request);
            });
        }

        public void CreateApplication(SubmitTraineeshipApplicationRequest request)
        {
            _logger.Debug("Creating traineeship application Id: {0}", request.ApplicationId);

            var applicationDetail = _apprenticeshipApplicationReadRepository.Get(request.ApplicationId, true);

            try
            {
                var candidate = _candidateReadRepository.Get(applicationDetail.CandidateId, true);

                if (candidate.LegacyCandidateId == 0)
                {
                    _logger.Info(
                        "Candidate with Id: {0} has not been created in the legacy system. Message will be requeued",
                        applicationDetail.CandidateId);
                    Requeue(request);
                }
                else
                {
                    // Update candidate disability status to match the application.
                    candidate.MonitoringInformation.DisabilityStatus = applicationDetail.CandidateInformation.DisabilityStatus;
                    _legacyCandidateProvider.UpdateCandidate(candidate);

                    applicationDetail.LegacyApplicationId = _legacyApplicationProvider.CreateApplication(applicationDetail);
                    _traineeeshipApplicationWriteRepository.Save(applicationDetail);
                }
            }
            catch (CustomException ex)
            {
                HandleCustomException(request, ex);
            }
            catch (Exception ex)
            {
                _logger.Error(string.Format("Submit traineeship application with Id = {0} request async process failed.", request.ApplicationId), ex);
                Requeue(request);
            }
        }

        private void HandleCustomException(SubmitTraineeshipApplicationRequest request, CustomException ex)
        {
            switch (ex.Code)
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
                    _logger.Error(string.Format("Traineeship application is in an invalid state: Application Id: \"{0}\"", request.ApplicationId), ex);
                    break;

                default:
                    _logger.Warn(string.Format("Submit traineeship application with Id = {0} request async process failed.", request.ApplicationId), ex);
                    Requeue(request);
                    break;
            }
        }

        private void Requeue(SubmitTraineeshipApplicationRequest request)
        {
            request.ProcessTime = request.ProcessTime.HasValue ? DateTime.Now.AddMinutes(5) : DateTime.Now.AddSeconds(30);
            _messageBus.PublishMessage(request);
        }
    }
}
