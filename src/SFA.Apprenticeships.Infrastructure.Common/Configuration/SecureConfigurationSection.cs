﻿namespace SFA.Apprenticeships.Infrastructure.Common.Configuration
{
    using System;
    using System.Configuration;
    using System.IO;
    using ConfigurationManager = SFA.Infrastructure.Configuration.ConfigurationManager;

    public abstract class SecureConfigurationSection<T> : ConfigurationSection where T : ConfigurationSection, new()
    {
        private static T _instance;
        private static string _configFile;
        private static string _configSectionName;

        protected SecureConfigurationSection(string configSectionName)
        {
            // If full path specified, i.e. in unit/integration tests
            _configFile = System.Configuration.ConfigurationManager.AppSettings[ConfigurationManager.ConfigurationFileAppSettingName];

            if (!File.Exists(_configFile))
            {
                // Relative path specified, i.e. in Web App for Azure deployment
                _configFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, _configFile);

                if (!File.Exists(_configFile))
                {
                    throw new FileNotFoundException("Config file does not exist", _configFile);
                }
            }

            _configSectionName = configSectionName;
        }

        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
// ReSharper disable UnusedVariable
                    var initialiser = new T(); //note: do not remove... required to initialise string fields
// ReSharper restore UnusedVariable
                    var configMap = new ExeConfigurationFileMap {ExeConfigFilename = _configFile};
                    var config = System.Configuration.ConfigurationManager.OpenMappedExeConfiguration(configMap, ConfigurationUserLevel.None);

                    _instance = (T) config.GetSection(_configSectionName);
                }

                return _instance;
            }
        }
    }
}
