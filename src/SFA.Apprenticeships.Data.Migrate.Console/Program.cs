﻿using SFA.Apprenticeships.Infrastructure.Repositories.Sql.Common;
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

            var genericSyncRepository = new GenericSyncRespository(log, sourceDatabase, targetDatabase);
            var avmsSyncRepository = new AvmsSyncRespository(log, sourceDatabase, targetDatabase);

            var controller = new Controller(
                configService.Get<MigrateFromAvmsConfiguration>(),
                log,
                genericSyncRepository,
                //tableSpec => new DummyMutateTarget(log, tableSpec),
                tableSpec => new MutateTarget(log, genericSyncRepository, 5000, tableSpec), // TODO: 5000 or 1
                new AvmsToAvmsPlusTables(log, avmsSyncRepository, true).All
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
