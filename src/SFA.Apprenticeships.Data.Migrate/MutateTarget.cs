namespace SFA.Apprenticeships.Data.Migrate
{
    using SFA.Infrastructure.Interfaces;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Simple, single threaded synchronous implementation
    /// </summary>
    public class MutateTarget : BaseMutateTarget, IMutateTarget
    {
        private IGenericSyncRespository _syncRepository;
        private int _maxBatchSize;

        private List<dynamic> _toInsert;
        private List<dynamic> _toUpdate;

        public MutateTarget(ILogService log, IGenericSyncRespository syncRepository, int maxBatchSize, ITableDetails tableDetails) : base(log, tableDetails)
        {
            _syncRepository = syncRepository;
            _maxBatchSize = maxBatchSize;
            _tableDetails = tableDetails;

            _toInsert = new List<dynamic>();
            _toUpdate = new List<dynamic>();
        }

        public void Insert(dynamic record)
        {
            //if (_maxBatchSize == 1)
            //    _log.Debug($"Queuing for insert: {record}");
            _toInsert.Add(record);
            NumberOfInserts++;
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

        public void Delete(dynamic record)
        {
            throw new NotImplementedException();
        }

        public void NoChange(dynamic record)
        {
            NumberUnchanged++;
        }

        private void FlushInsert(bool force)
        {
            if ((!force && _toInsert.Count < _maxBatchSize) || _toInsert.Count == 0)
                return;
            
            try
            {
                _log.Info($"Inserting {_toInsert.Count} records into {_tableDetails.Name}");
                _syncRepository.BulkInsert(_tableDetails, _toInsert.AsReadOnly());
            }
            finally
            {
                _toInsert.Clear();
            }
        }

        private void FlushUpdate(bool force)
        {
            if ((!force && _toUpdate.Count < _maxBatchSize) || _toUpdate.Count == 0)
                return;

            try
            {
                _log.Info($"Updating {_toUpdate.Count} records on {_tableDetails.Name}");
                _syncRepository.BulkUpdate(_tableDetails, _toUpdate.AsReadOnly());
            }
            finally
            {
                _toUpdate.Clear();
            }
        }

        public void Dispose()
        {
            FlushInsert(true);
            FlushUpdate(true);
            //FlushDelete(true);
            _log.Info(SummaryText);
        }

    }
}
