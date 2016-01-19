namespace SFA.Apprenticeship.Api.AvService.Providers
{
    using System;

    public interface IWebServiceAuthenticationProvider
    {
        WebServiceAuthenticationResult Authenticate(Guid externalSystemId, string publicKey);
    }
}
