namespace SFA.Apprenticeships.Infrastructure.Common.Configuration
{
    using System;
    using System.Globalization;
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

            var mongoConnectionString = _configurationManager.GetAppSetting<string>("Configuration.mongoDB");

            if (string.IsNullOrWhiteSpace(mongoConnectionString))
            {
                _loggerService.Warn("Configuration.mongoDB setting null, loading config from file");
                return default(TSettings);
            }

            var mongoDbName = MongoUrl.Create(mongoConnectionString).DatabaseName;

            var database = new MongoClient(mongoConnectionString).GetServer().GetDatabase(mongoDbName);
            var collection = database.GetCollection("configuration");
            var settings = collection.FindAll();

            _loggerService.Debug("Loaded confguration from mongo");

            var bson = settings.Single();
            bson.Remove("_id");
            string json = bson.ToString();
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
            }

            var tsetting = JsonConvert.DeserializeObject<TSettings>(elementJson);

            return tsetting;
        }

        public T GetCloudAppSetting<T>(string key)
        {
            var setting = CloudConfigurationManager.GetSetting(key);
            return (T)Convert.ChangeType(setting, typeof(T));
        }
    }
}
