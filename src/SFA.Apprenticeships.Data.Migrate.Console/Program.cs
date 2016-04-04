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
    using System.Collections.Generic;

    class Program
    {
        static void Main(string[] args)
        {
            var log = new ConsoleLogService();

            log.Info("Initialisation");

            var options = new Options();
            if (!CommandLine.Parser.Default.ParseArguments(args, options))
            {
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
                if (options.MaxNumberOfChangesToDetailPerTable == null)
                    options.MaxNumberOfChangesToDetailPerTable = persistentConfig.MaxNumberOfChangesToDetailPerTable;
                if (options.AnonymiseData == false)
                    options.AnonymiseData = persistentConfig.AnonymiseData;
            }

            if (options.Verbose)
            {
                Console.WriteLine($@"
SourceConnectionString             = {options.SourceConnectionString}
TargetConnectionString             = {options.TargetConnectionString}
RecordBatchSize                    = {options.RecordBatchSize}
MaxNumberOfChangesToDetailPerTable = {options.MaxNumberOfChangesToDetailPerTable}
AnonymiseData                      = {options.AnonymiseData}
SingleFullUpdate                   = {options.SingleFullUpdate}
");
            }

            var sourceDatabase = new GetOpenConnectionFromConnectionString(options.SourceConnectionString);
            var targetDatabase = new GetOpenConnectionFromConnectionString(options.TargetConnectionString);

            var genericSyncRepository = new GenericSyncRespository(log, sourceDatabase, targetDatabase);
            var avmsSyncRepository = new AvmsSyncRespository(log, sourceDatabase, targetDatabase);

            Func<ITableSpec, IMutateTarget> createMutateTarget;
            if (options.DummyRun)
                createMutateTarget = tableSpec => new DummyMutateTarget(log, tableSpec);
            else
                createMutateTarget = tableSpec => new MutateTarget(log, genericSyncRepository, (int)Math.Max(5000 * tableSpec.BatchSizeMultiplier, 1), tableSpec);

            var tables = new AvmsToAvmsPlusTables(log, options, avmsSyncRepository, !options.ExcludeVacancy).All;

            if (options.SingleTable != null)
                tables = tables.Where(t => t.Name == options.SingleTable);

            var controller = new Controller(options, log, genericSyncRepository, createMutateTarget, tables);

            if (options.Reset)
                controller.Reset();

            var cancellationTokenSource = new System.Threading.CancellationTokenSource();

            bool threaded = true;

            if (options.SingleFullUpdate)
                controller.DoFullScanForAll(threaded);
            else
                controller.DoAll(new System.Threading.CancellationTokenSource(), threaded);
        }
    }

    internal class Options : IMigrateConfiguration
    {
        [Option('s', "SourceConnectionString", HelpText = "e.g. Data Source=10.1.2.3;initial catalog=Avms;user id=<readonlyuser>;password=<password>")]
        public string SourceConnectionString { get; set; }

        [Option('t', "TargetConnectionString", HelpText = "e.g. Data Source=(local)\\SQLEXPRESS;Initial Catalog=AvmsPlus;Integrated Security=True")]
        public string TargetConnectionString { get; set; }

        [Option('b', "RecordBatchSize", HelpText = "The number of records to load at once when compare data. e.g. 15000")]
        public int RecordBatchSize { get; set; }

        [Option('r', "Reset", HelpText = "Reset (clear) the target database before starting. DummyRun option does not apply to the clearing of the database")]
        public bool Reset { get; set; }

        [Option("SingleTable", HelpText = "Ignore the built in configuration of which tables to synchronise and instead sychronise the single table specified, e.g. Vacancy")]
        public string SingleTable { get; set; }

        [Option("MaxNumberOfChangesToDetailPerTable", HelpText = "Detailing every change found can create a large amount of logging / slow the system down. Set this parameter to limit the number of changes that will be detailed.")]
        public int? MaxNumberOfChangesToDetailPerTable { get; set; }

        [Option('a', "AnonymiseData", HelpText = "Whether to anonymise personal data during the transfer.")]
        public bool AnonymiseData { get; set; }

        [Option("SingleFullUpdate", HelpText = "Update all of the tables in the destination (ignoring change tracking except to update the last state) once then exit")]
        public bool SingleFullUpdate { get; set; }

        [Option("DummyRun", HelpText = "Don't make any changes (doesn't apply to Reset option). Can be useful to combine with -v / --Verbose to see configuration in Azure")]
        public bool DummyRun { get; set; }

        [Option("ExcludeVacancy", HelpText = "Exclude Vacancy and related tables (for when only reference data is required)")]
        public bool ExcludeVacancy { get; set; }

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
