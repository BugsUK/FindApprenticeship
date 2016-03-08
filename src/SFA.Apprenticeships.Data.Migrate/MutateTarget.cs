namespace SFA.Apprenticeships.Data.Migrate
{
    using SFA.Infrastructure.Interfaces;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Simple, single threaded synchronous implementation
    /// </summary>
    public class MutateTarget : IMutateTarget
    {
        private ILogService _log;
        private IGenericSyncRespository _syncRepository;
        private int _maxBatchSize;
        private ITableDetails _tableDetails;

        private List<dynamic> _toInsert;
        private List<dynamic> _toUpdate;

        private int _insertCount;
        public int NumberOfUpdates { get; private set; }
        private int _unchangedCount;

        public MutateTarget(ILogService log, IGenericSyncRespository syncRepository, int maxBatchSize, ITableDetails tableDetails)
        {
            _log = log;
            _syncRepository = syncRepository;
            _maxBatchSize = maxBatchSize;
            _tableDetails = tableDetails;

            _toInsert = new List<dynamic>();
            _toUpdate = new List<dynamic>();

            _insertCount = 0;
            NumberOfUpdates = 0;
            _unchangedCount = 0;
        }

        public void Insert(dynamic record)
        {
            //if (_maxBatchSize == 1)
            //    _log.Debug($"Queuing for insert: {record}");
            _toInsert.Add(record);
            _insertCount++;
            FlushInsert(false);
        }

        public void Update(dynamic record)
        {
            //if (_maxBatchSize == 1)
            //    _log.Debug($"Queuing for update: {record}");
            _toUpdate.Add(record);
            NumberOfUpdates++;
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

            if (_toInsert.Count == 1)
            {
                try
                {
                    _syncRepository.InsertSingle(_tableDetails, _toInsert.Single());
                }
                catch (Exception ex) // SqlException
                {
                    if (ex.Message.Contains("FOREIGN KEY"))
                    {
                        _log.Error($"Hit foreign key constraint on {_tableDetails.Name}.\nData: {_toInsert.Single()}.\nDetail: {ex.Message}.");
                    }
                    throw;
                }
            }
            else
            {
                try
                {
                    _syncRepository.BulkInsert(_tableDetails, _toInsert.AsReadOnly());
                }
                catch (Exception ex) // SqlException
                {
                    if (ex.Message.Contains("FOREIGN KEY"))
                    {
                        _log.Info($"Hit foreign key constraint error on {_tableDetails.Name}. This may be genuine or due to a known bug in SqlBulkCopy with NULL foreign keys. Splitting the operation to identify the affected record.");
                        using (var m = new MutateTarget(_log, _syncRepository, Math.Max(1, _maxBatchSize / 4), _tableDetails))
                        {
                            foreach (var item in _toInsert)
                                m.Insert(item);
                        }
                    }
                    else
                    {
                        throw;
                    }
                }
            }

            _toInsert.Clear();
        }

        private void FlushUpdate(bool force)
        {
            if ((!force && _toUpdate.Count < _maxBatchSize) || _toUpdate.Count == 0)
                return;
            _log.Info($"Updating {_toUpdate.Count} records on {_tableDetails.Name}");
            _syncRepository.BulkUpdate(_tableDetails, _toUpdate.AsReadOnly());
            _toUpdate.Clear();
        }

        public void Dispose()
        {
            FlushInsert(true);
            FlushUpdate(true);
            _log.Info($"Summary for {_tableDetails.Name}: {_insertCount} inserts, {NumberOfUpdates} updates, {_unchangedCount} unchanged");
        }

    }
}
