
namespace SFA.Apprenticeships.Data.Migrate
{
    using Infrastructure.Repositories.Sql.Common;
    using System;
    using System.Data;
    using System.Linq;
    using Dapper;
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using SFA.Infrastructure.Interfaces;

    public class SyncRespository : ISyncRespository
    {
        private IGetOpenConnection _sourceDatabase;
        private IGetOpenConnection _targetDatabase;
        private ILogService _log;

        public SyncRespository(ILogService log, IGetOpenConnection sourceDatabase, IGetOpenConnection targetDatabase)
        {
            _log = log;
            _sourceDatabase = sourceDatabase;
            _targetDatabase = targetDatabase;
        }

        public ISnapshotSyncContext StartChangesOnlySnapshotSync()
        {
            return new ChangesOnlySnapshotSyncContext(_sourceDatabase, _targetDatabase);
        }

        public ITransactionlessSyncContext StartFullTransactionlessSync()
        {
            return new FullTransactionlessSyncContext(_sourceDatabase, _targetDatabase);
        }

        public void BulkInsert(ITableDetails table, IReadOnlyList<dynamic> records)
        {
            if (records.Any())
            {
                var columnTypes = GetColumnTypes(records);

                using (var connection = (SqlConnection)_targetDatabase.GetOpenConnection())
                {
                    BulkInsert(connection, table.Name, records, columnTypes);
                }
            }
        }

