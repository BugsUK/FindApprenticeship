namespace SFA.Apprenticeship.Api.AvService.UnitTests.Validators
{
    using DataContracts.Version51;
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

        [Test]
        [Ignore]
        public void ShouldRequireAvmsHeader()
        {
            // Arrange.
            var data = new VacancyUploadData();

            // Act.

            // Assert.
            Assert.Fail();
        }

        [Test]
        [Ignore]
        public void ShouldRequireVacancyUploadRequest()
        {
            // Arrange.

            // Act.

            // Assert.
            Assert.Fail();
        }

        [Test]
        [Ignore]
        public void ShouldRequireOneOrMoreVacancies()
        {
            // Arrange.

            // Act.

            // Assert.
            Assert.Fail();
        }

        [Test]
        [Ignore]
        public void ShouldRequireVacancyId()
        {
            // Arrange.

            // Act.

            // Assert.
            Assert.Fail();
        }

        [Test]
        [Ignore]
        public void ShouldRequireTitle()
        {
            // Arrange.

            // Act.

            // Assert.
            Assert.Fail();
        }

        [Test]
        [Ignore]
        public void ShouldRequireDescriptions()
        {
            // Arrange.

            // TODO: short, long.

            // Act.

            // Assert.
            Assert.Fail();
        }

        [Test]
        [Ignore]
        public void ShouldRequireLocationType()
        {
            // Arrange.

            // Act.

            // Assert.
            Assert.Fail();
        }

        [Test]
        [Ignore]
        public void ShouldRequireAddressLine1()
        {
            // Arrange.

            // Act.

            // Assert.
            Assert.Fail();
        }

        [Test]
        [Ignore]
        public void ShouldRequireCounty()
        {
            // Arrange.

            // Act.

            // Assert.
            Assert.Fail();
        }

        [Test]
        [Ignore]
        public void ShouldRequirePostcode()
        {
            // Arrange.

            // Act.

            // Assert.
            Assert.Fail();
        }

        [Test]
        [Ignore]
        public void ShouldRequireTown()
        {
            // Arrange.

            // Act.

            // Assert.
            Assert.Fail();
        }

        [Test]
        [Ignore]
        public void ShouldRequireNumberOfVacancies()
        {
            // Arrange.

            // Act.

            // Assert.
            Assert.Fail();
        }

        [Test]
        [Ignore]
        public void ShouldRequireSupplementaryQuestions()
        {
            // Arrange.

            // Act.

            // Assert.
            Assert.Fail();
        }

        [Test]
        [Ignore]
        public void ShouldRequireClosingDate()
        {
            // Arrange.

            // Act.

            // Assert.
            Assert.Fail();
        }

        [Test]
        [Ignore]
        public void ShouldRequireInterviewStartDate()
        {
            // Arrange.

            // Act.

            // Assert.
            Assert.Fail();
        }

        [Test]
        [Ignore]
        public void ShouldRequirePossibleStartDate()
        {
            // Arrange.

            // Act.

            // Assert.
            Assert.Fail();
        }

        [Test]
        [Ignore]
        public void ShouldRequireApplicationType()
        {
            // Arrange.

            // Act.

            // Assert.
            Assert.Fail();
        }

        [Test]
        [Ignore]
        public void ShouldRequireEmployerWebsiteWhenApplicationTypeIsOffline()
        {
            // Arrange.

            // Act.

            // Assert.
            Assert.Fail();
        }

        [Test]
        [Ignore]
        public void ShouldRequireValidFramework()
        {
            // Arrange.

            // Act.

            // Assert.
            Assert.Fail();
        }

        [Test]
        [Ignore]
        public void ShouldRequireApprenticeshipVacancyType()
        {
            // Arrange.

            // Act.

            // Assert.
            Assert.Fail();
        }

        [Test]
        [Ignore]
        public void ShouldRequireContractedProviderUkprn()
        {
            // Arrange.

            // TODO: review specification.

            // Act.

            // Assert.
            Assert.Fail();
        }

        [Test]
        [Ignore]
        public void ShouldRequireVacancyOwnerEdsUrn()
        {
            // Arrange.

            // TODO: review specification.

            // Act.

            // Assert.
            Assert.Fail();
        }

        [Test]
        [Ignore]
        public void ShouldRequireVacancyManagerEdsUrn()
        {
            // Arrange.

            // TODO: review specification.

            // Act.

            // Assert.
            Assert.Fail();
        }

        [Test]
        [Ignore]
        public void ShouldRequireDeliveryProviderEdsUrn()
        {
            // Arrange.

            // TODO: review specification.

            // Act.

            // Assert.
            Assert.Fail();
        }
        
        [Test]
        [Ignore]
        public void ShouldRequireIsSmallEmployerWageIncentive()
        {
            // Arrange.

            // TODO: non-nullable bool should not require a test.

            // Act.

            // Assert.
            Assert.Fail();
        }
    }

    public class VacancyUploadDataValidator
    {
    }
}
