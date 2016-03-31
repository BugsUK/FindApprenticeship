namespace SFA.Apprenticeships.Infrastructure.UnitTests.Raa
{
    using System.Collections.Generic;
    using System.Linq;
    using Domain.Entities.Raa.Reference;
    using Domain.Entities.Raa.Vacancies;
    using Moq;
    using NUnit.Framework;
    using Domain.Raa.Interfaces.Repositories;
    using FluentAssertions;
    using Infrastructure.Raa;

    public class ReferenceDataProviderTests
    {
        private Mock<IReferenceRepository> mockReferenceRepo;
        private ReferenceDataProvider providerUnderTest;
        private List<Sector> standardSectorList;
        private List<Standard> standardList;
        private List<Framework> frameworkList;
        private List<Occupation> occupationList;

        private Standard standardA;
        private Standard standardB;
        private Sector standardSector;
        private Framework frameworkA;
        private Framework frameworkB;
        private Occupation occupation;

        [SetUp]
        public void SetUpFixture()
        {            
            standardA = new Standard() {Id = 1,Name = "Standard A", ApprenticeshipLevel = ApprenticeshipLevel.Degree, ApprenticeshipSectorId = 10};
            standardB = new Standard() {Id = 2,Name = "Standard B", ApprenticeshipLevel = ApprenticeshipLevel.Advanced, ApprenticeshipSectorId = 10};
            standardList = new List<Standard>() { standardA, standardB };
            standardSector = new Sector() {Id = 10, Name = "Sector A", Standards = standardList};
            standardSectorList = new List<Sector>() { standardSector };
            frameworkA = new Framework(){Id = 1,CodeName = "FW.1",FullName = "Full Name",ShortName = "Short",ParentCategoryCodeName = "SSAT1.MFP" };
            frameworkB = new Framework(){Id = 2,CodeName = "FW.2",FullName = "Full Name2",ShortName = "Short",ParentCategoryCodeName = "SSAT1.MFP" };
            frameworkList = new List<Framework>() { frameworkA, frameworkB };
            occupation = new Occupation() { Id = 1, CodeName = "SSAT1.MFP", FullName = "Full Occupation name", Frameworks = frameworkList, ShortName = "short" };            
            occupationList = new List<Occupation>() { occupation };

            mockReferenceRepo = new Mock<IReferenceRepository>();
            providerUnderTest = new ReferenceDataProvider(mockReferenceRepo.Object);
            mockReferenceRepo.Setup(mrr => mrr.GetSectors()).Returns(this.standardSectorList);
            mockReferenceRepo.Setup(mrr => mrr.GetStandards()).Returns(standardList);
            mockReferenceRepo.Setup(mrr => mrr.GetFrameworks()).Returns(frameworkList);
            mockReferenceRepo.Setup(mrr => mrr.GetOccupations()).Returns(occupationList);
        }

        [Test]
        public void GetSectors()
        {
            //Arrange            
            
            //Act
            var sectors = providerUnderTest.GetSectors();

            //Assert
            sectors.Should().NotBeNullOrEmpty();
            sectors.Count().Should().Be(1);
        }

        [Test]
        public void GetCategories()
        {
            //Arrange            

            //Act
            var categories = providerUnderTest.GetCategories();

            //Assert
            categories.Should().NotBeNullOrEmpty();
            var category = categories.Single();
            category.SubCategories.Count.Should().Be(3); //infer the two frameworks and one sector
            category.SubCategories.Count(sc => sc.CodeName.StartsWith("FW")).Should().Be(2); //two frameworks
            category.SubCategories.Count(sc => sc.CodeName.StartsWith("STDSEC")).Should().Be(1);//one sector
            var sector = category.SubCategories.First(sc => sc.CodeName.StartsWith("STDSEC"));
            sector.SubCategories.Count.Should().Be(2); //2 standards under the one sector
        }

        /// <summary>
        /// Also do negative
        /// </summary>
        /// <param name="catCodeName"></param>
        [TestCase("SSAT1.MFP")]
        public void GetCategoryByCode(string catCodeName)
        {
            //Arrange            

            //Act
            var category = providerUnderTest.GetCategoryByCode(catCodeName);

            //Assert
            category.Should().NotBeNull();
            category.CodeName.Should().Be(catCodeName);
        }

        [TestCase("Some random code")]
        [TestCase("STDSEC.10")]
        [TestCase("FW.2")]
        public void GetCategoryByCode_ShouldReturnNull(string catCodeName)
        {
            //Arrange            

            //Act
            var category = providerUnderTest.GetCategoryByCode(catCodeName);

            //Assert
            category.Should().BeNull();
        }

        [TestCase("Full Occupation name")]
        public void GetCategoryByName(string catCodeName)
        {
            //Arrange            

            //Act
            var category = providerUnderTest.GetCategoryByName(catCodeName);

            //Assert
            category.Should().NotBeNull();
            category.FullName.Should().Be(catCodeName);
        }

        [TestCase("STDSEC.10")]
        [TestCase("FW.2")]
        public void GetSubCategoryByCode(string catCodeName)
        {
            //Arrange            

            //Act
            var category = providerUnderTest.GetSubCategoryByCode(catCodeName);

            //Assert
            category.Should().NotBeNull();
        }

        [TestCase("Full Name2")]
        [TestCase("Full Name")]
        public void GetSubCategoryByName(string subcatCodeName)
        {
            //Arrange            

            //Act
            var category = providerUnderTest.GetSubCategoryByName(subcatCodeName);

            //Assert
            category.Should().NotBeNull();
            category.FullName.Should().Be(subcatCodeName);
        }
    }
}