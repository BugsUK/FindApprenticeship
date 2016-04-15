namespace SFA.Apprenticeships.Infrastructure.Migrate.Faa
{
    using System;
    using System.Diagnostics;
    using System.Threading;
    using Microsoft.WindowsAzure.ServiceRuntime;
    using StructureMap;
    using Common.IoC;
    using Data.Migrate.Faa;
    using Logging.IoC;
    using SFA.Infrastructure.Interfaces;
    using Logging;

    public class WorkerRole : RoleEntryPoint
    {
        private const string ProcessName = "Migrate.Faa";

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
            var processor = new MigrationProcessor(configService, _logService);

            try
            {
                processor.Execute(_cancelSource.Token);
            }
            catch (Exception ex)
            {
                _logService.Error("Unhandled exception from processor.Execute method", ex);
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