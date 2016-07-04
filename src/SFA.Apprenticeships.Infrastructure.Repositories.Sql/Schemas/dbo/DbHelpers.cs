using System.Collections.Generic;
using System.Linq;

namespace SFA.Apprenticeships.Infrastructure.Repositories.Sql.Schemas.dbo
{
    public static class DbHelpers
    {
        private const int ChunkSize = 200;

        //TODO: Write unit tests
        public static int[][] SplitInputIntoChunks(IEnumerable<int> id , int chunkSize = ChunkSize)
        {           
            var idEnumerable = id as int[] ?? id.ToArray();
            if (idEnumerable.Length > chunkSize)
            {
                var i = 0;
                var chunks=new List<int[]>();
                while (i < idEnumerable.Length)
                {
                    var chunk = idEnumerable.Skip(i).Take(chunkSize).ToArray();                                        
                    i += chunk.Length;
                    chunks.Add(chunk);
                }
                return chunks.ToArray();
            }
            return new[] {idEnumerable.ToArray()};
        }
    }
}
