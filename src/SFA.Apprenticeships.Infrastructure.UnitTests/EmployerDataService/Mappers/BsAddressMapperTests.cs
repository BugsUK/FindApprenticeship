namespace SFA.Apprenticeships.Infrastructure.UnitTests.EmployerDataService.Mappers
{
    using System;
    using FluentAssertions;
    using Infrastructure.EmployerDataService.EmployerDataService;
    using Infrastructure.EmployerDataService.Mappers;
    using NUnit.Framework;

    /*
        Sample Address from EDS:

        "Address": {
            "SAON": null,
            "PAON": {
                "Items": [
                    "Arena Retail Park Unit 2"
                ],
                    "ItemsElementName": [
                    "Description"
                ]
            },
            "StreetDescription": "Classic Drive",
            "UniqueStreetReferenceNumber": null,
            "Items": null,
            "ItemsElementName": null,
            "PostTown": "Coventry",
            "PostCode": "CV6 6AS",
            "UniquePropertyReferenceNumber": null // NOTE: always appears to be null
        }
    */

    [TestFixture]
    public class BsAddressMapperTests
    {
        private BsAddressMapper _mapper;

        [SetUp]
        public void SetUp()
        {
            _mapper = new BsAddressMapper();
        }

        [TestCase(" Arena Retail Park Unit 2 ", false)]
        [TestCase("", true)]
        [TestCase(" ", true)]
        [TestCase(null, true)]
        public void ShouldMapPrimaryAddressableObjectNameToAddressLine1(string paon, bool expectNullAddressLine1ToBe)
        {
            // Arrange.
            var fromAddress = new BSaddressStructure
            {
                PAON = new AONstructure
                {
                    Items = new object[]
                    {
                        paon
                    }
                }
            };

            // Act.
            var toAddress = _mapper.ToAddress(fromAddress);

            // Assert.
            toAddress.Should().NotBeNull();

            if (expectNullAddressLine1ToBe)
            {
                toAddress.AddressLine1.Should().BeNull();
            }
            else
            {
                toAddress.AddressLine1.Should().Be(paon.Trim());
            }
        }

        [TestCase(" Classic Drive ", false)]
        [TestCase("", true)]
        [TestCase(" ", true)]
        [TestCase(null, true)]
        public void ShouldMapStreetDescriptionToAddressLine2(string streetDescription, bool expectNullAddressLine2)
        {
            // Arrange.
            var fromAddress = new BSaddressStructure
            {
                StreetDescription = streetDescription
            };

            // Act.
            var toAddress = _mapper.ToAddress(fromAddress);

            // Assert.
            toAddress.Should().NotBeNull();

            if (expectNullAddressLine2)
            {
                toAddress.AddressLine2.Should().BeNull();
            }
            else
            {
                toAddress.AddressLine2.Should().Be(streetDescription.Trim());
            }
        }

        [TestCase(" Coventry ", false)]
        [TestCase("", true)]
        [TestCase(" ", true)]
        [TestCase(null, true)]
        public void ShouldMapPostTownToAddressLine3(string postTown, bool expectNullAddressLine3)
        {
            // Arrange.
            var fromAddress = new BSaddressStructure
            {
                PostTown = postTown
            };

            // Act.
            var toAddress = _mapper.ToAddress(fromAddress);

            // Assert.
            toAddress.Should().NotBeNull();

            if (expectNullAddressLine3)
            {
                toAddress.AddressLine3.Should().BeNull();
            }
            else
            {
                toAddress.AddressLine3.Should().Be(postTown.Trim());
            }
        }

        [TestCase(" CV6 6AS ", false)]
        [TestCase("", true)]
        [TestCase(" ", true)]
        [TestCase(null, true)]
        public void ShouldMapPostcode(string postcode, bool expectNullPostcode)
        {
            // Arrange.
            var fromAddress = new BSaddressStructure
            {
                PostCode = postcode
            };

            // Act.
            var toAddress = _mapper.ToAddress(fromAddress);

            // Assert.
            toAddress.Should().NotBeNull();

            if (expectNullPostcode)
            {
                toAddress.Postcode.Should().BeNull();
            }
            else
            {
                toAddress.Postcode.Should().Be(postcode.Trim());
            }
        }

        [Test]
        public void ShouldThrowWhenFromAddressIsNull()
        {
            // Act.
            Action action = () => _mapper.ToAddress(default(BSaddressStructure));

            // Assert.
            action.ShouldThrow<ArgumentNullException>();
        }
    }
}
