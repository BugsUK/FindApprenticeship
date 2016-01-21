namespace SFA.Apprenticeship.Api.AvService.Providers
{
    using System;
    using Domain;

    public interface IWebServiceAuthenticationProvider
    {
        WebServiceAuthenticationResult Authenticate(
            Guid externalSystemId, string publicKey, WebServiceCategory webServiceCategory);
    }
}
