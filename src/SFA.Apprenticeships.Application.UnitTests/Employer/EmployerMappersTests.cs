namespace SFA.Apprenticeships.Application.UnitTests.Employer
{
    using System;
    using Apprenticeships.Application.Employer.Mappers;
    using Domain.Entities.Raa.Parties;
    using FluentAssertions;
    using NUnit.Framework;
    using Ploeh.AutoFixture;
    using SFA.Infrastructure.Interfaces;

    [TestFixture]
    public class EmployerMappersTests
    {
        private IMapper _mapper = new EmployerMappers();

        [Test]
        public void ShouldCreateMap()
        {
            new EmployerMappers().Mapper.AssertConfigurationIsValid();
        }

        [Test]
        public void ShouldMapFromVerifiedOrganisationSummary()
        {
            var summary = new Fixture().Create<VerifiedOrganisationSummary>();

            var employer = _mapper.Map<VerifiedOrganisationSummary, Employer>(summary);

            employer.EmployerId.Should().Be(0);
            employer.EmployerGuid.Should().Be(Guid.Empty);
            employer.EdsUrn.Should().Be(summary.ReferenceNumber);
            employer.Name.Should().Be(summary.Name);
            employer.Address.Should().NotBeNull();
            employer.Address.AddressLine1.Should().Be(summary.Address.AddressLine1);
            employer.Address.AddressLine2.Should().Be(summary.Address.AddressLine2);
            employer.Address.AddressLine3.Should().Be(summary.Address.AddressLine3);
            employer.Address.AddressLine4.Should().Be(summary.Address.AddressLine4);
            employer.Address.AddressLine5.Should().Be(summary.Address.AddressLine5);
            employer.Address.Town.Should().Be(summary.Address.Town);
            employer.Address.Postcode.Should().Be(summary.Address.Postcode);
            employer.Address.GeoPoint.Should().NotBeNull();
            employer.Address.GeoPoint.Latitude.Should().Be(summary.Address.GeoPoint.Latitude);
            employer.Address.GeoPoint.Longitude.Should().Be(summary.Address.GeoPoint.Longitude);
        }
    }
}