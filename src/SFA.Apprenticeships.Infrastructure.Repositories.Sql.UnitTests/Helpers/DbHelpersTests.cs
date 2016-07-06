namespace SFA.Apprenticeships.Infrastructure.Repositories.Sql.UnitTests.Helpers
{
    using System.Collections.Generic;
    using System.Linq;
    using FluentAssertions;
    using NUnit.Framework;
    using Schemas.dbo;

    [TestFixture]
    public class DbHelpersTests
    {
        [TestCase(new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 }, 5, 3, 2)]
        [TestCase(new[] { 1, 2, 3, 4, 5 }, 5, 1, 5)]
        [TestCase(new[] { 1, 2, 3 }, 5, 1, 3)]
        public void ShouldSplitIdsIntoChunks(IEnumerable<int> input, int chunkSize, int arrayCount, int lastArrayLength)
        {
            var result = DbHelpers.SplitIds(input, chunkSize);

            result.Length.Should().Be(arrayCount);
            result.Last().Length.Should().Be(lastArrayLength);
        }

        [Test]
        public void ShouldHandleEmptyArray()
        {
            var result = DbHelpers.SplitIds(new int[] { });

            result.Length.Should().Be(0);
        }

        [Test]
        public void ShouldDefaultChunkSize()
        {
            const int chunkCount = 3;

            var ids = Enumerable.Range(0, DbHelpers.DefaultChunkSize * chunkCount);
            var result = DbHelpers.SplitIds(ids);

            result.Length.Should().Be(chunkCount);

            Assert.That(result.All(each => each.Length == DbHelpers.DefaultChunkSize));
        }

        [Test]
        public void ShouldCopyOriginalIds()
        {
            const int chunkSize = 5;

            var array0 = new[] { 1, 2, 3, 4, 5  };
            var array1 = new[] { 6, 7 };

            var ids = new List<int>();

            ids.AddRange(array0);
            ids.AddRange(array1);

            var result = DbHelpers.SplitIds(ids, chunkSize);

            result.Length.Should().Be(2);

            result[0].ShouldBeEquivalentTo(array0);
            result[1].ShouldBeEquivalentTo(array1);
        }
    }
}
