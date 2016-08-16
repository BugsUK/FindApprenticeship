namespace SFA.Apprenticeships.Infrastructure.Repositories.Sql.IntegrationTests.Reporting
{
    using System;
    using Common;
    using FluentAssertions;
    using Moq;
    using NUnit.Framework;
    using SFA.Infrastructure.Interfaces;
    using Sql.Common;
    using Sql.Reporting;

    [TestFixture(Category = "Integration")]
    public class ReportingRepositoryTests
    {
        private ReportingRepository _repoUnderTest;
        private IGetOpenConnection _connection;
        private Mock<ILogService> _logger;


        [OneTimeSetUp]
        public void SetUpFixture()
        {
            _connection = new GetOpenConnectionFromConnectionString(DatabaseConfigurationProvider.Instance.TargetConnectionString);

            _logger = new Mock<ILogService>();

            _repoUnderTest = new ReportingRepository(_connection, _logger.Object);
        }

        [Test]
        public void GeoRegionsIncludingAllCanBeCalled()
        {
            //Arrange

            //Act
            var result = _repoUnderTest.GeoRegionsIncludingAll();

            //Assert
            result.Should().NotBeNull();
        }

        [Test]
        public void LocalAuthorityManagerGroupsCanBeCalled()
        {
            //Arrange

            //Act
            var result = _repoUnderTest.LocalAuthorityManagerGroups();

            //Assert
            result.Should().NotBeNull();
        }

        [Test]
        public void ReportSuccessfulCandidatesCanBeCalled()
        {
            //Arrange

            //Act
            var result = _repoUnderTest.ReportSuccessfulCandidates("-1", DateTime.Now.AddDays(-1), DateTime.Now, "-1", "-1", "-1");

            //Assert
            result.Should().NotBeNull();
        }

        [Test]
        public void ReportUnsuccessfulCandidatesCanBeCalled()
        {
            //Arrange

            //Act
            var result = _repoUnderTest.ReportUnsuccessfulCandidates("-1", DateTime.Now.AddDays(-1), DateTime.Now, "-1", "-1", "-1");

            //Assert
            result.Should().NotBeNull();
        }

        [Test]
        public void ReportVacanciesListCanBeCalled()
        {
            //Arrange

            //Act
            var result = _repoUnderTest.ReportVacanciesList(DateTime.Now.AddDays(-1), DateTime.Now);

            //Assert
            result.Should().NotBeNull();
        }

        [Test]
        public void ReportVacancyExtensionsCanBeCalled()
        {
            //Arrange

            //Act
            var result = _repoUnderTest.ReportVacancyExtensions(DateTime.Now.AddDays(-1), DateTime.Now, null, null);

            //Assert
            result.Should().NotBeNull();
        }
    }
}
