namespace SFA.Apprenticeships.Domain.Entities.Raa.UnitTests.Reference
{
    using FluentAssertions;
    using NUnit.Framework;
    using Raa.Reference;

    [TestFixture]
    public class RegionalTeamMapperTests
    {
        [Test]
        public void MapSunderlandToNorth()
        {
            //Arrange
            const string sunderland = "SR2 9HZ";

            //Act
            var regionalTeam = RegionalTeamMapper.GetRegionalTeam(sunderland);

            //Assert
            regionalTeam.Should().Be(RegionalTeam.North);
        }

        [Test]
        public void CaseInsensitive()
        {
            //Arrange
            const string sunderland = "sr2 9hz";

            //Act
            var regionalTeam = RegionalTeamMapper.GetRegionalTeam(sunderland);

            //Assert
            regionalTeam.Should().Be(RegionalTeam.North);
        }

        [Test]
        public void SpacesIgnored()
        {
            //Arrange
            const string sunderland = "s r 29 h z";

            //Act
            var regionalTeam = RegionalTeamMapper.GetRegionalTeam(sunderland);

            //Assert
            regionalTeam.Should().Be(RegionalTeam.North);
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        [TestCase("ZZZ")]
        public void InvalidPostcodeMapsToOther(string postcode)
        {
            //Arrange

            //Act
            var regionalTeam = RegionalTeamMapper.GetRegionalTeam(postcode);

            //Assert
            regionalTeam.Should().Be(RegionalTeam.Other);
        }

        [TestCase("DH2 1SL")]
        [TestCase("DL3 2XL")]
        [TestCase("HG42 3HJ")]
        [TestCase("NE5 3OI")]
        [TestCase("SR13 5KS")]
        [TestCase("TS2 8FG")]
        [TestCase("YO33 9YY")]
        public void MapToNorth(string postcode)
        {
            //Arrange
            
            //Act
            var regionalTeam = RegionalTeamMapper.GetRegionalTeam(postcode);

            //Assert
            regionalTeam.Should().Be(RegionalTeam.North);
        }

        [TestCase("BB22 1SL")]
        [TestCase("BL3 2XL")]
        [TestCase("CA4 3HJ")]
        [TestCase("CW5 3OI")]
        [TestCase("FY1 5KS")]
        [TestCase("L24 8FG")]
        [TestCase("LA3 9YY")]
        [TestCase("M3 9FG")]
        [TestCase("OL4 9ED")]
        [TestCase("PR56 9AS")]
        [TestCase("SK6 9YH")]
        [TestCase("WA72 9UJ")]
        [TestCase("WN8 9IL")]
        [TestCase("CH91 9CV")]
        public void MapToNorthWest(string postcode)
        {
            //Arrange
            
            //Act
            var regionalTeam = RegionalTeamMapper.GetRegionalTeam(postcode);

            //Assert
            regionalTeam.Should().Be(RegionalTeam.NorthWest);
        }

        [TestCase("BD22 1SL")]
        [TestCase("DN3 2XL")]
        [TestCase("HD4 3HJ")]
        [TestCase("HU5 3OI")]
        [TestCase("HX1 5KS")]
        [TestCase("LN4 8FG")]
        [TestCase("LS3 9YY")]
        [TestCase("S3 9FG")]
        [TestCase("WF4 9ED")]
        public void MapToYorkshireAndHumberside(string postcode)
        {
            //Arrange
            
            //Act
            var regionalTeam = RegionalTeamMapper.GetRegionalTeam(postcode);

            //Assert
            regionalTeam.Should().Be(RegionalTeam.YorkshireAndHumberside);
        }

        [TestCase("DE22 1SL")]
        [TestCase("LE3 2XL")]
        [TestCase("NG4 3HJ")]
        [TestCase("NN5 3OI")]
        public void MapToEastMidlands(string postcode)
        {
            //Arrange
            
            //Act
            var regionalTeam = RegionalTeamMapper.GetRegionalTeam(postcode);

            //Assert
            regionalTeam.Should().Be(RegionalTeam.EastMidlands);
        }

        [TestCase("B22 1SL")]
        [TestCase("CV3 2XL")]
        [TestCase("DY4 3HJ")]
        [TestCase("HR5 3OI")]
        [TestCase("ST1 5KS")]
        [TestCase("SY4 8FG")]
        [TestCase("TF3 9YY")]
        [TestCase("WR3 9FG")]
        [TestCase("WS4 9ED")]
        [TestCase("WV56 9AS")]
        public void MapToWestMidlands(string postcode)
        {
            //Arrange

            //Act
            var regionalTeam = RegionalTeamMapper.GetRegionalTeam(postcode);

            //Assert
            regionalTeam.Should().Be(RegionalTeam.WestMidlands);
        }

        [TestCase("AL22 1SL")]
        [TestCase("CB3 2XL")]
        [TestCase("CM4 3HJ")]
        [TestCase("CO5 3OI")]
        [TestCase("IG1 5KS")]
        [TestCase("IP4 8FG")]
        [TestCase("LU3 9YY")]
        [TestCase("NR3 9FG")]
        [TestCase("PE4 9ED")]
        [TestCase("RM56 9AS")]
        [TestCase("SG6 9YH")]
        [TestCase("SS72 9UJ")]
        [TestCase("WD8 9IL")]
        public void MapToEastAnglia(string postcode)
        {
            //Arrange

            //Act
            var regionalTeam = RegionalTeamMapper.GetRegionalTeam(postcode);

            //Assert
            regionalTeam.Should().Be(RegionalTeam.EastAnglia);
        }

        [TestCase("BN22 1SL")]
        [TestCase("BR3 2XL")]
        [TestCase("CR4 3HJ")]
        [TestCase("CT5 3OI")]
        [TestCase("DA1 5KS")]
        [TestCase("GU4 8FG")]
        [TestCase("EN3 9YY")]
        [TestCase("HA3 9FG")]
        [TestCase("HP4 9ED")]
        [TestCase("KT56 9AS")]
        [TestCase("ME6 9YH")]
        [TestCase("MK72 9UJ")]
        [TestCase("OX8 9IL")]
        [TestCase("RG91 9CV")]
        [TestCase("RH3 9YY")]
        [TestCase("SL3 9FG")]
        [TestCase("SM4 9ED")]
        [TestCase("SO56 9AS")]
        [TestCase("TN6 9YH")]
        [TestCase("TW72 9UJ")]
        [TestCase("UB8 9IL")]
        [TestCase("E22 1SL")]
        [TestCase("EC3 2XL")]
        [TestCase("N4 3HJ")]
        [TestCase("NW5 3OI")]
        [TestCase("SE1 5KS")]
        [TestCase("SW4 8FG")]
        [TestCase("W3 9YY")]
        [TestCase("WC3 9FG")]
        public void MapToSouthEast(string postcode)
        {
            //Arrange

            //Act
            var regionalTeam = RegionalTeamMapper.GetRegionalTeam(postcode);

            //Assert
            regionalTeam.Should().Be(RegionalTeam.SouthEast);
        }

        [TestCase("BA22 1SL")]
        [TestCase("BH3 2XL")]
        [TestCase("BS4 3HJ")]
        [TestCase("DT5 3OI")]
        [TestCase("EX1 5KS")]
        [TestCase("GL4 8FG")]
        [TestCase("PL3 9YY")]
        [TestCase("PO3 9FG")]
        [TestCase("SN4 9ED")]
        [TestCase("SP56 9AS")]
        [TestCase("TA6 9YH")]
        [TestCase("TQ72 9UJ")]
        [TestCase("TR8 9IL")]
        public void MapToSouthWest(string postcode)
        {
            //Arrange

            //Act
            var regionalTeam = RegionalTeamMapper.GetRegionalTeam(postcode);

            //Assert
            regionalTeam.Should().Be(RegionalTeam.SouthWest);
        }
    }
}