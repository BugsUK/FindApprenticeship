namespace SFA.Apprenticeship.Api.AvService.UnitTests.Validators
{
    using AvService.Validators;
    using Common;
    using DataContracts.Version51;
    using FluentAssertions;
    using FluentValidation.TestHelper;
    using NUnit.Framework;

    [TestFixture]
    public class VacancyUploadDataValidatorTests
    {
        private VacancyUploadDataValidator _validator;

        [SetUp]
        public void SetUp()
        {
            _validator = new VacancyUploadDataValidator();
        }

        [TestCase(null, true)]
        [TestCase("", true)]
        [TestCase(" ", true)]
        [TestCase("A", false)]
        public void ShouldRequireTitle(string value, bool expectValidationError)
        {
            // Arrange.
            var vacancy = new VacancyUploadData
            {
                Title = value
            };

            // Assert.
            if (expectValidationError)
            {
                var errorCodes = _validator
                    .Validate(vacancy)
                    .GetErrorCodes();

                errorCodes
                    .Should()
                    .ContainSingle(errorCode => errorCode.InterfaceErrorCode == ApiErrors.VacancyTitleIsMandatory.InterfaceErrorCode);
            }
            else
            {
                _validator.ShouldNotHaveValidationErrorFor(x => x.Title, vacancy);
            }
        }

        [TestCase(1, true)]
        [TestCase(0, false)]
        public void ShouldRequireTitleWithMaximumLength(int extraCharacterCount, bool expectValidationError)
        {
            // Arrange.
            var vacancy = new VacancyUploadData
            {
                Title = new string('X', VacancyUploadDataValidator.MaxVacancyTitleLength + extraCharacterCount)
            };

            // Assert.
            if (expectValidationError)
            {
                var errorCodes = _validator
                    .Validate(vacancy)
                    .GetErrorCodes();

                errorCodes
                    .Should()
                    .ContainSingle(errorCode => errorCode.InterfaceErrorCode == ApiErrors.VacancyTitleIsTooLong.InterfaceErrorCode);
            }
        }

        [TestCase(null, true)]
        [TestCase("", true)]
        [TestCase(" ", true)]
        [TestCase("A", false)]
        public void ShouldRequireShortDescription(string value, bool expectValidationError)
        {
            // Arrange.
            var vacancy = new VacancyUploadData
            {
                ShortDescription = value
            };

            // Assert.
            if (expectValidationError)
            {
                var errorCodes = _validator
                    .Validate(vacancy)
                    .GetErrorCodes();

                errorCodes
                    .Should()
                    .ContainSingle(errorCode => errorCode.InterfaceErrorCode == ApiErrors.VacancyShortDescriptionIsMandatory.InterfaceErrorCode);
            }
            else
            {
                _validator.ShouldNotHaveValidationErrorFor(x => x.ShortDescription, vacancy);
            }
        }

        [TestCase(1, true)]
        [TestCase(0, false)]
        public void ShouldRequireShortDescriptioneWithMaximumLength(int extraCharacterCount, bool expectValidationError)
        {
            // Arrange.
            var vacancy = new VacancyUploadData
            {
                ShortDescription = new string('X', VacancyUploadDataValidator.MaxVacancyShortDescriptionLength + extraCharacterCount)
            };

            // Assert.
            if (expectValidationError)
            {
                var errorCodes = _validator
                    .Validate(vacancy)
                    .GetErrorCodes();

                errorCodes
                    .Should()
                    .ContainSingle(errorCode => errorCode.InterfaceErrorCode == ApiErrors.VacancyShortDescriptionIsTooLong.InterfaceErrorCode);
            }
            else
            {
                _validator.ShouldNotHaveValidationErrorFor(x => x.ShortDescription, vacancy);
            }
        }

        [TestCase(null, true)]
        [TestCase("", true)]
        [TestCase(" ", true)]
        [TestCase("A", false)]
        public void ShouldRequireLongDescription(string value, bool expectValidationError)
        {
            // Arrange.
            var vacancy = new VacancyUploadData
            {
                LongDescription = value
            };

            // Assert.
            if (expectValidationError)
            {
                var errorCodes = _validator
                    .Validate(vacancy)
                    .GetErrorCodes();

                errorCodes
                    .Should()
                    .ContainSingle(errorCode => errorCode.InterfaceErrorCode == ApiErrors.VacancyLongDescriptionIsMandatory.InterfaceErrorCode);
            }
            else
            {
                _validator.ShouldNotHaveValidationErrorFor(x => x.LongDescription, vacancy);
            }
        }

        [TestCase(-1, true)]
        [TestCase(0, true)]
        [TestCase(1, false)]
        [TestCase(int.MaxValue, false)]
        public void ShouldRequireDeliveryProviderEdsUrn(int value, bool expectValidationError)
        {
            // Arrange.
            var vacancy = new VacancyUploadData
            {
                DeliveryProviderEdsUrn = value
            };

            // Assert.
            if (expectValidationError)
            {
                var errorCodes = _validator
                    .Validate(vacancy)
                    .GetErrorCodes();

                errorCodes
                    .Should()
                    .ContainSingle(errorCode => errorCode.InterfaceErrorCode == ApiErrors.DeliveryProviderEdsUrnIsMandatory.InterfaceErrorCode);
            }
            else
            {
                _validator.ShouldNotHaveValidationErrorFor(x => x.DeliveryProviderEdsUrn, vacancy);
            }
        }

        [TestCase(-1, true)]
        [TestCase(0, true)]
        [TestCase(1, false)]
        [TestCase(int.MaxValue, false)]
        public void ShouldRequireVacancyManagerEdsUrn(int value, bool expectValidationError)
        {
            // Arrange.
            var vacancy = new VacancyUploadData
            {
                VacancyManagerEdsUrn = value
            };

            // Assert.
            if (expectValidationError)
            {
                var errorCodes = _validator
                    .Validate(vacancy)
                    .GetErrorCodes();

                errorCodes
                    .Should()
                    .ContainSingle(errorCode => errorCode.InterfaceErrorCode == ApiErrors.VacancyManagerEdsUrnIsMandatory.InterfaceErrorCode);
            }
            else
            {
                _validator.ShouldNotHaveValidationErrorFor(x => x.VacancyManagerEdsUrn, vacancy);
            }
        }

        [TestCase(-1, true)]
        [TestCase(0, true)]
        [TestCase(1, false)]
        [TestCase(int.MaxValue, false)]
        public void ShouldRequireVacancyOwnerEdsUrn(int value, bool expectValidationError)
        {
            // Arrange.
            var vacancy = new VacancyUploadData
            {
                VacancyOwnerEdsUrn = value
            };

            // Assert.
            if (expectValidationError)
            {
                var errorCodes = _validator
                    .Validate(vacancy)
                    .GetErrorCodes();

                errorCodes
                    .Should()
                    .ContainSingle(errorCode => errorCode.InterfaceErrorCode == ApiErrors.VacancyOwnerEdsUrnIsMandatory.InterfaceErrorCode);
            }
            else
            {
                _validator.ShouldNotHaveValidationErrorFor(x => x.VacancyOwnerEdsUrn, vacancy);
            }
        }

        [Test]
        public void ShouldValidateEmployer()
        {
            /*
            // Arrange.
            var vacancy = new VacancyUploadData();

            // Assert.
            var errorCodes = _validator
                .Validate(vacancy)
                .GetErrorCodes();

            errorCodes
                .Should()
                .ContainSingle(errorCode => errorCode.InterfaceErrorCode == ApiErrors.Error10022.InterfaceErrorCode);
            */

            Assert.Inconclusive();
        }

        [Test]
        public void ShouldValidateApplication()
        {
            Assert.Inconclusive();
        }

        [Test]
        public void ShouldValidateApprenticeship()
        {
            Assert.Inconclusive();
        }

        [Test]
        public void ShouldValidateVacancy()
        {
            Assert.Inconclusive();
        }
    }
}
