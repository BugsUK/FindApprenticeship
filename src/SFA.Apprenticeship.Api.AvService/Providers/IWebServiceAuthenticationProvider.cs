namespace SFA.Apprenticeship.Api.AvService.Providers
{
    using System;

    public interface IWebServiceAuthenticationProvider
    {
        AuthenticationResult Authenticate(Guid externalSystemId, string publicKey);
    }
}
