namespace SFA.Apprenticeships.Infrastructure.FrameworkDataProvider
{
    using System.Collections.Generic;
    using System.Linq;
    using Application.ReferenceData;
    using Domain.Entities.ReferenceData;
    using Elastic.Common.Configuration;

    public class FrameworkDataProvider : IReferenceDataProvider
    {
        private const string IndexName = "categories";

        private readonly IElasticsearchClientFactory _elasticsearchClientFactory;

        public FrameworkDataProvider(IElasticsearchClientFactory elasticsearchClientFactory)
        {
            _elasticsearchClientFactory = elasticsearchClientFactory;
        }

        public IEnumerable<Category> GetCategories()
        {
            var client = _elasticsearchClientFactory.GetElasticClient();
            var allCategories = client.Search<Category>(s => s.Index(IndexName).Size(100));
            return allCategories.Documents;
        }

        public Category GetSubCategoryByName(string subCategoryName)
        {
            return GetCategories().SelectMany(c => c.SubCategories).FirstOrDefault(sc => sc.FullName == subCategoryName);
        }

        public Category GetCategoryByName(string categoryName)
        {
            return GetCategories().FirstOrDefault(c => c.FullName == categoryName);
        }

        public Category GetSubCategoryByCode(string subCategoryCode)
        {
            return GetCategories().SelectMany(c => c.SubCategories).FirstOrDefault(sc => sc.CodeName == subCategoryCode);
        }

        public Category GetCategoryByCode(string categoryCode)
        {
            return GetCategories().FirstOrDefault(c => c.CodeName == categoryCode);
        }
    }
}