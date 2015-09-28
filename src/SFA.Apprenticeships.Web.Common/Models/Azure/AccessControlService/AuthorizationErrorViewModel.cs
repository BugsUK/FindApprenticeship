// TODO: AG: could / should map this to generic view model (currently, they're Azure ACS-specific).
namespace SFA.Apprenticeships.Web.Common.Models.Azure.AccessControlService
{
    public class AuthorizationErrorViewModel
    {
        public string ErrorCode { get; set; }

        public string ErrorMessage { get; set; }
    }
}
