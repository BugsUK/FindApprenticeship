namespace SFA.Apprenticeships.Application.UnitTests.Employer.Strategies
{
    using Apprenticeships.Application.Employer.Strategies;
    using Domain.Raa.Interfaces.Repositories;
    using Interfaces.Organisations;
    using Moq;
    using NUnit.Framework;
    using Interfaces;

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

            strategy.GetEmployers(edsUrn, name, location, 1, 25);

            int resultCount;
            organisationService.Verify(s => s.GetVerifiedOrganisationSummaries(edsUrn, name, location, out resultCount));
        }

        private ISearchEmployersStrategy GetStrategy(Mock<IOrganisationService> orgService = null)
        {
            var organisationService = orgService ?? new Mock<IOrganisationService>();
            var mappers = new Mock<IMapper>();

            return new SearchEmployersStrategy(new Mock<IEmployerReadRepository>().Object, organisationService.Object, mappers.Object);
        }
    }
}