namespace SFA.Apprenticeships.Data.Migrate
{
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using System.Collections;

    public interface IGenericSyncRespository
    {
        ISnapshotSyncContext StartChangesOnlySnapshotSync();

        ITransactionlessSyncContext StartFullTransactionlessSync();

        void BulkInsert(ITableDetails table, IEnumerable<IDictionary<string, object>> records);
        void BulkUpdate(ITableDetails table, IEnumerable<IDictionary<string, object>> records);
        void BulkDelete(ITableDetails table, IEnumerable<Keys> keys);

        void DeleteAll(ITableSpec table);

        void Reset();
    }

    public interface ISnapshotSyncContext : IDisposable
    {
        bool AreAnyChanges();

        IEnumerable<IChangeTableRow> GetChangesForTable(ITableDetails table);

        Task<IEnumerable<dynamic>> GetSourceRecordsAsync(ITableDetails table, IEnumerable<IKeys> keys);
        IEnumerable<dynamic> GetTargetRecords(ITableDetails table, IEnumerable<IKeys> keys);

        void Success();
    }

    public interface ITransactionlessSyncContext
    {
        long GetMaxFirstId(ITableDetails table);
        Task<IEnumerable<dynamic>> GetSourceRecordsAsync(ITableDetails table, long startFirstId, long endFirstId);
        IEnumerable<dynamic> GetTargetRecords(ITableDetails table, long startFirstId, long endFirstId);

        void Success();
    }

    public interface ITableDetails
    {
        string Name { get; }

        IEnumerable<string> PrimaryKeys { get; }
    }

    public class FullScanRequiredException : Exception
    {
        public FullScanRequiredException() : base("Full scan required (change tracking does not go back far enough)")
        { }
    }

    public class FatalException : Exception
    {
        public FatalException(string message) : base(message)
        { }

    }

    public enum Operation { Insert, Update, Delete, Unknown };

    public interface IKeys : IComparable<IKeys>, IEnumerable<long>
    {
        int Length { get; }
    }

    public class Keys : IKeys
    {
        private static IKeys _maxValue = new Keys(new long[0] { });
        public static IKeys MaxValue { get { return _maxValue; } }

        private long[] _key;

        public static Keys GetPrimaryKeys(IDictionary<string, object> record, ITableDetails table)
        {
            var keys = new List<long>();
            foreach (var key in table.PrimaryKeys)
            {
                var sourceId = record[key];
                if (sourceId == null)
                    throw new FatalException($"Unknown column (may be case sensitive) or null value for {key} on {table.Name}");
                if (sourceId is long)
                    keys.Add((long)sourceId);
                else if (sourceId is int)
                    keys.Add((long)(int)sourceId);
                else if (sourceId is short)
                    keys.Add((long)(short)sourceId);
                else
                    throw new FatalException($"Unknown type {sourceId.GetType()}");
            }

            return new Keys(keys);
        }


        public Keys(IEnumerable<long> values)
        {
            _key = values.ToArray();
        }

        public int CompareTo(IKeys other)
        {
            if (this != MaxValue && other == MaxValue)
                return -1;
            if (this == MaxValue && other == MaxValue)
                return 0;
            if (this == MaxValue && other != MaxValue)
                return +1;

            if (this.Length != other.Length)
                throw new ArgumentException("Lengths differ");

            int result = 0;

            var thisKeyEnumerator = this.GetEnumerator();
            foreach (var otherKey in other)
            {
                thisKeyEnumerator.MoveNext();
                result = thisKeyEnumerator.Current.CompareTo(otherKey);
                if (result != 0)
                    break;
            }

            return result;
        }

        public int Length { get { return _key.Length; } }

        public override string ToString()
        {
            return this == MaxValue ? "MaxValue" : string.Join(", ", _key);
        }

        public IEnumerator<long> GetEnumerator()
        {
            return ((IEnumerable<long>)_key).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

    public interface IChangeTableRow
    {
        long ChangeVersion { get; }
        long? CreationVersion { get; }
        Operation Operation { get; }
        IKeys PrimaryKeys { get; }
    }
}
