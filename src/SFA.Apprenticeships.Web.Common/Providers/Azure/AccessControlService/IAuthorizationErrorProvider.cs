namespace SFA.Apprenticeships.Web.Common.Providers.Azure.AccessControlService
{
    using Common.Models.Azure.AccessControlService;

    public interface IAuthorizationErrorProvider
    {
        AuthorizationErrorDetailsViewModel GetAuthorizationErrorDetailsViewModel(string jsonErrorDetails);
    }
}
