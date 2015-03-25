namespace SFA.Apprenticeships.Infrastructure.Common.Configuration
{
    using System;
    using System.IO;
    using System.Linq;
    using Application.Interfaces.Logging;
    using Domain.Interfaces.Configuration;
    using Microsoft.WindowsAzure;
    using MongoDB.Driver;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    public class ConfigurationService : IConfigurationService
    {
        private readonly IConfigurationManager _configurationManager;
        private readonly ILogService _loggerService;

        public ConfigurationService(IConfigurationManager configurationManager, ILogService loggerService)
        {
            _configurationManager = configurationManager;
            _loggerService = loggerService;
        }

        public TSettings Get<TSettings>(string settingName) where TSettings : class
        {
            _loggerService.Debug("Loading confguration from mongo");

            var mongoConnectionString = _configurationManager.GetAppSetting<string>("ConfigurationDb");

            if (string.IsNullOrWhiteSpace(mongoConnectionString))
            {
                _loggerService.Warn("ConfigurationDb config setting null, can't load config");
                return null;
            }

            var json = LoadJson(mongoConnectionString);
            
            if (string.IsNullOrWhiteSpace(json))
            {
                _loggerService.Error("Failed to load configuration from mongo");
                return null;
            }

            string elementJson = null;

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
                _loggerService.Error("Error desrialising", ex);
                throw;
            }

            var tsetting = JsonConvert.DeserializeObject<TSettings>(elementJson);
            return tsetting;
        }

        private string LoadJson(string mongoConnectionString)
        {
            try
            {
                var mongoDbName = MongoUrl.Create(mongoConnectionString).DatabaseName;
                var database = new MongoClient(mongoConnectionString).GetServer().GetDatabase(mongoDbName);
                var collection = database.GetCollection("configuration");
                var settings = collection.FindAll();

                _loggerService.Debug("Loaded confguration from mongo");

                var bson = settings.Single();
                bson.Remove("_id");
                return bson.ToString();
            }
            catch (Exception ex)
            {
                _loggerService.Error("Failed to load confguration from mongo, either doesn't exist or contains more than 1 entry", ex);
                throw;
            }
        }

        public T GetCloudAppSetting<T>(string key)
        {
            var setting = CloudConfigurationManager.GetSetting(key);
            return (T)Convert.ChangeType(setting, typeof(T));
        }
    }
}
