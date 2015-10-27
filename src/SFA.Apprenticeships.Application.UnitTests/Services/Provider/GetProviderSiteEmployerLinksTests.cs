namespace SFA.Apprenticeships.Application.UnitTests.Services.Provider
{
    using System.Collections.Generic;
    using System.Linq;
    using Domain.Entities.Organisations;
    using Domain.Entities.Providers;
    using Domain.Interfaces.Repositories;
    using FluentAssertions;
    using Interfaces.Employers;
    using Interfaces.Organisations;
    using Moq;
    using NUnit.Framework;
    using Ploeh.AutoFixture;

    [TestFixture]
    public class GetProviderSiteEmployerLinksTests
    {
        private const string ProviderSiteErn = "654321";
        private const string Ern1 = "100000";
        private const string Ern2 = "200000";
        private const string Ern3 = "300000";
        private const int CurrentPage = 1;
        private const int PageSize = 10;

        private readonly EmployerSearchRequest _employerSearchRequest = new EmployerSearchRequest(ProviderSiteErn);

        private Employer _employer1;
        private Employer _employer2;
        private Employer _employer3;

        private ProviderSiteEmployerLink _providerSiteEmployerLink1;
        private ProviderSiteEmployerLink _providerSiteEmployerLink2;
        private ProviderSiteEmployerLink _providerSiteEmployerLink3;

        private List<ProviderSiteEmployerLink> _fromService;
        private List<ProviderSiteEmployerLink> _fromRepository;

        private Mock<IOrganisationService> _organisationService;
        private Mock<IProviderSiteEmployerLinkReadRepository> _providerSiteEmployerLinkReadRepository;

        [TestFixtureSetUp]
        public void TestFixtureSetUp()
        {
            _employer1 = new Fixture().Build<Employer>().With(e => e.Ern, Ern1).Create();
            _employer2 = new Fixture().Build<Employer>().With(e => e.Ern, Ern2).Create();

            _providerSiteEmployerLink1 =
                new Fixture().Build<ProviderSiteEmployerLink>()
                    .With(l => l.ProviderSiteErn, ProviderSiteErn)
                    .With(l => l.Employer, _employer1)
                    .Create();
            _providerSiteEmployerLink2 =
                new Fixture().Build<ProviderSiteEmployerLink>()
                    .With(l => l.ProviderSiteErn, ProviderSiteErn)
                    .With(l => l.Employer, _employer2)
                    .Create();

            _fromService = new List<ProviderSiteEmployerLink>
            {
                _providerSiteEmployerLink1,
                _providerSiteEmployerLink2
            };
        }

        [SetUp]
        public void Setup()
        {
            _employer3 = new Fixture().Build<Employer>().With(e => e.Ern, Ern3).Create();

            _providerSiteEmployerLink3 =
                new Fixture().Build<ProviderSiteEmployerLink>()
                    .With(l => l.ProviderSiteErn, ProviderSiteErn)
                    .With(l => l.Employer, _employer3)
                    .Create();

            _fromRepository = new List<ProviderSiteEmployerLink>
            {
                _providerSiteEmployerLink3
            };

            _organisationService = new Mock<IOrganisationService>();
            _organisationService.Setup(r => r.GetProviderSiteEmployerLinks(It.IsAny<EmployerSearchRequest>())).Returns(_fromService);
            _providerSiteEmployerLinkReadRepository = new Mock<IProviderSiteEmployerLinkReadRepository>();
            _providerSiteEmployerLinkReadRepository.Setup(r => r.GetForProviderSite(ProviderSiteErn)).Returns(_fromRepository);
        }

        [Test]
        public void FromService()
        {
            var service = new ProviderServiceBuilder().With(_organisationService.Object).Build();

            var linksPage = service.GetProviderSiteEmployerLinks(_employerSearchRequest, CurrentPage, PageSize);

            linksPage.Should().NotBeNull();
            linksPage.Page.Count().Should().Be(_fromService.Count);
            linksPage.Page.ShouldBeEquivalentTo(_fromService);
            linksPage.ResultsCount.Should().Be(_fromService.Count);
            linksPage.TotalNumberOfPages.Should().Be(1);
        }

        [Test]
        public void FromServiceAndRepository()
        {
            var service = new ProviderServiceBuilder().With(_organisationService.Object).With(_providerSiteEmployerLinkReadRepository.Object).Build();

            var linksPage = service.GetProviderSiteEmployerLinks(_employerSearchRequest, CurrentPage, PageSize);

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
                new Fixture().Build<ProviderSiteEmployerLink>()
                    .With(l => l.ProviderSiteErn, ProviderSiteErn)
                    .With(l => l.Employer, _employer2)
                    .Create();
            var fromRepository = new List<ProviderSiteEmployerLink> {duplicateEmployerLink};
            _providerSiteEmployerLinkReadRepository.Setup(r => r.GetForProviderSite(ProviderSiteErn)).Returns(fromRepository);
            var service = new ProviderServiceBuilder().With(_organisationService.Object).With(_providerSiteEmployerLinkReadRepository.Object).Build();

            var linksPage = service.GetProviderSiteEmployerLinks(_employerSearchRequest, CurrentPage, PageSize);

            //Results from repository should superceed those from service
            var expectedResults = new List<ProviderSiteEmployerLink> { _providerSiteEmployerLink1, duplicateEmployerLink };
            linksPage.Should().NotBeNull();
            linksPage.Page.Count().Should().Be(expectedResults.Count);
            linksPage.Page.ShouldBeEquivalentTo(expectedResults);
            linksPage.ResultsCount.Should().Be(expectedResults.Count);
            linksPage.TotalNumberOfPages.Should().Be(1);
        }

        [Test]
        public void ErnRepositorySearch()
        {
            var fromRepository = new List<ProviderSiteEmployerLink> { _providerSiteEmployerLink1, _providerSiteEmployerLink2, _providerSiteEmployerLink3 };
            _providerSiteEmployerLinkReadRepository.Setup(r => r.GetForProviderSite(ProviderSiteErn)).Returns(fromRepository);
            var service = new ProviderServiceBuilder().With(_providerSiteEmployerLinkReadRepository.Object).Build();
            var employerSearchRequest = new EmployerSearchRequest(ProviderSiteErn, Ern2);

            var linksPage = service.GetProviderSiteEmployerLinks(employerSearchRequest, CurrentPage, PageSize);

            var expectedResults = new List<ProviderSiteEmployerLink> { _providerSiteEmployerLink2 };
            linksPage.Should().NotBeNull();
            linksPage.Page.Count().Should().Be(expectedResults.Count);
            linksPage.Page.ShouldBeEquivalentTo(expectedResults);
            linksPage.ResultsCount.Should().Be(expectedResults.Count);
            linksPage.TotalNumberOfPages.Should().Be(1);
        }

        [Test]
        public void NameRepositorySearch()
        {
            var fromRepository = new List<ProviderSiteEmployerLink> { _providerSiteEmployerLink1, _providerSiteEmployerLink2, _providerSiteEmployerLink3 };
            _providerSiteEmployerLinkReadRepository.Setup(r => r.GetForProviderSite(ProviderSiteErn)).Returns(fromRepository);
            var service = new ProviderServiceBuilder().With(_providerSiteEmployerLinkReadRepository.Object).Build();
            var employerSearchRequest = new EmployerSearchRequest(ProviderSiteErn, _providerSiteEmployerLink3.Employer.Name.Substring(0, 10), null);

            var linksPage = service.GetProviderSiteEmployerLinks(employerSearchRequest, CurrentPage, PageSize);

            var expectedResults = new List<ProviderSiteEmployerLink> { _providerSiteEmployerLink3 };
            linksPage.Should().NotBeNull();
            linksPage.Page.Count().Should().Be(expectedResults.Count);
            linksPage.Page.ShouldBeEquivalentTo(expectedResults);
            linksPage.ResultsCount.Should().Be(expectedResults.Count);
            linksPage.TotalNumberOfPages.Should().Be(1);
        }

        [Test]
        public void NameAndPostCodeRepositorySearch()
        {
            var fromRepository = new List<ProviderSiteEmployerLink> { _providerSiteEmployerLink1, _providerSiteEmployerLink2, _providerSiteEmployerLink3 };
            _providerSiteEmployerLinkReadRepository.Setup(r => r.GetForProviderSite(ProviderSiteErn)).Returns(fromRepository);
            var service = new ProviderServiceBuilder().With(_providerSiteEmployerLinkReadRepository.Object).Build();
            var employerSearchRequest = new EmployerSearchRequest(ProviderSiteErn, _providerSiteEmployerLink1.Employer.Name.Substring(0, 10), _providerSiteEmployerLink1.Employer.Address.Postcode.Substring(0, 10));

            var linksPage = service.GetProviderSiteEmployerLinks(employerSearchRequest, CurrentPage, PageSize);

            var expectedResults = new List<ProviderSiteEmployerLink> { _providerSiteEmployerLink1 };
            linksPage.Should().NotBeNull();
            linksPage.Page.Count().Should().Be(expectedResults.Count);
            linksPage.Page.ShouldBeEquivalentTo(expectedResults);
            linksPage.ResultsCount.Should().Be(expectedResults.Count);
            linksPage.TotalNumberOfPages.Should().Be(1);
        }

        [Test]
        public void PostCodeRepositorySearch()
        {
            var fromRepository = new List<ProviderSiteEmployerLink> { _providerSiteEmployerLink1, _providerSiteEmployerLink2, _providerSiteEmployerLink3 };
            _providerSiteEmployerLinkReadRepository.Setup(r => r.GetForProviderSite(ProviderSiteErn)).Returns(fromRepository);
            var service = new ProviderServiceBuilder().With(_providerSiteEmployerLinkReadRepository.Object).Build();
            var employerSearchRequest = new EmployerSearchRequest(ProviderSiteErn, null, _providerSiteEmployerLink3.Employer.Address.Postcode.Substring(0, 10));

            var linksPage = service.GetProviderSiteEmployerLinks(employerSearchRequest, CurrentPage, PageSize);

            var expectedResults = new List<ProviderSiteEmployerLink> { _providerSiteEmployerLink3 };
            linksPage.Should().NotBeNull();
            linksPage.Page.Count().Should().Be(expectedResults.Count);
            linksPage.Page.ShouldBeEquivalentTo(expectedResults);
            linksPage.ResultsCount.Should().Be(expectedResults.Count);
            linksPage.TotalNumberOfPages.Should().Be(1);
        }
    }
}