        public void BulkUpdate(ITableDetails table, IReadOnlyList<dynamic> records)
        {
            const string tempTable = "#BulkUpdateTemp";

            if (records.Any())
            {
                var columnTypes = GetColumnTypes(records);

                var prototypeRecord = ((IDictionary<string, object>)records[0]);
                using (var connection = (SqlConnection)_targetDatabase.GetOpenConnection()) // TODO: Cast
                {
                    var columnsWithSqlTypes = columnTypes.Select(col => $"{col.Key} {SqlTypeFor(col.Value)}");
                    connection.Execute($"CREATE TABLE {tempTable} ({string.Join(", ", columnsWithSqlTypes)})");

                    BulkInsert(connection, tempTable, records, columnTypes);

                    var dataColumnNames = columnTypes.Keys.Where(k => k != table.PrimaryKey);
                    connection.Execute($@"
    UPDATE target
    SET    {string.Join(", ", dataColumnNames.Select(col => $"[{col}] = t.[{col}]"))}
    FROM   {table.Name} target
    JOIN   {tempTable}  t
    ON     t.{table.PrimaryKey} = target.{table.PrimaryKey}

    DROP TABLE {tempTable}
    ");
                }
            }
        }

        private static string SqlTypeFor(Type type)
        {
            if (type == typeof(string))
                return "VARCHAR(MAX)";
            if (type == typeof(int))
                return "INT";
            if (type == typeof(long))
                return "BIGINT";
            if (type == typeof(short))
                return "SMALLINT";
            if (type == typeof(DateTime))
                return "DATETIME2";
            if (type == typeof(bool))
                return "BIT";
            if (type == typeof(byte[]))
                return "VARBINARY(MAX)";
            if (type == typeof(decimal))
                return "DECIMAL(38,10)"; // TODO: This is sufficient for everything I have seen, but ideally should ensure it is big enough for values actually encountered

            throw new Exception($"No mapping from {type} to SQL Server type");
        }


        /// <summary>
        /// Every element in the list MUST contain the same keys and the same type of value.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data">Note that lists of many types of dynamic are suitable</param>
        /// <param name="tableName"></param>
        private static void BulkInsert(SqlConnection connection, string tableName, IReadOnlyList<dynamic> data, IDictionary<string, Type> columnTypes)
        {
            var dataTable = new DataTable();
            foreach (var column in columnTypes)
            {
                dataTable.Columns.Add(column.Key, column.Value);
            }

            foreach (IDictionary<string, object> row in data)
            {
                var dataRow = dataTable.NewRow();
                foreach (var name in columnTypes.Keys)
                {
                    dataRow[name] = row[name] ?? DBNull.Value;
                }
                dataTable.Rows.Add(dataRow);
            }

            using (var bulkCopy = new SqlBulkCopy(connection, SqlBulkCopyOptions.KeepIdentity, null))
            {
                // Mapping columns names is required whenever the data might not tie up positionwise with the target
                // This can happen to us when the supplied data has null for every value and therefore cannot include
                // it in the bulk loaded data because we cannot determine its type.
                // It could also happen if the target table has had the columns reordered
                foreach (var column in columnTypes)
                {
                    bulkCopy.ColumnMappings.Add(column.Key, column.Key);
                }

                bulkCopy.DestinationTableName = tableName;
                bulkCopy.WriteToServer(dataTable);
            }
        }

        /// <summary>
        /// Get a mapping between column name and the data type associated with it.
        /// The results does not include columns where all values are null as a) the type cannot be determined and b) there is no need to specify a value when uploading
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static IDictionary<string,Type> GetColumnTypes(IReadOnlyList<dynamic> data)
        {
            var result = new Dictionary<string, Type>();
            bool first = true;
            int nulls = 0;
            foreach (var row in data)
            {
                var record = ((IDictionary<string, object>)row);
                foreach (var column in record)
                {
                    string name = column.Key;
                    Type type = column.Value == null ? null : (Nullable.GetUnderlyingType(column.Value.GetType()) ?? column.Value.GetType());

                    if (first)
                    {
                        result[name] = type;
                        if (type == null)
                            nulls++;
                    }
                    else
                    {
                        Type previousType;
                        if (!result.TryGetValue(name, out previousType))
                        {
                            throw new Exception($"{name} is in some row(s), but not the first one");
                        }
                        if (previousType == null && type != null)
                        {
                            result[name] = type;
                            nulls--;
                        }
                        else if (type != previousType && type != null)
                        {
                            throw new Exception($"{name} has type {previousType} for the first non-null value, but {type} in a later row");
                        }
                    }

                    // if (nulls == 0) return result; TODO: Optimisation
                }

                first = false;
            }

            var nullNames = result.Where(r => r.Value == null).Select(r => r.Key).ToList();
            foreach (var name in nullNames)
                result.Remove(name);

            return result;
        }

        public void Truncate(ITableDetails table)
        {
            _log.Info($"DELETEing contents of {table.Name}");
            _targetDatabase.MutatingQuery<int>($"DELETE {table.Name}");
        }

        public void Reset()
        {
            _targetDatabase.MutatingQuery<int>("UPDATE sync.SyncParams SET LastSyncVersion = NULL");
        }


        private abstract class CommonSyncContext
        {
            protected IGetOpenConnection _sourceDatabase;
            protected IGetOpenConnection _targetDatabase;

            protected long _nextSyncVersion { get; set; }

            public CommonSyncContext(IGetOpenConnection sourceDatabase, IGetOpenConnection targetDatabase)
            {
                _sourceDatabase = sourceDatabase;
                _targetDatabase = targetDatabase;
            }

            protected void UpdateLastSyncVersion()
            {
                /*
                CREATE SCHEMA Sync
                GO
                CREATE TABLE Sync.SyncParams (LastSyncVersion INT)
                GO
                INSERT INTO SYNC.SyncParams VALUES (NULL)
                */
                _targetDatabase.MutatingQuery<int>($"UPDATE Sync.SyncParams SET LastSyncVersion = @lastSyncVersion", new { lastSyncVersion = _nextSyncVersion });
            }

            public abstract void Dispose();
            public abstract void Success();
        }

        private class ChangesOnlySnapshotSyncContext : CommonSyncContext, ISnapshotSyncContext
        {
            // Refer to this page: https://msdn.microsoft.com/en-us/library/bb933874.aspx

            private IDbConnection _sourceConnection;
            private IDbTransaction _sourceTransaction;

            private long _lastSyncVersion { get; set; }

            public ChangesOnlySnapshotSyncContext(IGetOpenConnection sourceDatabase, IGetOpenConnection targetDatabase) : base(sourceDatabase, targetDatabase)
            {
                var lastSyncVersion = _targetDatabase.Query<long?>($"SELECT LastSyncVersion FROM Sync.SyncParams").Single();
                if (lastSyncVersion == null)
                    throw new FullScanRequiredException();
                _lastSyncVersion = lastSyncVersion.Value;

                try
                {
                    _sourceConnection = _sourceDatabase.GetOpenConnection();
                    _sourceConnection.Execute("SET TRANSACTION ISOLATION LEVEL SNAPSHOT");
                    _sourceTransaction = _sourceConnection.BeginTransaction();

                    _nextSyncVersion = _sourceConnection.Query<long>($"SELECT CHANGE_TRACKING_CURRENT_VERSION()", transaction: _sourceTransaction, buffered: false).Single();
                }
                catch (Exception)
                {
                    Dispose();
                    throw;
                }
            }

            public IEnumerable<ChangeTableRow> GetChanges(string tableName)
            {
                long? minValidVersion = _sourceConnection.Query<long?>($"SELECT CHANGE_TRACKING_MIN_VALID_VERSION(OBJECT_ID('{tableName}'))", transaction: _sourceTransaction, buffered: false).Single();
                if (minValidVersion == null)
                    throw new Exception($"Change tracking not enabled, unknown table or insufficient permission on {tableName}");
                if (minValidVersion > _lastSyncVersion)
                    throw new FullScanRequiredException();

                // Note: The ordering of the results from CHANGETABLE is undefined and therefore it is difficult to do them in batches. Accordingly ideally the results would not be buffered.
                // However, without enabling SQL Server MARS, this will not allow other queries to proceed at the same time.
                return _sourceConnection.Query<ChangeTableRow>($"SELECT * FROM CHANGETABLE(CHANGES {tableName}, @lastSyncVersion) AS Dummy", new { lastSyncVersion = _lastSyncVersion }, transaction: _sourceTransaction);
            }

            public IEnumerable<dynamic> GetSourceRecords(ITableDetails table, IEnumerable<long> ids)
            {
                return _sourceConnection.Query<dynamic>($"SELECT * FROM {table.Name} WHERE {table.PrimaryKey} IN @Ids ORDER BY {table.PrimaryKey}", new { Ids = ids }, transaction: _sourceTransaction);
            }

            public IEnumerable<dynamic> GetTargetRecords(ITableDetails table, IEnumerable<long> ids)
            {
                return _targetDatabase.Query<dynamic>($"SELECT * FROM {table.Name} WHERE {table.PrimaryKey} IN @Ids ORDER BY {table.PrimaryKey}", new { Ids = ids });
            }

            public override void Success()
            {
                _sourceTransaction.Commit();
                UpdateLastSyncVersion();
            }
            public override void Dispose()
            {
                if (_sourceTransaction != null)
                    _sourceTransaction.Dispose();
                if (_sourceConnection != null)
                    _sourceConnection.Dispose();
            }
        }

        private class FullTransactionlessSyncContext : CommonSyncContext, ITransactionlessSyncContext
        {
            public FullTransactionlessSyncContext(IGetOpenConnection sourceDatabase, IGetOpenConnection targetDatabase) : base(sourceDatabase, targetDatabase)
            {
                /*
ALTER DATABASE LSC_MI_MS_CRM_Staging
SET CHANGE_TRACKING = ON
(CHANGE_RETENTION = 2 DAYS, AUTO_CLEANUP = ON)

USE LSC_MI_MS_CRM_Staging
EXEC sp_MSforeachtable 'ALTER TABLE ? ENABLE CHANGE_TRACKING'

GRANT VIEW CHANGE TRACKING ON SCHEMA ::dbo TO MSSQLReadOnly
                */

                var version = _sourceDatabase.Query<long?>($"SELECT CHANGE_TRACKING_CURRENT_VERSION() AS ver").Single();
                if (version == null)
                    throw new FatalException("Change tracking not enabled on source database");
                _nextSyncVersion = version.Value;
            }

            public long GetMaxId(ITableDetails table)
            {
                return _sourceDatabase.GetOpenConnection().Query<long>($"SELECT MAX({table.PrimaryKey}) FROM {table.Name}").Single();
            }

            public IEnumerable<dynamic> GetSourceRecords(ITableDetails table, long startId, long endId)
            {
                return _sourceDatabase.Query<dynamic>($"SELECT * FROM {table.Name} WHERE {table.PrimaryKey} BETWEEN @StartId AND @EndId ORDER BY {table.PrimaryKey}", new { StartId = startId, EndId = endId });
            }

            public IEnumerable<dynamic> GetTargetRecords(ITableDetails table, long startId, long endId)
            {
                return _targetDatabase.Query<dynamic>($"SELECT * FROM {table.Name} WHERE {table.PrimaryKey} BETWEEN @StartId AND @EndId ORDER BY {table.PrimaryKey}", new { StartId = startId, EndId = endId });
            }

            public override void Dispose()
            {
            }

            public override void Success()
            {
                UpdateLastSyncVersion();
            }
        }
    }
}
