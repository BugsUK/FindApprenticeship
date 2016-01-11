namespace SFA.Apprenticeship.Api.AvService.UnitTests.Providers.Version51.VacancyUploadProvider
{
    using System;
    using Apprenticeships.Application.Interfaces.VacancyPosting;
    using AvService.Builders.Version51;
    using AvService.Providers.Version51;
    using AvService.Validators;
    using FluentAssertions;
    using MessageContracts.Version51;
    using Moq;
    using NUnit.Framework;

    [TestFixture]
    public class ArgumentTests
    {
        private Mock<IVacancyPostingService> _mockVacancyPostingService;
        private Mock<ApprenticeshipVacancyBuilder> _mockVacancyUploadRequestMapper;

        private VacancyUploadProvider _provider;

        [SetUp]
        public void SetUp()
        {
            // Mappers.
            _mockVacancyUploadRequestMapper = new Mock<ApprenticeshipVacancyBuilder>();

            // Vacancy Posting Service.
            _mockVacancyPostingService = new Mock<IVacancyPostingService>();

            // Provider.
            _provider = new VacancyUploadProvider(
                new VacancyUploadDataValidator(),
                _mockVacancyUploadRequestMapper.Object,
                _mockVacancyPostingService.Object);
        }

        [Test]
        public void ShouldThrowIfRequestIsNull()
        {
            // Act.
            Action action = () => _provider.UploadVacancies(default(VacancyUploadRequest));

            // Assert.
            action.ShouldThrowExactly<ArgumentNullException>();
        }
    }
}
