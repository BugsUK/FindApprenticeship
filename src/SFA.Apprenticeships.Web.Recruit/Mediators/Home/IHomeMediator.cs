using System.Security.Claims;
using SFA.Apprenticeships.Web.Common.Mediators;
using SFA.Apprenticeships.Web.Recruit.ViewModels;

namespace SFA.Apprenticeships.Web.Recruit.Mediators.Home
{
    public interface IHomeMediator
    {
        MediatorResponse<AuthorizeResponseViewModel> Authorize(ClaimsPrincipal principal);
    }
}