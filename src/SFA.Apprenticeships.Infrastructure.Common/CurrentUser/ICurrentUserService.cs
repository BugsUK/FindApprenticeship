namespace SFA.Apprenticeships.Infrastructure.Common.CurrentUser
{
    using System.Threading;

    using SFA.Apprenticeships.Application.Interfaces;

    public class CurrentUserService : ICurrentUserService
    {
        public string CurrentUserName => Thread.CurrentPrincipal.Identity.Name;

        public bool IsInRole(string role) => Thread.CurrentPrincipal.IsInRole(role);
    }
}