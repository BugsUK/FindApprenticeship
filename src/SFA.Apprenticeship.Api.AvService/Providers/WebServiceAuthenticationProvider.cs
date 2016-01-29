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

            if (webServiceCategory == WebServiceCategory.Reference && !webServiceConsumer.AllowReferenceDataService)
            {
                return WebServiceAuthenticationResult.NotAllowed;
            }

            if (webServiceCategory == WebServiceCategory.VacancyUpload)
            {
                if (!webServiceConsumer.AllowVacancyUploadService ||
                    !(webServiceConsumer.WebServiceConsumerType == WebServiceConsumerType.Provider ||
                    webServiceConsumer.WebServiceConsumerType == WebServiceConsumerType.Employer))
                {
                    return WebServiceAuthenticationResult.NotAllowed;
                }
            }

            if (webServiceCategory == WebServiceCategory.VacancyDetail && !webServiceConsumer.AllowVacancyDetailService)
            {
                return WebServiceAuthenticationResult.NotAllowed;
            }

            if (webServiceCategory == WebServiceCategory.VacancySummary && !webServiceConsumer.AllowVacancySummaryService)
            {
                return WebServiceAuthenticationResult.NotAllowed;
            }

            return WebServiceAuthenticationResult.Authenticated;
        }
    }
}
