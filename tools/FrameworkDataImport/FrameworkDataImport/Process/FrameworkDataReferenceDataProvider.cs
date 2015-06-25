namespace FrameworkDataImport.Process
{
    using System.Collections.Generic;
    using SFA.Apprenticeships.Application.Interfaces.Logging;
    using SFA.Apprenticeships.Application.ReferenceData;
    using SFA.Apprenticeships.Domain.Entities.ReferenceData;
    using SFA.Apprenticeships.Infrastructure.Elastic.Common.Configuration;

    public class FrameworkDataReferenceDataProvider : IReferenceDataProvider
    {
        private const string IndexName = "categories";

        private readonly IElasticsearchClientFactory _elasticsearchClientFactory;
        private readonly ILogService _logger;

        public FrameworkDataReferenceDataProvider(IElasticsearchClientFactory elasticsearchClientFactory, ILogService logger)
        {
            _elasticsearchClientFactory = elasticsearchClientFactory;
            _logger = logger;
        }

        public IEnumerable<Category> GetCategories()
        {
            var client = _elasticsearchClientFactory.GetElasticClient();
            var allCategories = client.Search<Category>(s => s.Index(IndexName).Size(100));
            return allCategories.Documents;
        }

        public Category GetSubCategoryByName(string subCategoryName)
        {
            throw new System.NotImplementedException();
        }

        public Category GetCategoryByName(string categoryName)
        {
            throw new System.NotImplementedException();
        }

        public Category GetSubCategoryByCode(string subCategoryCode)
        {
            throw new System.NotImplementedException();
        }

        public Category GetCategoryByCode(string categoryCode)
        {
            throw new System.NotImplementedException();
        }
    }
}