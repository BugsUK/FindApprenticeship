namespace SFA.Apprenticeships.Infrastructure.Common.Configuration
{
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.Caching;
    using SFA.Infrastructure.Interfaces;
    using SFA.Infrastructure.Interfaces.Caching;
    using MongoDB.Driver;
    using MongoDB.Driver.Builders;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    public class ConfigurationService : IConfigurationService
    {
        private readonly IConfigurationManager _configurationManager;
        private readonly ILogService _loggerService;
        private readonly static string FileVersion = FileVersionInfo.GetVersionInfo(Assembly.GetAssembly(typeof(ConfigurationService)).Location).FileVersion;
        private static readonly string CacheKey = string.Format("Configuration_{0}", FileVersion);
        private readonly ObjectCache _cache;
        private readonly object _locker = new object();

        public ConfigurationService(IConfigurationManager configurationManager, ILogService loggerService)
        {
            _configurationManager = configurationManager;
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
                if (string.IsNullOrEmpty(json))
                {
                    _loggerService.Debug("Loading confguration from mongo");

                    var mongoConnectionString = _configurationManager.GetAppSetting<string>("ConfigurationDb");

                    if (string.IsNullOrWhiteSpace(mongoConnectionString))
                    {
                        _loggerService.Warn("ConfigurationDb config setting null, can't load config");
                        return null;
                    }

                    json = LoadJson(mongoConnectionString);

                    if (string.IsNullOrWhiteSpace(json))
                    {
                        _loggerService.Error("Failed to load configuration from mongo");
                        return null;
                    }

                    var cacheTimeSpan = TimeSpan.FromMinutes((int)CacheDuration.OneMinute);
                    var policy = new CacheItemPolicy { AbsoluteExpiration = DateTimeOffset.Now.Add(cacheTimeSpan) };
                    _cache.Add(CacheKey, json, policy);
                }
                return json;
            }
        }

        private string LoadJson(string mongoConnectionString)
        {
            try
            {
                var mongoDbName = MongoUrl.Create(mongoConnectionString).DatabaseName;
                var database = new MongoClient(mongoConnectionString).GetServer().GetDatabase(mongoDbName);
                var collection = database.GetCollection("configuration");

                var query = Query.EQ("DeploymentVersion", FileVersion);
                var settings = collection.Find(query);

                _loggerService.Debug("Loaded confguration from mongo");

                var bson = settings.Single();
                bson.Remove("_id");
                bson.Remove("DeploymentVersion");
                bson.Remove("DateTimeUpdated");
                return bson.ToString();
            }
            catch (Exception ex)
            {
                _loggerService.Error("Failed to load confguration from mongo, either doesn't exist or contains more than 1 entry", ex);
                throw;
            }
        }
    }
}
