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
        [TestCase("A", false)]
        {
            // Arrange.
            {
                Title = value
            };

            // Assert.
            Assert.Fail();
        }
            if (expectValidationError)
        [Test]
        [Ignore]
        public void ShouldRequireVacancyUploadRequest()
            {
                var errorCodes = _validator
                    .Validate(vacancy)
            // Act.
                    .GetErrorCodes();
                    .Should()
                    .ContainSingle(errorCode => errorCode.InterfaceErrorCode == ApiErrors.VacancyTitleIsMandatory.InterfaceErrorCode);
            }
            else
        [Test]
        [Ignore]
        public void ShouldRequireOneOrMoreVacancies()
            {
            // Arrange.
                _validator.ShouldNotHaveValidationErrorFor(x => x.Title, vacancy);
            // Act.
            }
            // Assert.
            Assert.Fail();
        }

        {
            // Arrange.
            var vacancy = new VacancyUploadData
            {
                Title = new string('X', VacancyUploadDataValidator.MaxVacancyTitleLength + extraCharacterCount)
            };

            // Assert.
            Assert.Fail();
        }
            if (expectValidationError)
        [Test]
        [Ignore]
        public void ShouldRequireTitle()
            {
                var errorCodes = _validator
                    .Validate(vacancy)
            // Act.
                    .GetErrorCodes();
                    .Should()
                    .ContainSingle(errorCode => errorCode.InterfaceErrorCode == ApiErrors.VacancyTitleIsTooLong.InterfaceErrorCode);
            }
        }
        [Test]
        [TestCase("", true)]
        [TestCase(" ", true)]
        [TestCase("A", false)]
        {
            // Arrange.
            var vacancy = new VacancyUploadData
            {
                ShortDescription = value
            };

            // Assert.
            Assert.Fail();
        }
            if (expectValidationError)
        [Test]
        [Ignore]
        public void ShouldRequireLocationType()
            {
                var errorCodes = _validator
                    .Validate(vacancy)
            // Act.
                    .GetErrorCodes();
                    .Should()
                    .ContainSingle(errorCode => errorCode.InterfaceErrorCode == ApiErrors.VacancyShortDescriptionIsMandatory.InterfaceErrorCode);
            }
            else
        [Test]
        [Ignore]
        public void ShouldRequireAddressLine1()
            {
            // Arrange.
                _validator.ShouldNotHaveValidationErrorFor(x => x.ShortDescription, vacancy);
            // Act.
            }
            // Assert.
            Assert.Fail();
        }

        {
            // Arrange.
            var vacancy = new VacancyUploadData
            {
                ShortDescription = new string('X', VacancyUploadDataValidator.MaxVacancyShortDescriptionLength + extraCharacterCount)
            };

            // Assert.
            Assert.Fail();
        }
            if (expectValidationError)
        [Test]
        [Ignore]
        public void ShouldRequirePostcode()
            {
                var errorCodes = _validator
                    .Validate(vacancy)
            // Act.
                    .GetErrorCodes();
                    .Should()
                    .ContainSingle(errorCode => errorCode.InterfaceErrorCode == ApiErrors.VacancyShortDescriptionIsTooLong.InterfaceErrorCode);
            }
            else
        [Test]
        [Ignore]
        public void ShouldRequireTown()
            {
            // Arrange.
                _validator.ShouldNotHaveValidationErrorFor(x => x.ShortDescription, vacancy);
            // Act.
            }
            // Assert.
            Assert.Fail();
        }

        [TestCase(null, true)]
        [TestCase("A", false)]
        {
            // Arrange.
            var vacancy = new VacancyUploadData
            {
                LongDescription = value
            };

            // Assert.
            Assert.Fail();
        }
            if (expectValidationError)
        [Test]
        [Ignore]
        public void ShouldRequireSupplementaryQuestions()
            {
                var errorCodes = _validator
                    .Validate(vacancy)
            // Act.
                    .GetErrorCodes();
                    .Should()
                    .ContainSingle(errorCode => errorCode.InterfaceErrorCode == ApiErrors.VacancyLongDescriptionIsMandatory.InterfaceErrorCode);
            }
            else
        [Test]
        [Ignore]
        public void ShouldRequireClosingDate()
            {
            // Arrange.
                _validator.ShouldNotHaveValidationErrorFor(x => x.LongDescription, vacancy);
            // Act.
            }
            // Assert.
            Assert.Fail();
        }

        [TestCase(-1, true)]
        [TestCase(int.MaxValue, false)]
        {
            // Arrange.
            var vacancy = new VacancyUploadData
            {
                DeliveryProviderEdsUrn = value
            };

            // Assert.
            Assert.Fail();
        }
            if (expectValidationError)
        [Test]
        [Ignore]
        public void ShouldRequirePossibleStartDate()
            {
                var errorCodes = _validator
                    .Validate(vacancy)
            // Act.
                    .GetErrorCodes();
                    .Should()
                    .ContainSingle(errorCode => errorCode.InterfaceErrorCode == ApiErrors.DeliveryProviderEdsUrnIsMandatory.InterfaceErrorCode);
            }
            else
        [Test]
        [Ignore]
        public void ShouldRequireApplicationType()
            {
            // Arrange.
                _validator.ShouldNotHaveValidationErrorFor(x => x.DeliveryProviderEdsUrn, vacancy);
            // Act.
            }
            // Assert.
            Assert.Fail();
        }

        [TestCase(-1, true)]
        [TestCase(int.MaxValue, false)]
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
            Assert.Fail();
                errorCodes
                    .Should()
                    .ContainSingle(errorCode => errorCode.InterfaceErrorCode == ApiErrors.VacancyManagerEdsUrnIsMandatory.InterfaceErrorCode);
            }
            else
        [Test]
        [Ignore]
        public void ShouldRequireValidFramework()
            {
            // Arrange.
                _validator.ShouldNotHaveValidationErrorFor(x => x.VacancyManagerEdsUrn, vacancy);
            // Act.
            }
            // Assert.
            Assert.Fail();
        }

        [TestCase(-1, true)]
        [TestCase(int.MaxValue, false)]
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
            Assert.Fail();
                errorCodes
                    .Should()
                    .ContainSingle(errorCode => errorCode.InterfaceErrorCode == ApiErrors.VacancyOwnerEdsUrnIsMandatory.InterfaceErrorCode);
            }
            else
        [Test]
        [Ignore]
        public void ShouldRequireContractedProviderUkprn()
            {
            // Arrange.
                _validator.ShouldNotHaveValidationErrorFor(x => x.VacancyOwnerEdsUrn, vacancy);
            // TODO: review specification.
            }
            // Act.

            // Assert.
            Assert.Fail();
        }

        [Test]
        [Ignore]
        {
            /*
            // TODO: review specification.

            // Act.

            var errorCodes = _validator
                .Validate(vacancy)
                .GetErrorCodes();

            errorCodes
                .Should()
                .ContainSingle(errorCode => errorCode.InterfaceErrorCode == ApiErrors.Error10022.InterfaceErrorCode);
            */

        }

        [Test]
        [Ignore]
        {
            // Arrange.

            // TODO: review specification.

            // Act.

            // Assert.
        }

        [Test]
        [Ignore]
        {
            // Arrange.

            // TODO: review specification.

            // Act.

            // Assert.
        }
        [Test]
        [Ignore]
        {
            // Arrange.

            // TODO: non-nullable bool should not require a test.

            // Act.

            // Assert.
        }
    }

    public class VacancyUploadDataValidator
    {
    }
}
