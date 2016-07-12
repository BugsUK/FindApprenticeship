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

    [TestFixture]
    public class GetVacancyPartyViewModelsTests
    {
        public class GetByProviderSiteId : TestBase
        {
            [Test]
            public void ShoulReturnSearchResultsPageForProviderSite()
            {
                // Arrange.
                const int pageSize = 10;
                const int pageNumber = 1;

                const int providerSiteId = 42;

                MockConfigurationService.Setup(mock => mock
                    .Get<RecruitWebConfiguration>())
                    .Returns(new RecruitWebConfiguration
                    {
                        PageSize = pageSize
                    });

                var employers = new Fixture()
                    .CreateMany<Employer>(pageSize)
                    .ToArray();

                var vacancyParties = new Fixture()
                    .CreateMany<VacancyParty>(pageSize)
                    .ToArray();

                var random = new Random();

                // Assign each vacancy party a random employer.
                foreach (var vacancyParty in vacancyParties)
                {
                    var employerIndex = random.Next(pageSize);

                    vacancyParty.EmployerId = employers[employerIndex].EmployerId;
                }

                /*
                var employerIds = Enumerable.Empty<int>();

                var vacancyPartyEmployerIds = vacancyParties
                    .Select(vacancyParty => vacancyParty.EmployerId)
                    .Distinct()
                    .OrderBy(employerId => employerId);
                */

                var pageableVacancyParties = new Fixture()
                    .Build<Pageable<VacancyParty>>()
                    .With(each => each.Page, vacancyParties)
                    .Create();

                Expression<Func<EmployerSearchRequest, bool>> matchingSearchRequest = it => it.ProviderSiteId == providerSiteId;

                MockProviderService.Setup(mock => mock
                    .GetVacancyParties(It.Is(matchingSearchRequest), pageNumber, pageSize))
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
                viewModel.EmployerResultsPage.Page.Count().Should().Be(pageSize);
            }
        }

        public class GetByEmployerSearchViewModel
        {
            [Test]
            public void ShouldBeAwesome()
            {
                // Arrange.

                // Act.

                // Assert.
                Assert.Fail();
            }
        }
    }
}
