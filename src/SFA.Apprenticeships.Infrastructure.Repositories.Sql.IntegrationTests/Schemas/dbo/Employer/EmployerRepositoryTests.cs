namespace SFA.Apprenticeships.Infrastructure.Repositories.Sql.IntegrationTests.Schemas.dbo.Employer
{
    using System.Collections.Generic;
    using System.Linq;
    using Common;
    using Domain.Raa.Interfaces.Repositories;
    using FluentAssertions;
    using Moq;
    using NUnit.Framework;

    using SFA.Apprenticeships.Application.Interfaces;
    using SFA.Infrastructure.Interfaces;
    using Sql.Common;
    using Sql.Schemas.dbo;
    using Sql.Schemas.Reference;

    [TestFixture]
    public class EmployerRepositoryTests
    {
        private readonly IMapper _mapper = new EmployerMappers();
        private IGetOpenConnection _connection;
        private IEmployerReadRepository _employerReadRepository;
        private IEmployerWriteRepository _employerWriteRepository;
        private IReferenceRepository _referenceRepository;

        [SetUp]
        public void SetUpFixture()
        {
            _connection = new GetOpenConnectionFromConnectionString(
                DatabaseConfigurationProvider.Instance.TargetConnectionString);

            _referenceRepository = new ReferenceRepository(_connection, new ReferenceMappers(), new Mock<ILogService>().Object);
            _employerReadRepository = new EmployerRepository(_connection, _mapper, _referenceRepository);
            _employerWriteRepository = new EmployerRepository(_connection, _mapper, _referenceRepository);
        }

        [Test]
        public void ShouldGetEmployerById()
        {
            // Act.
            var employer = _employerReadRepository.GetById(SeedData.Employers.AcmeCorp.EmployerId);

            // Assert.
            employer.Should().NotBeNull();
        }

        [Test]
        public void ShouldGetEmployerByEdsUrn()
        {
            // Act.
            var employer = _employerReadRepository.GetByEdsUrn(SeedData.Employers.AcmeCorp.EdsUrn);

            // Assert.
            employer.Should().NotBeNull();
        }

        [Test]
        public void ShouldGetEmployersByIds()
        {
            // Arrange.
            var employerIds = new List<int>
            {
                SeedData.Employers.AcmeCorp.EmployerId,
                SeedData.Employers.AwesomeInc.EmployerId
            };

            // Act.
            var employers = _employerReadRepository.GetByIds(employerIds);

            // Assert.
            employers.Should().NotBeNull();
            employers.Count.Should().Be(employerIds.Count);
            employers.All(employer => employerIds.Contains(employer.EmployerId)).Should().Be(true);
        }

        [Test]
        public void ShouldCreateEmployer()
        {
            // Arrange.
            var employer = _employerReadRepository.GetById(SeedData.Employers.AcmeCorp.EmployerId);
            employer.EmployerId = 0;
            employer.EdsUrn = "-" + employer.EdsUrn;

            // Act.
            var newEmployer = _employerWriteRepository.Save(employer);

            // Assert.
            newEmployer.Should().NotBeNull();
            newEmployer.EmployerId.Should().NotBe(0);
            newEmployer.EmployerId.Should().NotBe(SeedData.Employers.AcmeCorp.EmployerId);
        }

        [Test]
        public void ShouldUpdateEmployer()
        {
            // Act.
            var employer = _employerReadRepository.GetById(SeedData.Employers.AcmeCorp.EmployerId);

            // Assert.
            employer.Should().NotBeNull();

            // Arrange.
            var newName = new string(employer.FullName.Reverse().ToArray());

            employer.FullName = newName;

            // Act.
            var updatedEmployer = _employerWriteRepository.Save(employer);

            // Assert.
            updatedEmployer.Should().NotBeNull();
            updatedEmployer.EmployerId.Should().Be(employer.EmployerId);
            updatedEmployer.FullName.Should().Be(newName);
        }
    }
}
