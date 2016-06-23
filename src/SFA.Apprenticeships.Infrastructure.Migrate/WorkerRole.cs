namespace SFA.Apprenticeships.Infrastructure.Migrate
{
    using System;
    using System.Diagnostics;
    using System.Threading;
    using Microsoft.WindowsAzure.ServiceRuntime;
    using StructureMap;
    using Common.IoC;
    using Logging.IoC;
    using SFA.Infrastructure.Interfaces;
    using Logging;
    using Data.Migrate;
    using SFA.Apprenticeships.Infrastructure.Repositories.Sql.Common;

    public class WorkerRole : RoleEntryPoint
    {
        private const string ProcessName = "Migrate";

        private readonly CancellationTokenSource _cancelSource = new CancellationTokenSource();

        private Container _container;
        private ILogService _logService;

        public override bool OnStart()
        {
            Initialise();

            return base.OnStart();
        }

        public override void Run()
        {
            var configService = _container.GetInstance<IConfigurationService>();
            var options = configService.Get<MigrateFromAvmsConfiguration>();

            var sourceDatabase = new GetOpenConnectionFromConnectionString(options.SourceConnectionString);
            var targetDatabase = new GetOpenConnectionFromConnectionString(options.TargetConnectionString);

            var genericSyncRepository = new GenericSyncRespository(_logService, sourceDatabase, targetDatabase);
            var avmsSyncRepository = new AvmsSyncRespository(_logService, sourceDatabase, targetDatabase);

            var tables = new AvmsToAvmsPlusTables(_logService, options, avmsSyncRepository, true).All;

            var controller = new Controller(
                options,
                _logService,
                genericSyncRepository,
                tableSpec => new MutateTarget(_logService, genericSyncRepository, (int)Math.Max(5000 * tableSpec.BatchSizeMultiplier, 1), tableSpec),
                tables
                );

            try
            {
                controller.DoAll(_cancelSource);
            }
            catch (Exception ex)
            {
                _logService.Error("Unhandled exception from controller.DoAll method", ex);
                throw;
            }
        }

        public override void OnStop()
        {
            Stop();
            _cancelSource.Cancel();

            base.OnStop();
        }

        private void Initialise()
        {
            try
            {
                VersionLogging.SetVersion();

                InitializeIoC();
            }
            catch (Exception e)
            {
                if (_logService == null)
                {
                    Trace.TraceError($"{ProcessName} failed to initialise: \"{e}\"");
                }
                else
                {
                    _logService.Error($"{ProcessName} failed to initialise");
                }

                throw;
            }
        }

        private void InitializeIoC()
        {
            _container = new Container(x =>
            {
                x.AddRegistry<CommonRegistry>();
                x.AddRegistry<LoggingRegistry>();
            });

            _logService = _container.GetInstance<ILogService>();
        }

        private void Start()
        {
            _logService.Info($"Starting {ProcessName}");

            _logService.Info($"Started {ProcessName}");
        }

        private void Stop()
        {
            _logService.Info($"Stopping {ProcessName}");

            _logService.Info($"Stopped {ProcessName}");
        }
    }
}