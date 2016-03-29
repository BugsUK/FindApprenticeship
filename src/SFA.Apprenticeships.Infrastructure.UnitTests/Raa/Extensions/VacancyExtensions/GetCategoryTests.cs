namespace SFA.Apprenticeships.Infrastructure.UnitTests.Raa.Extensions.VacancyExtensions
{
    using System.Collections.Generic;
    using Domain.Entities.Raa.Vacancies;
    using Domain.Entities.ReferenceData;
    using FluentAssertions;
    using Infrastructure.Raa.Extensions;
    using NUnit.Framework;
    using Ploeh.AutoFixture;

    [TestFixture]
    public class GetCategoryTests
    {
        private List<Category> _categories;

        [SetUp]
        public void SetUp()
        {
            _categories = new List<Category>
            {
                new Category("SSAT1.ICT", "Information and Communication Technology", CategoryType.SectorSubjectAreaTier1,
                    new List<Category>
                    {
                        new Category("FW.101", "Software Developer", "SSAT1.ICT", CategoryType.Framework),
                        new Category("STDSEC.201", "Digital Industries", "SSAT1.ICT", CategoryType.StandardSector, 
                            new List<Category>
                            {
                                new Category("STD.1", "Network Engineer", "STDSEC.201", CategoryType.Standard),
                                new Category("STD.2", "Software Developer", "STDSEC.201", CategoryType.Standard)
                            }
                        )
                    }
                )
            };
        }

        [TestCase(VacancyType.Apprenticeship, "101", "SSAT1.ICT", "Information and Communication Technology")]
        [TestCase(VacancyType.Apprenticeship, null, "SSAT1.UNKNOWN", "Unknown Sector Subject Area Tier 1")]
        [TestCase(VacancyType.Apprenticeship, "XXX", "SSAT1.UNKNOWN", "Unknown Sector Subject Area Tier 1")]
        [TestCase(VacancyType.Traineeship, "101", "SSAT1.INVALID", "Invalid Sector Subject Area Tier 1")]
        [TestCase(VacancyType.Traineeship, null, "SSAT1.INVALID", "Invalid Sector Subject Area Tier 1")]
        public void ShouldGetCategoryForFramework(
            VacancyType vacancyType, string frameworkCodeName, string expectedCategoryCode, string expectedCategoryName)
        {
            // Arrange.
            var vacancySummary = new Fixture()
                .Build<VacancySummary>()
                .With(each => each.VacancyType, vacancyType)
                .With(each => each.TrainingType, TrainingType.Frameworks)
                .With(each => each.FrameworkCodeName, frameworkCodeName)
                .Create();

            // Act.
            var category = vacancySummary.GetCategory(_categories);

            // Assert.
            category.CodeName.Should().Be(expectedCategoryCode);
            category.FullName.Should().Be(expectedCategoryName);
        }

        [TestCase(VacancyType.Apprenticeship, 2, "SSAT1.ICT", "Information and Communication Technology")]
        [TestCase(VacancyType.Apprenticeship, null, "SSAT1.UNKNOWN", "Unknown Sector Subject Area Tier 1")]
        [TestCase(VacancyType.Apprenticeship, -1, "SSAT1.UNKNOWN", "Unknown Sector Subject Area Tier 1")]
        [TestCase(VacancyType.Traineeship, 2, "SSAT1.INVALID", "Invalid Sector Subject Area Tier 1")]
        [TestCase(VacancyType.Traineeship, null, "SSAT1.INVALID", "Invalid Sector Subject Area Tier 1")]
        public void ShouldGetCategoryForStandard(
            VacancyType vacancyType, int? standardId, string expectedCategoryCode, string expectedCategoryName)
        {
            // Arrange.
            var vacancySummary = new Fixture()
                .Build<VacancySummary>()
                .With(each => each.VacancyType, vacancyType)
                .With(each => each.TrainingType, TrainingType.Standards)
                .With(each => each.StandardId, standardId)
                .Create();

            // Act.
            var category = vacancySummary.GetCategory(_categories);

            // Assert.
            category.CodeName.Should().Be(expectedCategoryCode);
            category.FullName.Should().Be(expectedCategoryName);
        }

        [TestCase(VacancyType.Traineeship, "ICT", "SSAT1.ICT", "Information and Communication Technology")]
        [TestCase(VacancyType.Traineeship, null, "SSAT1.UNKNOWN", "Unknown Sector Subject Area Tier 1")]
        [TestCase(VacancyType.Traineeship, "XXX", "SSAT1.UNKNOWN", "Unknown Sector Subject Area Tier 1")]
        [TestCase(VacancyType.Apprenticeship, "ICT", "SSAT1.INVALID", "Invalid Sector Subject Area Tier 1")]
        [TestCase(VacancyType.Apprenticeship, null, "SSAT1.INVALID", "Invalid Sector Subject Area Tier 1")]
        public void ShouldGetCategoryForSector(
            VacancyType vacancyType, string sectorCodeName, string expectedCategoryCode, string expectedCategoryName)
        {
            // Arrange.
            var vacancySummary = new Fixture()
                .Build<VacancySummary>()
                .With(each => each.VacancyType, vacancyType)
                .With(each => each.TrainingType, TrainingType.Sectors)
                .With(each => each.SectorCodeName, sectorCodeName)
                .Create();

            // Act.
            var category = vacancySummary.GetCategory(_categories);

            // Assert.
            category.CodeName.Should().Be(expectedCategoryCode);
            category.FullName.Should().Be(expectedCategoryName);
        }

        [TestCase(VacancyType.Apprenticeship, "101", "SSAT1.ICT", "Information and Communication Technology")]
        [TestCase(VacancyType.Apprenticeship, null, "SSAT1.UNKNOWN", "Unknown Sector Subject Area Tier 1")]
        [TestCase(VacancyType.Apprenticeship, "XXX", "SSAT1.UNKNOWN", "Unknown Sector Subject Area Tier 1")]
        [TestCase(VacancyType.Traineeship, "101", "SSAT1.INVALID", "Invalid Sector Subject Area Tier 1")]
        [TestCase(VacancyType.Traineeship, null, "SSAT1.INVALID", "Invalid Sector Subject Area Tier 1")]
        public void ShouldGetCategoryForUnknown(
            VacancyType vacancyType, string frameworkCodeName, string expectedCategoryCode, string expectedCategoryName)
        {
            // Arrange.
            var vacancySummary = new Fixture()
                .Build<VacancySummary>()
                .With(each => each.VacancyType, vacancyType)
                .With(each => each.TrainingType, TrainingType.Unknown)
                .With(each => each.FrameworkCodeName, frameworkCodeName)
                .Create();

            // Act.
            var category = vacancySummary.GetCategory(_categories);

            // Assert.
            category.CodeName.Should().Be(expectedCategoryCode);
            category.FullName.Should().Be(expectedCategoryName);
        }

        [TestCase(VacancyType.Apprenticeship, "101", "FW.101", "Software Developer")]
        [TestCase(VacancyType.Apprenticeship, null, "FW.UNKNOWN", "Unknown Framework")]
        [TestCase(VacancyType.Apprenticeship, "XXX", "FW.UNKNOWN", "Unknown Framework")]
        [TestCase(VacancyType.Traineeship, "101", "FW.INVALID", "Invalid Framework")]
        [TestCase(VacancyType.Traineeship, null, "FW.INVALID", "Invalid Framework")]
        public void ShouldGetSubCategoryForFramework(
            VacancyType vacancyType, string frameworkCodeName, string expectedSubCategoryCode, string expectedCategoryName)
        {
            // Arrange.
            var vacancySummary = new Fixture()
                .Build<VacancySummary>()
                .With(each => each.VacancyType, vacancyType)
                .With(each => each.TrainingType, TrainingType.Frameworks)
                .With(each => each.FrameworkCodeName, frameworkCodeName)
                .Create();

            // Act.
            var subCategory = vacancySummary.GetSubCategory(_categories);

            // Assert.
            subCategory.CodeName.Should().Be(expectedSubCategoryCode);
            subCategory.FullName.Should().Be(expectedCategoryName);
        }

        [TestCase(VacancyType.Apprenticeship, 2, "STDSEC.201", "Digital Industries > Software Developer")]
        [TestCase(VacancyType.Apprenticeship, null, "STDSEC.UNKNOWN", "Unknown Standard Sector")]
        [TestCase(VacancyType.Apprenticeship, -1, "STDSEC.UNKNOWN", "Unknown Standard Sector")]
        [TestCase(VacancyType.Traineeship, 2, "STDSEC.INVALID", "Invalid Standard Sector")]
        [TestCase(VacancyType.Traineeship, null, "STDSEC.INVALID", "Invalid Standard Sector")]
        public void ShouldGetSubCategoryForStandard(
            VacancyType vacancyType, int? standardId, string expectedSubCategoryCode, string expectedCategoryName)
        {
            // Arrange.
            var vacancySummary = new Fixture()
                .Build<VacancySummary>()
                .With(each => each.VacancyType, vacancyType)
                .With(each => each.TrainingType, TrainingType.Standards)
                .With(each => each.StandardId, standardId)
                .Create();

            // Act.
            var subCategory = vacancySummary.GetSubCategory(_categories);

            // Assert.
            subCategory.CodeName.Should().Be(expectedSubCategoryCode);
            subCategory.FullName.Should().Be(expectedCategoryName);
        }

        [TestCase(VacancyType.Apprenticeship, "ICT", "SEC.INVALID", "Invalid Sector")]
        [TestCase(VacancyType.Apprenticeship, null, "SEC.INVALID", "Invalid Sector")]
        [TestCase(VacancyType.Apprenticeship, "XXX", "SEC.INVALID", "Invalid Sector")]
        [TestCase(VacancyType.Traineeship, "ICT", "SEC.INVALID", "Invalid Sector")]
        [TestCase(VacancyType.Traineeship, null, "SEC.INVALID", "Invalid Sector")]
        public void ShouldGetSubCategoryForSector(
            VacancyType vacancyType, string sectorCodeName, string expectedSubCategoryCode, string expectedCategoryName)
        {
            // Arrange.
            var vacancySummary = new Fixture()
                .Build<VacancySummary>()
                .With(each => each.VacancyType, vacancyType)
                .With(each => each.TrainingType, TrainingType.Sectors)
                .With(each => each.SectorCodeName, sectorCodeName)
                .Create();

            // Act.
            var subCategory = vacancySummary.GetSubCategory(_categories);

            // Assert.
            subCategory.CodeName.Should().Be(expectedSubCategoryCode);
            subCategory.FullName.Should().Be(expectedCategoryName);
        }

        [TestCase(VacancyType.Apprenticeship, "101", "FW.101", "Software Developer")]
        [TestCase(VacancyType.Apprenticeship, null, "FW.UNKNOWN", "Unknown Framework")]
        [TestCase(VacancyType.Apprenticeship, "XXX", "FW.UNKNOWN", "Unknown Framework")]
        [TestCase(VacancyType.Traineeship, "101", "FW.INVALID", "Invalid Framework")]
        [TestCase(VacancyType.Traineeship, null, "FW.INVALID", "Invalid Framework")]
        public void ShouldGetSubCategoryForUnknown(
            VacancyType vacancyType, string frameworkCodeName, string expectedSubCategoryCode, string expectedCategoryName)
        {
            // Arrange.
            var vacancySummary = new Fixture()
                .Build<VacancySummary>()
                .With(each => each.VacancyType, vacancyType)
                .With(each => each.TrainingType, TrainingType.Unknown)
                .With(each => each.FrameworkCodeName, frameworkCodeName)
                .Create();

            // Act.
            var subCategory = vacancySummary.GetSubCategory(_categories);

            // Assert.
            subCategory.CodeName.Should().Be(expectedSubCategoryCode);
            subCategory.FullName.Should().Be(expectedCategoryName);
        }
    }
}