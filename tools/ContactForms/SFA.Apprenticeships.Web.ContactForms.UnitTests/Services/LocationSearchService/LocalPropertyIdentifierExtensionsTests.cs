namespace SFA.Apprenticeships.Web.ContactForms.Tests.Services.LocationSearchService
{
    using Application.Services.LocationSearchService.Entities;
    using Application.Services.LocationSearchService.Extensions;
    using FluentAssertions;
    using NUnit.Framework;

    [TestFixture]
    public class LocalPropertyIdentifierExtensionsTests
    {
        [TestCase("C", true)]
        [TestCase("CE", true)]
        [TestCase("L", false)]
        [TestCase("M", false)]
        [TestCase("O", false)]
        [TestCase("P", false)]
        [TestCase("R", false)]
        [TestCase("RD", false)]
        [TestCase("U", false)]
        [TestCase("X", true)]
        [TestCase("Z", false)]
        public void IsCommercial(string classificationCode, bool isCommercial)
        {
            var lpi = new LocalPropertyIdentifier {ClassificationCode = classificationCode};
            lpi.IsCommercial().Should().Be(isCommercial);
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
                County = "Surrey",
                Postcode = "KT17 1NE"
            };

            var address = lpi.ToLocation().Address;

            address.Should().NotBeNull();

            address.CompanyName.Should().BeNullOrEmpty();
            address.AddressLine1.Should().Be("30 West Gardens");
            address.AddressLine2.Should().BeNullOrEmpty();
            address.AddressLine3.Should().BeNullOrEmpty();
            address.City.Should().Be("Epsom");
            address.Postcode.Should().Be("KT17 1NE");
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

            var address = lpi.ToLocation().Address;

            address.Should().NotBeNull();

            address.CompanyName.Should().BeNullOrEmpty();
            address.AddressLine1.Should().Be("2, Field Gate Farm");
            address.AddressLine2.Should().Be("Dish Lane");
            address.AddressLine3.Should().Be("Sutton On The Hill");
            address.City.Should().Be("Derby");
            address.Postcode.Should().Be("DE6 5JA");
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

            var address = lpi.ToLocation().Address;

            address.Should().NotBeNull();

            address.CompanyName.Should().Be("Bob The Builder Ltd.");
            address.AddressLine1.Should().Be("2, Field Gate Farm");
            address.AddressLine2.Should().Be("Dish Lane");
            address.AddressLine3.Should().Be("Sutton On The Hill");
            address.City.Should().Be("Derby");
            address.Postcode.Should().Be("DE6 5JA");
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

            var address = lpi.ToLocation().Address;

            address.Should().NotBeNull();

            address.CompanyName.Should().BeNullOrEmpty();
            address.AddressLine1.Should().Be("Flat 4");
            address.AddressLine2.Should().Be("66 Killieser Avenue");
            address.AddressLine3.Should().BeNullOrEmpty();
            address.City.Should().Be("London");
            address.Postcode.Should().Be("SW2 4NT");
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

            var address = lpi.ToLocation().Address;

            address.Should().NotBeNull();

            address.CompanyName.Should().BeNullOrEmpty();
            address.AddressLine1.Should().Be("Flat D");
            address.AddressLine2.Should().Be("4 Furlong Road");
            address.AddressLine3.Should().Be("Islington");
            address.City.Should().Be("London");
            address.Postcode.Should().Be("N7 8LS");
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

            var address = lpi.ToLocation().Address;

            address.Should().NotBeNull();

            address.CompanyName.Should().BeNullOrEmpty();
            address.AddressLine1.Should().Be("2B Killieser Avenue");
            address.AddressLine2.Should().BeNullOrEmpty();
            address.AddressLine3.Should().BeNullOrEmpty();
            address.City.Should().Be("London");
            address.Postcode.Should().Be("SW2 4NT");
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

            var address = lpi.ToLocation().Address;

            address.Should().NotBeNull();

            address.CompanyName.Should().Be("Executive Offices");
            address.AddressLine1.Should().Be("41 Lothbury");
            address.AddressLine2.Should().BeNullOrEmpty();
            address.AddressLine3.Should().BeNullOrEmpty();
            address.City.Should().Be("London");
            address.Postcode.Should().Be("EC2R 7HG");
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

            var address = lpi.ToLocation().Address;

            address.Should().NotBeNull();

            address.CompanyName.Should().Be("Skills Funding Agency");
            address.AddressLine1.Should().Be("Cheylesmore House");
            address.AddressLine2.Should().Be("5 Quinton Road");
            address.AddressLine3.Should().BeNullOrEmpty();
            address.City.Should().Be("Coventry");
            address.Postcode.Should().Be("CV1 2WT");
        }
    }
}