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
                new Category
                {
                    CodeName = "SSAT1.ICT",
                    FullName = "Information and Communication Technology",
                    SubCategories = new List<Category>
                    {
                        new Category
                        {
                            CodeName = "FW.101",
                            ParentCategoryCodeName = "SSAT1.ICT",
                            FullName = "Software Developer"
                        },
                        new Category
                        {
                            CodeName = "STDSEC.201",
                            ParentCategoryCodeName = "SSAT1.ICT",
                            FullName = "Digital Industries",
                            SubCategories = new List<Category>
                            {
                                new Category
                                {
                                    CodeName = "STD.1",
                                    ParentCategoryCodeName = "STDSEC.201",
                                    FullName = "Network Engineer"
                                },
                                new Category
                                {
                                    CodeName = "STD.2",
                                    ParentCategoryCodeName = "STDSEC.201",
                                    FullName = "Software Developer"
                                }
                            }
                        }
                    }
                }
            };
        }

        [TestCase(VacancyType.Apprenticeship, "101", "FW.101")]
        [TestCase(VacancyType.Apprenticeship, null, "FW.UNKNOWN")]
        [TestCase(VacancyType.Apprenticeship, "XXX", "FW.UNKNOWN")]
        [TestCase(VacancyType.Traineeship, "101", "FW.INVALID")]
        [TestCase(VacancyType.Traineeship, null, "FW.INVALID")]
        public void ShouldGetSubCategoryForFramework(
            VacancyType vacancyType, string frameworkCodeName, string expectedSubCategoryCode)
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
        }

        [TestCase(VacancyType.Apprenticeship, 2, "STDSEC.201")]
        [TestCase(VacancyType.Apprenticeship, null, "STDSEC.UNKNOWN")]
        [TestCase(VacancyType.Apprenticeship, -1, "STDSEC.UNKNOWN")]
        [TestCase(VacancyType.Traineeship, 2, "STDSEC.INVALID")]
        [TestCase(VacancyType.Traineeship, null, "STDSEC.INVALID")]
        public void ShouldGetSubCategoryForStandard(
            VacancyType vacancyType, int? standardId, string expectedSubCategoryCode)
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
        }

        [TestCase(VacancyType.Traineeship, "ICT", "SSAT1.ICT")]
        [TestCase(VacancyType.Traineeship, null, "SSAT1.UNKNOWN")]
        [TestCase(VacancyType.Traineeship, "XXX", "SSAT1.UNKNOWN")]
        [TestCase(VacancyType.Apprenticeship, "ICT", "SSAT1.INVALID")]
        [TestCase(VacancyType.Apprenticeship, null, "SSAT1.INVALID")]
        public void ShouldGetCategoryForSector(
            VacancyType vacancyType, string sectorCodeName, string expectedCategoryCode)
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
        }
    }
}