using SFA.Apprenticeships.Infrastructure.Repositories.Sql.Common;
using SFA.Infrastructure.Azure.Configuration;
using SFA.Infrastructure.Console;
using SFA.Infrastructure.Interfaces;
using System;

namespace SFA.Apprenticeships.Data.Migrate.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            var log = new ConsoleLogService();

            log.Info("IoC initialisation");

            var configService = new AzureBlobConfigurationService(new MyConfigurationManager(), log);

            var config = configService.Get<MigrateFromAvmsConfiguration>();

            var sourceDatabase = new GetOpenConnectionFromConnectionString(config.SourceConnectionString);
            var targetDatabase = new GetOpenConnectionFromConnectionString(config.TargetConnectionString);
            var syncRepository = new SyncRespository(log, sourceDatabase, targetDatabase);

            var controller = new Controller(
                configService.Get<MigrateFromAvmsConfiguration>(),
                log,
                syncRepository,
                //tableSpec => new DummyMutateTarget(log, tableSpec),
                tableSpec => new MutateTarget(log, syncRepository, 5000, tableSpec), // TODO: 5000 or 1
                new AvmsToAvmsPlusTables(log).All
                );

            //controller.Reset(); // TODO: Remove
            controller.DoAll();
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
