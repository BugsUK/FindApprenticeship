namespace SFA.Apprenticeships.Application.UserAccount.Strategies
{
    using Domain.Entities.Exceptions;
    using Domain.Entities.Users;
    using Domain.Interfaces.Repositories;
    using Interfaces;
    using System;
    using Domain.Interfaces.Messaging;
    using Entities;
    using ErrorCodes = Interfaces.Users.ErrorCodes;

    public class ActivateUserStrategy : IActivateUserStrategy
    {
        private readonly IUserReadRepository _userReadRepository;
        private readonly IUserWriteRepository _userWriteRepository;
        private readonly IAuditRepository _auditRepository;
        private readonly IServiceBus _serviceBus;

        public ActivateUserStrategy(IUserReadRepository userReadRepository, IUserWriteRepository userWriteRepository, IAuditRepository auditRepository, IServiceBus serviceBus)
        {
            _userReadRepository = userReadRepository;
            _userWriteRepository = userWriteRepository;
            _auditRepository = auditRepository;
            _serviceBus = serviceBus;
        }

        public void Activate(Guid id, string activationCode)
        {
            var user = _userReadRepository.Get(id);

            user.AssertState("Activate user", UserStatuses.PendingActivation);

            if (!user.ActivationCode.Equals(activationCode, StringComparison.InvariantCultureIgnoreCase))
            {
                throw new CustomException("Invalid activation code \"{0}\" for user with id \"{1}\"", ErrorCodes.UserActivationCodeError, activationCode, id);
            }

            user.SetStateActive();

            user.ActivationDate = DateTime.UtcNow;
            user.LastLogin = DateTime.UtcNow;

            _userWriteRepository.Save(user);
            _serviceBus.PublishMessage(new CandidateUserUpdate(user.EntityId, CandidateUserUpdateType.Update));
            _auditRepository.Audit(user, AuditEventTypes.UserActivatedAccount, user.EntityId);
        }
    }
}
