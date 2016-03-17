namespace SFA.Apprenticeships.Infrastructure.UnitTests.Presentation
{
    using Domain.Entities.Raa.Reference;
    using FluentAssertions;
    using Infrastructure.Presentation;
    using NUnit.Framework;

    [TestFixture]
    public class RegionalTeamPresenter
    {
        [TestCase(RegionalTeam.North, "North")]
        [TestCase(RegionalTeam.NorthWest, "North West")]
        [TestCase(RegionalTeam.YorkshireAndHumberside, "Yorkshire and Humberside")]
        [TestCase(RegionalTeam.EastMidlands, "East Midlands")]
        [TestCase(RegionalTeam.WestMidlands, "West Midlands")]
        [TestCase(RegionalTeam.EastAnglia, "East Anglia")]
        [TestCase(RegionalTeam.SouthEast, "South East")]
        [TestCase(RegionalTeam.SouthWest, "South West")]
        public void GetTitleTest(RegionalTeam regionalTeam, string expectedTitle)
        {
            var title = regionalTeam.GetTitle();

            title.Should().Be(expectedTitle);
        }
    }
}