namespace SFA.Apprenticeships.Infrastructure.Processes.Applications
{
    using System;
    using Application.Candidate;
    using Application.Interfaces.Applications;
    using SFA.Infrastructure.Interfaces;
    using Domain.Entities.Applications;
    using Domain.Entities.Exceptions;
    using Domain.Entities.Users;
    using Domain.Interfaces.Messaging;
    using Domain.Interfaces.Repositories;

    using SFA.Apprenticeships.Application.Interfaces;

    public class SubmitApprenticeshipApplicationRequestSubscriber : IServiceBusSubscriber<SubmitApprenticeshipApplicationRequest>
    {
        private readonly ILogService _logger;

        private readonly IApprenticeshipApplicationReadRepository _apprenticeshipApplicationReadRepository;
        private readonly IApprenticeshipApplicationWriteRepository _apprenticeshipApplicationWriteRepository;
        private readonly ICandidateReadRepository _candidateReadRepository;
        private readonly IUserReadRepository _userReadRepository;

        private readonly ILegacyApplicationProvider _legacyApplicationProvider;
        private readonly ILegacyCandidateProvider _legacyCandidateProvider;

        public SubmitApprenticeshipApplicationRequestSubscriber(
            ILogService logger,
            IApprenticeshipApplicationReadRepository apprenticeshipApplicationReadRepository,
            IApprenticeshipApplicationWriteRepository apprenticeshipApplicationWriteRepository,
            ICandidateReadRepository candidateReadRepository,
            IUserReadRepository userReadRepository,
            ILegacyApplicationProvider legacyApplicationProvider,
            ILegacyCandidateProvider legacyCandidateProvider)
        {
            _logger = logger;
            _apprenticeshipApplicationReadRepository = apprenticeshipApplicationReadRepository;
            _apprenticeshipApplicationWriteRepository = apprenticeshipApplicationWriteRepository;
            _candidateReadRepository = candidateReadRepository;
            _userReadRepository = userReadRepository;
            _legacyApplicationProvider = legacyApplicationProvider;
            _legacyCandidateProvider = legacyCandidateProvider;
        }

        [ServiceBusTopicSubscription(TopicName = "SubmitApprenticeshipApplication")]
        public ServiceBusMessageStates Consume(SubmitApprenticeshipApplicationRequest request)
        {
            return CreateApplication(request);
        }

        private ServiceBusMessageStates CreateApplication(SubmitApprenticeshipApplicationRequest request)
        {
            _logger.Debug("Creating apprenticeship application Id: {0}", request.ApplicationId);

            var applicationDetail = _apprenticeshipApplicationReadRepository.Get(request.ApplicationId, true);

            try
            {
                var candidate = _candidateReadRepository.Get(applicationDetail.CandidateId, true);

                if (candidate.LegacyCandidateId == 0)
                {
                    var user = _userReadRepository.Get(applicationDetail.CandidateId);

                    if (user == null || user.Status == UserStatuses.PendingDeletion)
                    {
                        _logger.Warn(
                            "User with Id: {0} is set as pending deletion. Application with Id: {1} cannot be submitted and will be set to draft. Message will not be requeued",
                            applicationDetail.CandidateId, request.ApplicationId);
                        applicationDetail.RevertStateToDraft();
                        _apprenticeshipApplicationWriteRepository.Save(applicationDetail);
                    }
                    else
                    {
                        _logger.Info(
                            "Candidate with Id: {0} has not been created in the legacy system. Message will be requeued",
                            applicationDetail.CandidateId);

                        return ServiceBusMessageStates.Requeue;
                    }
                }

                EnsureApplicationCanBeCreated(applicationDetail);

                // Update candidate disability status to match the application.
                candidate.MonitoringInformation.DisabilityStatus = applicationDetail.CandidateInformation.DisabilityStatus;
                _legacyCandidateProvider.UpdateCandidate(candidate);

                applicationDetail.LegacyApplicationId = _legacyApplicationProvider.CreateApplication(applicationDetail);
                SetApplicationStateSubmitted(applicationDetail);

                return ServiceBusMessageStates.Complete;
            }
            catch (CustomException e)
            {
                return HandleCustomException(request, e, applicationDetail);
            }
            catch (Exception e)
            {
                _logger.Error("Submit apprenticeship application with Id = {0} request async process failed, message will be requeued",
                    e, request.ApplicationId);

                return ServiceBusMessageStates.Requeue;
            }
        }

        private ServiceBusMessageStates HandleCustomException(
            SubmitApprenticeshipApplicationRequest request,
            CustomException e,
            ApprenticeshipApplicationDetail apprenticeshipApplication)
        {
            switch (e.Code)
            {
                case ErrorCodes.ApplicationDuplicatedError:
                    _logger.Info("Apprenticeship application has already been submitted to legacy system: Application Id: \"{0}\"", request.ApplicationId);
                    SetApplicationStateSubmitted(apprenticeshipApplication);
                    break;

                case Application.Interfaces.Candidates.ErrorCodes.CandidateStateError:
                    _logger.Error("Legacy candidate is in an invalid state. Apprenticeship application cannot be processed: Application Id: \"{0}\"", request.ApplicationId);
                    break;

                case Application.Interfaces.Candidates.ErrorCodes.CandidateNotFoundError:
                    //TODO: This can happen when a user requests that their account should be deleted. We need to work out what to do in that case. Probably set the candidate's status to inactive and the application's status to draft
                    _logger.Error("Legacy candidate was not found. Apprenticeship application cannot be processed: Application Id: \"{0}\"", request.ApplicationId);
                    break;

                case Application.Interfaces.Vacancies.ErrorCodes.LegacyVacancyStateError:
                    _logger.Info("Legacy vacancy was in an invalid state. Apprenticeship application cannot be processed: Application Id: \"{0}\"", request.ApplicationId);
                    SetStateExpiredOrWithdrawn(apprenticeshipApplication);
                    break;

                case Domain.Entities.ErrorCodes.EntityStateError:
                    _logger.Error(string.Format("Apprenticeship application is in an invalid state: Application Id: \"{0}\"", request.ApplicationId), e);
                    break;

                default:
                    _logger.Warn(string.Format("Submit apprenticeship application with Id = {0} request async process failed with code = {1}, message will be requeued", request.ApplicationId, e.Code), e);
                    return ServiceBusMessageStates.Requeue;
            }

            return ServiceBusMessageStates.Complete;
        }

        private void SetApplicationStateSubmitted(ApprenticeshipApplicationDetail apprenticeshipApplication)
        {
            apprenticeshipApplication.SetStateSubmitted();
            _apprenticeshipApplicationWriteRepository.Save(apprenticeshipApplication);
        }

        private void SetStateExpiredOrWithdrawn(ApprenticeshipApplicationDetail apprenticeshipApplication)
        {
            apprenticeshipApplication.SetStateExpiredOrWithdrawn();
            _apprenticeshipApplicationWriteRepository.Save(apprenticeshipApplication);
        }

        private static void EnsureApplicationCanBeCreated(ApprenticeshipApplicationDetail apprenticeshipApplicationDetail)
        {
            apprenticeshipApplicationDetail.AssertState("Create apprenticeship application", ApplicationStatuses.Submitting);
        }
    }
}
