namespace SFA.Apprenticeship.Api.AvService.Providers
{
    using System;
    using Apprenticeships.Domain.Entities.WebServices;
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
            Guid externalSystemId, string externalSystemPassword, WebServiceCategory webServiceCategory)
        {
            if (string.IsNullOrWhiteSpace(externalSystemPassword))
            {
                return WebServiceAuthenticationResult.InvalidExternalSystemPassword;
            }

            var webServiceConsumer = _webServiceConsumerService.Get(externalSystemId);

            if (webServiceConsumer == null)
            {
                return WebServiceAuthenticationResult.InvalidExternalSystemId;
            }

            if (webServiceConsumer.ExternalSystemPassword != externalSystemPassword)
            {
                return WebServiceAuthenticationResult.AuthenticationFailed;
            }

            // TODO: AG: FIXUP.

            /*
            if (!webServiceConsumer.AllowedWebServiceCategories.Contains(webServiceCategory))
            {
                return WebServiceAuthenticationResult.NotAllowed;
            }
            */

            return WebServiceAuthenticationResult.Authenticated;
        }
    }
}
