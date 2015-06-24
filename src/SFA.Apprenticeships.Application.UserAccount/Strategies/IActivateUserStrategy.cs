namespace SFA.Apprenticeships.Application.UserAccount.Strategies
{
    using System;

    public interface IActivateUserStrategy
    {
        void Activate(Guid id, string activationCode);
    }
}
