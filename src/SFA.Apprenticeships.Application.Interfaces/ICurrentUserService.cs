namespace SFA.Apprenticeships.Application.Interfaces
{
    using System.Security.Claims;

    public interface ICurrentUserService
    {
        string CurrentUserName { get; }

        bool IsInRole(string role);

        string GetClaimValue(string type);

        void AddClaim(Claim claim);
    }
}
