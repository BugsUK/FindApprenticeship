namespace SFA.Apprenticeships.Infrastructure.Repositories.Sql.UnitTests.Helpers
{
    using System.Collections.Generic;
    using NUnit.Framework;
    using Schemas.dbo;

    [TestFixture]
    public class DbHelpersTests
    {        
        [TestCase(new[] {1, 2, 3, 4, 5, 6, 7, 8, 9, 11, 12, 13},5,3)]
        [TestCase(new[] { 1, 2, 3, 4, 5 }, 5, 1)]
        [TestCase(new[] { 1, 2, 3 }, 5, 1)]                        
        public void SplitInputIntoChunks_ChunkSize5_ReturnListOf3Arrays(IEnumerable<int> input,int chunkSize, int arrayCount)
        {
            var result = DbHelpers.SplitInputIntoChunks(input, chunkSize);
            Assert.AreEqual(result.Length,arrayCount);
        }
    }
}
