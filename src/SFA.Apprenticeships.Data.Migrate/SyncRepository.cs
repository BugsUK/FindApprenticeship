
namespace SFA.Apprenticeships.Data.Migrate
{
    using Infrastructure.Repositories.Sql.Common;
    using System;
    using System.Data;
    using System.Linq;
    using Dapper;
    using System.Collections.Generic;
    using System.Data.SqlClient;

    public class SyncRespository : ISyncRespository
    {
        private IGetOpenConnection _sourceDatabase;
        private IGetOpenConnection _targetDatabase;

        public SyncRespository(IGetOpenConnection sourceDatabase, IGetOpenConnection targetDatabase)
        {
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

        public void BulkInsert(ITableDetails table, IReadOnlyList<IDictionary<string, object>> records)
        {
            using (var connection = (SqlConnection)_targetDatabase.GetOpenConnection()) // TODO: Cast
            {
                BulkInsert(connection, table.Name, records);
            }
        }

        public void BulkUpdate(ITableDetails table, IReadOnlyList<IDictionary<string, object>> records)
        {
            const string tempTable = "#BulkUpdateTemp";

            using (var connection = (SqlConnection)_targetDatabase.GetOpenConnection()) // TODO: Cast
            {
                var columns = records[0].Select(col => $"{col.Key} {SqlTypeFor(col.Value)}");
                connection.Execute($"CREATE TABLE {tempTable} ({string.Join(", ", columns)}");

                BulkInsert(connection, tempTable, records);

                // TODO: Avoid updating primary key
                connection.Execute($@"
UPDATE target
SET    {string.Join(", ", columns.Select(col => $"[{col}] = t.[{col}])"))}
FROM   {table.Name} target
JOIN   {tempTable}  t
ON     t.{table.PrimaryKey} = target.{table.PrimaryKey}

DROP TABLE {tempTable}
");
            }
        }

        private static string SqlTypeFor(object obj)
        {
            var type = obj.GetType();
            type = Nullable.GetUnderlyingType(type) ?? type;

            if (type == typeof(string))
                return "VARCHAR(MAX)";
            if (type == typeof(int))
                return "INT";
            if (type == typeof(long))
                return "BIGINT";
            if (type == typeof(DateTime))
                return "DATETIME2";
            if (type == typeof(bool))
                return "BIT";

            throw new Exception("No mapping from {type} to SQL Server type");
        }


        /// <summary>
        /// Every element in the list MUST contain the same keys and the same type of value.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data">Note that lists of many types of dynamic are suitable</param>
        /// <param name="tableName"></param>
        private static void BulkInsert(SqlConnection connection, string tableName, IReadOnlyList<IDictionary<string, object>> data)
        {
            var dataTable = new DataTable(tableName);
            foreach (var column in data[0])
            {
                var propertyType = column.GetType();
                dataTable.Columns.Add(column.Key, Nullable.GetUnderlyingType(propertyType) ?? propertyType);
            }

            foreach (var row in data)
            {
                var dataRow = dataTable.NewRow();
                foreach (var column in data[0])
                    row[column.Key] = row[column.Key];
                dataTable.Rows.Add(dataRow);
            }

            using (var bulkCopy = new SqlBulkCopy(connection))
            {
                bulkCopy.WriteToServer(dataTable);
            }
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
                long? minValidVersion = _sourceConnection.Query<long?>($"SELECT CHANGE_TRACKING_MIN_VALID_VERSION(OBJECT_ID({tableName}))", transaction: _sourceTransaction, buffered: false).Single();
                if (minValidVersion == null)
                    throw new Exception($"Change tracking not enabled, unknown table or insufficient permission on {tableName}");
                if (minValidVersion > _lastSyncVersion)
                    throw new FullScanRequiredException();

                // Note: The ordering of the results from CHANGETABLE is undefined and therefore it is difficult to do them in batches. Accordingly the results are not buffered.
                return _sourceConnection.Query<ChangeTableRow>($"SELECT * FROM CHANGETABLE(CHANGES {tableName}, @lastSyncVersion) AS Dummy", new { lastSyncVersion = _lastSyncVersion }, transaction: _sourceTransaction, buffered: false);
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
                _nextSyncVersion = _sourceDatabase.Query<long>($"SELECT CHANGE_TRACKING_CURRENT_VERSION()").Single();
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
