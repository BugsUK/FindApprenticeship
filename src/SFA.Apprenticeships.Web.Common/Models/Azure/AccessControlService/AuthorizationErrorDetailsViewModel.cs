// TODO: AG: could / should map this to generic view model (currently, they're Azure ACS-specific).
namespace SFA.Apprenticeships.Web.Common.Models.Azure.AccessControlService
{
    public class AuthorizationErrorDetailsViewModel
    {
        public string Context { get; set; }

        public string HttpReturnCode { get; set; }

        public string IdentityProvider { get; set; }

        public string TimeStamp { get; set; }

        public string TraceId { get; set; }

        public AuthorizationErrorViewModel[] Errors { get; set; }
    }
}
