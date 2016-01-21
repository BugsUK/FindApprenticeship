namespace SFA.Apprenticeship.Api.AvService.Providers
{
    using System;
    using Domain;
    using Services;

    // REF: Navms.Ms.ExternalInterfaces.ServiceImplementation.BulkVacancyUploadService
    // REF: Navms.Ms.ExternalInterfaces.ServiceImplementation.AvmsFacadeService
    // REF: Capgemini.LSC.Navms.Ms.BusinessLogic.Services.Security.Security

    public class WebServiceAuthenticationProvider : IWebServiceAuthenticationProvider
    {
        private readonly IWebServiceConsumerService _webServiceConsumerService;

        public WebServiceAuthenticationProvider(
            IWebServiceConsumerService webServiceConsumerService)
        {
            _webServiceConsumerService = webServiceConsumerService;
        }

        public WebServiceAuthenticationResult Authenticate(
            Guid externalSystemId, string publicKey, WebServiceCategory webServiceCategory)
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

            if (webServiceConsumer.PublicKey != publicKey)
            {
                return WebServiceAuthenticationResult.AuthenticationFailed;
            }

            if (!webServiceConsumer.AllowedWebServiceCategories.Contains(webServiceCategory))
            {
                return WebServiceAuthenticationResult.NotAllowed;
            }

            return WebServiceAuthenticationResult.Authenticated;
        }
    }
}
