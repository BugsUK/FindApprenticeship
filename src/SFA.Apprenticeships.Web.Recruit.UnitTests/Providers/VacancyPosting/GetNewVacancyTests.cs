namespace SFA.Apprenticeships.Web.Recruit.UnitTests.Providers.VacancyPosting
{
    using System;
    using System.Linq;
    using Domain.Entities.ReferenceData;
    using Domain.Entities.Users;
    using Domain.Entities.Vacancies.Apprenticeships;
    using FluentAssertions;
    using Moq;
    using NUnit.Framework;

    [TestFixture]
    public class GetNewVacancyTests : TestBase
    {
        protected const string ValidUserName = "john.doe@example.com";
        protected const string PreferredSiteErn = "42";

        private readonly ProviderUser _validUserProfile = new ProviderUser
        {
            Username = ValidUserName,
            PreferredSiteErn = PreferredSiteErn
        };

        private readonly Category[] _defaultCategories = new[]
        {
                new Category
                {
                    FullName = Guid.NewGuid().ToString()
                },
                new Category
                {
                    FullName = Guid.NewGuid().ToString()
                }
            };

        [SetUp]
        public void SetUp()
        {
            MockUserProfileService
                .Setup(mock => mock.GetProviderUser(ValidUserName))
                .Returns(_validUserProfile);

            MockReferenceDataService
                .Setup(mock => mock.GetCategories())
                .Returns(_defaultCategories);
        }

        [Test]
        public void ShouldGetProviderUserProfile()
        {
            // Arrange.
            var provider = GetProvider();

            // Act.
            var viewModel = provider.GetNewVacancy(ValidUserName);

            // Assert.
            MockUserProfileService.Verify(mock =>
                mock.GetProviderUser(ValidUserName), Times.Once);

            viewModel.Should().NotBeNull();
            viewModel.SiteUrn.Should().Be(PreferredSiteErn);
        }

        [Test]
        public void ShouldGetFrameworks()
        {
            // Arrange.
            var provider = GetProvider();

            // Act.
            var viewModel = provider.GetNewVacancy(ValidUserName);

            // Assert.
            viewModel.Should().NotBeNull();
            viewModel.Categories.Should().BeEquivalentTo(_defaultCategories.AsEnumerable());
        }

        [Test]
        public void ShouldDefaultApprenticeshipLevel()
        {
            // Arrange.
            var provider = GetProvider();

            // Act.
            var viewModel = provider.GetNewVacancy(ValidUserName);

            // Assert.
            viewModel.Should().NotBeNull();
            viewModel.ApprenticeshipLevel.Should().Be(ApprenticeshipLevel.Intermediate);
        }

        [Test]
        [Ignore]
        public void ShouldGetTrainingSites()
        {
            // Arrange.

            // Act.

            // Assert.
            Assert.Fail();
        }

        [Test]
        [Ignore]
        public void ShouldDefaultTrainingSite()
        {
            // Arrange.

            // Act.

            // Assert.
            Assert.Fail();
        }

        [Test]
        [Ignore]
        public void ShouldValidateVacancy()
        {
            // Arrange.

            // Act.

            // Assert.
            Assert.Fail();
        }
    }
}
