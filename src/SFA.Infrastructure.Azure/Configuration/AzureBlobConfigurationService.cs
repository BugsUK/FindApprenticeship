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
        private readonly IConfigurationManager _configurationManager;
        private readonly ILogService _loggerService;
        private static readonly string FileVersion = FileVersionInfo.GetVersionInfo(Assembly.GetAssembly(typeof(AzureBlobConfigurationService)).Location).FileVersion;
        private static readonly string CacheKey = string.Format("Configuration_{0}", FileVersion);
        private static readonly string BlobPath = string.Format("faa/{0}/settings.json", FileVersion);
        private readonly ObjectCache _cache;
        private readonly object _locker = new object();

        public AzureBlobConfigurationService(IConfigurationManager configurationManager, ILogService loggerService)
        {
            _configurationManager = configurationManager;
            _loggerService = loggerService;
            _cache = MemoryCache.Default;
        }

        public TSettings Get<TSettings>() where TSettings : class
        {
            var json = GetJson();

            var settingName = typeof(TSettings).Name;
            string elementJson;

            try
            {
                using (TextReader sr = new StringReader(json))
                {
                    var settingsObject = (JObject)JToken.ReadFrom(new JsonTextReader(sr));
                    var settingsElement = settingsObject.GetValue(settingName);
                    if (settingsElement == null)
                    {
                        var message = $"Could not find configuration for {settingName} in Azure Blob Storage. Have you loaded the correct configuration?";
                        _loggerService.Error(message);
                        throw new MissingConfigurationException(message);
                    }
                    elementJson = settingsElement.ToString();
                }
            }
            catch (MissingConfigurationException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _loggerService.Error($"Error getting {settingName}", ex);
                throw new Exception($"Error getting {settingName}", ex);
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

                var storageConnectionString = _configurationManager.GetAppSetting<string>("ConfigurationStorageConnectionString");

                if (string.IsNullOrWhiteSpace(storageConnectionString))
                {
                    _loggerService.Warn("ConfigurationStorageConnectionString config setting null, can't load config");
                    return null;
                }

                var storageAccount = CloudStorageAccount.Parse(storageConnectionString);

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

        private class MissingConfigurationException : Exception
        {
            public MissingConfigurationException(string message) : base(message) { }
        }
    }
}
