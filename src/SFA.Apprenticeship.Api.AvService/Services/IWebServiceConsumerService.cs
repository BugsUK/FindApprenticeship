namespace SFA.Apprenticeship.Api.AvService.Services
{
    using System;
    using Domain;

    public interface IWebServiceConsumerService
    {
        WebServiceConsumer Get(Guid externalSystemId);
    }
}
