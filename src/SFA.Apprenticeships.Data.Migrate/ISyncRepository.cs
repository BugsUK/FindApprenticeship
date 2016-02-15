
namespace SFA.Apprenticeships.Data.Migrate
{
    using Infrastructure.Repositories.Sql.Common;
    using System;
    using System.Data;
    using System.Linq;
    using Dapper;
    using System.Collections.Generic;

    public interface ISyncRespository
    {
        ISnapshotSyncContext StartChangesOnlySnapshotSync();

        ITransactionlessSyncContext StartFullTransactionlessSync();

        void BulkInsert(ITableDetails table, IReadOnlyList<IDictionary<string, object>> records);
        void BulkUpdate(ITableDetails table, IReadOnlyList<IDictionary<string, object>> records);
    }

    public interface ISnapshotSyncContext : IDisposable
    {
        IEnumerable<ChangeTableRow> GetChanges(string tableName);

        IEnumerable<dynamic> GetSourceRecords(ITableDetails table, IEnumerable<long> ids);
        IEnumerable<dynamic> GetTargetRecords(ITableDetails table, IEnumerable<long> ids);

        void Success();
    }

    public interface ITransactionlessSyncContext
    {
        long GetMaxId(ITableDetails table);
        IEnumerable<dynamic> GetSourceRecords(ITableDetails table, long startId, long endId);
        IEnumerable<dynamic> GetTargetRecords(ITableDetails table, long startId, long endId);

        void Success();
    }

    public interface ITableDetails
    {
        string Name { get; }
        string PrimaryKey { get; }
    }

    public class FullScanRequiredException : Exception
    {
        public FullScanRequiredException() : base("Full scan required (change tracking does not go back far enough)")
        { }
    }

    public enum Operation { Insert, Update, Delete, Unknown };

    public class ChangeTableRow
    {
        public long SYS_CHANGE_VERSION;
        public long SYS_CHANGE_CREATION_VERSION;
        public string SYS_CHANGE_OPERATION;
        public string SYS_CHANGE_COLUMNS;
        public string SYS_CHANGE_CONTEXT;
        public long Id;

        public Operation Operation
        {
            get
            {
                switch (SYS_CHANGE_OPERATION)
                {
                    case "I":
                        return Operation.Insert;
                    case "U":
                        return Operation.Update;
                    case "D":
                        return Operation.Delete;
                    default:
                        throw new Exception($"Unknown change {SYS_CHANGE_OPERATION} for id {Id}");
                }
            }
        }
    }
}
