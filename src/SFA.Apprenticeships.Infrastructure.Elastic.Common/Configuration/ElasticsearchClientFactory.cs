namespace SFA.Apprenticeships.Infrastructure.Elastic.Common.Configuration
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using SFA.Infrastructure.Interfaces;
    using Nest;
    using Newtonsoft.Json.Converters;

    public class ElasticsearchClientFactory : IElasticsearchClientFactory
    {
        private readonly ConnectionSettings _connectionSettings;
        private readonly Dictionary<Type, string> _typeIndexNameMap = new Dictionary<Type, string>();
        private readonly Dictionary<Type, string> _documentTypeNameMap = new Dictionary<Type, string>();

        public ElasticsearchClientFactory(IConfigurationService configurationService, ILogService logService)
        {
            var elasticsearchConfiguration = configurationService.Get<SearchConfiguration>();

            foreach (var index in elasticsearchConfiguration.Indexes)
            {
                var mappingType = Type.GetType(index.MappingType);

                if (mappingType == null)
                {
                    var error = string.Format("Mapping type not not valid: {0}", index.MappingType);
                    logService.Error(error);
                    throw new ArgumentException(error);
                }

                _typeIndexNameMap.Add(mappingType, index.Name);

                _documentTypeNameMap.Add(mappingType,
                    ((ElasticTypeAttribute)
                        mappingType.GetCustomAttributes(typeof(ElasticTypeAttribute), false)
                            .First()).Name);
            }

            _connectionSettings = new ConnectionSettings(new Uri(elasticsearchConfiguration.HostName));
            _connectionSettings.AddContractJsonConverters(t => typeof(Enum).IsAssignableFrom(t) ? new StringEnumConverter() : null);
        }

        public ElasticClient GetElasticClient()
        {
            var client = new ElasticClient(_connectionSettings);
            return client;
        }

        public string GetIndexNameForType(Type attributeMappedType)
        {
            return _typeIndexNameMap[attributeMappedType];
        }

        public string GetDocumentNameForType(Type attributeMappedType)
        {
            return _documentTypeNameMap[attributeMappedType];
        }
    }
}
