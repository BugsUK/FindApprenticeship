namespace SFA.Apprenticeships.Infrastructure.Repositories.Sql.UnitTests.Reference
{
    using FluentAssertions;
    using NUnit.Framework;
    using Ploeh.AutoFixture;
    using SFA.Infrastructure.Interfaces;
    using Schemas.Reference;
    using Schemas.Reference.Entities;
    using DomainSector = Domain.Entities.Raa.Vacancies.Sector;
    using DomainFramework = Domain.Entities.Raa.Reference.Framework;
    using DomainOccupation = Domain.Entities.Raa.Reference.Occupation;
    //using DbCounty = Schemas.Reference.Entities.County;
    //using DomainCounty = Domain.Entities.Raa.Reference.County;
    //using DbLocalAuthority = Schemas.Reference.Entities.LocalAuthority;
    //using DomainLocalAuthority = Domain.Entities.Raa.Reference.LocalAuthority;


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

        //[Test]
        //public void ShouldMapCounty_DomainToDb()
        //{
        //    //Arrange
        //    var source = new Fixture().Build<DomainCounty>().Create();

        //    //Act
        //    var destination = _mapper.Map<DomainCounty, DbCounty>(source);

        //    //Assert
        //    destination.Should().NotBeNull();
        //    destination.CountyId.Should().Be(source.CountyId);
        //    destination.CodeName.Should().Be(source.CodeName);
        //    destination.ShortName.Should().Be(source.ShortName);
        //    destination.FullName.Should().Be(source.FullName);
        //}

        //[Test]
        //public void ShouldMapCounty_DbToDomain()
        //{
        //    //Arrange
        //    var source = new Fixture().Build<DbCounty>().Create();

        //    //Act
        //    var destination = _mapper.Map<DbCounty, DomainCounty>(source);

        //    //Assert
        //    destination.Should().NotBeNull();
        //    destination.CountyId.Should().Be(source.CountyId);
        //    destination.CodeName.Should().Be(source.CodeName);
        //    destination.ShortName.Should().Be(source.ShortName);
        //    destination.FullName.Should().Be(source.FullName);
        //}

        //[Test]
        //public void ShouldMapRegion()
        //{
        //    //TODO: Fix this
        //    Assert.Inconclusive();

        //    //Arrange
        //    var source = new Fixture().Build<Region>().Create();

        //    //Act
        //    var destination = _mapper.Map<Region, Region>(source);

        //    //Assert
        //    destination.Should().NotBeNull();
        //    destination.RegionId.Should().Be(source.RegionId);
        //    destination.CodeName.Should().Be(source.CodeName);
        //    destination.ShortName.Should().Be(source.ShortName);
        //    destination.FullName.Should().Be(source.FullName);
        //}

        //[Test]
        //public void ShouldMapLocalAuthority_DomainToDb()
        //{
        //    //Arrange
        //    var source = new Fixture().Build<DomainLocalAuthority>().Create();

        //    //Act
        //    var destination = _mapper.Map<DomainLocalAuthority, DbLocalAuthority>(source);

        //    //Assert
        //    destination.Should().NotBeNull();
        //    destination.LocalAuthorityId.Should().Be(source.LocalAuthorityId);
        //    destination.CodeName.Should().Be(source.CodeName);
        //    destination.ShortName.Should().Be(source.ShortName);
        //    destination.FullName.Should().Be(source.FullName);
        //    destination.County.Should().NotBeNull();
        //    destination.County.CountyId.Should().Be(source.County.CountyId);
        //    destination.County.CodeName.Should().Be(source.County.CodeName);
        //    destination.County.ShortName.Should().Be(source.County.ShortName);
        //    destination.County.FullName.Should().Be(source.County.FullName);
        //}

        //[Test]
        //public void ShouldMapLocalAuthority_DbToDomain()
        //{
        //    //Arrange
        //    var source = new Fixture().Build<DbLocalAuthority>().Create();

        //    //Act
        //    var destination = _mapper.Map<DbLocalAuthority, DomainLocalAuthority>(source);

        //    //Assert
        //    destination.Should().NotBeNull();
        //    destination.LocalAuthorityId.Should().Be(source.LocalAuthorityId);
        //    destination.CodeName.Should().Be(source.CodeName);
        //    destination.ShortName.Should().Be(source.ShortName);
        //    destination.FullName.Should().Be(source.FullName);
        //    destination.County.Should().NotBeNull();
        //    destination.County.CountyId.Should().Be(source.County.CountyId);
        //    destination.County.CodeName.Should().Be(source.County.CodeName);
        //    destination.County.ShortName.Should().Be(source.County.ShortName);
        //    destination.County.FullName.Should().Be(source.County.FullName);
        //}

        [Test]
        public void ShouldMapSector_DbToDomain()
        {
            //Arrange
            var source = new StandardSector() {FullName = "my lovely name"};

            //Act
            var destination = _mapper.Map<StandardSector, DomainSector>(source);

            //Assert
            destination.Should().NotBeNull();
            destination.Name.Should().Be(source.FullName);
            destination.Id.Should().Be(source.StandardSectorId);
            destination.Standards.Should().BeNull();
        }

        [Test]
        public void ShouldMapSector_DomainToDb()
        {
            //Arrange
            var source = new Fixture().Build<DomainSector>().Create();

            //Act
            var destination = _mapper.Map<DomainSector, StandardSector>(source);

            //Assert
            destination.Should().NotBeNull();
            destination.FullName.Should().Be(source.Name);
            destination.StandardSectorId.Should().Be(source.Id);
        }

        [Test]
        public void ShouldMapFramework_DbToDomain()
        {
            //Arrange
            var source = new ApprenticeshipFramework()
                             {
                                 FullName = "Fake full name",
                                 CodeName = "fakeness",
                                 ShortName = "Shortie"
                             };

            //Act
            var destination = _mapper.Map<ApprenticeshipFramework, DomainFramework>(source);

            //Assert
            destination.Should().NotBeNull();
            destination.FullName.Should().Be(source.FullName);
            destination.CodeName.Should().Be(source.CodeName);
            destination.ShortName.Should().Be(source.ShortName);
            destination.Id.Should().Be(source.ApprenticeshipFrameworkId);            
            destination.Occupation.Id.Should().Be(source.ApprenticeshipOccupationId);       
        }

        [Test]
        public void ShouldMapFramework_DomainToDb()
        {
            //Arrange
            var source = new Fixture().Build<DomainFramework>().Create();

            //Act
            var destination = _mapper.Map<DomainFramework, ApprenticeshipFramework>(source);

            //Assert
            destination.Should().NotBeNull();
            destination.FullName.Should().Be(source.FullName);
            destination.CodeName.Should().Be(source.CodeName);
            destination.ShortName.Should().Be(source.ShortName);
            destination.ApprenticeshipFrameworkId.Should().Be(source.Id);
            destination.ApprenticeshipFrameworkStatusTypeId.Should().Be(0);
            destination.ClosedDate.Should().NotHaveValue();
            destination.PreviousApprenticeshipOccupationId.Should().NotHaveValue();
        }

        [Test]
        public void ShouldMapOccupation_DomainToDb()
        {
            //Arrange
            var source = new Fixture().Build<DomainOccupation>().Create();

            //Act
            var destination = _mapper.Map<DomainOccupation, ApprenticeshipOccupation>(source);

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
            var source = new ApprenticeshipOccupation() { FullName = "Fullness", CodeName = "Coded", ShortName = "Short name"};

            //Act
            var destination = _mapper.Map<ApprenticeshipOccupation, DomainOccupation>(source);

            //Assert
            destination.Should().NotBeNull();
            destination.FullName.Should().Be(source.FullName);
            destination.CodeName.Should().Be(source.CodeName);
            destination.ShortName.Should().Be(source.ShortName);
        }
    }
}