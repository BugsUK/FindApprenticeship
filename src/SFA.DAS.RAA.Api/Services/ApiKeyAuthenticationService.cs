namespace SFA.DAS.RAA.Api.Services
{
    using System;
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
                var apiKeyClaim = new Claim(ClaimTypes.Authentication, apiKey);

                Guid apiKeyGuid;
                if (Guid.TryParse(apiKey, out apiKeyGuid))
                {
                    var user = _raaApiUserRepository.GetUser(apiKey);

                    var userDataClaim = new Claim(ClaimTypes.UserData, JsonConvert.SerializeObject(user));

                    var identityClaims = new List<Claim> { apiKeyClaim, userDataClaim };

                    var namePostfix = "";
                    switch (user.UserType)
                    {
                        case RaaApiUserType.Provider:
                            identityClaims.Add(new Claim(ClaimTypes.Role, Roles.Provider));
                            namePostfix = "_Provider";
                            break;
                        case RaaApiUserType.Employer:
                            identityClaims.Add(new Claim(ClaimTypes.Role, Roles.Employer));
                            namePostfix = "_Employer";
                            break;
                    }

                    var isUnknownApiUser = Equals(user, RaaApiUser.UnknownApiUser);

                    var name = isUnknownApiUser ? $"RaaApiUser_UnknownApiKey_{apiKey}" : $"RaaApiUser_ApiKey_{apiKey}{namePostfix}";
                    identityClaims.Add(new Claim(ClaimTypes.Name, name));

                    var claimsIdentity = new ClaimsIdentity(identityClaims, isUnknownApiUser ? null : "ApiKey");
                    return new ClaimsPrincipal(claimsIdentity);
                }

                return new ClaimsPrincipal(new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, $"RaaApiUser_InvalidApiKey_{apiKey}"),
                    apiKeyClaim
                }));
            }

            return new ClaimsPrincipal(new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Name, "RaaApiUser_MissingApiKey")
            }));
        }
    }
}