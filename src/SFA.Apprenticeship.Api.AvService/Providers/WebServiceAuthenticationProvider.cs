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

        public WebServiceAuthenticationResult Authenticate(Guid externalSystemId, string publicKey)
        {
            if (string.IsNullOrWhiteSpace(publicKey))
            {
                return WebServiceAuthenticationResult.InvalidPublicKey;    
            }

            var webServiceConsumer = _webServiceConsumerService.Get(externalSystemId);

            if (webServiceConsumer == null)
            {
                return WebServiceAuthenticationResult.InvalidExternalSystemId;
            }

            if (webServiceConsumer.PublicKey == publicKey)
            {
                return WebServiceAuthenticationResult.Authenticated;
            }

            return WebServiceAuthenticationResult.AuthenticationFailed;
        }
    }
}
