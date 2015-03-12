namespace SFA.Apprenticeships.Domain.Entities.UnitTests.Extensions
{
    using Entities.Extensions;
    using FluentAssertions;
    using NUnit.Framework;

    [TestFixture]
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
    }
}