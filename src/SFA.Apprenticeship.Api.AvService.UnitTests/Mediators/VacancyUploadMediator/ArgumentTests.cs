namespace SFA.Apprenticeship.Api.AvService.UnitTests.Mediators.VacancyUploadMediator
{
    using System;
    using Apprenticeships.Application.Interfaces.Providers;
    using Apprenticeships.Application.Interfaces.VacancyPosting;
    using AvService.Mappers.Version51;
    using AvService.Mediators.Version51;
    using AvService.Validators;
    using FluentAssertions;
    using MessageContracts.Version51;
    using Moq;
    using NUnit.Framework;

    // TODO: US872: AG: rationalise VacancyUploadServiceMediator unit tests into a single file following refactor to mediator.

    [TestFixture]
    public class ArgumentTests
    {
        private Mock<IProviderService> _mockProviderService;
        private Mock<IVacancyPostingService> _mockVacancyPostingService;
        private Mock<VacancyUploadRequestMapper> _mockVacancyUploadRequestMapper;

        private VacancyUploadServiceMediator _vacancyUploadServiceMediator;

        [SetUp]
        public void SetUp()
        {
            // Mappers.
            _mockVacancyUploadRequestMapper = new Mock<VacancyUploadRequestMapper>();

            // Services.
            _mockProviderService = new Mock<IProviderService>();
            _mockVacancyPostingService = new Mock<IVacancyPostingService>();

            // Provider.
            _vacancyUploadServiceMediator = new VacancyUploadServiceMediator(
                new VacancyUploadDataValidator(),
                _mockVacancyUploadRequestMapper.Object,
                _mockProviderService.Object,
                _mockVacancyPostingService.Object);
        }

        [Test]
        public void ShouldThrowIfRequestIsNull()
        {
            // Act.
            Action action = () => _vacancyUploadServiceMediator.UploadVacancies(default(VacancyUploadRequest));

            // Assert.
            action.ShouldThrowExactly<ArgumentNullException>();
        }
    }
}
