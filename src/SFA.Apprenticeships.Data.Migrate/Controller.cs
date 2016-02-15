namespace SFA.Apprenticeships.Data.Migrate
{
    using SFA.Infrastructure.Interfaces;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using SFA.Apprenticeships.Infrastructure.Repositories.Sql.Common;

    public class Controller
    {
        private ILogService _log;
        private IMigrateConfiguration _migrateConfig;
        private Func<IMutateTarget> _createMutateTarget;
        private IEnumerable<ITableSpec> _tables;

        public ISyncRespository _syncRepository;

        public Controller(IMigrateConfiguration migrateConfig, ILogService log, ISyncRespository syncRepository, Func<IMutateTarget> createMutateTarget, IEnumerable<ITableSpec> tables)
        {
            _migrateConfig = migrateConfig;
            _log = log;
            _syncRepository = syncRepository;
            _createMutateTarget = createMutateTarget;
            _tables = tables;
        }

        public void DoAll()
        {
            _log.Info("DoAll Started");

            while (true)
            {
                try
                {
                    DoUpdatesForAll();
                }
                catch (FullScanRequiredException)
                {
                    _log.Warn("Change tracking unavailable. Doing full scan");
                    DoFullScanForAll();
                }
                catch (Exception ex)
                {
                    _log.Error("Error occurred. Sleeping for trying again", ex);
                    Thread.Sleep(60);
                }
            }

            _log.Info("DoAll Finished");
        }

        public void DoUpdatesForAll()
        {
            _log.Info("DoUpdatesForAll");
            using (var mutateTarget = _createMutateTarget())
            using (var context = _syncRepository.StartChangesOnlySnapshotSync())
            {
                ApplyToTables(tableSpec => DoUpdatesForTable(tableSpec, context, mutateTarget));
                context.Success();
            }
        }

        public void DoFullScanForAll()
        {
            using (var mutateTarget = _createMutateTarget())
            {
                var context = _syncRepository.StartFullTransactionlessSync();
                ApplyToTables(tableSpec => DoInitial(tableSpec, context, mutateTarget));
                context.Success();
            }
        }

        public void ApplyToTables(Action<ITableSpec> action)
        {
            var tables = _tables.Select(tableSpec =>
                new KeyValuePair<ITableSpec, Task>(tableSpec, new Task(() => action(tableSpec)))).ToDictionary(i => i.Key, i => i.Value);

            while (true)
            {
                _log.Info($"Scanning for tables to process");

                foreach (var table in tables)
                {
                    if (table.Value.Status != TaskStatus.Created)
                    {
                        _log.Info($"Already started {table.Key.Name} - status is {table.Value.Status}");
                    }
                    else if (table.Key.DependsOn.Where(dependency => !tables[dependency].IsCompleted).Any())
                    {
                        _log.Info($"Deferring {table.Key.Name} as dependent on " + string.Join(", ", table.Key.DependsOn.Where(dependency => !tables[dependency].IsCompleted).Select(t => t.Name)));
                    }
                    else
                    {
                        _log.Info($"Starting processing of {table.Key.Name}");
                        table.Value.Start();
                    }
                }

                var tasksRemaining = tables.Select(table => table.Value).Where(task => !task.IsCompleted).ToArray();
                if (!tasksRemaining.Any())
                    break;
                Task.WaitAny(tasksRemaining);
            }

        }

        public void DoDummy(ITableDetails table)
        {
            Thread.Sleep(4000);
        }

        public void DoUpdatesForTable(ITableSpec table, ISnapshotSyncContext syncContext, IMutateTarget mutateTarget)
        {
            const int maxRecordsInBatch = 2000; // SQL parameter limit is 2100, so this is pretty much fixed.

            var changes = syncContext.GetChanges(table.Name);

            using (var changesEnumerator = changes.GetEnumerator())
            {
                while (true)
                {
                    var changesOfInterest = new Dictionary<long, Operation>();
                    for (int i = 0; i < maxRecordsInBatch && changesEnumerator.MoveNext(); i++)
                    {
                        var change = changesEnumerator.Current;
                        switch (change.Operation)
                        {
                            case Operation.Delete:
                                _log.Warn($"Ignored delete of record {change.Id} from {table.Name}");
                                break;
                            case Operation.Insert:
                            case Operation.Update:
                                changesOfInterest.Add(change.Id, change.Operation);
                                break;
                            default:
                                throw new Exception($"Unknown change {change.Operation}");
                        }
                    }

                    if (!changesOfInterest.Any())
                        break;

                    var sourceRecords = syncContext.GetSourceRecords(table, changesOfInterest.Keys);
                    var targetRecords = syncContext.GetTargetRecords(table, changesOfInterest.Keys);

                    DoSlidingComparision(table, sourceRecords, targetRecords, changesOfInterest, mutateTarget);
                }
            }
        }


        public void DoInitial(ITableSpec table, ITransactionlessSyncContext syncContext, IMutateTarget mutateTarget)
        {
            long maxId = syncContext.GetMaxId(table);

            long startId = 0;

            while (startId < maxId)
            {
                long endId = startId + _migrateConfig.RecordBatchSize - 1;

                var sourceRecords = syncContext.GetSourceRecords(table, startId, endId);
                var targetRecords = syncContext.GetTargetRecords(table, startId, endId);

                DoSlidingComparision(table, sourceRecords, targetRecords, null, mutateTarget);

                // Note: To Insert use SqlBulkCopy (try to use my existing code for this). To Update use SqlBulkCopy into a temporary table the update from there.
                // See http://stackoverflow.com/questions/20635796/bulk-update-in-c-sharp

                startId = endId + 1;
            }
        }

        public void DoSlidingComparision(ITableSpec table, IEnumerable<dynamic> sourceRecords, IEnumerable<dynamic> targetRecords, IDictionary<long, Operation> operationById, IMutateTarget mutateTarget)
        {
            using (var targetRecordEnumerator = targetRecords.GetEnumerator())
            {
                var targetRecord = targetRecordEnumerator.MoveNext() ? targetRecordEnumerator.Current() : null;

                foreach (var sourceRecord in sourceRecords)
                {
                    long sourceId = (long)((IDictionary<string, object>)sourceRecord)[table.PrimaryKey];

                    while (true)
                    {
                        long targetId = (targetRecord != null) ? (long)((IDictionary<string, object>)targetRecord)[table.PrimaryKey] : long.MaxValue;

                        if (targetId < sourceId)
                        {
                            // Record is missing from source. Keep advancing target until back in sync.
                            targetRecord = targetRecordEnumerator.MoveNext() ? targetRecordEnumerator.Current() : null;
                            continue;
                        }
                        else if (targetId == sourceId)
                        {
                            // Record in both source and target
                            DoChange(mutateTarget, table, sourceRecord, targetRecord);
                            targetRecord = targetRecordEnumerator.MoveNext() ? targetRecordEnumerator.Current() : null;
                            break;
                        }
                        else if (targetId > sourceId)
                        {
                            // Record is missing from target. Keep inserting from source and advancing it until back in sync.
                            DoChange(mutateTarget, table, sourceRecord, null);
                            break;
                        }
                        else
                        {
                            throw new Exception("Impossible");
                        }
                    }
                }
            }
        }


        public void DoChange(IMutateTarget mutateTarget, ITableSpec table, dynamic sourceRecord, dynamic targetRecord)
        {
            if (table.Transform(targetRecord, sourceRecord))
            {
                if (targetRecord == null)
                    mutateTarget.Insert(sourceRecord);
                else
                    mutateTarget.Update(sourceRecord);
            }
        }
    }
}
