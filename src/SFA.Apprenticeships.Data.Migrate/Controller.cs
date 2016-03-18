namespace SFA.Apprenticeships.Data.Migrate
{
    using SFA.Infrastructure.Interfaces;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    public class Controller
    {
        private ILogService _log;
        private IMigrateConfiguration _migrateConfig;
        private Func<ITableSpec, IMutateTarget> _createMutateTarget;
        private IEnumerable<ITableSpec> _tables;

        public IGenericSyncRespository _syncRepository;

        public Controller(IMigrateConfiguration migrateConfig, ILogService log, IGenericSyncRespository syncRepository, Func<ITableSpec, IMutateTarget> createMutateTarget, IEnumerable<ITableSpec> tables)
        {
            _migrateConfig = migrateConfig;
            _log = log;
            _syncRepository = syncRepository;
            _createMutateTarget = createMutateTarget;
            _tables = tables;
        }

        /// <summary>
        /// Run the migration. Will only exit if either it is cancelled (via the cancellation token) or
        /// an error that is known to be fatal (not transitory and requires attention) occurs
        /// </summary>
        /// <param name="cancellationToken"></param>
        public void DoAll(CancellationTokenSource cancellationToken, bool threaded = false)
        {
            _log.Info("DoAll Started");

            while (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    DoUpdatesForAll(threaded);
                }
                catch (FatalException)
                {
                    throw;
                }
                catch (FullScanRequiredException)
                {
                    _log.Warn("Change tracking unavailable. Doing full scan");

                    try
                    {
                        DoFullScanForAll(threaded);
                    }
                    catch (FatalException)
                    {
                        throw;
                    }
                    catch (Exception ex)
                    {
                        _log.Error("Error occurred. Sleeping before trying again", ex);
                    }
                }
                catch (Exception ex)
                {
                    _log.Error("Error occurred. Sleeping before trying again", ex);
                }

                Thread.Sleep(10000);
            }

            _log.Info("DoAll Cancelled");
        }

        public void Reset()
        {
            ApplyToTablesUnthreadedReverseDependency(table => _syncRepository.DeleteAll(table));
            _syncRepository.Reset();
        }

        public void DoUpdatesForAll(bool threaded = false)
        {
            _log.Info("============ DoUpdatesForAll");
            using (var context = _syncRepository.StartChangesOnlySnapshotSync())
            {
                if (context.AreAnyChanges())
                    ApplyToTables(tableSpec => DoUpdatesForTable(tableSpec, context), threaded);
                else
                    _log.Info("No changes");
                context.Success();
            }
        }

        public void DoFullScanForAll(bool threaded = false)
        {
            _log.Info("============ DoFullScanForAll");
            var context = _syncRepository.StartFullTransactionlessSync();
            ApplyToTables(tableSpec => DoInitial(tableSpec, context), threaded);
            context.Success();
        }

        private void ApplyToTables(Action<ITableSpec> action, bool threaded)
        {
            if (threaded)
                ApplyToTablesThreaded(action);
            else
                ApplyToTablesUnthreaded(action);
        }

        public void ApplyToTablesThreaded(Action<ITableSpec> action)
        {
            if (_tables.Count() == 1)
            {
                // Special case for testing - ignore dependencies
                action(_tables.Single());
                return;
            }


            var tables = _tables.Select(tableSpec =>
                new KeyValuePair<ITableSpec, Task>(tableSpec, new Task(() => { try { action(tableSpec); } catch (Exception ex) { if (!(ex is FatalException)) _log.Error(ex); throw; } }))
                ).ToDictionary(i => i.Key, i => i.Value);

            while (true)
            {
                _log.Info($"---------- Scanning for tables to process");

                foreach (var table in tables)
                {
                    if (table.Value.IsFaulted && table.Value.Exception.GetBaseException() is FatalException)
                    {
                        throw table.Value.Exception.GetBaseException();
                    }
                    if (table.Value.Status != TaskStatus.Created)
                    {
                        _log.Debug($"Already started {table.Key.Name} - status is {table.Value.Status}");
                    }
                    else if (table.Key.DependsOn.Where(dependency => !tables[dependency].IsCompleted).Any())
                    {
                        _log.Debug($"Deferring {table.Key.Name} as dependent on " + string.Join(", ", table.Key.DependsOn.Where(dependency => !tables[dependency].IsCompleted).Select(t => t.Name)));
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

            // Rethrow first of any (non-fatal) exception that may have occurred
            foreach (var table in tables)
            {
                if (table.Value.IsFaulted)
                    throw table.Value.Exception;
            }

        }

        public void ApplyToTablesUnthreaded(Action<ITableSpec> action)
        {
            _log.Info($"---------- Scanning for tables to process");

            var exceptions = new List<Exception>();

            foreach (var table in _tables)
            {
                _log.Info($"Processing {table.Name}");
                try
                {
                    action(table);
                }
                catch (FatalException)
                {
                    // No point in carrying on for one of these
                    throw;
                }
                catch (Exception ex)
                {
                    // Report, but try to progress with the next table
                    // Re-throw the exception later
                    _log.Error(ex);
                    exceptions.Add(ex);
                }
            }

            if (exceptions.Any())
                throw exceptions.First();
        }

        public void ApplyToTablesUnthreadedReverseDependency(Action<ITableSpec> action)
        {
            var tables = _tables.Select(tableSpec =>
                new KeyValuePair<ITableSpec, bool>(tableSpec, false) // Table -> completed
                ).ToDictionary(i => i.Key, i => i.Value);

            while (true)
            {
                _log.Info($"---------- Scanning for tables to process");

                foreach (var table in tables)
                {
                    if (table.Value)
                    {
                        _log.Debug($"Already finished {table.Key.Name}");
                    }
                    else
                    {
                        var outstandingDependencies = tables.Where(t => t.Key.DependsOn.Any(t2 => t2 == table.Key) && !t.Value).Select(t => t.Key.Name);

                        if (outstandingDependencies.Any())
                        {
                            _log.Debug($"Deferring {table.Key.Name} as dependent on {string.Join(", ", outstandingDependencies)}");
                        }
                        else
                        {
                            _log.Info($"Processing {table.Key.Name}");
                            action(table.Key);
                            tables[table.Key] = true;
                            break;
                        }
                    }
                }

                if (!tables.Any(table => !table.Value))
                    break;
            }
        }


        public void DoDummy(ITableDetails table)
        {
            Thread.Sleep(4000);
        }

        public void DoUpdatesForTable(ITableSpec table, ISnapshotSyncContext syncContext)
        {
            using (var mutateTarget = _createMutateTarget(table))
            {
                var changes = syncContext.GetChangesForTable(table);

                var changesOfInterest = new Dictionary<IKeys, Operation>();
                var deletes = new List<IKeys>();

                foreach (var change in changes)
                {
                    switch (change.Operation)
                    {
                        case Operation.Delete:
                            deletes.Add(change.PrimaryKeys);
                            _log.Warn($"Ignored delete of record {change.PrimaryKeys} from {table.Name}");
                            break;
                        case Operation.Insert:
                        case Operation.Update:
                            changesOfInterest.Add(change.PrimaryKeys, change.Operation);
                            break;
                        default:
                            throw new Exception($"Unknown change {change.Operation}");
                    }
                }

                if (deletes.Any())
                {
                    var targetRecords = syncContext.GetTargetRecords(table, deletes);

                    DoDeletes(table, targetRecords, deletes, mutateTarget);
                }

                if (changesOfInterest.Any())
                {
                    var sourceRecords = syncContext.GetSourceRecordsAsync(table, changesOfInterest.Keys);
                    var targetRecords = syncContext.GetTargetRecords(table, changesOfInterest.Keys);

                    DoSlidingComparision(table, sourceRecords.Result, targetRecords, changesOfInterest, mutateTarget);
                }
            }
        }


        public void DoInitial(ITableSpec table, ITransactionlessSyncContext syncContext)
        {
            int batchSize = (int)(_migrateConfig.RecordBatchSize * table.BatchSizeMultiplier);
            using (var mutateTarget = _createMutateTarget(table))
            {
                long maxFirstId = (table.Name == "Vacancyxxx") ? 700000 : syncContext.GetMaxFirstId(table);

                long startFirstId = (table.Name == "Personxxxx") ? 4290000 : 0;

                while (startFirstId < maxFirstId)
                {
                    long endFirstId = Math.Min(startFirstId + batchSize - 1, maxFirstId);
                    _log.Debug($"Processing {startFirstId} to {endFirstId} on {table.Name} ({startFirstId / Math.Max(maxFirstId / 100, 1)}% done)");

                    var sourceRecords = syncContext.GetSourceRecordsAsync(table, startFirstId, endFirstId);
                    var targetRecords = syncContext.GetTargetRecords(table, startFirstId, endFirstId);

                    DoSlidingComparision(table, sourceRecords.Result, targetRecords, null, mutateTarget);

                    startFirstId = endFirstId + 1;
                }
            }
        }

        public void DoSlidingComparision(ITableSpec table, IEnumerable<dynamic> sourceRecords, IEnumerable<dynamic> targetRecords, IDictionary<IKeys, Operation> operationById, IMutateTarget mutateTarget)
        {
            // TODO: Don't completely ignore the operation claimed by the change tracking
            // And don't completely ignore change tracking where no record could be found on the source

            using (var targetRecordEnumerator = targetRecords.GetEnumerator())
            {
                var targetRecord = targetRecordEnumerator.MoveNext() ? targetRecordEnumerator.Current : null;

                foreach (var sourceRecord in sourceRecords)
                {
                    var sourcePrimaryKeys = Keys.GetPrimaryKeys(sourceRecord, table);

                    while (true)
                    {
                        var targetPrimaryKeys = (targetRecord != null) ? Keys.GetPrimaryKeys(targetRecord, table) : Keys.MaxValue;

                        var cmp = targetPrimaryKeys.CompareTo(sourcePrimaryKeys);
                        if (cmp < 0)
                        {
                            // Record is missing from source. Keep advancing target until back in sync.
                            targetRecord = targetRecordEnumerator.MoveNext() ? targetRecordEnumerator.Current : null;
                            continue;
                        }
                        else if (cmp == 0)
                        {
                            // Record in both source and target
                            DoChange(mutateTarget, table, sourceRecord, targetRecord);
                            targetRecord = targetRecordEnumerator.MoveNext() ? targetRecordEnumerator.Current : null;
                            break;
                        }
                        else if (cmp > 0)
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

        public void DoDeletes(ITableSpec tableSpec, IEnumerable<dynamic> targetRecords, IEnumerable<IKeys> deletes, IMutateTarget mutateTarget)
        {
            foreach (var targetRecord in targetRecords)
            {
                if (tableSpec.ShouldDelete(tableSpec, targetRecord))
                {
                    mutateTarget.Delete(targetRecord);
                }
            }
        }


        public void DoChange(IMutateTarget mutateTarget, ITableSpec table, dynamic sourceRecord, dynamic targetRecord)
        {
            if (table.Transform(table, targetRecord, sourceRecord))
            {
                if (targetRecord == null)
                {
                    mutateTarget.Insert(sourceRecord);
                }
                else
                {
                    int changesLeftToLog = _migrateConfig.MaxNumberOfChangesToDetailPerTable.GetValueOrDefault(int.MaxValue) - mutateTarget.NumberOfUpdates;

                    var sourceRecordDict = (IDictionary<string, object>)sourceRecord;
                    var targetRecordDict = (IDictionary<string, object>)targetRecord;

                    List<string> changes = null;
                    foreach (var sourceCol in sourceRecordDict)
                    {
                        var targetColValue = targetRecordDict[sourceCol.Key];

                        bool equal;
                        if (sourceCol.Value == null || targetColValue == null)
                        {
                            equal = (sourceCol.Value == null && targetColValue == null);
                        }
                        else if (sourceCol.Value.GetType() == typeof(byte[]))
                        {
                            var s = (byte[])sourceCol.Value;
                            var t = (byte[])targetColValue;
                            equal = s.SequenceEqual(t);
                        }
                        else
                        {
                            equal = sourceCol.Value.Equals(targetColValue);
                        }

                        if (!equal)
                        {
                            if (changes == null)
                            {
                                changes = new List<string>();
                                if (changesLeftToLog <= 0)
                                    break;
                            }
                            changes.Add($"{sourceCol.Key} '{targetColValue ?? "<null>"}' => '{sourceCol.Value ?? "<null>"}'");
                        }
                    }

                    if (changes != null)
                    {
                        if (changesLeftToLog == 0)
                            _log.Info($"{table.Name}: More records have changes but these will not be reported");
                        else if (changesLeftToLog > 0)
                            _log.Info($"{table.Name} {Keys.GetPrimaryKeys(sourceRecord, table)}: {string.Join(", ", changes)}");
                        mutateTarget.Update(sourceRecord);
                    }
                    else
                    {
                        mutateTarget.NoChange(sourceRecord);
                    }
                }
            }
        }

    }
}
