using System.Security.Claims;
using SFA.Apprenticeships.Web.Common.Mediators;
using SFA.Apprenticeships.Web.Manage.ViewModels;

namespace SFA.Apprenticeships.Web.Manage.Mediators.Home
{
    public interface IHomeMediator
    {
        MediatorResponse<AuthorizeResponseViewModel> Authorize(ClaimsPrincipal principal);
    }
}