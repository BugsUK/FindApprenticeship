// ReSharper disable PossibleMultipleEnumeration
namespace SFA.Apprenticeship.Api.AvmsCompatability.UnitTests.Mappers.Version51
{
    using System.Linq;
    using Apprenticeships.Domain.Entities.ReferenceData;
    using AvmsCompatability.Mappers.Version51;
    using FluentAssertions;
    using NUnit.Framework;

    [TestFixture]
    public class ApprenticeshipFrameworkMapperTests
    {
        // NOTE: cannot use Fixture here as Category data structure is recursive.
        private readonly Category[] _categories = {
            new Category
            {
                CodeName = "00",
                FullName = "Blacklisted Sector - 00",
                SubCategories = new []
                {
                    new Category()
                }
            },
            new Category
            {
                CodeName = "02",
                FullName = "Sector - 02",
                SubCategories = new []
                {
                    new Category
                    {
                        CodeName = "02.01",
                        FullName = "Framework - 02.01"
                    },
                    new Category
                    {
                        CodeName = "02.02",
                        FullName = "Framework - 02.02"
                    }
                }
            },
            new Category
            {
                CodeName = "03",
                FullName = "Sector - 03",
                SubCategories = new []
                {
                    new Category
                    {
                        CodeName = "03.01",
                        FullName = "Framework - 03.01"
                    }
                }
            },
            new Category
            {
                CodeName = "42",
                FullName = "Sector with no frameworks - 99"
            },
            new Category
            {
                CodeName = "99",
                FullName = "Blacklisted Sector - 99",
                SubCategories = new []
                {
                    new Category()
                }

            }
        };

        [Test]
        public void ShouldMapFromCategories()
        {
            // Arrange.
            var mapper = new ApprenticeshipFrameworkMapper();

            // Act.
            var frameworks = mapper.FromCategories(_categories);

            // Assert.
            frameworks.Should().NotBeNull();
            frameworks.Count().Should().Be(_categories.Length);
        }
    }
}
