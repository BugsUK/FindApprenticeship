using System.Security.Claims;

namespace SFA.Apprenticeships.Web.Recruit.UnitTests.Builders
{
    public class ClaimsPrincipalBuilder
    {
        private string _name = null;
        private string _ukprn = null;
        private string _role = null;

        public ClaimsPrincipal Build()
        {
            var identity = new ClaimsIdentity();
            if (_name != null)
            {
                identity.AddClaim(new Claim(ClaimTypes.Name, _name));
            }
            if (_ukprn != null)
            {
                identity.AddClaim(new Claim(Constants.ClaimTypes.Ukprn, _ukprn));
            }
            if (_role != null)
            {
                identity.AddClaim(new Claim(ClaimTypes.Role, _role));
            }
            var principal = new ClaimsPrincipal(identity);
            return principal;
        }
    }
}