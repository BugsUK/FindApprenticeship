namespace SFA.Apprenticeships.Data.Migrate
{
    using SFA.Infrastructure.Interfaces;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using SFA.Apprenticeships.Application.Interfaces;

    /// <summary>
    /// Simple, single threaded synchronous implementation
    /// </summary>
    public class MutateTarget : BaseMutateTarget, IMutateTarget
    {
        private IGenericSyncRespository _syncRepository;
        private int _maxBatchSize;

        private List<dynamic> _toInsert;
        private List<dynamic> _toUpdate;
        private List<Keys> _toDelete;

        public MutateTarget(ILogService log, IGenericSyncRespository syncRepository, int maxBatchSize, ITableDetails tableDetails) : base(log, tableDetails)
        {
            _syncRepository = syncRepository;
            _maxBatchSize = maxBatchSize;
            _tableDetails = tableDetails;

            _toInsert = new List<dynamic>();
            _toUpdate = new List<dynamic>();
            _toDelete = new List<Keys>();
        }

        public void Insert(dynamic record)
        {
            _toInsert.Add(record);
            NumberOfInserts++;
            FlushInsert(false);
        }

        public void Update(dynamic record)
        {
            _toUpdate.Add(record);
            NumberOfUpdates++;
            FlushUpdate(false);
        }

        public void Delete(dynamic record)
        {
            _toDelete.Add(Keys.GetPrimaryKeys(record, _tableDetails));
            NumberOfDeletes++;
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
                _syncRepository.BulkInsert(_tableDetails, _toInsert.Select(r => (IDictionary<string, object>)r));
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
                _syncRepository.BulkUpdate(_tableDetails, _toUpdate.Select(r => (IDictionary<string, object>)r));
            }
            finally
            {
                _toUpdate.Clear();
            }
        }

        public void FlushInsertsAndUpdates()
        {
            _log.Info(SummaryText);
            FlushUpdate(true);
            FlushInsert(true);
        }

        public void FlushDeletes()
        {
            if (_toDelete.Count == 0)
                return;

            _log.Info(SummaryTextDeletes);
            _syncRepository.BulkDelete(_tableDetails, _toDelete);
        }
    }
}
