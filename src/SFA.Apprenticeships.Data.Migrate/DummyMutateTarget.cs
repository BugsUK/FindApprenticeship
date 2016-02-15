namespace SFA.Apprenticeships.Data.Migrate
{
    using SFA.Infrastructure.Interfaces;
    using System;
    using System.Collections.Generic;

    public class DummyMutateTarget : IMutateTarget
    {
        private ILogService _log;

        public DummyMutateTarget(ILogService log)
        {
            _log = log;
        }

        public void Insert(dynamic record)
        {
            _log.Info($"Inserting {record}");
        }

        public void Update(dynamic record)
        {
            _log.Info($"Updating {record}");
        }

        public void Dispose()
        {
        }
    }
}
