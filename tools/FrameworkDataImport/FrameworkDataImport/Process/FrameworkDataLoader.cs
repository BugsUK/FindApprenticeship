namespace FrameworkDataImport.Process
{
    using System;
    using System.Collections.Generic;
    using Nest;
    using SFA.Apprenticeships.Domain.Entities.ReferenceData;

    public class FrameworkDataLoader : IFrameworkDataLoader
    {
        private const string IndexName = "categories";

        private readonly Uri _elasticUri;

        public FrameworkDataLoader(Uri elasticUri)
        {
            _elasticUri = elasticUri;
        }

        public void Load(List<Category> categories)
        {
            var settings = new ConnectionSettings(_elasticUri);
            settings.SetDefaultIndex(IndexName);

            var client = new ElasticClient(settings);

            if (client.IndexExists(IndexName).Exists)
            {
                client.DeleteIndex(IndexName);
            }

            client.CreateIndex(i => i.Index(IndexName));

            foreach (var category in categories)
            {
                client.Index(category);
            }
        }
    }
}