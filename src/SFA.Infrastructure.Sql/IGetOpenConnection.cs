using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Dapper.Contrib.Extensions;

namespace SFA.Infrastructure.Sql
{
    public interface IGetOpenConnection
    {
        SqlConnection GetOpenConnection();
    }

    public class GetOpenConnectionFromConnectionString : IGetOpenConnection
    {
        public string ConnectionString { get; private set; }

        public GetOpenConnectionFromConnectionString(string connectionString)
        {
            ConnectionString = connectionString;
        }

        public SqlConnection GetOpenConnection()
        {
            var conn = new SqlConnection(ConnectionString);
            conn.Open();
            return conn;
        }
    }

    public static class IGetOpenConnectionQueryExtensions
    {
        public static IList<T> Query<T>(this IGetOpenConnection goc, string sql, object param = null, int? commandTimeout = default(int?), CommandType? commandType = default(CommandType?))
        {
            // TODO: Log that user did this query
            // TODO: Retries
            using (var conn = goc.GetOpenConnection())
            {
                return (IList<T>)conn.Query<T>(sql, param, transaction: null, buffered: true, commandTimeout: commandTimeout, commandType: commandType);
            }
        }

        /// <summary>
        /// Progressively load the results as they are being iterated through. 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="goc"></param>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <param name="commandTimeout"></param>
        /// <param name="commandType"></param>
        /// <returns>An enumerable of the results. This must either be fully iterated or disposed of to avoid a resource leak. Using foreach will automatically dispose.</returns>
        public static IEnumerable<T> QueryProgressive<T>(this IGetOpenConnection goc, string sql, object param = null, int? commandTimeout = default(int?), CommandType? commandType = default(CommandType?))
        {
            using (var conn = goc.GetOpenConnection())
            {
                // TODO: Log that user did this query
                // TODO: Retries
                var results = conn.Query<T>(sql, param, transaction: null, buffered: false, commandTimeout: commandTimeout, commandType: commandType);

                // Need to keep the database connection open until have finished iterating through the results
                foreach (var row in results)
                {
                    yield return row;
                }

            }
        }

        public static IList<T> QueryCached<T>(this IGetOpenConnection goc, TimeSpan cacheDuration, string sql, object param = null, int? commandTimeout = default(int?), CommandType? commandType = default(CommandType?))
        {
            // TODO: Implement caching
            // TODO: Retries
            return goc.Query<T>(sql, param, commandTimeout, commandType);
        }
    }

    public static class IGetOpenConnectionMutateExtensions
    {
        public static long Insert<T>(this IGetOpenConnection goc, T entity, int? commandTimeout = null) where T : class
        {
            // TODO: Log that user did this query
            // TODO: Retries
            using (var conn = goc.GetOpenConnection())
            {
                return conn.Insert<T>(entity, null, commandTimeout);
            }
        }

        public static bool Update<T>(this IGetOpenConnection goc, T entity, int? commandTimeout = null) where T : class
        {
            // TODO: Log that user did this query
            // TODO: Retries
            using (var conn = goc.GetOpenConnection())
            {
                return conn.Update<T>(entity, null, commandTimeout);
            }
        }

        public static bool Delete<T>(this IGetOpenConnection goc, T entity, int? commandTimeout = null) where T : class
        {
            // TODO: Log that user did this query
            // TODO: Retries
            using (var conn = goc.GetOpenConnection())
            {
                return conn.Delete<T>(entity, null, commandTimeout);
            }
        }

        public static IList<T> MutatingQuery<T>(this IGetOpenConnection goc, string sql, object param = null, int? commandTimeout = default(int?), CommandType? commandType = default(CommandType?))
        {
            // TODO: Log that user did this query
            // TODO: Retries
            using (var conn = goc.GetOpenConnection())
            {
                return (IList<T>)conn.Query<T>(sql, param, transaction: null, buffered: true, commandTimeout: commandTimeout, commandType: commandType);
            }
        }

    }
}
