using System.Collections.Generic;
using System.Linq;

namespace SFA.Apprenticeships.Infrastructure.Repositories.Sql.Schemas.dbo
{    
    public static class DbHelpers
    {
        private const int ChunkSize = 2000;

        //TODO: Write unit tests
        public static int[][] SplitParametersIntoChunks(IEnumerable<int> id , int chunkSize = ChunkSize)
        {
            var enumerable = id as int[] ?? id.ToArray();
            if (enumerable.Length > chunkSize)
            {
                var chunks = enumerable
                    .Select((s, i) => new { Value = s, Index = i })
                    .GroupBy(x => x.Index / chunkSize)
                    .Select(grp => grp.Select(x => x.Value).ToArray())
                    .ToArray();
                return chunks;
            }
            return new[] { enumerable.ToArray()};
        }
    }
}
