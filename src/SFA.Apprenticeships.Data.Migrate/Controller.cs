namespace SFA.Apprenticeships.Data.Migrate
{
    using SFA.Infrastructure.Interfaces;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    using SFA.Apprenticeships.Application.Interfaces;

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
                    DoUpdatesForAll();
                }
                catch (FullScanRequiredException ex)
                {
                    _log.Warn($"Change tracking unavailable ({ex.Message}). Doing full scan");

                    try
                    {
                        DoFullScanForAll(threaded);
                    }
                    catch (FatalException)
                    {
                        throw;
                    }
                    catch (Exception ex2)
                    {
                        _log.Error("Error occurred. Sleeping before trying again", ex2);
                    }
                }
                catch (FatalException)
                {
                    throw;
                }
                catch (Exception ex)
                {
                    _log.Error("Error occurred. Sleeping before trying again", ex);
                }

                Thread.Sleep(10000);
            }

            _log.Info("DoAll Cancelled");
        }

        public void Reset(bool threaded = false)
        {
            var exceptions = ApplyToTablesReverseDependency(table => _syncRepository.DeleteAll(table), threaded);

            if (exceptions.Any())
                throw exceptions.First();

            _syncRepository.Reset();
        }

        public void DoUpdatesForAll()
        {
            _log.Info("============ DoUpdatesForAll");

            var mutators = _tables.ToDictionary(t => t, t => _createMutateTarget(t)); // TODO: Dispose
            using (var context = _syncRepository.StartChangesOnlySnapshotSync())
            {
                if (context.AreAnyChanges())
                {
                    _log.Info("====== Adds / Updates, first go at Deletes");
                    var exceptions = ApplyToTables(table => DoChangesForTable(table, context, mutators[table]), false);
                    _log.Info("====== Second go at Deletes");
                    exceptions = exceptions.Concat(ApplyToTablesReverseDependency(table => mutators[table].FlushDeletes(), false));

                    if (exceptions.Any())
                        throw exceptions.First();
                }
                else
                    _log.Info("No changes");
                context.Success();
            }
        }

        public void DoFullScanForAll(bool threaded = false)
        {
            _log.Info("============ DoFullScanForAll");
            var mutators = _tables.ToDictionary(t => t, t => _createMutateTarget(t));
            var context = _syncRepository.StartFullTransactionlessSync();

            // Getting this up from pretty much eliminates problems with new parent+child records being added after the parent has been processed,
            // but not the case where parents are updated to point to a new child.
            _log.Info("====== Getting maximum ids");
            var maxFirstId = new Dictionary<ITableSpec, long>();
            ApplyToTablesUnthreaded(table => maxFirstId[table] = context.GetMaxFirstId(table));

            var exceptions = ApplyToTables(table => DoInitial(table, context, mutators[table], maxFirstId[table]), threaded);
            exceptions = exceptions.Concat(ApplyToTablesReverseDependency(table => mutators[table].FlushDeletes(), threaded));

            if (exceptions.Any())
                throw exceptions.First();

            context.Success();
        }

        private IEnumerable<Exception> ApplyToTables(Action<ITableSpec> action, bool threaded)
        {
            if (threaded)
                return ApplyToTablesThreaded(action);
            else
                return ApplyToTablesUnthreaded(action);
        }

        public IEnumerable<Exception> ApplyToTablesThreaded(Action<ITableSpec> action)
        {
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
                        //_log.Debug($"Already started {table.Key.Name} - status is {table.Value.Status}");
                    }
                    else if (_tables.Count() != 1 && table.Key.DependsOn.Where(dependency => !tables[dependency].IsCompleted).Any()) // Special case for --SingleTable option - ignore dependencies
                    {
                        //_log.Debug($"Deferring {table.Key.Name} as dependent on " + string.Join(", ", table.Key.DependsOn.Where(dependency => !tables[dependency].IsCompleted).Select(t => t.Name)));
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
            return tables.Where(t => t.Value.IsFaulted).Select(t => t.Value.Exception);
        }

        public IEnumerable<Exception> ApplyToTablesUnthreaded(Action<ITableSpec> action)
        {
            //_log.Debug($"---------- Scanning for tables to process");

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

            return exceptions;
        }

        public IEnumerable<Exception> ApplyToTablesReverseDependency(Action<ITableSpec> action, bool threaded)
        {
            if (threaded)
                _log.Info("Threaded reverse dependency not supported");

            var exceptions = new List<Exception>();

            var tables = _tables.Select(tableSpec =>
                new KeyValuePair<ITableSpec, bool>(tableSpec, false) // Table -> completed
                ).ToDictionary(i => i.Key, i => i.Value);

            while (true)
            {
                //_log.Debug($"---------- Scanning for tables to process");

                foreach (var table in tables)
                {
                    if (table.Value)
                    {
                        //_log.Debug($"Already finished {table.Key.Name}");
                    }
                    else
                    {
                        var outstandingDependencies = tables.Where(t => t.Key.DependsOn.Any(t2 => t2 == table.Key) && !t.Value).Select(t => t.Key.Name);

                        if (outstandingDependencies.Any() && _tables.Count() != 1) // Special case for --SingleTable option - ignore dependencies
                        {
                            //_log.Debug($"Deferring {table.Key.Name} as dependent on {string.Join(", ", outstandingDependencies)}");
                        }
                        else
                        {
                            _log.Info($"Processing {table.Key.Name}");

                            try
                            {
                                action(table.Key);
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

                            tables[table.Key] = true;
                            break;
                        }
                    }
                }

                if (!tables.Any(table => !table.Value))
                    break;
            }

            return exceptions;
        }


        public void DoDummy(ITableDetails table)
        {
            Thread.Sleep(4000);
        }

        public void DoChangesForTable(ITableSpec table, ISnapshotSyncContext syncContext, IMutateTarget mutateTarget)
        {
            try
            {
                var changes = syncContext.GetChangesForTable(table);

                var changesOfInterest = new Dictionary<IKeys, Operation>();

                foreach (var change in changes)
                {
                    switch (change.Operation)
                    {
                        case Operation.Insert:
                        case Operation.Update:
                        case Operation.Delete:
                            changesOfInterest.Add(change.PrimaryKeys, change.Operation);
                            break;
                        default:
                            throw new Exception($"Unknown change {change.Operation}");
                    }
                }

                _log.Debug($"Change tracking reports {changesOfInterest.Count(c => c.Value == Operation.Insert)} inserts, {changesOfInterest.Count(c => c.Value == Operation.Update)} updates, {changesOfInterest.Count(c => c.Value == Operation.Delete)} deletes");
                if (changesOfInterest.Any())
                {
                    var sourceRecords = syncContext.GetSourceRecordsAsync(table, changesOfInterest.Keys);
                    var targetRecords = syncContext.GetTargetRecords(table, changesOfInterest.Keys);

                    _log.Debug($"Loaded {sourceRecords.Result.Count()} from source, {targetRecords.Count()} from target");

                    DoSlidingComparision(table, sourceRecords.Result, targetRecords, changesOfInterest, mutateTarget);
                }
            }
            finally
            {
                mutateTarget.FlushInsertsAndUpdates();
            }
        }


        public void DoInitial(ITableSpec table, ITransactionlessSyncContext syncContext, IMutateTarget mutateTarget, long maxFirstId)
        {
            int batchSize = (int)(_migrateConfig.RecordBatchSize * table.BatchSizeMultiplier);
            try
            {
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
            finally
            {
                try
                {
                    // Where records are deleted and then reinserted in is important that they are deleted before attempting inserts...
                    mutateTarget.FlushDeletes(); // TODO: Ideally pass parameter through to prevent WARNings and ERRORs.
                }
                catch (Exception)
                {
                    // ...but when they are on a child table then the parent will need to be deleted first.
                    _log.Info("Some deletes failed. These will be retried once the parent tables have been processed.");
                }

                mutateTarget.FlushInsertsAndUpdates();
            }
        }

        public void DoSlidingComparision(ITableSpec table, IEnumerable<dynamic> sourceRecords, IEnumerable<dynamic> targetRecords, IDictionary<IKeys, Operation> operationById, IMutateTarget mutateTarget)
        {
            // TODO: Don't completely ignore the operation claimed by the change tracking
            // And don't completely ignore change tracking where no record could be found on the source

            using (var sourceRecordEnumerator = sourceRecords.GetEnumerator())
            using (var targetRecordEnumerator = targetRecords.GetEnumerator())
            {
                var sourceRecord = sourceRecordEnumerator.MoveNext() ? sourceRecordEnumerator.Current : null;
                var targetRecord = targetRecordEnumerator.MoveNext() ? targetRecordEnumerator.Current : null;

                while (sourceRecord != null || targetRecord != null)
                {
                    Keys sourcePrimaryKeys = (sourceRecord != null) ? Keys.GetPrimaryKeys(sourceRecord, table) : Keys.MaxValue;
                    Keys targetPrimaryKeys = (targetRecord != null) ? Keys.GetPrimaryKeys(targetRecord, table) : Keys.MaxValue;

                    //_log.Debug($"({sourcePrimaryKeys}) ({targetPrimaryKeys})");

                    var cmp = targetPrimaryKeys.CompareTo(sourcePrimaryKeys);
                    if (cmp < 0)
                    {
                        // Record is missing from source. Keep advancing target until back in sync.
                        DoPossibleDelete(mutateTarget, table, targetRecord);
                        targetRecord = targetRecordEnumerator.MoveNext() ? targetRecordEnumerator.Current : null;
                        continue;
                    }
                    else if (cmp == 0)
                    {
                        // Record in both source and target
                        DoPossibleUpdate(mutateTarget, table, sourceRecord, targetRecord);
                        sourceRecord = sourceRecordEnumerator.MoveNext() ? sourceRecordEnumerator.Current : null;
                        targetRecord = targetRecordEnumerator.MoveNext() ? targetRecordEnumerator.Current : null;
                        continue;
                    }
                    else if (cmp > 0)
                    {
                        // Record is missing from target. Keep inserting from source and advancing it until back in sync.
                        DoPossibleInsert(mutateTarget, table, sourceRecord);
                        sourceRecord = sourceRecordEnumerator.MoveNext() ? sourceRecordEnumerator.Current : null;
                        continue;
                    }
                    else
                    {
                        throw new Exception("Impossible");
                    }
                }
            }
        }

        public void DoPossibleDelete(IMutateTarget mutateTarget, ITableSpec table, dynamic targetRecord)
        {
            if (table.CanMutate(table, targetRecord, null, Operation.Delete))
            {
                mutateTarget.Delete(targetRecord);
            }
        }


        private void DoPossibleUpdate(IMutateTarget mutateTarget, ITableSpec table, dynamic sourceRecord, dynamic targetRecord)
        {
            table.Transform(table, targetRecord, sourceRecord);

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
                    changes.Add($"{sourceCol.Key} '{(targetColValue == null ? "<null>" : targetColValue.ToString().Truncate(100, "..."))}' => '{(sourceCol.Value == null ? "<null>" : sourceCol.Value.ToString().Truncate(100, "..."))}'");
                }
            }

            if (changes != null && table.CanMutate(table, targetRecord, sourceRecord, Operation.Update))
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

        public void DoPossibleInsert(IMutateTarget mutateTarget, ITableSpec table, dynamic sourceRecord)
        {
            if (table.CanMutate(table, null, sourceRecord, Operation.Insert))
            {
                table.Transform(table, null, sourceRecord);
                mutateTarget.Insert(sourceRecord);
            }
        }
    }

    public static class StringExtensions
    {
        public static string Truncate(this string value, int maxTotalLength, string suffix = "")
        {
            if (value == null || value.Length <= maxTotalLength)
                return value;

            return value.Substring(0, maxTotalLength - suffix.Length) + suffix;
        }
    }
}
