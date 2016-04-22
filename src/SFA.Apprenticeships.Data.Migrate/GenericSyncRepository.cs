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
    using System.Threading.Tasks;

    // TODO: Move SFA.Apprenticeships.Infrastructure.Repositories.Sql.Common back into its own project so that the false dependencies on
    // SFA.Apprenticeships.Avms.Domain.Entities and SFA.Apprenticeships.Domain.Interfaces can be removed.

    public class GenericSyncRespository : IGenericSyncRespository
    {
        private IGetOpenConnection _sourceDatabase;
        private IGetOpenConnection _targetDatabase;
        private ILogService _log;

        public GenericSyncRespository(ILogService log, IGetOpenConnection sourceDatabase, IGetOpenConnection targetDatabase)
        {
            _log = log;
            _sourceDatabase = sourceDatabase;
            _targetDatabase = targetDatabase;
        }

        public ISnapshotSyncContext StartChangesOnlySnapshotSync()
        {
            return new ChangesOnlySnapshotSyncContext(_log, _sourceDatabase, _targetDatabase);
        }

        public ITransactionlessSyncContext StartFullTransactionlessSync()
        {
            return new FullTransactionlessSyncContext(_log, _sourceDatabase, _targetDatabase);
        }

        public void BulkInsert(ITableDetails table, IEnumerable<IDictionary<string, object>> records)
        {
            if (!records.Any())
                return;

            var tempTable = "_InsertTemp_" + table.Name;
            var columnTypes = GetColumnTypes(records);

            using (var connection = (SqlConnection)_targetDatabase.GetOpenConnection())
            {
                if (tempTable == null)
                {
                    try
                    {
                        // Bulk insert straight into destination table
                        BulkInsert(connection, table.Name, records, columnTypes.Where(c => c.Value != null), SqlBulkCopyOptions.CheckConstraints | SqlBulkCopyOptions.KeepIdentity);
                    }
                    catch (Exception ex)
                    {
                        _log.Warn($"Error inserting record. This may be due to more data having been inserted since the parent table was processed. Therefore inserting what can be before continuing. Error was {ex.Message}");
                        CreateAndInsertIntoTemp(table, columnTypes, tempTable, connection, records);
                        var errors = InsertFromTempOneAtATime(table, columnTypes, tempTable, connection);
                        if (errors.Any())
                            throw new Exception($"Errors inserting into {table.Name}:\n{GetErrorText(errors)}");
                    }
                }
                else
                {
                    // Insert via temp table.
                    // This is sometimes required where there is a nullable foreign key due to a bug in SqlBulkCopy whereby inserts with NULL values are rejected

                    CreateAndInsertIntoTemp(table, columnTypes, tempTable, connection, records);
                    try
                    {
                        InsertFromTemp(table, columnTypes, tempTable, connection);
                    }
                    catch (Exception ex)
                    {
                        _log.Warn($"Error inserting record. This may be due to more data having been inserted since the parent table was processed. Therefore inserting what can be before continuing. Error was {ex.Message}");
                        CreateAndInsertIntoTemp(table, columnTypes, tempTable, connection, records);
                        var errors = InsertFromTempOneAtATime(table, columnTypes, tempTable, connection);
                        if (errors.Any())
                            throw new Exception($"Errors inserting into {table.Name}:\n{GetErrorText(errors)}");
                    }
                }
            }
        }


        public void BulkUpdate(ITableDetails table, IEnumerable<IDictionary<string, object>> records)
        {
            if (!records.Any())
                return;

            var tempTable = "_UpdateTemp_" + table.Name;
            var columnTypes = GetColumnTypes(records);
            var nonNullColumnTypes = columnTypes.Where(c => c.Value != null);

            using (var connection = (SqlConnection)_targetDatabase.GetOpenConnection())
            {
                CreateAndInsertIntoTemp(table, columnTypes, tempTable, connection, records);

                try
                {
                    UpdateFromTempInOneGo(table, columnTypes, tempTable, connection);
                }
                catch (Exception ex)
                {
                    _log.Warn($"Error updating record. Therefore updating what can be before continuing. Error was {ex.Message}");
                    var errors = UpdateFromTempOneAtATime(table, columnTypes, tempTable, connection);
                    if (errors.Any())
                        throw new Exception($"Errors inserting into {table.Name}:\n{GetErrorText(errors)}");
                }
            }

        }

        public void BulkDelete(ITableDetails table, IEnumerable<Keys> keys)
        {
            var tempTable = "_DeleteTemp_" + table.Name;
            var primaryKeysWithTypes = table.PrimaryKeys.ToDictionary(k => k, k => typeof(long));
            var records = keys.Select(k => k.ToDictionary(r => table.PrimaryKeys.First(), r => (object)r)); // TODO: Only works if one primary key

            using (var connection = (SqlConnection)_targetDatabase.GetOpenConnection())
            {
                CreateAndInsertIntoTemp(table, primaryKeysWithTypes, tempTable, connection, records);

                try
                {
                    DeleteFromTempInOneGo(table, primaryKeysWithTypes, tempTable, connection);
                }
                catch (Exception ex)
                {
                    _log.Warn($"Error deleting record. Therefore updating what can be before continuing. Error was {ex.Message}");
                    var errors = DeleteFromTempOneAtATime(table, primaryKeysWithTypes, tempTable, connection);
                    if (errors.Any())
                        throw new Exception($"Errors inserting into {table.Name}:\n{GetErrorText(errors)}");
                }
            }
        }

        #region BulkInsert / Update support methods

        private string GetErrorText(IEnumerable<dynamic> errors)
        {
            return $"{string.Join("\n", errors)}";
        }


        private void CreateAndInsertIntoTemp(ITableDetails table, IDictionary<string, Type> columnTypes, string tempTable, SqlConnection connection, IEnumerable<IDictionary<string, object>> records)
        {
            var nonNullColumnTypes = columnTypes.Where(c => c.Value != null);
            var columnsWithSqlTypes = nonNullColumnTypes.Select(col => $"{col.Key} {SqlTypeFor(col.Value)}");

            var sql = $"CREATE TABLE {tempTable} ({string.Join(", ", columnsWithSqlTypes)})";
            if (!tempTable.StartsWith("#"))
            {
                sql = $@"
BEGIN TRY
    DROP TABLE {tempTable}
END TRY
BEGIN CATCH
END CATCH

{sql}
";
            }
            connection.Execute(sql);

            BulkInsert(connection, tempTable, records, nonNullColumnTypes);
        }


        private void InsertFromTemp(ITableDetails table, IDictionary<String, Type> columnTypes, string tempTable, IDbConnection connection)
        {
            if (table.IdentityInsert)
            {
                connection.Execute($@"{GetSetIdentityInsertSql(table)};{GetInsertSql(table, columnTypes, tempTable)};");
            }
            else
            {
                foreach (var primaryKey in table.PrimaryKeys)
                {
                    columnTypes.Remove(primaryKey);
                }

                connection.Execute($@"{GetInsertSql(table, columnTypes, tempTable)};");
            }
        }


        private IEnumerable<dynamic> InsertFromTempOneAtATime(ITableDetails table, IDictionary<String, Type> columnTypes, string tempTable, IDbConnection connection)
        {
            string primaryKeyList = $"{string.Join(", ", table.PrimaryKeys)}";

            var sql = $@"
DECLARE @totalRecords INT = (SELECT COUNT(*) FROM {tempTable});
DECLARE @thisRecord   INT = 0;

CREATE TABLE #Errors ({string.Join(", ", table.PrimaryKeys.Select(k => new KeyValuePair<string, Type>(k, columnTypes[k])).Select(col => $"{col.Key} {SqlTypeFor(col.Value)}"))}, Error NVARCHAR(MAX))

{GetSetIdentityInsertSql(table)};

WHILE @thisRecord < @totalRecords
BEGIN
    BEGIN TRY
        {GetInsertSql(table, columnTypes, tempTable, "        ")}
        ORDER BY {primaryKeyList}
        OFFSET (@thisRecord) ROWS FETCH NEXT (1) ROWS ONLY;
    END TRY
    BEGIN CATCH
        INSERT INTO #Errors
        SELECT {primaryKeyList}, ERROR_MESSAGE()
        FROM   {tempTable}
        ORDER BY {primaryKeyList}
        OFFSET (@thisRecord) ROWS FETCH NEXT (1) ROWS ONLY;
    END CATCH
    SET @thisRecord = @thisRecord + 1;
END

SELECT * FROM #Errors
";

            return connection.Query<dynamic>(sql);
        }

        private string GetInsertSql(ITableDetails table, IDictionary<String, Type> columnTypes, string tempTable, string indent = "")
        {
            return $@"
INSERT INTO {table.Name}
      ({string.Join(", ", columnTypes.Select(c => c.Key))})
SELECT {string.Join(", ", columnTypes.Select(c => (c.Value == null) ? "NULL" : c.Key))}
FROM   {tempTable}"
.Replace(@"
", $@"
{indent}");
        }

        private string GetSetIdentityInsertSql(ITableDetails table)
        {
            return $@"
BEGIN TRY
    SET IDENTITY_INSERT {table.Name} ON;
END TRY
BEGIN CATCH
END CATCH";
        }

        private string GetColumnAssignments(ITableDetails table, IDictionary<String, Type> columnTypes)
        {
            return string.Join(", ", 
                columnTypes
                .Where(c => !table.PrimaryKeys.Contains(c.Key))
                .Select(c => (c.Value == null) ? $"[{c.Key}] = NULL" : $"[{c.Key}] = temp.[{c.Key}]")
                );
        }

        private void UpdateFromTempInOneGo(ITableDetails table, IDictionary<string, Type> columnTypes, string tempTable, IDbConnection connection)
        {
            connection.Execute($@"
UPDATE target
SET    {GetColumnAssignments(table, columnTypes)}
FROM   {table.Name} target
JOIN   {tempTable}  temp
ON     {string.Join(" AND ", table.PrimaryKeys.Select(k => $"temp.{k} = target.{k}"))};
");
        }

        private IEnumerable<dynamic> UpdateFromTempOneAtATime(ITableDetails table, IDictionary<string, Type> columnTypes, string tempTable, IDbConnection connection)
        {
            string primaryKeyList = $"{string.Join(", ", table.PrimaryKeys)}";

            var sql = $@"
DECLARE @totalRecords INT = (SELECT COUNT(*) FROM {tempTable});
DECLARE @thisRecord   INT = 0;

CREATE TABLE #Errors ({string.Join(", ", table.PrimaryKeys.Select(k => new KeyValuePair<string, Type>(k, columnTypes[k])).Select(col => $"{col.Key} {SqlTypeFor(col.Value)}"))}, Error NVARCHAR(MAX))

{GetSetIdentityInsertSql(table)};

WHILE @thisRecord < @totalRecords
BEGIN
    BEGIN TRY
        UPDATE target
        SET    {GetColumnAssignments(table, columnTypes)}
        FROM   {table.Name} target
        JOIN   (
               SELECT * FROM {tempTable}
               ORDER BY {primaryKeyList}
               OFFSET (@thisRecord) ROWS FETCH NEXT (1) ROWS ONLY
               ) temp
        ON     {string.Join(" AND ", table.PrimaryKeys.Select(k => $"temp.{k} = target.{k}"))};
    END TRY
    BEGIN CATCH
        INSERT INTO #Errors
        SELECT {primaryKeyList}, ERROR_MESSAGE()
        FROM   {tempTable}
        ORDER BY {primaryKeyList}
        OFFSET (@thisRecord) ROWS FETCH NEXT (1) ROWS ONLY;
    END CATCH
    SET @thisRecord = @thisRecord + 1;
END

SELECT * FROM #Errors
";
            return connection.Query<dynamic>(sql);
        }

        private void DeleteFromTempInOneGo(ITableDetails table, IDictionary<string, Type> primaryKeysWithTypes, string tempTable, IDbConnection connection)
        {
            connection.Execute($@"
DELETE target
FROM   {table.Name} target
JOIN   {tempTable} temp
ON     {string.Join(" AND ", primaryKeysWithTypes.Keys.Select(k => $"temp.{k} = target.{k}"))}
");
        }

        private IEnumerable<dynamic> DeleteFromTempOneAtATime(ITableDetails table, IDictionary<string, Type> primaryKeysWithTypes, string tempTable, IDbConnection connection)
        {
            string primaryKeyList = $"{string.Join(", ", table.PrimaryKeys)}";

            var sql = $@"
DECLARE @totalRecords INT = (SELECT COUNT(*) FROM {tempTable});
DECLARE @thisRecord   INT = 0;

CREATE TABLE #Errors ({string.Join(", ", table.PrimaryKeys.Select(k => new KeyValuePair<string, Type>(k, primaryKeysWithTypes[k])).Select(col => $"{col.Key} {SqlTypeFor(col.Value)}"))}, Error NVARCHAR(MAX))

WHILE @thisRecord < @totalRecords
BEGIN
    BEGIN TRY
        DELETE target
        FROM   {table.Name} target
        JOIN   (
               SELECT * FROM {tempTable}
               ORDER BY {primaryKeyList}
               OFFSET (@thisRecord) ROWS FETCH NEXT (1) ROWS ONLY
               ) temp
        ON     {string.Join(" AND ", table.PrimaryKeys.Select(k => $"temp.{k} = target.{k}"))};
    END TRY
    BEGIN CATCH
        INSERT INTO #Errors
        SELECT {primaryKeyList}, ERROR_MESSAGE()
        FROM   {tempTable}
        ORDER BY {primaryKeyList}
        OFFSET (@thisRecord) ROWS FETCH NEXT (1) ROWS ONLY;
    END CATCH
    SET @thisRecord = @thisRecord + 1;
END

SELECT * FROM #Errors
";
            return connection.Query<dynamic>(sql);
        }

        private static string SqlTypeFor(Type type)
        {
            if (type == typeof(string))
                return "NVARCHAR(MAX)";
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
            if (type == typeof(Guid))
                return "UNIQUEIDENTIFIER";

            throw new Exception($"No mapping from {type} to SQL Server type");
        }


        /// <summary>
        /// Every element in the list MUST contain the same keys and the same type of value.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data">Note that lists of many types of dynamic are suitable</param>
        /// <param name="tableName"></param>
        private static void BulkInsert(SqlConnection connection, string tableName, IEnumerable<IDictionary<string, object>> data, IEnumerable<KeyValuePair<string, Type>> columnTypes, SqlBulkCopyOptions options = SqlBulkCopyOptions.Default)
        {
            var dataTable = new DataTable();
            foreach (var column in columnTypes)
            {
                dataTable.Columns.Add(column.Key, column.Value);
            }

            foreach (var row in data)
            {
                var dataRow = dataTable.NewRow();
                foreach (var column in columnTypes)
                {
                    dataRow[column.Key] = row[column.Key] ?? DBNull.Value;
                }
                dataTable.Rows.Add(dataRow);
            }

            using (var bulkCopy = new SqlBulkCopy(connection, options, null))
            {
                // Mapping columns names is required whenever the data might not tie up positionwise with the target
                // This can happen for a variety of reasons and therefore safest to always map
                foreach (var column in columnTypes)
                {
                    bulkCopy.ColumnMappings.Add(column.Key, column.Key);
                }

                bulkCopy.DestinationTableName = tableName;
                bulkCopy.WriteToServer(dataTable);
            }
        }

        /// <summary>
        /// Get a mapping between column name and the CLR type associated with it.
        /// Where a column contains only null values the type cannot be determined and is therefore set to null.
        /// For inserts this can be dealt with by excluding them. For updates this can be dealt with by explicitly setting them to null.
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private static IDictionary<string,Type> GetColumnTypes(IEnumerable<IDictionary<string, object>> data)
        {
            var result = new Dictionary<string, Type>();
            bool first = true;
            int nulls = 0;
            foreach (var record in data)
            {
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

            return result;
        }

        #endregion


        public void DeleteAll(ITableSpec table)
        {
            long batchSize = (long)(50000 * table.BatchSizeMultiplier * table.BatchSizeMultiplier);
            int miniBatchSize = 100;

            _log.Info($"Deleting {table.Name} with a batch size of {batchSize}");

            var maxFirstId = _targetDatabase.Query<long?>($"SELECT MAX({table.PrimaryKeys.First()}) AS max FROM {table.Name}").Single();
            if (maxFirstId == null)
            {
                _log.Info($"{table.Name} is already empty");
            }
            else
            {
                bool again = true;
                while (again)
                {
                    again = false;
                    for (long lastIdInBatch = maxFirstId.Value; lastIdInBatch >= 0; lastIdInBatch -= batchSize)
                    {
                        _log.Info($" {table.Name} - {(maxFirstId.Value - lastIdInBatch) / Math.Max(maxFirstId.Value / 100, 1)}%, remaining {lastIdInBatch}");

                        try
                        {
                            _targetDatabase.MutatingQuery<int>($"DELETE {table.Name} WHERE {table.PrimaryKeys.First()} BETWEEN @fromId AND @toId", new { fromid = lastIdInBatch - batchSize, toId = lastIdInBatch }, commandTimeout: 120);
                        }
                        catch (Exception ex)
                        {
                            if (ex.Message.Contains(" SAME TABLE REFERENCE "))
                            {
                                again = true;
                                _log.Info($" Found SAME TABLE REFERENCE constraint. Deleting batch one by one");
                                for (long lastIdInMiniBatch = lastIdInBatch; lastIdInMiniBatch >= lastIdInBatch - batchSize; lastIdInMiniBatch -= miniBatchSize)
                                {
                                    int errors = _targetDatabase.MutatingQuery<int>($@"
DECLARE @Errors INT = 0

WHILE @Id >= @FirstId
BEGIN TRY
	SET @Id = @Id - 1
	DELETE Vacancy WHERE VacancyId = (@Id + 1)
END TRY
BEGIN CATCH
	SET @Errors = @Errors + 1
END CATCH

SELECT @Errors
", new { Id = lastIdInMiniBatch, FirstId = lastIdInMiniBatch - miniBatchSize }).Single();
                                    _log.Info($" Deleted batch with {errors} errors");
                                }

                            }
                            else
                            {
                                throw;
                            }
                        }
                    }
                }
            }
        }

        public void Reset()
        {
            _targetDatabase.MutatingQuery<int>("UPDATE sync.SyncParams SET LastSyncVersion = NULL");
        }

        private abstract class CommonSyncContext
        {
            protected ILogService _log;
            protected IGetOpenConnection _sourceDatabase;
            protected IGetOpenConnection _targetDatabase;

            protected long _nextSyncVersion { get; set; }

            public CommonSyncContext(ILogService log, IGetOpenConnection sourceDatabase, IGetOpenConnection targetDatabase)
            {
                _log = log;
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

            public ChangesOnlySnapshotSyncContext(ILogService log, IGetOpenConnection sourceDatabase, IGetOpenConnection targetDatabase) : base(log, sourceDatabase, targetDatabase)
            {
                var lastSyncVersion = _targetDatabase.Query<long?>($"SELECT LastSyncVersion FROM Sync.SyncParams").Single();
                if (lastSyncVersion == null)
                    throw new FullScanRequiredException();
                _lastSyncVersion = lastSyncVersion.Value;
                _log.Info($"LastSyncVersion={_lastSyncVersion}");

                try
                {
                    _sourceConnection = _sourceDatabase.GetOpenConnection();
                    _sourceConnection.Execute("SET TRANSACTION ISOLATION LEVEL SNAPSHOT");
                    _sourceTransaction = _sourceConnection.BeginTransaction();
                    _nextSyncVersion = _sourceConnection.Query<long>($"SELECT CHANGE_TRACKING_CURRENT_VERSION()", transaction: _sourceTransaction, buffered: false).Single();
                    _log.Info($"NextSyncVersion={_nextSyncVersion}");
                }
                catch (Exception)
                {
                    Dispose();
                    throw;
                }
            }

            public bool AreAnyChanges()
            {
                return _nextSyncVersion > _lastSyncVersion;
            }

            public IEnumerable<IChangeTableRow> GetChangesForTable(ITableDetails table)
            {
                long? minValidVersion = _sourceConnection.Query<long?>($"SELECT CHANGE_TRACKING_MIN_VALID_VERSION(OBJECT_ID('{table.Name}'))", transaction: _sourceTransaction, buffered: false).Single();
                if (minValidVersion == null)
                    throw new Exception($"Change tracking not enabled, unknown table or insufficient permission on {table.Name}");
                if (minValidVersion > _lastSyncVersion)
                    throw new FullScanRequiredException();

                // Note: The ordering of the results from CHANGETABLE is undefined and therefore it is difficult to do them in batches. Accordingly ideally the results would not be buffered.
                // However, without enabling SQL Server MARS, this will not allow other queries to proceed at the same time.
                var result = _sourceConnection.Query<dynamic>($"SELECT * FROM CHANGETABLE(CHANGES {table.Name}, @lastSyncVersion) AS Dummy", new { lastSyncVersion = _lastSyncVersion }, transaction: _sourceTransaction);

                return result.Select(row => new ChangeTableRow(row, table));
            }

            public class ChangeTableRow : IChangeTableRow
            {
                public ChangeTableRow(dynamic data, ITableDetails tableDetails)
                {
                    PrimaryKeys = Keys.GetPrimaryKeys(data, tableDetails);
                    ChangeVersion = data.SYS_CHANGE_VERSION;
                    CreationVersion = data.SYS_CHANGE_CREATION_VERSION;
                    switch ((string)data.SYS_CHANGE_OPERATION)
                    {
                        case "I":
                            Operation = Operation.Insert;
                            break;
                        case "U":
                            Operation = Operation.Update;
                            break;
                        case "D":
                            Operation = Operation.Delete;
                            break;
                        default:
                            throw new Exception($"Unknown change {data.SYS_CHANGE_OPERATION} for keys {PrimaryKeys}");
                    }
                }

                public long ChangeVersion { get; private set; }
                public long? CreationVersion { get; private set; }
                public Operation Operation { get; private set; }
                public IKeys PrimaryKeys { get; private set; }
            }


            public Task<IEnumerable<dynamic>> GetSourceRecordsAsync(ITableDetails table, IEnumerable<IKeys> keys)
            {
                return GetChangedRecords(_sourceConnection, _sourceTransaction, table, keys);
            }

            public IEnumerable<dynamic> GetTargetRecords(ITableDetails table, IEnumerable<IKeys> keys)
            {
                using (var connection = _targetDatabase.GetOpenConnection())
                {
                    return GetChangedRecords(connection, null, table, keys).Result.ToList();
                }
            }


            private static Task<IEnumerable<dynamic>> GetChangedRecords(IDbConnection connection, IDbTransaction transaction, ITableDetails table, IEnumerable<IKeys> keys)
            {
                const string tempTable = "#GetChangedRecordsTemp";

                var dataTable = new DataTable();
                foreach (var columnName in table.PrimaryKeys)
                {
                    dataTable.Columns.Add(columnName, typeof(long));
                }

                foreach (var row in keys)
                {
                    var dataRow = dataTable.NewRow();

                    var keyEnum = row.GetEnumerator();
                    foreach (var columnName in table.PrimaryKeys)
                    {
                        keyEnum.MoveNext();
                        dataRow[columnName] = (object)keyEnum.Current ?? DBNull.Value;
                    }
                    dataTable.Rows.Add(dataRow);
                }


                var columnsWithSqlTypes = table.PrimaryKeys.Select(name => $"{name} BIGINT");
                connection.Execute($"CREATE TABLE {tempTable} ({string.Join(", ", columnsWithSqlTypes)})", transaction: transaction);

                using (var bulkCopy = new SqlBulkCopy((SqlConnection)connection, SqlBulkCopyOptions.Default, (SqlTransaction)transaction)) // TODO: Nasty casts
                {
                    bulkCopy.DestinationTableName = tempTable;
                    bulkCopy.WriteToServer(dataTable);
                }

                return Task.FromResult(connection.Query<dynamic>($@"
SELECT a.*
FROM   {table.Name} a
JOIN   {tempTable}  b
  ON   {string.Join(" AND ", table.PrimaryKeys.Select(k => $"b.{k} = a.{k}"))}
ORDER BY {string.Join(", ", table.PrimaryKeys.Select(name => $"a.{name}"))}

DROP TABLE {tempTable}
", transaction: transaction));
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
            public FullTransactionlessSyncContext(ILogService log, IGetOpenConnection sourceDatabase, IGetOpenConnection targetDatabase) : base(log, sourceDatabase, targetDatabase)
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

            public long GetMaxFirstId(ITableDetails table)
            {
                return _sourceDatabase.Query<long>($"SELECT MAX({table.PrimaryKeys.First()}) FROM {table.Name}").Single();
            }

            public async Task<IEnumerable<dynamic>> GetSourceRecordsAsync(ITableDetails table, long startFirstId, long endFirstId)
            {
                using (var conn = _sourceDatabase.GetOpenConnection())
                {
                    return await conn.QueryAsync<dynamic>($"SELECT * FROM {table.Name} WHERE {table.PrimaryKeys.First()} BETWEEN @StartId AND @EndId ORDER BY {table.PrimaryKeys.First()}", new { StartId = startFirstId, EndId = endFirstId });
                }
                // return Task.FromResult(_sourceDatabase.QueryProgressive<dynamic>($"SELECT * FROM {table.Name} WHERE {table.PrimaryKey} BETWEEN @StartId AND @EndId ORDER BY {table.PrimaryKey}", new { StartId = startId, EndId = endId }));
            }

            public IEnumerable<dynamic> GetTargetRecords(ITableDetails table, long startFirstId, long endFirstId)
            {
                return _targetDatabase.Query<dynamic>($"SELECT * FROM {table.Name} WHERE {table.PrimaryKeys.First()} BETWEEN @StartId AND @EndId ORDER BY {table.PrimaryKeys.First()}", new { StartId = startFirstId, EndId = endFirstId });
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
