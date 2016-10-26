namespace SFA.Apprenticeships.Application.Candidate.Strategies
{
    using Domain.Entities.Candidates;
    using Domain.Entities.Exceptions;
    using Domain.Interfaces.Repositories;
    using Interfaces;
    using System;
    using Domain.Interfaces.Messaging;
    using UserAccount.Entities;

    public class VerifyMobileStrategy : IVerifyMobileStrategy
    {
        private readonly ICandidateReadRepository _candidateReadRepository;
        private readonly ICandidateWriteRepository _candidateWriteRepository;
        private readonly IAuditRepository _auditRepository;
        private readonly IServiceBus _serviceBus;

        public VerifyMobileStrategy(ICandidateReadRepository candidateReadRepository, ICandidateWriteRepository candidateWriteRepository, IAuditRepository auditRepository, IServiceBus serviceBus)
        {
            _candidateReadRepository = candidateReadRepository;
            _candidateWriteRepository = candidateWriteRepository;
            _auditRepository = auditRepository;
            _serviceBus = serviceBus;
        }

        public void VerifyMobile(Guid candidateId, string verificationCode)
        {
            var candidate = _candidateReadRepository.Get(candidateId);

            if (!candidate.MobileVerificationRequired())
            {
                var message = string.Format("The mobile number associated with candidate Id: {0} does not require verification.", candidate.EntityId);
                throw new CustomException(message, Domain.Entities.ErrorCodes.EntityStateError);
            }

            if (candidate.CommunicationPreferences.MobileVerificationCode == verificationCode)
            {
                candidate.CommunicationPreferences.MobileVerificationCode = string.Empty;
                candidate.CommunicationPreferences.MobileVerificationCodeDateCreated = null;
                candidate.CommunicationPreferences.VerifiedMobile = true;

                _candidateWriteRepository.Save(candidate);
                _serviceBus.PublishMessage(new CandidateUserUpdate(candidate.EntityId, CandidateUserUpdateType.Update));
                _auditRepository.Audit(candidate, AuditEventTypes.CandidateVerifiedMobileNumber, candidate.EntityId);
            }
            else
            {
                var errorMessage = string.Format("Mobile verification code {0} is invalid for candidate {1} with mobile number {2}", verificationCode, candidateId, candidate.RegistrationDetails.PhoneNumber);
                throw new CustomException(errorMessage, Interfaces.Users.ErrorCodes.MobileCodeVerificationFailed);
            }
        }
    }
}
