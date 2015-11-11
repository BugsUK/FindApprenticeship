namespace SFA.Apprenticeships.Web.Common.Models.Common
{
    public class AuthorizationData
    {
        public AuthorizationData()
        {
            Claims = new AuthorizationClaim[0];
        }

        public string Username { get; set; }
        public AuthorizationClaim[] Claims { get; set; }
    }
}