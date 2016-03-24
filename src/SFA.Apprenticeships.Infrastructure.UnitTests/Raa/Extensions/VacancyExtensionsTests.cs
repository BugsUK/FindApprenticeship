namespace SFA.Apprenticeships.Infrastructure.UnitTests.Raa.Extensions
{
    using System;
    using System.Collections.Generic;
    using Domain.Entities.Raa.Vacancies;
    using Domain.Entities.ReferenceData;
    using Domain.Entities.Vacancies;
    using FluentAssertions;
    using Infrastructure.Raa.Extensions;
    using NUnit.Framework;
    using Ploeh.AutoFixture;
    using VacancyType = Domain.Entities.Raa.Vacancies.VacancyType;

    [TestFixture]
    public class VacancyExtensionsTests
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
                    FullName = "Sector Subject Area Tier 1: Information and Communication Technology",
                    SubCategories = new List<Category>
                    {
                        new Category
                        {
                            CodeName = "FW.101",
                            ParentCategoryCodeName = "SSAT1.ICT",
                            FullName = "Framework: Software Developer"
                        },
                        new Category
                        {
                            CodeName = "STDSEC.201",
                            ParentCategoryCodeName = "SSAT1.ICT",
                            FullName = "Standard sector: Digital Industries",
                            SubCategories = new List<Category>
                            {
                                new Category
                                {
                                    CodeName = "STD.1",
                                    ParentCategoryCodeName = "STDSEC.201",
                                    FullName = "Standard: Network Engineer"
                                },
                                new Category
                                {
                                    CodeName = "STD.2",
                                    ParentCategoryCodeName = "STDSEC.201",
                                    FullName = "Standard: Software Developer"
                                }
                            }
                        }
                    }
                }
            };
        }

        [TestCase(VacancyStatus.Unknown, VacancyStatuses.Unknown)]
        [TestCase(VacancyStatus.Draft, VacancyStatuses.Unavailable)]
        [TestCase(VacancyStatus.Submitted, VacancyStatuses.Unavailable)]
        [TestCase(VacancyStatus.Live, VacancyStatuses.Live)]
        [TestCase(VacancyStatus.ReservedForQA, VacancyStatuses.Unavailable)]
        [TestCase(VacancyStatus.Referred, VacancyStatuses.Unavailable)]
        [TestCase(VacancyStatus.Closed, VacancyStatuses.Expired)]
        public void ShouldMapVacancyStatus(VacancyStatus fromVacancyStatus, VacancyStatuses expectedVacancyStatus)
        {
            // Act.
            var actualVacancyStatus = fromVacancyStatus.GetVacancyStatuses();

            // Assert.
            actualVacancyStatus.Should().Be(expectedVacancyStatus);
        }

        [Test]
        public void ShouldThrowIfUnhandledVacancyStatus()
        {
            const VacancyStatus unhandledVacancyStatus = VacancyStatus.PostedInError;

            // Act.
            Action action = () => unhandledVacancyStatus.GetVacancyStatuses();

            // Assert.
            action.ShouldThrow<ArgumentException>();
        }

        [TestCase(VacancyType.Apprenticeship, "101", "FW.101")]
        [TestCase(VacancyType.Apprenticeship, null, "FW.UNKNOWN")]
        [TestCase(VacancyType.Apprenticeship, "XXX", "FW.UNKNOWN")]
        [TestCase(VacancyType.Traineeship, "101", "FW.INVALID")]
        [TestCase(VacancyType.Traineeship, null, "FW.INVALID")]
        public void ShouldGetPrefixedSubCategoryCodeForFramework(
            VacancyType vacancyType, string frameworkCodeName, string expectedSubCategoryCode)
        {
            // Arrange.
            var vacancySummary = new Fixture()
                .Build<Domain.Entities.Raa.Vacancies.VacancySummary>()
                .With(each => each.VacancyType, vacancyType)
                .With(each => each.TrainingType, TrainingType.Frameworks)
                .With(each => each.FrameworkCodeName, frameworkCodeName)
                .Create();

            // Act.
            var subCategoryCode = vacancySummary.GetSubCategoryCode(_categories);

            // Assert.
            subCategoryCode.Should().Be(expectedSubCategoryCode);
        }

        [TestCase(VacancyType.Apprenticeship, 2, "STDSEC.201")]
        [TestCase(VacancyType.Apprenticeship, null, "STDSEC.UNKNOWN")]
        [TestCase(VacancyType.Apprenticeship, -1, "STDSEC.UNKNOWN")]
        [TestCase(VacancyType.Traineeship, 2, "STDSEC.INVALID")]
        [TestCase(VacancyType.Traineeship, null, "STDSEC.INVALID")]
        public void ShouldGetPrefixedSubCategoryCodeForStandard(
            VacancyType vacancyType, int? standardId, string expectedSubCategoryCode)
        {
            // Arrange.
            var vacancySummary = new Fixture()
                .Build<Domain.Entities.Raa.Vacancies.VacancySummary>()
                .With(each => each.VacancyType, vacancyType)
                .With(each => each.TrainingType, TrainingType.Standards)
                .With(each => each.StandardId, standardId)
                .Create();

            // Act.
            var subCategoryCode = vacancySummary.GetSubCategoryCode(_categories);

            // Assert.
            subCategoryCode.Should().Be(expectedSubCategoryCode);
        }

        [TestCase(VacancyType.Traineeship, "ICT", "SSAT1.ICT")]
        [TestCase(VacancyType.Traineeship, null, "SSAT1.UNKNOWN")]
        [TestCase(VacancyType.Traineeship, "XXX", "SSAT1.UNKNOWN")]
        [TestCase(VacancyType.Apprenticeship, "ICT", "SSAT1.INVALID")]
        [TestCase(VacancyType.Apprenticeship, null, "SSAT1.INVALID")]
        public void ShouldGetPrefixedCategoryCodeForSector(
            VacancyType vacancyType, string sectorCodeName, string expectedCategoryCode)
        {
            // Arrange.
            var vacancySummary = new Fixture()
                .Build<Domain.Entities.Raa.Vacancies.VacancySummary>()
                .With(each => each.VacancyType, vacancyType)
                .With(each => each.TrainingType, TrainingType.Sectors)
                .With(each => each.SectorCodeName, sectorCodeName)
                .Create();

            // Act.
            var categoryCode = vacancySummary.GetCategoryCode(_categories);

            // Assert.
            categoryCode.Should().Be(expectedCategoryCode);
        }
    }
}
