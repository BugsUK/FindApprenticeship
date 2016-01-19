namespace SFA.Infrastructure.Console
{
    using System;
    using Newtonsoft.Json;
    using System.Collections.Generic;
    using Interfaces;

    public class ConsoleConfigurationService : IConfigurationService
    {
        private IDictionary<string, object> _cache = new Dictionary<string, object>();

        public TSettings Get<TSettings>() where TSettings : class
        {
            string nameOfSetting = typeof(TSettings).ToString();

            object value;
            if (!_cache.TryGetValue(nameOfSetting, out value))
            {
                Console.WriteLine("Please provide JSON for " + nameOfSetting);
                var json = Console.ReadLine();
                value = JsonConvert.DeserializeObject<TSettings>(json);
                _cache.Add(nameOfSetting, value);
            }

            return (TSettings)value;
        }
    }
}
