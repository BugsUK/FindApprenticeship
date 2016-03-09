namespace SFA.Apprenticeships.Application.UnitTests.Vacancies.Extensions
{
    using System.Linq;
    using Apprenticeships.Application.Vacancies.Extensions;
    using Domain.Entities.Vacancies.Apprenticeships;
    using FluentAssertions;
    using NUnit.Framework;
    using Ploeh.AutoFixture;

    [TestFixture]
    public class ApprenticeshipSummaryExtensionsTests
    {
        [Test]
        public void GetResultsHash()
        {
            var searchResults = new Fixture().Build<ApprenticeshipSearchResponse>().CreateMany(3).ToList();
            var hash = searchResults.GetResultsHash();
            hash.Should().NotBeNullOrEmpty();
            hash.Should().NotBe("hash");
        }

        [Test]
        public void GetResultsHashDifferent()
        {
            var existingSearchResults = new Fixture().Build<ApprenticeshipSearchResponse>().CreateMany(2).ToList();
            var newSearchResults = new Fixture().Build<ApprenticeshipSearchResponse>().CreateMany(3).ToList();
            var existingHash = existingSearchResults.GetResultsHash();
            var newHash = newSearchResults.GetResultsHash();
            existingHash.Should().NotBe(newHash);
        }
    }
}