

namespace SFA.Apprenticeships.Data.Migrate.Console
{
    using CommandLine;
    using CommandLine.Text;
    using SFA.Apprenticeships.Infrastructure.Repositories.Sql.Common;
    using SFA.Infrastructure.Azure.Configuration;
    using SFA.Infrastructure.Console;
    using SFA.Infrastructure.Interfaces;
    using System;
    using System.Linq;

    class Program
    {
        static void Main(string[] args)
        {
            var log = new ConsoleLogService();

            log.Info("Initialisation");

            var options = new Options();
            if (!CommandLine.Parser.Default.ParseArguments(args, options))
            {
                log.Info("Here");
                return;
            }

            if (options.SourceConnectionString == null || options.TargetConnectionString == null || options.RecordBatchSize == 0)
            {
                var configService = new AzureBlobConfigurationService(new MyConfigurationManager(), log);
                var persistentConfig = configService.Get<MigrateFromAvmsConfiguration>();

                if (options.SourceConnectionString == null)
                    options.SourceConnectionString = persistentConfig.SourceConnectionString;
                if (options.TargetConnectionString == null)
                    options.TargetConnectionString = persistentConfig.TargetConnectionString;
                if (options.RecordBatchSize == 0)
                    options.RecordBatchSize = persistentConfig.RecordBatchSize;
            }

            if (options.Verbose)
            {
                Console.WriteLine($"SourceConnectionString={options.SourceConnectionString}");
                Console.WriteLine($"TargetConnectionString={options.TargetConnectionString}");
                Console.WriteLine($"RecordBatchSize={options.RecordBatchSize}");
            }


            var sourceDatabase = new GetOpenConnectionFromConnectionString(options.SourceConnectionString);
            var targetDatabase = new GetOpenConnectionFromConnectionString(options.TargetConnectionString);

            var genericSyncRepository = new GenericSyncRespository(log, sourceDatabase, targetDatabase);
            var avmsSyncRepository = new AvmsSyncRespository(log, sourceDatabase, targetDatabase);

            var tables = new AvmsToAvmsPlusTables(log, avmsSyncRepository, true).All;

            if (options.SingleTable != null)
                tables = tables.Where(t => t.Name == options.SingleTable);

            var controller = new Controller(
                options,
                log,
                genericSyncRepository,
                //tableSpec => new DummyMutateTarget(log, tableSpec),
                tableSpec => new MutateTarget(log, genericSyncRepository, 5000, tableSpec),
                tables
                );

            if (options.Reset)
                controller.Reset();
            controller.DoAll();
        }
    }

    internal class Options : IMigrateConfiguration
    {
        [Option('s', "SourceConnectionString", Required=false, HelpText = "e.g. Data Source=10.1.2.3;initial catalog=Avms;user id=<readonlyuser>;password=<password>")]
        public string SourceConnectionString { get; set; }

        [Option('t', "TargetConnectionString", HelpText = "e.g. Data Source=(local)\\SQLEXPRESS;Initial Catalog=AvmsPlus;Integrated Security=True")]
        public string TargetConnectionString { get; set; }

        [Option('b', "RecordBatchSize", HelpText = "The number of records to load at once when compare data. e.g. 15000")]
        public int RecordBatchSize { get; set; }

        [Option('r', "Reset", HelpText = "Reset (clear) the target database before starting")]
        public bool Reset { get; set; }

        [Option("SingleTable", HelpText = "Ignore the built in configuration of which tables to synchronise and instead sychronise the single table specified, e.g. Vacancy")]
        public string SingleTable { get; set; }

        [Option('v', "Verbose", HelpText = "Verbose. WILL USUALLY WRITE PASSWORDS TO OUTPUT")]
        public bool Verbose { get; set; }


        [HelpOption]
        public string GetUsage()
        {
            return HelpText.AutoBuild(this,
              current => HelpText.DefaultParsingErrorsHandler(this, current));
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
