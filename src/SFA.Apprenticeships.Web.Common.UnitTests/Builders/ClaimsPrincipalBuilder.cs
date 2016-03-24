namespace SFA.Apprenticeships.Web.Common.UnitTests.Builders
{
    using System.Security.Claims;

    public class ClaimsPrincipalBuilder
    {
        private string _name;
        private string _role;
        private string _ukprn;
        private string _roleList;
        private string _groupClaim;

        public ClaimsPrincipal Build()
        {
            var identity = new ClaimsIdentity();
            if (_name != null)
            {
                identity.AddClaim(new Claim(ClaimTypes.Name, _name));
            }
            if (_role != null)
            {
                identity.AddClaim(new Claim(ClaimTypes.Role, _role));
            }
            if (_ukprn != null)
            {
                identity.AddClaim(new Claim(Constants.ClaimTypes.Ukprn, _ukprn));
            }
            if (_roleList != null)
            {
                identity.AddClaim(new Claim(Constants.ClaimTypes.RoleList, _roleList));
            }
            if (_groupClaim != null)
            {
                identity.AddClaim(new Claim(Constants.ClaimTypes.Group, _groupClaim));
            }

            var principal = new ClaimsPrincipal(identity);
            return principal;
        }

        public ClaimsPrincipalBuilder WithName(string name)
        {
            _name = name;
            return this;
        }

        public ClaimsPrincipalBuilder WithRole(string role)
        {
            _role = role;
            return this;
        }

        public ClaimsPrincipalBuilder WithUkprn(string ukprn)
        {
            _ukprn = ukprn;
            return this;
        }

        public ClaimsPrincipalBuilder WithRoleList(string roleList)
        {
            _roleList = roleList;
            return this;
        }

        public ClaimsPrincipalBuilder WithGroup(string groupClaim)
        {
            _groupClaim = groupClaim;
            return this;
        }
    }
}