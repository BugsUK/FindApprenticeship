namespace SFA.Apprenticeship.Api.AvService.Services
{
    using System;
    using Domain;
    using Repositories;

    public class WebServiceConsumerService : IWebServiceConsumerService
    {
        private readonly IWebServiceConsumerReadRepository _webServiceConsumerReadRepository;

        public WebServiceConsumerService(IWebServiceConsumerReadRepository webServiceConsumerReadRepository)
        {
            _webServiceConsumerReadRepository = webServiceConsumerReadRepository;
        }

        public WebServiceConsumer Get(Guid externalSystemId)
        {
            return _webServiceConsumerReadRepository.Get(externalSystemId);
        }
    }
}
