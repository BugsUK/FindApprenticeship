namespace SFA.Apprenticeships.Infrastructure.UnitTests.Raa
{
    using System.Collections.Generic;
    using System.Linq;
    using Domain.Entities.Raa.Reference;
    using Domain.Entities.Raa.Vacancies;
    using Domain.Entities.ReferenceData;
    using Moq;
    using NUnit.Framework;
    using Domain.Raa.Interfaces.Repositories;
    using FluentAssertions;
    using Infrastructure.Raa;
    using Infrastructure.Raa.Strategies;

    [TestFixture]
    [Parallelizable]
    public class ReferenceDataProviderTests
    {
        private Mock<IReferenceRepository> _mockReferenceRepo;
        private ReferenceDataProvider _providerUnderTest;
        private List<Sector> _standardSectorList;
        private List<Standard> _standardList;
        private List<Framework> _frameworkList;
        private List<Occupation> _occupationList;

        private Standard _standardA;
        private Standard _standardB;
        private Standard _standardC;
        private Sector _standardSector;
        private Sector _standardSectorWithNameSharedWithFramework;
        private Framework _frameworkA;
        private Framework _frameworkB;
        private Framework _frameworkC;
        private Framework _frameworkWithNameSharedWithStandardSector;
        private Occupation _occupation;

        [SetUp]
        public void SetUpFixture()
        {            
            _standardA = new Standard {Id = 1, Name = "Standard A", ApprenticeshipLevel = ApprenticeshipLevel.Degree, ApprenticeshipSectorId = 1};
            _standardB = new Standard {Id = 12, Name = "Standard B", ApprenticeshipLevel = ApprenticeshipLevel.Advanced, ApprenticeshipSectorId = 1};
            _standardC = new Standard {Id = 13, Name = "Standard C", ApprenticeshipLevel = ApprenticeshipLevel.FoundationDegree, ApprenticeshipSectorId = 11};
            _standardList = new List<Standard> { _standardA, _standardB, _standardC };
            _standardSector = new Sector {Id = 1, Name = "Sector A", ApprenticeshipOccupationId = 1, Standards = new List<Standard> { _standardA, _standardB } };
            _standardSectorWithNameSharedWithFramework = new Sector {Id = 11, Name = "Duplicate Name", ApprenticeshipOccupationId = 1, Standards = new List<Standard> { _standardC } };
            _standardSectorList = new List<Sector> { _standardSector, _standardSectorWithNameSharedWithFramework };
            _frameworkA = new Framework {Id = 1, CodeName = "FW.1", FullName = "Full Name", ShortName = "Short", ParentCategoryCodeName = "SSAT1.MFP" };
            _frameworkB = new Framework {Id = 12, CodeName = "FW.12", FullName = "Duplicate Framework Name (Lantra)", ShortName = "Short", ParentCategoryCodeName = "SSAT1.MFP" };
            _frameworkC = new Framework {Id = 13, CodeName = "FW.13", FullName = "Duplicate Framework Name", ShortName = "Short", ParentCategoryCodeName = "SSAT1.MFP" };
            _frameworkWithNameSharedWithStandardSector = new Framework {Id = 14, CodeName = "FW.14", FullName = "Duplicate Name (Lantra)", ShortName = "Short", ParentCategoryCodeName = "SSAT1.MFP" };
            _frameworkList = new List<Framework> { _frameworkA, _frameworkB, _frameworkC, _frameworkWithNameSharedWithStandardSector };
            _occupation = new Occupation { Id = 1, CodeName = "SSAT1.MFP", FullName = "Full Occupation name", Frameworks = _frameworkList, ShortName = "short" };            
            _occupationList = new List<Occupation> { _occupation };

            _mockReferenceRepo = new Mock<IReferenceRepository>();
            _providerUnderTest = new ReferenceDataProvider(_mockReferenceRepo.Object, new GetReleaseNotesStrategy(_mockReferenceRepo.Object));
            _mockReferenceRepo.Setup(mrr => mrr.GetSectors()).Returns(_standardSectorList);
            _mockReferenceRepo.Setup(mrr => mrr.GetStandards()).Returns(_standardList);
            _mockReferenceRepo.Setup(mrr => mrr.GetFrameworks()).Returns(_frameworkList);
            _mockReferenceRepo.Setup(mrr => mrr.GetOccupations()).Returns(_occupationList);
        }

        [Test]
        public void GetSectors()
        {
            //Arrange            
            
            //Act
            var sectors = _providerUnderTest.GetSectors().ToList();

            //Assert
            sectors.Should().NotBeNullOrEmpty();
            sectors.Count.Should().Be(2);
        }

        [Test]
        public void GetCategories()
        {
            //Arrange            

            //Act
            var categories = _providerUnderTest.GetCategories().ToList();

            //Assert
            categories.Should().NotBeNullOrEmpty();
            var category = categories.Single();
            category.SubCategories.Count.Should().Be(4); //infer the two frameworks, one duplicate, one standard sector and one de duplicated combined framework and standard sector
            category.SubCategories.Count(sc => sc.CategoryType == CategoryType.Framework).Should().Be(1); //one framework
            category.SubCategories.Count(sc => sc.CategoryType == CategoryType.StandardSector).Should().Be(1); //one sector
            category.SubCategories.Count(sc => sc.CategoryType == CategoryType.Combined).Should().Be(2); //one de duplicated combined framework and one de duplicated combined framework and standard sector
            var standardSector = category.SubCategories.First(sc => sc.CategoryType == CategoryType.StandardSector);
            standardSector.SubCategories.Count.Should().Be(2); //2 standards under the one sector
            var combinedFrameworkAndStandardSector = category.SubCategories.First(sc => sc.CodeName.Contains("STDSEC."));
            combinedFrameworkAndStandardSector.FullName.Should().Be("Duplicate Name");
            combinedFrameworkAndStandardSector.ParentCategoryCodeName.Should().Be("SSAT1.MFP");
            combinedFrameworkAndStandardSector.SubCategories.Count.Should().Be(1); //1 standards under the combined
            combinedFrameworkAndStandardSector.SubCategories[0].ParentCategoryCodeName.Should().Be("STDSEC.11|FW.14");
            var combinedFramework = category.SubCategories.First(sc => !sc.CodeName.Contains("STDSEC."));
            combinedFramework.FullName.Should().Be("Duplicate Framework Name");
            combinedFramework.SubCategories.Count.Should().Be(0);
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
            var category = _providerUnderTest.GetCategoryByCode(catCodeName);

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
            var category = _providerUnderTest.GetCategoryByCode(catCodeName);

            //Assert
            category.Should().BeNull();
        }

        [TestCase("Full Occupation name")]
        public void GetCategoryByName(string catCodeName)
        {
            //Arrange            

            //Act
            var category = _providerUnderTest.GetCategoryByName(catCodeName);

            //Assert
            category.Should().NotBeNull();
            category.FullName.Should().Be(catCodeName);
        }

        [TestCase("FW.1", "FW.1")]
        [TestCase("FW.12", "FW.13|FW.12")]
        [TestCase("FW.13", "FW.13|FW.12")]
        [TestCase("FW.14", "STDSEC.11|FW.14")]
        [TestCase("STDSEC.1", "STDSEC.1")]
        [TestCase("STDSEC.11", "STDSEC.11|FW.14")]
        public void GetSubCategoryByCode(string catCodeName, string expectedCode)
        {
            //Arrange            

            //Act
            var category = _providerUnderTest.GetSubCategoryByCode(catCodeName);

            //Assert
            category.Should().NotBeNull();
            category.CodeName.Should().Be(expectedCode);
        }

        [TestCase("Duplicate Framework Name")]
        [TestCase("Full Name")]
        public void GetSubCategoryByName(string subcatCodeName)
        {
            //Arrange            

            //Act
            var category = _providerUnderTest.GetSubCategoryByName(subcatCodeName);

            //Assert
            category.Should().NotBeNull();
            category.FullName.Should().Be(subcatCodeName);
        }
    }
}