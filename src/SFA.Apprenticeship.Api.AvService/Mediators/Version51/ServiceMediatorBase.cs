namespace SFA.Apprenticeship.Api.AvService.Mediators.Version51
{
    using System;
    using System.Security;
    using MessageContracts.Version51;
    using Providers;

    public abstract class ServiceMediatorBase
    {
        private readonly IWebServiceAuthenticationProvider _webServiceAuthenticationProvider;

        protected ServiceMediatorBase(IWebServiceAuthenticationProvider webServiceAuthenticationProvider)
        {
            _webServiceAuthenticationProvider = webServiceAuthenticationProvider;
        }

        protected void AuthenticateRequest(NavmsMessageHeader request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            var authenticationResult = _webServiceAuthenticationProvider.Authenticate(
                request.ExternalSystemId, request.PublicKey);

            if (authenticationResult != WebServiceAuthenticationResult.Authenticated)
            {
                throw new SecurityException();
            }
        }
    }
}
