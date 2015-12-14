namespace SFA.Apprenticeship.Api.AvService.UnitTests.Providers.Version51.VacancyDetailsProvider
{
    using System;
    using Apprenticeships.Domain.Interfaces.Configuration;
    using Apprenticeships.Domain.Interfaces.Repositories;
    using AvService.Mappers.Version51;
    using AvService.Providers.Version51;
    using DataContracts.Version51;
    using FluentAssertions;
    using MessageContracts.Version51;
    using Moq;
    using NUnit.Framework;

    [TestFixture]
    public class ExceptionHandlingTests
    {
        private IVacancyDetailsProvider _provider;

        private Mock<IConfigurationService> _mockConfigurationService;
        private Mock<IApprenticeshipVacancyReadRepository> _mockApprenticeshipVacancyReadRepository;

        [SetUp]
        public void SetUp()
        {
            // Repository.
            _mockApprenticeshipVacancyReadRepository = new Mock<IApprenticeshipVacancyReadRepository>();

            // Configuration.
            _mockConfigurationService = new Mock<IConfigurationService>();

            // Mappers.
            var addressMapper = new AddressMapper();
            var vacancyDurationMapper = new VacancyDurationMapper();
            var apprenticeshipVacancyMapper = new ApprenticeshipVacancyMapper(addressMapper, vacancyDurationMapper);
            var apprenticeshipVacancyQueryMapper = new ApprenticeshipVacancyQueryMapper();

            // Provider.
            _provider = new VacancyDetailsProvider(
                _mockConfigurationService.Object,
                _mockApprenticeshipVacancyReadRepository.Object,
                apprenticeshipVacancyMapper,
                apprenticeshipVacancyQueryMapper);
        }

        [Test]
        public void ShouldBeAwesome()
        {
            // Arrange.
            _mockApprenticeshipVacancyReadRepository.Setup(mock => mock
                .Get(It.IsAny<int>()))
                .Throws(new InvalidOperationException());

            var request = new VacancyDetailsRequest
            {
                VacancySearchCriteria = new VacancySearchData
                {
                    VacancyReferenceId = 1
                }
            };

            // Act.
            Action action = () => _provider.Get(request);

            // Assert.
            Assert.Fail();
        }
    }
}
