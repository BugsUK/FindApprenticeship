namespace SFA.Apprenticeships.Infrastructure.Repositories.Sql.UnitTests.Vacancy
{
    using FluentAssertions;
    using NUnit.Framework;
    using Ploeh.AutoFixture;
    using Schemas.Vacancy;
    using DbVacancyLocation = Schemas.Vacancy.Entities.VacancyLocation;
    using DomainVacancyLocation = Domain.Entities.Raa.Locations.VacancyLocation;

    [TestFixture]
    [Parallelizable]
    public class ApprenticeshipVacancyLocationMappersUnitTests : TestBase
    {
        
        [Test]
        public void DoesVacancyLocationDomainObjectToDatabaseObjectMapperChokeInPractice()
        {
            // Arrange
            var x = new VacancyMappers();

            var vacancy =
                new Fixture().Build<DomainVacancyLocation>()
                    .Create();

            // Act / Assert no exception
            x.Map<DomainVacancyLocation, DbVacancyLocation>(vacancy);
        }

        [Test]
        public void DoesDatabaseVacancyObjectToDomainObjectMapperChokeInPractice()
        {
            // Arrange
            var mapper = new VacancyMappers();
            var databaseVacancyLocation = CreateValidDatabaseVacancyLocation();

            // Act / Assert no exception
            mapper.Map<DbVacancyLocation, DomainVacancyLocation>(databaseVacancyLocation);
        }

        [Test]
        public void DoesDatabaseVacancyObjectMappingRoundTripViaDomainObjectExcludingHardOnes()
        {
            // Arrange
            var mapper = new VacancyMappers();
            var domainVacancyLocation1 = new Fixture().Create<DomainVacancyLocation>();

            // Act

            var databaseVacancyLocation = mapper.Map<DomainVacancyLocation, DbVacancyLocation>(domainVacancyLocation1);
            var domainVacancyLocation2 = mapper.Map<DbVacancyLocation, DomainVacancyLocation>(databaseVacancyLocation);

            // Assert
            domainVacancyLocation2.ShouldBeEquivalentTo(domainVacancyLocation1, options => options
                .Excluding(vl => vl.Address.PostalAddressId)
                .Excluding(vl => vl.Address.ValidationSourceCode)
                .Excluding(vl => vl.Address.ValidationSourceKeyValue)
                .Excluding(vl => vl.Address.DateValidated)
                .Excluding(vl => vl.Address.CountyId)
                .Excluding(vl => vl.Address.County)
                .Excluding(vl => vl.Address.LocalAuthorityId)
                .Excluding(vl => vl.Address.LocalAuthorityCodeName)
                .Excluding(vl => vl.Address.LocalAuthority)
                .Excluding(vl => vl.LocalAuthorityCode));
        }
        
        [Test]
        public void DoesVacancyLocationDomainObjectMappingRoundTripViaDatabaseObject()
        {
            // Arrange
            var mapper = new VacancyMappers();
            var databaseVacancyLocation1 = CreateValidDatabaseVacancyLocation();

            // Act
            var domainVacancyLocation = mapper.Map<DbVacancyLocation, DomainVacancyLocation>(databaseVacancyLocation1);
            var databaseVacancyLocation2 = mapper.Map<DomainVacancyLocation, DbVacancyLocation>(domainVacancyLocation);

            // Assert
            databaseVacancyLocation2.ShouldBeEquivalentTo(databaseVacancyLocation1, options => options
                .Excluding(vl => vl.EmployersWebsite)
                .Excluding(vl => vl.CountyId)
                .Excluding(vl => vl.LocalAuthorityId)
                .Excluding(vl => vl.GeocodeEasting)
                .Excluding(vl => vl.GeocodeNorthing)
                );
        }
    }
}