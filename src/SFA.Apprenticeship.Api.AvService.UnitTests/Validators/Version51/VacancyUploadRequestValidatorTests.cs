namespace SFA.Apprenticeship.Api.AvService.UnitTests.Validators.Version51
{
    using System.Collections.Generic;
    using System.Linq;
    using AvService.Validators.Version51;
    using DataContracts.Version51;
    using FluentAssertions;
    using MessageContracts.Version51;
    using NUnit.Framework;

    [TestFixture]
    public class VacancyUploadRequestValidatorTests
    {
        private VacancyUploadRequestValidator _validator;

        [SetUp]
        public void SetUp()
        {
            _validator = new VacancyUploadRequestValidator();
        }

        [TestCase(null)]
        [TestCase(0)]
        public void ShouldBeValidWhenNoVacancies(int? vacancyCount)
        {
            // Arrange.
            var request = new VacancyUploadRequest
            {
                Vacancies = vacancyCount.HasValue
                    ? new List<VacancyUploadData>()
                    : null
            };

            // Act.
            var result = _validator.Validate(request);

            // Assert.
            result.IsValid.Should().Be(true);
        }

        [Test]
        public void ShouldBeInvalidWhenOneOrMoreInvalidVacancies()
        {
            // Arrange.
            var request = new VacancyUploadRequest
            {
                Vacancies = new List<VacancyUploadData>()
                {
                    new VacancyUploadData()
                }
            };

            // Act.
            var result = _validator.Validate(request);

            // Assert.
            result.IsValid.Should().Be(false);
            result.Errors.Should().NotBeNullOrEmpty();

            var error = result.Errors.First().CustomState as ErrorCodesData;

            error.Should().NotBeNull();
        }
    }
}
