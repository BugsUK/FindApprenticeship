namespace SFA.Apprenticeship.Api.AvService.Repositories
{
    using System;
    using System.Linq;
    using Domain;

    public class FakeWebServiceConsumerRepository : IWebServiceConsumerReadRepository
    {
        private static readonly WebServiceConsumer[] WebServiceConsumers =
        {
            new WebServiceConsumer
            {
                ExternalSystemId = new Guid("bbd33d6d-fc94-4be3-9e52-282bf7293356"),
                PublicKey = "password",
                AllowedWebServiceCategories =
                {
                    WebServiceCategory.Reference,
                    WebServiceCategory.VacancyUpload,
                    WebServiceCategory.VacancySummary,
                    WebServiceCategory.VacancyDetail
                }
            },
            new WebServiceConsumer
            {
                ExternalSystemId = new Guid("63c545db-d08f-4a9b-b05b-33727432e987"),
                PublicKey = "letmein",
                AllowedWebServiceCategories =
                {
                    WebServiceCategory.Reference
                }
            }
        };

        public WebServiceConsumer Get(Guid externalSystemId)
        {
            return WebServiceConsumers.FirstOrDefault(
                each => each.ExternalSystemId == externalSystemId);
        }
    }
}
