namespace SFA.DAS.RAA.Api.Services
{
    using System.Collections.Generic;
    using System.Security.Claims;
    using System.Security.Principal;
    using Apprenticeships.Domain.Entities.Raa.RaaApi;
    using Apprenticeships.Domain.Raa.Interfaces.Repositories;
    using Constants;
    using Newtonsoft.Json;

    public class ApiKeyAuthenticationService : IAuthenticationService
    {
        public const string ApiKeyKey = "api_key";

        private readonly IRaaApiUserRepository _raaApiUserRepository;

        public ApiKeyAuthenticationService(IRaaApiUserRepository raaApiUserRepository)
        {
            _raaApiUserRepository = raaApiUserRepository;
        }

        public IPrincipal Authenticate(IDictionary<string, string> claims)
        {
            string apiKey;
            if (claims.TryGetValue(ApiKeyKey, out apiKey))
            {
                var user = _raaApiUserRepository.GetUser(apiKey);

                var apiKeyClaim = new Claim(ClaimTypes.Authentication, apiKey);
                var nameClaim = new Claim(ClaimTypes.Name, user.Name);
                var userDataClaim = new Claim(ClaimTypes.UserData, JsonConvert.SerializeObject(user));

                var identityClaims = new [] { apiKeyClaim, nameClaim, userDataClaim };
                var claimsIdentity = new ClaimsIdentity(identityClaims, Equals(user, RaaApiUser.UnknownApiUser) ? null : "ApiKey");

                switch (user.UserType)
                {
                    case RaaApiUserType.Provider:
                        claimsIdentity.AddClaim(new Claim(ClaimTypes.Role, Roles.Provider));
                        break;
                    case RaaApiUserType.Employer:
                        claimsIdentity.AddClaim(new Claim(ClaimTypes.Role, Roles.Employer));
                        break;
                }

                return new ClaimsPrincipal(claimsIdentity);
            }

            return new ClaimsPrincipal(new ClaimsIdentity());
        }
    }
}