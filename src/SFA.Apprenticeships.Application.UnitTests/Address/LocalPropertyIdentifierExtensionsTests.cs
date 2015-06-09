namespace SFA.Apprenticeships.Application.UnitTests.Address
{
    using FluentAssertions;
    using Infrastructure.Address.Entities;
    using Infrastructure.Address.Extensions;
    using NUnit.Framework;

    [TestFixture]
    public class LocalPropertyIdentifierExtensionsTests
    {
        [TestCase("C", false)]
        [TestCase("CE", false)]
        [TestCase("L", false)]
        [TestCase("M", false)]
        [TestCase("O", false)]
        [TestCase("P", true)]
        [TestCase("R", true)]
        [TestCase("RD", true)]
        [TestCase("U", false)]
        [TestCase("X", true)]
        [TestCase("Z", false)]
        public void IsResidential(string classificationCode, bool isResidential)
        {
            var lpi = new LocalPropertyIdentifier {ClassificationCode = classificationCode};
            lpi.IsResidential().Should().Be(isResidential);
        }

        [Test]
        public void Ampersand()
        {
            var lpi = new LocalPropertyIdentifier
            {
                Uprn = "22046079",
                SubBuildingNumber = 28,
                BuildingName = "OSPREY HOUSE",
                Street = "SILLWOOD PLACE",
                TownOrCity = "BRIGHTON",
                County = "BRIGHTON & HOVE",
                Postcode = "BN1 2ND"
            };

            var address = lpi.ToAddress();

            address.Should().NotBeNull();

            address.AddressLine1.Should().Be("28, Osprey House");
            address.AddressLine2.Should().Be("Sillwood Place");
            address.AddressLine3.Should().Be("Brighton");
            address.AddressLine4.Should().Be("Brighton And Hove");
            address.Postcode.Should().Be("BN1 2ND");
            address.Uprn.Should().Be("22046079");
        }

        [Test]
        public void HouseNumber()
        {
            var lpi = new LocalPropertyIdentifier
            {
                Uprn = "100061368366",
                BuildingNumber = 30,
                Street = "WEST GARDENS",
                TownOrCity = "EPSOM",
                County = "SURREY",
                Postcode = "KT17 1NE"
            };

            var address = lpi.ToAddress();

            address.Should().NotBeNull();

            address.AddressLine1.Should().Be("30 West Gardens");
            address.AddressLine2.Should().BeNullOrEmpty();
            address.AddressLine3.Should().Be("Epsom");
            address.AddressLine4.Should().Be("Surrey");
            address.Postcode.Should().Be("KT17 1NE");
            address.Uprn.Should().Be("100061368366");
        }

        [Test]
        public void HouseName()
        {
            var lpi = new LocalPropertyIdentifier
            {
                Uprn = "100030231701",
                SubBuildingNumber = 2,
                BuildingName = "FIELD GATE FARM",
                Street = "DISH LANE",
                Village = "SUTTON ON THE HILL",
                TownOrCity = "DERBY",
                County = "DERBYSHIRE",
                Postcode = "DE6 5JA"
            };

            var address = lpi.ToAddress();

            address.Should().NotBeNull();

            address.AddressLine1.Should().Be("2, Field Gate Farm");
            address.AddressLine2.Should().Be("Dish Lane, Sutton On The Hill");
            address.AddressLine3.Should().Be("Derby");
            address.AddressLine4.Should().Be("Derbyshire");
            address.Postcode.Should().Be("DE6 5JA");
            address.Uprn.Should().Be("100030231701");
        }

        [Test]
        public void HouseNamePlusCompany()
        {
            var lpi = new LocalPropertyIdentifier
            {
                Uprn = "100030231701",
                OrganisationName = "BOB THE BUILDER LTD.",
                SubBuildingNumber = 2,
                BuildingName = "FIELD GATE FARM",
                Street = "DISH LANE",
                Village = "SUTTON ON THE HILL",
                TownOrCity = "DERBY",
                County = "DERBYSHIRE",
                Postcode = "DE6 5JA"
            };

            var address = lpi.ToAddress();

            address.Should().NotBeNull();

            address.AddressLine1.Should().Be("Bob The Builder Ltd., 2, Field Gate Farm");
            address.AddressLine2.Should().Be("Dish Lane, Sutton On The Hill");
            address.AddressLine3.Should().Be("Derby");
            address.AddressLine4.Should().Be("Derbyshire");
            address.Postcode.Should().Be("DE6 5JA");
            address.Uprn.Should().Be("100030231701");
        }

        [Test]
        public void FlatName()
        {
            var lpi = new LocalPropertyIdentifier
            {
                Uprn = "100023352765",
                SubBuildingName = "FLAT 4",
                BuildingNumber = 66,
                Street = "KILLIESER AVENUE",
                TownOrCity = "LONDON",
                County = "LAMBETH",
                Postcode = "SW2 4NT"
            };

            var address = lpi.ToAddress();

            address.Should().NotBeNull();

            address.AddressLine1.Should().Be("Flat 4");
            address.AddressLine2.Should().Be("66 Killieser Avenue");
            address.AddressLine3.Should().Be("London");
            address.AddressLine4.Should().Be("Lambeth");
            address.Postcode.Should().Be("SW2 4NT");
            address.Uprn.Should().Be("100023352765");
        }

        [Test]
        public void FlatNameNoVillage()
        {
            var lpi = new LocalPropertyIdentifier
            {
                Uprn = "5300034717",
                SubBuildingName = "FLAT D",
                BuildingNumber = 4,
                Street = "FURLONG ROAD",
                Village = "ISLINGTON",
                TownOrCity = "LONDON",
                County = "ISLINGTON",
                Postcode = "N7 8LS"
            };

            var address = lpi.ToAddress();

            address.Should().NotBeNull();

            address.AddressLine1.Should().Be("Flat D");
            address.AddressLine2.Should().Be("4 Furlong Road");
            address.AddressLine3.Should().Be("London");
            address.AddressLine4.Should().Be("Islington");
            address.Postcode.Should().Be("N7 8LS");
            address.Uprn.Should().Be("5300034717");
        }

        [Test]
        public void MaisonetteName()
        {
            var lpi = new LocalPropertyIdentifier
            {
                Uprn = "100021857757",
                BuildingNumber = 2,
                BuildingNumberSuffix = "B",
                Street = "KILLIESER AVENUE",
                TownOrCity = "LONDON",
                County = "Lambeth",
                Postcode = "SW2 4NT"
            };

            var address = lpi.ToAddress();

            address.Should().NotBeNull();

            address.AddressLine1.Should().Be("2B Killieser Avenue");
            address.AddressLine2.Should().BeNullOrEmpty();
            address.AddressLine3.Should().Be("London");
            address.AddressLine4.Should().Be("Lambeth");
            address.Postcode.Should().Be("SW2 4NT");
            address.Uprn.Should().Be("100021857757");
        }

        [Test]
        public void CompanyName()
        {
            var lpi = new LocalPropertyIdentifier
            {
                Uprn = "10091781210",
                OrganisationName = "EXECUTIVE OFFICES",
                BuildingName = "EXECUTIVE OFFICES",
                BuildingNumber = 41,
                Street = "LOTHBURY",
                TownOrCity = "LONDON",
                County = "CITY OF LONDON",
                Postcode = "EC2R 7HG"
            };

            var address = lpi.ToAddress();

            address.Should().NotBeNull();

            address.AddressLine1.Should().Be("Executive Offices");
            address.AddressLine2.Should().Be("41 Lothbury");
            address.AddressLine3.Should().Be("London");
            address.AddressLine4.Should().Be("City Of London");
            address.Postcode.Should().Be("EC2R 7HG");
            address.Uprn.Should().Be("10091781210");
        }

        [Test]
        public void CompanyNameInBuilding()
        {
            var lpi = new LocalPropertyIdentifier
            {
                Uprn = "10023037881",
                OrganisationName = "SKILLS FUNDING AGENCY",
                BuildingName = "CHEYLESMORE HOUSE",
                BuildingNumber = 5,
                Street = "QUINTON ROAD",
                TownOrCity = "COVENTRY",
                County = "COVENTRY",
                Postcode = "CV1 2WT"
            };

            var address = lpi.ToAddress();

            address.Should().NotBeNull();

            address.AddressLine1.Should().Be("Skills Funding Agency, Cheylesmore House");
            address.AddressLine2.Should().Be("5 Quinton Road");
            address.AddressLine3.Should().Be("Coventry");
            address.AddressLine4.Should().BeNullOrEmpty();
            address.Postcode.Should().Be("CV1 2WT");
            address.Uprn.Should().Be("10023037881");
        }
    }
}