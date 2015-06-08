namespace SFA.Apprenticeships.Web.ContactForms.Tests.Services.LocationSearchService
{
    using Application.Services.LocationSearchService.Entities;
    using Application.Services.LocationSearchService.Extensions;
    using FluentAssertions;
    using NUnit.Framework;

    [TestFixture]
    public class DeliveryPointAddressExtensionsTests
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
            var dpa = new DeliveryPointAddress {ClassificationCode = classificationCode};
            dpa.IsCommercial().Should().Be(isCommercial);
        }

        [Test]
        public void HouseNumber()
        {
            var dpa = new DeliveryPointAddress
            {
                Uprn = "100061368366",
                BuildingNumber = 30,
                Street = "WEST GARDENS",
                TownOrCity = "EPSOM",
                Postcode = "KT17 1NE"
            };

            var address = dpa.ToLocation().Address;

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
            var dpa = new DeliveryPointAddress
            {
                Uprn = "100030231701",
                BuildingName = "GREYSTONES",
                Street = "BROOK LANE",
                Village = "SUTTON-ON-THE-HILL",
                TownOrCity = "ASHBOURNE",
                Postcode = "DE6 5JA"
            };

            var address = dpa.ToLocation().Address;

            address.Should().NotBeNull();

            address.CompanyName.Should().BeNullOrEmpty();
            address.AddressLine1.Should().Be("Greystones");
            address.AddressLine2.Should().Be("Brook Lane");
            address.AddressLine3.Should().Be("Sutton-On-The-Hill");
            address.City.Should().Be("Ashbourne");
            address.Postcode.Should().Be("DE6 5JA");
        }

        [Test]
        public void FlatName()
        {
            var dpa = new DeliveryPointAddress
            {
                Uprn = "100023352765",
                SubBuildingName = "FLAT 4",
                BuildingNumber = 66,
                Street = "KILLIESER AVENUE",
                TownOrCity = "LONDON",
                Postcode = "SW2 4NT"
            };

            var address = dpa.ToLocation().Address;

            address.Should().NotBeNull();

            address.CompanyName.Should().BeNullOrEmpty();
            address.AddressLine1.Should().Be("Flat 4");
            address.AddressLine2.Should().Be("66 Killieser Avenue");
            address.AddressLine3.Should().BeNullOrEmpty();
            address.City.Should().Be("London");
            address.Postcode.Should().Be("SW2 4NT");
        }

        [Test]
        public void CompanyName()
        {
            var dpa = new DeliveryPointAddress
            {
                Uprn = "10091781210",
                OrganisationName = "EXECUTIVE OFFICES",
                BuildingNumber = 41,
                Street = "LOTHBURY",
                TownOrCity = "LONDON",
                Postcode = "EC2R 7HG"
            };

            var address = dpa.ToLocation().Address;

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
            var dpa = new DeliveryPointAddress
            {
                Uprn = "10023037881",
                OrganisationName = "SKILLS FUNDING AGENCY",
                BuildingName = "CHEYLESMORE HOUSE",
                BuildingNumber = 5,
                Street = "QUINTON ROAD",
                TownOrCity = "COVENTRY",
                Postcode = "CV1 2WT"
            };

            var address = dpa.ToLocation().Address;

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