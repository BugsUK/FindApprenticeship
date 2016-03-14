namespace SFA.Apprenticeships.Application.UnitTests.Employer.Strategies
{
    using Apprenticeships.Application.Employer.Mappers;
    using Apprenticeships.Application.Employer.Strategies;
    using Domain.Entities.Raa.Parties;
    using Domain.Raa.Interfaces.Repositories;
    using FluentAssertions;
    using Interfaces.Organisations;
    using Moq;
    using NUnit.Framework;
    using Ploeh.AutoFixture;
    using SFA.Infrastructure.Interfaces;

    [TestFixture]
    public class GetByEdsUrnStrategyTests
    {
        private readonly Mock<IEmployerReadRepository> _employerReadRepository = new Mock<IEmployerReadRepository>();
        private readonly Mock<IEmployerWriteRepository> _employerWriteRepository = new Mock<IEmployerWriteRepository>();
        private readonly Mock<IOrganisationService> _organisationService = new Mock<IOrganisationService>();
        private IGetByEdsUrnStrategy _strategy;

        [SetUp]
        public void Setup()
        {
            _employerWriteRepository.Setup(r => r.Save(It.IsAny<Employer>())).Returns<Employer>(e => e);
            _strategy = new GetByEdsUrnStrategy(_employerReadRepository.Object, _employerWriteRepository.Object, _organisationService.Object, new EmployerMappers(), new Mock<ILogService>().Object);
        }

        [Test]
        public void GetEmployerFromRepository()
        {
            //Arrange
            const string edsUrn = "1234";
            var repositoryEmployer = new Fixture().Create<Employer>();
            repositoryEmployer.EdsUrn = edsUrn;
            _employerReadRepository.Setup(e => e.GetByEdsUrn(edsUrn)).Returns(repositoryEmployer);

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
            verifiedOrganisationSummary.ReferenceNumber = edsUrn;
            _organisationService.Setup(os => os.GetVerifiedOrganisationSummary(edsUrn)).Returns(verifiedOrganisationSummary);

            //Act
            var employer = _strategy.Get(edsUrn);

            //Assert
            employer.EdsUrn.Should().Be(verifiedOrganisationSummary.ReferenceNumber);
            _organisationService.Verify(os => os.GetVerifiedOrganisationSummary(edsUrn), Times.Once);
            _employerWriteRepository.Verify(r => r.Save(It.IsAny<Employer>()), Times.Once);
        }
    }
}