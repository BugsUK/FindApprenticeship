namespace SFA.Apprenticeships.Infrastructure.Repositories.Sql.UnitTests.Reference
{
    using System;
    using Domain.Entities.Raa.Vacancies;
    using FluentAssertions;
    using NUnit.Framework;
    using Ploeh.AutoFixture;
    using SFA.Infrastructure.Interfaces;
    using Schemas.Reference;
    using Schemas.Reference.Entities;
    using DbStandard = Schemas.Reference.Entities.Standard;
    using DomainStandard = Domain.Entities.Raa.Vacancies.Standard;
    using DbSector = Schemas.Reference.Entities.Sector;
    using DomainSector = Domain.Entities.Raa.Vacancies.Sector;
    using DbFramework = Schemas.Reference.Entities.Framework;
    using DomainFramework = Domain.Entities.Raa.Reference.Framework;
    using DbOccupation = Schemas.Reference.Entities.Occupation;
    using DomainOccupation = Domain.Entities.Raa.Reference.Occupation;


    [TestFixture]
    public class ReferenceMappersTests
    {
        private IMapper _mapper;

        [TestFixtureSetUp]
        public void Setup()
        {
            _mapper = new ReferenceMappers();
        }

        [Test]
        public void ShouldCreateMap()
        {
            new ReferenceMappers().Mapper.AssertConfigurationIsValid();
        }

        [Test]
        public void ShouldMapCounty()
        {
            //TODO: Fix this
            Assert.Inconclusive();

            //Arrange
            var source = new Fixture().Build<County>().Without(c => c.PostalAddresses).Create();

            //Act
            var destination = _mapper.Map<County, County>(source);

            //Assert
            destination.Should().NotBeNull();
            destination.CountyId.Should().Be(source.CountyId);
            destination.CodeName.Should().Be(source.CodeName);
            destination.ShortName.Should().Be(source.ShortName);
            destination.FullName.Should().Be(source.FullName);
        }

        [Test]
        public void ShouldMapRegion()
        {
            //TODO: Fix this
            Assert.Inconclusive();
            
            //Arrange
            var source = new Fixture().Build<Region>().Create();

            //Act
            var destination = _mapper.Map<Region, Region>(source);

            //Assert
            destination.Should().NotBeNull();
            destination.RegionId.Should().Be(source.RegionId);
            destination.CodeName.Should().Be(source.CodeName);
            destination.ShortName.Should().Be(source.ShortName);
            destination.FullName.Should().Be(source.FullName);
        }

        [Test]
        public void ShouldMapLocalAuthority()
        {
            //TODO: Fix this
            Assert.Inconclusive();
            
            //Arrange
            var source = new Fixture().Build<LocalAuthority>().Create();

            //Act
            var destination = _mapper.Map<LocalAuthority, LocalAuthority>(source);

            //Assert
            destination.Should().NotBeNull();
            destination.LocalAuthorityId.Should().Be(source.LocalAuthorityId);
            destination.CodeName.Should().Be(source.CodeName);
            destination.ShortName.Should().Be(source.ShortName);
            destination.FullName.Should().Be(source.FullName);
            destination.County.Should().NotBeNull();
            destination.County.CountyId.Should().Be(source.County.CountyId);
            destination.County.CodeName.Should().Be(source.County.CodeName);
            destination.County.ShortName.Should().Be(source.County.ShortName);
            destination.County.FullName.Should().Be(source.County.FullName);
        }

        [Test]
        public void ShouldMapStandard_DbToDomain()
        {
            //Arrange
            var source = new DbStandard() { Level = new Level() {LevelCode = ApprenticeshipLevel.Degree.ToString("D") } };

            //Act
            var destination = _mapper.Map<DbStandard, DomainStandard>(source);

            //Assert
            destination.Should().NotBeNull();
            destination.ApprenticeshipLevel.ToString("D").Should().Be(source.Level.LevelCode);
            destination.Name.Should().Be(source.FullName);
            destination.ApprenticeshipSectorId.Should().Be(source.SectorId);
            destination.Id.Should().Be(source.StandardId);
        }

        [Test]
        public void ShouldMapStandard_DomainToDb()
        {
            //Arrange
            var source = new Fixture().Build<DomainStandard>().Create();

            //Act
            var destination = _mapper.Map<DomainStandard, DbStandard>(source);

            //Assert
            destination.Should().NotBeNull();
            destination.FullName.Should().Be(source.Name);
            destination.Level.Should().NotBeNull();
            destination.LevelCode.Should().Be(source.ApprenticeshipLevel.ToString("D"));
            destination.Level.LevelCode.Should().Be(source.ApprenticeshipLevel.ToString("D"));
        }

        [Test]
        public void ShouldMapSector_DbToDomain()
        {
            //Arrange
            var source = new DbSector() {FullName = "my lovely name"};

            //Act
            var destination = _mapper.Map<DbSector, DomainSector>(source);

            //Assert
            destination.Should().NotBeNull();
            destination.Name.Should().Be(source.FullName);
            destination.Id.Should().Be(source.SectorId);
            destination.Standards.Should().BeNull();
        }

        [Test]
        public void ShouldMapSector_DomainToDb()
        {
            //Arrange
            var source = new Fixture().Build<DomainSector>().Create();

            //Act
            var destination = _mapper.Map<DomainSector, DbSector>(source);

            //Assert
            destination.Should().NotBeNull();
            destination.FullName.Should().Be(source.Name);
            destination.SectorId.Should().Be(source.Id);
            destination.Standards.Should().BeEmpty();
        }

        [Test]
        public void ShouldMapFramework_DbToDomain()
        {
            //Arrange
            var source = new DbFramework() { FullName = "Fake full name", CodeName = "fakeness", ShortName = "Shortie"};

            //Act
            var destination = _mapper.Map<DbFramework, DomainFramework>(source);

            //Assert
            destination.Should().NotBeNull();
            destination.FullName.Should().Be(source.FullName);
            destination.CodeName.Should().Be(source.CodeName);
            destination.ShortName.Should().Be(source.ShortName);
            destination.Id.Should().Be(source.FrameworkId);
            destination.Occupation.Should().BeNull();
        }

        [Test]
        public void ShouldMapFramework_DomainToDb()
        {
            //Arrange
            var source = new Fixture().Build<DomainFramework>().Create();

            //Act
            var destination = _mapper.Map<DomainFramework, DbFramework>(source);

            //Assert
            destination.Should().NotBeNull();
            destination.FullName.Should().Be(source.FullName);
            destination.CodeName.Should().Be(source.CodeName);
            destination.ShortName.Should().Be(source.ShortName);
            destination.FrameworkId.Should().Be(source.Id);
            destination.FrameworkStatus.Should().BeNull();
            destination.FrameworkStatusId.Should().Be(0);
            destination.ClosedDate.Should().NotHaveValue();
            destination.Occupation1.Should().BeNull();
            destination.Occupation.Should().BeNull();
            destination.PreviousOccupationId.Should().NotHaveValue();
            destination.Vacancies.Should().BeEmpty();
        }

        [Test]
        public void ShouldMapOccupation_DomainToDb()
        {
            //Arrange
            var source = new Fixture().Build<DomainOccupation>().Create();

            //Act
            var destination = _mapper.Map<DomainOccupation, DbOccupation>(source);

            //Assert
            destination.Should().NotBeNull();
            destination.FullName.Should().Be(source.FullName);
            destination.CodeName.Should().Be(source.CodeName);
            destination.ShortName.Should().Be(source.ShortName);
        }

        [Test]
        public void ShouldMapOccupation_DbToDomain()
        {
            //Arrange
            var source = new DbOccupation() { FullName = "Fullness", CodeName = "Coded", ShortName = "Short name"};

            //Act
            var destination = _mapper.Map<DbOccupation, DomainOccupation>(source);

            //Assert
            destination.Should().NotBeNull();
            destination.FullName.Should().Be(source.FullName);
            destination.CodeName.Should().Be(source.CodeName);
            destination.ShortName.Should().Be(source.ShortName);
        }
    }
}