namespace SFA.Infrastructure.Azure.Configuration
{
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Reflection;
    using System.Runtime.Caching; // TODO: Why needed rather than SFA.Infrastructure.Interfaces.Caching?
    using Interfaces;
    using Interfaces.Caching;
    using Microsoft.WindowsAzure.Storage;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    public class AzureBlobConfigurationService : IConfigurationService
    {
        private readonly string _configurationStorageConnectionString;
        private readonly ILogService _loggerService;
        private static readonly string FileVersion = FileVersionInfo.GetVersionInfo(Assembly.GetAssembly(typeof(AzureBlobConfigurationService)).Location).FileVersion;
        private static readonly string CacheKey = $"Configuration_{FileVersion}";
        private static readonly string BlobPath = $"faa/{FileVersion}/settings.json";
        private readonly ObjectCache _cache;
        private readonly object _locker = new object();

        public AzureBlobConfigurationService(string configurationStorageConnectionString, ILogService loggerService)
        {
            _configurationStorageConnectionString = configurationStorageConnectionString;
            _loggerService = loggerService;
            _cache = MemoryCache.Default;
        }

        public TSettings Get<TSettings>() where TSettings : class
        {
            var json = GetJson();

            var settingName = typeof (TSettings).Name;
            string elementJson;

            try
            {
                using (TextReader sr = new StringReader(json))
                {
                    var settingsObject = (JObject) JToken.ReadFrom(new JsonTextReader(sr));
                    var settingsElement = settingsObject.GetValue(settingName);
                    elementJson = settingsElement.ToString();
                }
            }
            catch (Exception ex)
            {
                _loggerService.Error("Error deserialising", ex);
                throw;
            }

            var tsetting = JsonConvert.DeserializeObject<TSettings>(elementJson);
            return tsetting;
        }

        private string GetJson()
        {
            lock (_locker)
            {
                var json = (string)_cache.Get(CacheKey);
                
                if (!string.IsNullOrEmpty(json)) return json;
                
                _loggerService.Debug("Loading configuration from Azure Blob Storage");

                if (string.IsNullOrWhiteSpace(_configurationStorageConnectionString))
                {
                    _loggerService.Warn("ConfigurationStorageConnectionString config setting null, can't load config");
                    return null;
                }

                var storageAccount = CloudStorageAccount.Parse(_configurationStorageConnectionString);

                var blobClient = storageAccount.CreateCloudBlobClient();

                var container = blobClient.GetContainerReference("configs");

                var blockBlob = container.GetBlockBlobReference(BlobPath);

                json = blockBlob.DownloadText();

                if (string.IsNullOrWhiteSpace(json))
                {
                    _loggerService.Error("Failed to load configuration from Azure Blob Storage");
                    return null;
                }

                var cacheTimeSpan = TimeSpan.FromMinutes((int)CacheDuration.OneMinute);
                var policy = new CacheItemPolicy { AbsoluteExpiration = DateTimeOffset.Now.Add(cacheTimeSpan) };
                _cache.Add(CacheKey, json, policy);
                return json;
            }
        }
    }
}
