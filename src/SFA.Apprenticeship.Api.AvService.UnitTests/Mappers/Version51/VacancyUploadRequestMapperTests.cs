namespace SFA.Apprenticeship.Api.AvService.UnitTests.Mappers.Version51
{
    using System;
    using AvService.Mappers.Version51;
    using Builders.Version51;
    using Common;
    using FluentAssertions;
    using NUnit.Framework;
    using ProviderVacancies = Apprenticeships.Domain.Entities.Vacancies.ProviderVacancies;
    using WageType = Common.WageType;

    [TestFixture]
    public class VacancyUploadRequestMapperTests
    {
        private VacancyUploadDataBuilder _vacancyUploadDataBuilder;
        private VacancyUploadRequestMapper _mapper;

        [SetUp]
        public void SetUp()
        {
            _mapper = new VacancyUploadRequestMapper();
            _vacancyUploadDataBuilder = new VacancyUploadDataBuilder();
        }

        [Test]
        public void ShouldSetEntityId()
        {
            // Arrange.
            var vacancyUploadData = _vacancyUploadDataBuilder
                .Build();

            // Act.
            var mappedVacancy = _mapper.ToVacancy(0, vacancyUploadData, null);

            // Assert.
            mappedVacancy.EntityId.Should().NotBe(Guid.Empty);
        }

        [TestCase(1)]
        [TestCase(42)]
        public void ShouldNotSetVacancyReferenceNumber(long vacancyReferenceNumber)
        {
            // Arrange.
            var vacancyUploadData = _vacancyUploadDataBuilder
                .Build();

            // Act.
            var mappedVacancy = _mapper.ToVacancy(vacancyReferenceNumber, vacancyUploadData, null);

            // Assert.
            mappedVacancy.VacancyReferenceNumber.Should().Be(vacancyReferenceNumber);
        }

        [Test]
        public void ShouldMapContractedProviderUkprn()
        {
            // Arrange.
            const int contractedProviderUkprn = 123;

            var vacancyUploadData = _vacancyUploadDataBuilder
                .WithContractedProviderUkprn(contractedProviderUkprn)
                .Build();

            // Act.
            var mappedVacancy = _mapper.ToVacancy(0, vacancyUploadData, null);

            // Assert.
            mappedVacancy.Ukprn.Should().Be(contractedProviderUkprn.ToString());
        }

        [Test]
        public void ShouldMapTitle()
        {
            // Arrange.
            const string title = "Some title";

            var vacancyUploadData = _vacancyUploadDataBuilder
                .WithTitle(title)
                .Build();

            // Act.
            var mappedVacancy = _mapper.ToVacancy(0, vacancyUploadData, null);

            // Assert.
            mappedVacancy.Title.Should().Be(title);
            mappedVacancy.TitleComment.Should().BeNull();
        }

        [Test]
        public void ShouldMapShortDescription()
        {
            // Arrange.
            const string shortDescription = "Some short description";

            var vacancyUploadData = _vacancyUploadDataBuilder
                .WithShortDescription(shortDescription)
                .Build();

            // Act.
            var mappedVacancy = _mapper.ToVacancy(0, vacancyUploadData, null);

            // Assert.
            mappedVacancy.ShortDescription.Should().Be(shortDescription);
            mappedVacancy.ShortDescriptionComment.Should().BeNull();
        }

        [Test]
        public void ShouldMapLongDescription()
        {
            // Arrange.
            const string longDescription = "Some long description";

            var vacancyUploadData = _vacancyUploadDataBuilder
                .WithLongDescription(longDescription)
                .Build();

            // Act.
            var mappedVacancy = _mapper.ToVacancy(0, vacancyUploadData, null);

            // Assert.
            mappedVacancy.LongDescription.Should().Be(longDescription);
            mappedVacancy.LongDescriptionComment.Should().BeNull();
        }

        [Test]
        public void ShouldMapWorkingWeek()
        {
            // Arrange.
            const string workingWeek = "40 hours";

            var vacancyUploadData = _vacancyUploadDataBuilder
                .WithWorkingWeek(workingWeek)
                .Build();

            // Act.
            var mappedVacancy = _mapper.ToVacancy(0, vacancyUploadData, null);

            // Assert.
            mappedVacancy.WorkingWeek.Should().Be(workingWeek);
        }

        [Test]
        public void ShouldSetWorkingWeekComment()
        {
            // Arrange.
            var vacancyUploadData = _vacancyUploadDataBuilder
                .Build();

            // Act.
            var mappedVacancy = _mapper.ToVacancy(0, vacancyUploadData, null);

            // Assert.
            mappedVacancy.WorkingWeekComment.Should().NotBeNullOrWhiteSpace();
        }

        [Test]
        public void ShouldNotSetHoursPerWeek()
        {
            // Arrange.
            var vacancyUploadData = _vacancyUploadDataBuilder
                .Build();

            // Act.
            var mappedVacancy = _mapper.ToVacancy(0, vacancyUploadData, null);

            // Assert.
            mappedVacancy.HoursPerWeek.Should().Be(default(decimal?));
        }

        [TestCase("0")]
        [TestCase("123.45")]
        [Description("Vacancy upload API only supports weekly wage type.")]
        public void ShouldMapWageTypeWeekly(string weeklyWageString)
        {
            // Arrange.
            decimal weeklyWage;
            decimal.TryParse(weeklyWageString, out weeklyWage);

            var vacancyUploadData = _vacancyUploadDataBuilder
                .WithWage(WageType.Text, weeklyWage)
                .Build();

            // Act.
            var mappedVacancy = _mapper.ToVacancy(0, vacancyUploadData, null);

            // Assert.
            mappedVacancy.WageType.Should().Be(ProviderVacancies.WageType.Custom);
            mappedVacancy.WageUnit.Should().Be(ProviderVacancies.WageUnit.Weekly);
            mappedVacancy.Wage.Should().Be(weeklyWage);
            mappedVacancy.WageComment.Should().BeNull();
        }

        [Test]
        public void ShouldMapExpectedDuration()
        {
            // Arrange.
            const string expectedDuration = "Some expected duration";

            var vacancyUploadData = _vacancyUploadDataBuilder
                .WithExpectedDuration(expectedDuration)
                .Build();

            // Act.
            var mappedVacancy = _mapper.ToVacancy(0, vacancyUploadData, null);

            // Assert.
            mappedVacancy.DurationType.Should().Be(ProviderVacancies.DurationType.Weeks);
            mappedVacancy.Duration.Should().Be(default(int?));
            mappedVacancy.DurationComment.Should().Be(expectedDuration);
        }

        [Test]
        public void ShouldMapClosingDate()
        {
            // Arrange.
            var closingDate = DateTime.Today.AddDays(90);

            var vacancyUploadData = _vacancyUploadDataBuilder
                .WithClosingDate(closingDate)
                .Build();

            // Act.
            var mappedVacancy = _mapper.ToVacancy(0, vacancyUploadData, null);

            // Assert.
            mappedVacancy.ClosingDate.Should().Be(closingDate);
            mappedVacancy.ClosingDateComment.Should().BeNull();
        }

        [Test]
        public void ShouldMapInterviewStartDate()
        {
            // Arrange.
            var interviewStartDate = DateTime.Today.AddDays(120);

            var vacancyUploadData = _vacancyUploadDataBuilder
                .WithInterviewStartDate(interviewStartDate)
                .Build();

            // Act.
            var mappedVacancy = _mapper.ToVacancy(0, vacancyUploadData, null);

            // Assert.
            mappedVacancy.InterviewStartDate.Should().Be(interviewStartDate);
        }

        [Test]
        public void ShouldMapPossibleStartDate()
        {
            // Arrange.
            var possibleStartDate = DateTime.Today.AddDays(120);

            var vacancyUploadData = _vacancyUploadDataBuilder
                .WithPossibleStartDate(possibleStartDate)
                .Build();

            // Act.
            var mappedVacancy = _mapper.ToVacancy(0, vacancyUploadData, null);

            // Assert.
            mappedVacancy.PossibleStartDate.Should().Be(possibleStartDate);
            mappedVacancy.PossibleStartDateComment.Should().BeNull();
        }

        [Test]
        public void ShouldMapSkillsRequired()
        {
            // Arrange.
            const string skillsRequired = "Some skills required";

            var vacancyUploadData = _vacancyUploadDataBuilder
                .WithSkillsRequired(skillsRequired)
                .Build();

            // Act.
            var mappedVacancy = _mapper.ToVacancy(0, vacancyUploadData, null);

            // Assert.
            mappedVacancy.DesiredSkills.Should().Be(skillsRequired);
            mappedVacancy.DesiredSkillsComment.Should().BeNull();
        }

        [Test]
        public void ShouldMapFutureProspects()
        {
            // Arrange.
            const string futureProspects = "Some future prospects";

            var vacancyUploadData = _vacancyUploadDataBuilder
                .WithFutureProspects(futureProspects)
                .Build();

            // Act.
            var mappedVacancy = _mapper.ToVacancy(0, vacancyUploadData, null);

            // Assert.
            mappedVacancy.FutureProspects.Should().Be(futureProspects);
            mappedVacancy.FutureProspectsComment.Should().BeNull();
        }

        [Test]
        public void ShouldMapPersonalQualities()
        {
            // Arrange.
            const string personalQualities = "Some personal qualities";

            var vacancyUploadData = _vacancyUploadDataBuilder
                .WithPersonalQualities(personalQualities)
                .Build();

            // Act.
            var mappedVacancy = _mapper.ToVacancy(0, vacancyUploadData, null);

            // Assert.
            mappedVacancy.PersonalQualities.Should().Be(personalQualities);
            mappedVacancy.PersonalQualitiesComment.Should().BeNull();
        }        

        [Test]
        public void ShouldMapQualificationRequired()
        {
            // Arrange.
            const string qualificationRequired = "Some qualification required";

            var vacancyUploadData = _vacancyUploadDataBuilder
                .WithQualificationRequired(qualificationRequired)
                .Build();

            // Act.
            var mappedVacancy = _mapper.ToVacancy(0, vacancyUploadData, null);

            // Assert.
            mappedVacancy.DesiredQualifications.Should().Be(qualificationRequired);
            mappedVacancy.DesiredQualificationsComment.Should().BeNull();
        }

        [Test]
        public void ShouldMapRealityCheck()
        {
            // Arrange.
            const string realityCheck = "Some reality check";

            var vacancyUploadData = _vacancyUploadDataBuilder
                .WithRealityCheck(realityCheck)
                .Build();

            // Act.
            var mappedVacancy = _mapper.ToVacancy(0, vacancyUploadData, null);

            // Assert.
            mappedVacancy.ThingsToConsider.Should().Be(realityCheck);
            mappedVacancy.ThingsToConsiderComment.Should().BeNull();
        }

        [Test]
        public void ShouldMapSupplementaryQuestions()
        {
            // Arrange.
            const string supplementaryQuestion1 = "Q1?";
            const string supplementaryQuestion2 = "Q2?";

            var vacancyUploadData = _vacancyUploadDataBuilder
                .WithSupplementaryQuestions(supplementaryQuestion1, supplementaryQuestion2)
                .Build();

            // Act.
            var mappedVacancy = _mapper.ToVacancy(0, vacancyUploadData, null);

            // Assert.
            mappedVacancy.FirstQuestion.Should().Be(supplementaryQuestion1);
            mappedVacancy.FirstQuestionComment.Should().BeNull();

            mappedVacancy.SecondQuestion.Should().Be(supplementaryQuestion2);
            mappedVacancy.SecondQuestionComment.Should().BeNull();
        }

        [TestCase(ApplicationType.Online, false)]
        [TestCase(ApplicationType.Offline, true)]
        public void ShouldMapApplicationTypeOnlineAndOffline(ApplicationType applicationType, bool expectedOfflineVacancy)
        {
            // Arrange.
            var vacancyUploadData = _vacancyUploadDataBuilder
                .WithApplicationType(applicationType)
                .Build();

            // Act.
            var mappedVacancy = _mapper.ToVacancy(0, vacancyUploadData, null);

            // Assert.
            mappedVacancy.OfflineVacancy.Should().Be(expectedOfflineVacancy);
        }

        [Test]
        public void ShouldMapEmployerWebsite()
        {
            // Arrange.
            const string employerWebsite = "https://example.com";

            var vacancyUploadData = _vacancyUploadDataBuilder
                .WithEmployerWebsite(employerWebsite)
                .Build();

            // Act.
            var mappedVacancy = _mapper.ToVacancy(0, vacancyUploadData, null);

            // Assert.
            mappedVacancy.OfflineApplicationUrl.Should().Be(employerWebsite);
            mappedVacancy.OfflineApplicationUrlComment.Should().BeNull();
        }

        [Test]
        public void ShouldMapEmployersApplicationInstructions()
        {
            // Arrange.
            const string employersApplicationInstructions = "Some instructions";

            var vacancyUploadData = _vacancyUploadDataBuilder
                .WithEmployersApplicationInstructions(employersApplicationInstructions)
                .Build();

            // Act.
            var mappedVacancy = _mapper.ToVacancy(0, vacancyUploadData, null);

            // Assert.
            mappedVacancy.OfflineApplicationInstructions.Should().Be(employersApplicationInstructions);
            mappedVacancy.OfflineApplicationInstructionsComment.Should().BeNull();
        }

        [Test]
        public void ShouldMapApprenticeshipFramework()
        {
            // Arrange.
            const string frameworkCode = "123";

            var vacancyUploadData = _vacancyUploadDataBuilder
                .WithApprenticeshipFrameworkCode(frameworkCode)
                .Build();

            // Act.
            var mappedVacancy = _mapper.ToVacancy(0, vacancyUploadData, null);

            // Assert.
            mappedVacancy.FrameworkCodeName.Should().Be(frameworkCode);
            mappedVacancy.FrameworkCodeNameComment.Should().BeNull();
        }

        [Test]
        public void ShouldNotSetStandardId()
        {
            // Arrange.
            var vacancyUploadData = _vacancyUploadDataBuilder
                .Build();

            // Act.
            var mappedVacancy = _mapper.ToVacancy(0, vacancyUploadData, null);

            // Assert.
            mappedVacancy.StandardId.Should().Be(default(int?));
            mappedVacancy.StandardIdComment.Should().BeNull();
        }

        [TestCase(VacancyApprenticeshipType.IntermediateLevelApprenticeship, ProviderVacancies.Apprenticeship.ApprenticeshipLevel.Intermediate)]
        [TestCase(VacancyApprenticeshipType.AdvancedLevelApprenticeship, ProviderVacancies.Apprenticeship.ApprenticeshipLevel.Advanced)]
        [TestCase(VacancyApprenticeshipType.HigherApprenticeship, ProviderVacancies.Apprenticeship.ApprenticeshipLevel.Higher)]
        public void ShouldMapVacancyApprenticeshipType(
            VacancyApprenticeshipType vacancyApprenticeshipType,
            ProviderVacancies.Apprenticeship.ApprenticeshipLevel expectedApprenticeshipLevel)
        {
            // Arrange.
            var vacancyUploadData = _vacancyUploadDataBuilder
                .WithVacancyApprenticeshipType(vacancyApprenticeshipType)
                .Build();

            // Act.
            var mappedVacancy = _mapper.ToVacancy(0, vacancyUploadData, null);

            // Assert.
            mappedVacancy.ApprenticeshipLevel.Should().Be(expectedApprenticeshipLevel);
            mappedVacancy.ApprenticeshipLevelComment.Should().BeNull();
        }

        [TestCase(VacancyApprenticeshipType.Traineeship)]
        [TestCase(VacancyApprenticeshipType.Unspecified)]
        public void ShouldThrowIfUnknownVacancyApprenticeshipType(VacancyApprenticeshipType vacancyApprenticeshipType)
        {
            // Arrange.
            var vacancyUploadData = _vacancyUploadDataBuilder
                .WithVacancyApprenticeshipType(vacancyApprenticeshipType)
                .Build();

            // Act.
            Action action = () => _mapper.ToVacancy(0, vacancyUploadData, null);

            // Assert.
            action.ShouldThrow<ArgumentOutOfRangeException>();
        }

        [TestCase(VacancyApprenticeshipType.Traineeship)]
        [Ignore]
        public void ShouldMapVacancyTraineeshipType(VacancyApprenticeshipType vacancyApprenticeshipType)
        {
            // TODO: US872: AG: map traineeship vacancy.
        }

        [Test]
        public void ShouldSetVacancyStatusToDraft()
        {
            // Arrange.
            var vacancyUploadData = _vacancyUploadDataBuilder
                .Build();

            // Act.
            var mappedVacancy = _mapper.ToVacancy(0, vacancyUploadData, null);

            // Assert.
            mappedVacancy.Status.Should().Be(ProviderVacancies.ProviderVacancyStatuses.Draft);
        }

        [Test]
        public void ShouldSetDateCreated()
        {
            // Arrange.
            var vacancyUploadData = _vacancyUploadDataBuilder
                .Build();

            // Act.
            var mappedVacancy = _mapper.ToVacancy(0, vacancyUploadData, null);

            // Assert.
            mappedVacancy.DateCreated.Should().BeCloseTo(DateTime.UtcNow, 1000);
        }

        [Test]
        public void ShouldSetTrainingType()
        {
            // Arrange.
            var vacancyUploadData = _vacancyUploadDataBuilder
                .Build();

            // Act.
            var mappedVacancy = _mapper.ToVacancy(0, vacancyUploadData, null);

            // Assert.
            mappedVacancy.TrainingType.Should().Be(ProviderVacancies.TrainingType.Frameworks);
        }
    }
}
