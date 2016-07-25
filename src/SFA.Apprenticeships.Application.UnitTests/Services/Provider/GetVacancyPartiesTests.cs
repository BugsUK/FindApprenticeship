namespace SFA.Apprenticeships.Application.UnitTests.Services.Provider
{
    using System.Collections.Generic;
    using System.Linq;
    using Domain.Entities.Raa.Parties;
    using Domain.Raa.Interfaces.Repositories;
    using FluentAssertions;
    using Interfaces.Employers;
    using Moq;
    using NUnit.Framework;
    using Ploeh.AutoFixture;

    [TestFixture]
    public class GetVacancyPartiesTests
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
        private List<VacancyParty> _fromRepository;

        private Mock<IEmployerService> _employerService;
        private Mock<IVacancyPartyReadRepository> _vacancyPartyReadRepository;

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

            _employerService = new Mock<IEmployerService>();
            _employerService.Setup(r => r.GetEmployers(It.IsAny<IEnumerable<int>>())).Returns(_employersFromService);
            _vacancyPartyReadRepository = new Mock<IVacancyPartyReadRepository>();
            _vacancyPartyReadRepository.Setup(r => r.GetByProviderSiteId(ProviderSiteId)).Returns(_fromRepository);
        }

        [Test]
        public void FromRepository()
        {
            var service = new ProviderServiceBuilder().With(_employerService.Object).With(_vacancyPartyReadRepository.Object).Build();

            var linksPage = service.GetVacancyParties(_employerSearchRequest, CurrentPage, PageSize);

            linksPage.Should().NotBeNull();
            linksPage.Page.Count().Should().Be(_fromRepository.Count);
            linksPage.Page.ShouldBeEquivalentTo(_fromRepository);
            linksPage.ResultsCount.Should().Be(_fromRepository.Count);
            linksPage.TotalNumberOfPages.Should().Be(1);
        }

        [Test]
        public void ErnRepositorySearch()
        {
            var fromRepository = new List<VacancyParty> { _providerSiteEmployerLink1, _providerSiteEmployerLink2, _providerSiteEmployerLink3 };
            _vacancyPartyReadRepository.Setup(r => r.GetByProviderSiteId(ProviderSiteId)).Returns(fromRepository);
            var service = new ProviderServiceBuilder().With(_employerService.Object).With(_vacancyPartyReadRepository.Object).Build();
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
            _vacancyPartyReadRepository.Setup(r => r.GetByProviderSiteId(ProviderSiteId)).Returns(fromRepository);
            var service = new ProviderServiceBuilder().With(_employerService.Object).With(_vacancyPartyReadRepository.Object).Build();
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
            _vacancyPartyReadRepository.Setup(r => r.GetByProviderSiteId(ProviderSiteId)).Returns(fromRepository);
            var service = new ProviderServiceBuilder().With(_employerService.Object).With(_vacancyPartyReadRepository.Object).Build();
            var employerSearchRequest = new EmployerSearchRequest(ProviderSiteId, _employer1.Name.Substring(0, 10), _employer1.Address.Postcode.Substring(0, 10));

            var linksPage = service.GetVacancyParties(employerSearchRequest, CurrentPage, PageSize);

            var expectedResults = new List<VacancyParty> { _providerSiteEmployerLink1 };
            linksPage.Should().NotBeNull();
            linksPage.Page.Count().Should().Be(expectedResults.Count);
            linksPage.Page.ShouldBeEquivalentTo(expectedResults);
            linksPage.ResultsCount.Should().Be(expectedResults.Count);
            linksPage.TotalNumberOfPages.Should().Be(1);
        }

        [TestCase("AddressLine4Kidderminster", null)]
        [TestCase(null, "TownKidderminster")]
        public void NameAndLocationRepositorySearch(string addressLine4, string town)
        {
            // Arrange.
            var fromRepository = new List<VacancyParty>
            {
                _providerSiteEmployerLink1,
                _providerSiteEmployerLink2,
                _providerSiteEmployerLink3
            };

            _vacancyPartyReadRepository.Setup(r => r.
                GetByProviderSiteId(ProviderSiteId))
                .Returns(fromRepository);

            var service = new ProviderServiceBuilder()
                .With(_employerService.Object)
                .With(_vacancyPartyReadRepository.Object)
                .Build();

            _employer1.Address.AddressLine4 = addressLine4;
            _employer1.Address.Town = town;

            var employerName = _employer1.Name.Substring(0, 10);
            var location = (addressLine4 ?? town).Substring(0, 15);

            var employerSearchRequest = new EmployerSearchRequest(
                ProviderSiteId, employerName, location);

            var expectedResults = new List<VacancyParty>
            {
                _providerSiteEmployerLink1
            };

            // Act.
            var linksPage = service.GetVacancyParties(employerSearchRequest, CurrentPage, PageSize);

            // Assert.
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
            _vacancyPartyReadRepository.Setup(r => r.GetByProviderSiteId(ProviderSiteId)).Returns(fromRepository);
            var service = new ProviderServiceBuilder().With(_employerService.Object).With(_vacancyPartyReadRepository.Object).Build();
            var employerSearchRequest = new EmployerSearchRequest(ProviderSiteId, null, _employer3.Address.Postcode.Substring(0, 10));

            var linksPage = service.GetVacancyParties(employerSearchRequest, CurrentPage, PageSize);

            var expectedResults = new List<VacancyParty> { _providerSiteEmployerLink3 };
            linksPage.Should().NotBeNull();
            linksPage.Page.Count().Should().Be(expectedResults.Count);
            linksPage.Page.ShouldBeEquivalentTo(expectedResults);
            linksPage.ResultsCount.Should().Be(expectedResults.Count);
            linksPage.TotalNumberOfPages.Should().Be(1);
        }

        [TestCase("AddressLine4Kidderminster", null)]
        [TestCase(null, "TownKidderminster")]
        public void LocationRepositorySearch(string addressLine4, string town)
        {
            // Arrange.
            var fromRepository = new List<VacancyParty>
            {
                _providerSiteEmployerLink1, _providerSiteEmployerLink2, _providerSiteEmployerLink3
            };

            _vacancyPartyReadRepository.Setup(r => r
                .GetByProviderSiteId(ProviderSiteId))
                .Returns(fromRepository);

            var service = new ProviderServiceBuilder()
                .With(_employerService.Object)
                .With(_vacancyPartyReadRepository.Object)
                .Build();

            _employer3.Address.AddressLine4 = addressLine4;
            _employer3.Address.Town = town;

            var location = (addressLine4 ?? town).Substring(0, 15);

            var employerSearchRequest = new EmployerSearchRequest(
                ProviderSiteId, null, location);

            var expectedResults = new List<VacancyParty>
            {
                _providerSiteEmployerLink3
            };

            // Act.
            var linksPage = service.GetVacancyParties(employerSearchRequest, CurrentPage, PageSize);

            // Assert.
            linksPage.Should().NotBeNull();

            linksPage.Page.Count().Should().Be(expectedResults.Count);
            linksPage.Page.ShouldBeEquivalentTo(expectedResults);

            linksPage.ResultsCount.Should().Be(expectedResults.Count);
            linksPage.TotalNumberOfPages.Should().Be(1);
        }
    }
}
