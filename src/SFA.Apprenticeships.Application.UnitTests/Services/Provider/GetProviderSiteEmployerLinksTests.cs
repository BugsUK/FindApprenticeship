﻿namespace SFA.Apprenticeships.Application.UnitTests.Services.Provider
{
    using System.Collections.Generic;
    using System.Linq;
    using Domain.Entities.Raa.Parties;
    using Domain.Raa.Interfaces.Repositories;
    using FluentAssertions;
    using Interfaces.Employers;
    using Interfaces.Organisations;
    using Moq;
    using NUnit.Framework;
    using Ploeh.AutoFixture;

    [TestFixture]
    public class GetProviderSiteEmployerLinksTests
    {
        private const int ProviderSiteId = 654321;
        private const string EdsUrn1 = "100000";
        private const string EdsUrn2 = "200000";
        private const string EdsUrn3 = "300000";
        private const int EmployerId1 = 100000;
        private const int EmployerId2 = 200000;
        private const int EmployerId3 = 300000;
        private const int CurrentPage = 1;
        private const int PageSize = 10;

        private readonly EmployerSearchRequest _employerSearchRequest = new EmployerSearchRequest(ProviderSiteId);

        private Employer _employer1;
        private Employer _employer2;
        private Employer _employer3;

        private VacancyParty _providerSiteEmployerLink1;
        private VacancyParty _providerSiteEmployerLink2;
        private VacancyParty _providerSiteEmployerLink3;

        private List<Employer> _employersFromService;
        private List<VacancyParty> _fromService;
        private List<VacancyParty> _fromRepository;

        private Mock<IOrganisationService> _organisationService;
        private Mock<IEmployerService> _employerService;
        private Mock<IVacancyPartyReadRepository> _providerSiteEmployerLinkReadRepository;

        [TestFixtureSetUp]
        public void TestFixtureSetUp()
        {
            _employer1 = new Fixture().Build<Employer>().With(e => e.EdsUrn, EdsUrn1).With(e => e.EmployerId, EmployerId1).Create();
            _employer2 = new Fixture().Build<Employer>().With(e => e.EdsUrn, EdsUrn2).With(e => e.EmployerId, EmployerId2).Create();
            _employer3 = new Fixture().Build<Employer>().With(e => e.EdsUrn, EdsUrn3).With(e => e.EmployerId, EmployerId3).Create();

            _providerSiteEmployerLink1 =
                new Fixture().Build<VacancyParty>()
                    .With(l => l.ProviderSiteId, ProviderSiteId)
                    .With(l => l.EmployerId, EmployerId1)
                    .Create();
            _providerSiteEmployerLink2 =
                new Fixture().Build<VacancyParty>()
                    .With(l => l.ProviderSiteId, ProviderSiteId)
                    .With(l => l.EmployerId, EmployerId2)
                    .Create();

            _employersFromService = new List<Employer>
            {
                _employer1,
                _employer2,
                _employer3
            };

            _fromService = new List<VacancyParty>
            {
                _providerSiteEmployerLink1,
                _providerSiteEmployerLink2
            };
        }

        [SetUp]
        public void Setup()
        {
            _providerSiteEmployerLink3 =
                new Fixture().Build<VacancyParty>()
                    .With(l => l.ProviderSiteId, ProviderSiteId)
                    .With(l => l.EmployerId, EmployerId3)
                    .Create();

            _fromRepository = new List<VacancyParty>
            {
                _providerSiteEmployerLink3
            };

            _organisationService = new Mock<IOrganisationService>();
            _organisationService.Setup(r => r.GetVacancyParties(It.IsAny<int>())).Returns(_fromService);
            _employerService = new Mock<IEmployerService>();
            _employerService.Setup(r => r.GetEmployers(It.IsAny<IEnumerable<int>>())).Returns(_employersFromService);
            _providerSiteEmployerLinkReadRepository = new Mock<IVacancyPartyReadRepository>();
            _providerSiteEmployerLinkReadRepository.Setup(r => r.GetForProviderSite(ProviderSiteId)).Returns(_fromRepository);
        }

        [Test]
        public void FromService()
        {
            var service = new ProviderServiceBuilder().With(_organisationService.Object).With(_employerService.Object).Build();

            var linksPage = service.GetVacancyParties(_employerSearchRequest, CurrentPage, PageSize);

            linksPage.Should().NotBeNull();
            linksPage.Page.Count().Should().Be(_fromService.Count);
            linksPage.Page.ShouldBeEquivalentTo(_fromService);
            linksPage.ResultsCount.Should().Be(_fromService.Count);
            linksPage.TotalNumberOfPages.Should().Be(1);
        }

        [Test]
        public void FromServiceAndRepository()
        {
            var service = new ProviderServiceBuilder().With(_organisationService.Object).With(_employerService.Object).With(_providerSiteEmployerLinkReadRepository.Object).Build();

            var linksPage = service.GetVacancyParties(_employerSearchRequest, CurrentPage, PageSize);

            linksPage.Should().NotBeNull();
            linksPage.Page.Count().Should().Be(_fromService.Count + _fromRepository.Count);
            linksPage.Page.ShouldBeEquivalentTo(_fromService.Union(_fromRepository));
            linksPage.ResultsCount.Should().Be(_fromService.Count + _fromRepository.Count);
            linksPage.TotalNumberOfPages.Should().Be(1);
        }

        [Test]
        public void FromServiceAndRepositoryRemovingDuplicatesRepositoryMaster()
        {
            var duplicateEmployerLink =
                new Fixture().Build<VacancyParty>()
                    .With(l => l.ProviderSiteId, ProviderSiteId)
                    .With(l => l.EmployerId, EmployerId2)
                    .Create();
            var fromRepository = new List<VacancyParty> {duplicateEmployerLink};
            _providerSiteEmployerLinkReadRepository.Setup(r => r.GetForProviderSite(ProviderSiteId)).Returns(fromRepository);
            var service = new ProviderServiceBuilder().With(_organisationService.Object).With(_employerService.Object).With(_providerSiteEmployerLinkReadRepository.Object).Build();

            var linksPage = service.GetVacancyParties(_employerSearchRequest, CurrentPage, PageSize);

            //Results from repository should superceed those from service
            var expectedResults = new List<VacancyParty> { _providerSiteEmployerLink1, duplicateEmployerLink };
            linksPage.Should().NotBeNull();
            linksPage.Page.Count().Should().Be(expectedResults.Count);
            linksPage.Page.ShouldBeEquivalentTo(expectedResults);
            linksPage.ResultsCount.Should().Be(expectedResults.Count);
            linksPage.TotalNumberOfPages.Should().Be(1);
        }

        [Test]
        public void ErnRepositorySearch()
        {
            var fromRepository = new List<VacancyParty> { _providerSiteEmployerLink1, _providerSiteEmployerLink2, _providerSiteEmployerLink3 };
            _providerSiteEmployerLinkReadRepository.Setup(r => r.GetForProviderSite(ProviderSiteId)).Returns(fromRepository);
            var service = new ProviderServiceBuilder().With(_employerService.Object).With(_providerSiteEmployerLinkReadRepository.Object).Build();
            var employerSearchRequest = new EmployerSearchRequest(ProviderSiteId, EdsUrn2);

            var linksPage = service.GetVacancyParties(employerSearchRequest, CurrentPage, PageSize);

            var expectedResults = new List<VacancyParty> { _providerSiteEmployerLink2 };
            linksPage.Should().NotBeNull();
            linksPage.Page.Count().Should().Be(expectedResults.Count);
            linksPage.Page.ShouldBeEquivalentTo(expectedResults);
            linksPage.ResultsCount.Should().Be(expectedResults.Count);
            linksPage.TotalNumberOfPages.Should().Be(1);
        }

        [Test]
        public void NameRepositorySearch()
        {
            var fromRepository = new List<VacancyParty> { _providerSiteEmployerLink1, _providerSiteEmployerLink2, _providerSiteEmployerLink3 };
            _providerSiteEmployerLinkReadRepository.Setup(r => r.GetForProviderSite(ProviderSiteId)).Returns(fromRepository);
            var service = new ProviderServiceBuilder().With(_employerService.Object).With(_providerSiteEmployerLinkReadRepository.Object).Build();
            var employerSearchRequest = new EmployerSearchRequest(ProviderSiteId, _employer3.Name.Substring(0, 10), null);

            var linksPage = service.GetVacancyParties(employerSearchRequest, CurrentPage, PageSize);

            var expectedResults = new List<VacancyParty> { _providerSiteEmployerLink3 };
            linksPage.Should().NotBeNull();
            linksPage.Page.Count().Should().Be(expectedResults.Count);
            linksPage.Page.ShouldBeEquivalentTo(expectedResults);
            linksPage.ResultsCount.Should().Be(expectedResults.Count);
            linksPage.TotalNumberOfPages.Should().Be(1);
        }

        [Test]
        public void NameAndPostCodeRepositorySearch()
        {
            var fromRepository = new List<VacancyParty> { _providerSiteEmployerLink1, _providerSiteEmployerLink2, _providerSiteEmployerLink3 };
            _providerSiteEmployerLinkReadRepository.Setup(r => r.GetForProviderSite(ProviderSiteId)).Returns(fromRepository);
            var service = new ProviderServiceBuilder().With(_employerService.Object).With(_providerSiteEmployerLinkReadRepository.Object).Build();
            var employerSearchRequest = new EmployerSearchRequest(ProviderSiteId, _employer1.Name.Substring(0, 10), _employer1.Address.Postcode.Substring(0, 10));

            var linksPage = service.GetVacancyParties(employerSearchRequest, CurrentPage, PageSize);

            var expectedResults = new List<VacancyParty> { _providerSiteEmployerLink1 };
            linksPage.Should().NotBeNull();
            linksPage.Page.Count().Should().Be(expectedResults.Count);
            linksPage.Page.ShouldBeEquivalentTo(expectedResults);
            linksPage.ResultsCount.Should().Be(expectedResults.Count);
            linksPage.TotalNumberOfPages.Should().Be(1);
        }

        [Test]
        public void NameAndLocationRepositorySearch()
        {
            var fromRepository = new List<VacancyParty> { _providerSiteEmployerLink1, _providerSiteEmployerLink2, _providerSiteEmployerLink3 };
            _providerSiteEmployerLinkReadRepository.Setup(r => r.GetForProviderSite(ProviderSiteId)).Returns(fromRepository);
            var service = new ProviderServiceBuilder().With(_employerService.Object).With(_providerSiteEmployerLinkReadRepository.Object).Build();
            var employerSearchRequest = new EmployerSearchRequest(ProviderSiteId, _employer1.Name.Substring(0, 10), _employer1.Address.AddressLine4.Substring(0, 15));

            var linksPage = service.GetVacancyParties(employerSearchRequest, CurrentPage, PageSize);

            var expectedResults = new List<VacancyParty> { _providerSiteEmployerLink1 };
            linksPage.Should().NotBeNull();
            linksPage.Page.Count().Should().Be(expectedResults.Count);
            linksPage.Page.ShouldBeEquivalentTo(expectedResults);
            linksPage.ResultsCount.Should().Be(expectedResults.Count);
            linksPage.TotalNumberOfPages.Should().Be(1);
        }

        [Test]
        public void PostCodeRepositorySearch()
        {
            var fromRepository = new List<VacancyParty> { _providerSiteEmployerLink1, _providerSiteEmployerLink2, _providerSiteEmployerLink3 };
            _providerSiteEmployerLinkReadRepository.Setup(r => r.GetForProviderSite(ProviderSiteId)).Returns(fromRepository);
            var service = new ProviderServiceBuilder().With(_employerService.Object).With(_providerSiteEmployerLinkReadRepository.Object).Build();
            var employerSearchRequest = new EmployerSearchRequest(ProviderSiteId, null, _employer3.Address.Postcode.Substring(0, 10));

            var linksPage = service.GetVacancyParties(employerSearchRequest, CurrentPage, PageSize);

            var expectedResults = new List<VacancyParty> { _providerSiteEmployerLink3 };
            linksPage.Should().NotBeNull();
            linksPage.Page.Count().Should().Be(expectedResults.Count);
            linksPage.Page.ShouldBeEquivalentTo(expectedResults);
            linksPage.ResultsCount.Should().Be(expectedResults.Count);
            linksPage.TotalNumberOfPages.Should().Be(1);
        }

        [Test]
        public void LocationRepositorySearch()
        {
            var fromRepository = new List<VacancyParty> { _providerSiteEmployerLink1, _providerSiteEmployerLink2, _providerSiteEmployerLink3 };
            _providerSiteEmployerLinkReadRepository.Setup(r => r.GetForProviderSite(ProviderSiteId)).Returns(fromRepository);
            var service = new ProviderServiceBuilder().With(_employerService.Object).With(_providerSiteEmployerLinkReadRepository.Object).Build();
            var employerSearchRequest = new EmployerSearchRequest(ProviderSiteId, null, _employer3.Address.AddressLine4.Substring(0, 15));

            var linksPage = service.GetVacancyParties(employerSearchRequest, CurrentPage, PageSize);

            var expectedResults = new List<VacancyParty> { _providerSiteEmployerLink3 };
            linksPage.Should().NotBeNull();
            linksPage.Page.Count().Should().Be(expectedResults.Count);
            linksPage.Page.ShouldBeEquivalentTo(expectedResults);
            linksPage.ResultsCount.Should().Be(expectedResults.Count);
            linksPage.TotalNumberOfPages.Should().Be(1);
        }
    }
}