namespace SFA.DAS.RAA.Api.Extensions
{
    using System.Security.Claims;
    using Apprenticeships.Domain.Entities.Raa.Api;
    using Apprenticeships.Domain.Entities.Raa.RaaApi;
    using Newtonsoft.Json;

    public static class ClaimsIdentityExtensions
    {
        public static RaaApiUser GetRaaApiUser(this ClaimsIdentity claimsIdentity)
        {
            if (claimsIdentity.HasClaim(c => c.Type == ClaimTypes.UserData))
            {
                var userData = claimsIdentity.FindFirst(c => c.Type == ClaimTypes.UserData);
                var raaApiUser = JsonConvert.DeserializeObject<RaaApiUser>(userData.Value);
                return raaApiUser;
            }

            return RaaApiUser.UnknownApiUser;
        }
    }
}