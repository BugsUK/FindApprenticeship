namespace SFA.Apprenticeships.Data.Migrate
{
    using SFA.Infrastructure.Interfaces;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class Controller
    {
        private IConfigurationService _config;
        private ILogService _log;

        public Controller(IConfigurationService config, ILogService log)
        {
            _config = config;
            _log = log;
        }

        public void DoAll()
        {
            _log.Info("DoAll Started");


            _log.Info("DoAll Finished");
        }
    }
}
