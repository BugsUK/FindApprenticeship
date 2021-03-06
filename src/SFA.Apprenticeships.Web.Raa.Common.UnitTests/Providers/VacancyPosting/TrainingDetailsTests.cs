﻿namespace SFA.Apprenticeships.Web.Raa.Common.UnitTests.Providers.VacancyPosting
{
    using System.Collections.Generic;
    using System.Linq;
    using Domain.Entities.Raa.Parties;
    using Domain.Entities.Raa.Vacancies;
    using Domain.Entities.ReferenceData;
    using FluentAssertions;
    using Moq;
    using NUnit.Framework;
    using ViewModels.Vacancy;
    using Web.Common.Configuration;

    [TestFixture]
    [Parallelizable]
    public class TrainingDetailsTests : TestBase
    {
        private const string EdsUrn = "112";
        private const int EmployerId = 1;
        private const int ProviderSiteId = 2;
        private const int VacancyReferenceNumber = 1;

        private readonly Vacancy _existingVacancy = new Vacancy()
        {
            VacancyOwnerRelationshipId = 42
        };

        private readonly VacancyOwnerRelationship _vacancyOwnerRelationship = new VacancyOwnerRelationship
        {
            ProviderSiteId = ProviderSiteId,
            EmployerDescription = "description",
            EmployerId = EmployerId,
        };

        private readonly CommonWebConfiguration _webConfiguration = new CommonWebConfiguration
        {
            BlacklistedCategoryCodes = "00,99"
        };

        // NOTE: cannot use Fixture here as Category data structure is recursive.
        private readonly Category[] _categories =
        {
            new Category(0, "00", "Blacklisted Sector - 00", CategoryType.SectorSubjectAreaTier1, CategoryStatus.Active),
            new Category(2, "02", "Sector - 02", CategoryType.SectorSubjectAreaTier1, CategoryStatus.Active, new List<Category>
                {
                    new Category(1, "02.01", "Framework - 02.01", CategoryType.Framework, CategoryStatus.Active),
                    new Category(2, "02.02", "Framework - 02.02", CategoryType.Framework, CategoryStatus.Active)
                }
            ),
            new Category(3, "03", "Sector - 03", CategoryType.SectorSubjectAreaTier1, CategoryStatus.Active, new List<Category>
                {
                    new Category(1, "03.01", "Framework - 03.01", CategoryType.Framework, CategoryStatus.Active)
                }
            ),
            new Category(42, "42", "Sector with no frameworks - 99", CategoryType.SectorSubjectAreaTier1, CategoryStatus.Active
            ),
            new Category(99, "99", "Blacklisted Sector - 99", CategoryType.SectorSubjectAreaTier1, CategoryStatus.Active, new List<Category>
                {
                    Category.EmptyFramework
                }
            )
        };

        [SetUp]
        public void SetUp()
        {
            MockVacancyPostingService.Setup(mock => mock.GetVacancyByReferenceNumber(It.IsAny<int>()))
                .Returns(_existingVacancy);
            MockVacancyPostingService.Setup(mock => mock.CreateVacancy(It.IsAny<Vacancy>()))
                .Returns<Vacancy>(v => v);
            MockReferenceDataService.Setup(mock => mock.GetSectors())
                .Returns(new List<Sector>
                {
                    new Sector
                    {
                        Id = 1,
                        Standards =
                            new List<Standard>
                            {
                                new Standard {Id = 1, ApprenticeshipSectorId = 1, ApprenticeshipLevel = ApprenticeshipLevel.Intermediate},
                                new Standard {Id = 2, ApprenticeshipSectorId = 1, ApprenticeshipLevel = ApprenticeshipLevel.Advanced},
                                new Standard {Id = 3, ApprenticeshipSectorId = 1, ApprenticeshipLevel = ApprenticeshipLevel.Higher},
                                new Standard {Id = 4, ApprenticeshipSectorId = 1, ApprenticeshipLevel = ApprenticeshipLevel.FoundationDegree},
                                new Standard {Id = 5, ApprenticeshipSectorId = 1, ApprenticeshipLevel = ApprenticeshipLevel.Degree},
                                new Standard {Id = 6, ApprenticeshipSectorId = 1, ApprenticeshipLevel = ApprenticeshipLevel.Masters}
                            }
                    }
                });
            MockProviderService.Setup(s => s.GetVacancyOwnerRelationship(ProviderSiteId, EdsUrn))
                .Returns(_vacancyOwnerRelationship);

            MockConfigurationService
                .Setup(mock => mock.Get<CommonWebConfiguration>())
                .Returns(_webConfiguration);

            MockReferenceDataService
                .Setup(mock => mock.GetCategories())
                .Returns(_categories);
        }

        [TestCase(1, ApprenticeshipLevel.Intermediate)]
        [TestCase(2, ApprenticeshipLevel.Advanced)]
        [TestCase(3, ApprenticeshipLevel.Higher)]
        [TestCase(4, ApprenticeshipLevel.FoundationDegree)]
        [TestCase(5, ApprenticeshipLevel.Degree)]
        [TestCase(6, ApprenticeshipLevel.Masters)]
        public void ShouldUpdateApprenticeshipLevelIfTrainingTypeStandard(int standardId, ApprenticeshipLevel expectedApprenticeshipLevel)
        {
            // Arrange.
            var trainingDetailsViewModel = new TrainingDetailsViewModel
            {
                VacancyReferenceNumber = VacancyReferenceNumber,
                ApprenticeshipLevel = ApprenticeshipLevel.Unknown,
                TrainingType = TrainingType.Standards,
                StandardId = standardId
            };

            MockVacancyPostingService.Setup(s => s.UpdateVacancy(It.IsAny<Vacancy>())).Returns<Vacancy>(v => v);
            MockMapper.Setup(m => m.Map<Vacancy, TrainingDetailsViewModel>(It.IsAny<Vacancy>()))
                .Returns((Vacancy av) => new TrainingDetailsViewModel() { ApprenticeshipLevel = av.ApprenticeshipLevel });

            var provider = GetVacancyPostingProvider();

            // Act.
            var viewModel = provider.UpdateVacancy(trainingDetailsViewModel);

            // Assert.
            viewModel.ApprenticeshipLevel.Should().Be(expectedApprenticeshipLevel);
        }

        [TestCase(1, ApprenticeshipLevel.Intermediate)]
        [TestCase(2, ApprenticeshipLevel.Advanced)]
        [TestCase(3, ApprenticeshipLevel.Higher)]
        [TestCase(4, ApprenticeshipLevel.FoundationDegree)]
        [TestCase(5, ApprenticeshipLevel.Degree)]
        [TestCase(6, ApprenticeshipLevel.Masters)]
        public void ShouldCreateApprenticeshipLevelIfTrainingTypeStandard(int standardId, ApprenticeshipLevel expectedApprenticeshipLevel)
        {
            // Arrange.
            var trainingDetailsViewModel = new TrainingDetailsViewModel
            {
                VacancyReferenceNumber = VacancyReferenceNumber,
                ApprenticeshipLevel = expectedApprenticeshipLevel,
                TrainingType = TrainingType.Standards,
                StandardId = standardId
            };

            MockVacancyPostingService.Setup(s => s.UpdateVacancy(It.IsAny<Vacancy>())).Returns<Vacancy>(v => v);
            MockMapper.Setup(m => m.Map<Vacancy, TrainingDetailsViewModel>(It.IsAny<Vacancy>()))
                .Returns((Vacancy av) => new TrainingDetailsViewModel() { ApprenticeshipLevel = av.ApprenticeshipLevel });

            var provider = GetVacancyPostingProvider();

            // Act.
            var viewModel = provider.UpdateVacancy(trainingDetailsViewModel);

            // Assert.
            viewModel.ApprenticeshipLevel.Should().Be(expectedApprenticeshipLevel);
        }

        [Test]
        public void ShouldUpdateNullFrameworkCodeIfTrainingTypeStandard()
        {
            // Arrange.
            var trainingDetailsViewModel = new TrainingDetailsViewModel
            {
                VacancyReferenceNumber = VacancyReferenceNumber,
                ApprenticeshipLevel = ApprenticeshipLevel.Unknown,
                TrainingType = TrainingType.Standards,
                StandardId = 1,
                FrameworkCodeName = "ShouldBeNulled"
            };

            MockVacancyPostingService.Setup(s => s.UpdateVacancy(It.IsAny<Vacancy>())).Returns<Vacancy>(v => v);
            MockMapper.Setup(m => m.Map<Vacancy, TrainingDetailsViewModel>(It.IsAny<Vacancy>()))
                .Returns((Vacancy av) => new TrainingDetailsViewModel() { FrameworkCodeName = av.FrameworkCodeName });
            var provider = GetVacancyPostingProvider();

            // Act.
            var viewModel = provider.UpdateVacancy(trainingDetailsViewModel);

            // Assert.
            viewModel.FrameworkCodeName.Should().BeNullOrEmpty();
        }

        [Test]
        public void ShouldCreateNullFrameworkCodeIfTrainingTypeStandard()
        {
            // Arrange.
            var trainingDetailsViewModel = new TrainingDetailsViewModel
            {
                VacancyReferenceNumber = VacancyReferenceNumber,
                ApprenticeshipLevel = ApprenticeshipLevel.Unknown,
                TrainingType = TrainingType.Standards,
                StandardId = 1,
                FrameworkCodeName = "ShouldBeNulled"
            };

            MockVacancyPostingService.Setup(s => s.UpdateVacancy(It.IsAny<Vacancy>())).Returns<Vacancy>(v => v);
            MockMapper.Setup(m => m.Map<Vacancy, TrainingDetailsViewModel>(It.IsAny<Vacancy>()))
                .Returns((Vacancy av) => new TrainingDetailsViewModel() { FrameworkCodeName = av.FrameworkCodeName });


            var provider = GetVacancyPostingProvider();

            // Act.
            var viewModel = provider.UpdateVacancy(trainingDetailsViewModel);

            // Assert.
            viewModel.FrameworkCodeName.Should().BeNullOrEmpty();
        }

        [Test]
        public void ShouldFillApprenticeshipLevelIfTrainingTypeIsStandardAndApprenticeshipLevelIsUnknown()
        {
            // Arrange.
            var trainingDetailsViewModel = new TrainingDetailsViewModel
            {
                VacancyReferenceNumber = VacancyReferenceNumber,
                ApprenticeshipLevel = ApprenticeshipLevel.Unknown,
                TrainingType = TrainingType.Standards,
                StandardId = 1
            };

            MockVacancyPostingService.Setup(s => s.UpdateVacancy(It.IsAny<Vacancy>())).Returns<Vacancy>(v => v);
            MockMapper.Setup(m => m.Map<Vacancy, TrainingDetailsViewModel>(It.IsAny<Vacancy>()))
                .Returns((Vacancy av) => new TrainingDetailsViewModel() { FrameworkCodeName = av.FrameworkCodeName, ApprenticeshipLevel = av.ApprenticeshipLevel });

            var provider = GetVacancyPostingProvider();

            // Act.
            var viewModel = provider.UpdateVacancy(trainingDetailsViewModel);

            // Assert.
            viewModel.ApprenticeshipLevel.Should().NotBe(ApprenticeshipLevel.Unknown);
        }

        [Test]
        public void ShouldGetSectorsAndFrameworks()
        {
            // Arrange.
            MockMapper.Setup(m => m.Map<Vacancy, TrainingDetailsViewModel>(It.IsAny<Vacancy>())).Returns(new TrainingDetailsViewModel());
            var provider = GetVacancyPostingProvider();

            // Act.
            var viewModel = provider.GetTrainingDetailsViewModel(VacancyReferenceNumber);

            // Assert.
            viewModel.Should().NotBeNull();
            viewModel.SectorsAndFrameworks.Should().NotBeNull();
            viewModel.SectorsAndFrameworks.Count.Should().BePositive();
        }

        [Test]
        public void ShouldNotGetBlacklistedSectorsAndFrameworks()
        {
            // Arrange.
            MockMapper.Setup(m => m.Map<Vacancy, TrainingDetailsViewModel>(It.IsAny<Vacancy>())).Returns(new TrainingDetailsViewModel());
            var provider = GetVacancyPostingProvider();
            var blackListCodes = _webConfiguration.BlacklistedCategoryCodes.Split(',').Select(each => each.Trim()).ToArray();

            // Act.
            var viewModel = provider.GetTrainingDetailsViewModel(VacancyReferenceNumber);

            // Assert.
            viewModel.Should().NotBeNull();
            viewModel.SectorsAndFrameworks.Should().NotBeNull();

            Assert.That(!viewModel.SectorsAndFrameworks.Any(sector => blackListCodes.Any(bc => sector.Value != string.Empty && bc.StartsWith(sector.Value))));
        }

        [Test]
        public void ShouldDefaultApprenticeshipLevel()
        {
            // Arrange.
            MockMapper.Setup(m => m.Map<Vacancy, TrainingDetailsViewModel>(It.IsAny<Vacancy>())).Returns(new TrainingDetailsViewModel());
            var provider = GetVacancyPostingProvider();

            // Act.
            var viewModel = provider.GetTrainingDetailsViewModel(VacancyReferenceNumber);

            // Assert.
            viewModel.Should().NotBeNull();
            viewModel.ApprenticeshipLevel.Should().Be(ApprenticeshipLevel.Unknown);
        }

        [Test]
        public void ShouldSetTrainingTypeIfTraineeship()
        {
            // Arrange.
            MockMapper.Setup(m => m.Map<Vacancy, TrainingDetailsViewModel>(It.IsAny<Vacancy>())).Returns(new TrainingDetailsViewModel
            {
                VacancyType = VacancyType.Traineeship
            });
            var provider = GetVacancyPostingProvider();

            // Act.
            var viewModel = provider.GetTrainingDetailsViewModel(VacancyReferenceNumber);

            // Assert.
            viewModel.Should().NotBeNull();
            viewModel.TrainingType.Should().Be(TrainingType.Sectors);
        }
    }
}