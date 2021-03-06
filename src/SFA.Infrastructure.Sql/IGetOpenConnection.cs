﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Dapper.Contrib.Extensions;
using Microsoft.Practices.EnterpriseLibrary.TransientFaultHandling;
using System.Diagnostics;

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

    /// <summary>
    /// Dapper-like database access methods. Intended to be used for read-only queries, perhaps on a read-only database connection.
    /// </summary>
    public static class IGetOpenConnectionQueryExtensions
    {
        public static RetryStrategy RetryStrategy = new Incremental(5, TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(2));
        public static RetryPolicy RetryPolicy = new RetryPolicy<SqlDatabaseTransientErrorDetectionStrategy>(RetryStrategy);

        /// <summary>
        /// Execute a query. Very similar to Dapper's IDbConnection.Query except:
        /// <list type="bullet">
        ///     <item><description>It manages (obtains, opens, closes and returns) the database connection itself</description></item> 
        ///     <item><description>The result is always entirely loaded and returned as an IList ("buffered" cannot be set to false)</description></item>
        ///     <item><description>Transient errors are automatically retried</description></item> 
        ///     <item><description>Transactions are not supported (in order to support retries)</description></item> 
        /// </list>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="goc"></param>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <param name="commandTimeout"></param>
        /// <param name="commandType"></param>
        /// <returns></returns>
        public static IList<T> Query<T>(this IGetOpenConnection goc, string sql, object param = null, int? commandTimeout = default(int?), CommandType? commandType = default(CommandType?))
        {
            // TODO: Log that user did this query

            return RetryPolicy.ExecuteAction<IList<T>>(() =>
            {
                using (var conn = goc.GetOpenConnection())
                {
                    try
                    {
                        return
                            (IList<T>)
                                conn.Query<T>(sql, param, transaction: null, buffered: true,
                                    commandTimeout: commandTimeout, commandType: commandType);
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
            }
            );
        }

        /// <summary>
        /// Execute a query and progressively load the data. Very similar to Dapper's IDbConnection.Query except:
        /// <list type="bullet">
        ///     <item><description>It manages (obtains, opens, closes and returns) the database connection itself</description></item> 
        ///     <item><description>Data will be progressively loaded as the result is iterated through (i.e. buffered=false in Dapper).</description></item>
        ///     <item><description>Transient errors are automatically retried, up to and including the first row (only)</description></item> 
        ///     <item><description>Transactions are not supported (in order to support retries)</description></item> 
        /// </list>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="goc"></param>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <param name="commandTimeout"></param>
        /// <param name="commandType"></param>
        /// <returns>An enumerable of the results. This must either be fully iterated or disposed of to avoid a resource leak. Using foreach will automatically dispose. It can only be iterated through once.</returns>
        /// <remarks>Once the first value has been returned any transient errors will not be retried. Ideally the caller would be carrying out an idempotent operation and would retry from the beginning.
        /// Transient errors can be detected with "new SqlDatabaseTransientErrorDetectionStrategy().IsTransient(ex)"</remarks>
        public static IEnumerable<T> QueryProgressive<T>(this IGetOpenConnection goc, string sql, object param = null, int? commandTimeout = default(int?), CommandType? commandType = default(CommandType?))
        {
            // TODO: Log that user did this query

            IDbConnection conn = null;
            IEnumerator<T> enumerator = null;
            try
            {
                var hasFirstRecord = RetryPolicy.ExecuteAction<bool>(() =>
                {
                    conn = goc.GetOpenConnection();
                    var results = conn.Query<T>(sql, param, transaction: null, buffered: false, commandTimeout: commandTimeout, commandType: commandType);
                    enumerator = results.GetEnumerator();
                    return enumerator.MoveNext();
                });

                Debug.Assert(enumerator != null);

                if (hasFirstRecord)
                    yield return enumerator.Current;

                while (enumerator.MoveNext())
                    yield return enumerator.Current;
            }
            finally
            {
                if (enumerator != null)
                    enumerator.Dispose();
                if (conn != null)
                    conn.Dispose();
            }
        }

        /// <summary>
        /// Execute a query that returns multiple datasets. Similar in principal to Dapper's IDbConnection.QueryMultiple except:
        /// <list type="bullet">
        ///     <item><description>It manages (obtains, opens, closes and returns) the database connection itself</description></item>
        ///     <item><description>All the results are fully loaded and returned in a Tuple of ILists (Dapper defers loading until GridReader.Read is called)</description></item>      
        ///     <item><description>Transient errors are automatically retried</description></item> 
        ///     <item><description>Transactions are not supported (in order to support retries)</description></item> 
        /// </list>
        /// </summary>
        /// <param name="goc"></param>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <param name="commandTimeout"></param>
        /// <param name="commandType"></param>
        /// <returns>An enumerable of the results. This must either be fully iterated or disposed of to avoid a resource leak. Using foreach will automatically dispose. It can only be iterated through once.</returns>
        /// <remarks>Once the first value has been returned any transient errors will not be retried. Ideally the caller would be carrying out an idempotent operation and would retry from the beginning.
        /// Transient errors can be detected with "new SqlDatabaseTransientErrorDetectionStrategy().IsTransient(ex)"</remarks>
        public static Tuple<IList<T1>,IList<T2>> QueryMultiple<T1,T2>(this IGetOpenConnection goc, string sql, object param = null, int? commandTimeout = default(int?), CommandType? commandType = default(CommandType?))
        {
            // TODO: Log that user did this query

            return RetryPolicy.ExecuteAction<Tuple<IList<T1>, IList<T2>>>(() =>
            {
                using (var conn = goc.GetOpenConnection())
                {
                    var allResults = conn.QueryMultiple(sql, param, transaction: null, commandTimeout: commandTimeout, commandType: commandType);
                    return new Tuple<IList<T1>, IList<T2>>((IList<T1>)allResults.Read<T1>(), (IList<T2>)allResults.Read<T2>());
                }
            }
            );
        }

        /// <summary>
        /// Execute a query that returns multiple datasets. Similar in principal to Dapper's IDbConnection.QueryMultiple except:
        /// <list type="bullet">
        ///     <item><description>It manages (obtains, opens, closes and returns) the database connection itself</description></item>
        ///     <item><description>All the results are fully loaded and returned in a Tuple of ILists (Dapper defers loading until GridReader.Read is called)</description></item>      
        ///     <item><description>Transient errors are automatically retried</description></item> 
        ///     <item><description>Transactions are not supported (in order to support retries)</description></item> 
        /// </list>
        /// </summary>
        /// <param name="goc"></param>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <param name="commandTimeout"></param>
        /// <param name="commandType"></param>
        /// <returns>An enumerable of the results. This must either be fully iterated or disposed of to avoid a resource leak. Using foreach will automatically dispose. It can only be iterated through once.</returns>
        /// <remarks>Once the first value has been returned any transient errors will not be retried. Ideally the caller would be carrying out an idempotent operation and would retry from the beginning.
        /// Transient errors can be detected with "new SqlDatabaseTransientErrorDetectionStrategy().IsTransient(ex)"</remarks>
        public static Tuple<IList<T1>, IList<T2>, IList<T3>> QueryMultiple<T1, T2, T3>(this IGetOpenConnection goc, string sql, object param = null, int? commandTimeout = default(int?), CommandType? commandType = default(CommandType?))
        {
            // TODO: Log that user did this query

            return RetryPolicy.ExecuteAction<Tuple<IList<T1>, IList<T2>, IList<T3>>>(() =>
            {
                using (var conn = goc.GetOpenConnection())
                {
                    var allResults = conn.QueryMultiple(sql, param, transaction: null, commandTimeout: commandTimeout, commandType: commandType);
                    return new Tuple<IList<T1>, IList<T2>, IList<T3>>((IList<T1>)allResults.Read<T1>(), (IList<T2>)allResults.Read<T2>(), (IList<T3>)allResults.Read<T3>());
                }
            }
            );
        }

        /// <summary>
        /// Execute a query that returns multiple datasets. Similar in principal to Dapper's IDbConnection.QueryMultiple except:
        /// <list type="bullet">
        ///     <item><description>It manages (obtains, opens, closes and returns) the database connection itself</description></item>
        ///     <item><description>All the results are fully loaded and returned in a Tuple of ILists (Dapper defers loading until GridReader.Read is called)</description></item>      
        ///     <item><description>Transient errors are automatically retried</description></item> 
        ///     <item><description>Transactions are not supported (in order to support retries)</description></item> 
        /// </list>
        /// </summary>
        /// <param name="goc"></param>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <param name="commandTimeout"></param>
        /// <param name="commandType"></param>
        /// <returns>An enumerable of the results. This must either be fully iterated or disposed of to avoid a resource leak. Using foreach will automatically dispose. It can only be iterated through once.</returns>
        /// <remarks>Once the first value has been returned any transient errors will not be retried. Ideally the caller would be carrying out an idempotent operation and would retry from the beginning.
        /// Transient errors can be detected with "new SqlDatabaseTransientErrorDetectionStrategy().IsTransient(ex)"</remarks>
        public static Tuple<IList<T1>, IList<T2>, IList<T3>, IList<T4>> QueryMultiple<T1, T2, T3, T4>(this IGetOpenConnection goc, string sql, object param = null, int? commandTimeout = default(int?), CommandType? commandType = default(CommandType?))
        {
            // TODO: Log that user did this query

            return RetryPolicy.ExecuteAction<Tuple<IList<T1>, IList<T2>, IList<T3>, IList<T4>>>(() =>
            {
                using (var conn = goc.GetOpenConnection())
                {
                    var allResults = conn.QueryMultiple(sql, param, transaction: null, commandTimeout: commandTimeout, commandType: commandType);
                    return new Tuple<IList<T1>, IList<T2>, IList<T3>, IList<T4>>(
                        (IList<T1>)allResults.Read<T1>(),
                        (IList<T2>)allResults.Read<T2>(),
                        (IList<T3>)allResults.Read<T3>(),
                        (IList<T4>)allResults.Read<T4>());
                }
            }
            );
        }

        /// <summary>
        /// Execute a query that returns multiple datasets. Similar in principal to Dapper's IDbConnection.QueryMultiple except:
        /// <list type="bullet">
        ///     <item><description>It manages (obtains, opens, closes and returns) the database connection itself</description></item>
        ///     <item><description>All the results are fully loaded and returned in a Tuple of ILists (Dapper defers loading until GridReader.Read is called)</description></item>      
        ///     <item><description>Transient errors are automatically retried</description></item> 
        ///     <item><description>Transactions are not supported (in order to support retries)</description></item> 
        /// </list>
        /// </summary>
        /// <param name="goc"></param>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <param name="commandTimeout"></param>
        /// <param name="commandType"></param>
        /// <returns>An enumerable of the results. This must either be fully iterated or disposed of to avoid a resource leak. Using foreach will automatically dispose. It can only be iterated through once.</returns>
        /// <remarks>Once the first value has been returned any transient errors will not be retried. Ideally the caller would be carrying out an idempotent operation and would retry from the beginning.
        /// Transient errors can be detected with "new SqlDatabaseTransientErrorDetectionStrategy().IsTransient(ex)"</remarks>
        public static Tuple<IList<T1>, IList<T2>, IList<T3>, IList<T4>, IList<T5>> QueryMultiple<T1, T2, T3, T4, T5>(this IGetOpenConnection goc, string sql, object param = null, int? commandTimeout = default(int?), CommandType? commandType = default(CommandType?))
        {
            // TODO: Log that user did this query

            return RetryPolicy.ExecuteAction<Tuple<IList<T1>, IList<T2>, IList<T3>, IList<T4>, IList<T5>>>(() =>
            {
                using (var conn = goc.GetOpenConnection())
                {
                    var allResults = conn.QueryMultiple(sql, param, transaction: null, commandTimeout: commandTimeout, commandType: commandType);
                    return new Tuple<IList<T1>, IList<T2>, IList<T3>, IList<T4>, IList<T5>>(
                        (IList<T1>)allResults.Read<T1>(),
                        (IList<T2>)allResults.Read<T2>(),
                        (IList<T3>)allResults.Read<T3>(),
                        (IList<T4>)allResults.Read<T4>(),
                        (IList<T5>)allResults.Read<T5>());
                }
            }
            );
        }

        /// <summary>
        /// Return cached data for the query and associated parameters if available, otherwise query the data and add it to the cache. In the query case
        /// it is very similar to Dapper's IDbConnection.Query except:
        /// <list type="bullet">
        ///     <item><description>It manages (obtains, opens, closes and returns) the database connection itself</description></item> 
        ///     <item><description>The result is always entirely loaded and returned as an IList ("buffered" cannot be set to false)</description></item>
        ///     <item><description>Transient errors are automatically retried</description></item> 
        ///     <item><description>Transactions are not supported (in order to support retries)</description></item> 
        /// </list>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="goc"></param>
        /// <param name="cacheDuration">This is advisory only. It end up being cached for less time (perhaps due to memory limitations) or longer
        /// (perhaps if the query fails, or is slow).</param>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <param name="commandTimeout"></param>
        /// <param name="commandType"></param>
        /// <returns></returns>
        /// <remarks>Once the first value has been returned any transient errors will not be retried. Ideally the caller would be carrying out an idempotent operation and would retry from the beginning.
        public static IList<T> QueryCached<T>(this IGetOpenConnection goc, TimeSpan cacheDuration, string sql, object param = null, int? commandTimeout = default(int?), CommandType? commandType = default(CommandType?))
        {
            // TODO: Implement caching. Consider using older values in case of error / slow response

            return goc.Query<T>(sql, param, commandTimeout, commandType);
        }
    }

    /// <summary>
    /// Dapper / Dapper.Contrib.SqlMapperExtensions-like database access methods. Intended to be used for mutating operations only.
    /// </summary>
    public static class IGetOpenConnectionMutateExtensions
    {
        public static RetryStrategy RetryStrategy = new Incremental(5, TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(2));
        public static RetryPolicy RetryPolicy = new RetryPolicy<SqlDatabaseTransientErrorDetectionStrategy>(RetryStrategy);

        /// <summary>
        /// Insert a new record containing data from the specified object. The primary key in the specified record must be zero.
        /// Very similar to Dapper.Contrib.SqlMapperExtensions Insert method except:
        /// <list type="bullet">
        ///     <item><description>It manages (obtains, opens, closes and returns) the database connection itself</description></item> 
        ///     <item><description>Transient errors are automatically retried</description></item> 
        ///     <item><description>Transactions are not supported (in order to support retries)</description></item> 
        ///     <item><description>Table / primary key naming conventions are &lt;EntityName>.&lt;EntityName>Id rather than &lt;EntityName>s.Id</description></item> 
        /// </list>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="goc"></param>
        /// <param name="entity"></param>
        /// <param name="commandTimeout"></param>
        /// <returns>The primary key of the </returns>
        public static long Insert<T>(this IGetOpenConnection goc, T entity, int? commandTimeout = null) where T : class
        {
            // TODO: Log that user did this query
            return RetryPolicy.ExecuteAction<long>(() =>
            {
                using (var conn = goc.GetOpenConnection())
                {
                    return conn.Insert<T>(entity, null, commandTimeout);
                }
            });
        }

        /// <summary>
        /// Update the single record with a primary key matching the specified object.
        /// Very similar to Dapper.Contrib.SqlMapperExtensions Update method except:
        /// <list type="bullet">
        ///     <item><description>It manages (obtains, opens, closes and returns) the database connection itself</description></item> 
        ///     <item><description>Transient errors are automatically retried</description></item> 
        ///     <item><description>Transactions are not supported (in order to support retries)</description></item> 
        ///     <item><description>Table / primary key naming conventions are &lt;EntityName>.&lt;EntityName>Id rather than &lt;EntityName>s.Id</description></item>
        ///     <item><description>It only supports updating a single record at a time and may rollback / throw an exception if more than one is affected</description></item>      
        /// </list>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="goc"></param>
        /// <param name="entity"></param>
        /// <param name="commandTimeout"></param>
        /// <returns>true if at least one record was updated, otherwise false</returns>
        /// <remarks>Consider issuing a custom update if not all columns are changed.</remarks>
        public static bool UpdateSingle<T>(this IGetOpenConnection goc, T entity, int? commandTimeout = null) where T : class
        {
            // TODO: Log that user did this query
            // TODO: Do in a transaction and check that only one record updated before committing
            return RetryPolicy.ExecuteAction<bool>(() =>
            {
                using (var conn = goc.GetOpenConnection())
                {
                    return conn.Update<T>(entity, null, commandTimeout);
                }
            });
        }

        /// <summary>
        /// Delete the single record with a primary key matching the specified object.
        /// Very similar to Dapper.Contrib.SqlMapperExtensions Delete method except:
        /// <list type="bullet">
        ///     <item><description>It manages (obtains, opens, closes and returns) the database connection itself</description></item> 
        ///     <item><description>Transient errors are automatically retried</description></item> 
        ///     <item><description>Transactions are not supported (in order to support retries)</description></item> 
        ///     <item><description>Table / primary key naming conventions are &lt;EntityName>.&lt;EntityName>Id rather than &lt;EntityName>s.Id</description></item>
        ///     <item><description>It only supports deleting a single record at a time and may rollback / throw an exception if more than one is affected</description></item>      
        /// </list>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="goc"></param>
        /// <param name="entity"></param>
        /// <param name="commandTimeout"></param>
        /// <returns></returns>
        public static bool DeleteSingle<T>(this IGetOpenConnection goc, T entity, int? commandTimeout = null) where T : class
        {
            // TODO: Replace with method that takes primary key (does this have the same design fault as entity framework?)
            // TODO: Log that user did this query
            // TODO: Do in a transaction and check that only one record deleted before committing
            return RetryPolicy.ExecuteAction<bool>(() =>
            {
                using (var conn = goc.GetOpenConnection())
                {
                    return conn.Delete<T>(entity, null, commandTimeout);
                }
            });
        }

        /// <summary>
        /// Peform a mutating query returning the specified type. This may update multiple tables.
        /// The caller must perform logging of the changes made.
        /// Very similar to Dapper's IDbConnection.Query except:
        /// <list type="bullet">
        ///     <item><description>It manages (obtains, opens, closes and returns) the database connection itself</description></item> 
        ///     <item><description>The result is always entirely loaded and returned as an IList ("buffered" cannot be set to false)</description></item>
        ///     <item><description>Transient errors are automatically retried</description></item> 
        ///     <item><description>Transactions are not supported (in order to support retries)</description></item> 
        /// </list>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="goc"></param>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <param name="commandTimeout"></param>
        /// <param name="commandType"></param>
        /// <returns></returns>
        public static IList<T> MutatingQuery<T>(this IGetOpenConnection goc, string sql, object param = null, int? commandTimeout = default(int?), CommandType? commandType = default(CommandType?))
        {
            // TODO: Log that user did this query
            return RetryPolicy.ExecuteAction<IList<T>>(() =>
            {
                using (var conn = goc.GetOpenConnection())
                {
                    return (IList<T>)conn.Query<T>(sql, param, transaction: null, buffered: true, commandTimeout: commandTimeout, commandType: commandType);
                }
            });
        }

    }
}
