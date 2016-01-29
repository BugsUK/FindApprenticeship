namespace SFA.Apprenticeship.Api.AvService.Repositories
{
    using System;
    using System.Linq;
    using Apprenticeships.Domain.Entities.WebServices;
    using Apprenticeships.Domain.Interfaces.Repositories.SFA.Apprenticeship.Api.AvService.Repositories;

    // TODO: AG: US876: remove deadcode, fake repository not required.

    public class FakeWebServiceConsumerRepository : IWebServiceConsumerReadRepository
    {
        private static readonly WebServiceConsumer[] WebServiceConsumers =
        {
            new WebServiceConsumer
            {
                ExternalSystemId = new Guid("bbd33d6d-fc94-4be3-9e52-282bf7293356"),
                ExternalSystemPassword = "password",
                WebServiceConsumerType = WebServiceConsumerType.Provider,
                AllowReferenceDataService = true,
                AllowVacancyUploadService = true,
                AllowVacancySummaryService = true,
                AllowVacancyDetailService = true,
            },
            new WebServiceConsumer
            {
                ExternalSystemId = new Guid("63c545db-d08f-4a9b-b05b-33727432e987"),
                ExternalSystemPassword = "letmein",
                WebServiceConsumerType = WebServiceConsumerType.Employer,
                AllowReferenceDataService = true
            }
        };

        public WebServiceConsumer Get(Guid externalSystemId)
        {
            return WebServiceConsumers.FirstOrDefault(
                each => each.ExternalSystemId == externalSystemId);
        }
    }
}
