namespace SFA.Apprenticeships.Data.Migrate
{
    using SFA.Infrastructure.Interfaces;
    using System;
    using System.Collections.Generic;

    public class DummyMutateTarget : IMutateTarget
    {
        private ILogService _log;
        private ITableDetails _table;

        private int _insertCount;
        private int _updateCount;
        private int _unchangedCount;

        public DummyMutateTarget(ILogService log, ITableDetails table)
        {
            _log = log;
            _table = table;
            _insertCount = 0;
            _updateCount = 0;
            _unchangedCount = 0;
        }

        public void Insert(dynamic record)
        {
            if (_insertCount++ == 0)
                _log.Info($"First insert into {_table.Name}: {record}");
        }

        public void Update(dynamic record)
        {
            if (_updateCount++ == 0)
                _log.Info($"First update of {_table.Name}: {record}");
        }

        public void NoChange(dynamic record)
        {
            if (_unchangedCount++ == 0)
                _log.Info($"First no change of {_table.Name}: {record}");
        }

        public void Dispose()
        {
            _log.Info($"Summary for {_table.Name} : {_insertCount} inserts, {_updateCount} updates, {_unchangedCount} unchanged");
        }
    }
}
