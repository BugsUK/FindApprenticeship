namespace SFA.Apprenticeships.Web.Common.Providers.Azure.AccessControlService
{
    using Common.Models.Azure.AccessControlService;
    using Newtonsoft.Json;

    public class AuthorizationErrorProvider : IAuthorizationErrorProvider
    {
        public AuthorizationErrorDetailsViewModel GetAuthorizationErrorDetailsViewModel(string jsonErrorDetails)
        {
            if (jsonErrorDetails == null)
            {
                return null;
            }

            return JsonConvert.DeserializeObject<AuthorizationErrorDetailsViewModel>(jsonErrorDetails);
        }
    }
}
