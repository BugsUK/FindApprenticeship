namespace SFA.Apprenticeships.Application.UnitTests.Employer.Strategies
{
    using Apprenticeships.Application.Employer.Strategies;
    using Domain.Entities.Raa.Parties;
    using Domain.Raa.Interfaces.Repositories;
    using FluentAssertions;
    using Interfaces.Organisations;
    using Moq;
    using NUnit.Framework;
    using Ploeh.AutoFixture;
    using Interfaces;

    [TestFixture]
    public class GetByEdsUrnStrategyTests
    {
        private readonly Mock<IEmployerReadRepository> _employerReadRepository = new Mock<IEmployerReadRepository>();
        private readonly Mock<IEmployerWriteRepository> _employerWriteRepository = new Mock<IEmployerWriteRepository>();
        private readonly Mock<IOrganisationService> _organisationService = new Mock<IOrganisationService>();
        private readonly Mock<IMapper> _mapper = new Mock<IMapper>();
        private IGetByEdsUrnStrategy _strategy;

        [SetUp]
        public void Setup()
        {
            _employerWriteRepository.Setup(r => r.Save(It.IsAny<Employer>())).Returns<Employer>(e => e);
            _strategy = new GetByEdsUrnStrategy(_employerReadRepository.Object, _employerWriteRepository.Object, _organisationService.Object, _mapper.Object, new Mock<ILogService>().Object);
        }

        [Test]
        public void GetEmployerFromRepository()
        {
            //Arrange
            const string edsUrn = "1234";
            var repositoryEmployer = new Fixture().Create<Employer>();
            repositoryEmployer.EdsUrn = edsUrn;
            _employerReadRepository.Reset();
            _employerReadRepository.Setup(e => e.GetByEdsUrn(edsUrn, It.IsAny<bool>())).Returns(repositoryEmployer);
            _employerWriteRepository.Reset();
            _organisationService.Reset();

            //Act
            var employer = _strategy.Get(edsUrn);

            //Assert
            employer.Should().Be(repositoryEmployer);
            _organisationService.Verify(os => os.GetVerifiedOrganisationSummary(It.IsAny<string>()), Times.Once);
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
            _strategy.Get(edsUrn);

            //Assert
            _organisationService.Verify(os => os.GetVerifiedOrganisationSummary(edsUrn), Times.Once);
            _employerWriteRepository.Verify(r => r.Save(mappedEmployer), Times.Once);
        }
    }
}