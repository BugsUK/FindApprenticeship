namespace SFA.Apprenticeships.Web.Recruit.UnitTests.Providers.VacancyPosting
{
    using System;
    using System.Linq;
    using Common.Configuration;
    using Domain.Entities.ReferenceData;
    using Domain.Entities.Users;
    using Domain.Entities.Vacancies.Apprenticeships;
    using FluentAssertions;
    using Moq;
    using NUnit.Framework;

    [TestFixture]
    public class GetNewVacancyTests : TestBase
    {
        protected static readonly string ValidUserName = $"{Guid.NewGuid()}@example.com";
        protected static readonly string PreferredSiteUrn = Guid.NewGuid().ToString();
        protected static readonly string Ukprn = Guid.NewGuid().ToString();

        private readonly ProviderUser _userProfile = new ProviderUser
        {
            Username = ValidUserName,
            Ukprn = Ukprn,
            PreferredSiteErn = PreferredSiteUrn
        };

        private readonly WebConfiguration _webConfiguration = new WebConfiguration
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
                .Setup(mock => mock.Get<WebConfiguration>())
                .Returns(_webConfiguration);

            MockUserProfileService
                .Setup(mock => mock.GetProviderUser(ValidUserName))
                .Returns(_userProfile);

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
            var viewModel = provider.GetNewVacancyViewModel(ValidUserName);

            // Assert.
            MockUserProfileService.Verify(mock =>
                mock.GetProviderUser(ValidUserName), Times.Once);

            viewModel.Should().NotBeNull();
            viewModel.TrainingSiteErn.Should().Be(PreferredSiteUrn);
        }

        [Test]
        public void ShouldGetSectorsAndFrameworks()
        {
            // Arrange.
            var provider = GetProvider();

            // Act.
            var viewModel = provider.GetNewVacancyViewModel(ValidUserName);

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

            // Act.
            var viewModel = provider.GetNewVacancyViewModel(ValidUserName);

            // Assert.
            viewModel.Should().NotBeNull();
            viewModel.SectorsAndFrameworks.Should().NotBeNull();

            Assert.That(!viewModel.SectorsAndFrameworks.
                Any(sector => _webConfiguration.BlacklistedCategoryCodes.Contains(sector.CodeName)));
        }

        [Test]
        public void ShouldNotGetSectorsWithoutFrameworks()
        {
            // Arrange.
            var provider = GetProvider();

            // Act.
            var viewModel = provider.GetNewVacancyViewModel(ValidUserName);

            // Assert.
            viewModel.Should().NotBeNull();
            viewModel.SectorsAndFrameworks.Should().NotBeNull();
            Assert.That(viewModel.SectorsAndFrameworks.All(sector => sector.Frameworks?.Count > 0));
        }

        [Test]
        public void ShouldDefaultApprenticeshipLevel()
        {
            // Arrange.
            var provider = GetProvider();

            // Act.
            var viewModel = provider.GetNewVacancyViewModel(ValidUserName);

            // Assert.
            viewModel.Should().NotBeNull();
            viewModel.ApprenticeshipLevel.Should().Be(ApprenticeshipLevel.Intermediate);
        }
    }
}
