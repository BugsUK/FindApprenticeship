namespace SFA.Apprenticeship.Api.AvService.Providers
{
    using System;
    using Services;

    public class WebServiceAuthenticationProvider : IWebServiceAuthenticationProvider
    {
        private readonly IWebServiceConsumerService _webServiceConsumerService;

        public WebServiceAuthenticationProvider(IWebServiceConsumerService webServiceConsumerService)
        {
            _webServiceConsumerService = webServiceConsumerService;
        }

        public AuthenticationResult Authenticate(Guid externalSystemId, string publicKey)
        {
            if (string.IsNullOrWhiteSpace(publicKey))
            {
                return AuthenticationResult.InvalidPublicKey;    
            }

            var webServiceConsumer = _webServiceConsumerService.Get(externalSystemId);

            if (webServiceConsumer == null)
            {
                return AuthenticationResult.InvalidExternalSystemId;
            }

            if (webServiceConsumer.PublicKey == publicKey)
            {
                return AuthenticationResult.Authenticated;
            }

            return AuthenticationResult.AuthenticationFailed;
        }
    }
}
