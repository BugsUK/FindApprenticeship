namespace SFA.Apprenticeships.Application.UnitTests.Employer.Strategies
{
    using Apprenticeships.Application.Employer.Strategies;
    using Interfaces.Organisations;
    using Moq;
    using NUnit.Framework;
    using SFA.Infrastructure.Interfaces;

    [TestFixture]
    public class GetPagedEmployerSearchResultsStrategyTests
    {
        [Test]
        public void GetEmployersShouldCallOrganisationService()
        {
            var organisationService = new Mock<IOrganisationService>();
            var strategy = GetStrategy(organisationService);
            const string edsUrn = "EdsUrn";
            const string name = "name";
            const string location = "Location";

            strategy.Get(edsUrn, name, location, 1, 25);

            organisationService.Verify(s => s.GetVerifiedOrganisationSummaries(edsUrn, name, location));
        }

        private IGetPagedEmployerSearchResultsStrategy GetStrategy(Mock<IOrganisationService> orgService = null)
        {
            var organisationService = orgService ?? new Mock<IOrganisationService>();
            var mappers = new Mock<IMapper>();

            return new GetPagedEmployerSearchResultsStrategy(organisationService.Object, mappers.Object);
        }
    }
}