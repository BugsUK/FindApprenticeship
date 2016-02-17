namespace SFA.Apprenticeships.Data.Migrate
{
    using SFA.Infrastructure.Interfaces;
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Simple, single threaded synchronous implementation
    /// </summary>
    public class MutateTarget : IMutateTarget
    {
        private ILogService _log;
        private ISyncRespository _syncRepository;
        private int _maxBatchSize;
        private ITableDetails _tableDetails;

        private List<dynamic> _toInsert;
        private List<dynamic> _toUpdate;

        private int _insertCount;
        private int _updateCount;
        private int _unchangedCount;

        public MutateTarget(ILogService log, ISyncRespository syncRepository, int maxBatchSize, ITableDetails tableDetails)
        {
            _log = log;
            _syncRepository = syncRepository;
            _maxBatchSize = maxBatchSize;
            _tableDetails = tableDetails;

            _toInsert = new List<dynamic>();
            _toUpdate = new List<dynamic>();

            _insertCount = 0;
            _updateCount = 0;
            _unchangedCount = 0;
        }

        public void Insert(dynamic record)
        {
            if (_maxBatchSize == 1)
                _log.Debug($"Queuing for insert: {record}");
            _toInsert.Add(record);
            _insertCount++;
            FlushInsert(false);
        }

        public void Update(dynamic record)
        {
            if (_maxBatchSize == 1)
                _log.Debug($"Queuing for update: {record}");
            _toUpdate.Add(record);
            _updateCount++;
            FlushUpdate(false);
        }
        public void NoChange(dynamic record)
        {
            _unchangedCount++;
        }

        private void FlushInsert(bool force)
        {
            if ((!force && _toInsert.Count < _maxBatchSize) || _toInsert.Count == 0)
                return;
            _log.Info($"Inserting {_toInsert.Count} records into {_tableDetails.Name}");
            _syncRepository.BulkInsert(_tableDetails, _toInsert.AsReadOnly());
            _toInsert.Clear();
        }

        private void FlushUpdate(bool force)
        {
            if ((!force && _toUpdate.Count < _maxBatchSize) || _toUpdate.Count ==0)
                return;
            _log.Info($"Updating {_toUpdate.Count} records into {_tableDetails.Name}");
            _syncRepository.BulkUpdate(_tableDetails, _toUpdate.AsReadOnly());
            _toUpdate.Clear();
        }

        public void Dispose()
        {
            FlushInsert(true);
            FlushUpdate(true);
            _log.Info($"Summary for {_tableDetails.Name}: {_insertCount} inserts, {_updateCount} updates, {_unchangedCount} unchanged");
        }

    }
}
