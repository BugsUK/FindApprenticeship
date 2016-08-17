namespace SFA.Apprenticeships.Application.Interfaces
{
    public interface ICurrentUserService
    {
        string CurrentUserName { get; }

        bool IsInRole(string role);

        string GetClaimValue(string type);
    }
}
