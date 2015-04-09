namespace SFA.Apprenticeships.Application.UserAccount.Strategies
{
    using System;

    public interface ISendPendingUsernameCodeStrategy
    {
        void SendPendingUsernameCode(Guid userId);
    }
}