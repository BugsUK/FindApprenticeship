namespace SFA.Apprenticeships.Infrastructure.Common.Configuration
{
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using Application.Interfaces.Logging;
    using Domain.Entities.Locations;
    using Domain.Interfaces.Configuration;
    using MongoDB.Driver;
    using MongoDB.Driver.Builders;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    public class ConfigurationService : IConfigurationService
    {
        private readonly IConfigurationManager _configurationManager;
        private readonly ILogService _loggerService;
        private readonly static string FileVersion = FileVersionInfo.GetVersionInfo(Assembly.GetAssembly(typeof(ConfigurationService)).Location).FileVersion;

        public ConfigurationService(IConfigurationManager configurationManager, ILogService loggerService)
        {
            _configurationManager = configurationManager;
            _loggerService = loggerService;
        }

        public TSettings Get<TSettings>() where TSettings : class
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

            string settingName = typeof (TSettings).Name;
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
