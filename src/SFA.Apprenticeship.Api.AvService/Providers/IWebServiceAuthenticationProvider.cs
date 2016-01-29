namespace SFA.Apprenticeship.Api.AvService.Providers
{
    using System;
    using Apprenticeships.Domain.Entities.WebServices;

    public interface IWebServiceAuthenticationProvider
    {
        WebServiceAuthenticationResult Authenticate(
            Guid externalSystemId, string externalSystemPassword, WebServiceCategory webServiceCategory);
    }
}
