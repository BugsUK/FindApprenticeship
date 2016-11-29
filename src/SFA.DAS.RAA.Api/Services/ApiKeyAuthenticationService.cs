namespace SFA.DAS.RAA.Api.Services
{
    using System.Collections.Generic;
    using System.Security.Claims;
    using System.Security.Principal;

    public class ApiKeyAuthenticationService : IAuthenticationService
    {
        public const string ApiKeyKey = "api_key";

        public IPrincipal Authenticate(IDictionary<string, string> claims)
        {
            string apiKey;
            if (claims.TryGetValue(ApiKeyKey, out apiKey))
            {
                var apiKeyClaim = new Claim(ClaimTypes.Authentication, apiKey);
                var claimsIdentity = new ClaimsIdentity(new[] {apiKeyClaim});
                return new ClaimsPrincipal(claimsIdentity);
            }

            return new ClaimsPrincipal(new ClaimsIdentity());
        }
    }
}