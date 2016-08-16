namespace SFA.Apprenticeships.Web.Raa.Common.UnitTests.Providers.ProviderProvider
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using Application.Interfaces.Employers;
    using Application.Interfaces.Generic;
    using Configuration;
    using Domain.Entities.Raa.Parties;
    using FluentAssertions;
    using Moq;
    using NUnit.Framework;
    using Ploeh.AutoFixture;
    using ViewModels.VacancyPosting;
    using Web.Common.ViewModels;

    [TestFixture]
    [Parallelizable]
    public class GetVacancyPartyViewModelsTests : TestBase
    {
        private const int PageSize = 10;

        [SetUp]
        public void SetUp2()
        {
            MockConfigurationService.Setup(mock => mock
                .Get<RecruitWebConfiguration>())
                .Returns(new RecruitWebConfiguration
                {
                    PageSize = PageSize
                });
        }

        [TestCase(0)]
        [TestCase(1)]
        [TestCase(PageSize)]
        public void ShoulReturnEmployersForProviderSite(int unactivatedEmployerCount)
        {
            // Arrange.
            const int pageNumber = 1;
            const int providerSiteId = 42;

            var employers = new Fixture()
                .CreateMany<Employer>(PageSize)
                .ToArray();

            var vacancyParties = BuildFakeVacancyParties(employers, unactivatedEmployerCount);

            var pageableVacancyParties = new Fixture()
                .Build<Pageable<VacancyParty>>()
                .With(each => each.Page, vacancyParties)
                .Create();

            Expression<Func<EmployerSearchRequest, bool>> matchingSearchRequest = it => it.ProviderSiteId == providerSiteId;

            MockProviderService.Setup(mock => mock
                .GetVacancyParties(It.Is(matchingSearchRequest), pageNumber, PageSize))
                .Returns(pageableVacancyParties);

            Expression<Func<IEnumerable<int>, bool>> matchingEmployerIds = it => true;

            MockEmployerService.Setup(mock => mock
                .GetEmployers(It.Is(matchingEmployerIds)))
                .Returns(employers);

            var provider = GetProviderProvider();

            // Act.
            var viewModel = provider.GetVacancyPartyViewModels(providerSiteId);

            // Assert.
            viewModel.Should().NotBeNull();

            viewModel.ProviderSiteId.Should().Be(providerSiteId);

            viewModel.EmployerResultsPage.Should().NotBeNull();
            viewModel.EmployerResultsPage.Page.Count().Should().Be(PageSize - unactivatedEmployerCount);
        }

        [TestCase(0)]
        [TestCase(1)]
        [TestCase(PageSize)]
        public void ShoulReturnEmployersForEmployerSearchViewModel(int unactivatedEmployerCount)
        {
            // Arrange.
            var searchViewModel = new EmployerSearchViewModel
            {
                FilterType = EmployerFilterType.Undefined,
                ProviderSiteId = 42,
                EmployerResultsPage = new PageableViewModel<EmployerResultViewModel>
                {
                    CurrentPage = 3
                }
            };

            var employers = new Fixture()
                .CreateMany<Employer>(PageSize)
                .ToArray();

            var vacancyParties = BuildFakeVacancyParties(employers, unactivatedEmployerCount);

            var pageableVacancyParties = new Fixture()
                .Build<Pageable<VacancyParty>>()
                .With(each => each.Page, vacancyParties)
                .Create();

            Expression<Func<EmployerSearchRequest, bool>> matchingSearchRequest = it => it.ProviderSiteId == searchViewModel.ProviderSiteId;

            MockProviderService.Setup(mock => mock
                .GetVacancyParties(It.Is(matchingSearchRequest), searchViewModel.EmployerResultsPage.CurrentPage, PageSize))
                .Returns(pageableVacancyParties);

            Expression<Func<IEnumerable<int>, bool>> matchingEmployerIds = it => true;

            MockEmployerService.Setup(mock => mock
                .GetEmployers(It.Is(matchingEmployerIds)))
                .Returns(employers);

            var provider = GetProviderProvider();

            // Act.
            var viewModel = provider.GetVacancyPartyViewModels(searchViewModel);

            // Assert.
            viewModel.Should().NotBeNull();

            viewModel.ProviderSiteId.Should().Be(searchViewModel.ProviderSiteId);

            viewModel.EmployerResultsPage.Should().NotBeNull();
            viewModel.EmployerResultsPage.Page.Count().Should().Be(PageSize - unactivatedEmployerCount);
        }

        private static IEnumerable<VacancyParty> BuildFakeVacancyParties(IReadOnlyList<Employer> employers, int unactivatedEmployerCount)
        {
            var vacancyParties = new Fixture()
                .CreateMany<VacancyParty>(PageSize)
                .ToArray();

            var random = new Random();

            // Assign each vacancy party a random employer.
            foreach (var vacancyParty in vacancyParties)
            {
                var employerIndex = random.Next(PageSize);

                vacancyParty.EmployerId = employers[employerIndex].EmployerId;
            }

            // Assign 'unactivated' employers to vacancy parties.
            for (var i = 0; i < unactivatedEmployerCount; i++)
            {
                vacancyParties[i].EmployerId = employers.Max(employer => employer.EmployerId) + 1;
            }

            return vacancyParties;
        }
    }
}
