namespace SFA.Apprenticeships.Web.Recruit.UnitTests.Providers.VacancyPosting
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Common.Configuration;
    using Domain.Entities.Locations;
    using Domain.Entities.Organisations;
    using Domain.Entities.Providers;
    using Domain.Entities.ReferenceData;
    using Domain.Entities.Vacancies.Apprenticeships;
    using FluentAssertions;
    using Moq;
    using NUnit.Framework;

    [TestFixture]
    public class GetNewVacancyTests : TestBase
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
        }

        [Test]
        public void ShouldDefaultToPreferredSite()
        {
            // Arrange.
            var provider = GetProvider();

            // Act.
            var viewModel = provider.GetNewVacancyViewModel(Ukprn, ProviderSiteUrn, Ern);

            // Assert.
            MockProviderService.Verify(mock =>
                mock.GetProviderSiteEmployerLink(ProviderSiteUrn, Ern), Times.Once);

            viewModel.Should().NotBeNull();
            viewModel.ProviderSiteEmployerLink.ProviderSiteErn.Should().Be(ProviderSiteUrn);
        }

        [Test]
        public void ShouldGetSectorsAndFrameworks()
        {
            // Arrange.
            var provider = GetProvider();

            // Act.
            var viewModel = provider.GetNewVacancyViewModel(Ukprn, ProviderSiteUrn, Ern);

            // Assert.
            viewModel.Should().NotBeNull();
            viewModel.SectorsAndFrameworks.Should().NotBeNull();
            viewModel.SectorsAndFrameworks.Count.Should().BePositive();
        }

        [Test]
        public void ShouldNotGetBlacklistedSectorsAndFrameworks()
        {
            // Arrange.
            var provider = GetProvider();
            var blackListCodes = _webConfiguration.BlacklistedCategoryCodes.Split(',').Select(each => each.Trim()).ToArray();

            // Act.
            var viewModel = provider.GetNewVacancyViewModel(Ukprn, ProviderSiteUrn, Ern);

            // Assert.
            viewModel.Should().NotBeNull();
            viewModel.SectorsAndFrameworks.Should().NotBeNull();

            Assert.That(!viewModel.SectorsAndFrameworks.Any(sector => blackListCodes.Any(bc => sector.Value != string.Empty && bc.StartsWith(sector.Value))));
        }


        [Test]
        public void ShouldDefaultApprenticeshipLevel()
        {
            // Arrange.
            var provider = GetProvider();

            // Act.
            var viewModel = provider.GetNewVacancyViewModel(Ukprn, ProviderSiteUrn, Ern);

            // Assert.
            viewModel.Should().NotBeNull();
            viewModel.ApprenticeshipLevel.Should().Be(ApprenticeshipLevel.Unknown);
        }
    }
}
