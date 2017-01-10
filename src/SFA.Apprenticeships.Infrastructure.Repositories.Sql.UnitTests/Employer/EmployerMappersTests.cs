namespace SFA.Apprenticeships.Infrastructure.Repositories.Sql.UnitTests.Employer
{
    using System;
    using FluentAssertions;
    using NUnit.Framework;
    using Ploeh.AutoFixture;
    using Schemas.dbo;

    using SFA.Apprenticeships.Application.Interfaces;
    using SFA.Infrastructure.Interfaces;
    using Domain = Domain.Entities.Raa.Parties;
    using Database = Schemas.dbo.Entities;

    [TestFixture]
    [Parallelizable]
    public class EmployerMappersTests
    {
        private readonly IMapper _mapper = new EmployerMappers();

        [Test]
        public void DoMappersMapEverything()
        {
            // Arrange.
            var mapper = new EmployerMappers();

            // Act.
            mapper.Initialise();

            // Assert.
            mapper.Mapper.AssertConfigurationIsValid();
        }

        [Test]
        public void ShouldMapFromVerifiedOrganisationSummary()
        {
            // Arrange.
            var summary = new Fixture().Create<Domain.VerifiedOrganisationSummary>();

            // Act.
            var employer = _mapper.Map<Domain.VerifiedOrganisationSummary, Domain.Employer>(summary);

            // Assert.
            employer.EmployerId.Should().Be(0);
            employer.EmployerGuid.Should().Be(Guid.Empty);
            employer.EdsUrn.Should().Be(summary.ReferenceNumber);
            employer.FullName.Should().Be(summary.FullName);

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

        [TestCase(5)]
        public void ShouldMapFromDatabaseToDomain(int howMany)
        {
            // Arrange.
            var dbEmployers = new Fixture()
                .Build<Database.Employer>()
                .CreateMany(howMany);

            foreach (var dbEmployer in dbEmployers)
            {
                // Act.
                var employer = _mapper.Map<Database.Employer, Domain.Employer>(dbEmployer);

                // Assert.
                employer.Should().NotBeNull();

                employer.EmployerId.Should().Be(dbEmployer.EmployerId);
                employer.EmployerGuid.Should().Be(Guid.Empty); // TODO: US1259: AG: remove.
                employer.EdsUrn.Should().Be(Convert.ToString(dbEmployer.EdsUrn));
                employer.FullName.Should().Be(dbEmployer.FullName);
                employer.TradingName.Should().Be(dbEmployer.TradingName);
                employer.PrimaryContact.Should().Be(dbEmployer.PrimaryContact);
                employer.IsPositiveAboutDisability.Should().Be(dbEmployer.DisableAllowed);

                employer.Address.Should().NotBeNull();

                employer.Address.PostalAddressId.Should().Be(0); // TODO: US1259: AG: remove.
                employer.Address.AddressLine1.Should().Be(dbEmployer.AddressLine1);
                employer.Address.AddressLine2.Should().Be(dbEmployer.AddressLine2);
                employer.Address.AddressLine3.Should().Be(dbEmployer.AddressLine3);
                employer.Address.AddressLine4.Should().Be(dbEmployer.AddressLine4);
                employer.Address.AddressLine5.Should().Be(dbEmployer.AddressLine5);
                employer.Address.Town.Should().Be(dbEmployer.Town);
                employer.Address.Postcode.Should().Be(dbEmployer.PostCode);
                employer.Address.ValidationSourceCode.Should().BeNull();
                employer.Address.ValidationSourceKeyValue.Should().BeNull();
                // employer.Address.DateValidated.Should().Be(default(DateTime?)); // TODO: US1259: ?

                employer.Address.GeoPoint.Should().NotBeNull();

                // employer.Address.GeoPoint.Easting.Should().Be(dbEmployer.AddressLine1); // TODO: US1259: AG: add.
                // employer.Address.GeoPoint.Northing.Should().Be(dbEmployer.AddressLine1); // TODO: US1259: AG: add.
                employer.Address.GeoPoint.Latitude.Should().Be(Convert.ToDouble(dbEmployer.Latitude ?? 0m));
                employer.Address.GeoPoint.Longitude.Should().Be(Convert.ToDouble(dbEmployer.Longitude ?? 0m));
                employer.Address.County.Should().Be(dbEmployer.County);
            }
        }

        [TestCase(5)]
        public void ShouldMapFromDomainToDatabase(int howMany)
        {
            // Arrange.
            var fixture = new Fixture();

            var employers = fixture
                .Build<Domain.Employer>()
                .With(each => each.EdsUrn, Convert.ToString(fixture.Create<int>()))
                .CreateMany(howMany);

            foreach (var employer in employers)
            {
                // Act.
                var dbEmployer = _mapper.Map<Domain.Employer, Database.Employer>(employer);

                // Assert.
                dbEmployer.EmployerId.Should().Be(employer.EmployerId);

                dbEmployer.EdsUrn.Should().Be(Convert.ToInt32(employer.EdsUrn));
                dbEmployer.FullName.Should().Be(employer.FullName);
                dbEmployer.TradingName.Should().Be(employer.TradingName);

                dbEmployer.AddressLine1.Should().Be(employer.Address.AddressLine1);
                dbEmployer.AddressLine2.Should().Be(employer.Address.AddressLine2);
                dbEmployer.AddressLine3.Should().Be(employer.Address.AddressLine3);
                dbEmployer.AddressLine4.Should().Be(employer.Address.AddressLine4);
                dbEmployer.AddressLine5.Should().Be(employer.Address.AddressLine5);

                dbEmployer.Town.Should().Be(employer.Address.Town);
                dbEmployer.CountyId.Should().Be(employer.Address.CountyId);
                dbEmployer.PostCode.Should().Be(employer.Address.Postcode);

                dbEmployer.LocalAuthorityId.Should().Be(employer.Address.LocalAuthorityId);

                dbEmployer.Latitude.Should().Be(Convert.ToDecimal(employer.Address.GeoPoint.Latitude));
                dbEmployer.Longitude.Should().Be(Convert.ToDecimal(employer.Address.GeoPoint.Longitude));
                dbEmployer.GeocodeEasting.Should().Be(employer.Address.GeoPoint.Easting);
                dbEmployer.GeocodeNorthing.Should().Be(employer.Address.GeoPoint.Northing);

                dbEmployer.PrimaryContact.Should().Be(employer.PrimaryContact);

                dbEmployer.NumberofEmployeesAtSite.Should().Be(default(int?)); // TODO: US1259: ?
                dbEmployer.NumberOfEmployeesInGroup.Should().Be(default(int?)); // TODO: US1259: ?

                dbEmployer.OwnerOrgnistaion.Should().BeNull(); // TODO: US1259: ?
                dbEmployer.CompanyRegistrationNumber.Should().BeNull(); // TODO: US1259: ?
                dbEmployer.TotalVacanciesPosted.Should().Be(default(int?)); // TODO: US1259: ?

                dbEmployer.BeingSupportedBy.Should().BeNull(); // TODO: US1259: ?
                dbEmployer.LockedForSupportUntil.Should().Be(default(DateTime?)); // TODO: US1259: ?

                dbEmployer.EmployerStatusTypeId.Should().Be((int)employer.EmployerStatus);

                dbEmployer.DisableAllowed.Should().Be(employer.IsPositiveAboutDisability);
                dbEmployer.TrackingAllowed.Should().Be(false); // TODO: US1259: ?
            }
        }
    }
}
