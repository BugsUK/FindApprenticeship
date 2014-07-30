﻿namespace SFA.Apprenticeships.Application.UserAccount.Strategies
{
    using Domain.Entities.Users;

    public interface ILockAccountStrategy
    {
        void LockAccount(User user);
    }
}
