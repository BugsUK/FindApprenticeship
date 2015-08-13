namespace SFA.Apprenticeships.Application.Interfaces.Users
{
    using System;
    using Domain.Entities.Users;

    /// <summary>
    /// For self-service users to register, activate and manage their credentials. 
    /// Role agnostic (i.e. is not aware of specific roles such as candidate).
    /// Uses the user repository
    /// </summary>
    //todo: review whether operations should be defined in the account or candidate provider/service interfaces
    public interface IUserAccountService
    {
        bool IsUsernameAvailable(string username);

        void Register(string username, Guid userId, string activationCode, UserRoles roles);

        void Activate(Guid id, string activationCode);

        void ResendActivationCode(string username);

        void SendPasswordResetCode(string username);

        void ResetForgottenPassword(string username, string passwordCode, string newPassword);

        void ResendAccountUnlockCode(string username);

        void UnlockAccount(string username, string accountUnlockCode);

        User GetUser(Guid userId);

        User GetUser(string username, bool errorIfNotFound = true);

        string[] GetRoleNames(Guid userId);

        void UpdateUsername(Guid userId, string newUsername);

        void UpdateUsername(Guid userId, string verfiyCode, string password);

        void ResendUpdateUsernameCode(Guid userId);
    }
}