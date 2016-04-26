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


        [TestFixtureSetUp]
        public void SetUpFixture()
        {
            _connection = new GetOpenConnectionFromConnectionString(DatabaseConfigurationProvider.Instance.TargetConnectionString);

            _logger = new Mock<ILogService>();

            _repoUnderTest = new ReportingRepository(_connection, _logger.Object);
        }

        [Test]
        public void ReportReturnsResults()
        {
            //Arrange

            //Act
            var result = _repoUnderTest.ReportVacanciesList(new DateTime(2016, 1, 1), new DateTime(2016, 1, 6));

            //Assert
            result.Should().NotBeNull();
            result.Count.Should().BePositive();
        }
    }
}
