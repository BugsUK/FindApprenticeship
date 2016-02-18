namespace SFA.Apprenticeships.Application.UnitTests.Employer
{
    using Application.Employer;
    using Domain.Entities.Raa.Parties;
    using Domain.Raa.Interfaces.Repositories;
    using FluentAssertions;
    using Interfaces.Employers;
    using Infrastructure.Interfaces;
    using Interfaces.Organisations;
    using Moq;
    using NUnit.Framework;

    [TestFixture]
    public class EmployerServiceTests
    {
        [Test]
        [ExpectedException]
        public void ShouldThrowAnExceptionIfErnIsNullWhenGettingAnEmployer()
        {
            var service = GetService();

            service.GetEmployer(null);
        }

        [Test]
        [ExpectedException]
        public void ShouldThrowAnExceptionIfErnIsEmptyWhenGettingAnEmployer()
        {
            var service = GetService();

            service.GetEmployer(string.Empty);
        }

        [Test]
        public void IfRepositoryReturnsAnEmployerReturnIt()
        {
            const string ern = "ern";
            var employer = new Employer
            {
                EdsErn = ern
            };
            var employerReadRepository = new Mock<IEmployerReadRepository>();
            var organisationServie = new Mock<IOrganisationService>();
            employerReadRepository.Setup(r => r.Get(ern)).Returns(employer);

            var service = GetService(employerReadRepository);

            var result = service.GetEmployer(ern);
            result.ShouldBeEquivalentTo(employer);
            employerReadRepository.Verify(r => r.Get(ern));
            organisationServie.Verify(s => s.GetEmployer(ern), Times.Never);
        }

        [Test]
        public void IfRepositoryDoesNotReturnsAnEmployerReturnItFromOrganisationService()
        {
            const string ern = "ern";
            Employer nullEmployer = null;
            var employer = new Employer
            {
                EdsErn = ern
            };
            var employerReadRepository = new Mock<IEmployerReadRepository>();
            var organisationServie = new Mock<IOrganisationService>();
            employerReadRepository.Setup(r => r.Get(ern)).Returns(nullEmployer);
            organisationServie.Setup(s => s.GetEmployer(ern)).Returns(employer);

            var service = GetService(employerReadRepository, organisationServie);

            var result = service.GetEmployer(ern);
            result.ShouldBeEquivalentTo(employer);
            employerReadRepository.Verify(r => r.Get(ern));
            organisationServie.Verify(s => s.GetEmployer(ern), Times.Once);
        }

        [Test]
        public void GetEmployersShouldCalOrganisationService()
        {
            var organisationServie = new Mock<IOrganisationService>();
            var service = GetService(null, organisationServie);
            const string ern = "Ern";
            const string name = "name";
            const string location = "Location";

            service.GetEmployers(ern, name, location);

            organisationServie.Verify(s => s.GetEmployers(ern, name, location));
        }

        [Test]
        public void SaveEmployerShouldCallWriteRepository()
        {
            const string ern = "ern";
            var employer = new Employer
            {
                EdsErn = ern
            };
            var writeRepository = new Mock<IEmployerWriteRepository>();
            var service = GetService(null, null, writeRepository);

            service.SaveEmployer(employer);

            writeRepository.Verify(w => w.Save(It.Is<Employer>(e => e.EdsErn == ern)));
        }

        [Test]
        public void GetEmployersGetsThemFromOrganisationalService()
        {
            const string ern = "ern";
            const string name = "name";
            const string location = "location";
            const int currentPage = 1;
            const int pageSize = 2;

            var organisationServie = new Mock<IOrganisationService>();
            var service = GetService(null, organisationServie);

            service.GetEmployers(ern, name, location, currentPage, pageSize);

            organisationServie.Verify(s => s.GetEmployers(ern, name, location, currentPage, pageSize));
        }

        private IEmployerService GetService(Mock<IEmployerReadRepository> readRepository = null, 
            Mock<IOrganisationService> orgService = null, Mock<IEmployerWriteRepository> writeRepo = null)
        {
            var organisationService = orgService ?? new Mock<IOrganisationService>();
            var employerReadRepository = readRepository ?? new Mock<IEmployerReadRepository>();
            var employerWriteRepository = writeRepo ?? new Mock<IEmployerWriteRepository>();
            var logService = new Mock<ILogService>();

            return new EmployerService(organisationService.Object, employerReadRepository.Object,
                employerWriteRepository.Object, logService.Object);
        }
    }
}