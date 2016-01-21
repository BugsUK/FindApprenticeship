namespace SFA.Apprenticeship.Api.AvService.Mediators.Version51
{
    using System;
    using System.Security;
    using Domain;
    using MessageContracts.Version51;
    using Providers;

    public abstract class ServiceMediatorBase
    {
        private readonly IWebServiceAuthenticationProvider _webServiceAuthenticationProvider;
        private readonly WebServiceCategory _webServiceCategory;

        protected ServiceMediatorBase(
            IWebServiceAuthenticationProvider webServiceAuthenticationProvider,
            WebServiceCategory webServiceCategory)
        {
            _webServiceAuthenticationProvider = webServiceAuthenticationProvider;
            _webServiceCategory = webServiceCategory;
        }

        protected void AuthenticateRequest(NavmsMessageHeader request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            var authenticationResult = _webServiceAuthenticationProvider.Authenticate(
                request.ExternalSystemId, request.PublicKey, _webServiceCategory);

            if (authenticationResult != WebServiceAuthenticationResult.Authenticated)
            {
                throw new SecurityException();
            }
        }
    }
}
