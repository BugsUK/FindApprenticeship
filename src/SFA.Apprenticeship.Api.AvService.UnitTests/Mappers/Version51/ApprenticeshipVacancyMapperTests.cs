namespace SFA.Apprenticeship.Api.AvService.UnitTests.Mappers.Version51
{
    using System;
    using Apprenticeships.Domain.Entities.Locations;
    using Apprenticeships.Domain.Entities.Organisations;
    using Apprenticeships.Domain.Entities.Providers;
    using Apprenticeships.Domain.Entities.Vacancies.ProviderVacancies;
    using Apprenticeships.Domain.Entities.Vacancies.ProviderVacancies.Apprenticeship;
    using AvService.Mappers.Version51;
    using FluentAssertions;
    using NUnit.Framework;

    [TestFixture]
    public class ApprenticeshipVacancyMapperTests
    {
        private const string Todo = "TODO";

        private IApprenticeshipVacancyMapper _apprenticeshipVacancyMapper;

        [SetUp]
        public void SetUp()
        {
            var addressMapper = new AddressMapper();
            var vacancyDurationMapper = new VacancyDurationMapper();

            _apprenticeshipVacancyMapper = new ApprenticeshipVacancyMapper(addressMapper, vacancyDurationMapper);
        }

        [Test]
        public void ShouldMapVacancyLocationType()
        {
            // Arrange.
            var vacancy = new ApprenticeshipVacancy();

            // Act.
            var mappedVacancy = _apprenticeshipVacancyMapper.MapToVacancyFullData(vacancy);

            // Assert.
            mappedVacancy.Should().NotBeNull();
            mappedVacancy.VacancyLocationType.Should().Be(Todo);
        }

        [Test]
        public void ShouldMapApprenticeshipFramework()
        {
            // Arrange.
            var vacancy = new ApprenticeshipVacancy
            {
                FrameworkCodeName = "A001"
            };

            // Act.
            var mappedVacancy = _apprenticeshipVacancyMapper.MapToVacancyFullData(vacancy);

            // Assert.
            mappedVacancy.Should().NotBeNull();
            mappedVacancy.ApprenticeshipFramework.Should().StartWith(Todo);
            mappedVacancy.ApprenticeshipFramework.Should().EndWith(vacancy.FrameworkCodeName);
        }

        [TestCase(42)]
        [TestCase(null)]
        public void ShouldMapClosingDate(int? addDays)
        {
            // Arrange.
            var vacancy = new ApprenticeshipVacancy
            {
                ClosingDate = addDays.HasValue ? DateTime.Today.AddDays(42) : default(DateTime)
            };

            // Act.
            var mappedVacancy = _apprenticeshipVacancyMapper.MapToVacancyFullData(vacancy);

            // Assert.
            mappedVacancy.Should().NotBeNull();

            if (addDays.HasValue)
            {
                // ReSharper disable once PossibleInvalidOperationException
                mappedVacancy.ClosingDate.Should().Be(vacancy.ClosingDate.Value);
            }
            else
            {
                mappedVacancy.ClosingDate.Should().Be(DateTime.MinValue);
            }
        }

        [Test]
        public void ShouldMapShortDescription()
        {
            // Arrange.
            var vacancy = new ApprenticeshipVacancy
            {
                ShortDescription = "Some short description"
            };

            // Act.
            var mappedVacancy = _apprenticeshipVacancyMapper.MapToVacancyFullData(vacancy);

            // Assert.
            mappedVacancy.Should().NotBeNull();
            mappedVacancy.ShortDescription.Should().Be(vacancy.ShortDescription);
        }

        [Test]
        public void ShouldMapEmployerName()
        {
            // Arrange.
            const string employerName = "Some Corp";

            var vacancy = new ApprenticeshipVacancy
            {
                ProviderSiteEmployerLink = new ProviderSiteEmployerLink
                {
                    Employer = new Employer
                    {
                        Name = employerName
                    }
                }
            };

            // Act.
            var mappedVacancy = _apprenticeshipVacancyMapper.MapToVacancyFullData(vacancy);

            // Assert.
            mappedVacancy.Should().NotBeNull();
            mappedVacancy.EmployerName.Should().Be(employerName);
        }

        [Test]
        public void ShouldMapEmployerAddress()
        {
            // Arrange.
            var address = new Address
            {
                AddressLine1 = "14 Acacia Avenue",
                Postcode = "AC14 4AA"
            };

            var vacancy = new ApprenticeshipVacancy
            {
                ProviderSiteEmployerLink = new ProviderSiteEmployerLink
                {
                    Employer = new Employer
                    {
                        Address = address
                    }
                }
            };

            // Act.
            var mappedVacancy = _apprenticeshipVacancyMapper.MapToVacancyFullData(vacancy);

            // Assert.
            mappedVacancy.Should().NotBeNull();
            mappedVacancy.VacancyAddress.Should().NotBeNull();

            // NOTE: basic address checks here only, address mapper unit tested elsewhere.
            mappedVacancy.VacancyAddress.AddressLine1.Should().Be(address.AddressLine1);
            mappedVacancy.VacancyAddress.PostCode.Should().Be(address.Postcode);
        }

        [Test]
        public void ShouldMapNumberOfPositions()
        {
            // Arrange.
            var vacancy = new ApprenticeshipVacancy();

            // Act.
            var mappedVacancy = _apprenticeshipVacancyMapper.MapToVacancyFullData(vacancy);

            // Assert.
            mappedVacancy.Should().NotBeNull();
            mappedVacancy.NumberOfPositions.Should().Be(1);
        }

        [Test]
        public void ShouldMapVacancyTitle()
        {
            // Arrange.
            var vacancy = new ApprenticeshipVacancy
            {
                Title = "Some title"
            };

            // Act.
            var mappedVacancy = _apprenticeshipVacancyMapper.MapToVacancyFullData(vacancy);

            // Assert.
            mappedVacancy.Should().NotBeNull();
            mappedVacancy.VacancyTitle.Should().Be(vacancy.Title);
        }

        [Test]
        public void ShouldMapCreatedDateTime()
        {
            // Arrange.
            var vacancy = new ApprenticeshipVacancy
            {
                DateCreated = DateTime.Today.AddDays(-1)
            };

            // Act.
            var mappedVacancy = _apprenticeshipVacancyMapper.MapToVacancyFullData(vacancy);

            // Assert.
            mappedVacancy.Should().NotBeNull();
            mappedVacancy.CreatedDateTime.Should().Be(vacancy.DateCreated);
        }

        [Test]
        public void ShouldMapVacancyReference()
        {
            // Arrange.
            var vacancy = new ApprenticeshipVacancy
            {
                VacancyReferenceNumber = 5
            };

            // Act.
            var mappedVacancy = _apprenticeshipVacancyMapper.MapToVacancyFullData(vacancy);

            // Assert.
            mappedVacancy.Should().NotBeNull();
            mappedVacancy.VacancyReference.Should().Be(Convert.ToInt32(vacancy.VacancyReferenceNumber));
        }

        [Test]
        public void ShouldMapVacancyType()
        {
            // Arrange.
            var vacancy = new ApprenticeshipVacancy();

            // Act.
            var mappedVacancy = _apprenticeshipVacancyMapper.MapToVacancyFullData(vacancy);

            // Assert.
            mappedVacancy.Should().NotBeNull();
            mappedVacancy.VacancyType.Should().Be(Todo);
        }

        [Test]
        public void ShouldMapVacancyUrl()
        {
            // Arrange.
            var vacancy = new ApprenticeshipVacancy();

            // Act.
            var mappedVacancy = _apprenticeshipVacancyMapper.MapToVacancyFullData(vacancy);

            // Assert.
            mappedVacancy.Should().NotBeNull();
            mappedVacancy.VacancyUrl.Should().Be(Todo);
        }

        [Test]
        public void ShouldMapFullDescription()
        {
            // Arrange.
            var vacancy = new ApprenticeshipVacancy
            {
                LongDescription = "Some long description"
            };

            // Act.
            var mappedVacancy = _apprenticeshipVacancyMapper.MapToVacancyFullData(vacancy);

            // Assert.
            mappedVacancy.Should().NotBeNull();
            mappedVacancy.FullDescription.Should().Be(vacancy.LongDescription);
        }

        [Test]
        public void ShouldMapSupplementaryQuestions()
        {
            // Arrange.
            var vacancy = new ApprenticeshipVacancy
            {
                FirstQuestion = "Some first question",
                SecondQuestion = "Some second question"
            };

            // Act.
            var mappedVacancy = _apprenticeshipVacancyMapper.MapToVacancyFullData(vacancy);

            // Assert.
            mappedVacancy.Should().NotBeNull();
            mappedVacancy.SupplementaryQuestion1.Should().Be(vacancy.FirstQuestion);
            mappedVacancy.SupplementaryQuestion2.Should().Be(vacancy.SecondQuestion);
        }

        [Test]
        public void ShouldMapContactPerson()
        {
            // Arrange.
            var vacancy = new ApprenticeshipVacancy();

            // Act.
            var mappedVacancy = _apprenticeshipVacancyMapper.MapToVacancyFullData(vacancy);

            // Assert.
            mappedVacancy.Should().NotBeNull();
            mappedVacancy.ContactPerson.Should().Be(Todo);
        }

        [Test]
        public void ShouldMapEmployerDescription()
        {
            // Arrange.
            var vacancy = new ApprenticeshipVacancy
            {
                ProviderSiteEmployerLink = new ProviderSiteEmployerLink
                {
                    Description = "Some employer description"
                }
            };

            // Act.
            var mappedVacancy = _apprenticeshipVacancyMapper.MapToVacancyFullData(vacancy);

            // Assert.
            mappedVacancy.Should().NotBeNull();
            mappedVacancy.EmployerDescription.Should().Be(vacancy.ProviderSiteEmployerLink.Description);
        }

        [TestCase(12, DurationType.Months, "12 months")]
        public void ShouldMapExpectedDuration(int duration, DurationType durationType, string expectedExpectedDuration)
        {
            // Arrange.
            var vacancy = new ApprenticeshipVacancy
            {
                Duration = duration,
                DurationType = durationType
            };

            // Act.
            var mappedVacancy = _apprenticeshipVacancyMapper.MapToVacancyFullData(vacancy);

            // Assert.
            mappedVacancy.Should().NotBeNull();
            mappedVacancy.ExpectedDuration.Should().Be(expectedExpectedDuration);
        }

        [Test]
        public void ShouldMapFutureProspects()
        {
            // Arrange.
            var vacancy = new ApprenticeshipVacancy
            {
                FutureProspects = "Some prospects"
            };

            // Act.
            var mappedVacancy = _apprenticeshipVacancyMapper.MapToVacancyFullData(vacancy);

            // Assert.
            mappedVacancy.Should().NotBeNull();
            mappedVacancy.FutureProspects.Should().Be(vacancy.FutureProspects);
        }

        [Test]
        public void ShouldMapInterviewFromDate()
        {
            // Arrange.
            var vacancy = new ApprenticeshipVacancy();

            // Act.
            var mappedVacancy = _apprenticeshipVacancyMapper.MapToVacancyFullData(vacancy);

            // Assert.
            mappedVacancy.Should().NotBeNull();
            mappedVacancy.InterviewFromDate.Should().Be(DateTime.MinValue);
        }

        [Test]
        public void ShouldAlwaysMapLearningProviderName()
        {
            // Arrange.
            var vacancy = new ApprenticeshipVacancy
            {
                Ukprn = "5"
            };

            // Act.
            var mappedVacancy = _apprenticeshipVacancyMapper.MapToVacancyFullData(vacancy);

            // Assert.
            mappedVacancy.Should().NotBeNull();
            mappedVacancy.LearningProviderName.Should().StartWith(Todo);
            mappedVacancy.LearningProviderName.Should().EndWith(vacancy.Ukprn);
        }

        [Test]
        public void ShouldAlwaysMapLearningProviderDesc()
        {
            // Arrange.
            var vacancy = new ApprenticeshipVacancy
            {
                Ukprn = "5"
            };

            // Act.
            var mappedVacancy = _apprenticeshipVacancyMapper.MapToVacancyFullData(vacancy);

            // Assert.
            mappedVacancy.Should().NotBeNull();
            mappedVacancy.LearningProviderDesc.Should().StartWith(Todo);
            mappedVacancy.LearningProviderDesc.Should().EndWith(vacancy.Ukprn);
        }

        [Test]
        public void ShouldAlwaysMapLearningProviderSectorPassRateToNull()
        {
            // Arrange.
            var vacancy = new ApprenticeshipVacancy();

            // Act.
            var mappedVacancy = _apprenticeshipVacancyMapper.MapToVacancyFullData(vacancy);

            // Assert.
            mappedVacancy.Should().NotBeNull();
            mappedVacancy.LearningProviderSectorPassRate.Should().Be(null);
        }

        [Test]
        public void ShouldMapPersonalQualities()
        {
            // Arrange.
            var vacancy = new ApprenticeshipVacancy
            {
                PersonalQualities = "Some personal qualities"
            };

            // Act.
            var mappedVacancy = _apprenticeshipVacancyMapper.MapToVacancyFullData(vacancy);

            // Assert.
            mappedVacancy.Should().NotBeNull();
            mappedVacancy.PersonalQualities.Should().Be(vacancy.PersonalQualities);
        }

        [Test]
        public void ShouldMapPossibleStartDate()
        {
            // Arrange.
            var vacancy = new ApprenticeshipVacancy
            {
                PossibleStartDate = DateTime.Today.AddMonths(3)
            };

            // Act.
            var mappedVacancy = _apprenticeshipVacancyMapper.MapToVacancyFullData(vacancy);

            // Assert.
            mappedVacancy.Should().NotBeNull();
            // ReSharper disable once PossibleInvalidOperationException
            mappedVacancy.PossibleStartDate.Should().Be(vacancy.PossibleStartDate.Value);
        }

        [Test]
        public void ShouldMapQualificationRequired()
        {
            // Arrange.
            var vacancy = new ApprenticeshipVacancy
            {
                DesiredQualifications = "Some desired qualifications"
            };

            // Act.
            var mappedVacancy = _apprenticeshipVacancyMapper.MapToVacancyFullData(vacancy);

            // Assert.
            mappedVacancy.Should().NotBeNull();
            mappedVacancy.QualificationRequired.Should().Be(vacancy.DesiredQualifications);
        }

        [Test]
        public void ShouldMapTrainingToBeProvided()
        {
            // Arrange.
            var vacancy = new ApprenticeshipVacancy();

            // Act.
            var mappedVacancy = _apprenticeshipVacancyMapper.MapToVacancyFullData(vacancy);

            // Assert.
            mappedVacancy.Should().NotBeNull();
            mappedVacancy.TrainingToBeProvided.Should().Be(null);
        }

        [Test]
        public void ShouldMapSkillsRequired()
        {
            // Arrange.
            var vacancy = new ApprenticeshipVacancy
            {
                DesiredQualifications = "Some skills required"
            };

            // Act.
            var mappedVacancy = _apprenticeshipVacancyMapper.MapToVacancyFullData(vacancy);

            // Assert.
            mappedVacancy.Should().NotBeNull();
            mappedVacancy.SkillsRequired.Should().Be(vacancy.DesiredSkills);
        }

        [Test]
        public void ShouldMapOtherImportantInformation()
        {
            // Arrange.
            var vacancy = new ApprenticeshipVacancy();

            // Act.
            var mappedVacancy = _apprenticeshipVacancyMapper.MapToVacancyFullData(vacancy);

            // Assert.
            mappedVacancy.Should().NotBeNull();
            mappedVacancy.OtherImportantInformation.Should().Be(Todo);
        }

        [Test]
        public void ShouldMapEmployerWebsite()
        {
            // Arrange.
            var vacancy = new ApprenticeshipVacancy
            {
                ProviderSiteEmployerLink = new ProviderSiteEmployerLink
                {
                    WebsiteUrl = "http://example.com"
                }
            };

            // Act.
            var mappedVacancy = _apprenticeshipVacancyMapper.MapToVacancyFullData(vacancy);

            // Assert.
            mappedVacancy.Should().NotBeNull();
            mappedVacancy.EmployerWebsite.Should().Be(vacancy.ProviderSiteEmployerLink.WebsiteUrl);
        }

        [Test]
        public void ShouldMapVacancyOwner()
        {
            // Arrange.
            var vacancy = new ApprenticeshipVacancy();

            // Act.
            var mappedVacancy = _apprenticeshipVacancyMapper.MapToVacancyFullData(vacancy);

            // Assert.
            mappedVacancy.Should().NotBeNull();
            mappedVacancy.VacancyOwner.Should().Be(Todo);
        }

        [Test]
        public void ShouldMapContractOwner()
        {
            // Arrange.
            var vacancy = new ApprenticeshipVacancy();

            // Act.
            var mappedVacancy = _apprenticeshipVacancyMapper.MapToVacancyFullData(vacancy);

            // Assert.
            mappedVacancy.Should().NotBeNull();
            mappedVacancy.ContractOwner.Should().Be(Todo);
        }

        [Test]
        public void ShouldMapDeliverOrganisation()
        {
            // Arrange.
            var vacancy = new ApprenticeshipVacancy();

            // Act.
            var mappedVacancy = _apprenticeshipVacancyMapper.MapToVacancyFullData(vacancy);

            // Assert.
            mappedVacancy.Should().NotBeNull();
            mappedVacancy.DeliveryOrganisation.Should().Be(Todo);
        }

        [Test]
        public void ShouldMapVacancyManager()
        {
            // Arrange.
            var vacancy = new ApprenticeshipVacancy();

            // Act.
            var mappedVacancy = _apprenticeshipVacancyMapper.MapToVacancyFullData(vacancy);

            // Assert.
            mappedVacancy.Should().NotBeNull();
            mappedVacancy.VacancyManager.Should().Be(Todo);
        }

        [Test]
        public void ShouldAlwaysMapIsDisplayRecruitmentAgencyToNull()
        {
            // Arrange.
            var vacancy = new ApprenticeshipVacancy();

            // Act.
            var mappedVacancy = _apprenticeshipVacancyMapper.MapToVacancyFullData(vacancy);

            // Assert.
            mappedVacancy.Should().NotBeNull();
            mappedVacancy.IsDisplayRecruitmentAgency.Should().Be(null);
        }

        [Test]
        public void ShouldAlwaysMapIsSmallEmployerWageIncentiveToFalse()
        {
            // Arrange.
            var vacancy = new ApprenticeshipVacancy();

            // Act.
            var mappedVacancy = _apprenticeshipVacancyMapper.MapToVacancyFullData(vacancy);

            // Assert.
            mappedVacancy.Should().NotBeNull();
            mappedVacancy.IsSmallEmployerWageIncentive.Should().BeFalse();
        }
    }
}
