namespace SFA.Apprenticeships.Application.UnitTests.Employer.Strategies
{
    using Apprenticeships.Application.Employer.Strategies;
    using Domain.Entities.Locations;
    using Domain.Entities.Raa.Parties;
    using Domain.Raa.Interfaces.Repositories;
    using FluentAssertions;
    using Interfaces.Organisations;
    using Location;
    using Moq;
    using NUnit.Framework;
    using Ploeh.AutoFixture;

    using SFA.Apprenticeships.Application.Interfaces;
    using SFA.Infrastructure.Interfaces;

    [TestFixture]
    public class GetByEdsUrnStrategyTests
    {
        private readonly Mock<IEmployerReadRepository> _employerReadRepository = new Mock<IEmployerReadRepository>();
        private readonly Mock<IEmployerWriteRepository> _employerWriteRepository = new Mock<IEmployerWriteRepository>();
        private readonly Mock<IOrganisationService> _organisationService = new Mock<IOrganisationService>();
        private readonly Mock<IAddressLookupProvider> _addressLookupProvider = new Mock<IAddressLookupProvider>();
        private readonly Mock<IMapper> _mapper = new Mock<IMapper>();
        private IGetByEdsUrnStrategy _strategy;

        [SetUp]
        public void Setup()
        {
            _employerWriteRepository.Setup(r => r.Save(It.IsAny<Employer>())).Returns<Employer>(e => e);
            _strategy = new GetByEdsUrnStrategy(_employerReadRepository.Object, _employerWriteRepository.Object, _organisationService.Object, _addressLookupProvider.Object, _mapper.Object, new Mock<ILogService>().Object);
        }

        [Test]
        public void GetEmployerFromRepository()
        {
            //Arrange
            const string edsUrn = "1234";
            var repositoryEmployer = new Fixture().Create<Employer>();
            repositoryEmployer.EdsUrn = edsUrn;
            _employerReadRepository.Reset();
            _employerReadRepository.Setup(e => e.GetByEdsUrn(edsUrn)).Returns(repositoryEmployer);
            _employerWriteRepository.Reset();
            _organisationService.Reset();

            //Act
            var employer = _strategy.Get(edsUrn);

            //Assert
            employer.Should().Be(repositoryEmployer);
            _organisationService.Verify(os => os.GetVerifiedOrganisationSummary(It.IsAny<string>()), Times.Never);
            _employerWriteRepository.Verify(r => r.Save(It.IsAny<Employer>()), Times.Never);
        }

        [Test]
        public void GetAndStoreEmployerFromOrganisationServiceWhenNotFoundInRepository()
        {
            //Arrange
            const string edsUrn = "4567";
            var verifiedOrganisationSummary = new Fixture().Create<VerifiedOrganisationSummary>();
            var mappedEmployer = new Fixture().Create<Employer>();
            verifiedOrganisationSummary.ReferenceNumber = edsUrn;
            _employerReadRepository.Reset();
            _employerWriteRepository.Reset();
            _organisationService.Reset();
            _organisationService.Setup(os => os.GetVerifiedOrganisationSummary(edsUrn)).Returns(verifiedOrganisationSummary);
            _mapper.Setup(m => m.Map<VerifiedOrganisationSummary, Employer>(verifiedOrganisationSummary))
                .Returns(mappedEmployer);

            //Act
            var employer = _strategy.Get(edsUrn);

            //Assert
            _organisationService.Verify(os => os.GetVerifiedOrganisationSummary(edsUrn), Times.Once);
            _employerWriteRepository.Verify(r => r.Save(mappedEmployer), Times.Once);
        }

        [Test]
        public void IfTheEmployerHasNoCountySetFromEdsWebServiceTryToSetIt()
        {
            //Arrange
            const string edsUrn = "4567";
            var verifiedOrganisationSummary = new Fixture().Create<VerifiedOrganisationSummary>();
            var mappedEmployer = new Fixture().Create<Employer>();
            mappedEmployer.Address.County = null;
            verifiedOrganisationSummary.ReferenceNumber = edsUrn;
            _organisationService.Setup(os => os.GetVerifiedOrganisationSummary(edsUrn)).Returns(verifiedOrganisationSummary);
            _mapper.Setup(m => m.Map<VerifiedOrganisationSummary, Employer>(verifiedOrganisationSummary))
                .Returns(mappedEmployer);

            //Act
            var employer = _strategy.Get(edsUrn);

            //Assert
            _addressLookupProvider.Verify(p => p.GetPossibleAddresses(verifiedOrganisationSummary.Address.Postcode));
        }

        [Test]
        public void IfTheEmployerHasNoCountySetFromEdsWebServiceSaveTheOneRetrievedByPostCodeAnywhere()
        {
            //Arrange
            const string edsUrn = "4567";
            var verifiedOrganisationSummary = new Fixture().Create<VerifiedOrganisationSummary>();
            var mappedEmployer = new Fixture().Create<Employer>();
            mappedEmployer.Address.County = null;
            verifiedOrganisationSummary.ReferenceNumber = edsUrn;
            _organisationService.Setup(os => os.GetVerifiedOrganisationSummary(edsUrn)).Returns(verifiedOrganisationSummary);
            _mapper.Setup(m => m.Map<VerifiedOrganisationSummary, Employer>(verifiedOrganisationSummary))
                .Returns(mappedEmployer);

            var address = new Fixture().Create<Address>();
            _addressLookupProvider.Setup(p => p.GetPossibleAddresses(verifiedOrganisationSummary.Address.Postcode)).Returns(new[] {address});

            //Act
            var employer = _strategy.Get(edsUrn);

            //Assert
            _employerWriteRepository.Verify(r => r.Save(It.Is<Employer>(e => e.Address.County == address.County)));
        }
    }
}