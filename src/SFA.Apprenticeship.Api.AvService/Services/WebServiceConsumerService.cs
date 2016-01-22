namespace SFA.Apprenticeship.Api.AvService.Services
{
    using System;
    using Apprenticeships.Domain.Entities.WebServices;
    using Apprenticeships.Domain.Interfaces.Repositories.SFA.Apprenticeship.Api.AvService.Repositories;

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
