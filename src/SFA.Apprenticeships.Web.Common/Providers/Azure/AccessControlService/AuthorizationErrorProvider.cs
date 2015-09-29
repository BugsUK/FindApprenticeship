namespace SFA.Apprenticeships.Web.Common.Providers.Azure.AccessControlService
{
    using Models.Azure.AccessControlService;
    using Newtonsoft.Json;

    public class AuthorizationErrorProvider : IAuthorizationErrorProvider
    {
        public AuthorizationErrorDetailsViewModel GetAuthorizationErrorDetailsViewModel(string jsonErrorDetails)
        {
            return jsonErrorDetails == null
                ? null
                : JsonConvert.DeserializeObject<AuthorizationErrorDetailsViewModel>(jsonErrorDetails);
        }
    }
}
