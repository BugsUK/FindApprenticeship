namespace SFA.Apprenticeships.Infrastructure.Repositories.Sql.Tests.Schemas.dbo.Employer
{
    using System.Collections.Generic;
    using System.Linq;
    using Common;
    using Domain.Raa.Interfaces.Repositories;
    using FluentAssertions;
    using NUnit.Framework;
    using SFA.Infrastructure.Interfaces;
    using Sql.Common;
    using Sql.Schemas.dbo;

    [TestFixture]
    public class EmployerRepositoryTests
    {
        private readonly IMapper _mapper = new EmployerMappers();
        private IGetOpenConnection _connection;
        private IEmployerReadRepository _employerReadRepository;

        [SetUp]
        public void SetUpFixture()
        {
            _connection = new GetOpenConnectionFromConnectionString(
                DatabaseConfigurationProvider.Instance.TargetConnectionString);

            _employerReadRepository = new EmployerRepository(_connection, _mapper);
        }

        [Test]
        public void ShouldGetEmployerById()
        {
            // Act.
            var employer = _employerReadRepository.GetById(1);

            // Assert.
            employer.Should().NotBeNull();
        }

        [Test]
        public void ShouldGetEmployerByEdsUrn()
        {
            // Act.
            var employer = _employerReadRepository.GetByEdsUrn("101");

            // Assert.
            employer.Should().NotBeNull();
        }

        [Test]
        public void ShouldGetEmployersByIds()
        {
            // Arrange.
            var employerIds = new List<int>
            {
                1,
                2
            };

            // Act.
            var employers = _employerReadRepository.GetByIds(employerIds);

            // Assert.
            employers.Should().NotBeNull();
            employers.Count.Should().Be(employerIds.Count);
            employers.All(employer => employerIds.Contains(employer.EmployerId)).Should().Be(true);
        }
    }
}
