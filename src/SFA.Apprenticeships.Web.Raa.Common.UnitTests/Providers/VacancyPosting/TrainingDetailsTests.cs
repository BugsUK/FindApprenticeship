﻿namespace SFA.Apprenticeships.Web.Raa.Common.UnitTests.Providers.VacancyPosting
{
    using System.Collections.Generic;
    using System.Linq;
    using Domain.Entities.Locations;
    using Domain.Entities.Organisations;
    using Domain.Entities.Providers;
    using Domain.Entities.ReferenceData;
    using Domain.Entities.Vacancies.ProviderVacancies;
    using Domain.Entities.Vacancies.ProviderVacancies.Apprenticeship;
    using FluentAssertions;
    using Moq;
    using NUnit.Framework;
    using ViewModels.Vacancy;
    using Web.Common.Configuration;

    [TestFixture]
    public class TrainingDetailsTests : TestBase
    {
        private const string Ern = "ern";
        private const string ProviderSiteErn = "providerSiteErn";
        private const long VacancyReferenceNumber = 1;

        private readonly ApprenticeshipVacancy _existingApprenticeshipVacancy = new ApprenticeshipVacancy()
        {
            ProviderSiteEmployerLink = new ProviderSiteEmployerLink()
            {
                Employer = new Employer()
                {
                    Address = new Address()
                }
            }
        };

        private readonly ProviderSiteEmployerLink _providerSiteEmployerLink = new ProviderSiteEmployerLink
        {
            ProviderSiteErn = ProviderSiteErn,
            Description = "description",
            Employer = new Employer
            {
                Address = new Address()
            }
        };

        private readonly CommonWebConfiguration _webConfiguration = new CommonWebConfiguration
        {
            BlacklistedCategoryCodes = "00,99"
        };

        // NOTE: cannot use Fixture here as Category data structure is recursive.
        private readonly Category[] _categories = {
            new Category
            {
                CodeName = "00",
                FullName = "Blacklisted Sector - 00",
                SubCategories = new []
                {
                    new Category()
                }
            },
            new Category
            {
                CodeName = "02",
                FullName = "Sector - 02",
                SubCategories = new []
                {
                    new Category
                    {
                        CodeName = "02.01",
                        FullName = "Framework - 02.01"
                    },
                    new Category
                    {
                        CodeName = "02.02",
                        FullName = "Framework - 02.02"
                    }
                }
            },
            new Category
            {
                CodeName = "03",
                FullName = "Sector - 03",
                SubCategories = new []
                {
                    new Category
                    {
                        CodeName = "03.01",
                        FullName = "Framework - 03.01"
                    }
                }
            },
            new Category
            {
                CodeName = "42",
                FullName = "Sector with no frameworks - 99"
            },
            new Category
            {
                CodeName = "99",
                FullName = "Blacklisted Sector - 99",
                SubCategories = new []
                {
                    new Category()
                }

            }
        };

        [SetUp]
        public void SetUp()
        {
            MockVacancyPostingService.Setup(mock => mock.GetVacancy(It.IsAny<long>()))
                .Returns(_existingApprenticeshipVacancy);
            MockVacancyPostingService.Setup(mock => mock.CreateApprenticeshipVacancy(It.IsAny<ApprenticeshipVacancy>()))
                .Returns<ApprenticeshipVacancy>(v => v);
            MockVacancyPostingService.Setup(mock => mock.SaveApprenticeshipVacancy(It.IsAny<ApprenticeshipVacancy>()))
                .Returns<ApprenticeshipVacancy>(v => v);
            MockVacancyPostingService.Setup(mock => mock.ShallowSaveApprenticeshipVacancy(It.IsAny<ApprenticeshipVacancy>()))
                .Returns<ApprenticeshipVacancy>(v => v);
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
            MockProviderService.Setup(s => s.GetProviderSiteEmployerLink(ProviderSiteErn, Ern))
                .Returns(_providerSiteEmployerLink);

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

            MockMapper.Setup(m => m.Map<ApprenticeshipVacancy, TrainingDetailsViewModel>(It.IsAny<ApprenticeshipVacancy>()))
                .Returns((ApprenticeshipVacancy av) => new TrainingDetailsViewModel() { ApprenticeshipLevel = av.ApprenticeshipLevel });

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
                ApprenticeshipLevel = ApprenticeshipLevel.Unknown,
                TrainingType = TrainingType.Standards,
                StandardId = standardId
            };

            MockMapper.Setup(m => m.Map<ApprenticeshipVacancy, TrainingDetailsViewModel>(It.IsAny<ApprenticeshipVacancy>()))
                .Returns((ApprenticeshipVacancy av) =>
                {
                    return new TrainingDetailsViewModel() { ApprenticeshipLevel = av.ApprenticeshipLevel };
                });

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

            MockMapper.Setup(m => m.Map<ApprenticeshipVacancy, TrainingDetailsViewModel>(It.IsAny<ApprenticeshipVacancy>()))
                .Returns((ApprenticeshipVacancy av) => new TrainingDetailsViewModel() { FrameworkCodeName = av.FrameworkCodeName });
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

            MockMapper.Setup(m => m.Map<ApprenticeshipVacancy, TrainingDetailsViewModel>(It.IsAny<ApprenticeshipVacancy>()))
                .Returns((ApprenticeshipVacancy av) =>
                {
                    return new TrainingDetailsViewModel() { FrameworkCodeName = av.FrameworkCodeName };
                });

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

            MockMapper.Setup(m => m.Map<ApprenticeshipVacancy, TrainingDetailsViewModel>(It.IsAny<ApprenticeshipVacancy>()))
                .Returns((ApprenticeshipVacancy av) =>
                {
                    return new TrainingDetailsViewModel() { FrameworkCodeName = av.FrameworkCodeName, ApprenticeshipLevel = av.ApprenticeshipLevel };
                });

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
            MockMapper.Setup(m => m.Map<ApprenticeshipVacancy, TrainingDetailsViewModel>(It.IsAny<ApprenticeshipVacancy>())).Returns(new TrainingDetailsViewModel());
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
            MockMapper.Setup(m => m.Map<ApprenticeshipVacancy, TrainingDetailsViewModel>(It.IsAny<ApprenticeshipVacancy>())).Returns(new TrainingDetailsViewModel());
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
            MockMapper.Setup(m => m.Map<ApprenticeshipVacancy, TrainingDetailsViewModel>(It.IsAny<ApprenticeshipVacancy>())).Returns(new TrainingDetailsViewModel());
            var provider = GetVacancyPostingProvider();

            // Act.
            var viewModel = provider.GetTrainingDetailsViewModel(VacancyReferenceNumber);

            // Assert.
            viewModel.Should().NotBeNull();
            viewModel.ApprenticeshipLevel.Should().Be(ApprenticeshipLevel.Unknown);
        }
    }
}