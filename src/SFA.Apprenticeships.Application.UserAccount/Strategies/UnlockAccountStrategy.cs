namespace SFA.Apprenticeships.Application.UserAccount.Strategies
{
    using System;
    using Domain.Entities.Exceptions;
    using Domain.Entities.Users;
    using Domain.Interfaces.Messaging;
    using Domain.Interfaces.Repositories;
    using Entities;
    using ErrorCodes = Interfaces.Users.ErrorCodes;

    public class UnlockAccountStrategy : IUnlockAccountStrategy
    {
        private readonly IUserReadRepository _userReadRepository;
        private readonly IUserWriteRepository _userWriteRepository;
        private readonly ISendAccountUnlockCodeStrategy _sendAccountUnlockCodeStrategy;
        private readonly IServiceBus _serviceBus;

        public UnlockAccountStrategy(
            IUserReadRepository userReadRepository,
            IUserWriteRepository userWriteRepository,
            ISendAccountUnlockCodeStrategy sendAccountUnlockCodeStrategy, IServiceBus serviceBus)
        {
            _userReadRepository = userReadRepository;
            _userWriteRepository = userWriteRepository;
            _sendAccountUnlockCodeStrategy = sendAccountUnlockCodeStrategy;
            _serviceBus = serviceBus;
        }

        public void UnlockAccount(string username, string accountUnlockCode)
        {
            var user = _userReadRepository.Get(username);

            user.AssertState("Unlock user account", UserStatuses.Locked);

            if (user.AccountUnlockCodeExpiry < DateTime.UtcNow)
            {
                // NOTE: account unlock code has expired, send a new one.
                _sendAccountUnlockCodeStrategy.SendAccountUnlockCode(username);
                throw new CustomException("Account unlock code has expired, new account unlock code has been sent.",
                    ErrorCodes.AccountUnlockCodeExpired);
            }

            if (!user.AccountUnlockCode.Equals(accountUnlockCode, StringComparison.InvariantCultureIgnoreCase))
            {
                throw new CustomException("Account unlock code \"{0}\" is invalid for user \"{1}\"", ErrorCodes.AccountUnlockCodeInvalid, accountUnlockCode, username);
            }

            user.SetStateActive();
            _userWriteRepository.Save(user);
            _serviceBus.PublishMessage(new CandidateUserUpdate(user.EntityId, CandidateUserUpdateType.Update));
        }
    }
}
