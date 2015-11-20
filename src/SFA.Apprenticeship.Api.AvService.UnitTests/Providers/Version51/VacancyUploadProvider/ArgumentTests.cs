namespace SFA.Apprenticeship.Api.AvService.UnitTests.Providers.Version51.VacancyUploadProvider
{
    using System;
    using Apprenticeships.Application.Interfaces.VacancyPosting;
    using AvService.Providers.Version51;
    using FluentAssertions;
    using MessageContracts.Version51;
    using Moq;
    using NUnit.Framework;

    [TestFixture]
    public class ArgumentTests
    {
        private Mock<IVacancyPostingService> _mockVacancyPostingService;

        private VacancyUploadProvider _provider;

        [SetUp]
        public void SetUp()
        {
            _mockVacancyPostingService = new Mock<IVacancyPostingService>();

            _provider = new VacancyUploadProvider(
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
