namespace SFA.DAS.RAA.Api.Services
{
    using System.Collections.Generic;
    using System.Security.Principal;

    public interface IAuthenticationService
    {
        IPrincipal Authenticate(IDictionary<string, string> claims);
    }
}