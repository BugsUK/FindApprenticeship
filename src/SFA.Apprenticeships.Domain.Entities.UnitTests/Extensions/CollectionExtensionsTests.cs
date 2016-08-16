namespace SFA.Apprenticeships.Domain.Entities.UnitTests.Extensions
{
    using System;
    using Entities.Extensions;
    using FluentAssertions;
    using NUnit.Framework;

    [TestFixture]
    [Parallelizable]
    public class CollectionExtensionsTests
    {
        [Test]
        public void SubCategoriesTest()
        {
            var subCategories = new[]
            {
                "1_1",
                "1_2",
                "2_1"
            };

            var queryString = subCategories.ToQueryString("SubCategories");

            queryString.Should().Be("&SubCategories=1_1&SubCategories=1_2&SubCategories=2_1");
        }

        [Test]
        public void ZeroLengthCollectionTest()
        {
            var collection = new string[] { };
            var queryString = collection.ToQueryString("things");

            queryString.Should().Be(string.Empty);
        }

        [Test]
        public void NullCollectionTest()
        {
            string[] collection = null;

            // ReSharper disable once ExpressionIsAlwaysNull
            var queryString = collection.ToQueryString("things");

            queryString.Should().Be(string.Empty);
        }

        [Test]
        public void NullParameterNameTest()
        {
            var collection = new string[] { };
            
            Action action = () => collection.ToQueryString(null);

            action.ShouldThrow<ArgumentNullException>();
        }
    }
}