using System.Security.Claims;
using SFA.Apprenticeships.Web.Common.Mediators;

namespace SFA.Apprenticeships.Web.Recruit.Mediators.Home
{
    public interface IHomeMediator
    {
        MediatorResponse Authorize(ClaimsPrincipal principal);
    }
}