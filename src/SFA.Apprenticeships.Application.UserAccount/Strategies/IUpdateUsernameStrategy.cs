namespace SFA.Apprenticeships.Application.UserAccount.Strategies
{
    using System;

    public interface IUpdateUsernameStrategy
    {
        void UpdateUsername(Guid userId, string newUsername);

        void UpdateUsername(Guid userId, string verfiyCode, string password);
    }
}
