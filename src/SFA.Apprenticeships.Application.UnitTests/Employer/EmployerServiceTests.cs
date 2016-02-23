namespace SFA.Apprenticeships.Application.UnitTests.Employer
{
    using Application.Employer;
    using Interfaces.Employers;
    using Infrastructure.Interfaces;
    using Interfaces.Organisations;
    using Moq;
    using NUnit.Framework;

    [TestFixture]
    public class EmployerServiceTests
    {
        [Test]
        public void GetEmployersShouldCalOrganisationService()
        {
            var organisationServie = new Mock<IOrganisationService>();
            var service = GetService(organisationServie);
            const string edsUrn = "EdsUrn";
            const string name = "name";
            const string location = "Location";

            service.GetEmployers(edsUrn, name, location);

            organisationServie.Verify(s => s.GetEmployers(edsUrn, name, location));
        }

        [Test]
        public void GetEmployersGetsThemFromOrganisationalService()
        {
            const string edsUrn = "edsUrn";
            const string name = "name";
            const string location = "location";
            const int currentPage = 1;
            const int pageSize = 2;

            var organisationServie = new Mock<IOrganisationService>();
            var service = GetService(organisationServie);

            service.GetEmployers(edsUrn, name, location, currentPage, pageSize);

            organisationServie.Verify(s => s.GetEmployers(edsUrn, name, location, currentPage, pageSize));
        }

        private IEmployerService GetService(Mock<IOrganisationService> orgService = null)
        {
            var organisationService = orgService ?? new Mock<IOrganisationService>();
            var logService = new Mock<ILogService>();

            return new EmployerService(organisationService.Object, logService.Object);
        }
    }
}