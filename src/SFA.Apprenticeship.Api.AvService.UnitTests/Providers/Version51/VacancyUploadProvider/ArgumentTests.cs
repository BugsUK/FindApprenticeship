namespace SFA.Apprenticeship.Api.AvService.UnitTests.Providers.Version51.VacancyUploadProvider
{
    using System;
    using Apprenticeships.Application.Interfaces.VacancyPosting;
    using AvService.Mappers.Version51;
    using AvService.Providers.Version51;
    using FluentAssertions;
    using MessageContracts.Version51;
    using Moq;
    using NUnit.Framework;

    [TestFixture]
    public class ArgumentTests
    {
        private Mock<IVacancyPostingService> _mockVacancyPostingService;
        private Mock<IVacancyUploadRequestMapper> _mockVacancyUploadRequestMapper;

        private VacancyUploadProvider _provider;

        [SetUp]
        public void SetUp()
        {
            // Mappers.
            _mockVacancyUploadRequestMapper = new Mock<IVacancyUploadRequestMapper>();

            // Vacancy Posting Service.
            _mockVacancyPostingService = new Mock<IVacancyPostingService>();

            // Provider.
            _provider = new VacancyUploadProvider(
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
