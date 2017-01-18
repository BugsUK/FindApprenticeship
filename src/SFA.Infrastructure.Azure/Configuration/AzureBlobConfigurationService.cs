namespace SFA.Infrastructure.Azure.Configuration
{
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Reflection;
    using System.Runtime.Caching; // TODO: Why needed rather than SFA.Infrastructure.Interfaces.Caching?
    using Interfaces;
    using Microsoft.WindowsAzure.Storage;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using Apprenticeships.Application.Interfaces;
    using Apprenticeships.Application.Interfaces.Caching;

    public class AzureBlobConfigurationService : IConfigurationService
    {
        private readonly IConfigurationManager _configurationManager;
        private static readonly string FileVersion = FileVersionInfo.GetVersionInfo(Assembly.GetAssembly(typeof(AzureBlobConfigurationService)).Location).FileVersion;
        private static readonly string CacheKey = string.Format("Configuration_{0}", FileVersion);
        private static readonly string BlobPath = string.Format("faa/{0}/settings.json", FileVersion);
        private readonly ObjectCache _cache;
        private readonly object _locker = new object();

        public AzureBlobConfigurationService(IConfigurationManager configurationManager)
        {
            _configurationManager = configurationManager;
            _cache = MemoryCache.Default;
        }

        public TSettings Get<TSettings>() where TSettings : class
        {
            var settingsName = typeof(TSettings).Name;
            var elementJson = Get(settingsName);
            var tsetting = JsonConvert.DeserializeObject<TSettings>(elementJson);
            return tsetting;
        }

        public object Get(Type settingsType)
        {
            var settingsName = settingsType.Name;
            var elementJson = Get(settingsName);
            var tsetting = JsonConvert.DeserializeObject(elementJson, settingsType);
            return tsetting;
        }

        private string Get(string settingsName)
        {
            var json = GetJson();

            string elementJson;

            try
            {
                using (TextReader sr = new StringReader(json))
                {
                    var settingsObject = (JObject)JToken.ReadFrom(new JsonTextReader(sr));
                    var settingsElement = settingsObject.GetValue(settingsName);
                    if (settingsElement == null)
                    {
                        var message = $"Could not find configuration for {settingsName} in Azure Blob Storage. Have you loaded the correct configuration?";
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
                throw new Exception($"Error getting {settingsName}", ex);
            }

            return elementJson;
        }

        private string GetJson()
        {
            lock (_locker)
            {
                var json = (string)_cache.Get(CacheKey);

                if (!string.IsNullOrEmpty(json)) return json;

                var storageConnectionString = _configurationManager.GetAppSetting<string>("ConfigurationStorageConnectionString");

                if (string.IsNullOrWhiteSpace(storageConnectionString))
                {
                    return null;
                }

                var storageAccount = CloudStorageAccount.Parse(storageConnectionString);

                var blobClient = storageAccount.CreateCloudBlobClient();

                var container = blobClient.GetContainerReference("configs");

                var blockBlob = container.GetBlockBlobReference(BlobPath);

                json = blockBlob.DownloadText();

                if (string.IsNullOrWhiteSpace(json))
                {
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
