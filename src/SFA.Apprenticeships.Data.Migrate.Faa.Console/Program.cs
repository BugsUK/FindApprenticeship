namespace SFA.Apprenticeships.Data.Migrate.Faa.Console
{
    using Infrastructure.Azure.Configuration;
    using Infrastructure.Console;
    using Infrastructure.Interfaces;
    using System;

    public class Program
    {
        public static void Main(string[] args)
        {
            var log = new ConsoleLogService();
            var configService = new AzureBlobConfigurationService(new MyConfigurationManager(), log);

            var processor = new MigrationProcessor(configService, log);

            processor.Execute(new System.Threading.CancellationTokenSource().Token);

            Console.ReadKey();
        }
    }

    internal class MyConfigurationManager : IConfigurationManager
    {
        public string ConfigurationFilePath
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public string GetAppSetting(string key)
        {
            throw new NotImplementedException();
        }

        public T GetAppSetting<T>(string key)
        {
            if (key == "ConfigurationStorageConnectionString")
                return (T)Convert.ChangeType("UseDevelopmentStorage=true", typeof(T));

            throw new NotImplementedException();
        }

        public string TryGetAppSetting(string key)
        {
            throw new NotImplementedException();
        }
    }
}
