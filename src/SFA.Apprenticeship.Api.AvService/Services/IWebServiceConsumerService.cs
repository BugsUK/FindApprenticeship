namespace SFA.Apprenticeship.Api.AvService.Services
{
    using System;
    using Apprenticeships.Domain.Entities.WebServices;

    public interface IWebServiceConsumerService
    {
        WebServiceConsumer Get(Guid externalSystemId);
    }
}
