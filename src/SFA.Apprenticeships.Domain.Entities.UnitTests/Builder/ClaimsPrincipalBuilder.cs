namespace SFA.Apprenticeships.Domain.Entities.UnitTests.Builder
{
    using System.Security.Claims;

    public class ClaimsPrincipalBuilder
    {
        private string _name;
        private string _role;

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
    }
}