namespace SFA.Apprenticeships.Web.Common.UnitTests
{
    using System.Text.RegularExpressions;
    using Constants;
    using FluentAssertions;
    using NUnit.Framework;

    [TestFixture]
    public class YearRegexRangeGeneratorTests
    {
        [TestCase("1913", "2014", false)]
        [TestCase("2014", null, false)]
        [TestCase("", "2014", false)]
        [TestCase("2014", "", false)]
        [TestCase("1910", "2010", true)]
        [TestCase("1910", "2011", false)]
        [TestCase("1910", "2009", true)]
        [TestCase("2015", "2014", false)]
        [TestCase("1989", "2014", true)]
        [TestCase("1923", "2024", false)]
        [TestCase("2015", "2025", true)]
        [TestCase("1989", "2024", true)]
        [TestCase("xyz", "2014", false)]
        [TestCase("2014", "--", false)]
        public void GetsRegexFor2014(string fromYear, string toYear, bool passes)
        {
            var regex = YearRegexRangeGenerator.GetRegex(toYear);
            Regex.Match(fromYear, regex).Success.Should().Be(passes);
        }
    }
}