namespace SFA.Apprenticeships.Application.Interfaces.Users
{
    using System;
    using Domain.Entities.Users;

    /// <summary>
    /// For self-service users to register, activate and manage their credentials. 
    /// Role agnostic (i.e. is not aware of specific roles such as candidate).
    /// Uses the user repository
    /// </summary>
    public interface IUserAccountService
    {
        bool IsUsernameAvailable(string username);

        void Register(string username, Guid userId, string activationCode, UserRoles roles);

        void Activate(string username, string activationCode);

        void ResendActivationCode(string username);

        void SendPasswordResetCode(string username);

        void ResetForgottenPassword(string username, string passwordCode, string newPassword);

        void ResendAccountUnlockCode(string username);

        void UnlockAccount(string username, string accountUnlockCode);

        UserStatuses? GetUserStatus(string username);

        User GetUser(Guid userId);

        string[] GetRoleNames(string username);

        void UpdateUsername(Guid userId, string newUsername);

        void VerifyUpdateUsername(Guid userId, string verfiyCode, string password);

        void ResendUpdateUsernameCode(Guid userId);
    }
}
