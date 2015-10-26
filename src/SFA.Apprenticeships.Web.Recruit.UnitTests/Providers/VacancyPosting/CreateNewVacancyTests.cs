using System.Collections.Generic;
using SFA.Apprenticeships.Domain.Entities.Vacancies.Apprenticeships;
using SFA.Apprenticeships.Domain.Entities.Vacancies.ProviderVacancies.Apprenticeship;
using SFA.Apprenticeships.Web.Common.ViewModels.Locations;
using SFA.Apprenticeships.Web.Recruit.ViewModels.Frameworks;
using SFA.Apprenticeships.Web.Recruit.ViewModels.Provider;
using SFA.Apprenticeships.Web.Recruit.ViewModels.Vacancy;

namespace SFA.Apprenticeships.Web.Recruit.UnitTests.Providers.VacancyPosting
{
    using System;
    using Common.Configuration;
    using Domain.Entities.Locations;
    using Domain.Entities.Organisations;
    using Domain.Entities.Providers;
    using Domain.Entities.ReferenceData;
    using FluentAssertions;
    using Moq;
    using NUnit.Framework;

    [TestFixture]
    public class CreateNewVacancyTests : TestBase
    {
        protected static readonly string ValidUserName = $"{Guid.NewGuid()}@example.com";
        protected static readonly string ProviderSiteUrn = Guid.NewGuid().ToString();
        protected static readonly string Ern = Guid.NewGuid().ToString();
        protected static readonly string Ukprn = Guid.NewGuid().ToString();

        private static readonly Employer Employer = new Employer
        {
            Ern = Ern,
            Address = new Address
            {
                GeoPoint = new GeoPoint()
            }
        };

        private static readonly ProviderSiteEmployerLink ProviderSiteEmployerLink = new ProviderSiteEmployerLink
        {
            ProviderSiteErn = ProviderSiteUrn,
            Employer = Employer
        };

        private readonly CommonWebConfiguration _webConfiguration = new CommonWebConfiguration
        {
            BlacklistedCategoryCodes = "00,99"
        };

        // NOTE: cannot use Fixture here as Category data structure is recursive.
        private readonly Category[] _categories = new[]
        {
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

        private NewVacancyViewModel _validNewVacancyViewModelWithReferenceNumber = new NewVacancyViewModel()
        {
            VacancyReferenceNumber = 1,
            ApprenticeshipLevel = ApprenticeshipLevel.Advanced
        };

        private ApprenticeshipVacancy _existingApprenticeshipVacancy = new ApprenticeshipVacancy()
        {
            ProviderSiteEmployerLink = new ProviderSiteEmployerLink()
            {
                Employer = new Employer()
                {
                    Address = new Address()
                }
            }
        };

        private NewVacancyViewModel _validNewVacancyViewModelSansReferenceNumber = new NewVacancyViewModel
        {
            SectorsAndFrameworks = new List<SectorSelectItemViewModel>(),
            ProviderSiteEmployerLink = new ProviderSiteEmployerLinkViewModel()
            {
                Employer = new EmployerViewModel()
                {
                    Address = new AddressViewModel()
                }
            }
        };

        [SetUp]
        public void SetUp()
        {
            MockConfigurationService
                .Setup(mock => mock.Get<CommonWebConfiguration>())
                .Returns(_webConfiguration);

            MockProviderService
                .Setup(mock => mock.GetProviderSiteEmployerLink(ProviderSiteUrn, Ern))
                .Returns(ProviderSiteEmployerLink);

            MockReferenceDataService
                .Setup(mock => mock.GetCategories())
                .Returns(_categories);

            MockVacancyPostingService.Setup(mock => mock.GetVacancy(_validNewVacancyViewModelWithReferenceNumber.VacancyReferenceNumber.Value))
                .Returns(_existingApprenticeshipVacancy);
            MockVacancyPostingService.Setup(mock => mock.SaveApprenticeshipVacancy(It.IsAny<ApprenticeshipVacancy>()))
                .Returns(_existingApprenticeshipVacancy);
        }

        [Test]
        public void ShouldUpdateIfVacancyReferenceIsPresent()
        {
            // Arrange.
            var provider = GetProvider();

            // Act.
            var viewModel = provider.CreateVacancy(_validNewVacancyViewModelWithReferenceNumber);

            // Assert.
            MockVacancyPostingService.Verify(mock =>
                mock.GetVacancy(_validNewVacancyViewModelWithReferenceNumber.VacancyReferenceNumber.Value), Times.Once);
            MockVacancyPostingService.Verify(mock => mock.GetNextVacancyReferenceNumber(), Times.Never);
            MockVacancyPostingService.Verify(mock =>
                mock.SaveApprenticeshipVacancy(It.IsAny<ApprenticeshipVacancy>()), Times.Once);

            viewModel.VacancyReferenceNumber.Should().HaveValue();
        }

        [Test]
        public void ShouldCreateNewIfVacancyReferenceIsNotPresent()
        {
            // Arrange.
            var provider = GetProvider();

            // Act.
            var viewModel = provider.CreateVacancy(_validNewVacancyViewModelSansReferenceNumber);

            // Assert.
            MockVacancyPostingService.Verify(mock =>
                mock.GetVacancy(It.IsAny<long>()), Times.Never);
            MockVacancyPostingService.Verify(mock => mock.GetNextVacancyReferenceNumber(), Times.Once);
            MockVacancyPostingService.Verify(mock =>
                mock.SaveApprenticeshipVacancy(It.IsAny<ApprenticeshipVacancy>()), Times.Once);

            viewModel.VacancyReferenceNumber.Should().HaveValue();
        }

    }
}
