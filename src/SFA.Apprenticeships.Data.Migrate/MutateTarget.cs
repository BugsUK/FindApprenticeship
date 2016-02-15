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

        public MutateTarget(ILogService log, ISyncRespository syncRepository, int maxBatchSize, ITableDetails tableDetails)
        {
            _log = log;
            _syncRepository = syncRepository;
            _maxBatchSize = maxBatchSize;
            _tableDetails = tableDetails;
        }

        public void Insert(dynamic record)
        {
            _log.Debug($"Queuing for insert: {record}");
            _toInsert.Add(record);
            FlushInsert(false);
        }

        public void Update(dynamic record)
        {
            _log.Debug($"Queuing for update: {record}");
            _toUpdate.Add(record);
            FlushUpdate(false);
        }

        private void FlushInsert(bool force)
        {
            if (!force && _toInsert.Count < _maxBatchSize)
                return;
            _syncRepository.BulkInsert(_tableDetails, (IReadOnlyList<IDictionary<string, object>>)_toInsert.AsReadOnly());
            _toInsert = new List<dynamic>();
        }

        private void FlushUpdate(bool force)
        {
            if (!force && _toUpdate.Count < _maxBatchSize)
                return;
            _syncRepository.BulkUpdate(_tableDetails, (IReadOnlyList<IDictionary<string, object>>)_toUpdate.AsReadOnly());
            _toUpdate = new List<dynamic>();
        }

        public void Dispose()
        {
            FlushInsert(true);
            FlushUpdate(true);
        }
    }
}
