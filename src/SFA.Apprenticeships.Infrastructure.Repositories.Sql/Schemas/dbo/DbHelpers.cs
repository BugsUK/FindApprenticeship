namespace SFA.Apprenticeships.Infrastructure.Repositories.Sql.Schemas.dbo
{
    using System.Collections.Generic;
    using System.Linq;

    public static class DbHelpers
    {
        public const int DefaultChunkSize = 2000;

        /// <summary>
        /// Splits an array of integer ids into an array of arrays that can be used in a SQL 'WHERE IN' clause.
        /// This is necessary due to a 2100 parameter limit in SQL Server.
        /// there is a 
        /// </summary>
        /// <param name="ids">Array of integer ids.</param>
        /// <param name="chunkSize">Size of array in which to split ids (default is 2000).</param>
        /// <returns></returns>
        public static int[][] SplitIds(IEnumerable<int> ids, int chunkSize = DefaultChunkSize)
        {
            var enumerableIds = ids as int[] ?? ids.ToArray();

            var i = 0;
            var chunks = new List<int[]>();

            while (i < enumerableIds.Length)
            {
                var chunk = enumerableIds.Skip(i).Take(chunkSize).ToArray();

                chunks.Add(chunk);
                i += chunk.Length;
            }

            return chunks.ToArray();
        }
    }
}
