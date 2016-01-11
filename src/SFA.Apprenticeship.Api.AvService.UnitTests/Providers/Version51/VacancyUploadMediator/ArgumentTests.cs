namespace SFA.Apprenticeship.Api.AvService.UnitTests.Providers.Version51.VacancyUploadMediator
{
    using System;
    using Apprenticeships.Application.Interfaces.Providers;
    using Apprenticeships.Application.Interfaces.VacancyPosting;
    using AvService.Mappers.Version51;
    using AvService.Validators;
    using FluentAssertions;
    using Mediators.Version51;
    using MessageContracts.Version51;
    using Moq;
    using NUnit.Framework;

    [TestFixture]
    public class ArgumentTests
    {
        private Mock<IProviderService> _mockProviderService;
        private Mock<IVacancyPostingService> _mockVacancyPostingService;
        private Mock<VacancyUploadRequestMapper> _mockVacancyUploadRequestMapper;

        private VacancyUploadMediator _mediator;

        [SetUp]
        public void SetUp()
        {
            // Mappers.
            _mockVacancyUploadRequestMapper = new Mock<VacancyUploadRequestMapper>();

            // Services.
            _mockProviderService = new Mock<IProviderService>();
            _mockVacancyPostingService = new Mock<IVacancyPostingService>();

            // Provider.
            _mediator = new VacancyUploadMediator(
                new VacancyUploadDataValidator(),
                _mockVacancyUploadRequestMapper.Object,
                _mockProviderService.Object,
                _mockVacancyPostingService.Object);
        }

        [Test]
        public void ShouldThrowIfRequestIsNull()
        {
            // Act.
            Action action = () => _mediator.UploadVacancies(default(VacancyUploadRequest));

            // Assert.
            action.ShouldThrowExactly<ArgumentNullException>();
        }
    }
}
